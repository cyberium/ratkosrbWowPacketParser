using WowPacketParser.Misc;
using WowPacketParser.SQL;
using WowPacketParser.Enums;

namespace WowPacketParser.Store.Objects
{
    public sealed class NpcVendor : IDataModel
    {
        [DBFieldName("entry", true)]
        public uint? Entry;

        [DBFieldName("slot")]
        public int? Slot;

        [DBFieldName("item", true)]
        public int? Item;

        [DBFieldName("maxcount")]
        public uint? MaxCount;

        [DBFieldName("extended_cost", TargetedDbExpansion.Classic, TargetedDbExpansion.Zero, true, DbType = (TargetedDbType.WPP))]
        [DBFieldName("ExtendedCost", TargetedDbExpansion.Classic, TargetedDbExpansion.Zero, true, DbType = (TargetedDbType.TRINITY | TargetedDbType.CMANGOS))]
        [DBFieldName("extended_cost", TargetedDbExpansion.TheBurningCrusade, true, DbType = (TargetedDbType.WPP))]
        [DBFieldName("ExtendedCost", TargetedDbExpansion.TheBurningCrusade, true, DbType = (TargetedDbType.TRINITY | TargetedDbType.CMANGOS))]
        public uint? ExtendedCost;

        [DBFieldName("type", TargetedDbExpansion.Classic, TargetedDbExpansion.Zero, true, DbType = (TargetedDbType.WPP | TargetedDbType.TRINITY))]
        [DBFieldName("type", TargetedDbExpansion.Cataclysm, true, DbType = (TargetedDbType.WPP | TargetedDbType.TRINITY))]
        public uint? Type;

        [DBFieldName("player_condition_id", TargetedDbExpansion.Classic, TargetedDbExpansion.Zero, DbType = (TargetedDbType.WPP))]
        [DBFieldName("PlayerConditionID", TargetedDbExpansion.Classic, TargetedDbExpansion.Zero, DbType = (TargetedDbType.TRINITY))]
        [DBFieldName("player_condition_id", TargetedDbExpansion.Cataclysm, DbType = (TargetedDbType.WPP))]
        [DBFieldName("PlayerConditionID", TargetedDbExpansion.Cataclysm, DbType = (TargetedDbType.TRINITY))]
        public uint? PlayerConditionID;

        [DBFieldName("ignore_filtering", TargetedDbExpansion.Classic, TargetedDbExpansion.Zero, DbType = (TargetedDbType.WPP))]
        [DBFieldName("IgnoreFiltering", TargetedDbExpansion.Classic, TargetedDbExpansion.Zero, DbType = (TargetedDbType.TRINITY))]
        [DBFieldName("ignore_filtering", TargetedDbExpansion.WarlordsOfDraenor, DbType = (TargetedDbType.WPP))]
        [DBFieldName("IgnoreFiltering", TargetedDbExpansion.WarlordsOfDraenor, DbType = (TargetedDbType.TRINITY))]
        public bool IgnoreFiltering = false;

        [DBFieldName("sniff_build", DbType = (TargetedDbType.WPP))]
        [DBFieldName("VerifiedBuild", DbType = (TargetedDbType.TRINITY))]
        public int? VerifiedBuild = ClientVersion.BuildInt;
    }
}
