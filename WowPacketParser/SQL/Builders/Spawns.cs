using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Store;
using WowPacketParser.Store.Objects;

namespace WowPacketParser.SQL.Builders
{
    [BuilderClass]
    public static class Spawns
    {
        private static bool GetTransportMap(WoWObject @object, out int mapId)
        {
            mapId = -1;

            WoWObject transport;
            if (!Storage.Objects.TryGetValue(@object.Movement.TransportGuid, out transport))
                return false;

            if (SQLConnector.Enabled)
            {
                var transportTemplates = SQLDatabase.Get(new RowList<GameObjectTemplate> { new GameObjectTemplate { Entry = (uint)transport.ObjectData.EntryID } });
                if (transportTemplates.Count == 0)
                    return false;

                mapId = transportTemplates.First().Data.Data[6].GetValueOrDefault();
            }

            return true;
        }

        [BuilderMethod(Units = true)]
        public static string Creature(Dictionary<WowGuid, Unit> units)
        {
            if (units.Count == 0)
                return string.Empty;

            if (!Settings.SqlTables.creature)
                return string.Empty;

            uint count = 0;
            var rows = new RowList<Creature>();
            var addonRows = new RowList<CreatureAddon>();
            var create1Rows = new RowList<CreatureCreate1>();
            var create2Rows = new RowList<CreatureCreate2>();
            var destroyRows = new RowList<CreatureDestroy>();
            var movementRows = new RowList<CreatureMovement>();
            var movementSplineRows = new RowList<CreatureMovementSpline>();
            var updateRows = new RowList<CreatureUpdate>();
            string emoteRows = "";
            foreach (var unit in units)
            {
                Row<Creature> row = new Row<Creature>();
                bool badTransport = false;

                Unit creature = unit.Value;

                if (Settings.AreaFilters.Length > 0)
                    if (!(creature.Area.ToString(CultureInfo.InvariantCulture).MatchesFilters(Settings.AreaFilters)))
                        continue;

                if (Settings.MapFilters.Length > 0)
                    if (!(creature.Map.ToString(CultureInfo.InvariantCulture).MatchesFilters(Settings.MapFilters)))
                        continue;

                uint entry = (uint)creature.ObjectData.EntryID;
                if (entry == 0)
                    continue;   // broken entry, nothing to spawn

                if (creature.IsTemporarySpawn() && !Settings.SaveTempSpawns)
                    continue;

                uint movementType = 0;
                uint spawnDist = 0;
                row.Data.AreaID = 0;
                row.Data.ZoneID = 0;

                if (creature.Movement.HasWpsOrRandMov)
                {
                    movementType = 1;
                    spawnDist = 10;
                }

                row.Data.GUID = "@CGUID+" + count;

                row.Data.ID = entry;
                if (!creature.IsOnTransport())
                    row.Data.Map = creature.Map;
                else
                {
                    int mapId;
                    badTransport = !GetTransportMap(creature, out mapId);
                    if (mapId != -1)
                        row.Data.Map = (uint)mapId;
                }

                if (creature.Area != -1)
                    row.Data.AreaID = (uint)creature.Area;

                if (creature.Zone != -1)
                    row.Data.ZoneID = (uint)creature.Zone;

                row.Data.SpawnMask = (uint)creature.GetDefaultSpawnMask();

                if (ClientVersion.AddedInVersion(ClientVersionBuild.V7_0_3_22248))
                {
                    string data = string.Join(",", creature.GetDefaultSpawnDifficulties());
                    if (string.IsNullOrEmpty(data))
                        data = "0";

                    row.Data.spawnDifficulties = data;
                }

                row.Data.PhaseMask = creature.PhaseMask;

                if (ClientVersion.AddedInVersion(ClientVersionBuild.V4_3_4_15595) && creature.Phases != null)
                {
                    string data = string.Join(" - ", creature.Phases);
                    if (string.IsNullOrEmpty(data))
                        data = "0";

                    row.Data.PhaseID = data;
                }

                if (!creature.WasOriginallyOnTransport())
                {
                    row.Data.PositionX = creature.OriginalMovement.Position.X;
                    row.Data.PositionY = creature.OriginalMovement.Position.Y;
                    row.Data.PositionZ = creature.OriginalMovement.Position.Z;
                    row.Data.Orientation = creature.OriginalMovement.Orientation;
                }
                else
                {
                    row.Data.PositionX = creature.OriginalMovement.TransportOffset.X;
                    row.Data.PositionY = creature.OriginalMovement.TransportOffset.Y;
                    row.Data.PositionZ = creature.OriginalMovement.TransportOffset.Z;
                    row.Data.Orientation = creature.OriginalMovement.TransportOffset.O;
                }

                //row.Data.SpawnTimeSecs = creature.GetDefaultSpawnTime(creature.DifficultyID);
                row.Data.WanderDistance = spawnDist;
                row.Data.MovementType = movementType;

                // set some defaults
                Store.Objects.UpdateFields.IUnitData unitData = creature.UnitDataOriginal != null ? creature.UnitDataOriginal : creature.UnitData;
                row.Data.PhaseGroup = 0;
                row.Data.TemporarySpawn = 0;
                row.Data.CreatedBy = unitData.CreatedBy.GetEntry();
                row.Data.SummonedBy = unitData.SummonedBy.GetEntry();
                row.Data.SummonSpell = (uint)unitData.CreatedBySpell;
                row.Data.DisplayID = (uint)unitData.DisplayID;
                row.Data.FactionTemplate = (uint)unitData.FactionTemplate;
                row.Data.Level = (uint)unitData.Level;
                row.Data.CurHealth = (uint)unitData.CurHealth;
                row.Data.CurMana = (uint)unitData.CurMana;
                row.Data.MaxHealth = (uint)unitData.MaxHealth;
                row.Data.MaxMana = (uint)unitData.MaxMana;
                row.Data.SpeedWalk = creature.OriginalMovement.WalkSpeed;
                row.Data.SpeedRun = creature.OriginalMovement.RunSpeed;
                row.Data.Scale = creature.ObjectData.Scale;
                row.Data.BaseAttackTime = unitData.AttackRoundBaseTime[0];
                row.Data.RangedAttackTime = unitData.RangedAttackRoundBaseTime;
                row.Data.NpcFlag = (uint)unitData.NpcFlags[0];
                row.Data.UnitFlag = (uint)unitData.Flags;
                row.Data.SniffId = creature.SourceSniffId;

                row.Comment = StoreGetters.GetName(StoreNameType.Unit, (int)unit.Key.GetEntry(), false);
                row.Comment += " (Area: " + StoreGetters.GetName(StoreNameType.Area, creature.Area, false) + " - ";
                row.Comment += "Difficulty: " + StoreGetters.GetName(StoreNameType.Difficulty, (int)creature.DifficultyID, false) + ")";

                string auras = string.Empty;
                string commentAuras = string.Empty;
                if (creature.Auras != null && creature.Auras.Count != 0)
                {
                    foreach (Aura aura in creature.Auras)
                    {
                        if (aura == null)
                            continue;

                        // usually "template auras" do not have caster
                        if (ClientVersion.AddedInVersion(ClientType.MistsOfPandaria) ? !aura.AuraFlags.HasAnyFlag(AuraFlagMoP.NoCaster) : !aura.AuraFlags.HasAnyFlag(AuraFlag.NotCaster))
                            continue;

                        auras += aura.SpellId + " ";
                        commentAuras += aura.SpellId + " - " + StoreGetters.GetName(StoreNameType.Spell, (int)aura.SpellId, false) + ", ";
                    }

                    auras = auras.TrimEnd(' ');
                    commentAuras = commentAuras.TrimEnd(',', ' ');

                    row.Comment += " (Auras: " + commentAuras + ")";
                }

                var addonRow = new Row<CreatureAddon>();
                if (Settings.SqlTables.creature_addon)
                {
                    addonRow.Data.GUID = "@CGUID+" + count;
                    addonRow.Data.PathID = 0;
                    addonRow.Data.Mount = (uint)unitData.MountDisplayID;
                    addonRow.Data.Bytes1 = creature.Bytes1;
                    addonRow.Data.StandState = unitData.StandState;
                    addonRow.Data.PetTalentPoints = unitData.PetTalentPoints;
                    addonRow.Data.VisFlags = unitData.VisFlags;
                    addonRow.Data.AnimTier = unitData.AnimTier;
                    addonRow.Data.Bytes2 = creature.Bytes2;
                    addonRow.Data.SheatheState = unitData.SheatheState;
                    addonRow.Data.PvpFlags = unitData.PvpFlags;
                    addonRow.Data.PetFlags = unitData.PetFlags;
                    addonRow.Data.ShapeshiftForm = unitData.ShapeshiftForm;
                    addonRow.Data.Emote = (uint)unitData.EmoteState;
                    addonRow.Data.Auras = auras;
                    addonRow.Data.AIAnimKit = creature.AIAnimKit.GetValueOrDefault(0);
                    addonRow.Data.MovementAnimKit = creature.MovementAnimKit.GetValueOrDefault(0);
                    addonRow.Data.MeleeAnimKit = creature.MeleeAnimKit.GetValueOrDefault(0);
                    addonRow.Comment += StoreGetters.GetName(StoreNameType.Unit, (int)unit.Key.GetEntry(), false);
                    if (!string.IsNullOrWhiteSpace(auras))
                        addonRow.Comment += " - " + commentAuras;
                    addonRows.Add(addonRow);
                }

                if (Settings.SqlTables.creature_create1_time)
                {
                    if (Storage.ObjectCreate1Times.ContainsKey(unit.Key))
                    {
                        foreach (var createTime in Storage.ObjectCreate1Times[unit.Key])
                        {
                            var create1Row = new Row<CreatureCreate1>();
                            create1Row.Data.GUID = "@CGUID+" + count;
                            create1Row.Data.UnixTime = (uint)Utilities.GetUnixTimeFromDateTime(createTime);
                            create1Rows.Add(create1Row);
                        }
                    }
                }

                if (Settings.SqlTables.creature_create2_time)
                {
                    if (Storage.ObjectCreate2Times.ContainsKey(unit.Key))
                    {
                        foreach (var createTime in Storage.ObjectCreate2Times[unit.Key])
                        {
                            var create2Row = new Row<CreatureCreate2>();
                            create2Row.Data.GUID = "@CGUID+" + count;
                            create2Row.Data.UnixTime = (uint)Utilities.GetUnixTimeFromDateTime(createTime);
                            create2Rows.Add(create2Row);
                        }
                    }
                }

                if (Settings.SqlTables.creature_destroy_time)
                {
                    if (Storage.ObjectDestroyTimes.ContainsKey(unit.Key))
                    {
                        foreach (var createTime in Storage.ObjectDestroyTimes[unit.Key])
                        {
                            var destroyRow = new Row<CreatureDestroy>();
                            destroyRow.Data.GUID = "@CGUID+" + count;
                            destroyRow.Data.UnixTime = (uint)Utilities.GetUnixTimeFromDateTime(createTime);
                            destroyRows.Add(destroyRow);
                        }
                    }
                }

                if (Settings.SqlTables.creature_update)
                {
                    if (Storage.CreatureUpdates.ContainsKey(unit.Key))
                    {
                        foreach (var update in Storage.CreatureUpdates[unit.Key])
                        {
                            var updateRow = new Row<CreatureUpdate>();
                            updateRow.Data = update;
                            updateRow.Data.GUID = "@CGUID+" + count;
                            updateRows.Add(updateRow);
                        }
                    }
                }

                if (Settings.SqlTables.creature_movement &&
                    creature.Waypoints != null &&
                    creature.OriginalMovement.Position != null)
                {
                    try
                    {
                        if (creature.MovementSplines != null)
                        {
                            foreach (CreatureMovementSpline waypoint in creature.MovementSplines)
                            {
                                var movementRow = new Row<CreatureMovementSpline>();
                                movementRow.Data = waypoint;
                                movementRow.Data.GUID = "@CGUID+" + count;
                                movementRow.Comment += StoreGetters.GetName(StoreNameType.Unit, (int)unit.Key.GetEntry(), false); ;
                                movementSplineRows.Add(movementRow);
                            }
                        }

                        float maxDistanceFromSpawn = 0;
                        foreach (CreatureMovement waypoint in creature.Waypoints)
                        {
                            if (waypoint == null)
                                break;

                            // Get max wander distance
                            float distanceFromSpawn = Utilities.GetDistance3D(creature.OriginalMovement.Position.X, creature.OriginalMovement.Position.Y, creature.OriginalMovement.Position.Z, waypoint.StartPositionX, waypoint.StartPositionY, waypoint.StartPositionZ);
                            if (distanceFromSpawn > maxDistanceFromSpawn)
                                maxDistanceFromSpawn = distanceFromSpawn;

                            var movementRow = new Row<CreatureMovement>();
                            movementRow.Data = waypoint;
                            movementRow.Data.GUID = "@CGUID+" + count;
                            movementRow.Comment += StoreGetters.GetName(StoreNameType.Unit, (int)unit.Key.GetEntry(), false); ;
                            movementRows.Add(movementRow);
                        }
                        row.Data.WanderDistance = maxDistanceFromSpawn;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("{0} Exception caught while parsing waypoints.", e);
                        Console.WriteLine(WowPacketParser.Program.currentSniffFile);
                    }
                }

                if (Settings.SqlTables.creature_emote)
                {
                    if (Storage.Emotes.ContainsKey(unit.Key))
                    {
                        foreach (var emote in Storage.Emotes[unit.Key])
                        {
                            if (emoteRows != "")
                                emoteRows += ",\n";
                            emoteRows += "(@CGUID+" + count.ToString() + ", " + (uint)emote.emote + ", '" + emote.emote + "', " + (uint)Utilities.GetUnixTimeFromDateTime(emote.time) + ")";
                        }
                    }
                }

                // Likely to be waypoints if distance is big
                if (row.Data.WanderDistance > 20)
                    row.Data.MovementType = 2;

                if (creature.IsTemporarySpawn())
                    row.Data.TemporarySpawn = 1;

                if (creature.IsOnTransport() && badTransport)
                {
                    row.CommentOut = true;
                    row.Comment += " - !!! on transport - transport template not found !!!";
                    if (Settings.SqlTables.creature_addon)
                    {
                        addonRow.CommentOut = true;
                        addonRow.Comment += " - !!! on transport - transport template not found !!!";
                    }
                }
                else
                    ++count;

                if (creature.Movement.HasWpsOrRandMov)
                    row.Comment += " (possible waypoints or random movement)";

                rows.Add(row);
            }

            if (count == 0)
                return string.Empty;

            StringBuilder result = new StringBuilder();
            // delete query for GUIDs
            var delete = new SQLDelete<Creature>(Tuple.Create("@CGUID+0", "@CGUID+" + --count));
            result.Append(delete.Build());
            var sql = new SQLInsert<Creature>(rows, false);
            result.Append(sql.Build());

            if (Settings.SqlTables.creature_addon)
            {
                var addonDelete = new SQLDelete<CreatureAddon>(Tuple.Create("@CGUID+0", "@CGUID+" + count));
                result.Append(addonDelete.Build());
                var addonSql = new SQLInsert<CreatureAddon>(addonRows, false);
                result.Append(addonSql.Build());
            }

            if (Settings.SqlTables.creature_create1_time)
            {
                var create1Delete = new SQLDelete<CreatureCreate1>(Tuple.Create("@CGUID+0", "@CGUID+" + count));
                result.Append(create1Delete.Build());
                var createSql = new SQLInsert<CreatureCreate1>(create1Rows, false);
                result.Append(createSql.Build());
            }

            if (Settings.SqlTables.creature_create2_time)
            {
                var create2Delete = new SQLDelete<CreatureCreate2>(Tuple.Create("@CGUID+0", "@CGUID+" + count));
                result.Append(create2Delete.Build());
                var createSql = new SQLInsert<CreatureCreate2>(create2Rows, false);
                result.Append(createSql.Build());
            }

            if (Settings.SqlTables.creature_destroy_time)
            {
                var destroyDelete = new SQLDelete<CreatureDestroy>(Tuple.Create("@CGUID+0", "@CGUID+" + count));
                result.Append(destroyDelete.Build());
                var destroySql = new SQLInsert<CreatureDestroy>(destroyRows, false);
                result.Append(destroySql.Build());
            }

            if (Settings.SqlTables.creature_update)
            {
                var updateDelete = new SQLDelete<CreatureUpdate>(Tuple.Create("@CGUID+0", "@CGUID+" + count));
                result.Append(updateDelete.Build());
                var updateSql = new SQLInsert<CreatureUpdate>(updateRows, false);
                result.Append(updateSql.Build());
            }

            if (Settings.SqlTables.creature_movement)
            {
                // creature_movement
                var movementDelete = new SQLDelete<CreatureMovement>(Tuple.Create("@CGUID+0", "@CGUID+" + count));
                result.Append(movementDelete.Build());
                var movementSql = new SQLInsert<CreatureMovement>(movementRows, false);
                result.Append(movementSql.Build());

                // creature_movement_spline
                var movementSplineDelete = new SQLDelete<CreatureMovementSpline>(Tuple.Create("@CGUID+0", "@CGUID+" + count));
                result.Append(movementSplineDelete.Build());
                var movementSplineSql = new SQLInsert<CreatureMovementSpline>(movementSplineRows, false);
                result.Append(movementSplineSql.Build());
            }

            if (emoteRows != "")
            {
                result.Append("\nINSERT INTO `creature_emote` (`guid`, `emote_id`, `emote_name`, `unixtime`) VALUES\n");
                result.Append(emoteRows);
                result.Append(";\n\n");
            }

            return result.ToString();
        }

        [BuilderMethod(Gameobjects = true)]
        public static string GameObject(Dictionary<WowGuid, GameObject> gameObjects)
        {
            if (gameObjects.Count == 0)
                return string.Empty;

            if (!Settings.SqlTables.gameobject)
                return string.Empty;

            uint count = 0;
            var rows = new RowList<GameObjectModel>();
            var addonRows = new RowList<GameObjectAddon>();
            var create1Rows = new RowList<GameObjectCreate1>();
            var create2Rows = new RowList<GameObjectCreate2>();
            var destroyRows = new RowList<GameObjectDestroy>();
            var updateRows = new RowList<GameObjectUpdate>();
            foreach (var gameobject in gameObjects)
            {
                Row<GameObjectModel> row = new Row<GameObjectModel>();

                GameObject go = gameobject.Value;

                if (Settings.AreaFilters.Length > 0)
                    if (!(go.Area.ToString(CultureInfo.InvariantCulture).MatchesFilters(Settings.AreaFilters)))
                        continue;

                if (Settings.MapFilters.Length > 0)
                    if (!(go.Map.ToString(CultureInfo.InvariantCulture).MatchesFilters(Settings.MapFilters)))
                        continue;

                uint entry = (uint)go.ObjectData.EntryID;
                if (entry == 0)
                    continue;   // broken entry, nothing to spawn

                if (go.IsTemporarySpawn() && !Settings.SaveTempSpawns)
                    continue;

                bool badTransport = false;

                row.Data.GUID = "@OGUID+" + count;

                row.Data.ID = entry;
                if (!go.IsOnTransport())
                    row.Data.Map = go.Map;
                else
                {
                    int mapId;
                    badTransport = !GetTransportMap(go, out mapId);
                    if (mapId != -1)
                        row.Data.Map = (uint)mapId;
                }

                row.Data.ZoneID = 0;
                row.Data.AreaID = 0;

                if (go.Area != -1)
                    row.Data.AreaID = (uint)go.Area;

                if (go.Zone != -1)
                    row.Data.ZoneID = (uint)go.Zone;

                row.Data.SpawnMask = (uint)go.GetDefaultSpawnMask();


                if (ClientVersion.AddedInVersion(ClientVersionBuild.V7_0_3_22248))
                {
                    string data = string.Join(",", go.GetDefaultSpawnDifficulties());
                    if (string.IsNullOrEmpty(data))
                        data = "0";

                    row.Data.spawnDifficulties = data;
                }

                row.Data.PhaseMask = go.PhaseMask;

                if (ClientVersion.AddedInVersion(ClientVersionBuild.V4_3_4_15595) && go.Phases != null)
                    row.Data.PhaseID = string.Join(" - ", go.Phases);

                if (!go.IsOnTransport())
                {
                    row.Data.PositionX = go.Movement.Position.X;
                    row.Data.PositionY = go.Movement.Position.Y;
                    row.Data.PositionZ = go.Movement.Position.Z;
                    row.Data.Orientation = go.Movement.Orientation;
                }
                else
                {
                    row.Data.PositionX = go.Movement.TransportOffset.X;
                    row.Data.PositionY = go.Movement.TransportOffset.Y;
                    row.Data.PositionZ = go.Movement.TransportOffset.Z;
                    row.Data.Orientation = go.Movement.TransportOffset.O;
                }

                var rotation = go.GetStaticRotation();
                row.Data.Rotation = new float?[] { rotation.X, rotation.Y, rotation.Z, rotation.W };

                bool add = true;
                var addonRow = new Row<GameObjectAddon>();
                if (Settings.SqlTables.gameobject_addon)
                {
                    addonRow.Data.GUID = "@OGUID+" + count;

                    var parentRotation = go.GetParentRotation();

                    if (parentRotation != null)
                    {
                        addonRow.Data.parentRot0 = parentRotation.Value.X;
                        addonRow.Data.parentRot1 = parentRotation.Value.Y;
                        addonRow.Data.parentRot2 = parentRotation.Value.Z;
                        addonRow.Data.parentRot3 = parentRotation.Value.W;

                        if (addonRow.Data.parentRot0 == 0.0f &&
                            addonRow.Data.parentRot1 == 0.0f &&
                            addonRow.Data.parentRot2 == 0.0f &&
                            addonRow.Data.parentRot3 == 1.0f)
                            add = false;
                    }
                    else
                        add = false;

                    addonRow.Comment += StoreGetters.GetName(StoreNameType.GameObject, (int)gameobject.Key.GetEntry(), false);

                    if (add)
                        addonRows.Add(addonRow);
                }

                if (Settings.SqlTables.gameobject_create1_time)
                {
                    if (Storage.ObjectCreate1Times.ContainsKey(gameobject.Key))
                    {
                        foreach (var createTime in Storage.ObjectCreate1Times[gameobject.Key])
                        {
                            var create1Row = new Row<GameObjectCreate1>();
                            create1Row.Data.GUID = "@OGUID+" + count;
                            create1Row.Data.UnixTime = (uint)Utilities.GetUnixTimeFromDateTime(createTime);
                            create1Rows.Add(create1Row);
                        }
                    }
                }

                if (Settings.SqlTables.gameobject_create2_time)
                {
                    if (Storage.ObjectCreate2Times.ContainsKey(gameobject.Key))
                    {
                        foreach (var createTime in Storage.ObjectCreate2Times[gameobject.Key])
                        {
                            var create2Row = new Row<GameObjectCreate2>();
                            create2Row.Data.GUID = "@OGUID+" + count;
                            create2Row.Data.UnixTime = (uint)Utilities.GetUnixTimeFromDateTime(createTime);
                            create2Rows.Add(create2Row);
                        }
                    }
                }

                if (Settings.SqlTables.gameobject_destroy_time)
                {
                    if (Storage.ObjectDestroyTimes.ContainsKey(gameobject.Key))
                    {
                        foreach (var createTime in Storage.ObjectDestroyTimes[gameobject.Key])
                        {
                            var destroyRow = new Row<GameObjectDestroy>();
                            destroyRow.Data.GUID = "@OGUID+" + count;
                            destroyRow.Data.UnixTime = (uint)Utilities.GetUnixTimeFromDateTime(createTime);
                            destroyRows.Add(destroyRow);
                        }
                    }
                }

                if (Settings.SqlTables.gameobject_update)
                {
                    if (Storage.GameObjectUpdates.ContainsKey(gameobject.Key))
                    {
                        foreach (var update in Storage.GameObjectUpdates[gameobject.Key])
                        {
                            var updateRow = new Row<GameObjectUpdate>();
                            updateRow.Data = update;
                            updateRow.Data.GUID = "@OGUID+" + count;
                            updateRows.Add(updateRow);
                        }
                    }
                }

                row.Data.CreatedBy = go.GameObjectData.CreatedBy.GetEntry();
                //row.Data.SpawnTimeSecs = go.GetDefaultSpawnTime(go.DifficultyID);
                row.Data.AnimProgress = go.GameObjectDataOriginal.PercentHealth;
                row.Data.State = (uint)go.GameObjectDataOriginal.State;
                row.Data.SniffId = go.SourceSniffId;

                // set some defaults
                row.Data.PhaseGroup = 0;
                row.Data.TemporarySpawn = 0;

                if (go.IsTemporarySpawn())
                    row.Data.TemporarySpawn = 1;

                row.Comment = StoreGetters.GetName(StoreNameType.GameObject, (int)gameobject.Key.GetEntry(), false);
                row.Comment += " (Area: " + StoreGetters.GetName(StoreNameType.Area, go.Area, false) + " - ";
                row.Comment += "Difficulty: " + StoreGetters.GetName(StoreNameType.Difficulty, (int)go.DifficultyID, false) + ")";

                if (go.IsTransport())
                {
                    row.CommentOut = true;
                    row.Comment += " - !!! transport !!!";
                    if (Settings.SqlTables.gameobject_addon)
                    {
                        addonRow.CommentOut = true;
                        addonRow.Comment += " - !!! transport !!!";
                    }
                }
                else if (go.IsOnTransport() && badTransport)
                {
                    row.CommentOut = true;
                    row.Comment += " - !!! on transport - transport template not found !!!";
                    if (Settings.SqlTables.gameobject_addon)
                    {
                        addonRow.CommentOut = true;
                        addonRow.Comment += " - !!! on transport - transport template not found !!!";
                    }
                }
                else
                    ++count;

                rows.Add(row);
            }

            if (count == 0)
                return String.Empty;

            StringBuilder result = new StringBuilder();
            // delete query for GUIDs
            var delete = new SQLDelete<GameObjectModel>(Tuple.Create("@OGUID+0", "@OGUID+" + --count));
            result.Append(delete.Build());

            var sql = new SQLInsert<GameObjectModel>(rows, false);
            result.Append(sql.Build());

            if (Settings.SqlTables.gameobject_addon)
            {
                var addonDelete = new SQLDelete<GameObjectAddon>(Tuple.Create("@OGUID+0", "@OGUID+" + count));
                result.Append(addonDelete.Build());
                var addonSql = new SQLInsert<GameObjectAddon>(addonRows, false);
                result.Append(addonSql.Build());
            }

            if (Settings.SqlTables.gameobject_create1_time)
            {
                var create1Delete = new SQLDelete<GameObjectCreate1>(Tuple.Create("@OGUID+0", "@OGUID+" + count));
                result.Append(create1Delete.Build());
                var createSql = new SQLInsert<GameObjectCreate1>(create1Rows, false);
                result.Append(createSql.Build());
            }

            if (Settings.SqlTables.gameobject_create2_time)
            {
                var create2Delete = new SQLDelete<GameObjectCreate2>(Tuple.Create("@OGUID+0", "@OGUID+" + count));
                result.Append(create2Delete.Build());
                var createSql = new SQLInsert<GameObjectCreate2>(create2Rows, false);
                result.Append(createSql.Build());
            }

            if (Settings.SqlTables.gameobject_destroy_time)
            {
                var destroyDelete = new SQLDelete<GameObjectDestroy>(Tuple.Create("@OGUID+0", "@OGUID+" + count));
                result.Append(destroyDelete.Build());
                var destroySql = new SQLInsert<GameObjectDestroy>(destroyRows, false);
                result.Append(destroySql.Build());
            }

            if (Settings.SqlTables.gameobject_update)
            {
                var updateDelete = new SQLDelete<GameObjectUpdate>(Tuple.Create("@OGUID+0", "@OGUID+" + count));
                result.Append(updateDelete.Build());
                var updateSql = new SQLInsert<GameObjectUpdate>(updateRows, false);
                result.Append(updateSql.Build());
            }

            return result.ToString();
        }
    }
}
