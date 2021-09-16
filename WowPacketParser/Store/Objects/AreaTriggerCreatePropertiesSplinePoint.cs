using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.SQL;

namespace WowPacketParser.Store.Objects
{
    [DBTableName("areatrigger_create_properties_spline_point")]
    public sealed class AreaTriggerCreatePropertiesSplinePoint : IDataModel
    {
        [DBFieldName("SpellMiscId", TargetedDbExpansion.Zero, TargetedDbExpansion.Shadowlands, true)]
        [DBFieldName("AreaTriggerCreatePropertiesId", TargetedDbExpansion.Shadowlands, true)]
        public uint? AreaTriggerCreatePropertiesId;

        [DBFieldName("Idx", true)]
        public uint? Idx;

        [DBFieldName("X")]
        public float? X;

        [DBFieldName("Y")]
        public float? Y;

        [DBFieldName("Z")]
        public float? Z;

        [DBFieldName("VerifiedBuild")]
        public int? VerifiedBuild = ClientVersion.BuildInt;

        // Will be inserted as comment
        public uint spellId = 0;

        public WowGuid areatriggerGuid;
    }
}
