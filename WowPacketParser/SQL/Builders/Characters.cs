﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WowPacketParser.Enums;
using WowPacketParser.Enums.Version;
using WowPacketParser.Misc;
using WowPacketParser.Store;
using WowPacketParser.Store.Objects;

namespace WowPacketParser.SQL.Builders
{
    [BuilderClass]
    public static class Characters
    {
        private static Random random = new Random();
        public static string GetRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string randomString = new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
            return randomString.Substring(0, 1) + randomString.Substring(1).ToLower();
        }

        [BuilderMethod]
        public static string CharactersBuilder()
        {
            if (!Settings.SqlTables.characters && !Settings.SqlTables.player)
                return string.Empty;

            StringBuilder result = new StringBuilder();
            uint maxDbGuid = 0;
            uint itemGuidCounter = 0;
            var characterRows = new RowList<CharacterTemplate>();
            var characterInventoryRows = new RowList<CharacterInventory>();
            var characterItemInstaceRows = new RowList<CharacterItemInstance>();
            var guildMemberRows = new RowList<GuildMember>();
            var playerRows = new RowList<PlayerTemplate>();
            var playerGuidValuesRows = new RowList<CreatureGuidValues>();
            var playerAttackLogRows = new RowList<UnitMeleeAttackLog>();
            var playerAttackStartRows = new RowList<CreatureAttackToggle>();
            var playerAttackStopRows = new RowList<CreatureAttackToggle>();
            var playerAurasUpdateRows = new RowList<CreatureAurasUpdate>();
            var playerCreate1Rows = new RowList<PlayerCreate1>();
            var playerCreate2Rows = new RowList<PlayerCreate2>();
            var playerDestroyRows = new RowList<PlayerDestroy>();
            var playerEmoteRows = new RowList<CreatureEmote>();
            var playerEquipmentValuesUpdateRows = new RowList<CreatureEquipmentValuesUpdate>();
            var playerGuidValuesUpdateRows = new RowList<CreatureGuidValuesUpdate>();
            var playerValuesUpdateRows = new RowList<CreatureValuesUpdate>();
            var playerSpeedUpdateRows = new RowList<CreatureSpeedUpdate>();
            var playerServerMovementRows = new RowList<ServerSideMovement>();
            var playerServerMovementSplineRows = new RowList<ServerSideMovementSpline>();
            Dictionary<WowGuid, uint> accountIdDictionary = new Dictionary<WowGuid, uint>();
            foreach (var objPair in Storage.Objects)
            {
                if (objPair.Key.GetObjectType() != ObjectType.Player)
                    continue;

                Player player = objPair.Value.Item1 as Player;
                if (player == null)
                    continue;

                if (!player.IsActivePlayer && Settings.SkipOtherPlayers)
                    continue;

                Row<CharacterTemplate> row = new Row<CharacterTemplate>();

                row.Data.Guid = "@PGUID+" + player.DbGuid;
                if (accountIdDictionary.ContainsKey(player.PlayerDataOriginal.WowAccount))
                    row.Data.Account = "@ACCID+" + accountIdDictionary[player.PlayerDataOriginal.WowAccount];
                else
                {
                    uint id = (uint)accountIdDictionary.Count;
                    accountIdDictionary.Add(player.PlayerDataOriginal.WowAccount, id);
                    row.Data.Account = "@ACCID+" + id;
                }

                row.Data.Name = Settings.RandomizePlayerNames ? GetRandomString(8) : StoreGetters.GetName(objPair.Key);
                row.Data.Race = player.UnitDataOriginal.RaceId;
                row.Data.Class = player.UnitDataOriginal.ClassId;
                row.Data.Gender = player.UnitDataOriginal.Sex;
                row.Data.Level = (uint)player.UnitDataOriginal.Level;
                row.Data.XP = player.PlayerDataOriginal.Experience;
                row.Data.Money = player.PlayerDataOriginal.Money;
                row.Data.PlayerBytes = player.PlayerDataOriginal.PlayerBytes1;
                row.Data.PlayerBytes2 = player.PlayerDataOriginal.PlayerBytes2;
                row.Data.PlayerFlags = player.PlayerDataOriginal.PlayerFlags;
                MovementInfo moveData = player.OriginalMovement == null ? player.Movement : player.OriginalMovement;
                if (moveData != null)
                {
                    row.Data.PositionX = moveData.Position.X;
                    row.Data.PositionY = moveData.Position.Y;
                    row.Data.PositionZ = moveData.Position.Z;
                    row.Data.Orientation = moveData.Orientation;
                }
                row.Data.Map = player.Map;
                row.Data.Health = (uint)player.UnitDataOriginal.MaxHealth;
                row.Data.Power1 = (uint)player.UnitDataOriginal.MaxMana;

                Store.Objects.UpdateFields.IVisibleItem[] visibleItems = player.PlayerDataOriginal.VisibleItems;

                for (int i = 0; i < 19; i++)
                {
                    int itemId = visibleItems[i].ItemID;
                    ushort enchantId = visibleItems[i].ItemVisual;

                    Row<CharacterInventory> inventoryRow = new Row<CharacterInventory>();
                    inventoryRow.Data.Guid = row.Data.Guid;
                    inventoryRow.Data.Bag = 0;
                    inventoryRow.Data.Slot = (uint)i / 2;
                    inventoryRow.Data.ItemGuid = "@IGUID+" + itemGuidCounter;
                    inventoryRow.Data.ItemTemplate = (uint)itemId;
                    characterInventoryRows.Add(inventoryRow);

                    Row<CharacterItemInstance> itemInstanceRow = new Row<CharacterItemInstance>();
                    itemInstanceRow.Data.Guid = "@IGUID+" + itemGuidCounter;
                    itemInstanceRow.Data.ItemEntry = (uint)itemId;
                    itemInstanceRow.Data.OwnerGuid = row.Data.Guid;
                    characterItemInstaceRows.Add(itemInstanceRow);

                    itemGuidCounter++;

                    if (row.Data.EquipmentCache.Length > 0)
                        row.Data.EquipmentCache += " ";

                    row.Data.EquipmentCache += itemId + " " + enchantId;
                }

                characterRows.Add(row);

                if (maxDbGuid < player.DbGuid)
                    maxDbGuid = player.DbGuid;

                // Character wasn't actually seen in game, so there is no replay data.
                // Object was constructed from characters enum packet (before enter world).
                if (moveData == null)
                    continue;

                if (Settings.SqlTables.player)
                {
                    Row<PlayerTemplate> playerRow = new Row<PlayerTemplate>();
                    playerRow.Data.Guid = row.Data.Guid;
                    playerRow.Data.Name = row.Data.Name;
                    playerRow.Data.Race = row.Data.Race;
                    playerRow.Data.Class = row.Data.Class;
                    playerRow.Data.Gender = row.Data.Gender;
                    playerRow.Data.Level = row.Data.Level;
                    playerRow.Data.XP = row.Data.XP;
                    playerRow.Data.Money = row.Data.Money;
                    playerRow.Data.PlayerBytes = row.Data.PlayerBytes;
                    playerRow.Data.PlayerBytes2 = row.Data.PlayerBytes2;
                    playerRow.Data.PlayerFlags = row.Data.PlayerFlags;
                    playerRow.Data.PositionX = row.Data.PositionX;
                    playerRow.Data.PositionY = row.Data.PositionY;
                    playerRow.Data.PositionZ = row.Data.PositionZ;
                    playerRow.Data.Orientation = row.Data.Orientation;
                    playerRow.Data.Map = row.Data.Map;
                    playerRow.Data.DisplayID = (uint)player.UnitDataOriginal.DisplayID;
                    playerRow.Data.NativeDisplayID = (uint)player.UnitDataOriginal.NativeDisplayID;
                    playerRow.Data.MountDisplayID = (uint)player.UnitDataOriginal.MountDisplayID;
                    playerRow.Data.FactionTemplate = (uint)player.UnitDataOriginal.FactionTemplate;
                    playerRow.Data.UnitFlags = player.UnitDataOriginal.Flags;
                    playerRow.Data.UnitFlags2 = player.UnitDataOriginal.Flags2;
                    playerRow.Data.CurHealth = (uint)player.UnitDataOriginal.CurHealth;
                    playerRow.Data.MaxHealth = (uint)player.UnitDataOriginal.MaxHealth;
                    playerRow.Data.CurMana = (uint)player.UnitDataOriginal.CurMana;
                    playerRow.Data.MaxMana = (uint)player.UnitDataOriginal.MaxMana;
                    playerRow.Data.AuraState = player.UnitDataOriginal.AuraState;
                    playerRow.Data.EmoteState = (uint)player.UnitDataOriginal.EmoteState;
                    playerRow.Data.StandState = player.UnitDataOriginal.StandState;
                    playerRow.Data.PetTalentPoints = player.UnitDataOriginal.PetTalentPoints;
                    playerRow.Data.VisFlags = player.UnitDataOriginal.VisFlags;
                    playerRow.Data.AnimTier = player.UnitDataOriginal.AnimTier;
                    playerRow.Data.SheatheState = player.UnitDataOriginal.SheatheState;
                    playerRow.Data.PvpFlags = player.UnitDataOriginal.PvpFlags;
                    playerRow.Data.PetFlags = player.UnitDataOriginal.PetFlags;
                    playerRow.Data.ShapeshiftForm = player.UnitDataOriginal.ShapeshiftForm;
                    playerRow.Data.SpeedWalk = moveData.WalkSpeed / MovementInfo.DEFAULT_WALK_SPEED;
                    playerRow.Data.SpeedRun = moveData.RunSpeed / MovementInfo.DEFAULT_RUN_SPEED;
                    playerRow.Data.SpeedRunBack = moveData.RunBackSpeed / MovementInfo.DEFAULT_RUN_BACK_SPEED;
                    playerRow.Data.SpeedSwim = moveData.SwimSpeed / MovementInfo.DEFAULT_SWIM_SPEED;
                    playerRow.Data.SpeedSwimBack = moveData.SwimBackSpeed / MovementInfo.DEFAULT_SWIM_BACK_SPEED;
                    playerRow.Data.SpeedFly = moveData.FlightSpeed / MovementInfo.DEFAULT_FLY_SPEED;
                    playerRow.Data.SpeedFlyBack = moveData.FlightBackSpeed / MovementInfo.DEFAULT_FLY_BACK_SPEED;
                    playerRow.Data.TurnRate = moveData.TurnRate / MovementInfo.DEFAULT_TURN_RATE;
                    playerRow.Data.PitchRate = moveData.PitchRate / MovementInfo.DEFAULT_PITCH_RATE;
                    playerRow.Data.Scale = player.ObjectDataOriginal.Scale;
                    playerRow.Data.BoundingRadius = player.UnitDataOriginal.BoundingRadius;
                    playerRow.Data.CombatReach = player.UnitDataOriginal.CombatReach;
                    playerRow.Data.ModMeleeHaste = player.UnitDataOriginal.ModHaste;
                    playerRow.Data.ModRangedHaste = player.UnitDataOriginal.ModRangedHaste;
                    playerRow.Data.MainHandAttackTime = player.UnitDataOriginal.AttackRoundBaseTime[0];
                    playerRow.Data.OffHandAttackTime = player.UnitDataOriginal.AttackRoundBaseTime[1];
                    playerRow.Data.RangedAttackTime = player.UnitDataOriginal.RangedAttackRoundBaseTime;
                    playerRow.Data.Auras = player.GetAurasString(false);
                    playerRow.Data.EquipmentCache = row.Data.EquipmentCache;
                    playerRows.Add(playerRow);
                }

                if (Settings.SqlTables.player_guid_values)
                {
                    if (!player.UnitDataOriginal.Charm.IsEmpty() ||
                        !player.UnitDataOriginal.Summon.IsEmpty() ||
                        !player.UnitDataOriginal.CharmedBy.IsEmpty() ||
                        !player.UnitDataOriginal.CreatedBy.IsEmpty() ||
                        !player.UnitDataOriginal.SummonedBy.IsEmpty() ||
                        !player.UnitDataOriginal.Target.IsEmpty())
                    {
                        Row<CreatureGuidValues> guidsRow = new Row<CreatureGuidValues>();
                        guidsRow.Data.GUID = row.Data.Guid;
                        Storage.GetObjectDbGuidEntryType(player.UnitDataOriginal.Charm, out guidsRow.Data.CharmGuid, out guidsRow.Data.CharmId, out guidsRow.Data.CharmType);
                        Storage.GetObjectDbGuidEntryType(player.UnitDataOriginal.Summon, out guidsRow.Data.SummonGuid, out guidsRow.Data.SummonId, out guidsRow.Data.SummonType);
                        Storage.GetObjectDbGuidEntryType(player.UnitDataOriginal.CharmedBy, out guidsRow.Data.CharmedByGuid, out guidsRow.Data.CharmedById, out guidsRow.Data.CharmedByType);
                        Storage.GetObjectDbGuidEntryType(player.UnitDataOriginal.CreatedBy, out guidsRow.Data.CreatedByGuid, out guidsRow.Data.CreatedById, out guidsRow.Data.CreatedByType);
                        Storage.GetObjectDbGuidEntryType(player.UnitDataOriginal.SummonedBy, out guidsRow.Data.SummonedByGuid, out guidsRow.Data.SummonedById, out guidsRow.Data.SummonedByType);
                        Storage.GetObjectDbGuidEntryType(player.UnitDataOriginal.Target, out guidsRow.Data.TargetGuid, out guidsRow.Data.TargetId, out guidsRow.Data.TargetType);
                        playerGuidValuesRows.Add(guidsRow);
                    }
                }

                if (Settings.SqlTables.player_attack_log)
                {
                    if (Storage.UnitAttackLogs.ContainsKey(objPair.Key))
                    {
                        foreach (var attack in Storage.UnitAttackLogs[objPair.Key])
                        {
                            Row<UnitMeleeAttackLog> attackRow = new Row<UnitMeleeAttackLog>();
                            attackRow.Data = attack;
                            attackRow.Data.GUID = row.Data.Guid;
                            Storage.GetObjectDbGuidEntryType(attack.Victim, out attackRow.Data.VictimGuid, out attackRow.Data.VictimId, out attackRow.Data.VictimType);
                            attackRow.Data.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(attack.Time);
                            playerAttackLogRows.Add(attackRow);
                        }
                    }
                }

                if (Settings.SqlTables.player_attack_start)
                {
                    if (Storage.UnitAttackStartTimes.ContainsKey(objPair.Key))
                    {
                        foreach (var attack in Storage.UnitAttackStartTimes[objPair.Key])
                        {
                            Row<CreatureAttackToggle> attackRow = new Row<CreatureAttackToggle>();
                            attackRow.Data.GUID = row.Data.Guid;
                            Storage.GetObjectDbGuidEntryType(attack.victim, out attackRow.Data.VictimGuid, out attackRow.Data.VictimId, out attackRow.Data.VictimType);
                            attackRow.Data.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(attack.time);
                            playerAttackStartRows.Add(attackRow);
                        }
                    }
                }

                if (Settings.SqlTables.player_attack_stop)
                {
                    if (Storage.UnitAttackStopTimes.ContainsKey(objPair.Key))
                    {
                        foreach (var attack in Storage.UnitAttackStopTimes[objPair.Key])
                        {
                            Row<CreatureAttackToggle> attackRow = new Row<CreatureAttackToggle>();
                            attackRow.Data.GUID = row.Data.Guid;
                            Storage.GetObjectDbGuidEntryType(attack.victim, out attackRow.Data.VictimGuid, out attackRow.Data.VictimId, out attackRow.Data.VictimType);
                            attackRow.Data.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(attack.time);
                            playerAttackStopRows.Add(attackRow);
                        }
                    }
                }

                if (Settings.SqlTables.player_create1_time)
                {
                    if (Storage.ObjectCreate1Times.ContainsKey(objPair.Key))
                    {
                        foreach (var createTime in Storage.ObjectCreate1Times[objPair.Key])
                        {
                            var create1Row = new Row<PlayerCreate1>();
                            create1Row.Data.GUID = row.Data.Guid;
                            create1Row.Data.PositionX = createTime.PositionX;
                            create1Row.Data.PositionY = createTime.PositionY;
                            create1Row.Data.PositionZ = createTime.PositionZ;
                            create1Row.Data.Orientation = createTime.Orientation;
                            create1Row.Data.UnixTimeMs = createTime.UnixTimeMs;
                            playerCreate1Rows.Add(create1Row);
                        }
                    }
                }

                if (Settings.SqlTables.player_create2_time)
                {
                    if (Storage.ObjectCreate2Times.ContainsKey(objPair.Key))
                    {
                        foreach (var createTime in Storage.ObjectCreate2Times[objPair.Key])
                        {
                            var create2Row = new Row<PlayerCreate2>();
                            create2Row.Data.GUID = row.Data.Guid;
                            create2Row.Data.PositionX = createTime.PositionX;
                            create2Row.Data.PositionY = createTime.PositionY;
                            create2Row.Data.PositionZ = createTime.PositionZ;
                            create2Row.Data.Orientation = createTime.Orientation;
                            create2Row.Data.UnixTimeMs = createTime.UnixTimeMs;
                            playerCreate2Rows.Add(create2Row);
                        }
                    }
                }

                if (Settings.SqlTables.player_destroy_time)
                {
                    if (Storage.ObjectDestroyTimes.ContainsKey(objPair.Key))
                    {
                        foreach (var createTime in Storage.ObjectDestroyTimes[objPair.Key])
                        {
                            var destroyRow = new Row<PlayerDestroy>();
                            destroyRow.Data.GUID = row.Data.Guid;
                            destroyRow.Data.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(createTime);
                            playerDestroyRows.Add(destroyRow);
                        }
                    }
                }

                if (Settings.SqlTables.player_emote)
                {
                    if (Storage.Emotes.ContainsKey(objPair.Key))
                    {
                        foreach (var emote in Storage.Emotes[objPair.Key])
                        {
                            var emoteRow = new Row<CreatureEmote>();
                            emoteRow.Data = emote;
                            emoteRow.Data.GUID = row.Data.Guid;
                            playerEmoteRows.Add(emoteRow);
                        }
                    }
                }

                if (Settings.SqlTables.player_equipment_values_update)
                {
                    if (Storage.UnitEquipmentValuesUpdates.ContainsKey(objPair.Key))
                    {
                        foreach (var update in Storage.UnitEquipmentValuesUpdates[objPair.Key])
                        {
                            Row<CreatureEquipmentValuesUpdate> updateRow = new Row<CreatureEquipmentValuesUpdate>();
                            updateRow.Data = update;
                            updateRow.Data.GUID = row.Data.Guid;
                            updateRow.Data.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(update.time);
                            playerEquipmentValuesUpdateRows.Add(updateRow);
                        }
                    }
                }

                if (Settings.SqlTables.player_guid_values_update)
                {
                    if (Storage.UnitGuidValuesUpdates.ContainsKey(objPair.Key))
                    {
                        foreach (var update in Storage.UnitGuidValuesUpdates[objPair.Key])
                        {
                            Row<CreatureGuidValuesUpdate> updateRow = new Row<CreatureGuidValuesUpdate>();
                            updateRow.Data.GUID = row.Data.Guid;
                            updateRow.Data.FieldName = update.FieldName;
                            Storage.GetObjectDbGuidEntryType(update.guid, out updateRow.Data.ObjectGuid, out updateRow.Data.ObjectId, out updateRow.Data.ObjectType);
                            updateRow.Data.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(update.time);
                            playerGuidValuesUpdateRows.Add(updateRow);
                        }
                    }
                }

                if (Settings.SqlTables.player_auras_update)
                {
                    if (Storage.UnitAurasUpdates.ContainsKey(objPair.Key))
                    {
                        uint updateId = 0;
                        foreach (var update in Storage.UnitAurasUpdates[objPair.Key])
                        {
                            updateId++;
                            foreach (var aura in update.Item1)
                            {
                                var updateRow = new Row<CreatureAurasUpdate>();
                                updateRow.Data.GUID = row.Data.Guid;
                                updateRow.Data.UpdateId = updateId;
                                updateRow.Data.Slot = aura.Slot;
                                updateRow.Data.SpellId = aura.SpellId;
                                updateRow.Data.VisualId = aura.VisualId;
                                updateRow.Data.AuraFlags = aura.AuraFlags;
                                updateRow.Data.ActiveFlags = aura.ActiveFlags;
                                updateRow.Data.Level = aura.Level;
                                updateRow.Data.Charges = aura.Charges;
                                updateRow.Data.ContentTuningId = aura.ContentTuningId;
                                updateRow.Data.Duration = aura.Duration;
                                updateRow.Data.MaxDuration = aura.MaxDuration;
                                Storage.GetObjectDbGuidEntryType(aura.CasterGuid, out updateRow.Data.CasterGuid, out updateRow.Data.CasterId, out updateRow.Data.CasterType);
                                updateRow.Data.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(update.Item2);
                                playerAurasUpdateRows.Add(updateRow);
                            }
                        }
                    }
                }

                if (Settings.SqlTables.player_values_update)
                {
                    if (Storage.UnitValuesUpdates.ContainsKey(objPair.Key))
                    {
                        foreach (var update in Storage.UnitValuesUpdates[objPair.Key])
                        {
                            var updateRow = new Row<CreatureValuesUpdate>();
                            updateRow.Data = update;
                            updateRow.Data.GUID = row.Data.Guid;
                            playerValuesUpdateRows.Add(updateRow);
                        }
                    }
                }

                if (Settings.SqlTables.player_speed_update)
                {
                    if (Storage.UnitSpeedUpdates.ContainsKey(objPair.Key))
                    {
                        foreach (var update in Storage.UnitSpeedUpdates[objPair.Key])
                        {
                            var updateRow = new Row<CreatureSpeedUpdate>();
                            updateRow.Data = update;
                            updateRow.Data.GUID = row.Data.Guid;
                            playerSpeedUpdateRows.Add(updateRow);
                        }
                    }
                }

                if (Settings.SqlTables.player_chat)
                {
                    foreach (var text in Storage.CharacterTexts)
                    {
                        if (text.Item1.SenderGUID == objPair.Key)
                        {
                            text.Item1.Guid = "@PGUID+" + player.DbGuid;
                            text.Item1.SenderName = row.Data.Name;
                        }
                    }
                }

                if (Settings.SqlTables.player_movement_server)
                {
                    foreach (ServerSideMovementSpline waypoint in player.CombatMovementSplines)
                    {
                        var movementSplineRow = new Row<ServerSideMovementSpline>();
                        movementSplineRow.Data = waypoint;
                        movementSplineRow.Data.GUID = "@PGUID+" + player.DbGuid;
                        playerServerMovementSplineRows.Add(movementSplineRow);
                    }

                    foreach (ServerSideMovement waypoint in player.CombatMovements)
                    {
                        if (waypoint == null)
                            break;

                        var movementRow = new Row<ServerSideMovement>();
                        movementRow.Data = waypoint;
                        movementRow.Data.GUID = "@PGUID+" + player.DbGuid;
                        playerServerMovementRows.Add(movementRow);
                    }
                }

                if (Settings.SqlTables.guild)
                {
                    if (player.UnitData.GuildGUID.Low != 0)
                    {
                        var guildRow = new Row<GuildMember>();
                        guildRow.Data.GuildGUID = player.UnitDataOriginal.GuildGUID.Low;
                        guildRow.Data.Guid = "@PGUID+" + player.DbGuid;
                        guildRow.Data.GuildRank = player.PlayerDataOriginal.GuildRankID;
                        guildMemberRows.Add(guildRow);
                    }
                }
            }

            if (Settings.SqlTables.characters)
            {
                var characterDelete = new SQLDelete<CharacterTemplate>(Tuple.Create("@PGUID+0", "@PGUID+" + maxDbGuid));
                result.Append(characterDelete.Build());
                var characterSql = new SQLInsert<CharacterTemplate>(characterRows, false);
                result.Append(characterSql.Build());
                result.AppendLine();
            }

            if (Settings.SqlTables.character_inventory)
            {
                var inventoryDelete = new SQLDelete<CharacterInventory>(Tuple.Create("@IGUID+0", "@IGUID+" + itemGuidCounter));
                result.Append(inventoryDelete.Build());
                var inventorySql = new SQLInsert<CharacterInventory>(characterInventoryRows, false);
                result.Append(inventorySql.Build());
                result.AppendLine();

                var itemInstanceDelete = new SQLDelete<CharacterItemInstance>(Tuple.Create("@IGUID+0", "@IGUID+" + itemGuidCounter));
                result.Append(itemInstanceDelete.Build());
                var itemInstanceSql = new SQLInsert<CharacterItemInstance>(characterItemInstaceRows, false);
                result.Append(itemInstanceSql.Build());
                result.AppendLine();
            }

            if (Settings.SqlTables.guild)
            {
                var guildSql = new SQLInsert<GuildMember>(guildMemberRows, false);
                result.Append(guildSql.Build());
                result.AppendLine();
            }

            if (Settings.SqlTables.player)
            {
                var playerDelete = new SQLDelete<PlayerTemplate>(Tuple.Create("@PGUID+0", "@PGUID+" + maxDbGuid));
                result.Append(playerDelete.Build());
                var playerSql = new SQLInsert<PlayerTemplate>(playerRows, false);
                result.Append(playerSql.Build());
                result.AppendLine();
            }

            if (Settings.SqlTables.player_guid_values)
            {
                var guidValuesDelete = new SQLDelete<CreatureGuidValues>(Tuple.Create("@PGUID+0", "@PGUID+" + maxDbGuid));
                guidValuesDelete.tableNameOverride = "player_guid_values";
                result.Append(guidValuesDelete.Build());
                var guidValuesSql = new SQLInsert<CreatureGuidValues>(playerGuidValuesRows, false, false, "player_guid_values");
                result.Append(guidValuesSql.Build());
                result.AppendLine();
            }

            if (Settings.SqlTables.player_active_player)
            {
                var activePlayersRows = new RowList<CharacterActivePlayer>();
                foreach (var itr in Storage.PlayerActiveCreateTime)
                {
                    Row<CharacterActivePlayer> row = new Row<CharacterActivePlayer>();
                    row.Data.Guid = Storage.GetObjectDbGuid(itr.Guid);
                    row.Data.UnixTime = (uint)Utilities.GetUnixTimeFromDateTime(itr.Time);
                    activePlayersRows.Add(row);
                }
                var activePlayersSql = new SQLInsert<CharacterActivePlayer>(activePlayersRows, true);
                result.Append(activePlayersSql.Build());
                result.AppendLine();
            }

            if (Settings.SqlTables.player_create1_time)
            {
                var createSql = new SQLInsert<PlayerCreate1>(playerCreate1Rows, false);
                result.Append(createSql.Build());
                result.AppendLine();
            }

            if (Settings.SqlTables.player_create2_time)
            {
                var createSql = new SQLInsert<PlayerCreate2>(playerCreate2Rows, false);
                result.Append(createSql.Build());
                result.AppendLine();
            }

            if (Settings.SqlTables.player_destroy_time)
            {
                var destroySql = new SQLInsert<PlayerDestroy>(playerDestroyRows, false);
                result.Append(destroySql.Build());
                result.AppendLine();
            }

            if (Settings.SqlTables.player_movement_client)
            {
                var movementRows = new RowList<ClientSideMovement>();
                foreach (var movement in Storage.PlayerMovements)
                {
                    if (Storage.Objects.ContainsKey(movement.guid))
                    {
                        Player player = Storage.Objects[movement.guid].Item1 as Player;
                        if (player == null)
                            continue;

                        if (Settings.SkipOtherPlayers && !player.IsActivePlayer &&
                           (movement.OpcodeDirection != Direction.ClientToServer))
                            continue;

                        Row<ClientSideMovement> row = new Row<ClientSideMovement>();
                        row.Data.Guid = "@PGUID+" + player.DbGuid;
                        row.Data.MoveFlags = movement.MoveFlags;
                        row.Data.MoveTime = movement.MoveTime;
                        row.Data.Map = movement.Map;
                        row.Data.PositionX = movement.Position.X;
                        row.Data.PositionY = movement.Position.Y;
                        row.Data.PositionZ = movement.Position.Z;
                        row.Data.Orientation = movement.Position.O;
                        row.Data.Opcode = Opcodes.GetOpcodeName(movement.Opcode, movement.OpcodeDirection);
                        row.Data.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(movement.Time);
                        movementRows.Add(row);
                    }
                }

                var movementSql = new SQLInsert<ClientSideMovement>(movementRows, false);
                result.Append(movementSql.Build());
                result.AppendLine();
            }

            if (Settings.SqlTables.player_movement_server)
            {
                var movementSql = new SQLInsert<ServerSideMovement>(playerServerMovementRows, false, false, "player_movement_server");
                result.Append(movementSql.Build());
                result.AppendLine();

                var movementSplineSql = new SQLInsert<ServerSideMovementSpline>(playerServerMovementSplineRows, false, false, "player_movement_server_spline");
                result.Append(movementSplineSql.Build());
                result.AppendLine();
            }

            if (Settings.SqlTables.player_attack_log)
            {
                var attackLogSql = new SQLInsert<UnitMeleeAttackLog>(playerAttackLogRows, false, false, "player_attack_log");
                result.Append(attackLogSql.Build());
                result.AppendLine();
            }

            if (Settings.SqlTables.player_attack_start)
            {
                var attackStartSql = new SQLInsert<CreatureAttackToggle>(playerAttackStartRows, false, false, "player_attack_start");
                result.Append(attackStartSql.Build());
                result.AppendLine();
            }

            if (Settings.SqlTables.player_attack_stop)
            {
                var attackStopSql = new SQLInsert<CreatureAttackToggle>(playerAttackStopRows, false, false, "player_attack_stop");
                result.Append(attackStopSql.Build());
                result.AppendLine();
            }

            if (Settings.SqlTables.player_emote)
            {
                var emoteSql = new SQLInsert<CreatureEmote>(playerEmoteRows, false, false, "player_emote");
                result.Append(emoteSql.Build());
                result.AppendLine();
            }

            if (Settings.SqlTables.player_equipment_values_update)
            {
                var equipmentUpdateSql = new SQLInsert<CreatureEquipmentValuesUpdate>(playerEquipmentValuesUpdateRows, false, false, "player_equipment_values_update");
                result.Append(equipmentUpdateSql.Build());
                result.AppendLine();
            }

            if (Settings.SqlTables.player_guid_values_update)
            {
                var guidsUpdateSql = new SQLInsert<CreatureGuidValuesUpdate>(playerGuidValuesUpdateRows, false, false, "player_guid_values_update");
                result.Append(guidsUpdateSql.Build());
                result.AppendLine();
            }

            if (Settings.SqlTables.player_auras_update)
            {
                var aurasUpdateSql = new SQLInsert<CreatureAurasUpdate>(playerAurasUpdateRows, false, false, "player_auras_update");
                result.Append(aurasUpdateSql.Build());
                result.AppendLine();
            }

            if (Settings.SqlTables.player_values_update)
            {
                var valuesUpdateSql = new SQLInsert<CreatureValuesUpdate>(playerValuesUpdateRows, false, false, "player_values_update");
                result.Append(valuesUpdateSql.Build());
                result.AppendLine();
            }

            if (Settings.SqlTables.player_speed_update)
            {
                var speedUpdateSql = new SQLInsert<CreatureSpeedUpdate>(playerSpeedUpdateRows, false, false, "player_speed_update");
                result.Append(speedUpdateSql.Build());
                result.AppendLine();
            }

            if (Settings.SqlTables.player_chat && !Storage.CharacterTexts.IsEmpty())
            {
                foreach (var text in Storage.CharacterTexts)
                {
                    if (text.Item1.Guid == null)
                    {
                        text.Item1.Guid = "0";
                        if (String.IsNullOrEmpty(text.Item1.SenderName) && !text.Item1.SenderGUID.IsEmpty())
                            text.Item1.SenderName = StoreGetters.GetName(text.Item1.SenderGUID);
                    }
                    if (text.Item1.ChannelName == null)
                        text.Item1.ChannelName = "";
                }
                result.Append(SQLUtil.Compare(Storage.CharacterTexts, SQLDatabase.Get(Storage.CharacterTexts), t => t.SenderName, false));
            }

            return result.ToString();
        }
    }
}
