using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.SQL;

namespace WowPacketParser.Store.Objects
{
    [DBTableName("page_text")]
    public sealed class PageText : IDataModel
    {
        [DBFieldName("entry", true, DbType = (TargetedDbType.WPP | TargetedDbType.VMANGOS | TargetedDbType.CMANGOS))]
        [DBFieldName("ID", true, DbType = (TargetedDbType.TRINITY))]
        public uint? ID;

        [DBFieldName("text", DbType = (TargetedDbType.WPP | TargetedDbType.VMANGOS | TargetedDbType.CMANGOS))]
        [DBFieldName("Text", DbType = (TargetedDbType.TRINITY))]
        public string Text;

        [DBFieldName("next_page", DbType = (TargetedDbType.WPP | TargetedDbType.VMANGOS | TargetedDbType.CMANGOS))]
        [DBFieldName("NextPageID", DbType = (TargetedDbType.TRINITY))]
        public uint? NextPageID;

        [DBFieldName("player_condition_id", TargetedDbExpansion.BurningCrusadeClassic, TargetedDbExpansion.Zero, DbType = (TargetedDbType.WPP))]
        [DBFieldName("PlayerConditionID", TargetedDbExpansion.BurningCrusadeClassic, TargetedDbExpansion.Zero, DbType = (TargetedDbType.TRINITY))]
        [DBFieldName("player_condition_id", TargetedDbExpansion.Legion, DbType = (TargetedDbType.WPP))]
        [DBFieldName("PlayerConditionID", TargetedDbExpansion.Legion, DbType = (TargetedDbType.TRINITY))]
        public int? PlayerConditionID;

        [DBFieldName("flags", TargetedDbExpansion.BurningCrusadeClassic, TargetedDbExpansion.Zero, DbType = (TargetedDbType.WPP))]
        [DBFieldName("Flags", TargetedDbExpansion.BurningCrusadeClassic, TargetedDbExpansion.Zero, DbType = (TargetedDbType.TRINITY))]
        [DBFieldName("flags", TargetedDbExpansion.Legion, DbType = (TargetedDbType.WPP))]
        [DBFieldName("Flags", TargetedDbExpansion.Legion, DbType = (TargetedDbType.TRINITY))]
        public byte? Flags;

        [DBFieldName("sniff_build", DbType = (TargetedDbType.WPP))]
        [DBFieldName("VerifiedBuild", DbType = (TargetedDbType.TRINITY))]
        public int? VerifiedBuild = ClientVersion.BuildInt;
    }
}
