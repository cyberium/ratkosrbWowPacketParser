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

        [DBFieldName("spell_id", true, DbType = (TargetedDbType.WPP))]
        [DBFieldName("spell", true, DbType = (TargetedDbType.VMANGOS | TargetedDbType.CMANGOS))]
        [DBFieldName("SpellID", true, DbType = (TargetedDbType.TRINITY))]
        public int? SpellID;

        [DBFieldName("money_cost", true, DbType = (TargetedDbType.WPP))]
        [DBFieldName("spellcost", true, DbType = (TargetedDbType.VMANGOS | TargetedDbType.CMANGOS))]
        [DBFieldName("MoneyCost", true, DbType = (TargetedDbType.TRINITY))]
        public uint? MoneyCost;

        [DBFieldName("required_skill_id", DbType = (TargetedDbType.WPP))]
        [DBFieldName("reqskill", DbType = (TargetedDbType.VMANGOS | TargetedDbType.CMANGOS))]
        [DBFieldName("ReqSkillLine", DbType = (TargetedDbType.TRINITY))]
        public uint? ReqSkillLine;

        [DBFieldName("required_skill_value", DbType = (TargetedDbType.WPP))]
        [DBFieldName("reqskillvalue", DbType = (TargetedDbType.VMANGOS | TargetedDbType.CMANGOS))]
        [DBFieldName("ReqSkillRank", DbType = (TargetedDbType.TRINITY))]
        public uint? ReqSkillRank;

        [DBFieldName("required_level", DbType = (TargetedDbType.WPP))]
        [DBFieldName("reqlevel", DbType = (TargetedDbType.VMANGOS | TargetedDbType.CMANGOS))]
        [DBFieldName("ReqLevel", DbType = (TargetedDbType.TRINITY))]
        public uint? ReqLevel;

        [DBFieldName("sniff_build", DbType = (TargetedDbType.WPP))]
        [DBFieldName("VerifiedBuild", DbType = (TargetedDbType.TRINITY))]
        public int? VerifiedBuild = ClientVersion.BuildInt;
    }
}
