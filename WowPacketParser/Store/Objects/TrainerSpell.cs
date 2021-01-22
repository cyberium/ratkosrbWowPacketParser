using WowPacketParser.Misc;
using WowPacketParser.SQL;
using WowPacketParser.Enums;

namespace WowPacketParser.Store.Objects
{
    [DBTableName("trainer_spell")]
    public sealed class TrainerSpell : IDataModel
    {
        public uint[] ReqAbility;

        public void ConvertToDBStruct()
        {
            ReqAbility1 = ReqAbility[0];
            ReqAbility2 = ReqAbility[1];
            ReqAbility3 = ReqAbility[2];
        }

        [DBFieldName("trainer_id", true, DbType = (TargetedDbType.WPP))]
        [DBFieldName("TrainerId", true, DbType = (TargetedDbType.TRINITY))]
        public uint? TrainerId;

        [DBFieldName("spell_id", true, DbType = (TargetedDbType.WPP))]
        [DBFieldName("SpellId", true, DbType = (TargetedDbType.TRINITY))]
        public uint? SpellId;

        [DBFieldName("money_cost", DbType = (TargetedDbType.WPP))]
        [DBFieldName("MoneyCost", DbType = (TargetedDbType.TRINITY))]
        public uint? MoneyCost;

        [DBFieldName("required_skill_id", DbType = (TargetedDbType.WPP))]
        [DBFieldName("ReqSkillLine", DbType = (TargetedDbType.TRINITY))]
        public uint? ReqSkillLine;

        [DBFieldName("required_skill_value", DbType = (TargetedDbType.WPP))]
        [DBFieldName("ReqSkillRank", DbType = (TargetedDbType.TRINITY))]
        public uint? ReqSkillRank;

        [DBFieldName("required_ability1", DbType = (TargetedDbType.WPP))]
        [DBFieldName("ReqAbility1", DbType = (TargetedDbType.TRINITY))]
        public uint? ReqAbility1;

        [DBFieldName("required_ability2", DbType = (TargetedDbType.WPP))]
        [DBFieldName("ReqAbility2", DbType = (TargetedDbType.TRINITY))]
        public uint? ReqAbility2;

        [DBFieldName("required_ability3", DbType = (TargetedDbType.WPP))]
        [DBFieldName("ReqAbility3", DbType = (TargetedDbType.TRINITY))]
        public uint? ReqAbility3;

        [DBFieldName("required_level", DbType = (TargetedDbType.WPP))]
        [DBFieldName("ReqLevel", DbType = (TargetedDbType.TRINITY))]
        public byte? ReqLevel;

        [DBFieldName("sniff_build", DbType = (TargetedDbType.WPP))]
        [DBFieldName("VerifiedBuild", DbType = (TargetedDbType.TRINITY))]
        public int? VerifiedBuild = ClientVersion.BuildInt;

        public string FactionHelper;
    }
}
