using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.SQL;

namespace WowPacketParser.Store.Objects
{
    [DBTableName("trainer")]
    public sealed class Trainer : IDataModel
    {
        [DBFieldName("id", true, DbType = (TargetedDbType.WPP))]
        [DBFieldName("Id", true, DbType = (TargetedDbType.TRINITY))]
        public uint? Id;

        [DBFieldName("type", DbType = (TargetedDbType.WPP))]
        [DBFieldName("Type", DbType = (TargetedDbType.TRINITY))]
        public TrainerType? Type;

        [DBFieldName("greeting", DbType = (TargetedDbType.WPP))]
        [DBFieldName("Greeting", DbType = (TargetedDbType.TRINITY))]
        public string Greeting;

        [DBFieldName("sniff_build", DbType = (TargetedDbType.WPP))]
        [DBFieldName("VerifiedBuild", DbType = (TargetedDbType.TRINITY))]
        public int? VerifiedBuild = ClientVersion.BuildInt;
    }
}
