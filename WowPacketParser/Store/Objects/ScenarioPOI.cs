using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.SQL;

namespace WowPacketParser.Store.Objects
{
    [DBTableName("scenario_poi")]
    public sealed class ScenarioPOI : IDataModel
    {
        [DBFieldName("CriteriaTreeID", true)]
        public int? CriteriaTreeID;

        [DBFieldName("BlobIndex", true)]
        public int? BlobIndex;

        [DBFieldName("Idx1", true)]
        public uint? Idx1;

        [DBFieldName("MapID")]
        public int? MapID;

        [DBFieldName("WorldMapAreaId", TargetedDbExpansion.Zero, TargetedDbExpansion.BattleForAzeroth)]
        [DBFieldName("UiMapID", TargetedDbExpansion.BattleForAzeroth)]
        public int? WorldMapAreaId;

        [DBFieldName("Floor", TargetedDbExpansion.Zero, TargetedDbExpansion.BattleForAzeroth)]
        public int? Floor;

        [DBFieldName("Priority")]
        public int? Priority;

        [DBFieldName("Flags")]
        public int? Flags;

        [DBFieldName("WorldEffectID")]
        public int? WorldEffectID;

        [DBFieldName("PlayerConditionID")]
        public int? PlayerConditionID;

        [DBFieldName("VerifiedBuild")]
        public int? VerifiedBuild = ClientVersion.BuildInt;
    }
}