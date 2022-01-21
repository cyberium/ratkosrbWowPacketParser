using WowPacketParser.Misc;
using WowPacketParser.SQL;
using WowPacketParser.Enums;

namespace WowPacketParser.Store.Objects
{
    public sealed class NpcVendor : ITableWithSniffIdList
    {
        [DBFieldName("entry", true)]
        public uint? Entry;

        [DBFieldName("slot", true, DbType = (TargetedDbType.WPP))]
        [DBFieldName("slot", DbType = (TargetedDbType.TRINITY))]
        public int? Slot;

        [DBFieldName("item", true)]
        public int? Item;

        [DBFieldName("maxcount", true, DbType = (TargetedDbType.WPP))]
        [DBFieldName("maxcount", DbType = (TargetedDbType.TRINITY | TargetedDbType.VMANGOS | TargetedDbType.CMANGOS))]
        public uint? MaxCount;

        [DBFieldName("extended_cost", TargetedDbExpansion.BurningCrusadeClassic, TargetedDbExpansion.Zero, true, DbType = (TargetedDbType.WPP))]
        [DBFieldName("ExtendedCost", TargetedDbExpansion.BurningCrusadeClassic, TargetedDbExpansion.Zero, true, DbType = (TargetedDbType.TRINITY | TargetedDbType.CMANGOS))]
        [DBFieldName("extended_cost", TargetedDbExpansion.TheBurningCrusade, true, DbType = (TargetedDbType.WPP))]
        [DBFieldName("ExtendedCost", TargetedDbExpansion.TheBurningCrusade, true, DbType = (TargetedDbType.TRINITY | TargetedDbType.CMANGOS))]
        public uint? ExtendedCost;

        [DBFieldName("type", TargetedDbExpansion.BurningCrusadeClassic, TargetedDbExpansion.Zero, true, DbType = (TargetedDbType.WPP | TargetedDbType.TRINITY))]
        [DBFieldName("type", TargetedDbExpansion.Cataclysm, true, DbType = (TargetedDbType.WPP | TargetedDbType.TRINITY))]
        public uint? Type;

        [DBFieldName("player_condition_id", TargetedDbExpansion.BurningCrusadeClassic, TargetedDbExpansion.Zero, DbType = (TargetedDbType.WPP))]
        [DBFieldName("PlayerConditionID", TargetedDbExpansion.BurningCrusadeClassic, TargetedDbExpansion.Zero, DbType = (TargetedDbType.TRINITY))]
        [DBFieldName("player_condition_id", TargetedDbExpansion.Cataclysm, DbType = (TargetedDbType.WPP))]
        [DBFieldName("PlayerConditionID", TargetedDbExpansion.Cataclysm, DbType = (TargetedDbType.TRINITY))]
        public uint? PlayerConditionID;

        [DBFieldName("ignore_filtering", TargetedDbExpansion.BurningCrusadeClassic, TargetedDbExpansion.Zero, DbType = (TargetedDbType.WPP))]
        [DBFieldName("IgnoreFiltering", TargetedDbExpansion.BurningCrusadeClassic, TargetedDbExpansion.Zero, DbType = (TargetedDbType.TRINITY))]
        [DBFieldName("ignore_filtering", TargetedDbExpansion.WarlordsOfDraenor, DbType = (TargetedDbType.WPP))]
        [DBFieldName("IgnoreFiltering", TargetedDbExpansion.WarlordsOfDraenor, DbType = (TargetedDbType.TRINITY))]
        public bool IgnoreFiltering = false;

        [DBFieldName("VerifiedBuild", DbType = (TargetedDbType.TRINITY))]
        public int? VerifiedBuild = ClientVersion.BuildInt;
    }
}
