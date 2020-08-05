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

        [DBFieldName("loot_id", true)]
        public uint LootId;

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
        [DBFieldName("loot_id", true)]
        public uint? LootId;

        [DBFieldName("item_id", true)]
        public uint? ItemId;

        [DBFieldName("count")]
        public uint? Count;
    }
}
