using System.Collections.Generic;
using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.SQL;

namespace WowPacketParser.Store.Objects
{
    [DBTableName("loot_entry")]
    public sealed class LootEntry : IDataModel
    {
        [DBFieldName("entry", true)]
        public uint Entry;

        public uint LootId;

        [DBFieldName("loot_id", true, true)]
        public string LootIdString;

        [DBFieldName("money")]
        public uint Money;

        [DBFieldName("items_count")]
        public uint ItemsCount;

        public static uint LootIdCounter = 0;

        public List<LootItem> ItemsList = new List<LootItem>();
    }

    [DBTableName("loot_item")]
    public sealed class LootItem : IDataModel
    {
        public uint LootId;

        [DBFieldName("loot_id", true, true)]
        public string LootIdString;

        [DBFieldName("item_id", true)]
        public uint? ItemId;

        [DBFieldName("count")]
        public uint? Count;
    }
}
