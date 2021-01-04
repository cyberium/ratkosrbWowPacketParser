using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.SQL;
using WowPacketParser.Enums;

namespace WowPacketParser.Store.Objects
{
    [DBTableName("points_of_interest")]
    public sealed class PointsOfInterest : IDataModel
    {
        [DBFieldName("entry", true, true, DbType = (TargetedDbType.WPP | TargetedDbType.VMANGOS | TargetedDbType.CMANGOS))]
        [DBFieldName("ID", true, true, DbType = (TargetedDbType.TRINITY))]
        public object ID;

        [DBFieldName("x", DbType = (TargetedDbType.WPP | TargetedDbType.VMANGOS | TargetedDbType.CMANGOS))]
        [DBFieldName("PositionX", DbType = (TargetedDbType.TRINITY))]
        public float? PositionX;

        [DBFieldName("y", DbType = (TargetedDbType.WPP | TargetedDbType.VMANGOS | TargetedDbType.CMANGOS))]
        [DBFieldName("PositionY", DbType = (TargetedDbType.TRINITY))]
        public float? PositionY;

        [DBFieldName("icon", DbType = (TargetedDbType.WPP | TargetedDbType.VMANGOS | TargetedDbType.CMANGOS))]
        [DBFieldName("Icon", DbType = (TargetedDbType.TRINITY))]
        public GossipPOIIcon? Icon;

        [DBFieldName("flags", DbType = (TargetedDbType.WPP | TargetedDbType.VMANGOS | TargetedDbType.CMANGOS))]
        [DBFieldName("Flags", DbType = (TargetedDbType.TRINITY))]
        public uint? Flags;

        [DBFieldName("data", DbType = (TargetedDbType.WPP | TargetedDbType.VMANGOS | TargetedDbType.CMANGOS))]
        [DBFieldName("Importance", DbType = (TargetedDbType.TRINITY))]
        public uint? Importance;

        [DBFieldName("icon_name", DbType = (TargetedDbType.WPP | TargetedDbType.VMANGOS | TargetedDbType.CMANGOS))]
        [DBFieldName("Name", DbType = (TargetedDbType.TRINITY))]
        public string Name;

        [DBFieldName("sniff_build", DbType = (TargetedDbType.WPP))]
        [DBFieldName("VerifiedBuild", DbType = (TargetedDbType.TRINITY))]
        public int? VerifiedBuild = ClientVersion.BuildInt;
    }
}
