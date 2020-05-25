using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.SQL;

namespace WowPacketParser.Store.Objects
{
    [DBTableName("creature_display_info_addon")]
    public sealed class ModelData : IDataModel
    {
        [DBFieldName("display_id", true)]
        public uint? DisplayID;

        [DBFieldName("bounding_radius")]
        public float? BoundingRadius;

        [DBFieldName("combat_reach")]
        public float? CombatReach;

        [DBFieldName("gender", TargetedDatabase.Classic, TargetedDatabase.WarlordsOfDraenor)]
        public Gender? Gender;

        [DBFieldName("VerifiedBuild", TargetedDatabase.Legion)]
        public int? VerifiedBuild = ClientVersion.BuildInt;
    }
}
