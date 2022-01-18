using System;
using System.Collections.Generic;
using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Store.Objects;
using System.Linq;
using System.Collections;
using WowPacketParser.Enums.Version;

namespace WowPacketParser.Store
{
    public static class Storage
    {
        // Stores opcodes read, npc/GOs/spell/item/etc IDs found in sniffs
        // and other miscellaneous stuff
        public static readonly DataBag<SniffData> SniffData = new DataBag<SniffData>(Settings.SqlTables.SniffData || Settings.SqlTables.SniffDataOpcodes);

        /* Key: Guid */
        public static uint CurrentTaxiNode = 0;
        public static WowGuid CurrentActivePlayer = WowGuid64.Empty;
        public static void SetCurrentActivePlayer(WowGuid guid, DateTime time)
        {
            Storage.CurrentActivePlayer = guid;
            ActivePlayerCreateTime activePlayer = new ActivePlayerCreateTime
            {
                Guid = guid,
                Time = time,
            };
            Storage.PlayerActiveCreateTime.Add(activePlayer);

            // initial spells packet is sent before create object for own player
            if (CharacterSpells.ContainsKey(WowGuid64.Empty))
            {
                if (CharacterSpells.ContainsKey(guid))
                {
                    CharacterSpells[guid] = CharacterSpells[WowGuid64.Empty];
                }
                else
                {
                    Storage.CharacterSpells.Add(guid, CharacterSpells[WowGuid64.Empty]);
                }
                CharacterSpells.Remove(WowGuid64.Empty);
            }
            // initial factions packet is sent before create object for own player
            if (CharacterReputations.ContainsKey(WowGuid64.Empty))
            {
                if (CharacterReputations.ContainsKey(guid))
                {
                    CharacterReputations[guid] = CharacterReputations[WowGuid64.Empty];
                }
                else
                {
                    Storage.CharacterReputations.Add(guid, CharacterReputations[WowGuid64.Empty]);
                }
                CharacterReputations.Remove(WowGuid64.Empty);
            }
        }

        // Units, GameObjects, Players, Items
        public static readonly StoreDictionary<WowGuid, WoWObject> Objects = new StoreDictionary<WowGuid, WoWObject>(new List<SQLOutput>());
        public static void StoreNewObject(WowGuid guid, WoWObject obj, ObjectCreateType type, Packet packet)
        {
            obj.OriginalMovement = obj.Movement != null ? obj.Movement.CopyFromMe() : null;
            obj.OriginalUpdateFields = obj.UpdateFields != null ? new Dictionary<int, UpdateField>(obj.UpdateFields) : null;

            if (!string.IsNullOrWhiteSpace(Settings.SQLFileName) && Settings.DumpFormatWithSQL())
                obj.SourceSniffId = packet.SniffId;
            obj.SourceSniffBuild = ClientVersion.BuildInt;

            obj.FirstCreateTime = packet.Time;
            obj.FirstCreateType = type;
            obj.LastCreateTime = packet.Time;
            obj.LastCreateType = type;

            Unit creature = obj as Unit;
            if (creature != null)
            {
                if (type == ObjectCreateType.Create2 &&
                    Settings.SqlTables.creature_respawn_time &&
                    guid.GetHighType() == HighGuidType.Creature &&
                    obj.OriginalMovement != null)
                {
                    Tuple<WowGuid, DateTime> lastDeath;
                    if (CreatureDeathTimes.TryGetValue(obj.OriginalMovement.Position, out lastDeath))
                    {
                        CreatureRespawnTime respawnTime = new CreatureRespawnTime
                        {
                            OldGUID = Storage.GetObjectDbGuid(lastDeath.Item1),
                            NewGUID = "@CGUID+" + creature.DbGuid,
                            RespawnTime = (uint)(packet.Time - lastDeath.Item2).TotalSeconds
                        };
                        CreatureRespawnTimes.Add(respawnTime);
                        CreatureDeathTimes.Remove(obj.OriginalMovement.Position);
                    }
                }

                if (creature.IsInCombat())
                    creature.EnterCombatTime = packet.Time;
            }

            Storage.Objects.Add(guid, obj, packet.TimeSpan);
        }
        public static string GetObjectDbGuid(WowGuid guid)
        {
            if (Objects.ContainsKey(guid))
            {
                if (guid.GetObjectType() == ObjectType.Unit)
                {
                    Unit creature = Objects[guid].Item1 as Unit;
                    if (creature != null)
                        return "@CGUID+" + creature.DbGuid;
                }
                else if (guid.GetObjectType() == ObjectType.GameObject)
                {
                    GameObject gameobject = Objects[guid].Item1 as GameObject;
                    if (gameobject != null)
                        return "@OGUID+" + gameobject.DbGuid;
                }
                else if (guid.GetObjectType() == ObjectType.Player ||
                         guid.GetObjectType() == ObjectType.ActivePlayer)
                {
                    Player player = Objects[guid].Item1 as Player;
                    if (player != null)
                        return "@PGUID+" + player.DbGuid;
                }
            }
            return "0";
        }
        public static string GetObjectTypeNameForDB(WowGuid guid)
        {
            if (guid.IsEmpty())
                return "";

            ObjectType objectType = guid.GetObjectType();

            if (objectType == ObjectType.Unit)
            {
                switch (guid.GetHighType())
                {
                    case HighGuidType.Vehicle:
                    case HighGuidType.Creature:
                    case HighGuidType.Pet:
                        return guid.GetHighType().ToString();
                    default: // vanilla or tbc sniff with broken high types
                        return "Creature";
                }
            }
            else if (objectType == ObjectType.Player ||
                     objectType == ObjectType.ActivePlayer)
            {
                return "Player";
            }
            else if (objectType == ObjectType.GameObject)
            {
                switch (guid.GetHighType())
                {
                    case HighGuidType.GameObject:
                    case HighGuidType.Transport:
                        return guid.GetHighType().ToString();
                    default: // vanilla or tbc sniff with broken high types
                        return "GameObject";
                }
            }

            return objectType.ToString();
        }
        public static void GetObjectDbGuidEntryType(WowGuid guid, out string objectGuid, out uint objectEntry, out string objectType)
        {
            if (guid == null || guid.IsEmpty())
            {
                objectGuid = "0";
                objectEntry = 0;
                objectType = "";
                return;
            }

            objectType = GetObjectTypeNameForDB(guid);

            if (Objects.ContainsKey(guid))
            {
                if (guid.GetObjectType() == ObjectType.Unit)
                {
                    Unit creature = Objects[guid].Item1 as Unit;
                    if (creature != null)
                        objectGuid = "@CGUID+" + creature.DbGuid;
                    else
                        objectGuid = "0";

                    objectEntry = guid.GetEntry();

                    return;
                }
                else if (guid.GetObjectType() == ObjectType.GameObject)
                {
                    GameObject gameobject = Objects[guid].Item1 as GameObject;
                    if (gameobject != null)
                        objectGuid = "@OGUID+" + gameobject.DbGuid;
                    else
                        objectGuid = "0";

                    objectEntry = guid.GetEntry();

                    return;
                }
                else if (guid.GetObjectType() == ObjectType.Player ||
                         guid.GetObjectType() == ObjectType.ActivePlayer)
                {
                    Player player = Objects[guid].Item1 as Player;
                    if (player != null)
                        objectGuid = "@PGUID+" + player.DbGuid;
                    else
                        objectGuid = "0";

                    objectEntry = 0;

                    return;
                }
                else if (guid.GetObjectType() == ObjectType.Item)
                {
                    objectGuid = "0";
                    objectEntry = (uint)Objects[guid].Item1.ObjectData.EntryID;

                    return;
                }
            }
            objectGuid = "0";
            objectEntry = guid.GetEntry();
        }
        public static uint GetObjectEntry(WowGuid guid)
        {
            if (guid.HasEntry() && guid.GetHighType() != HighGuidType.Pet)
                return guid.GetEntry();

            if (Objects.ContainsKey(guid))
                return (uint)Objects[guid].Item1.ObjectData.EntryID;

            return 0;
        }
        public static uint GetCurrentObjectEntry(WowGuid guid)
        {
            if (Objects.ContainsKey(guid))
                return (uint)Objects[guid].Item1.ObjectData.EntryID;

            if (guid.HasEntry() && guid.GetHighType() != HighGuidType.Pet)
                return guid.GetEntry();

            return 0;
        }

        public static readonly Dictionary<WowGuid, List<DateTime>> ObjectDestroyTimes = new Dictionary<WowGuid, List<DateTime>>();
        public static void StoreObjectDestroyTime(WowGuid guid, DateTime time)
        {
            if (guid.GetObjectType() != ObjectType.Unit &&
                guid.GetObjectType() != ObjectType.GameObject &&
                guid.GetObjectType() != ObjectType.DynamicObject &&
                guid.GetObjectType() != ObjectType.Player &&
                guid.GetObjectType() != ObjectType.ActivePlayer)
                return;

            if (guid.GetObjectType() == ObjectType.Unit && !Settings.SqlTables.creature_destroy_time)
                return;

            if (guid.GetObjectType() == ObjectType.GameObject && !Settings.SqlTables.gameobject_destroy_time)
                return;

            if (guid.GetObjectType() == ObjectType.DynamicObject && !Settings.SqlTables.dynamicobject_destroy_time)
                return;

            if (guid.GetObjectType() == ObjectType.Player && !Settings.SqlTables.player_destroy_time)
                return;

            if (guid.GetObjectType() == ObjectType.ActivePlayer && !Settings.SqlTables.player_destroy_time)
                return;

            if (Storage.ObjectDestroyTimes.ContainsKey(guid))
            {
                Storage.ObjectDestroyTimes[guid].Add(time);
            }
            else
            {
                List<DateTime> timeList = new List<DateTime>();
                timeList.Add(time);
                Storage.ObjectDestroyTimes.Add(guid, timeList);
            }
        }
        public static readonly Dictionary<WowGuid, List<ObjectCreate>> ObjectCreate1Times = new Dictionary<WowGuid, List<ObjectCreate>>();
        public static void StoreObjectCreate1Time(WowGuid guid, uint map, MovementInfo movement, DateTime time)
        {
            if (guid.GetObjectType() != ObjectType.Unit &&
                guid.GetObjectType() != ObjectType.GameObject &&
                guid.GetObjectType() != ObjectType.DynamicObject &&
                guid.GetObjectType() != ObjectType.Player &&
                guid.GetObjectType() != ObjectType.ActivePlayer)
                return;

            if (guid.GetObjectType() == ObjectType.Unit && !Settings.SqlTables.creature_create1_time)
                return;

            if (guid.GetObjectType() == ObjectType.GameObject && !Settings.SqlTables.gameobject_create1_time)
                return;

            if (guid.GetObjectType() == ObjectType.DynamicObject && !Settings.SqlTables.dynamicobject_create1_time)
                return;

            if (guid.GetObjectType() == ObjectType.Player && !Settings.SqlTables.player_create1_time)
                return;

            if (guid.GetObjectType() == ObjectType.ActivePlayer && !Settings.SqlTables.player_create1_time)
                return;

            ObjectCreate createData = new ObjectCreate();
            createData.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(time);
            if (movement != null)
            {
                createData.Map = map;
                createData.MoveInfo = movement.CopyFromMe();
            }

            if (Storage.ObjectCreate1Times.ContainsKey(guid))
            {
                Storage.ObjectCreate1Times[guid].Add(createData);
            }
            else
            {
                List<ObjectCreate> createList = new List<ObjectCreate>();
                createList.Add(createData);
                Storage.ObjectCreate1Times.Add(guid, createList);
            }
        }
        public static readonly Dictionary<WowGuid, List<ObjectCreate>> ObjectCreate2Times = new Dictionary<WowGuid, List<ObjectCreate>>();
        public static void StoreObjectCreate2Time(WowGuid guid, uint map, MovementInfo movement, DateTime time)
        {
            if (guid.GetObjectType() != ObjectType.Unit &&
                guid.GetObjectType() != ObjectType.GameObject &&
                guid.GetObjectType() != ObjectType.DynamicObject &&
                guid.GetObjectType() != ObjectType.Player &&
                guid.GetObjectType() != ObjectType.ActivePlayer)
                return;

            if (guid.GetObjectType() == ObjectType.Unit && !Settings.SqlTables.creature_create2_time)
                return;

            if (guid.GetObjectType() == ObjectType.GameObject && !Settings.SqlTables.gameobject_create2_time)
                return;

            if (guid.GetObjectType() == ObjectType.DynamicObject && !Settings.SqlTables.dynamicobject_create2_time)
                return;

            if (guid.GetObjectType() == ObjectType.Player && !Settings.SqlTables.player_create2_time)
                return;

            if (guid.GetObjectType() == ObjectType.ActivePlayer && !Settings.SqlTables.player_create2_time)
                return;

            ObjectCreate createData = new ObjectCreate();
            createData.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(time);
            if (movement != null)
            {
                createData.Map = map;
                createData.MoveInfo = movement.CopyFromMe();
            }

            if (Storage.ObjectCreate2Times.ContainsKey(guid))
            {
                Storage.ObjectCreate2Times[guid].Add(createData);
            }
            else
            {
                List<ObjectCreate> createList = new List<ObjectCreate>();
                createList.Add(createData);
                Storage.ObjectCreate2Times.Add(guid, createList);
            }
        }
        public static void StoreObjectCreateTime(WowGuid guid, uint map, MovementInfo movement, DateTime time, ObjectCreateType type)
        {
            if (type == ObjectCreateType.Create1)
                StoreObjectCreate1Time(guid, map, movement, time);
            else if (type == ObjectCreateType.Create2)
                StoreObjectCreate2Time(guid, map, movement, time);

            WoWObject obj;
            if (Storage.Objects.TryGetValue(guid, out obj))
            {
                obj.LastCreateTime = time;
                obj.LastCreateType = type;
            }
        }
        public static readonly DataBag<CreatureRespawnTime> CreatureRespawnTimes = new DataBag<CreatureRespawnTime>(Settings.SqlTables.creature_respawn_time);
        public static readonly Dictionary<Vector3, Tuple<WowGuid, DateTime>> CreatureDeathTimes = new Dictionary<Vector3, Tuple<WowGuid, DateTime>>();
        public static Tuple<uint, DateTime> LastCreatureKill = null;
        public static void StoreCreatureDeathTime(WowGuid guid, DateTime time)
        {
            if (Settings.SqlTables.creature_kill_reputation)
            {
                LastCreatureKill = new Tuple<uint, DateTime>(guid.GetEntry(), time);
            }
            if (Settings.SqlTables.creature_respawn_time)
            {
                WoWObject obj;
                if (Storage.Objects.TryGetValue(guid, out obj))
                {
                    CreatureDeathTimes.Remove(obj.OriginalMovement.Position);
                    CreatureDeathTimes.Add(obj.OriginalMovement.Position, new Tuple<WowGuid, DateTime>(guid, time));
                }
            }
        }
        public static readonly DataBag<SpellAuraFlags> SpellAuraFlags = new DataBag<SpellAuraFlags>(Settings.SqlTables.spell_aura_flags);
        public static readonly Dictionary<WowGuid, List<Tuple<List<Aura>, DateTime>>> UnitAurasUpdates = new Dictionary<WowGuid, List<Tuple<List<Aura>, DateTime>>>();
        public static void StoreUnitAurasUpdate(WowGuid guid, List<Aura> auras, DateTime time, bool isFullUpdate)
        {
            if (Settings.SqlTables.spell_aura_flags && auras != null)
            {
                foreach (Aura aura in auras)
                {
                    if (aura.SpellId == 0)
                        continue;

                    SpellAuraFlags flags = new SpellAuraFlags
                    {
                        SpellId = aura.SpellId,
                        Flags = aura.AuraFlags
                    };
                    SpellAuraFlags.Add(flags);
                }
            }

            if (Storage.Objects.ContainsKey(guid))
            {
                var unit = Storage.Objects[guid].Item1 as Unit;
                if (unit != null)
                {
                    // All previous auras are cleared on full update.
                    if (isFullUpdate && unit.Auras != null)
                        unit.Auras = null;

                    // If this is the first packet that sends auras
                    // (hopefully at spawn time) add it to the "Auras" field
                    if (unit.AurasOriginal == null && (isFullUpdate || unit.FirstCreateType == ObjectCreateType.Create2) && 
                      !(unit.FirstCreateTime > time) && ((time - unit.FirstCreateTime).TotalSeconds <= 1))
                    {
                        unit.AurasOriginal = auras;
                        unit.Auras = auras.Select(aura => aura.Clone()).ToList();
                    }
                    else
                        unit.ApplyAuraUpdates(auras);

                    // All no caster auras get added to a set.
                    if (guid.GetObjectType() == ObjectType.Unit)
                        unit.CheckForTemplateAuras();

                    if (Settings.SqlTables.creature_spell_timers &&
                        guid.GetObjectType() == ObjectType.Unit &&
                        unit.HasAuraMatchingCriteria(HardcodedData.IsCrowdControlAura))
                    {
                        unit.DontSaveCombatSpellTimers = true;
                    }
                }
            }

            if (guid.GetObjectType() == ObjectType.Unit)
            {
                if (!Settings.SqlTables.creature_auras_update)
                    return;
            }
            else if (guid.GetObjectType() == ObjectType.Player ||
                     guid.GetObjectType() == ObjectType.ActivePlayer)
            {
                if (!Settings.SqlTables.player_auras_update)
                    return;
            }
            else
                return;

            if (Storage.UnitAurasUpdates.ContainsKey(guid))
            {
                Storage.UnitAurasUpdates[guid].Add(new Tuple<List<Aura>, DateTime>(auras, time));
            }
            else
            {
                List<Tuple<List<Aura>, DateTime>> updateList = new List<Tuple<List<Aura>, DateTime>>();
                updateList.Add(new Tuple<List<Aura>, DateTime>(auras, time));
                Storage.UnitAurasUpdates.Add(guid, updateList);
            }
        }
        public static readonly Dictionary<WowGuid, List<CreatureGuidValuesUpdate>> UnitGuidValuesUpdates = new Dictionary<WowGuid, List<CreatureGuidValuesUpdate>>();
        public static void StoreUnitGuidValuesUpdate(WowGuid guid, CreatureGuidValuesUpdate update)
        {
            if (guid.GetObjectType() == ObjectType.Unit)
            {
                if (!Settings.SqlTables.creature_guid_values_update)
                    return;
            }
            else if (guid.GetObjectType() == ObjectType.Player ||
                     guid.GetObjectType() == ObjectType.ActivePlayer)
            {
                if (!Settings.SqlTables.player_guid_values_update)
                    return;
            }
            else
                return;

            if (Storage.UnitGuidValuesUpdates.ContainsKey(guid))
            {
                Storage.UnitGuidValuesUpdates[guid].Add(update);
            }
            else
            {
                List<CreatureGuidValuesUpdate> updateList = new List<CreatureGuidValuesUpdate>();
                updateList.Add(update);
                Storage.UnitGuidValuesUpdates.Add(guid, updateList);
            }
        }
        public static readonly Dictionary<WowGuid, List<CreatureEquipmentValuesUpdate>> UnitEquipmentValuesUpdates = new Dictionary<WowGuid, List<CreatureEquipmentValuesUpdate>>();
        public static void StoreUnitEquipmentValuesUpdate(WowGuid guid, CreatureEquipmentValuesUpdate update)
        {
            if (guid.GetObjectType() == ObjectType.Unit)
            {
                if (!Settings.SqlTables.creature_equipment_values_update)
                    return;
            }
            else if (guid.GetObjectType() == ObjectType.Player ||
                     guid.GetObjectType() == ObjectType.ActivePlayer)
            {
                if (!Settings.SqlTables.player_equipment_values_update)
                    return;
            }
            else
                return;

            if (Storage.UnitEquipmentValuesUpdates.ContainsKey(guid))
            {
                Storage.UnitEquipmentValuesUpdates[guid].Add(update);
            }
            else
            {
                List<CreatureEquipmentValuesUpdate> updateList = new List<CreatureEquipmentValuesUpdate>();
                updateList.Add(update);
                Storage.UnitEquipmentValuesUpdates.Add(guid, updateList);
            }
        }
        public static readonly Dictionary<WowGuid, List<CreatureValuesUpdate>> UnitValuesUpdates = new Dictionary<WowGuid, List<CreatureValuesUpdate>>();
        public static void StoreUnitValuesUpdate(WowGuid guid, CreatureValuesUpdate update)
        {
            if (guid.GetObjectType() == ObjectType.Unit)
            {
                if (!Settings.SqlTables.creature_values_update)
                    return;
            }
            else if (guid.GetObjectType() == ObjectType.Player ||
                     guid.GetObjectType() == ObjectType.ActivePlayer)
            {
                if (!Settings.SqlTables.player_values_update)
                    return;
            }
            else
                return;

            if (Storage.UnitValuesUpdates.ContainsKey(guid))
            {
                Storage.UnitValuesUpdates[guid].Add(update);
            }
            else
            {
                List<CreatureValuesUpdate> updateList = new List<CreatureValuesUpdate>();
                updateList.Add(update);
                Storage.UnitValuesUpdates.Add(guid, updateList);
            }
        }
        public static readonly Dictionary<WowGuid, List<CreaturePowerValuesUpdate>> UnitPowerValuesUpdates = new Dictionary<WowGuid, List<CreaturePowerValuesUpdate>>();
        public static void StoreUnitPowerValuesUpdate(WowGuid guid, CreaturePowerValuesUpdate update)
        {
            if (guid.GetObjectType() == ObjectType.Unit)
            {
                if (!Settings.SqlTables.creature_power_values_update)
                    return;
            }
            else if (guid.GetObjectType() == ObjectType.Player ||
                     guid.GetObjectType() == ObjectType.ActivePlayer)
            {
                if (!Settings.SqlTables.player_power_values_update)
                    return;
            }
            else
                return;

            if (Storage.UnitPowerValuesUpdates.ContainsKey(guid))
            {
                Storage.UnitPowerValuesUpdates[guid].Add(update);
            }
            else
            {
                List<CreaturePowerValuesUpdate> updateList = new List<CreaturePowerValuesUpdate>();
                updateList.Add(update);
                Storage.UnitPowerValuesUpdates.Add(guid, updateList);
            }
        }
        public static readonly Dictionary<WowGuid, List<CreatureSpeedUpdate>> UnitSpeedUpdates = new Dictionary<WowGuid, List<CreatureSpeedUpdate>>();
        public static void StoreUnitSpeedUpdate(WowGuid guid, CreatureSpeedUpdate update)
        {
            if (guid.GetObjectType() == ObjectType.Unit)
            {
                if (!Settings.SqlTables.creature_speed_update)
                    return;
            }
            else if (guid.GetObjectType() == ObjectType.Player ||
                     guid.GetObjectType() == ObjectType.ActivePlayer)
            {
                if (!Settings.SqlTables.player_speed_update)
                    return;
            }
            else
                return;

            if (Storage.UnitSpeedUpdates.ContainsKey(guid))
            {
                Storage.UnitSpeedUpdates[guid].Add(update);
            }
            else
            {
                List<CreatureSpeedUpdate> updateList = new List<CreatureSpeedUpdate>();
                updateList.Add(update);
                Storage.UnitSpeedUpdates.Add(guid, updateList);
            }
        }
        public static readonly Dictionary<WowGuid, List<GameObjectUpdate>> GameObjectUpdates = new Dictionary<WowGuid, List<GameObjectUpdate>>();
        public static void StoreGameObjectUpdate(WowGuid guid, GameObjectUpdate update)
        {
            if (!Settings.SqlTables.gameobject_values_update)
                return;

            if (guid.GetObjectType() != ObjectType.GameObject)
                return;

            if (Storage.GameObjectUpdates.ContainsKey(guid))
            {
                Storage.GameObjectUpdates[guid].Add(update);
            }
            else
            {
                List<GameObjectUpdate> updateList = new List<GameObjectUpdate>();
                updateList.Add(update);
                Storage.GameObjectUpdates.Add(guid, updateList);
            }
        }
        public static readonly DataBag<GameObjectUniqueAnim> GameObjectUniqueAnims = new DataBag<GameObjectUniqueAnim>(Settings.SqlTables.gameobject_unique_anim);
        public static readonly Dictionary<WowGuid, List<GameObjectCustomAnim>> GameObjectCustomAnims = new Dictionary<WowGuid, List<GameObjectCustomAnim>>();
        public static void StoreGameObjectCustomAnim(WowGuid guid, GameObjectCustomAnim animData, int sniffId)
        {
            if (Settings.SqlTables.gameobject_unique_anim)
            {
                GameObjectUniqueAnim uniqueData = new GameObjectUniqueAnim
                {
                    GameObjectEntry = guid.GetEntry(),
                    AnimId = animData.AnimId,
                    AsDespawn = animData.AsDespawn,
                    SniffId = sniffId,
                };
                GameObjectUniqueAnims.Add(uniqueData);
            }

            if (!Settings.SqlTables.gameobject_custom_anim)
                return;

            if (guid.GetObjectType() != ObjectType.GameObject)
                return;

            if (Storage.GameObjectCustomAnims.ContainsKey(guid))
            {
                Storage.GameObjectCustomAnims[guid].Add(animData);
            }
            else
            {
                List<GameObjectCustomAnim> animList = new List<GameObjectCustomAnim>();
                animList.Add(animData);
                Storage.GameObjectCustomAnims.Add(guid, animList);
            }
        }
        public static readonly Dictionary<WowGuid, List<DateTime>> GameObjectDespawnAnims = new Dictionary<WowGuid, List<DateTime>>();
        public static void StoreGameObjectDespawnAnim(WowGuid guid, DateTime time)
        {
            if (!Settings.SqlTables.gameobject_despawn_anim)
                return;

            if (Storage.GameObjectDespawnAnims.ContainsKey(guid))
            {
                Storage.GameObjectDespawnAnims[guid].Add(time);
            }
            else
            {
                List<DateTime> timeList = new List<DateTime>();
                timeList.Add(time);
                Storage.GameObjectDespawnAnims.Add(guid, timeList);
            }
        }
        public static readonly Dictionary<WowGuid, List<DateTime>> GameObjectClientUseTimes = new Dictionary<WowGuid, List<DateTime>>();
        public static void StoreGameObjectUse(WowGuid guid, DateTime time)
        {
            if (!Settings.SqlTables.client_gameobject_use)
                return;

            if (Storage.GameObjectClientUseTimes.ContainsKey(guid))
            {
                Storage.GameObjectClientUseTimes[guid].Add(time);
            }
            else
            {
                List<DateTime> usesList = new List<DateTime>();
                usesList.Add(time);
                Storage.GameObjectClientUseTimes.Add(guid, usesList);
            }
        }
        public static readonly Dictionary<WowGuid, List<DateTime>> CreatureClientInteractTimes = new Dictionary<WowGuid, List<DateTime>>();
        public static void StoreCreatureInteract(WowGuid guid, DateTime time)
        {
            if (!Settings.SqlTables.client_creature_interact)
                return;

            if (Storage.CreatureClientInteractTimes.ContainsKey(guid))
            {
                Storage.CreatureClientInteractTimes[guid].Add(time);
            }
            else
            {
                List<DateTime> usesList = new List<DateTime>();
                usesList.Add(time);
                Storage.CreatureClientInteractTimes.Add(guid, usesList);
            }
        }
        public static readonly DataBag<CreatureUniqueEmote> CreatureUniqueEmotes = new DataBag<CreatureUniqueEmote>(Settings.SqlTables.creature_unique_emote);
        public static readonly Dictionary<WowGuid, List<CreatureEmote>> Emotes = new Dictionary<WowGuid, List<CreatureEmote>>();
        public static void StoreUnitEmote(WowGuid guid, EmoteType emote, Packet packet)
        {
            if (guid.GetObjectType() == ObjectType.Unit)
            {
                if (Settings.SqlTables.creature_unique_emote)
                {
                    CreatureUniqueEmote uniqueEmote = new CreatureUniqueEmote
                    {
                        Entry = guid.GetEntry(),
                        EmoteId = (uint)emote,
                        EmoteName = emote.ToString(),
                        SniffId = packet.SniffId
                    };
                    CreatureUniqueEmotes.Add(uniqueEmote);
                }

                if (!Settings.SqlTables.creature_emote)
                    return;
            }
            else if (guid.GetObjectType() == ObjectType.Player ||
                     guid.GetObjectType() == ObjectType.ActivePlayer)
            {
                if (!Settings.SqlTables.player_emote)
                    return;
            }
            else
                return;

            if (Storage.Emotes.ContainsKey(guid))
            {
                Storage.Emotes[guid].Add(new CreatureEmote(emote, packet.Time));
            }
            else
            {
                List<CreatureEmote> emotesList = new List<CreatureEmote>();
                emotesList.Add(new CreatureEmote(emote, packet.Time));
                Storage.Emotes.Add(guid, emotesList);
            }
        }
        public static readonly Dictionary<WowGuid, Dictionary<uint, DateTime>> LastCreatureCastGo = new Dictionary<WowGuid, Dictionary<uint, DateTime>>();
        public static void StoreCreatureCastGoTime(WowGuid guid, uint spellId, DateTime time)
        {
            if (!Settings.SqlTables.creature_pet_remaining_cooldown &&
                !Settings.SqlTables.creature_spell_timers)
                return;

            if (LastCreatureCastGo.ContainsKey(guid))
            {
                if (LastCreatureCastGo[guid].ContainsKey(spellId))
                {
                    LastCreatureCastGo[guid][spellId] = time;
                }
                else
                {
                    LastCreatureCastGo[guid].Add(spellId, time);
                }
            }
            else
            {
                Dictionary<uint, DateTime> dict = new Dictionary<uint, DateTime>();
                dict.Add(spellId, time);
                LastCreatureCastGo.Add(guid, dict);
            }
        }
        public static DateTime? GetLastCastGoTimeForCreature(WowGuid guid, uint spellId)
        {
            if (!LastCreatureCastGo.ContainsKey(guid))
                return null;

            if (!LastCreatureCastGo[guid].ContainsKey(spellId))
                return null;

            return LastCreatureCastGo[guid][spellId];
        }
        public static readonly Dictionary<WowGuid, List<CreatureThreatUpdate>> CreatureThreatUpdates = new Dictionary<WowGuid, List<CreatureThreatUpdate>>();
        public static void StoreCreatureThreatUpdate(WowGuid guid, CreatureThreatUpdate update)
        {
            if (!Settings.SqlTables.creature_threat_update)
                return;

            if (Storage.CreatureThreatUpdates.ContainsKey(guid))
            {
                Storage.CreatureThreatUpdates[guid].Add(update);
            }
            else
            {
                List<CreatureThreatUpdate> threatList = new List<CreatureThreatUpdate>();
                threatList.Add(update);
                Storage.CreatureThreatUpdates.Add(guid, threatList);
            }
        }
        public static readonly Dictionary<WowGuid, List<CreatureThreatClear>> CreatureThreatClears = new Dictionary<WowGuid, List<CreatureThreatClear>>();
        public static void StoreCreatureThreatClear(WowGuid guid, CreatureThreatClear update)
        {
            if (!Settings.SqlTables.creature_threat_clear)
                return;

            if (Storage.CreatureThreatClears.ContainsKey(guid))
            {
                Storage.CreatureThreatClears[guid].Add(update);
            }
            else
            {
                List<CreatureThreatClear> threatList = new List<CreatureThreatClear>();
                threatList.Add(update);
                Storage.CreatureThreatClears.Add(guid, threatList);
            }
        }
        public static readonly Dictionary<WowGuid, List<CreatureThreatRemove>> CreatureThreatRemoves = new Dictionary<WowGuid, List<CreatureThreatRemove>>();
        public static void StoreCreatureThreatRemove(WowGuid guid, CreatureThreatRemove update)
        {
            if (!Settings.SqlTables.creature_threat_remove)
                return;

            if (Storage.CreatureThreatRemoves.ContainsKey(guid))
            {
                Storage.CreatureThreatRemoves[guid].Add(update);
            }
            else
            {
                List<CreatureThreatRemove> threatList = new List<CreatureThreatRemove>();
                threatList.Add(update);
                Storage.CreatureThreatRemoves.Add(guid, threatList);
            }
        }
        public static readonly Dictionary<uint, Dictionary<uint, List<CreatureDamageTaken>>> CreatureMeleeDamageTaken = new Dictionary<uint, Dictionary<uint, List<CreatureDamageTaken>>>();
        private static void StoreCreatureMeleeDamageTaken(uint entry, uint level, CreatureDamageTaken damage)
        {
            if (CreatureMeleeDamageTaken.ContainsKey(entry))
            {
                if (CreatureMeleeDamageTaken[entry].ContainsKey(level))
                {
                    CreatureMeleeDamageTaken[entry][level].Add(damage);
                }
                else
                {
                    List<CreatureDamageTaken> damageList = new List<CreatureDamageTaken>();
                    damageList.Add(damage);
                    CreatureMeleeDamageTaken[entry].Add(level, damageList);
                }
            }
            else
            {
                Dictionary<uint, List<CreatureDamageTaken>> levelDict = new Dictionary<uint, List<CreatureDamageTaken>>();
                List<CreatureDamageTaken> damageList = new List<CreatureDamageTaken>();
                damageList.Add(damage);
                levelDict.Add(level, damageList);
                CreatureMeleeDamageTaken.Add(entry, levelDict);
            }
        }
        public static readonly Dictionary<uint, Dictionary<uint, List<double>>> CreatureMeleeAttackDamage = new Dictionary<uint, Dictionary<uint, List<double>>>();
        public static readonly Dictionary<uint, Dictionary<uint, List<double>>> CreatureMeleeAttackDamageDirty = new Dictionary<uint, Dictionary<uint, List<double>>>();
        private static void StoreCreatureMeleeAttackDamage(uint entry, uint level, double damage, bool dirty)
        {
            Dictionary<uint, Dictionary<uint, List<double>>> damageDict = dirty ? CreatureMeleeAttackDamageDirty : CreatureMeleeAttackDamage;
            if (damageDict.ContainsKey(entry))
            {
                if (damageDict[entry].ContainsKey(level))
                {
                    damageDict[entry][level].Add(damage);
                }
                else
                {
                    List<double> damageList = new List<double>();
                    damageList.Add(damage);
                    damageDict[entry].Add(level, damageList);
                }
            }
            else
            {
                Dictionary<uint, List<double>> levelDict = new Dictionary<uint, List<double>>();
                List<double> damageList = new List<double>();
                damageList.Add(damage);
                levelDict.Add(level, damageList);
                damageDict.Add(entry, levelDict);
            }
        }
        public static readonly Dictionary<uint, uint> CreatureMeleeAttackSchool = new Dictionary<uint, uint>();
        private static void StoreCreatureMeleeAttackSchool(uint entry, uint schoolMask)
        {
            if (CreatureMeleeAttackSchool.ContainsKey(entry))
            {
                CreatureMeleeAttackSchool[entry] |= schoolMask;
            }
            else
            {
                CreatureMeleeAttackSchool.Add(entry, schoolMask);
            }
        }
        public static readonly Dictionary<WowGuid, List<UnitMeleeAttackLog>> UnitAttackLogs = new Dictionary<WowGuid, List<UnitMeleeAttackLog>>();
        public static void StoreUnitAttackLog(UnitMeleeAttackLog attackData)
        {
            bool saveCreatureArmor = Settings.SqlTables.creature_armor &&
                                      attackData.Victim.GetHighType() == HighGuidType.Creature;
            bool saveCreatureDamage = Settings.SqlTables.creature_melee_damage &&
                                      attackData.Attacker.GetHighType() == HighGuidType.Creature;

            if ((saveCreatureArmor || saveCreatureDamage) &&
                attackData.VictimState == (uint)VictimStates.VICTIMSTATE_NORMAL &&
                attackData.TotalSchoolMask != 0 && attackData.SpellId == 0 &&
                attackData.Damage != 0 && attackData.OriginalDamage != 0 &&
                Storage.Objects.ContainsKey(attackData.Attacker))
            {
                Unit attacker = Storage.Objects[attackData.Attacker].Item1 as Unit;

                uint allowedHitInfoFlags = (uint)(SpellHitInfo.HITINFO_AFFECTS_VICTIM |
                                                      SpellHitInfo.HITINFO_UNK10 |
                                                      SpellHitInfo.HITINFO_UNK11 |
                                                      SpellHitInfo.HITINFO_UNK12);

                bool isNormalHit = ((attackData.HitInfo & (uint)SpellHitInfo.HITINFO_AFFECTS_VICTIM) != 0) &&
                                   ((attackData.HitInfo & allowedHitInfoFlags) == attackData.HitInfo) &&
                                     attackData.TotalAbsorbedDamage == 0 && attackData.TotalResistedDamage == 0 &&
                                     attackData.BlockedDamage <= 0 && attackData.OverkillDamage <= 0 &&
                                    (attacker.UnitData.Flags & (uint)UnitFlags.MainHandDisarmed) == 0;

                if (saveCreatureDamage)
                {
                    uint entry = (uint)attacker.ObjectData.EntryID;
                    uint level = (uint)attacker.UnitData.Level;

                    if (isNormalHit && !attacker.HasAuraMatchingCriteria(HardcodedData.IsModMainHandDamageAura))
                    {
                        StoreCreatureMeleeAttackDamage(entry, level, attackData.OriginalDamage, false);
                    }
                    else
                    {
                        StoreCreatureMeleeAttackDamage(entry, level, attackData.OriginalDamage, true);
                    }

                    StoreCreatureMeleeAttackSchool(entry, attackData.TotalSchoolMask);
                }

                if (saveCreatureArmor && isNormalHit &&
                    attackData.OriginalDamage >= 10 &&
                    attackData.Damage < attackData.OriginalDamage &&
                    Storage.Objects.ContainsKey(attackData.Victim))
                {
                    Unit victim = Storage.Objects[attackData.Victim].Item1 as Unit;
                    uint victimEntry = (uint)victim.ObjectData.EntryID;
                    int victimLevel = victim.UnitData.Level;
                    int attackerLevel = attacker.UnitData.Level;

                    if ( Math.Abs(victimLevel - attackerLevel) < 10 &&
                        !victim.HasAuraMatchingCriteria(HardcodedData.IsModResistAura) &&
                        !victim.HasAuraMatchingCriteria(HardcodedData.IsModPhysicalDamageTakenAura))
                    {
                        StoreCreatureMeleeDamageTaken(victimEntry, (uint)victimLevel, new CreatureDamageTaken((uint)attackerLevel, attackData.Damage, attackData.OriginalDamage));
                    }
                }
            }

            ObjectType attackerType = attackData.Attacker.GetObjectType();

            if (attackerType == ObjectType.Unit)
            {
                if (!Settings.SqlTables.creature_attack_log)
                    return;
            }
            else if (attackerType == ObjectType.Player ||
                     attackerType == ObjectType.ActivePlayer)
            {
                if (!Settings.SqlTables.player_attack_log)
                    return;
            }
            else
                return;

            if (Storage.UnitAttackLogs.ContainsKey(attackData.Attacker))
            {
                Storage.UnitAttackLogs[attackData.Attacker].Add(attackData);
            }
            else
            {
                List<UnitMeleeAttackLog> attacksList = new List<UnitMeleeAttackLog>();
                attacksList.Add(attackData);
                Storage.UnitAttackLogs.Add(attackData.Attacker, attacksList);
            }
        }
        public static readonly Dictionary<WowGuid, List<CreatureAttackData>> UnitAttackStartTimes = new Dictionary<WowGuid, List<CreatureAttackData>>();
        public static readonly Dictionary<WowGuid, List<CreatureAttackData>> UnitAttackStopTimes = new Dictionary<WowGuid, List<CreatureAttackData>>();
        public static void StoreUnitAttackToggle(WowGuid attackerGuid, WowGuid victimGuid, DateTime time, bool start)
        {
            Dictionary<WowGuid, List<CreatureAttackData>> store = null;
            if (start)
            {
                if (attackerGuid.GetObjectType() == ObjectType.Unit &&
                    !Settings.SqlTables.creature_attack_start)
                    return;
                else if ((attackerGuid.GetObjectType() == ObjectType.Player || attackerGuid.GetObjectType() == ObjectType.ActivePlayer) &&
                         !Settings.SqlTables.player_attack_start)
                    return;

                store = UnitAttackStartTimes;
            }
            else
            {
                if (attackerGuid.GetObjectType() == ObjectType.Unit &&
                    !Settings.SqlTables.creature_attack_stop)
                    return;
                else if ((attackerGuid.GetObjectType() == ObjectType.Player || attackerGuid.GetObjectType() == ObjectType.ActivePlayer) &&
                         !Settings.SqlTables.player_attack_stop)
                    return;

                store = UnitAttackStopTimes;
            }

            if (store.ContainsKey(attackerGuid))
            {
                store[attackerGuid].Add(new CreatureAttackData(victimGuid, time));
            }
            else
            {
                List<CreatureAttackData> attackList = new List<CreatureAttackData>();
                attackList.Add(new CreatureAttackData(victimGuid, time));
                store.Add(attackerGuid, attackList);
            }
        }

        public static readonly Dictionary<WowGuid, List<uint>> CharacterSpells = new Dictionary<WowGuid, List<uint>>();
        public static void StoreCharacterSpell(WowGuid guid, uint spellId)
        {
            if (!Settings.SqlTables.character_spell)
                return;

            if (Storage.CharacterSpells.ContainsKey(guid))
            {
                Storage.CharacterSpells[guid].Add(spellId);
            }
            else
            {
                List<uint> spellList = new List<uint>();
                spellList.Add(spellId);
                Storage.CharacterSpells.Add(guid, spellList);
            }
        }
        public static void ClearTemporarySpellList()
        {
            if (Storage.CharacterSpells.ContainsKey(WowGuid64.Empty))
                Storage.CharacterSpells[WowGuid64.Empty].Clear();
        }

        public static readonly Dictionary<WowGuid, Dictionary<uint, CharacterReputationData>> CharacterReputations = new Dictionary<WowGuid, Dictionary<uint, CharacterReputationData>>();
        public static void StoreCharacterReputation(CharacterReputationData repData)
        {
            if (!Settings.SqlTables.character_reputation &&
                !Settings.SqlTables.creature_kill_reputation)
                return;

            WowGuid guid = Storage.CurrentActivePlayer;

            if (Storage.CharacterReputations.ContainsKey(guid))
            {
                if (Storage.CharacterReputations[guid].ContainsKey(repData.Faction))
                {
                    Storage.CharacterReputations[guid][repData.Faction].Standing = repData.Standing;
                    if (repData.Flags != null)
                        Storage.CharacterReputations[guid][repData.Faction].Flags = repData.Flags;
                }
                else
                {
                    if (repData.Flags == null)
                        repData.Flags = 0;
                    Storage.CharacterReputations[guid].Add(repData.Faction, repData);
                }
            }
            else
            {
                if (repData.Flags == null)
                    repData.Flags = 0;

                Dictionary<uint, CharacterReputationData> repDict = new Dictionary<uint, CharacterReputationData>();
                repDict.Add(repData.Faction, repData);
                Storage.CharacterReputations.Add(guid, repDict);
            }
        }
        public static readonly DataBag<CreatureKillReputation> CreatureKillReputations = new DataBag<CreatureKillReputation>(Settings.SqlTables.creature_kill_reputation);
        public static void StoreFactionStandingUpdate(FactionStandingUpdate update, Packet packet)
        {
            if ((Settings.SqlTables.character_reputation || Settings.SqlTables.creature_kill_reputation) &&
                Storage.CurrentActivePlayer != null &&
                Storage.CurrentActivePlayer != WowGuid64.Empty)
            {
                
                if (Settings.SqlTables.creature_kill_reputation &&
                    LastCreatureKill != null && ((packet.Time - LastCreatureKill.Item2).TotalSeconds <= 1) &&
                    Storage.Objects.ContainsKey(Storage.CurrentActivePlayer))
                {
                    
                    int? oldStanding = null;
                    if (Storage.CharacterReputations.ContainsKey(Storage.CurrentActivePlayer))
                        if (Storage.CharacterReputations[Storage.CurrentActivePlayer].ContainsKey((uint)update.ReputationListId))
                            oldStanding = Storage.CharacterReputations[Storage.CurrentActivePlayer][(uint)update.ReputationListId].Standing;

                    if (oldStanding != null)
                    {
                        Player player = Storage.Objects[Storage.CurrentActivePlayer].Item1 as Player;
                        CreatureKillReputation killRep = new CreatureKillReputation
                        {
                            Entry = LastCreatureKill.Item1,
                            ReputationListId = (uint)update.ReputationListId,
                            OldStanding = (int)oldStanding,
                            NewStanding = update.Standing,
                            PlayerLevel = (uint)player.UnitData.Level,
                            PlayerRace = player.UnitData.RaceId,
                            SniffId = packet.SniffIdString,
                            SniffBuild = ClientVersion.BuildInt,
                        };
                        CreatureKillReputations.Add(killRep);
                    }
                }

                CharacterReputationData repData = new CharacterReputationData();
                repData.Faction = (uint)update.ReputationListId;
                repData.Standing = update.Standing;
                StoreCharacterReputation(repData);
            }

            update.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(packet.Time);
            Storage.FactionStandingUpdates.Add(update);
        }
        public static void ClearTemporaryReputationList()
        {
            WowGuid guid = CurrentActivePlayer != null ? CurrentActivePlayer : WowGuid64.Empty;
            if (Storage.CharacterReputations.ContainsKey(guid))
                Storage.CharacterReputations[guid].Clear();
        }

        public static readonly List<PlayerMovement> PlayerMovements = new List<PlayerMovement>();
        public static void StorePlayerMovement(WowGuid moverGuid, MovementInfo moveInfo, Packet packet)
        {
            if (!Settings.SqlTables.player_movement_client &&
                !Settings.SqlTables.creature_movement_client)
                return;

            PlayerMovement moveData = new PlayerMovement();
            moveData.Guid = moverGuid;
            moveData.MoveInfo = moveInfo;
            moveData.Map = WowPacketParser.Parsing.Parsers.MovementHandler.CurrentMapId;
            moveData.Opcode = packet.Opcode;
            moveData.OpcodeDirection = packet.Direction;
            moveData.Time = packet.Time;
            Storage.PlayerMovements.Add(moveData);
        }
        public static readonly List<ActivePlayerCreateTime> PlayerActiveCreateTime = new List<ActivePlayerCreateTime>();

        /* Key: Entry */

        // Templates
        public static readonly DataBag<AreaTriggerTemplate> AreaTriggerTemplates = new DataBag<AreaTriggerTemplate>(Settings.SqlTables.areatrigger_template);
        public static readonly DataBag<SpellAreatriggerSpline> SpellAreaTriggerSplines = new DataBag<SpellAreatriggerSpline>(Settings.SqlTables.spell_areatrigger_splines);
        public static readonly DataBag<SpellAreatriggerVertices> SpellAreaTriggerVertices = new DataBag<SpellAreatriggerVertices>(Settings.SqlTables.spell_areatrigger_vertices);
        public static readonly DataBag<ConversationActor> ConversationActors = new DataBag<ConversationActor>(Settings.SqlTables.conversation_actors);
        public static readonly DataBag<ConversationActorTemplate> ConversationActorTemplates = new DataBag<ConversationActorTemplate>(Settings.SqlTables.conversation_actor_template);
        public static readonly DataBag<ConversationLineTemplate> ConversationLineTemplates = new DataBag<ConversationLineTemplate>(Settings.SqlTables.conversation_line_template);
        public static readonly DataBag<ConversationTemplate> ConversationTemplates = new DataBag<ConversationTemplate>(Settings.SqlTables.conversation_template);
        public static readonly DataBag<GameObjectTemplate> GameObjectTemplates = new DataBag<GameObjectTemplate>(Settings.SqlTables.gameobject_template);
        public static readonly DataBag<GameObjectTemplateQuestItem> GameObjectTemplateQuestItems = new DataBag<GameObjectTemplateQuestItem>(Settings.SqlTables.gameobject_template);
        public static readonly DataBag<ItemClientUse> ItemClientUseTimes = new DataBag<ItemClientUse>(Settings.SqlTables.client_item_use);
        public static readonly DataBag<ItemTemplate> ItemTemplates = new DataBag<ItemTemplate>(Settings.SqlTables.item_template);
        public static readonly DataBag<QuestTemplate> QuestTemplates = new DataBag<QuestTemplate>(Settings.SqlTables.quest_template);
        public static readonly DataBag<QuestObjective> QuestObjectives = new DataBag<QuestObjective>(Settings.SqlTables.quest_template);
        public static readonly DataBag<QuestVisualEffect> QuestVisualEffects = new DataBag<QuestVisualEffect>(Settings.SqlTables.quest_template);
        public static readonly DataBag<QuestRewardDisplaySpell> QuestRewardDisplaySpells = new DataBag<QuestRewardDisplaySpell>(Settings.SqlTables.quest_template);
        public static readonly DataBag<CreatureTemplate> CreatureTemplates = new DataBag<CreatureTemplate>(Settings.SqlTables.creature_template_wdb);
        public static readonly DataBag<CreatureTemplateNonWDB> CreatureTemplatesNonWDB = new DataBag<CreatureTemplateNonWDB>(Settings.SqlTables.creature_template);
        public static readonly DataBag<CreatureTemplateQuestItem> CreatureTemplateQuestItems = new DataBag<CreatureTemplateQuestItem>(Settings.SqlTables.creature_template_wdb);
        public static readonly DataBag<CreatureTemplateScaling> CreatureTemplateScalings = new DataBag<CreatureTemplateScaling>(Settings.SqlTables.creature_template_scaling);
        public static readonly DataBag<CreatureTemplateModel> CreatureTemplateModels = new DataBag<CreatureTemplateModel>(Settings.SqlTables.creature_template);
        public static readonly DataBag<CreatureStats> CreatureStats = new DataBag<CreatureStats>(Settings.SqlTables.creature_stats);
        public static readonly DataBag<CreatureStats> CreatureStatsDirty = new DataBag<CreatureStats>(Settings.SqlTables.creature_stats);

        public static void StoreCreatureStats(Unit npc, BitArray updateMaskArray, bool isPet, Packet packet)
        {
            if (!Settings.SqlTables.creature_stats)
                return;

            if (npc == null)
                return;

            uint entry = (uint)npc.ObjectData.EntryID;
            if (entry == 0)
                return;   // broken entry

            // Update fields system changed in BfA.
            if (ClientVersion.IsUsingNewUpdateFieldSystem())
                return;

            List<int> statUpdateFields = new List<int>();

            int UNIT_FIELD_MINDAMAGE = UpdateFields.GetUpdateField(UnitField.UNIT_FIELD_MINDAMAGE);
            if (UNIT_FIELD_MINDAMAGE > 0)
                statUpdateFields.Add(UNIT_FIELD_MINDAMAGE);

            int UNIT_FIELD_MAXDAMAGE = UpdateFields.GetUpdateField(UnitField.UNIT_FIELD_MAXDAMAGE);
            if (UNIT_FIELD_MAXDAMAGE > 0)
                statUpdateFields.Add(UNIT_FIELD_MAXDAMAGE);

            int UNIT_FIELD_MINOFFHANDDAMAGE = UpdateFields.GetUpdateField(UnitField.UNIT_FIELD_MINOFFHANDDAMAGE);
            if (UNIT_FIELD_MINOFFHANDDAMAGE > 0)
                statUpdateFields.Add(UNIT_FIELD_MINOFFHANDDAMAGE);

            int UNIT_FIELD_MAXOFFHANDDAMAGE = UpdateFields.GetUpdateField(UnitField.UNIT_FIELD_MAXOFFHANDDAMAGE);
            if (UNIT_FIELD_MAXOFFHANDDAMAGE > 0)
                statUpdateFields.Add(UNIT_FIELD_MAXOFFHANDDAMAGE);

            int UNIT_FIELD_MINRANGEDDAMAGE = UpdateFields.GetUpdateField(UnitField.UNIT_FIELD_MINRANGEDDAMAGE);
            if (UNIT_FIELD_MINRANGEDDAMAGE > 0)
                statUpdateFields.Add(UNIT_FIELD_MINRANGEDDAMAGE);

            int UNIT_FIELD_MAXRANGEDDAMAGE = UpdateFields.GetUpdateField(UnitField.UNIT_FIELD_MAXRANGEDDAMAGE);
            if (UNIT_FIELD_MAXRANGEDDAMAGE > 0)
                statUpdateFields.Add(UNIT_FIELD_MAXRANGEDDAMAGE);

            int UNIT_FIELD_ATTACK_POWER = UpdateFields.GetUpdateField(UnitField.UNIT_FIELD_ATTACK_POWER);
            if (UNIT_FIELD_ATTACK_POWER > 0)
                statUpdateFields.Add(UNIT_FIELD_ATTACK_POWER);

            int UNIT_FIELD_ATTACK_POWER_MODS = UpdateFields.GetUpdateField(UnitField.UNIT_FIELD_ATTACK_POWER_MODS);
            if (UNIT_FIELD_ATTACK_POWER_MODS > 0)
                statUpdateFields.Add(UNIT_FIELD_ATTACK_POWER_MODS);

            int UNIT_FIELD_ATTACK_POWER_MOD_POS = UpdateFields.GetUpdateField(UnitField.UNIT_FIELD_ATTACK_POWER_MOD_POS);
            if (UNIT_FIELD_ATTACK_POWER_MOD_POS > 0)
                statUpdateFields.Add(UNIT_FIELD_ATTACK_POWER_MOD_POS);

            int UNIT_FIELD_ATTACK_POWER_MOD_NEG = UpdateFields.GetUpdateField(UnitField.UNIT_FIELD_ATTACK_POWER_MOD_NEG);
            if (UNIT_FIELD_ATTACK_POWER_MOD_NEG > 0)
                statUpdateFields.Add(UNIT_FIELD_ATTACK_POWER_MOD_NEG);

            int UNIT_FIELD_ATTACK_POWER_MULTIPLIER = UpdateFields.GetUpdateField(UnitField.UNIT_FIELD_ATTACK_POWER_MULTIPLIER);
            if (UNIT_FIELD_ATTACK_POWER_MULTIPLIER > 0)
                statUpdateFields.Add(UNIT_FIELD_ATTACK_POWER_MULTIPLIER);

            int UNIT_FIELD_RANGED_ATTACK_POWER = UpdateFields.GetUpdateField(UnitField.UNIT_FIELD_RANGED_ATTACK_POWER);
            if (UNIT_FIELD_RANGED_ATTACK_POWER > 0)
                statUpdateFields.Add(UNIT_FIELD_RANGED_ATTACK_POWER);

            int UNIT_FIELD_RANGED_ATTACK_POWER_MODS = UpdateFields.GetUpdateField(UnitField.UNIT_FIELD_RANGED_ATTACK_POWER_MODS);
            if (UNIT_FIELD_RANGED_ATTACK_POWER_MODS > 0)
                statUpdateFields.Add(UNIT_FIELD_RANGED_ATTACK_POWER_MODS);

            int UNIT_FIELD_RANGED_ATTACK_POWER_MOD_POS = UpdateFields.GetUpdateField(UnitField.UNIT_FIELD_RANGED_ATTACK_POWER_MOD_POS);
            if (UNIT_FIELD_RANGED_ATTACK_POWER_MOD_POS > 0)
                statUpdateFields.Add(UNIT_FIELD_RANGED_ATTACK_POWER_MOD_POS);

            int UNIT_FIELD_RANGED_ATTACK_POWER_MOD_NEG = UpdateFields.GetUpdateField(UnitField.UNIT_FIELD_RANGED_ATTACK_POWER_MOD_NEG);
            if (UNIT_FIELD_RANGED_ATTACK_POWER_MOD_NEG > 0)
                statUpdateFields.Add(UNIT_FIELD_RANGED_ATTACK_POWER_MOD_NEG);

            int UNIT_FIELD_RANGED_ATTACK_POWER_MULTIPLIER = UpdateFields.GetUpdateField(UnitField.UNIT_FIELD_RANGED_ATTACK_POWER_MULTIPLIER);
            if (UNIT_FIELD_RANGED_ATTACK_POWER_MULTIPLIER > 0)
                statUpdateFields.Add(UNIT_FIELD_RANGED_ATTACK_POWER_MULTIPLIER);

            int UNIT_FIELD_BASE_HEALTH = UpdateFields.GetUpdateField(UnitField.UNIT_FIELD_BASE_HEALTH);
            if (UNIT_FIELD_BASE_HEALTH > 0)
                statUpdateFields.Add(UNIT_FIELD_BASE_HEALTH);

            int UNIT_FIELD_BASE_MANA = UpdateFields.GetUpdateField(UnitField.UNIT_FIELD_BASE_MANA);
            // public field
            //if (UNIT_FIELD_BASE_MANA > 0)
            //    statUpdateFields.Add(UNIT_FIELD_BASE_MANA);

            int UNIT_FIELD_STAT = UpdateFields.GetUpdateField(UnitField.UNIT_FIELD_STAT);
            if (UNIT_FIELD_STAT <= 0)
                UNIT_FIELD_STAT = UpdateFields.GetUpdateField(UnitField.UNIT_FIELD_STAT0);
            if (UNIT_FIELD_STAT > 0)
                statUpdateFields.Add(UNIT_FIELD_STAT);

            int UNIT_FIELD_POSSTAT = UpdateFields.GetUpdateField(UnitField.UNIT_FIELD_POSSTAT);
            if (UNIT_FIELD_POSSTAT <= 0)
                UNIT_FIELD_POSSTAT = UpdateFields.GetUpdateField(UnitField.UNIT_FIELD_POSSTAT0);
            if (UNIT_FIELD_POSSTAT > 0)
                statUpdateFields.Add(UNIT_FIELD_POSSTAT);

            int UNIT_FIELD_NEGSTAT = UpdateFields.GetUpdateField(UnitField.UNIT_FIELD_NEGSTAT);
            if (UNIT_FIELD_NEGSTAT <= 0)
                UNIT_FIELD_NEGSTAT = UpdateFields.GetUpdateField(UnitField.UNIT_FIELD_NEGSTAT0);
            if (UNIT_FIELD_NEGSTAT > 0)
                statUpdateFields.Add(UNIT_FIELD_NEGSTAT);

            int UNIT_FIELD_RESISTANCES = UpdateFields.GetUpdateField(UnitField.UNIT_FIELD_RESISTANCES);
            if (UNIT_FIELD_RESISTANCES <= 0)
                UNIT_FIELD_RESISTANCES = UpdateFields.GetUpdateField(UnitField.UNIT_FIELD_RESISTANCES_ARMOR);
            if (UNIT_FIELD_RESISTANCES > 0)
                statUpdateFields.Add(UNIT_FIELD_RESISTANCES);

            int UNIT_FIELD_RESISTANCEBUFFMODSPOSITIVE = UpdateFields.GetUpdateField(UnitField.UNIT_FIELD_RESISTANCEBUFFMODSPOSITIVE);
            if (UNIT_FIELD_RESISTANCEBUFFMODSPOSITIVE <= 0)
                UNIT_FIELD_RESISTANCEBUFFMODSPOSITIVE = UpdateFields.GetUpdateField(UnitField.UNIT_FIELD_RESISTANCEBUFFMODSPOSITIVE_ARMOR);
            if (UNIT_FIELD_RESISTANCEBUFFMODSPOSITIVE <= 0)
                UNIT_FIELD_RESISTANCEBUFFMODSPOSITIVE = UpdateFields.GetUpdateField(UnitField.UNIT_FIELD_BONUS_RESISTANCE_MODS);
            if (UNIT_FIELD_RESISTANCEBUFFMODSPOSITIVE > 0)
                statUpdateFields.Add(UNIT_FIELD_RESISTANCEBUFFMODSPOSITIVE);

            int UNIT_FIELD_RESISTANCEBUFFMODSNEGATIVE = UpdateFields.GetUpdateField(UnitField.UNIT_FIELD_RESISTANCEBUFFMODSNEGATIVE);
            if (UNIT_FIELD_RESISTANCEBUFFMODSNEGATIVE <= 0)
                UNIT_FIELD_RESISTANCEBUFFMODSNEGATIVE = UpdateFields.GetUpdateField(UnitField.UNIT_FIELD_RESISTANCEBUFFMODSNEGATIVE_ARMOR);
            if (UNIT_FIELD_RESISTANCEBUFFMODSNEGATIVE <= 0)
                UNIT_FIELD_RESISTANCEBUFFMODSNEGATIVE = UpdateFields.GetUpdateField(UnitField.UNIT_FIELD_BONUS_RESISTANCE_MODS);
            if (UNIT_FIELD_RESISTANCEBUFFMODSNEGATIVE > 0)
                statUpdateFields.Add(UNIT_FIELD_RESISTANCEBUFFMODSNEGATIVE);

            if (statUpdateFields.Count == 0)
                return;

            bool hasStatUpdate = updateMaskArray == null; // continue anyway if no mask array
            if (!hasStatUpdate)
            {
                foreach (int uf in statUpdateFields)
                {
                    if (updateMaskArray[uf])
                    {
                        hasStatUpdate = true;
                        break;
                    }
                }

                if (!hasStatUpdate)
                    return;
            }

            bool hasModStatsAura = npc.HasAuraMatchingCriteria(HardcodedData.IsModStatAura);
            bool hasModMainHandDamageAura = npc.HasAuraMatchingCriteria(HardcodedData.IsModMainHandDamageAura);
            bool hasModOffHandDamageAura = npc.HasAuraMatchingCriteria(HardcodedData.IsModOffHandDamageAura);
            bool hasModRangedDamageAura = npc.HasAuraMatchingCriteria(HardcodedData.IsModRangedDamageAura);
            bool hasModMeleeAttackPowerAura = npc.HasAuraMatchingCriteria(HardcodedData.IsModMeleeAttackPowerAura);
            bool hasModRangedAttackPowerAura = npc.HasAuraMatchingCriteria(HardcodedData.IsModRangedAttackPowerAura);
            bool hasModResistAura = npc.HasAuraMatchingCriteria(HardcodedData.IsModResistAura);
            bool hasAnyBadAuras = hasModStatsAura || hasModMainHandDamageAura || hasModOffHandDamageAura ||
                                  hasModRangedDamageAura || hasModMeleeAttackPowerAura || hasModRangedAttackPowerAura ||
                                  hasModResistAura;
            bool hasData = false;
            CreatureStats creatureStats = new CreatureStats();
            UpdateField value;

            if (!hasModMainHandDamageAura || isPet)
            {
                if (npc.UpdateFields.TryGetValue(UNIT_FIELD_MINDAMAGE, out value))
                {
                    hasData = true;
                    creatureStats.DmgMin = value.FloatValue;
                }
                if (npc.UpdateFields.TryGetValue(UNIT_FIELD_MAXDAMAGE, out value))
                {
                    hasData = true;
                    creatureStats.DmgMax = value.FloatValue;
                }
            }

            if (!hasModOffHandDamageAura || isPet)
            {
                if (npc.UpdateFields.TryGetValue(UNIT_FIELD_MINOFFHANDDAMAGE, out value))
                {
                    hasData = true;
                    creatureStats.OffhandDmgMin = value.FloatValue;
                }
                if (npc.UpdateFields.TryGetValue(UNIT_FIELD_MAXOFFHANDDAMAGE, out value))
                {
                    hasData = true;
                    creatureStats.OffhandDmgMax = value.FloatValue;
                }
            }
            
            if (!hasModRangedDamageAura || isPet)
            {
                if (npc.UpdateFields.TryGetValue(UNIT_FIELD_MINRANGEDDAMAGE, out value))
                {
                    hasData = true;
                    creatureStats.RangedDmgMin = value.FloatValue;
                }
                if (npc.UpdateFields.TryGetValue(UNIT_FIELD_MAXRANGEDDAMAGE, out value))
                {
                    hasData = true;
                    creatureStats.RangedDmgMax = value.FloatValue;
                }
            }

            if (!hasModMeleeAttackPowerAura || isPet)
            {
                if (npc.UpdateFields.TryGetValue(UNIT_FIELD_ATTACK_POWER, out value))
                {
                    hasData = true;
                    creatureStats.AttackPower = value.Int32Value;
                }
                if (npc.UpdateFields.TryGetValue(UNIT_FIELD_ATTACK_POWER_MODS, out value))
                {
                    hasData = true;
                    creatureStats.PositiveAttackPower = (int)(value.UInt32Value & 0x0000FFFF);
                    creatureStats.NegativeAttackPower = (int)((value.UInt32Value & 0xFFFF0000) >> 16);
                }
                else
                {
                    if (npc.UpdateFields.TryGetValue(UNIT_FIELD_ATTACK_POWER_MOD_POS, out value))
                    {
                        hasData = true;
                        creatureStats.PositiveAttackPower = value.Int32Value;
                    }
                    if (npc.UpdateFields.TryGetValue(UNIT_FIELD_ATTACK_POWER_MOD_NEG, out value))
                    {
                        hasData = true;
                        creatureStats.NegativeAttackPower = value.Int32Value;
                    }
                }
                if (npc.UpdateFields.TryGetValue(UNIT_FIELD_ATTACK_POWER_MULTIPLIER, out value))
                {
                    hasData = true;
                    creatureStats.AttackPowerMultiplier = value.FloatValue;
                }
            }

            if (!hasModRangedAttackPowerAura || isPet)
            {
                if (npc.UpdateFields.TryGetValue(UNIT_FIELD_RANGED_ATTACK_POWER, out value))
                {
                    hasData = true;
                    creatureStats.RangedAttackPower = value.Int32Value;
                }
                if (npc.UpdateFields.TryGetValue(UNIT_FIELD_RANGED_ATTACK_POWER_MODS, out value))
                {
                    hasData = true;
                    creatureStats.PositiveRangedAttackPower = (int)(value.UInt32Value & 0x0000FFFF);
                    creatureStats.NegativeRangedAttackPower = (int)((value.UInt32Value & 0xFFFF0000) >> 16);
                }
                else
                {
                    if (npc.UpdateFields.TryGetValue(UNIT_FIELD_RANGED_ATTACK_POWER_MOD_POS, out value))
                    {
                        hasData = true;
                        creatureStats.PositiveRangedAttackPower = value.Int32Value;
                    }
                    if (npc.UpdateFields.TryGetValue(UNIT_FIELD_RANGED_ATTACK_POWER_MOD_NEG, out value))
                    {
                        hasData = true;
                        creatureStats.NegativeRangedAttackPower = value.Int32Value;
                    }
                }
                if (npc.UpdateFields.TryGetValue(UNIT_FIELD_RANGED_ATTACK_POWER_MULTIPLIER, out value))
                {
                    hasData = true;
                    creatureStats.RangedAttackPowerMultiplier = value.FloatValue;
                }
            }
            
            if (npc.UpdateFields.TryGetValue(UNIT_FIELD_BASE_HEALTH, out value))
            {
                hasData = true;
                creatureStats.BaseHealth = value.UInt32Value;
            }
            if (npc.UpdateFields.TryGetValue(UNIT_FIELD_BASE_MANA, out value))
            {
                //hasData = true; public field
                creatureStats.BaseMana = value.UInt32Value;
            }

            if (!hasModStatsAura || isPet)
            {
                Func<int, bool> SaveStats = delegate (int field)
                {
                    if (npc.UpdateFields.TryGetValue(field, out value))
                    {
                        hasData = true;
                        creatureStats.Strength = value.Int32Value;
                    }
                    if (npc.UpdateFields.TryGetValue(field + 1, out value))
                    {
                        hasData = true;
                        creatureStats.Agility = value.Int32Value;
                    }
                    if (npc.UpdateFields.TryGetValue(field + 2, out value))
                    {
                        hasData = true;
                        creatureStats.Stamina = value.Int32Value;
                    }
                    if (npc.UpdateFields.TryGetValue(field + 3, out value))
                    {
                        hasData = true;
                        creatureStats.Intellect = value.Int32Value;
                    }
                    if (npc.UpdateFields.TryGetValue(field + 4, out value))
                    {
                        hasData = true;
                        creatureStats.Spirit = value.Int32Value;
                    }
                    return true;
                };
                if (UNIT_FIELD_STAT > 0)
                    SaveStats(UNIT_FIELD_STAT);

                Func<int, bool> SavePosStats = delegate (int field)
                {
                    if (npc.UpdateFields.TryGetValue(field, out value))
                    {
                        hasData = true;
                        creatureStats.PositiveStrength = value.Int32Value;
                    }
                    if (npc.UpdateFields.TryGetValue(field + 1, out value))
                    {
                        hasData = true;
                        creatureStats.PositiveAgility = value.Int32Value;
                    }
                    if (npc.UpdateFields.TryGetValue(field + 2, out value))
                    {
                        hasData = true;
                        creatureStats.PositiveStamina = value.Int32Value;
                    }
                    if (npc.UpdateFields.TryGetValue(field + 3, out value))
                    {
                        hasData = true;
                        creatureStats.PositiveIntellect = value.Int32Value;
                    }
                    if (npc.UpdateFields.TryGetValue(field + 4, out value))
                    {
                        hasData = true;
                        creatureStats.PositiveSpirit = value.Int32Value;
                    }
                    return true;
                };
                if (UNIT_FIELD_POSSTAT > 0)
                    SavePosStats(UNIT_FIELD_POSSTAT);

                Func<int, bool> SaveNegStats = delegate (int field)
                {
                    if (npc.UpdateFields.TryGetValue(field, out value))
                    {
                        hasData = true;
                        creatureStats.NegativeStrength = value.Int32Value;
                    }
                    if (npc.UpdateFields.TryGetValue(field + 1, out value))
                    {
                        hasData = true;
                        creatureStats.NegativeAgility = value.Int32Value;
                    }
                    if (npc.UpdateFields.TryGetValue(field + 2, out value))
                    {
                        hasData = true;
                        creatureStats.NegativeStamina = value.Int32Value;
                    }
                    if (npc.UpdateFields.TryGetValue(field + 3, out value))
                    {
                        hasData = true;
                        creatureStats.NegativeIntellect = value.Int32Value;
                    }
                    if (npc.UpdateFields.TryGetValue(field + 4, out value))
                    {
                        hasData = true;
                        creatureStats.NegativeSpirit = value.Int32Value;
                    }
                    return true;
                };
                if (UNIT_FIELD_NEGSTAT > 0)
                    SaveNegStats(UNIT_FIELD_NEGSTAT);
            }

            if (!hasModResistAura || isPet)
            {
                Func<int, bool> SaveResistances = delegate (int field)
                {
                    if (npc.UpdateFields.TryGetValue(field, out value))
                    {
                        hasData = true;
                        creatureStats.Armor = value.Int32Value;
                    }
                    if (npc.UpdateFields.TryGetValue(field + 1, out value))
                    {
                        hasData = true;
                        creatureStats.HolyResistance = value.Int32Value;
                    }
                    if (npc.UpdateFields.TryGetValue(field + 2, out value))
                    {
                        hasData = true;
                        creatureStats.FireResistance = value.Int32Value;
                    }
                    if (npc.UpdateFields.TryGetValue(field + 3, out value))
                    {
                        hasData = true;
                        creatureStats.NatureResistance = value.Int32Value;
                    }
                    if (npc.UpdateFields.TryGetValue(field + 4, out value))
                    {
                        hasData = true;
                        creatureStats.FrostResistance = value.Int32Value;
                    }
                    if (npc.UpdateFields.TryGetValue(field + 5, out value))
                    {
                        hasData = true;
                        creatureStats.ShadowResistance = value.Int32Value;
                    }
                    if (npc.UpdateFields.TryGetValue(field + 6, out value))
                    {
                        hasData = true;
                        creatureStats.ArcaneResistance = value.Int32Value;
                    }
                    return true;
                };
                if (UNIT_FIELD_RESISTANCES > 0)
                    SaveResistances(UNIT_FIELD_RESISTANCES);

                Func<int, bool> SavePositiveResistances = delegate (int field)
                {
                    if (npc.UpdateFields.TryGetValue(field, out value))
                    {
                        hasData = true;
                        creatureStats.PositiveArmor = value.Int32Value;
                    }
                    if (npc.UpdateFields.TryGetValue(field + 1, out value))
                    {
                        hasData = true;
                        creatureStats.PositiveHolyResistance = value.Int32Value;
                    }
                    if (npc.UpdateFields.TryGetValue(field + 2, out value))
                    {
                        hasData = true;
                        creatureStats.PositiveFireResistance = value.Int32Value;
                    }
                    if (npc.UpdateFields.TryGetValue(field + 3, out value))
                    {
                        hasData = true;
                        creatureStats.PositiveNatureResistance = value.Int32Value;
                    }
                    if (npc.UpdateFields.TryGetValue(field + 4, out value))
                    {
                        hasData = true;
                        creatureStats.PositiveFrostResistance = value.Int32Value;
                    }
                    if (npc.UpdateFields.TryGetValue(field + 5, out value))
                    {
                        hasData = true;
                        creatureStats.PositiveShadowResistance = value.Int32Value;
                    }
                    if (npc.UpdateFields.TryGetValue(field + 6, out value))
                    {
                        hasData = true;
                        creatureStats.PositiveArcaneResistance = value.Int32Value;
                    }
                    return true;
                };
                if (UNIT_FIELD_RESISTANCEBUFFMODSPOSITIVE > 0)
                    SavePositiveResistances(UNIT_FIELD_RESISTANCEBUFFMODSPOSITIVE);

                Func<int, bool> SaveNegativeResistances = delegate (int field)
                {
                    if (npc.UpdateFields.TryGetValue(field, out value))
                    {
                        hasData = true;
                        creatureStats.NegativeArmor = value.Int32Value;
                    }
                    if (npc.UpdateFields.TryGetValue(field + 1, out value))
                    {
                        hasData = true;
                        creatureStats.NegativeHolyResistance = value.Int32Value;
                    }
                    if (npc.UpdateFields.TryGetValue(field + 2, out value))
                    {
                        hasData = true;
                        creatureStats.NegativeFireResistance = value.Int32Value;
                    }
                    if (npc.UpdateFields.TryGetValue(field + 3, out value))
                    {
                        hasData = true;
                        creatureStats.NegativeNatureResistance = value.Int32Value;
                    }
                    if (npc.UpdateFields.TryGetValue(field + 4, out value))
                    {
                        hasData = true;
                        creatureStats.NegativeFrostResistance = value.Int32Value;
                    }
                    if (npc.UpdateFields.TryGetValue(field + 5, out value))
                    {
                        hasData = true;
                        creatureStats.NegativeShadowResistance = value.Int32Value;
                    }
                    if (npc.UpdateFields.TryGetValue(field + 6, out value))
                    {
                        hasData = true;
                        creatureStats.NegativeArcaneResistance = value.Int32Value;
                    }
                    return true;
                };
                if (UNIT_FIELD_RESISTANCEBUFFMODSNEGATIVE > 0)
                    SaveNegativeResistances(UNIT_FIELD_RESISTANCEBUFFMODSNEGATIVE);
            }

            if (hasData)
            {
                creatureStats.Entry = entry;
                creatureStats.Level = (uint)npc.UnitData.Level;
                creatureStats.IsPet = isPet;
                creatureStats.IsDirty = hasAnyBadAuras;
                creatureStats.SniffId = packet.SniffIdString;

                if (hasAnyBadAuras)
                {
                    creatureStats.Auras = npc.GetAurasString(false);
                    Storage.CreatureStatsDirty.Add(creatureStats);
                }
                else
                    Storage.CreatureStats.Add(creatureStats);
            }
        }

        // Vendor & trainer
        public static readonly DataBag<NpcTrainer> NpcTrainers = new DataBag<NpcTrainer>(Settings.SqlTables.npc_trainer); // legacy 3.3.5 support
        public static readonly DataBag<NpcVendor> NpcVendors = new DataBag<NpcVendor>(Settings.SqlTables.npc_vendor);
        public static readonly DataBag<Trainer> Trainers = new DataBag<Trainer>(Settings.SqlTables.trainer);
        public static readonly DataBag<TrainerSpell> TrainerSpells = new DataBag<TrainerSpell>(Settings.SqlTables.trainer);
        public static readonly DataBag<CreatureTrainer> CreatureTrainers = new DataBag<CreatureTrainer>(Settings.SqlTables.trainer);

        // Loot
        public static readonly Dictionary<uint, Dictionary<WowGuid, LootEntry>> CreatureLoot = new Dictionary<uint, Dictionary<WowGuid, LootEntry>>();
        public static readonly Dictionary<uint, Dictionary<WowGuid, LootEntry>> GameObjectLoot = new Dictionary<uint, Dictionary<WowGuid, LootEntry>>();
        public static void StoreLoot(LootEntry loot, WowGuid objectGuid, WowGuid lootGuid)
        {
            Dictionary<uint, Dictionary<WowGuid, LootEntry>> lootStorage = null;
            if (objectGuid.GetObjectType() == ObjectType.Unit)
            {
                if (!Settings.SqlTables.creature_loot)
                    return;

                lootStorage = CreatureLoot;
            }
            else if (objectGuid.GetObjectType() == ObjectType.GameObject)
            {
                if (!Settings.SqlTables.gameobject_loot)
                    return;

                lootStorage = GameObjectLoot;
            }
            if (lootStorage == null)
                return;

            if (lootStorage.ContainsKey(loot.Entry))
            {
                if (lootStorage[loot.Entry].ContainsKey(lootGuid))
                    return;

                loot.LootId = LootEntry.LootIdCounter++;
                foreach (LootItem item in loot.ItemsList)
                    item.LootId = loot.LootId;

                lootStorage[loot.Entry].Add(lootGuid, loot);
            }
            else
            {
                loot.LootId = LootEntry.LootIdCounter++;
                foreach (LootItem item in loot.ItemsList)
                    item.LootId = loot.LootId;

                Dictionary<WowGuid, LootEntry> dict = new Dictionary<WowGuid, LootEntry>();
                dict.Add(lootGuid, loot);
                lootStorage.Add(loot.Entry, dict);
            }
        }

        // Page & npc text
        public static readonly DataBag<PageText> PageTexts = new DataBag<PageText>(Settings.SqlTables.page_text);
        public static readonly DataBag<NpcText> NpcTexts = new DataBag<NpcText>(Settings.SqlTables.npc_text);
        public static readonly DataBag<NpcTextMop> NpcTextsMop = new DataBag<NpcTextMop>(Settings.SqlTables.npc_text);

        // Chat packet data (says, yells, etc.)
        public static readonly DataBag<WorldText> WorldTexts = new DataBag<WorldText>(Settings.SqlTables.world_text);
        public static readonly DataBag<CreatureText> CreatureTexts = new DataBag<CreatureText>(Settings.SqlTables.creature_text);
        public static readonly StoreMulti<uint, CreatureTextTemplate> CreatureTextTemplates = new StoreMulti<uint, CreatureTextTemplate>(Settings.SqlTables.creature_text_template);
        public static readonly DataBag<CharacterChat> CharacterTexts = new DataBag<CharacterChat>(Settings.SqlTables.player_chat);

        public static void StoreText(ChatPacketData text, Packet packet)
        {
            uint creatureId = 0;
            if (text.SenderGUID.GetObjectType() == ObjectType.Unit)
                creatureId = text.SenderGUID.GetEntry();
            else if (text.ReceiverGUID != null && text.ReceiverGUID.GetObjectType() == ObjectType.Unit)
                creatureId = text.ReceiverGUID.GetEntry();

            text.Time = packet.Time;

            if (creatureId != 0)
            {
                if (Settings.SqlTables.creature_text_template)
                {
                    CreatureTextTemplate textTemplate = new CreatureTextTemplate(text, packet.SniffId);
                    textTemplate.Entry = creatureId;
                    if (Storage.Objects.ContainsKey(textTemplate.SenderGUID))
                    {
                        var obj = Storage.Objects[textTemplate.SenderGUID].Item1 as Unit;
                        textTemplate.HealthPercent = obj.UnitData.HealthPercent;
                    }
                    Storage.CreatureTextTemplates.Add(creatureId, textTemplate, packet.TimeSpan);

                    if (Settings.SqlTables.creature_text)
                    {
                        CreatureText textEntry = new CreatureText();
                        textEntry.Entry = creatureId;
                        textEntry.Text = textTemplate.Text;
                        textEntry.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(packet.Time);
                        textEntry.SenderGUID = textTemplate.SenderGUID;
                        textEntry.ReceiverGUID = textTemplate.ReceiverGUID;
                        Storage.CreatureTexts.Add(textEntry);
                    }
                }
            }
            else if (((text.SenderGUID.GetObjectType() == ObjectType.Player) || (text.SenderName != null && text.TypeNormalized == ChatMessageType.Channel)) &&
                     (text.Language != Language.Addon && text.Language != Language.AddonBfA && text.Language != Language.AddonLogged))
            {
                if (Settings.SqlTables.player_chat)
                {
                    var textEntry = new CharacterChat
                    {
                        SenderGUID = text.SenderGUID,
                        SenderName = text.SenderName,
                        Text = text.Text,
                        Type = text.TypeOriginal,
                        ChannelName = text.ChannelName,
                        UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(packet.Time)
                    };
                    Storage.CharacterTexts.Add(textEntry);
                }
            }
            else if (text.SenderGUID.IsEmpty() && (text.TypeNormalized == ChatMessageType.BattlegroundNeutral))
            {
                if (Settings.SqlTables.world_text)
                {
                    var worldText = new WorldText
                    {
                        UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(packet.Time),
                        Type = text.TypeOriginal,
                        Language = text.Language,
                        Text = text.Text
                    };
                    Storage.WorldTexts.Add(worldText);
                }
            }
        }
        // Points of Interest
        public static readonly DataBag<PointsOfInterest> GossipPOIs = new DataBag<PointsOfInterest>(Settings.SqlTables.points_of_interest);

        // "Helper" stores, do not match a specific table
        public static readonly List<ObjectSound> Sounds = new List<ObjectSound>();
        public static readonly DataBag<PlayMusic> Music = new DataBag<PlayMusic>(Settings.SqlTables.play_music);
        public static readonly StoreDictionary<uint, List<uint?>> SpellsX = new StoreDictionary<uint, List<uint?>>(Settings.SqlTables.creature_template); // `creature_template`.`spellsX`
        public static readonly DataBag<QuestOfferReward> QuestOfferRewards = new DataBag<QuestOfferReward>(Settings.SqlTables.quest_template);
        public static readonly StoreDictionary<Tuple<uint, uint>, object> GossipSelects = new StoreDictionary<Tuple<uint, uint>, object>(Settings.SqlTables.points_of_interest || Settings.SqlTables.gossip_menu || Settings.SqlTables.gossip_menu_option);

        /* Key: Misc */

        // Start info (Race, Class)
        public static readonly DataBag<PlayerCreateInfoAction> StartActions = new DataBag<PlayerCreateInfoAction>(Settings.SqlTables.playercreateinfo_action);
        public static readonly DataBag<PlayerCreateInfo> StartPositions = new DataBag<PlayerCreateInfo>(Settings.SqlTables.playercreateinfo);
        public static readonly DataBag<PlayerClassLevelStats> PlayerClassLevelStats = new DataBag<PlayerClassLevelStats>(Settings.SqlTables.player_classlevelstats);
        public static readonly DataBag<PlayerLevelStats> PlayerLevelStats = new DataBag<PlayerLevelStats>(Settings.SqlTables.player_levelstats);
        public static readonly DataBag<PlayerLevelupInfo> PlayerLevelupInfos = new DataBag<PlayerLevelupInfo>(Settings.SqlTables.player_levelup_info);
        public static readonly DataBag<PlayerCritChance> PlayerCritChances = new DataBag<PlayerCritChance>(Settings.SqlTables.player_crit_chance);
        public static readonly DataBag<PlayerDodgeChance> PlayerDodgeChances = new DataBag<PlayerDodgeChance>(Settings.SqlTables.player_dodge_chance);
        public static void SavePlayerStats(WoWObject obj, bool useInitialData)
        {
            if (!Settings.SqlTables.player_levelstats && !Settings.SqlTables.player_classlevelstats)
                return;

            Player player = obj as Player;
            if (player == null)
                return;

            var unitData = useInitialData ? player.UnitDataOriginal : player.UnitData;

            PlayerClassLevelStats classLevelStats = new PlayerClassLevelStats();
            classLevelStats.ClassId = unitData.ClassId;
            classLevelStats.Level = unitData.Level;
            classLevelStats.BaseHP = unitData.BaseHealth;
            classLevelStats.BaseMana = unitData.BaseMana;
            if (classLevelStats.BaseHP != 0)
                Storage.PlayerClassLevelStats.Add(classLevelStats);

            var stats = unitData.Stats;
            var posstats = unitData.StatPosBuff;
            var negstats = unitData.StatNegBuff;

            PlayerLevelStats levelStats = new PlayerLevelStats();
            levelStats.RaceId = unitData.RaceId;
            levelStats.ClassId = unitData.ClassId;
            levelStats.Level = unitData.Level;
            levelStats.Strength = stats[(int)StatType.Strength] - posstats[(int)StatType.Strength] - negstats[(int)StatType.Strength];
            levelStats.Agility = stats[(int)StatType.Agility] - posstats[(int)StatType.Agility] - negstats[(int)StatType.Agility];
            levelStats.Stamina = stats[(int)StatType.Stamina] - posstats[(int)StatType.Stamina] - negstats[(int)StatType.Stamina];
            levelStats.Intellect = stats[(int)StatType.Intellect] - posstats[(int)StatType.Intellect] - negstats[(int)StatType.Intellect];
            if (ClientVersion.RemovedInVersion(ClientType.Legion) || ClientVersion.IsClassicClientVersionBuild(ClientVersion.Build))
                levelStats.Spirit = stats[(int)StatType.Spirit] - posstats[(int)StatType.Spirit] - negstats[(int)StatType.Spirit];
            if (levelStats.Strength != 0 || levelStats.Agility != 0 ||
                levelStats.Stamina != 0 || levelStats.Intellect != 0 ||
                levelStats.Spirit != 0)
                Storage.PlayerLevelStats.Add(levelStats);
        }
        public static void SavePlayerCrit(WoWObject obj)
        {
            if (!Settings.SqlTables.player_crit_chance)
                return;

            Player player = obj as Player;
            if (player == null)
                return;

            float critChance = player.ActivePlayerData.CritPercentage;
            if (critChance <= 0)
                return;

            int agility = player.UnitData.Stats[(int)StatType.Agility];
            if (agility <= 0)
                return;

            ushort skillId = 0;
            ushort skillRank = 0;
            ushort skillMaxRank = 0;
            uint itemId = player.GetMainHandWeapon();

            if (itemId == 0 || HardcodedData.WeaponFists.Contains(itemId))
                skillId = (ushort)SkillType.Unarmed;
            else if (HardcodedData.WeaponAxes.Contains(itemId))
                skillId = (ushort)SkillType.Axes;
            else if (HardcodedData.WeaponTwoHandedAxes.Contains(itemId))
                skillId = (ushort)SkillType.TwoHandedAxes;
            else if (HardcodedData.WeaponMaces.Contains(itemId))
                skillId = (ushort)SkillType.Maces;
            else if (HardcodedData.WeaponTwoHandedMaces.Contains(itemId))
                skillId = (ushort)SkillType.TwoHandedMaces;
            else if (HardcodedData.WeaponPolearms.Contains(itemId))
                skillId = (ushort)SkillType.Polearms;
            else if (HardcodedData.WeaponSwords.Contains(itemId))
                skillId = (ushort)SkillType.Swords;
            else if (HardcodedData.WeaponTwoHandedSwords.Contains(itemId))
                skillId = (ushort)SkillType.TwoHandedSwords;
            else if (HardcodedData.WeaponStaves.Contains(itemId))
                skillId = (ushort)SkillType.Staves;
            else if (HardcodedData.WeaponDaggers.Contains(itemId))
                skillId = (ushort)SkillType.Daggers;
            else
                return;

            player.GetSkill(skillId, out skillRank, out skillMaxRank);
            if (skillRank == 0 || skillMaxRank == 0)
                return;

            PlayerCritChance critData = new PlayerCritChance();
            critData.RaceId = player.UnitData.RaceId;
            critData.ClassId = player.UnitData.ClassId;
            critData.Level = player.UnitData.Level;
            critData.Agility = agility;
            critData.CritChance = critChance;
            critData.WeaponItemId = itemId;
            critData.WeaponSkillId = skillId;
            critData.SkillCurrentValue = skillRank;
            critData.SkillMaxValue = skillMaxRank;
            critData.RelevantAuras = player.GetAurasStringMatchingCriteria(HardcodedData.IsModCritPercentAura);
            PlayerCritChances.Add(critData);
        }
        public static void SavePlayerDodge(WoWObject obj)
        {
            if (!Settings.SqlTables.player_dodge_chance)
                return;

            Player player = obj as Player;
            if (player == null)
                return;

            float dodgeChance = player.ActivePlayerData.DodgePercentage;
            if (dodgeChance <= 0)
                return;

            int agility = player.UnitData.Stats[(int)StatType.Agility];
            if (agility <= 0)
                return;

            ushort skillRank = 0;
            ushort skillMaxRank = 0;

            player.GetSkill((ushort)SkillType.Defense, out skillRank, out skillMaxRank);
            if (skillRank == 0 || skillMaxRank == 0)
                return;

            PlayerDodgeChance dodgeData = new PlayerDodgeChance();
            dodgeData.RaceId = player.UnitData.RaceId;
            dodgeData.ClassId = player.UnitData.ClassId;
            dodgeData.Level = player.UnitData.Level;
            dodgeData.Agility = agility;
            dodgeData.DodgeChance = dodgeChance;
            dodgeData.DefenseCurrentValue = skillRank;
            dodgeData.DefenseMaxValue = skillMaxRank;
            dodgeData.RelevantAuras = player.GetAurasStringMatchingCriteria(HardcodedData.IsModDodgePercentAura);
            PlayerDodgeChances.Add(dodgeData);
        }

        // Gossips (MenuId, TextId)
        public static readonly Dictionary<uint, uint> CreatureDefaultGossips = new Dictionary<uint, uint>();
        public static readonly DataBag<CreatureGossip> CreatureGossips = new DataBag<CreatureGossip>(Settings.SqlTables.creature_unique_gossip);
        public static readonly DataBag<GossipMenu> Gossips = new DataBag<GossipMenu>(Settings.SqlTables.gossip_menu);
        public static readonly DataBag<GossipMenuOption> GossipMenuOptions = new DataBag<GossipMenuOption>(Settings.SqlTables.gossip_menu_option);
        public static readonly DataBag<GossipMenuOptionAction> GossipMenuOptionActions = new DataBag<GossipMenuOptionAction>(Settings.SqlTables.gossip_menu_option);
        public static readonly DataBag<GossipMenuOptionBox> GossipMenuOptionBoxes = new DataBag<GossipMenuOptionBox>(Settings.SqlTables.gossip_menu_option);
        public static void StoreCreatureGossip(WowGuid guid, uint menuId, Packet packet)
        {
            if (menuId == 0)
                return;

            if (guid.GetObjectType() != ObjectType.Unit)
                return;

            bool isDefault = false;
            if (!Storage.CreatureDefaultGossips.ContainsKey(guid.GetEntry()))
            {
                isDefault = true;
                Storage.CreatureDefaultGossips.Add(guid.GetEntry(), (uint)menuId);
            }
            else if (Storage.CreatureDefaultGossips[guid.GetEntry()] == menuId)
                isDefault = true;
            else if (WowPacketParser.Parsing.Parsers.NpcHandler.CanBeDefaultGossipMenu)
                isDefault = true;

            CreatureGossip newGossip = new CreatureGossip
            {
                CreatureId = guid.GetEntry(),
                GossipMenuId = menuId,
                IsDefault = isDefault,
                SniffId = packet.SniffId,
            };
            Storage.CreatureGossips.Add(newGossip, packet.TimeSpan);
        }
        // Quest POI (QuestId, Id)
        public static readonly DataBag<QuestPOI> QuestPOIs = new DataBag<QuestPOI>(Settings.SqlTables.quest_poi_points);
        public static readonly DataBag<QuestPOIPoint> QuestPOIPoints = new DataBag<QuestPOIPoint>(Settings.SqlTables.quest_poi_points); // WoD

        // Quest Misc
        public static readonly DataBag<QuestStarter> QuestStarters = new DataBag<QuestStarter>(Settings.SqlTables.quest_starter);
        public static readonly DataBag<QuestEnder> QuestEnders = new DataBag<QuestEnder>(Settings.SqlTables.quest_ender);
        public static readonly DataBag<QuestClientAccept> QuestClientAcceptTimes = new DataBag<QuestClientAccept>(Settings.SqlTables.client_quest_accept);
        public static readonly DataBag<QuestClientComplete> QuestClientCompleteTimes = new DataBag<QuestClientComplete>(Settings.SqlTables.client_quest_complete);
        public static readonly DataBag<QuestCompleteTime> QuestCompleteTimes = new DataBag<QuestCompleteTime>(Settings.SqlTables.quest_update_complete);
        public static readonly DataBag<QuestFailTime> QuestFailTimes = new DataBag<QuestFailTime>(Settings.SqlTables.quest_update_failed);
        public static readonly DataBag<QuestGreeting> QuestGreetings = new DataBag<QuestGreeting>(Settings.SqlTables.quest_template);
        public static readonly DataBag<QuestDetails> QuestDetails = new DataBag<QuestDetails>(Settings.SqlTables.quest_template);
        public static readonly DataBag<QuestRequestItems> QuestRequestItems = new DataBag<QuestRequestItems>(Settings.SqlTables.quest_template);

        // Names
        public static readonly DataBag<ObjectName> ObjectNames = new DataBag<ObjectName>();

        // Vehicle Template Accessory
        public static readonly DataBag<VehicleTemplateAccessory> VehicleTemplateAccessories = new DataBag<VehicleTemplateAccessory>(Settings.SqlTables.vehicle_template_accessory);

        // Weather updates
        public static readonly DataBag<WeatherUpdate> WeatherUpdates = new DataBag<WeatherUpdate>(Settings.SqlTables.weather_updates);

        // XP updates
        public static readonly DataBag<XpGainAborted> XpGainAborted = new DataBag<XpGainAborted>(Settings.SqlTables.xp_gain_aborted);
        public static readonly DataBag<XpGainLog> XpGainLogs = new DataBag<XpGainLog>(Settings.SqlTables.xp_gain_log);

        // Reputation updates
        public static readonly DataBag<FactionStandingUpdate> FactionStandingUpdates = new DataBag<FactionStandingUpdate>(Settings.SqlTables.faction_standing_update);

        // Npc Spell Click
        public static readonly StoreBag<WowGuid> NpcSpellClicks = new StoreBag<WowGuid>(Settings.SqlTables.npc_spellclick_spells);
        public static readonly DataBag<NpcSpellClick> SpellClicks = new DataBag<NpcSpellClick>(Settings.SqlTables.npc_spellclick_spells);

        // Locales
        public static readonly DataBag<CreatureTemplateLocale> LocalesCreatures = new DataBag<CreatureTemplateLocale>(Settings.SqlTables.creature_template_locale);
        public static readonly DataBag<LocalesQuest> LocalesQuests = new DataBag<LocalesQuest>(Settings.SqlTables.locales_quest);
        public static readonly DataBag<QuestObjectivesLocale> LocalesQuestObjectives = new DataBag<QuestObjectivesLocale>(Settings.SqlTables.locales_quest_objectives);
        public static readonly DataBag<QuestOfferRewardLocale> LocalesQuestOfferRewards = new DataBag<QuestOfferRewardLocale>(Settings.SqlTables.locales_quest);
        public static readonly DataBag<QuestGreetingLocale> LocalesQuestGreeting = new DataBag<QuestGreetingLocale>(Settings.SqlTables.locales_quest);
        public static readonly DataBag<QuestRequestItemsLocale> LocalesQuestRequestItems = new DataBag<QuestRequestItemsLocale>(Settings.SqlTables.locales_quest);
        public static readonly DataBag<PageTextLocale> LocalesPageText = new DataBag<PageTextLocale>(Settings.SqlTables.page_text_locale);

        // Spell Casts
        public static readonly DataBag<PlaySpellVisualKit> SpellPlayVisualKit = new DataBag<PlaySpellVisualKit>(Settings.SqlTables.play_spell_visual_kit);
        public static readonly DataBag<SpellChannelStart> SpellChannelStart = new DataBag<SpellChannelStart>(Settings.SqlTables.spell_channel_start);
        public static readonly DataBag<SpellChannelUpdate> SpellChannelUpdate = new DataBag<SpellChannelUpdate>(Settings.SqlTables.spell_channel_update);
        public static readonly DataBag<SpellCastFailed> SpellCastFailed = new DataBag<SpellCastFailed>(Settings.SqlTables.spell_cast_failed);
        public static readonly DataBag<SpellCastData> SpellCastStart = new DataBag<SpellCastData>(Settings.SqlTables.spell_cast_start);
        public static readonly DataBag<SpellCastData> SpellCastGo = new DataBag<SpellCastData>(Settings.SqlTables.spell_cast_go);
        public static readonly DataBag<SpellUniqueCaster> SpellUniqueCasters = new DataBag<SpellUniqueCaster>(Settings.SqlTables.spell_unique_caster);
        public static readonly DataBag<CreatureSpellImmunity> CreatureSpellImmunity = new DataBag<CreatureSpellImmunity>(Settings.SqlTables.creature_spell_immunity);

        public static readonly Dictionary<uint /*creature*/, Dictionary<uint /*spell*/, List<double /*delay*/>>> CreatureInitialSpellTimers = new Dictionary<uint, Dictionary<uint, List<double>>>();
        private static void StoreCreatureInitialSpellTimer(uint creatureId, uint spellId, uint delay)
        {
            if (CreatureInitialSpellTimers.ContainsKey(creatureId))
            {
                if (CreatureInitialSpellTimers[creatureId].ContainsKey(spellId))
                {
                    CreatureInitialSpellTimers[creatureId][spellId].Add(delay);
                }
                else
                {
                    List<double> delayList = new List<double>();
                    delayList.Add(delay);
                    CreatureInitialSpellTimers[creatureId].Add(spellId, delayList);
                }
            }
            else
            {
                Dictionary<uint, List<double>> spellDict = new Dictionary<uint, List<double>>();
                List<double> delayList = new List<double>();
                delayList.Add(delay);
                spellDict.Add(spellId, delayList);
                CreatureInitialSpellTimers.Add(creatureId, spellDict);
            }
        }
        public static readonly Dictionary<uint /*creature*/, Dictionary<uint /*spell*/, List<double /*delay*/>>> CreatureRepeatSpellTimers = new Dictionary<uint, Dictionary<uint, List<double>>>();
        private static void StoreCreatureRepeatSpellTimer(uint creatureId, uint spellId, uint delay)
        {
            if (CreatureRepeatSpellTimers.ContainsKey(creatureId))
            {
                if (CreatureRepeatSpellTimers[creatureId].ContainsKey(spellId))
                {
                    CreatureRepeatSpellTimers[creatureId][spellId].Add(delay);
                }
                else
                {
                    List<double> delayList = new List<double>();
                    delayList.Add(delay);
                    CreatureRepeatSpellTimers[creatureId].Add(spellId, delayList);
                }
            }
            else
            {
                Dictionary<uint, List<double>> spellDict = new Dictionary<uint, List<double>>();
                List<double> delayList = new List<double>();
                delayList.Add(delay);
                spellDict.Add(spellId, delayList);
                CreatureRepeatSpellTimers.Add(creatureId, spellDict);
            }
        }
        public static void CalculateCreatureSpellTimer(SpellCastData castData, DateTime castTime)
        {
            if (!Settings.SqlTables.creature_spell_timers)
                return;

            if (!Storage.Objects.ContainsKey(castData.CasterGuid))
                return;

            Unit creature = Storage.Objects[castData.CasterGuid].Item1 as Unit;
            if (creature == null || creature.EnterCombatTime == null ||
                creature.EnterCombatTime > castTime || !creature.IsInCombat() ||
                creature.DontSaveCombatSpellTimers || creature.UnitData.Health == 0)
                return;

            // If creature was already in combat when we saw it, and it didn't just spawn,
            // we should not save it as initial, cause there could be casts we missed.
            bool isRepeatCast = creature.LastCreateTime == creature.EnterCombatTime &&
                                creature.LastCreateType != ObjectCreateType.Create2;

            // We lost sight of the creature at some point. Don't save as initial.
            if (creature.EnterCombatTime < creature.LastCreateTime)
                isRepeatCast = true;

            DateTime? lastCastTime = Storage.GetLastCastGoTimeForCreature(castData.CasterGuid, castData.SpellID);
            
            // No casts for the spell in the current combat session.
            if (lastCastTime != null && lastCastTime < creature.EnterCombatTime)
                lastCastTime = null;

            // We lost sight of the creature some time between last cast and now. Abort.
            if (lastCastTime != null && lastCastTime < creature.LastCreateTime)
                return;

            if (lastCastTime != null)
                isRepeatCast = true;

            uint delayMs = isRepeatCast ?
                           Utilities.GetTimeDiffInMs(lastCastTime, castTime) :
                           Utilities.GetTimeDiffInMs(creature.EnterCombatTime, castTime);

            if (isRepeatCast && delayMs == 0)
                return;

            if (isRepeatCast)
                Storage.StoreCreatureRepeatSpellTimer((uint)creature.ObjectData.EntryID, castData.SpellID, delayMs);
            else
                Storage.StoreCreatureInitialSpellTimer((uint)creature.ObjectData.EntryID, castData.SpellID, delayMs);
        }
        public static void StoreSpellCastData(SpellCastData castData, CastDataType type, Packet packet)
        {
            if (type == CastDataType.Go && castData.CasterGuid.GetHighType() == HighGuidType.Creature)
            {
                Storage.CalculateCreatureSpellTimer(castData, packet.Time);
                Storage.StoreCreatureCastGoTime(castData.CasterGuid, castData.SpellID, packet.Time);
            }

            if (Settings.SqlTables.creature_spell_immunity &&
                castData.MissTargetsCount == castData.MissReasonsCount &&
                castData.MissTargetsList != null && castData.MissReasonsList != null)
            {
                for (int i = 0; i < castData.MissTargetsCount; i++)
                {
                    WowGuid guid = castData.MissTargetsList[i];
                    uint reason = castData.MissReasonsList[i];
                    if (guid.GetHighType() == HighGuidType.Creature &&
                        reason == (uint)SpellMissType.Immune1)
                    {
                        CreatureSpellImmunity immunity = new CreatureSpellImmunity
                        {
                            Entry = Storage.GetCurrentObjectEntry(guid),
                            SpellID = castData.SpellID,
                            SniffId = packet.SniffId,
                        };
                        Storage.CreatureSpellImmunity.Add(immunity);
                    }
                }
            }
            
            if (Settings.SqlTables.spell_unique_caster &&
                (castData.CasterGuid.GetObjectType() == ObjectType.Unit ||
                castData.CasterGuid.GetObjectType() == ObjectType.GameObject))
            {
                SpellUniqueCaster uniqueCast = new SpellUniqueCaster();
                uniqueCast.SpellId = castData.SpellID;
                uniqueCast.CasterId = castData.CasterGuid.GetEntry();
                uniqueCast.CasterType = GetObjectTypeNameForDB(castData.CasterGuid);
                uniqueCast.SniffId = packet.SniffId;
                SpellUniqueCasters.Add(uniqueCast);
            }

            if (!Settings.SqlTables.spell_cast_start &&
                !Settings.SqlTables.spell_cast_go)
                return;

            if (!Settings.SavePlayerCasts && castData.CasterGuid.GetObjectType() == ObjectType.Player)
                return;

            castData.Time = packet.Time;
            DataBag<SpellCastData> storage = type == CastDataType.Start ? Storage.SpellCastStart : Storage.SpellCastGo;
            storage.Add(castData, packet.TimeSpan);
        }
        public static readonly DataBag<CreaturePetCooldown> CreaturePetCooldown = new DataBag<CreaturePetCooldown>(Settings.SqlTables.creature_pet_cooldown);
        public static readonly DataBag<CreaturePetRemainingCooldown> CreaturePetRemainingCooldown = new DataBag<CreaturePetRemainingCooldown>(Settings.SqlTables.creature_pet_remaining_cooldown);
        public static readonly DataBag<CreaturePetActions> CreaturePetActions = new DataBag<CreaturePetActions>(Settings.SqlTables.creature_pet_actions);
        public static readonly DataBag<SpellTargetPosition> SpellTargetPositions = new DataBag<SpellTargetPosition>(Settings.SqlTables.spell_target_position);
        public static void StoreSpellTargetPosition(uint spellID, int mapID, Vector3 dstPosition, float orientation)
        {
            if (!Settings.SqlTables.spell_target_position)
                return;

            if (Settings.UseDBC)
            {
                for (uint i = 0; i < 32; i++)
                {
                    var tuple = Tuple.Create(spellID, i);
                    if (DBC.DBC.SpellEffectStores.ContainsKey(tuple))
                    {
                        var effect = DBC.DBC.SpellEffectStores[tuple];
                        if ((Targets)effect.ImplicitTarget[0] == Targets.TARGET_DEST_DB || (Targets)effect.ImplicitTarget[1] == Targets.TARGET_DEST_DB)
                        {
                            string effectHelper = $"Spell: { StoreGetters.GetName(StoreNameType.Spell, (int)spellID) } Efffect: { effect.Effect } ({ (SpellEffects)effect.Effect })";

                            var spellTargetPosition = new SpellTargetPosition
                            {
                                ID = spellID,
                                EffectIndex = (byte)i,
                                PositionX = dstPosition.X,
                                PositionY = dstPosition.Y,
                                PositionZ = dstPosition.Z,
                                MapID = (ushort)mapID,
                                EffectHelper = effectHelper
                            };

                            if (!Storage.SpellTargetPositions.ContainsKey(spellTargetPosition))
                                Storage.SpellTargetPositions.Add(spellTargetPosition);
                        }
                    }
                }
            }
            else if (HardcodedData.DbCoordinateSpells.Contains(spellID))
            {
                string effectHelper = $"Spell: { StoreGetters.GetName(StoreNameType.Spell, (int)spellID) } Efffect: { 0 } ({ (SpellEffects)0 })";

                var spellTargetPosition = new SpellTargetPosition
                {
                    ID = spellID,
                    EffectIndex = (byte)0,
                    PositionX = dstPosition.X,
                    PositionY = dstPosition.Y,
                    PositionZ = dstPosition.Z,
                    Orientation = orientation,
                    MapID = (ushort)mapID,
                    EffectHelper = effectHelper
                };

                if (!Storage.SpellTargetPositions.ContainsKey(spellTargetPosition))
                    Storage.SpellTargetPositions.Add(spellTargetPosition);
            }
        }
        public static readonly DataBag<SpellScriptTarget> SpellScriptTargets = new DataBag<SpellScriptTarget>(Settings.SqlTables.spell_script_target);

        public static void StoreSpellScriptTarget(uint spellId, WowGuid hitTarget)
        {
            if (Settings.SqlTables.spell_script_target &&
                HardcodedData.DbTargetSpells.Contains(spellId))
            {
                ObjectType targetType = hitTarget.GetObjectType();
                uint targetDbType = 255;

                if (Settings.TargetedDbType != TargetedDbType.WPP)
                {
                    switch (targetType)
                    {
                        case ObjectType.GameObject:
                            targetDbType = 0;
                            break;
                        case ObjectType.Unit:
                            targetDbType = 1;
                            break;
                        default:
                            return;
                    }
                }

                SpellScriptTarget targetData = new SpellScriptTarget();
                targetData.SpellId = spellId;
                targetData.Type = targetDbType;
                targetData.TargetId = Storage.GetObjectEntry(hitTarget);
                targetData.TargetType = GetObjectTypeNameForDB(hitTarget);
                Storage.SpellScriptTargets.Add(targetData);
            }
        }

        // World state
        public static readonly DataBag<WorldStateInit> WorldStateInits = new DataBag<WorldStateInit>(Settings.SqlTables.world_state_init);
        public static readonly DataBag<WorldStateUpdate> WorldStateUpdates = new DataBag<WorldStateUpdate>(Settings.SqlTables.world_state_update);

        public static readonly DataBag<HotfixData> HotfixDatas = new DataBag<HotfixData>(Settings.SqlTables.hotfix_data);
        public static readonly DataBag<HotfixBlob> HotfixBlobs = new DataBag<HotfixBlob>(Settings.SqlTables.hotfix_blob);
        public static readonly DataBag<HotfixOptionalData> HotfixOptionalDatas = new DataBag<HotfixOptionalData>(new List<SQLOutput> { });

        // Scenes
        public static readonly DataBag<SceneTemplate> Scenes = new DataBag<SceneTemplate>(Settings.SqlTables.scene_template);

        // Scenario
        public static readonly DataBag<ScenarioPOI> ScenarioPOIs = new DataBag<ScenarioPOI>(Settings.SqlTables.scenario_poi);
        public static readonly DataBag<ScenarioPOIPoint> ScenarioPOIPoints = new DataBag<ScenarioPOIPoint>(Settings.SqlTables.scenario_poi);

        public static readonly DataBag<BroadcastText> BroadcastTexts = new DataBag<BroadcastText>(Settings.SqlTables.broadcast_text);
        public static readonly DataBag<BroadcastTextLocale> BroadcastTextLocales = new DataBag<BroadcastTextLocale>(Settings.SqlTables.broadcast_text_locale);

        //Player Choice
        public static readonly DataBag<PlayerChoiceTemplate> PlayerChoices = new DataBag<PlayerChoiceTemplate>(Settings.SqlTables.playerchoice);
        public static readonly DataBag<PlayerChoiceLocaleTemplate> PlayerChoiceLocales = new DataBag<PlayerChoiceLocaleTemplate>(Settings.SqlTables.playerchoice);
        public static readonly DataBag<PlayerChoiceResponseTemplate> PlayerChoiceResponses = new DataBag<PlayerChoiceResponseTemplate>(Settings.SqlTables.playerchoice);
        public static readonly DataBag<PlayerChoiceResponseLocaleTemplate> PlayerChoiceResponseLocales = new DataBag<PlayerChoiceResponseLocaleTemplate>(Settings.SqlTables.playerchoice);
        public static readonly DataBag<PlayerChoiceResponseRewardTemplate> PlayerChoiceResponseRewards = new DataBag<PlayerChoiceResponseRewardTemplate>(Settings.SqlTables.playerchoice);
        public static readonly DataBag<PlayerChoiceResponseRewardCurrencyTemplate> PlayerChoiceResponseRewardCurrencies = new DataBag<PlayerChoiceResponseRewardCurrencyTemplate>(Settings.SqlTables.playerchoice);
        public static readonly DataBag<PlayerChoiceResponseRewardFactionTemplate> PlayerChoiceResponseRewardFactions = new DataBag<PlayerChoiceResponseRewardFactionTemplate>(Settings.SqlTables.playerchoice);
        public static readonly DataBag<PlayerChoiceResponseRewardItemTemplate> PlayerChoiceResponseRewardItems = new DataBag<PlayerChoiceResponseRewardItemTemplate>(Settings.SqlTables.playerchoice);

        // Client Actions
        public static readonly DataBag<ClientAreatriggerEnter> ClientAreatriggerEnterTimes = new DataBag<ClientAreatriggerEnter>(Settings.SqlTables.client_areatrigger_enter);
        public static readonly DataBag<ClientAreatriggerLeave> ClientAreatriggerLeaveTimes = new DataBag<ClientAreatriggerLeave>(Settings.SqlTables.client_areatrigger_leave);
        public static void StoreClientAreatriggerTime(uint areatriggerId, bool entered, DateTime time)
        {
            if (entered)
            {
                ClientAreatriggerEnter trigger = new ClientAreatriggerEnter();
                trigger.AreatriggerId = areatriggerId;
                trigger.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(time);
                ClientAreatriggerEnterTimes.Add(trigger);
            }
            else
            {
                ClientAreatriggerLeave trigger = new ClientAreatriggerLeave();
                trigger.AreatriggerId = areatriggerId;
                trigger.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(time);
                ClientAreatriggerLeaveTimes.Add(trigger);
            }
        }
        public static readonly DataBag<ClientReclaimCorpse> ClientReclaimCorpseTimes = new DataBag<ClientReclaimCorpse>(Settings.SqlTables.client_reclaim_corpse);
        public static readonly DataBag<ClientReleaseSpirit> ClientReleaseSpiritTimes = new DataBag<ClientReleaseSpirit>(Settings.SqlTables.client_release_spirit);

        // Logout Time
        public static readonly DataBag<LogoutTime> LogoutTimes = new DataBag<LogoutTime>(Settings.SqlTables.logout_time);

        // Cinematic
        public static readonly DataBag<CinematicBegin> CinematicBeginTimes = new DataBag<CinematicBegin>(Settings.SqlTables.cinematic_begin);
        public static readonly DataBag<CinematicEnd> CinematicEndTimes = new DataBag<CinematicEnd>(Settings.SqlTables.cinematic_end);

        // Guild
        public static readonly DataBag<GuildTemplate> Guild = new DataBag<GuildTemplate>(Settings.SqlTables.guild);
        public static readonly DataBag<GuildRankTemplate> GuildRank = new DataBag<GuildRankTemplate>(Settings.SqlTables.guild_rank);

        // Mail
        public static readonly DataBag<MailTemplate> MailTemplates = new DataBag<MailTemplate>(Settings.SqlTables.mail_template);
        public static readonly DataBag<MailTemplateItem> MailTemplateItems = new DataBag<MailTemplateItem>(Settings.SqlTables.mail_template);
        public static void StoreMailTemplate(MailTemplate mailTemplate)
        {
            foreach (var existingMail in MailTemplates)
            {
                if (existingMail.Item1.Entry == mailTemplate.Entry &&
                    existingMail.Item1.Money >= mailTemplate.Money &&
                    existingMail.Item1.ItemsCount >= mailTemplate.ItemsCount)
                    return;
            }

            MailTemplates.Add(mailTemplate);
        }

        // Called every time processing a sniff file finishes,
        // and a new one is about to be loaded and parsed.
        public static void ClearTemporaryData()
        {
            CurrentActivePlayer = WowGuid64.Empty;
            ClearDataOnMapChange();
        }

        // Called from SMSG_NEW_WORLD
        public static void ClearDataOnMapChange()
        {
            CurrentTaxiNode = 0;
            LastCreatureCastGo.Clear();
            CreatureDeathTimes.Clear();
            LastCreatureKill = null;
            WowPacketParser.Parsing.Parsers.NpcHandler.CanBeDefaultGossipMenu = true;
        }

        // Only called if not in multi sniff sql mode.
        public static void ClearContainers()
        {
            ClearTemporaryData();

            SniffData.Clear();

            Objects.Clear();
            ObjectDestroyTimes.Clear();
            ObjectCreate1Times.Clear();
            ObjectCreate2Times.Clear();

            AreaTriggerTemplates.Clear();
            SpellAreaTriggerSplines.Clear();
            SpellAreaTriggerVertices.Clear();

            ConversationActors.Clear();
            ConversationActorTemplates.Clear();
            ConversationLineTemplates.Clear();
            ConversationTemplates.Clear();

            CharacterSpells.Clear();
            CharacterReputations.Clear();

            PlayerMovements.Clear();
            PlayerActiveCreateTime.Clear();

            GameObjectClientUseTimes.Clear();
            GameObjectCustomAnims.Clear();
            GameObjectDespawnAnims.Clear();
            GameObjectLoot.Clear();
            GameObjectTemplates.Clear();
            GameObjectTemplateQuestItems.Clear();
            GameObjectUpdates.Clear();

            ItemClientUseTimes.Clear();
            ItemTemplates.Clear();

            QuestTemplates.Clear();
            QuestObjectives.Clear();
            QuestVisualEffects.Clear();
            QuestRewardDisplaySpells.Clear();

            UnitAttackLogs.Clear();
            UnitAttackStartTimes.Clear();
            UnitAttackStopTimes.Clear();
            CreatureClientInteractTimes.Clear();
            CreatureLoot.Clear();
            CreatureStats.Clear();
            CreatureStatsDirty.Clear();

            CreatureKillReputations.Clear();
            CreatureRespawnTimes.Clear();
            CreatureMeleeDamageTaken.Clear();
            CreatureMeleeAttackDamage.Clear();
            CreatureMeleeAttackDamageDirty.Clear();
            CreatureMeleeAttackSchool.Clear();
            CreatureTemplates.Clear();
            CreatureTemplatesNonWDB.Clear();
            CreatureTemplateQuestItems.Clear();
            CreatureTemplateScalings.Clear();
            CreatureTemplateModels.Clear();
            CreatureInitialSpellTimers.Clear();
            CreatureRepeatSpellTimers.Clear();
            CreatureThreatUpdates.Clear();
            CreatureThreatClears.Clear();
            CreatureThreatRemoves.Clear();
            UnitAurasUpdates.Clear();
            UnitEquipmentValuesUpdates.Clear();
            UnitGuidValuesUpdates.Clear();
            UnitValuesUpdates.Clear();
            UnitPowerValuesUpdates.Clear();
            UnitSpeedUpdates.Clear();

            NpcTrainers.Clear();
            NpcVendors.Clear();
            Trainers.Clear();
            TrainerSpells.Clear();
            CreatureTrainers.Clear();

            PageTexts.Clear();
            NpcTexts.Clear();
            NpcTextsMop.Clear();

            WorldTexts.Clear();
            CreatureTexts.Clear();
            CreatureTextTemplates.Clear();
            CharacterTexts.Clear();

            GossipPOIs.Clear();

            Emotes.Clear();
            Music.Clear();
            Sounds.Clear();
            SpellsX.Clear();
            QuestOfferRewards.Clear();
            GossipSelects.Clear();

            StartActions.Clear();
            StartPositions.Clear();
            PlayerClassLevelStats.Clear();
            PlayerLevelStats.Clear();
            PlayerLevelupInfos.Clear();
            PlayerCritChances.Clear();
            PlayerDodgeChances.Clear();

            CreatureDefaultGossips.Clear();
            CreatureGossips.Clear();
            Gossips.Clear();
            GossipMenuOptions.Clear();
            GossipMenuOptionActions.Clear();
            GossipMenuOptionBoxes.Clear();

            QuestPOIs.Clear();
            QuestPOIPoints.Clear();

            QuestStarters.Clear();
            QuestEnders.Clear();
            QuestClientAcceptTimes.Clear();
            QuestClientCompleteTimes.Clear();
            QuestCompleteTimes.Clear();
            QuestFailTimes.Clear();
            QuestGreetings.Clear();
            QuestDetails.Clear();
            QuestRequestItems.Clear();

            ObjectNames.Clear();

            VehicleTemplateAccessories.Clear();

            WeatherUpdates.Clear();

            XpGainAborted.Clear();
            XpGainLogs.Clear();
            FactionStandingUpdates.Clear();

            NpcSpellClicks.Clear();
            SpellClicks.Clear();

            SpellPlayVisualKit.Clear();
            SpellChannelStart.Clear();
            SpellChannelUpdate.Clear();
            SpellCastFailed.Clear();
            SpellCastStart.Clear();
            SpellCastGo.Clear();
            SpellUniqueCasters.Clear();
            CreaturePetActions.Clear();
            CreaturePetCooldown.Clear();
            SpellTargetPositions.Clear();
            SpellScriptTargets.Clear();

            LocalesCreatures.Clear();
            LocalesQuests.Clear();
            LocalesQuestObjectives.Clear();
            LocalesQuestOfferRewards.Clear();
            LocalesQuestGreeting.Clear();
            LocalesQuestRequestItems.Clear();
            LocalesPageText.Clear();

            WorldStateInits.Clear();
            WorldStateUpdates.Clear();

            HotfixDatas.Clear();
            HotfixBlobs.Clear();
            HotfixOptionalDatas.Clear();

            Scenes.Clear();

            ScenarioPOIs.Clear();
            ScenarioPOIPoints.Clear();

            BroadcastTexts.Clear();
            BroadcastTextLocales.Clear();

            PlayerChoices.Clear();
            PlayerChoiceLocales.Clear();
            PlayerChoiceResponses.Clear();
            PlayerChoiceResponseLocales.Clear();
            PlayerChoiceResponseRewards.Clear();
            PlayerChoiceResponseRewardCurrencies.Clear();
            PlayerChoiceResponseRewardFactions.Clear();
            PlayerChoiceResponseRewardItems.Clear();

            ClientAreatriggerEnterTimes.Clear();
            ClientAreatriggerLeaveTimes.Clear();
            ClientReclaimCorpseTimes.Clear();
            ClientReleaseSpiritTimes.Clear();
            LogoutTimes.Clear();
            CinematicBeginTimes.Clear();
            CinematicEndTimes.Clear();

            Guild.Clear();
            GuildRank.Clear();

            MailTemplates.Clear();
            MailTemplateItems.Clear();
        }
    }
}
