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
            uint maxDbGuid = 0;
            var rows = new RowList<Creature>();
            var addonRows = new RowList<CreatureAddon>();
            var interactRows = new RowList<CreatureClientInteract>();
            var create1Rows = new RowList<CreatureCreate1>();
            var create2Rows = new RowList<CreatureCreate2>();
            var destroyRows = new RowList<CreatureDestroy>();
            var movementRows = new RowList<CreatureMovement>();
            var movementCombatRows = new RowList<CreatureMovement>();
            var movementSplineRows = new RowList<CreatureMovementSpline>();
            var updateValuesRows = new RowList<CreatureValuesUpdate>();
            var updateSpeedRows = new RowList<CreatureSpeedUpdate>();
            var attackLogRows = new RowList<UnitMeleeAttackLog>();
            var attackStartRows = new RowList<CreatureTargetChange>();
            var attackStopRows = new RowList<CreatureTargetChange>();
            var targetChangeRows = new RowList<CreatureTargetChange>();
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

                row.Data.GUID = "@CGUID+" + creature.DbGuid;

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
                row.Data.SpeedWalk = creature.OriginalMovement.WalkSpeed / MovementInfo.DEFAULT_WALK_SPEED;
                row.Data.SpeedRun = creature.OriginalMovement.RunSpeed / MovementInfo.DEFAULT_RUN_SPEED;
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
                    addonRow.Data.GUID = "@CGUID+" + creature.DbGuid;
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

                if (Settings.SqlTables.creature_client_interact)
                {
                    if (Storage.CreatureClientInteractTimes.ContainsKey(unit.Key))
                    {
                        foreach (var interactTime in Storage.CreatureClientInteractTimes[unit.Key])
                        {
                            var interactRow = new Row<CreatureClientInteract>();
                            interactRow.Data.GUID = "@CGUID+" + creature.DbGuid;
                            interactRow.Data.UnixTime = (uint)Utilities.GetUnixTimeFromDateTime(interactTime);
                            interactRows.Add(interactRow);
                        }
                    }
                }

                if (Settings.SqlTables.creature_create1_time)
                {
                    if (Storage.ObjectCreate1Times.ContainsKey(unit.Key))
                    {
                        foreach (var createTime in Storage.ObjectCreate1Times[unit.Key])
                        {
                            var create1Row = new Row<CreatureCreate1>();
                            create1Row.Data.GUID = "@CGUID+" + creature.DbGuid;
                            create1Row.Data.PositionX = createTime.PositionX;
                            create1Row.Data.PositionY = createTime.PositionY;
                            create1Row.Data.PositionZ = createTime.PositionZ;
                            create1Row.Data.Orientation = createTime.Orientation;
                            create1Row.Data.UnixTime = createTime.UnixTime;
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
                            create2Row.Data.GUID = "@CGUID+" + creature.DbGuid;
                            create2Row.Data.PositionX = createTime.PositionX;
                            create2Row.Data.PositionY = createTime.PositionY;
                            create2Row.Data.PositionZ = createTime.PositionZ;
                            create2Row.Data.Orientation = createTime.Orientation;
                            create2Row.Data.UnixTime = createTime.UnixTime;
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
                            destroyRow.Data.GUID = "@CGUID+" + creature.DbGuid;
                            destroyRow.Data.UnixTime = (uint)Utilities.GetUnixTimeFromDateTime(createTime);
                            destroyRows.Add(destroyRow);
                        }
                    }
                }

                if (Settings.SqlTables.creature_values_update)
                {
                    if (Storage.UnitValuesUpdates.ContainsKey(unit.Key))
                    {
                        foreach (var update in Storage.UnitValuesUpdates[unit.Key])
                        {
                            var updateRow = new Row<CreatureValuesUpdate>();
                            updateRow.Data = update;
                            updateRow.Data.GUID = "@CGUID+" + creature.DbGuid;
                            updateValuesRows.Add(updateRow);
                        }
                    }
                }

                if (Settings.SqlTables.creature_speed_update)
                {
                    if (Storage.UnitSpeedUpdates.ContainsKey(unit.Key))
                    {
                        foreach (var update in Storage.UnitSpeedUpdates[unit.Key])
                        {
                            var updateRow = new Row<CreatureSpeedUpdate>();
                            updateRow.Data = update;
                            updateRow.Data.GUID = "@CGUID+" + creature.DbGuid;
                            updateSpeedRows.Add(updateRow);
                        }
                    }
                }

                if (Settings.SqlTables.creature_movement &&
                        creature.Waypoints != null &&
                        creature.OriginalMovement.Position != null)
                {
                    if (creature.MovementSplines != null)
                    {
                        foreach (CreatureMovementSpline waypoint in creature.MovementSplines)
                        {
                            var movementRow = new Row<CreatureMovementSpline>();
                            movementRow.Data = waypoint;
                            movementRow.Data.GUID = "@CGUID+" + creature.DbGuid;
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
                        movementRow.Data.GUID = "@CGUID+" + creature.DbGuid;
                        movementRow.Comment += StoreGetters.GetName(StoreNameType.Unit, (int)unit.Key.GetEntry(), false); ;
                        movementRows.Add(movementRow);
                    }
                    row.Data.WanderDistance = maxDistanceFromSpawn;
                }

                if (Settings.SqlTables.creature_movement_combat &&
                    creature.CombatMovements != null)
                {
                    foreach (CreatureMovement waypoint in creature.CombatMovements)
                    {
                        if (waypoint == null)
                            break;

                        var movementCombatRow = new Row<CreatureMovement>();
                        movementCombatRow.Data = waypoint;
                        movementCombatRow.Data.GUID = "@CGUID+" + creature.DbGuid;
                        movementCombatRow.Comment += StoreGetters.GetName(StoreNameType.Unit, (int)unit.Key.GetEntry(), false); ;
                        movementCombatRows.Add(movementCombatRow);
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
                            emoteRows += "(@CGUID+" + creature.DbGuid.ToString() + ", " + (uint)emote.emote + ", '" + emote.emote + "', " + (uint)Utilities.GetUnixTimeFromDateTime(emote.time) + ")";
                        }
                    }
                }

                if (Settings.SqlTables.creature_attack_log)
                {
                    if (Storage.UnitAttackLogs.ContainsKey(unit.Key))
                    {
                        foreach (var attack in Storage.UnitAttackLogs[unit.Key])
                        {
                            var attackLogRow = new Row<UnitMeleeAttackLog>();
                            attackLogRow.Data = attack;
                            attackLogRow.Data.GUID = "@CGUID+" + creature.DbGuid;
                            Storage.GetObjectDbGuidEntryType(attack.Victim, out attackLogRow.Data.VictimGuid, out attackLogRow.Data.VictimId, out attackLogRow.Data.VictimType);
                            attackLogRow.Data.UnixTime = (uint)Utilities.GetUnixTimeFromDateTime(attack.Time);
                            attackLogRows.Add(attackLogRow);
                        }
                    }
                }

                if (Settings.SqlTables.creature_attack_start)
                {
                    if (Storage.UnitAttackStartTimes.ContainsKey(unit.Key))
                    {
                        foreach (var attack in Storage.UnitAttackStartTimes[unit.Key])
                        {
                            var attackStartRow = new Row<CreatureTargetChange>();

                            attackStartRow.Data.GUID = "@CGUID+" + creature.DbGuid;
                            Storage.GetObjectDbGuidEntryType(attack.victim, out attackStartRow.Data.VictimGuid, out attackStartRow.Data.VictimId, out attackStartRow.Data.VictimType);
                            attackStartRow.Data.UnixTime = (uint)Utilities.GetUnixTimeFromDateTime(attack.time);
                            attackStartRows.Add(attackStartRow);
                        }
                    }
                }

                if (Settings.SqlTables.creature_attack_stop)
                {
                    if (Storage.UnitAttackStopTimes.ContainsKey(unit.Key))
                    {
                        foreach (var attack in Storage.UnitAttackStopTimes[unit.Key])
                        {
                            var attackStopRow = new Row<CreatureTargetChange>();

                            attackStopRow.Data.GUID = "@CGUID+" + creature.DbGuid;
                            Storage.GetObjectDbGuidEntryType(attack.victim, out attackStopRow.Data.VictimGuid, out attackStopRow.Data.VictimId, out attackStopRow.Data.VictimType);
                            attackStopRow.Data.UnixTime = (uint)Utilities.GetUnixTimeFromDateTime(attack.time);
                            attackStopRows.Add(attackStopRow);
                        }
                    }
                }

                if (Settings.SqlTables.creature_target_change)
                {
                    if (Storage.UnitTargetChanges.ContainsKey(unit.Key))
                    {
                        foreach (var attack in Storage.UnitTargetChanges[unit.Key])
                        {
                            var targetChangeRow = new Row<CreatureTargetChange>();

                            targetChangeRow.Data.GUID = "@CGUID+" + creature.DbGuid;
                            Storage.GetObjectDbGuidEntryType(attack.victim, out targetChangeRow.Data.VictimGuid, out targetChangeRow.Data.VictimId, out targetChangeRow.Data.VictimType);
                            targetChangeRow.Data.UnixTime = (uint)Utilities.GetUnixTimeFromDateTime(attack.time);
                            targetChangeRows.Add(targetChangeRow);
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
                {
                    ++count;

                    if (creature.DbGuid > maxDbGuid)
                        maxDbGuid = creature.DbGuid;
                }

                if (creature.Movement.HasWpsOrRandMov)
                    row.Comment += " (possible waypoints or random movement)";

                rows.Add(row);
            }

            if (count == 0)
                return string.Empty;

            StringBuilder result = new StringBuilder();
            // delete query for GUIDs
            var delete = new SQLDelete<Creature>(Tuple.Create("@CGUID+0", "@CGUID+" + maxDbGuid));
            result.Append(delete.Build());
            var sql = new SQLInsert<Creature>(rows, false);
            result.Append(sql.Build());
            result.AppendLine();

            if (Settings.SqlTables.creature_addon)
            {
                var addonDelete = new SQLDelete<CreatureAddon>(Tuple.Create("@CGUID+0", "@CGUID+" + maxDbGuid));
                result.Append(addonDelete.Build());
                var addonSql = new SQLInsert<CreatureAddon>(addonRows, false);
                result.Append(addonSql.Build());
                result.AppendLine();
            }

            if (Settings.SqlTables.creature_client_interact)
            {
                var interactDelete = new SQLDelete<CreatureClientInteract>(Tuple.Create("@CGUID+0", "@CGUID+" + maxDbGuid));
                result.Append(interactDelete.Build());
                var interactSql = new SQLInsert<CreatureClientInteract>(interactRows, false);
                result.Append(interactSql.Build());
                result.AppendLine();
            }

            if (Settings.SqlTables.creature_create1_time)
            {
                var create1Delete = new SQLDelete<CreatureCreate1>(Tuple.Create("@CGUID+0", "@CGUID+" + maxDbGuid));
                result.Append(create1Delete.Build());
                var createSql = new SQLInsert<CreatureCreate1>(create1Rows, false);
                result.Append(createSql.Build());
                result.AppendLine();
            }

            if (Settings.SqlTables.creature_create2_time)
            {
                var create2Delete = new SQLDelete<CreatureCreate2>(Tuple.Create("@CGUID+0", "@CGUID+" + maxDbGuid));
                result.Append(create2Delete.Build());
                var createSql = new SQLInsert<CreatureCreate2>(create2Rows, false);
                result.Append(createSql.Build());
                result.AppendLine();
            }

            if (Settings.SqlTables.creature_destroy_time)
            {
                var destroyDelete = new SQLDelete<CreatureDestroy>(Tuple.Create("@CGUID+0", "@CGUID+" + maxDbGuid));
                result.Append(destroyDelete.Build());
                var destroySql = new SQLInsert<CreatureDestroy>(destroyRows, false);
                result.Append(destroySql.Build());
                result.AppendLine();
            }

            if (Settings.SqlTables.creature_values_update)
            {
                var updateDelete = new SQLDelete<CreatureValuesUpdate>(Tuple.Create("@CGUID+0", "@CGUID+" + maxDbGuid));
                result.Append(updateDelete.Build());
                var updateSql = new SQLInsert<CreatureValuesUpdate>(updateValuesRows, false);
                result.Append(updateSql.Build());
                result.AppendLine();
            }

            if (Settings.SqlTables.creature_speed_update)
            {
                var updateDelete = new SQLDelete<CreatureSpeedUpdate>(Tuple.Create("@CGUID+0", "@CGUID+" + maxDbGuid));
                result.Append(updateDelete.Build());
                var updateSql = new SQLInsert<CreatureSpeedUpdate>(updateSpeedRows, false);
                result.Append(updateSql.Build());
                result.AppendLine();
            }

            if (Settings.SqlTables.creature_movement)
            {
                // creature_movement
                var movementDelete = new SQLDelete<CreatureMovement>(Tuple.Create("@CGUID+0", "@CGUID+" + maxDbGuid));
                result.Append(movementDelete.Build());
                var movementSql = new SQLInsert<CreatureMovement>(movementRows, false);
                result.Append(movementSql.Build());
                result.AppendLine();

                // creature_movement_spline
                var movementSplineDelete = new SQLDelete<CreatureMovementSpline>(Tuple.Create("@CGUID+0", "@CGUID+" + maxDbGuid));
                result.Append(movementSplineDelete.Build());
                var movementSplineSql = new SQLInsert<CreatureMovementSpline>(movementSplineRows, false);
                result.Append(movementSplineSql.Build());
                result.AppendLine();
            }

            if (Settings.SqlTables.creature_movement_combat)
            {
                // creature_movement
                var movementDelete = new SQLDelete<CreatureMovement>(Tuple.Create("@CGUID+0", "@CGUID+" + maxDbGuid));
                movementDelete.tableNameOverride = "creature_movement_combat";
                result.Append(movementDelete.Build());
                var movementSql = new SQLInsert<CreatureMovement>(movementCombatRows, false, false, "creature_movement_combat");
                result.Append(movementSql.Build());
                result.AppendLine();
            }

            if (emoteRows != "")
            {
                result.Append("\nINSERT INTO `creature_emote` (`guid`, `emote_id`, `emote_name`, `unixtime`) VALUES\n");
                result.Append(emoteRows);
                result.Append(";\n\n");
            }

            if (Settings.SqlTables.creature_attack_log)
            {
                var attackDelete = new SQLDelete<UnitMeleeAttackLog>(Tuple.Create("@CGUID+0", "@CGUID+" + maxDbGuid));
                attackDelete.tableNameOverride = "creature_attack_log";
                result.Append(attackDelete.Build());
                var attackSql = new SQLInsert<UnitMeleeAttackLog>(attackLogRows, false, false, "creature_attack_log");
                result.Append(attackSql.Build());
                result.AppendLine();
            }

            if (Settings.SqlTables.creature_attack_start)
            {
                var attackDelete = new SQLDelete<CreatureTargetChange>(Tuple.Create("@CGUID+0", "@CGUID+" + maxDbGuid));
                attackDelete.tableNameOverride = "creature_attack_start";
                result.Append(attackDelete.Build());
                var attackSql = new SQLInsert<CreatureTargetChange>(attackStartRows, false, false, "creature_attack_start");
                result.Append(attackSql.Build());
                result.AppendLine();
            }

            if (Settings.SqlTables.creature_attack_stop)
            {
                var attackDelete = new SQLDelete<CreatureTargetChange>(Tuple.Create("@CGUID+0", "@CGUID+" + maxDbGuid));
                attackDelete.tableNameOverride = "creature_attack_stop";
                result.Append(attackDelete.Build());
                var attackSql = new SQLInsert<CreatureTargetChange>(attackStopRows, false, false, "creature_attack_stop");
                result.Append(attackSql.Build());
                result.AppendLine();
            }

            if (Settings.SqlTables.creature_target_change)
            {
                var attackDelete = new SQLDelete<CreatureTargetChange>(Tuple.Create("@CGUID+0", "@CGUID+" + maxDbGuid));
                attackDelete.tableNameOverride = "creature_target_change";
                result.Append(attackDelete.Build());
                var attackSql = new SQLInsert<CreatureTargetChange>(targetChangeRows, false, false, "creature_target_change");
                result.Append(attackSql.Build());
                result.AppendLine();
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
            uint maxDbGuid = 0;
            var rows = new RowList<GameObjectModel>();
            var addonRows = new RowList<GameObjectAddon>();
            var create1Rows = new RowList<GameObjectCreate1>();
            var create2Rows = new RowList<GameObjectCreate2>();
            var customAnimRows = new RowList<GameObjectCustomAnim>();
            var despawnAnimRows = new RowList<GameObjectDespawnAnim>();
            var destroyRows = new RowList<GameObjectDestroy>();
            var updateRows = new RowList<GameObjectUpdate>();
            var useRows = new RowList<GameObjectClientUse>();
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

                row.Data.GUID = "@OGUID+" + go.DbGuid;

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
                    addonRow.Data.GUID = "@OGUID+" + go.DbGuid;

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
                            create1Row.Data.GUID = "@OGUID+" + go.DbGuid;
                            create1Row.Data.PositionX = createTime.PositionX;
                            create1Row.Data.PositionY = createTime.PositionY;
                            create1Row.Data.PositionZ = createTime.PositionZ;
                            create1Row.Data.Orientation = createTime.Orientation;
                            create1Row.Data.UnixTime = createTime.UnixTime;
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
                            create2Row.Data.GUID = "@OGUID+" + go.DbGuid;
                            create2Row.Data.PositionX = createTime.PositionX;
                            create2Row.Data.PositionY = createTime.PositionY;
                            create2Row.Data.PositionZ = createTime.PositionZ;
                            create2Row.Data.Orientation = createTime.Orientation;
                            create2Row.Data.UnixTime = createTime.UnixTime;
                            create2Rows.Add(create2Row);
                        }
                    }
                }

                if (Settings.SqlTables.gameobject_custom_anim)
                {
                    if (Storage.GameObjectCustomAnims.ContainsKey(gameobject.Key))
                    {
                        foreach (var animTime in Storage.GameObjectCustomAnims[gameobject.Key])
                        {
                            var customAnimRow = new Row<GameObjectCustomAnim>();
                            customAnimRow.Data = animTime;
                            customAnimRow.Data.GUID = "@OGUID+" + go.DbGuid;
                            customAnimRows.Add(customAnimRow);
                        }
                    }
                }

                if (Settings.SqlTables.gameobject_despawn_anim)
                {
                    if (Storage.GameObjectDespawnAnims.ContainsKey(gameobject.Key))
                    {
                        foreach (var animTime in Storage.GameObjectDespawnAnims[gameobject.Key])
                        {
                            var despawnAnimRow = new Row<GameObjectDespawnAnim>();
                            despawnAnimRow.Data.GUID = "@OGUID+" + go.DbGuid;
                            despawnAnimRow.Data.UnixTime = (uint)Utilities.GetUnixTimeFromDateTime(animTime);
                            despawnAnimRows.Add(despawnAnimRow);
                        }
                    }
                }

                if (Settings.SqlTables.gameobject_destroy_time)
                {
                    if (Storage.ObjectDestroyTimes.ContainsKey(gameobject.Key))
                    {
                        foreach (var destroyTime in Storage.ObjectDestroyTimes[gameobject.Key])
                        {
                            var destroyRow = new Row<GameObjectDestroy>();
                            destroyRow.Data.GUID = "@OGUID+" + go.DbGuid;
                            destroyRow.Data.UnixTime = (uint)Utilities.GetUnixTimeFromDateTime(destroyTime);
                            destroyRows.Add(destroyRow);
                        }
                    }
                }

                if (Settings.SqlTables.gameobject_values_update)
                {
                    if (Storage.GameObjectUpdates.ContainsKey(gameobject.Key))
                    {
                        foreach (var update in Storage.GameObjectUpdates[gameobject.Key])
                        {
                            var updateRow = new Row<GameObjectUpdate>();
                            updateRow.Data = update;
                            updateRow.Data.GUID = "@OGUID+" + go.DbGuid;
                            updateRows.Add(updateRow);
                        }
                    }
                }

                if (Settings.SqlTables.gameobject_client_use)
                {
                    if (Storage.GameObjectClientUseTimes.ContainsKey(gameobject.Key))
                    {
                        foreach (var useTime in Storage.GameObjectClientUseTimes[gameobject.Key])
                        {
                            var useRow = new Row<GameObjectClientUse>();
                            useRow.Data.GUID = "@OGUID+" + go.DbGuid;
                            useRow.Data.UnixTime = (uint)Utilities.GetUnixTimeFromDateTime(useTime);
                            useRows.Add(useRow);
                        }
                    }
                }

                row.Data.CreatedBy = go.GameObjectData.CreatedBy.GetEntry();
                //row.Data.SpawnTimeSecs = go.GetDefaultSpawnTime(go.DifficultyID);
                row.Data.AnimProgress = go.GameObjectDataOriginal.PercentHealth;
                row.Data.State = (uint)go.GameObjectDataOriginal.State;
                row.Data.Flags = go.GameObjectDataOriginal.Flags;
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
                {
                    ++count;

                    if (go.DbGuid > maxDbGuid)
                        maxDbGuid = go.DbGuid;
                }

                rows.Add(row);
            }

            if (count == 0)
                return String.Empty;

            StringBuilder result = new StringBuilder();
            // delete query for GUIDs
            var delete = new SQLDelete<GameObjectModel>(Tuple.Create("@OGUID+0", "@OGUID+" + maxDbGuid));
            result.Append(delete.Build());

            var sql = new SQLInsert<GameObjectModel>(rows, false);
            result.Append(sql.Build());

            if (Settings.SqlTables.gameobject_addon)
            {
                var addonDelete = new SQLDelete<GameObjectAddon>(Tuple.Create("@OGUID+0", "@OGUID+" + maxDbGuid));
                result.Append(addonDelete.Build());
                var addonSql = new SQLInsert<GameObjectAddon>(addonRows, false);
                result.Append(addonSql.Build());
            }

            if (Settings.SqlTables.gameobject_create1_time)
            {
                var create1Delete = new SQLDelete<GameObjectCreate1>(Tuple.Create("@OGUID+0", "@OGUID+" + maxDbGuid));
                result.Append(create1Delete.Build());
                var createSql = new SQLInsert<GameObjectCreate1>(create1Rows, false);
                result.Append(createSql.Build());
            }

            if (Settings.SqlTables.gameobject_create2_time)
            {
                var create2Delete = new SQLDelete<GameObjectCreate2>(Tuple.Create("@OGUID+0", "@OGUID+" + maxDbGuid));
                result.Append(create2Delete.Build());
                var createSql = new SQLInsert<GameObjectCreate2>(create2Rows, false);
                result.Append(createSql.Build());
            }

            if (Settings.SqlTables.gameobject_custom_anim)
            {
                var animDelete = new SQLDelete<GameObjectCustomAnim>(Tuple.Create("@OGUID+0", "@OGUID+" + maxDbGuid));
                result.Append(animDelete.Build());
                var animSql = new SQLInsert<GameObjectCustomAnim>(customAnimRows, false);
                result.Append(animSql.Build());
            }

            if (Settings.SqlTables.gameobject_despawn_anim)
            {
                var animDelete = new SQLDelete<GameObjectDespawnAnim>(Tuple.Create("@OGUID+0", "@OGUID+" + maxDbGuid));
                result.Append(animDelete.Build());
                var animSql = new SQLInsert<GameObjectDespawnAnim>(despawnAnimRows, false);
                result.Append(animSql.Build());
            }

            if (Settings.SqlTables.gameobject_destroy_time)
            {
                var destroyDelete = new SQLDelete<GameObjectDestroy>(Tuple.Create("@OGUID+0", "@OGUID+" + maxDbGuid));
                result.Append(destroyDelete.Build());
                var destroySql = new SQLInsert<GameObjectDestroy>(destroyRows, false);
                result.Append(destroySql.Build());
            }

            if (Settings.SqlTables.gameobject_values_update)
            {
                var updateDelete = new SQLDelete<GameObjectUpdate>(Tuple.Create("@OGUID+0", "@OGUID+" + maxDbGuid));
                result.Append(updateDelete.Build());
                var updateSql = new SQLInsert<GameObjectUpdate>(updateRows, false);
                result.Append(updateSql.Build());
            }

            if (Settings.SqlTables.gameobject_client_use)
            {
                var useDelete = new SQLDelete<GameObjectClientUse>(Tuple.Create("@OGUID+0", "@OGUID+" + maxDbGuid));
                result.Append(useDelete.Build());
                var useSql = new SQLInsert<GameObjectClientUse>(useRows, false);
                result.Append(useSql.Build());
            }

            return result.ToString();
        }
    }
}
