using WowPacketParser.Enums;
using WowPacketParser.SQL;

namespace WowPacketParser.Store.Objects
{
    [DBTableName("creature_template_addon")]
    public sealed class CreatureTemplateAddon : IDataModel
    {
        [DBFieldName("entry", true)]
        public uint? Entry;

        [DBFieldName("mount", DbType = (TargetedDbType.TRINITY | TargetedDbType.CMANGOS))]
        public uint? MountID;

        [DBFieldName("bytes1", DbType = (TargetedDbType.TRINITY | TargetedDbType.CMANGOS))]
        public uint? Bytes1;

        [DBFieldName("bytes2", DbType = (TargetedDbType.TRINITY))]
        public uint? Bytes2;

        [DBFieldName("b2_0_sheath", DbType = (TargetedDbType.CMANGOS))]
        public uint? SheathState;

        [DBFieldName("b2_1_flags", TargetedDbExpansion.BurningCrusadeClassic, TargetedDbExpansion.WrathOfTheLichKing, DbType = (TargetedDbType.CMANGOS))]
        [DBFieldName("b2_1_pvp_state", TargetedDbExpansion.WrathOfTheLichKing, DbType = (TargetedDbType.CMANGOS))]
        public uint? PvpFlags;

        [DBFieldName("emote", DbType = (TargetedDbType.TRINITY | TargetedDbType.CMANGOS))]
        public uint? Emote;

        [DBFieldName("moveflags", DbType = (TargetedDbType.CMANGOS))]
        public uint? MoveFlags;

        [DBFieldName("aiAnimKit", TargetedDbExpansion.Legion, DbType = (TargetedDbType.TRINITY))]
        public ushort? AIAnimKit;

        [DBFieldName("movementAnimKit", TargetedDbExpansion.Legion, DbType = (TargetedDbType.TRINITY))]
        public ushort? MovementAnimKit;

        [DBFieldName("meleeAnimKit", TargetedDbExpansion.Legion, DbType = (TargetedDbType.TRINITY))]
        public ushort? MeleeAnimKit;

        // visibilityDistanceType exists in all database versions but because UnitFlags2 to detect the value from sniff doesn't exist in earlier client version
        // we pretend the field doesn't exist
        [DBFieldName("visibilityDistanceType", TargetedDbExpansion.WarlordsOfDraenor, DbType = (TargetedDbType.TRINITY))]
        public byte? VisibilityDistanceType;

        [DBFieldName("auras")]
        public string Auras;
    }
}
