using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.SQL;

namespace WowPacketParser.Store.Objects
{
    [DBTableName("quest_template")]
    public sealed class QuestTemplate : IDataModel
    {
        [DBFieldName("ID", true)]
        public uint? ID;

        [DBFieldName("QuestType")]
        public QuestType? QuestType;

        [DBFieldName("QuestLevel", TargetedDbExpansion.Classic, TargetedDbExpansion.Shadowlands)]
        public int? QuestLevel;

        [DBFieldName("ScalingFactionGroup", TargetedDbExpansion.Classic, TargetedDbExpansion.Zero)]
        [DBFieldName("ScalingFactionGroup", TargetedDbExpansion.BattleForAzeroth, TargetedDbExpansion.Shadowlands)]
        public int? QuestScalingFactionGroup;

        [DBFieldName("MaxScalingLevel", TargetedDbExpansion.Classic, TargetedDbExpansion.Zero)]
        [DBFieldName("MaxScalingLevel", TargetedDbExpansion.Legion, TargetedDbExpansion.Shadowlands)]
        public int? QuestMaxScalingLevel;

        [DBFieldName("QuestPackageID", TargetedDbExpansion.Classic, TargetedDbExpansion.Zero)]
        [DBFieldName("QuestPackageID", TargetedDbExpansion.WarlordsOfDraenor)]
        public uint? QuestPackageID;

        [DBFieldName("ContentTuningID", TargetedDbExpansion.Shadowlands)]
        public int? ContentTuningID;

        [DBFieldName("MinLevel", TargetedDbExpansion.Classic, TargetedDbExpansion.Shadowlands)]
        public int? MinLevel;

        [DBFieldName("MaxLevel", TargetedDbExpansion.Cataclysm, TargetedDbExpansion.WarlordsOfDraenor)]
        public uint? MaxLevel;

        [DBFieldName("QuestSortID")]
        public QuestSort? QuestSortID;

        [DBFieldName("QuestInfoID")]
        public QuestInfo? QuestInfoID;

        [DBFieldName("SuggestedGroupNum")]
        public uint? SuggestedGroupNum;

        [DBFieldName("RequiredClasses", TargetedDbExpansion.Cataclysm, TargetedDbExpansion.WarlordsOfDraenor)]
        public uint? RequiredClasses;

        [DBFieldName("RequiredSkillId", TargetedDbExpansion.Cataclysm, TargetedDbExpansion.WarlordsOfDraenor)]
        public uint? RequriedSkillID;

        [DBFieldName("RequiredSkillPoints", TargetedDbExpansion.Cataclysm, TargetedDbExpansion.WarlordsOfDraenor)]
        public uint? RequiredSkillPoints;

        [DBFieldName("RequiredFactionId", TargetedDbExpansion.Zero, TargetedDbExpansion.WarlordsOfDraenor, 2)]
        public uint?[] RequiredFactionID;

        [DBFieldName("RequiredFactionValue",TargetedDbExpansion.Zero, TargetedDbExpansion.WarlordsOfDraenor, 2)]
        public int?[] RequiredFactionValue;

        [DBFieldName("RewardNextQuest")]
        public uint? RewardNextQuest;

        [DBFieldName("RewardXPDifficulty")]
        public uint? RewardXPDifficulty;

        [DBFieldName("RewardXPMultiplier", TargetedDbExpansion.Classic, TargetedDbExpansion.Zero)]
        [DBFieldName("RewardXPMultiplier", TargetedDbExpansion.WarlordsOfDraenor)]
        public float? RewardXPMultiplier;

        [DBFieldName("RewardMoney")]
        public int? RewardMoney;

        [DBFieldName("RewardMoneyDifficulty", TargetedDbExpansion.Classic, TargetedDbExpansion.Zero)]
        [DBFieldName("RewardMoneyDifficulty", TargetedDbExpansion.WarlordsOfDraenor)]
        public uint? RewardMoneyDifficulty;

        [DBFieldName("RewardMoneyMultiplier", TargetedDbExpansion.Classic, TargetedDbExpansion.Zero)]
        [DBFieldName("RewardMoneyMultiplier", TargetedDbExpansion.WarlordsOfDraenor)]
        public float? RewardMoneyMultiplier;

        [DBFieldName("RewardBonusMoney")]
        public uint? RewardBonusMoney;

        [DBFieldName("RewardDisplaySpell", TargetedDbExpansion.Zero, TargetedDbExpansion.WarlordsOfDraenor)]
        public uint? RewardDisplaySpell;

        [DBFieldName("RewardDisplaySpell", TargetedDbExpansion.Classic, TargetedDbExpansion.Zero, 3)]
        [DBFieldName("RewardDisplaySpell", TargetedDbExpansion.Legion, TargetedDbExpansion.Shadowlands, 3)]
        public uint?[] RewardDisplaySpellLegion;

        [DBFieldName("RewardSpell", TargetedDbExpansion.Zero, TargetedDbExpansion.Cataclysm)]
        public int? RewardSpell;

        [DBFieldName("RewardSpell", TargetedDbExpansion.Classic, TargetedDbExpansion.Zero)]
        [DBFieldName("RewardSpell", TargetedDbExpansion.WarlordsOfDraenor)]
        public uint? RewardSpellWod;

        [DBFieldName("RequiredMinRepFaction", TargetedDbExpansion.Cataclysm, TargetedDbExpansion.WarlordsOfDraenor)]
        public uint? RequiredMinRepFaction;

        [DBFieldName("RequiredMaxRepFaction", TargetedDbExpansion.Cataclysm, TargetedDbExpansion.WarlordsOfDraenor)]
        public uint? RequiredMaxRepFaction;

        [DBFieldName("RequiredMinRepValue", TargetedDbExpansion.Cataclysm, TargetedDbExpansion.WarlordsOfDraenor)]
        public int? RequiredMinRepValue;

        [DBFieldName("RequiredMaxRepValue", TargetedDbExpansion.Cataclysm, TargetedDbExpansion.WarlordsOfDraenor)]
        public int? RequiredMaxRepValue;

        [DBFieldName("PrevQuestId", TargetedDbExpansion.Cataclysm, TargetedDbExpansion.WarlordsOfDraenor)]
        public int? PrevQuestID;

        [DBFieldName("NextQuestId", TargetedDbExpansion.Cataclysm, TargetedDbExpansion.WarlordsOfDraenor)]
        public int? NextQuestID;

        [DBFieldName("ExclusiveGroup", TargetedDbExpansion.Cataclysm, TargetedDbExpansion.WarlordsOfDraenor)]
        public int? ExclusiveGroup;

        [DBFieldName("RewardHonor", TargetedDbExpansion.Zero, TargetedDbExpansion.WarlordsOfDraenor)]
        public int? RewardHonor;

        [DBFieldName("RewardHonor", TargetedDbExpansion.Classic, TargetedDbExpansion.Zero)]
        [DBFieldName("RewardHonor", TargetedDbExpansion.WarlordsOfDraenor)]
        public uint? RewardHonorWod;

        [DBFieldName("RewardKillHonor")]
        public float? RewardKillHonor;

        [DBFieldName("StartItem")]
        public uint? StartItem;

        [DBFieldName("RewardArtifactXPDifficulty", TargetedDbExpansion.Classic, TargetedDbExpansion.Zero)]
        [DBFieldName("RewardArtifactXPDifficulty", TargetedDbExpansion.Legion)]
        public uint? RewardArtifactXPDifficulty;

        [DBFieldName("RewardArtifactXPMultiplier", TargetedDbExpansion.Classic, TargetedDbExpansion.Zero)]
        [DBFieldName("RewardArtifactXPMultiplier", TargetedDbExpansion.Legion)]
        public float? RewardArtifactXPMultiplier;

        [DBFieldName("RewardArtifactCategoryID", TargetedDbExpansion.Classic, TargetedDbExpansion.Zero)]
        [DBFieldName("RewardArtifactCategoryID", TargetedDbExpansion.Legion)]
        public uint? RewardArtifactCategoryID;

        [DBFieldName("RewardMailTemplateId", TargetedDbExpansion.Cataclysm, TargetedDbExpansion.WarlordsOfDraenor)]
        public uint? RewardMailTemplateID;

        [DBFieldName("RewardMailDelay", TargetedDbExpansion.Cataclysm, TargetedDbExpansion.WarlordsOfDraenor)]
        public int? RewardMailDelay;

        [DBFieldName("SourceItemCount", TargetedDbExpansion.Cataclysm, TargetedDbExpansion.WarlordsOfDraenor)]
        public uint? SourceItemCount;

        [DBFieldName("SourceSpellId", TargetedDbExpansion.Cataclysm, TargetedDbExpansion.WarlordsOfDraenor)]
        public uint? SourceSpellID;

        [DBFieldName("Flags")]
        public QuestFlags? Flags;

        [DBFieldName("FlagsEx", TargetedDbExpansion.Classic, TargetedDbExpansion.Zero)]
        [DBFieldName("SpecialFlags", TargetedDbExpansion.Cataclysm, TargetedDbExpansion.WarlordsOfDraenor)]
        [DBFieldName("FlagsEx", TargetedDbExpansion.WarlordsOfDraenor)]
        public QuestFlagsEx? FlagsEx;

        [DBFieldName("FlagsEx2", TargetedDbExpansion.Classic, TargetedDbExpansion.Zero)]
        [DBFieldName("FlagsEx2", TargetedDbExpansion.BattleForAzeroth)]
        public QuestFlagsEx2? FlagsEx2;

        [DBFieldName("MinimapTargetMark", TargetedDbExpansion.Cataclysm, TargetedDbExpansion.WarlordsOfDraenor)]
        public uint? MinimapTargetMark;

        [DBFieldName("RequiredPlayerKills", TargetedDbExpansion.Zero, TargetedDbExpansion.WarlordsOfDraenor)]
        public uint? RequiredPlayerKills;

        [DBFieldName("RewardSkillLineID", TargetedDbExpansion.Classic, TargetedDbExpansion.Zero)]
        [DBFieldName("RewardSkillId", TargetedDbExpansion.Cataclysm, TargetedDbExpansion.WarlordsOfDraenor)]
        [DBFieldName("RewardSkillLineID", TargetedDbExpansion.WarlordsOfDraenor)]
        public uint? RewardSkillLineID;

        [DBFieldName("RewardNumSkillUps", TargetedDbExpansion.Classic, TargetedDbExpansion.Zero)]
        [DBFieldName("RewardSkillPoints", TargetedDbExpansion.Cataclysm, TargetedDbExpansion.WarlordsOfDraenor)]
        [DBFieldName("RewardNumSkillUps", TargetedDbExpansion.WarlordsOfDraenor)]
        public uint? RewardNumSkillUps;

        [DBFieldName("RewardReputationMask", TargetedDbExpansion.Cataclysm, TargetedDbExpansion.WarlordsOfDraenor)]
        public uint? RewardReputationMask;

        [DBFieldName("PortraitGiver", TargetedDbExpansion.Classic, TargetedDbExpansion.Zero)]
        [DBFieldName("QuestGiverPortrait", TargetedDbExpansion.Cataclysm, TargetedDbExpansion.WarlordsOfDraenor)]
        [DBFieldName("PortraitGiver", TargetedDbExpansion.WarlordsOfDraenor)]
        public uint? QuestGiverPortrait;

        [DBFieldName("PortraitGiverMount", TargetedDbExpansion.Classic, TargetedDbExpansion.Zero)]
        [DBFieldName("PortraitGiverMount", TargetedDbExpansion.BattleForAzeroth)]
        public uint? PortraitGiverMount;

        [DBFieldName("PortraitTurnIn", TargetedDbExpansion.Classic, TargetedDbExpansion.Zero)]
        [DBFieldName("QuestTurnInPortrait", TargetedDbExpansion.Cataclysm, TargetedDbExpansion.WarlordsOfDraenor)]
        [DBFieldName("PortraitTurnIn", TargetedDbExpansion.WarlordsOfDraenor)]
        public uint? QuestTurnInPortrait;

        [DBFieldName("RewardItem", 4)]
        public uint?[] RewardItem;

        [DBFieldName("RewardAmount", 4)]
        public uint?[] RewardAmount;

        [DBFieldName("ItemDrop", 4)]
        public uint?[] ItemDrop;

        [DBFieldName("ItemDropQuantity", 4)]
        public uint?[] ItemDropQuantity;

        [DBFieldName("RewardChoiceItemID", 6)]
        public uint?[] RewardChoiceItemID;

        [DBFieldName("RewardChoiceItemQuantity", 6)]
        public uint?[] RewardChoiceItemQuantity;

        [DBFieldName("RewardChoiceItemDisplayID", TargetedDbExpansion.Classic, TargetedDbExpansion.Zero, 6)]
        [DBFieldName("RewardChoiceItemDisplayID", TargetedDbExpansion.WarlordsOfDraenor, 6)]
        public uint?[] RewardChoiceItemDisplayID;

        [DBFieldName("POIContinent")]
        public uint? POIContinent;

        [DBFieldName("POIx")]
        public float? POIx;

        [DBFieldName("POIy")]
        public float? POIy;

        [DBFieldName("POIPriority", TargetedDbExpansion.Zero, TargetedDbExpansion.WarlordsOfDraenor)]
        public uint? POIPriority;

        [DBFieldName("POIPriority", TargetedDbExpansion.Classic, TargetedDbExpansion.Zero)]
        [DBFieldName("POIPriority", TargetedDbExpansion.WarlordsOfDraenor)]
        public int? POIPriorityWod;

        [DBFieldName("RewardTitle")]
        public uint? RewardTitle;

        [DBFieldName("RewardTalents", TargetedDbExpansion.Zero, TargetedDbExpansion.WarlordsOfDraenor)]
        public uint? RewardTalents;

        [DBFieldName("RewardArenaPoints")]
        public uint? RewardArenaPoints;

        [DBFieldName("RewardFactionID", 5)]
        public uint?[] RewardFactionID;

        [DBFieldName("RewardFactionValue", 5)]
        public int?[] RewardFactionValue;

        [DBFieldName("RewardFactionCapIn", TargetedDbExpansion.Classic, TargetedDbExpansion.Zero, 5)]
        [DBFieldName("RewardFactionCapIn", TargetedDbExpansion.Legion, 5)]
        public int?[] RewardFactionCapIn;

        [DBFieldName("RewardFactionOverride", 5)]
        public int?[] RewardFactionOverride;

        [DBFieldName("RewardFactionFlags", TargetedDbExpansion.Classic, TargetedDbExpansion.Zero)]
        [DBFieldName("RewardFactionFlags", TargetedDbExpansion.WarlordsOfDraenor)]
        public uint? RewardFactionFlags;

        [DBFieldName("AreaGroupID", TargetedDbExpansion.Classic, TargetedDbExpansion.Zero)]
        [DBFieldName("AreaGroupID", TargetedDbExpansion.WarlordsOfDraenor)]
        public uint? AreaGroupID;

        [DBFieldName("TimeAllowed")]
        public uint? TimeAllowed;

        [DBFieldName("AllowableRaces", TargetedDbExpansion.Zero, TargetedDbExpansion.WarlordsOfDraenor)]
        public RaceMask? AllowableRaces;

        [DBFieldName("AllowableRaces", TargetedDbExpansion.Classic, TargetedDbExpansion.Zero)]
        [DBFieldName("AllowableRaces", TargetedDbExpansion.WarlordsOfDraenor)]
        public ulong? AllowableRacesWod;

        [DBFieldName("TreasurePickerID", TargetedDbExpansion.Classic, TargetedDbExpansion.Zero)]
        [DBFieldName("QuestRewardID", TargetedDbExpansion.Legion, TargetedDbExpansion.BattleForAzeroth)]
        [DBFieldName("TreasurePickerID", TargetedDbExpansion.BattleForAzeroth)]
        public int? QuestRewardID;

        [DBFieldName("Expansion", TargetedDbExpansion.Classic, TargetedDbExpansion.Zero)]
        [DBFieldName("Expansion", TargetedDbExpansion.Legion)]
        public int? Expansion;

        [DBFieldName("ManagedWorldStateID", TargetedDbExpansion.BattleForAzeroth)]
        public int? ManagedWorldStateID;

        [DBFieldName("QuestSessionBonus", TargetedDbExpansion.BattleForAzeroth)]
        public int? QuestSessionBonus;

        [DBFieldName("LogTitle", LocaleConstant.enUS)]
        public string LogTitle;

        [DBFieldName("LogDescription", LocaleConstant.enUS)]
        public string LogDescription;

        [DBFieldName("QuestDescription", LocaleConstant.enUS)]
        public string QuestDescription;

        [DBFieldName("AreaDescription", LocaleConstant.enUS)]
        public string AreaDescription;

        [DBFieldName("QuestCompletionLog", LocaleConstant.enUS)]
        public string QuestCompletionLog;

        [DBFieldName("RequiredNpcOrGo", TargetedDbExpansion.Zero, TargetedDbExpansion.WarlordsOfDraenor, 4)]
        public int?[] RequiredNpcOrGo;

        [DBFieldName("RequiredNpcOrGoCount", TargetedDbExpansion.Zero, TargetedDbExpansion.WarlordsOfDraenor, 4)]
        public uint?[] RequiredNpcOrGoCount;

        [DBFieldName("RequiredItemId", TargetedDbExpansion.Zero, TargetedDbExpansion.WarlordsOfDraenor, 6)]
        public uint?[] RequiredItemID;

        [DBFieldName("RequiredItemCount", TargetedDbExpansion.Zero, TargetedDbExpansion.WarlordsOfDraenor, 6)]
        public uint?[] RequiredItemCount;

        [DBFieldName("Unknown0", TargetedDbExpansion.Zero, TargetedDbExpansion.Cataclysm)]
        public uint? Unk0;

        [DBFieldName("RequiredSpell", TargetedDbExpansion.Cataclysm, TargetedDbExpansion.WarlordsOfDraenor)]
        public uint? RequiredSpell;

        [DBFieldName("ObjectiveText", TargetedDbExpansion.Zero, TargetedDbExpansion.WarlordsOfDraenor, 4)]
        public string[] ObjectiveText;

        [DBFieldName("RewardCurrencyID", TargetedDbExpansion.Classic, TargetedDbExpansion.Zero, 4)]
        [DBFieldName("RewardCurrencyId", TargetedDbExpansion.Cataclysm, TargetedDbExpansion.WarlordsOfDraenor, 4)]
        [DBFieldName("RewardCurrencyID", TargetedDbExpansion.WarlordsOfDraenor, 4)]
        public uint?[] RewardCurrencyID;

        [DBFieldName("RewardCurrencyQty", TargetedDbExpansion.Classic, TargetedDbExpansion.Zero, 4)]
        [DBFieldName("RewardCurrencyCount", TargetedDbExpansion.Cataclysm, TargetedDbExpansion.WarlordsOfDraenor, 4)]
        [DBFieldName("RewardCurrencyQty", TargetedDbExpansion.WarlordsOfDraenor, 4)]
        public uint?[] RewardCurrencyCount;

        [DBFieldName("RequiredCurrencyId", TargetedDbExpansion.Cataclysm, TargetedDbExpansion.WarlordsOfDraenor, 4)]
        public uint?[] RequiredCurrencyID;

        [DBFieldName("RequiredCurrencyCount", TargetedDbExpansion.Cataclysm, TargetedDbExpansion.WarlordsOfDraenor, 4)]
        public uint?[] RequiredCurrencyCount;

        [DBFieldName("PortraitGiverText", TargetedDbExpansion.Classic, TargetedDbExpansion.Zero, LocaleConstant.enUS)]
        [DBFieldName("QuestGiverTextWindow", TargetedDbExpansion.Cataclysm, TargetedDbExpansion.WarlordsOfDraenor)]
        [DBFieldName("PortraitGiverText", TargetedDbExpansion.WarlordsOfDraenor, LocaleConstant.enUS)]
        public string QuestGiverTextWindow;

        [DBFieldName("PortraitGiverName", TargetedDbExpansion.Classic, TargetedDbExpansion.Zero, LocaleConstant.enUS)]
        [DBFieldName("QuestGiverTargetName", TargetedDbExpansion.Cataclysm, TargetedDbExpansion.WarlordsOfDraenor)]
        [DBFieldName("PortraitGiverName", TargetedDbExpansion.WarlordsOfDraenor, LocaleConstant.enUS)]
        public string QuestGiverTargetName;

        [DBFieldName("PortraitTurnInText", TargetedDbExpansion.Classic, TargetedDbExpansion.Zero, LocaleConstant.enUS)]
        [DBFieldName("QuestTurnTextWindow", TargetedDbExpansion.Cataclysm, TargetedDbExpansion.WarlordsOfDraenor)]
        [DBFieldName("PortraitTurnInText", TargetedDbExpansion.WarlordsOfDraenor, LocaleConstant.enUS)]
        public string QuestTurnTextWindow;

        [DBFieldName("PortraitTurnInName", TargetedDbExpansion.Classic, TargetedDbExpansion.Zero, LocaleConstant.enUS)]
        [DBFieldName("QuestTurnTargetName", TargetedDbExpansion.Cataclysm, TargetedDbExpansion.WarlordsOfDraenor)]
        [DBFieldName("PortraitTurnInName", TargetedDbExpansion.WarlordsOfDraenor, LocaleConstant.enUS)]
        public string QuestTurnTargetName;

        [DBFieldName("AcceptedSoundKitID", TargetedDbExpansion.Classic, TargetedDbExpansion.Zero)]
        [DBFieldName("SoundAccept", TargetedDbExpansion.Cataclysm, TargetedDbExpansion.WarlordsOfDraenor)]
        [DBFieldName("AcceptedSoundKitID", TargetedDbExpansion.WarlordsOfDraenor)]
        public uint? SoundAccept;

        [DBFieldName("CompleteSoundKitID", TargetedDbExpansion.Classic, TargetedDbExpansion.Zero)]
        [DBFieldName("SoundTurnIn", TargetedDbExpansion.Cataclysm, TargetedDbExpansion.WarlordsOfDraenor)]
        [DBFieldName("CompleteSoundKitID", TargetedDbExpansion.WarlordsOfDraenor)]
        public uint? SoundTurnIn;

        [DBFieldName("VerifiedBuild")]
        public int? VerifiedBuild = ClientVersion.BuildInt;
    }

    [DBTableName("quest_starter")]
    public sealed class QuestStarter : IDataModel
    {
        [DBFieldName("object_id", true)]
        public uint? ObjectId;

        [DBFieldName("object_type", true)]
        public string ObjectType;

        [DBFieldName("quest_id", true)]
        public uint? QuestId;
    }

    [DBTableName("quest_ender")]
    public sealed class QuestEnder : IDataModel
    {
        [DBFieldName("object_id", true)]
        public uint? ObjectId;

        [DBFieldName("object_type", true)]
        public string ObjectType;

        [DBFieldName("quest_id", true)]
        public uint? QuestId;
    }

    [DBTableName("quest_update_complete")]
    public sealed class QuestCompleteTime : IDataModel
    {
        [DBFieldName("unixtimems", true)]
        public ulong UnixTimeMs;

        [DBFieldName("quest_id", true)]
        public uint QuestId;
    }

    [DBTableName("quest_update_failed")]
    public sealed class QuestFailTime : IDataModel
    {
        [DBFieldName("unixtimems", true)]
        public ulong UnixTimeMs;

        [DBFieldName("quest_id", true)]
        public uint QuestId;
    }
}
