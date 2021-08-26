using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.SQL;

namespace WowPacketParser.Store.Objects
{
    [DBTableName("creature_display_info_addon", TargetedDbType.WPP | TargetedDbType.VMANGOS)]
    [DBTableName("creature_model_info", TargetedDbType.TRINITY | TargetedDbType.CMANGOS)]
    public sealed class ModelData : IDataModel
    {
        [DBFieldName("display_id", true, DbType = (TargetedDbType.WPP | TargetedDbType.VMANGOS))]
        [DBFieldName("DisplayID", true, DbType = (TargetedDbType.TRINITY))]
        [DBFieldName("modelid", true, DbType = (TargetedDbType.CMANGOS))]
        public uint? DisplayID;

        [DBFieldName("bounding_radius", DbType = (TargetedDbType.WPP | TargetedDbType.VMANGOS | TargetedDbType.CMANGOS))]
        [DBFieldName("BoundingRadius", DbType = (TargetedDbType.TRINITY))]
        public float? BoundingRadius;

        [DBFieldName("combat_reach", DbType = (TargetedDbType.WPP | TargetedDbType.VMANGOS | TargetedDbType.CMANGOS))]
        [DBFieldName("CombatReach", DbType = (TargetedDbType.TRINITY))]
        public float? CombatReach;

        [DBFieldName("speed_walk", DbType = (TargetedDbType.WPP))]
        [DBFieldName("SpeedWalk", DbType = (TargetedDbType.CMANGOS))]
        public float? SpeedWalk;

        [DBFieldName("speed_run", DbType = (TargetedDbType.WPP))]
        [DBFieldName("SpeedRun", DbType = (TargetedDbType.CMANGOS))]
        public float? SpeedRun;

        [DBFieldName("gender", TargetedDbExpansion.BurningCrusadeClassic, TargetedDbExpansion.WarlordsOfDraenor, DbType = (TargetedDbType.WPP | TargetedDbType.VMANGOS | TargetedDbType.CMANGOS))]
        [DBFieldName("Gender", TargetedDbExpansion.BurningCrusadeClassic, TargetedDbExpansion.WarlordsOfDraenor, DbType = (TargetedDbType.TRINITY))]
        public Gender? Gender;

        [DBFieldName("sniff_build", DbType = (TargetedDbType.WPP))]
        [DBFieldName("VerifiedBuild", TargetedDbExpansion.Legion, DbType = (TargetedDbType.TRINITY))]
        public int? VerifiedBuild = ClientVersion.BuildInt;
    }
}
