using WowPacketParser.Enums;
using WowPacketParser.SQL;

namespace WowPacketParser.Store.Objects
{
    public sealed class CreatureAddon : IDataModel
    {
        [DBFieldName("guid", true, noQuotes: true)]
        public string GUID;

        [DBFieldName("path_id", noQuotes: true, DbType = (TargetedDbType.TRINITY))]
        public string PathID;

        [DBFieldName("mount", DbType = (TargetedDbType.TRINITY | TargetedDbType.CMANGOS))]
        [DBFieldName("mount_display_id", DbType = (TargetedDbType.VMANGOS))]
        public uint? Mount;

        [DBFieldName("bytes1", DbType = (TargetedDbType.TRINITY | TargetedDbType.CMANGOS))]
        public uint? Bytes1;

        [DBFieldName("bytes2", DbType = (TargetedDbType.TRINITY))]
        public uint? Bytes2;

        [DBFieldName("sheath_state", DbType = (TargetedDbType.VMANGOS))]
        [DBFieldName("b2_0_sheath", DbType = (TargetedDbType.CMANGOS))]
        public uint? SheathState;

        [DBFieldName("b2_1_flags", TargetedDbExpansion.BurningCrusadeClassic, TargetedDbExpansion.WrathOfTheLichKing, DbType = (TargetedDbType.CMANGOS))]
        [DBFieldName("b2_1_pvp_state", TargetedDbExpansion.WrathOfTheLichKing, DbType = (TargetedDbType.CMANGOS))]
        public uint? PvpFlags;

        [DBFieldName("emote", DbType = (TargetedDbType.TRINITY | TargetedDbType.CMANGOS))]
        [DBFieldName("emote_state", DbType = (TargetedDbType.VMANGOS))]
        public uint? Emote;

        [DBFieldName("moveflags", DbType = (TargetedDbType.CMANGOS))]
        public uint? MoveFlags;

        [DBFieldName("aiAnimKit", TargetedDbExpansion.Legion, DbType = (TargetedDbType.TRINITY))]
        public ushort? AIAnimKit;

        [DBFieldName("movementAnimKit", TargetedDbExpansion.Legion, DbType = (TargetedDbType.TRINITY))]
        public ushort? MovementAnimKit;

        [DBFieldName("meleeAnimKit", TargetedDbExpansion.Legion, DbType = (TargetedDbType.TRINITY))]
        public ushort? MeleeAnimKit;

        [DBFieldName("auras")]
        public string Auras;
    }
}
