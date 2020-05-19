using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.SQL;

namespace WowPacketParser.Store.Objects
{
    [DBTableName("creature_template_wdb")]
    public sealed class CreatureTemplate : IDataModel
    {
        [DBFieldName("entry", true)]
        public uint? Entry;

        [DBFieldName("KillCredit", 2)]
        public uint?[] KillCredits;

        [DBFieldName("display_id", TargetedDatabase.Zero, TargetedDatabase.BattleForAzeroth, 4)]
        public uint?[] ModelIDs;

        [DBFieldName("name")]
        public string Name;

        [DBFieldName("femaleName", TargetedDatabase.Cataclysm)]
        public string FemaleName;

        [DBFieldName("subname", nullable: true)]
        public string SubName;

        [DBFieldName("TitleAlt", TargetedDatabase.WarlordsOfDraenor /*Mists of Pandaria*/, nullable: true)]
        public string TitleAlt;

        [DBFieldName("IconName", nullable: true)]
        public string IconName;

        [DBFieldName("HealthScalingExpansion", TargetedDatabase.WarlordsOfDraenor)]
        public ClientType? HealthScalingExpansion;

        [DBFieldName("RequiredExpansion", TargetedDatabase.Cataclysm)]
        public ClientType? RequiredExpansion;

        [DBFieldName("VignetteID", TargetedDatabase.Legion)]
        public uint? VignetteID;

        [DBFieldName("unit_class", TargetedDatabase.BattleForAzeroth)]
        public uint? UnitClass;

        [DBFieldName("FadeRegionRadius", TargetedDatabase.BattleForAzeroth)]
        public float? FadeRegionRadius;

        [DBFieldName("WidgetSetID", TargetedDatabase.BattleForAzeroth)]
        public int? WidgetSetID;

        [DBFieldName("WidgetSetUnitConditionID", TargetedDatabase.BattleForAzeroth)]
        public int? WidgetSetUnitConditionID;

        [DBFieldName("rank")]
        public CreatureRank? Rank;

        [DBFieldName("family")]
        public CreatureFamily? Family;

        [DBFieldName("type")]
        public CreatureType? Type;

        [DBFieldName("type_flags")]
        public CreatureTypeFlag? TypeFlags;

        [DBFieldName("type_flags2", TargetedDatabase.Cataclysm)]
        public uint? TypeFlags2;

        [DBFieldName("PetSpellDataId", TargetedDatabase.Zero, TargetedDatabase.Cataclysm)]
        public uint? PetSpellDataID;

        [DBFieldName("HealthModifier")]
        public float? HealthModifier;

        [DBFieldName("ManaModifier")]
        public float? ManaModifier;

        [DBFieldName("RacialLeader")]
        public bool? RacialLeader;

        [DBFieldName("movementId")]
        public uint? MovementID;

        [DBFieldName("VerifiedBuild")]
        public int? VerifiedBuild = ClientVersion.BuildInt;
    }

    [DBTableName("creature_template")]
    public sealed class CreatureTemplateNonWDB : IDataModel
    {
        [DBFieldName("entry", true)]
        public uint? Entry;

        [DBFieldName("gossip_menu_id")]
        public uint? GossipMenuId;

        [DBFieldName("minlevel")]
        public int? MinLevel;

        [DBFieldName("maxlevel")]
        public int? MaxLevel;

        [DBFieldName("faction")]
        public uint? Faction;

        [DBFieldName("npcflag")]
        public NPCFlags? NpcFlag;

        [DBFieldName("speed_walk")]
        public float? SpeedWalk;

        [DBFieldName("speed_run")]
        public float? SpeedRun;

        [DBFieldName("BaseAttackTime")]
        public uint? BaseAttackTime;

        [DBFieldName("RangeAttackTime")]
        public uint? RangedAttackTime;

        [DBFieldName("unit_class", TargetedDatabase.Zero, TargetedDatabase.BattleForAzeroth)]
        public uint? UnitClass;

        [DBFieldName("unit_flags")]
        public UnitFlags? UnitFlags;

        [DBFieldName("unit_flags2")]
        public UnitFlags2? UnitFlags2;

        [DBFieldName("unit_flags3", TargetedDatabase.Legion)]
        public UnitFlags3? UnitFlags3;

        [DBFieldName("dynamicflags", TargetedDatabase.Zero, TargetedDatabase.WarlordsOfDraenor)]
        public UnitDynamicFlags? DynamicFlags;

        [DBFieldName("dynamicflags", TargetedDatabase.WarlordsOfDraenor)]
        public UnitDynamicFlagsWOD? DynamicFlagsWod;

        [DBFieldName("VehicleId")]
        public uint? VehicleID;

        [DBFieldName("HoverHeight")]
        public float? HoverHeight;

    }

    [DBTableName("creature_questitem")]
    public sealed class CreatureTemplateQuestItem : IDataModel
    {
        [DBFieldName("CreatureEntry", true)]
        public uint? CreatureEntry;

        [DBFieldName("Idx", true)]
        public uint? Idx;

        [DBFieldName("ItemId")]
        public uint? ItemId;

        [DBFieldName("VerifiedBuild", TargetedDatabase.WarlordsOfDraenor)]
        public int? VerifiedBuild = ClientVersion.BuildInt;
    }

    [DBTableName("creature_template_wdb")]
    public sealed class CreatureTemplateClassic : IDataModel
    {
        [DBFieldName("entry", true)]
        public uint? Entry;

        [DBFieldName("KillCredit", 2)]
        public uint?[] KillCredits;

        [DBFieldName("DisplayId1")]
        public uint DisplayId1;

        [DBFieldName("DisplayId2")]
        public uint DisplayId2;

        [DBFieldName("DisplayId3")]
        public uint DisplayId3;

        [DBFieldName("DisplayId4")]
        public uint DisplayId4;

        [DBFieldName("DisplayScale1")]
        public float DisplayScale1;

        [DBFieldName("DisplayScale2")]
        public float DisplayScale2;

        [DBFieldName("DisplayScale3")]
        public float DisplayScale3;

        [DBFieldName("DisplayScale4")]
        public float DisplayScale4;

        [DBFieldName("DisplayProbability1")]
        public float DisplayProbability1;

        [DBFieldName("DisplayProbability2")]
        public float DisplayProbability2;

        [DBFieldName("DisplayProbability3")]
        public float DisplayProbability3;

        [DBFieldName("DisplayProbability4")]
        public float DisplayProbability4;

        [DBFieldName("name")]
        public string Name;

        [DBFieldName("femaleName")]
        public string FemaleName;

        [DBFieldName("subname", nullable: true)]
        public string SubName;

        [DBFieldName("TitleAlt", nullable: true)]
        public string TitleAlt;

        [DBFieldName("IconName", nullable: true)]
        public string IconName;

        [DBFieldName("HealthScalingExpansion")]
        public ClientType? HealthScalingExpansion;

        [DBFieldName("RequiredExpansion")]
        public ClientType? RequiredExpansion;

        [DBFieldName("VignetteID")]
        public uint? VignetteID;

        [DBFieldName("unit_class")]
        public uint? UnitClass;

        [DBFieldName("rank")]
        public CreatureRank? Rank;

        [DBFieldName("family")]
        public CreatureFamily? Family;

        [DBFieldName("type")]
        public CreatureType? Type;

        [DBFieldName("type_flags")]
        public CreatureTypeFlag? TypeFlags;

        [DBFieldName("type_flags2")]
        public uint? TypeFlags2;

        [DBFieldName("PetSpellDataId")]
        public uint? PetSpellDataID;

        [DBFieldName("HealthModifier")]
        public float? HealthModifier;

        [DBFieldName("ManaModifier")]
        public float? ManaModifier;

        [DBFieldName("Civilian")]
        public bool? Civilian;

        [DBFieldName("RacialLeader")]
        public bool? RacialLeader;

        [DBFieldName("movementId")]
        public uint? MovementID;

        [DBFieldName("VerifiedBuild")]
        public int? VerifiedBuild = ClientVersion.BuildInt;
    }
}
