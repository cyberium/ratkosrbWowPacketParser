using System.Collections.Generic;
using System.Linq;
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

            var templateDb = SQLDatabase.Get(Storage.SniffData, Settings.WPPDatabase);

            return SQLUtil.Compare(Storage.SniffData, templateDb, x => string.Empty);
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
                    template.Faction == 1629 || template.Faction == 2203 || template.Faction == 2204) // player factions
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

            return new SQLInsert<WeatherUpdate>(rows, ignore: true, withDelete: false).Build();
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
            uint rowsCount = 0;
            uint rowsCount2 = 0;
            string query = "INSERT INTO `" + entryTable + "` (`entry`, `loot_id`, `money`, `items_count`) VALUES\n";
            string query2 = "INSERT INTO `" + itemTable + "` (`loot_id`, `item_id`, `count`) VALUES\n";
            foreach (var pair1 in storage)
            {
                foreach (var pair2 in pair1.Value)
                {
                    if (rowsCount > 0)
                        query += ",\n";
                    query += "(" + pair2.Value.Entry + ", " + pair2.Value.LootId + ", " + pair2.Value.Money + ", " + pair2.Value.ItemsCount + ")";
                    rowsCount++;

                    foreach (var itr in pair2.Value.ItemsList)
                    {
                        if (rowsCount2 > 0)
                            query2 += ",\n";
                        query2 += "(" + itr.LootId + ", " + itr.ItemId + ", " + itr.Count + ")";
                        rowsCount2++;
                    }
                }
            }
            query += ";\n";
            query += query2;
            query += ";\n\n";
            return query;
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
                query += BuildLootQuery(Storage.GameObjectLoot, "gameobject_loot", "gameobject_loot_item");
            }

            return query;
        }

        [BuilderMethod]
        public static string ItemClientUseTimes()
        {
            if (Storage.ItemClientUseTimes.IsEmpty())
                return string.Empty;

            if (!Settings.SqlTables.item_client_use)
                return string.Empty;

            var templateDb = SQLDatabase.Get(Storage.ItemClientUseTimes, Settings.TDBDatabase);

            return SQLUtil.Compare(Storage.ItemClientUseTimes, templateDb, StoreNameType.None);
        }

        [BuilderMethod]
        public static string WorldTexts()
        {
            if (Storage.WorldTexts.IsEmpty())
                return string.Empty;

            if (!Settings.SqlTables.world_text)
                return string.Empty;

            var templateDb = SQLDatabase.Get(Storage.WorldTexts, Settings.TDBDatabase);

            return SQLUtil.Compare(Storage.WorldTexts, templateDb, StoreNameType.None);
        }

        [BuilderMethod]
        public static string ClientReclaimCorpseTimes()
        {
            if (Storage.ClientReclaimCorpseTimes.IsEmpty())
                return string.Empty;

            if (!Settings.SqlTables.client_reclaim_corpse)
                return string.Empty;

            var templateDb = SQLDatabase.Get(Storage.ClientReclaimCorpseTimes, Settings.TDBDatabase);

            return SQLUtil.Compare(Storage.ClientReclaimCorpseTimes, templateDb, StoreNameType.None);
        }

        [BuilderMethod]
        public static string ClientReleaseSpiritTimes()
        {
            if (Storage.ClientReleaseSpiritTimes.IsEmpty())
                return string.Empty;

            if (!Settings.SqlTables.client_release_spirit)
                return string.Empty;

            var templateDb = SQLDatabase.Get(Storage.ClientReleaseSpiritTimes, Settings.TDBDatabase);

            return SQLUtil.Compare(Storage.ClientReleaseSpiritTimes, templateDb, StoreNameType.None);
        }
    }
}
