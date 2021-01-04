using WowPacketParser.Misc;
using WowPacketParser.SQL;
using WowPacketParser.Enums;

namespace WowPacketParser.Store.Objects
{
    public sealed class SpellTargetPosition : IDataModel
    {
        [DBFieldName("entry", true, DbType = (TargetedDbType.WPP))]
        [DBFieldName("ID", true, DbType = (TargetedDbType.TRINITY))]
        [DBFieldName("id", true, DbType = (TargetedDbType.VMANGOS | TargetedDbType.CMANGOS))]
        public uint? ID;

        [DBFieldName("effect_index", true, DbType = (TargetedDbType.WPP))]
        [DBFieldName("EffectIndex", true, DbType = (TargetedDbType.TRINITY))]
        public byte? EffectIndex;

        [DBFieldName("map", DbType = (TargetedDbType.WPP))]
        [DBFieldName("MapID", DbType = (TargetedDbType.TRINITY))]
        [DBFieldName("target_map", DbType = (TargetedDbType.VMANGOS | TargetedDbType.CMANGOS))]
        public ushort? MapID;

        [DBFieldName("position_x", DbType = (TargetedDbType.WPP))]
        [DBFieldName("PositionX", DbType = (TargetedDbType.TRINITY))]
        [DBFieldName("target_position_x", DbType = (TargetedDbType.VMANGOS | TargetedDbType.CMANGOS))]
        public float? PositionX;

        [DBFieldName("position_y", DbType = (TargetedDbType.WPP))]
        [DBFieldName("PositionY", DbType = (TargetedDbType.TRINITY))]
        [DBFieldName("target_position_y", DbType = (TargetedDbType.VMANGOS | TargetedDbType.CMANGOS))]
        public float? PositionY;

        [DBFieldName("position_z", DbType = (TargetedDbType.WPP))]
        [DBFieldName("PositionZ", DbType = (TargetedDbType.TRINITY))]
        [DBFieldName("target_position_z", DbType = (TargetedDbType.VMANGOS | TargetedDbType.CMANGOS))]
        public float? PositionZ;

        [DBFieldName("orientation", DbType = (TargetedDbType.WPP))]
        [DBFieldName("target_orientation", DbType = (TargetedDbType.VMANGOS | TargetedDbType.CMANGOS))]
        public float? Orientation;

        [DBFieldName("sniff_build", DbType = (TargetedDbType.WPP))]
        [DBFieldName("VerifiedBuild", DbType = (TargetedDbType.TRINITY))]
        public int? VerifiedBuild = ClientVersion.BuildInt;

        public string EffectHelper;
    }
}
