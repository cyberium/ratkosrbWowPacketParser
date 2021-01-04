using WowPacketParser.SQL;
using WowPacketParser.Enums;
using WowPacketParser.Misc;

namespace WowPacketParser.Store.Objects
{
    public sealed class NpcTrainer : IDataModel
    {
        [DBFieldName("entry", true, DbType = (TargetedDbType.WPP | TargetedDbType.VMANGOS | TargetedDbType.CMANGOS))]
        [DBFieldName("ID", true, DbType = (TargetedDbType.TRINITY))]
        public uint? ID;

        [DBFieldName("spell", true, DbType = (TargetedDbType.WPP | TargetedDbType.VMANGOS | TargetedDbType.CMANGOS))]
        [DBFieldName("SpellID", true, DbType = (TargetedDbType.TRINITY))]
        public int? SpellID;

        [DBFieldName("spellcost", DbType = (TargetedDbType.WPP | TargetedDbType.VMANGOS | TargetedDbType.CMANGOS))]
        [DBFieldName("MoneyCost", DbType = (TargetedDbType.TRINITY))]
        public uint? MoneyCost;

        [DBFieldName("reqskill", DbType = (TargetedDbType.WPP | TargetedDbType.VMANGOS | TargetedDbType.CMANGOS))]
        [DBFieldName("ReqSkillLine", DbType = (TargetedDbType.TRINITY))]
        public uint? ReqSkillLine;

        [DBFieldName("reqskillvalue", DbType = (TargetedDbType.WPP | TargetedDbType.VMANGOS | TargetedDbType.CMANGOS))]
        [DBFieldName("ReqSkillRank", DbType = (TargetedDbType.TRINITY))]
        public uint? ReqSkillRank;

        [DBFieldName("reqlevel", DbType = (TargetedDbType.WPP | TargetedDbType.VMANGOS | TargetedDbType.CMANGOS))]
        [DBFieldName("ReqLevel", DbType = (TargetedDbType.TRINITY))]
        public uint? ReqLevel;

        [DBFieldName("sniff_build", DbType = (TargetedDbType.WPP))]
        [DBFieldName("VerifiedBuild", DbType = (TargetedDbType.TRINITY))]
        public int? VerifiedBuild = ClientVersion.BuildInt;
    }
}
