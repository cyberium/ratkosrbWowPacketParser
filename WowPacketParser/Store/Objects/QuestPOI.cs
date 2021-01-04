using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.SQL;

namespace WowPacketParser.Store.Objects
{
    [DBTableName("quest_poi")]
    public sealed class QuestPOI : IDataModel
    {
        [DBFieldName("QuestID", true)]
        public int? QuestID;

        [DBFieldName("BlobIndex", TargetedDbExpansion.WarlordsOfDraenor, true)]
        public int? BlobIndex;

        [DBFieldName("id", true)]
        public int? ID;

        [DBFieldName("ObjectiveIndex")]
        public int? ObjectiveIndex;

        [DBFieldName("QuestObjectiveID", TargetedDbExpansion.WarlordsOfDraenor)]
        public int? QuestObjectiveID;

        [DBFieldName("QuestObjectID", TargetedDbExpansion.WarlordsOfDraenor)]
        public int? QuestObjectID;

        [DBFieldName("MapID")]
        public int? MapID;

        [DBFieldName("UiMapID", TargetedDbExpansion.BattleForAzeroth)]
        public int? UiMapID;

        [DBFieldName("WorldMapAreaId", TargetedDbExpansion.Zero, TargetedDbExpansion.BattleForAzeroth)]
        public int? WorldMapAreaId;

        [DBFieldName("Floor", TargetedDbExpansion.Zero, TargetedDbExpansion.BattleForAzeroth)]
        public int? Floor;

        [DBFieldName("Priority")]
        public int? Priority;

        [DBFieldName("Flags")]
        public int? Flags;

        [DBFieldName("WorldEffectID", TargetedDbExpansion.WarlordsOfDraenor)]
        public int? WorldEffectID;

        [DBFieldName("PlayerConditionID", TargetedDbExpansion.WarlordsOfDraenor)]
        public int? PlayerConditionID;

        [DBFieldName("NavigationPlayerConditionID", TargetedDbExpansion.Shadowlands)]
        public int? NavigationPlayerConditionID;

        [DBFieldName("SpawnTrackingID", TargetedDbExpansion.WarlordsOfDraenor)]
        public int? SpawnTrackingID;

        [DBFieldName("AlwaysAllowMergingBlobs", TargetedDbExpansion.Legion)]
        public bool? AlwaysAllowMergingBlobs;

        [DBFieldName("VerifiedBuild")]
        public int? VerifiedBuild = ClientVersion.BuildInt;
    }
}
