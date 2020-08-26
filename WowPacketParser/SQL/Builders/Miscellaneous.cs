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

        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        [BuilderMethod]
        public static string Characters()
        {
            if (!Settings.SqlTables.characters)
                return string.Empty;

            StringBuilder result = new StringBuilder();
            uint maxDbGuid = 0;
            uint itemGuidCounter = 0;
            var characterRows = new RowList<CharacterTemplate>();
            var characterInventoryRows = new RowList<CharacterInventory>();
            var characterItemInstaceRows = new RowList<CharacterItemInstance>();
            Dictionary<int, uint> accountIdDictionary = new Dictionary<int, uint>();
            foreach (var objPair in Storage.Objects)
            {
                if (objPair.Key.GetObjectType() != ObjectType.Player)
                    continue;

                Player player = objPair.Value.Item1 as Player;
                if (player == null)
                    continue;

                Row<CharacterTemplate> row = new Row<CharacterTemplate>();

                row.Data.Guid = "@PGUID+" + player.DbGuid;
                if (accountIdDictionary.ContainsKey(player.UnitData.PlayerAccount))
                    row.Data.Account = "@ACCID+" + accountIdDictionary[player.UnitData.PlayerAccount];
                else
                {
                    uint id = (uint)accountIdDictionary.Count;
                    accountIdDictionary.Add(player.UnitData.PlayerAccount, id);
                    row.Data.Account = "@ACCID+" + id;
                }

                row.Data.Name = StoreGetters.GetName(objPair.Key);
                row.Data.Race = player.UnitData.RaceId;
                row.Data.Class = player.UnitData.ClassId;
                row.Data.Gender = player.UnitData.Sex;
                row.Data.Level = (uint)player.UnitData.Level;
                row.Data.XP = (uint)player.UnitData.PlayerExperience;
                row.Data.Money = (uint)player.UnitData.PlayerMoney;
                row.Data.PlayerBytes = player.UnitData.PlayerBytes1;
                row.Data.PlayerBytes2 = player.UnitData.PlayerBytes2;
                row.Data.PlayerFlags = (uint)player.UnitData.PlayerFlags;
                row.Data.PositionX = player.OriginalMovement.Position.X;
                row.Data.PositionY = player.OriginalMovement.Position.Y;
                row.Data.PositionZ = player.OriginalMovement.Position.Z;
                row.Data.Orientation = player.OriginalMovement.Orientation;
                row.Data.Map = player.Map;
                row.Data.Health = (uint)player.UnitData.CurHealth;
                row.Data.Power1 = (uint)player.UnitData.CurMana;

                for (int i = 0; i < 38; i++)
                {
                    int itemId = 0;

                    UpdateField value;
                    if (player.UpdateFields.TryGetValue(Enums.Version.UpdateFields.GetUpdateField(PlayerField.PLAYER_VISIBLE_ITEM) + i, out value))
                    {
                        itemId = value.Int32Value;

                        // even indexes are item ids, odd indexes are enchant ids
                        if ((itemId != 0) && (i % 2 == 0))
                        {
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
                        }
                    }

                    if (row.Data.EquipmentCache.Length > 0)
                        row.Data.EquipmentCache += " ";

                    row.Data.EquipmentCache += itemId;
                }

                characterRows.Add(row);

                if (maxDbGuid < player.DbGuid)
                    maxDbGuid = player.DbGuid;
            }

            var characterDelete = new SQLDelete<CharacterTemplate>(Tuple.Create("@PGUID+0", "@PGUID+" + maxDbGuid));
            result.Append(characterDelete.Build());
            var characterSql = new SQLInsert<CharacterTemplate>(characterRows, false);
            result.Append(characterSql.Build());

            var inventoryDelete = new SQLDelete<CharacterInventory>(Tuple.Create("@IGUID+0", "@IGUID+" + itemGuidCounter));
            result.Append(inventoryDelete.Build());
            var inventorySql = new SQLInsert<CharacterInventory>(characterInventoryRows, false);
            result.Append(inventorySql.Build());

            var itemInstanceDelete = new SQLDelete<CharacterItemInstance>(Tuple.Create("@IGUID+0", "@IGUID+" + itemGuidCounter));
            result.Append(itemInstanceDelete.Build());
            var itemInstanceSql = new SQLInsert<CharacterItemInstance>(characterItemInstaceRows, false);
            result.Append(itemInstanceSql.Build());

            return result.ToString();
        }
    }
}
