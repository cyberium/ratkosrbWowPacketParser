using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.SQL;

namespace WowPacketParser.Store.Objects
{
    [DBTableName("creature_template_scaling")]
    public sealed class CreatureTemplateScaling : IDataModel
    {
        [DBFieldName("Entry", true)]
        public uint? Entry;

        [DBFieldName("DifficultyID", true)]
        public uint? DifficultyID;

        [DBFieldName("LevelScalingMin", TargetedDbExpansion.Legion, TargetedDbExpansion.Shadowlands)]
        public uint? LevelScalingMin;

        [DBFieldName("LevelScalingMax", TargetedDbExpansion.Legion, TargetedDbExpansion.Shadowlands)]
        public uint? LevelScalingMax;

        [DBFieldName("LevelScalingDeltaMin")]
        public int? LevelScalingDeltaMin;

        [DBFieldName("LevelScalingDeltaMax")]
        public int? LevelScalingDeltaMax;

        [DBFieldName("SandboxScalingID", TargetedDbExpansion.Legion, TargetedDbExpansion.BattleForAzeroth)]
        [DBFieldName("ContentTuningID", TargetedDbExpansion.BattleForAzeroth)]
        public int? ContentTuningID;

        [DBFieldName("VerifiedBuild")]
        public int? VerifiedBuild = ClientVersion.BuildInt;
    }
}
