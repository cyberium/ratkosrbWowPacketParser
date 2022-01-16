using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Store;
using WowPacketParser.Store.Objects;

namespace WowPacketParser.SQL.Builders
{
    [BuilderClass]
    public static class Miscellaneous
    {
        [BuilderMethod]
        public static string StartInformation()
        {
            string result = string.Empty;

            if (!Storage.StartActions.IsEmpty() && Settings.SqlTables.playercreateinfo_action)
            {
                result += SQLUtil.Compare(Storage.StartActions, SQLDatabase.Get(Storage.StartActions), a =>
                {
                    if (a.Type == ActionButtonType.Spell)
                        return StoreGetters.GetName(StoreNameType.Spell, (int)a.Action.GetValueOrDefault(), false);
                    if (a.Type == ActionButtonType.Item)
                        return StoreGetters.GetName(StoreNameType.Item, (int)a.Action.GetValueOrDefault(), false);

                    return string.Empty;
                });

            }

            if (!Storage.StartPositions.IsEmpty() && Settings.SqlTables.playercreateinfo)
            {
                var dataDb = SQLDatabase.Get(Storage.StartPositions);

                result += SQLUtil.Compare(Storage.StartPositions, dataDb, StoreNameType.None);
            }

            return result;
        }

        [BuilderMethod]
        public static string ObjectNames()
        {
            if (Storage.ObjectNames.IsEmpty())
                return string.Empty;

            if (!Settings.SqlTables.ObjectNames)
                return string.Empty;

            var templateDb = SQLDatabase.Get(Storage.ObjectNames, Settings.WPPDatabase);

            return SQLUtil.Compare(Storage.ObjectNames, templateDb, StoreNameType.None);
        }

        [BuilderMethod]
        public static string SniffData()
        {
            if (Storage.SniffData.IsEmpty())
                return string.Empty;

            if (Settings.DumpFormat != DumpFormatType.SniffDataOnly)
                if (!Settings.SqlTables.SniffData && !Settings.SqlTables.SniffDataOpcodes)
                    return string.Empty;

            var rows = new RowList<SniffData>();
            foreach (var data in Storage.SniffData)
            {
                Row<SniffData> row = new Row<SniffData>();
                row.Data = data.Item1;
                rows.Add(row);
            }

            return new SQLInsert<SniffData>(rows, false, true).Build();
        }

        // Non-WDB data but nevertheless data that should be saved to gameobject_template
        /*[BuilderMethod(Gameobjects = true)]
        public static string GameobjectTemplateNonWDB(Dictionary<WowGuid, GameObject> gameobjects)
        {
            if (gameobjects.Count == 0)
                return string.Empty;

            if (!Settings.SqlTables.gameobject_template))
                return string.Empty;

            var templates = new StoreDictionary<uint, GameObjectTemplateNonWDB>();
            foreach (var goT in gameobjects)
            {
                if (templates.ContainsKey(goT.Key.GetEntry()))
                    continue;

                var go = goT.Value;

                if (Settings.AreaFilters.Length > 0)
                    if (!(go.Area.ToString(CultureInfo.InvariantCulture).MatchesFilters(Settings.AreaFilters)))
                        continue;

                if (Settings.MapFilters.Length > 0)
                    if (!(go.Map.ToString(CultureInfo.InvariantCulture).MatchesFilters(Settings.MapFilters)))
                        continue;

                var template = new GameObjectTemplateNonWDB
                {
                    Size = go.Size.GetValueOrDefault(1.0f),
                    Faction = go.Faction.GetValueOrDefault(0),
                    Flags = go.Flags.GetValueOrDefault(GameObjectFlag.None)
                };

                if (template.Faction == 1 || template.Faction == 2 || template.Faction == 3 ||
                    template.Faction == 4 || template.Faction == 5 || template.Faction == 6 ||
                    template.Faction == 115 || template.Faction == 116 || template.Faction == 1610 ||
                    template.Faction == 1629 || template.Faction == 2203 || template.Faction == 2204 ||
                    template.Faction == 2395 || template.Faction == 2401 || template.Faction == 2402) // player factions
                    template.Faction = 0;

                template.Flags &= ~GameObjectFlag.Triggered;
                template.Flags &= ~GameObjectFlag.Damaged;
                template.Flags &= ~GameObjectFlag.Destroyed;

                templates.Add(goT.Key.GetEntry(), template);
            }

            var templatesDb = SQLDatabase.GetDict<uint, GameObjectTemplateNonWDB>(templates.Keys());
            return SQLUtil.CompareDicts(templates, templatesDb, StoreNameType.GameObject);
        }*/

        [BuilderMethod]
        public static string WeatherUpdates()
        {
            if (Storage.WeatherUpdates.IsEmpty())
                return string.Empty;

            if (!Settings.SqlTables.weather_updates)
                return string.Empty;

            var rows = new RowList<WeatherUpdate>();
            foreach (var row in Storage.WeatherUpdates.Select(weatherUpdate => new Row<WeatherUpdate>
            {
                Data = weatherUpdate.Item1,
                Comment = StoreGetters.GetName(StoreNameType.Map, (int)weatherUpdate.Item1.MapId.GetValueOrDefault(), false) +
                          " - " + weatherUpdate.Item1.State + " - " + weatherUpdate.Item1.Grade
            }))
            {
                rows.Add(row);
            }

            return new SQLInsert<WeatherUpdate>(rows, false, false).Build();
        }

        [BuilderMethod]
        public static string SceneTemplates()
        {
            if (Storage.Scenes.IsEmpty())
                return string.Empty;

            if (!Settings.SqlTables.scene_template)
                return string.Empty;

            var templateDb = SQLDatabase.Get(Storage.Scenes, Settings.TDBDatabase);

            return SQLUtil.Compare(Storage.Scenes, templateDb, StoreNameType.None);
        }

        public static string BuildLootQuery(Dictionary<uint, Dictionary<WowGuid, LootEntry>> storage, string entryTable, string itemTable)
        {
            var lootTemplateRows = new RowList<LootEntry>();
            var lootItemRows = new RowList<LootItem>();
            uint maxLootId = 0;
            foreach (var pair1 in storage)
            {
                foreach (var pair2 in pair1.Value)
                {
                    Row<LootEntry> row = new Row<LootEntry>();
                    row.Data = pair2.Value;
                    row.Data.LootIdString = "@LOOTID+" + row.Data.LootId;
                    lootTemplateRows.Add(row);

                    foreach (var itr in pair2.Value.ItemsList)
                    {
                        Row<LootItem> row2 = new Row<LootItem>();
                        row2.Data = itr;
                        row2.Data.LootIdString = "@LOOTID+" + row2.Data.LootId;
                        lootItemRows.Add(row2);
                    }

                    if (pair2.Value.LootId > maxLootId)
                        maxLootId = pair2.Value.LootId;
                }
            }
            StringBuilder result = new StringBuilder();

            if (lootTemplateRows.Count != 0)
            {
                result.AppendLine("DELETE FROM `" + entryTable + "` WHERE `loot_id` BETWEEN @LOOTID+0 AND @LOOTID+" + maxLootId + ";");
                var templateSql = new SQLInsert<LootEntry>(lootTemplateRows, false, false, entryTable);
                result.Append(templateSql.Build());
            }
            
            if (lootItemRows.Count != 0)
            {
                result.AppendLine("DELETE FROM `" + itemTable + "` WHERE `loot_id` BETWEEN @LOOTID+0 AND @LOOTID+" + maxLootId + ";");
                var itemSql = new SQLInsert<LootItem>(lootItemRows, false, false, itemTable);
                result.Append(itemSql.Build());
            }
            
            return result.ToString();
        }

        [BuilderMethod]
        public static string LootTemplates()
        {
            string query = "";

            if (Storage.CreatureLoot.Count > 0 && Settings.SqlTables.creature_loot)
            {
                query += BuildLootQuery(Storage.CreatureLoot, "creature_loot", "creature_loot_item");
            }

            if (Storage.GameObjectLoot.Count > 0 && Settings.SqlTables.gameobject_loot)
            {
                if (!String.IsNullOrEmpty(query))
                    query += Environment.NewLine;

                query += BuildLootQuery(Storage.GameObjectLoot, "gameobject_loot", "gameobject_loot_item");
            }

            return query;
        }

        [BuilderMethod]
        public static string ItemClientUseTimes()
        {
            if (Storage.ItemClientUseTimes.IsEmpty())
                return string.Empty;

            if (!Settings.SqlTables.client_item_use)
                return string.Empty;

            return SQLUtil.Insert(Storage.ItemClientUseTimes, false, false);
        }

        [BuilderMethod]
        public static string WorldTexts()
        {
            if (Storage.WorldTexts.IsEmpty())
                return string.Empty;

            if (!Settings.SqlTables.world_text)
                return string.Empty;

            return SQLUtil.Insert(Storage.WorldTexts, false, false);
        }

        [BuilderMethod]
        public static string ClientAreatriggerEnterTimes()
        {
            if (Storage.ClientAreatriggerEnterTimes.IsEmpty())
                return string.Empty;

            if (!Settings.SqlTables.client_areatrigger_enter)
                return string.Empty;

            return SQLUtil.Insert(Storage.ClientAreatriggerEnterTimes, false, false);
        }

        [BuilderMethod]
        public static string ClientAreatriggerLeaveTimes()
        {
            if (Storage.ClientAreatriggerLeaveTimes.IsEmpty())
                return string.Empty;

            if (!Settings.SqlTables.client_areatrigger_leave)
                return string.Empty;

            return SQLUtil.Insert(Storage.ClientAreatriggerLeaveTimes, false, false);
        }

        [BuilderMethod]
        public static string ClientReclaimCorpseTimes()
        {
            if (Storage.ClientReclaimCorpseTimes.IsEmpty())
                return string.Empty;

            if (!Settings.SqlTables.client_reclaim_corpse)
                return string.Empty;

            return SQLUtil.Insert(Storage.ClientReclaimCorpseTimes, false, false);
        }

        [BuilderMethod]
        public static string ClientReleaseSpiritTimes()
        {
            if (Storage.ClientReleaseSpiritTimes.IsEmpty())
                return string.Empty;

            if (!Settings.SqlTables.client_release_spirit)
                return string.Empty;

            return SQLUtil.Insert(Storage.ClientReleaseSpiritTimes, false, false);
        }

        [BuilderMethod]
        public static string WorldStateInits()
        {
            if (Storage.WorldStateInits.IsEmpty())
                return string.Empty;

            if (!Settings.SqlTables.world_state_init)
                return string.Empty;

            return SQLUtil.Insert(Storage.WorldStateInits, false, false);
        }

        [BuilderMethod]
        public static string WorldStateUpdates()
        {
            if (Storage.WorldStateUpdates.IsEmpty())
                return string.Empty;

            if (!Settings.SqlTables.world_state_update)
                return string.Empty;

            return SQLUtil.Insert(Storage.WorldStateUpdates, false, false);
        }

        [BuilderMethod]
        public static string XpGainLogs()
        {
            if (Storage.XpGainLogs.IsEmpty())
                return string.Empty;

            if (!Settings.SqlTables.xp_gain_log)
                return string.Empty;

            var rows = new RowList<XpGainLog>();
            foreach (var log in Storage.XpGainLogs)
            {
                Row<XpGainLog> row = new Row<XpGainLog>();
                row.Data = log.Item1;
                Storage.GetObjectDbGuidEntryType(row.Data.GUID, out row.Data.VictimGuid, out row.Data.VictimId, out row.Data.VictimType);
                row.Data.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(row.Data.Time);
                rows.Add(row);
            }

            var sql = new SQLInsert<XpGainLog>(rows, false);
            return sql.Build();
        }

        [BuilderMethod]
        public static string XpGainAborted()
        {
            if (Storage.XpGainAborted.IsEmpty())
                return string.Empty;

            if (!Settings.SqlTables.xp_gain_aborted)
                return string.Empty;

            var rows = new RowList<XpGainAborted>();
            foreach (var log in Storage.XpGainAborted)
            {
                Row<XpGainAborted> row = new Row<XpGainAborted>();
                row.Data = log.Item1;
                Storage.GetObjectDbGuidEntryType(row.Data.GUID, out row.Data.VictimGuid, out row.Data.VictimId, out row.Data.VictimType);
                row.Data.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(row.Data.Time);
                rows.Add(row);
            }

            var sql = new SQLInsert<XpGainAborted>(rows, false);
            return sql.Build();
        }

        [BuilderMethod]
        public static string FactionStandingUpdates()
        {
            if (Storage.FactionStandingUpdates.IsEmpty())
                return string.Empty;

            if (!Settings.SqlTables.faction_standing_update)
                return string.Empty;

            var rows = new RowList<FactionStandingUpdate>();
            foreach (var log in Storage.FactionStandingUpdates)
            {
                Row<FactionStandingUpdate> row = new Row<FactionStandingUpdate>();
                row.Data = log.Item1;
                rows.Add(row);
            }

            var sql = new SQLInsert<FactionStandingUpdate>(rows, false);
            return sql.Build();
        }

        [BuilderMethod]
        public static string LogoutTimes()
        {
            if (Storage.LogoutTimes.IsEmpty())
                return string.Empty;

            if (!Settings.SqlTables.logout_time)
                return string.Empty;

            return SQLUtil.Insert(Storage.LogoutTimes, false, false);
        }

        [BuilderMethod]
        public static string CinematicBeginTimes()
        {
            if (Storage.CinematicBeginTimes.IsEmpty())
                return string.Empty;

            if (!Settings.SqlTables.cinematic_begin)
                return string.Empty;

            return SQLUtil.Insert(Storage.CinematicBeginTimes, false, false);
        }

        [BuilderMethod]
        public static string CinematicEndTimes()
        {
            if (Storage.CinematicEndTimes.IsEmpty())
                return string.Empty;

            if (!Settings.SqlTables.cinematic_end)
                return string.Empty;

            return SQLUtil.Insert(Storage.CinematicEndTimes, false, false);
        }

        [BuilderMethod]
        public static string MailTemplates()
        {
            if (Storage.MailTemplates.IsEmpty())
                return string.Empty;

            if (!Settings.SqlTables.mail_template)
                return string.Empty;

            return SQLUtil.Insert(Storage.MailTemplates, false, true);
        }

        [BuilderMethod]
        public static string MailTemplateItems()
        {
            if (Storage.MailTemplateItems.IsEmpty())
                return string.Empty;

            if (!Settings.SqlTables.mail_template)
                return string.Empty;

            return SQLUtil.Insert(Storage.MailTemplateItems, false, true);
        }

        [BuilderMethod]
        public static string GameObjectUniqueAnims()
        {
            if (Storage.GameObjectUniqueAnims.IsEmpty())
                return string.Empty;

            if (!Settings.SqlTables.gameobject_unique_anim)
                return string.Empty;

            return SQLUtil.MakeInsertWithSniffIdList(Storage.GameObjectUniqueAnims, false, true);
        }
    }
}
