using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.SQL;

namespace WowPacketParser.Store.Objects
{
    [DBTableName("creature_template")]
    public sealed class CreatureTemplateNonWDB : IDataModel
    {
        [DBFieldName("entry", true, DbType = (TargetedDbType.WPP | TargetedDbType.TRINITY | TargetedDbType.VMANGOS))]
        [DBFieldName("Entry", true, DbType = (TargetedDbType.CMANGOS))]
        public uint? Entry;

        [DBFieldName("gossip_menu_id", DbType = (TargetedDbType.WPP | TargetedDbType.TRINITY | TargetedDbType.VMANGOS))]
        [DBFieldName("GossipMenuId", DbType = (TargetedDbType.CMANGOS))]
        public uint? GossipMenuId;

        [DBFieldName("level_min", DbType = (TargetedDbType.WPP | TargetedDbType.VMANGOS))]
        [DBFieldName("minlevel", DbType = (TargetedDbType.TRINITY))]
        [DBFieldName("MinLevel", DbType = (TargetedDbType.CMANGOS))]
        public int? MinLevel;

        [DBFieldName("level_max", DbType = (TargetedDbType.WPP | TargetedDbType.VMANGOS))]
        [DBFieldName("maxlevel", DbType = (TargetedDbType.TRINITY))]
        [DBFieldName("MaxLevel", DbType = (TargetedDbType.CMANGOS))]
        public int? MaxLevel;

        [DBFieldName("faction", DbType = (TargetedDbType.WPP | TargetedDbType.TRINITY | TargetedDbType.VMANGOS))]
        [DBFieldName("Faction", DbType = (TargetedDbType.CMANGOS))]
        public uint? Faction;

        [DBFieldName("npc_flags", DbType = (TargetedDbType.WPP | TargetedDbType.VMANGOS))]
        [DBFieldName("npcflag", DbType = (TargetedDbType.TRINITY))]
        [DBFieldName("NpcFlags", DbType = (TargetedDbType.CMANGOS))]
        public NPCFlags? NpcFlag;

        [DBFieldName("speed_walk", DbType = (TargetedDbType.WPP | TargetedDbType.TRINITY | TargetedDbType.VMANGOS))]
        [DBFieldName("SpeedWalk", DbType = (TargetedDbType.CMANGOS))]
        public float? SpeedWalk;

        [DBFieldName("speed_run", DbType = (TargetedDbType.WPP | TargetedDbType.TRINITY | TargetedDbType.VMANGOS))]
        [DBFieldName("SpeedRun", DbType = (TargetedDbType.CMANGOS))]
        public float? SpeedRun;

        [DBFieldName("scale", DbType = (TargetedDbType.WPP | TargetedDbType.TRINITY | TargetedDbType.VMANGOS))]
        [DBFieldName("Scale", DbType = (TargetedDbType.CMANGOS))]
        public float? Scale;

        [DBFieldName("base_attack_time", DbType = (TargetedDbType.WPP | TargetedDbType.VMANGOS))]
        [DBFieldName("BaseAttackTime", DbType = (TargetedDbType.TRINITY))]
        [DBFieldName("MeleeBaseAttackTime", DbType = (TargetedDbType.CMANGOS))]
        public uint? BaseAttackTime;

        [DBFieldName("unit_class", TargetedDbExpansion.Zero, TargetedDbExpansion.BattleForAzeroth, DbType = (TargetedDbType.WPP | TargetedDbType.VMANGOS | TargetedDbType.TRINITY))]
        [DBFieldName("UnitClass", TargetedDbExpansion.Zero, TargetedDbExpansion.BattleForAzeroth, DbType = (TargetedDbType.CMANGOS))]
        public uint? UnitClass;

        [DBFieldName("unit_flags", DbType = (TargetedDbType.WPP | TargetedDbType.VMANGOS | TargetedDbType.TRINITY))]
        [DBFieldName("UnitFlags", DbType = (TargetedDbType.CMANGOS))]
        public UnitFlags? UnitFlags;

        [DBFieldName("unit_flags2", DbType = (TargetedDbType.WPP | TargetedDbType.TRINITY))]
        public UnitFlags2? UnitFlags2;

        [DBFieldName("unit_flags3", TargetedDbExpansion.Legion, DbType = (TargetedDbType.WPP | TargetedDbType.TRINITY))]
        public UnitFlags3? UnitFlags3;

        [DBFieldName("dynamicflags", TargetedDbExpansion.Zero, TargetedDbExpansion.WarlordsOfDraenor, DbType = (TargetedDbType.TRINITY))]
        [DBFieldName("dynamic_flags", TargetedDbExpansion.Zero, TargetedDbExpansion.WarlordsOfDraenor, DbType = (TargetedDbType.WPP | TargetedDbType.VMANGOS))]
        [DBFieldName("DynamicFlags", TargetedDbExpansion.Zero, TargetedDbExpansion.WarlordsOfDraenor, DbType = (TargetedDbType.CMANGOS))]
        public UnitDynamicFlags? DynamicFlags;

        [DBFieldName("dynamicflags", TargetedDbExpansion.Classic, TargetedDbExpansion.Zero, DbType = (TargetedDbType.TRINITY))]
        [DBFieldName("dynamic_flags", TargetedDbExpansion.Classic, TargetedDbExpansion.Zero, DbType = (TargetedDbType.VMANGOS))]
        [DBFieldName("DynamicFlags", TargetedDbExpansion.Classic, TargetedDbExpansion.Zero, DbType = (TargetedDbType.CMANGOS))]
        [DBFieldName("dynamicflags", TargetedDbExpansion.WarlordsOfDraenor, DbType = (TargetedDbType.TRINITY))]
        [DBFieldName("dynamic_flags", TargetedDbExpansion.WarlordsOfDraenor, DbType = (TargetedDbType.WPP | TargetedDbType.VMANGOS))]
        [DBFieldName("DynamicFlags", TargetedDbExpansion.WarlordsOfDraenor, DbType = (TargetedDbType.CMANGOS))]
        public UnitDynamicFlagsWOD? DynamicFlagsWod;

        [DBFieldName("vehicle_id", DbType = (TargetedDbType.WPP))]
        [DBFieldName("VehicleId", DbType = (TargetedDbType.TRINITY))]
        [DBFieldName("VehicleTemplateId", DbType = (TargetedDbType.CMANGOS))]
        public uint? VehicleID;

        [DBFieldName("hover_height", DbType = (TargetedDbType.WPP))]
        [DBFieldName("HoverHeight", DbType = (TargetedDbType.TRINITY))]
        public float? HoverHeight;

        [DBFieldName("auras", DbType = (TargetedDbType.WPP | TargetedDbType.VMANGOS))]
        public string Auras;
    }

    [DBTableName("creature_questitem")]
    public sealed class CreatureTemplateQuestItem : IDataModel
    {
        [DBFieldName("entry", true, DbType = (TargetedDbType.WPP))]
        [DBFieldName("CreatureEntry", true, DbType = (TargetedDbType.TRINITY))]
        public uint? CreatureEntry;

        [DBFieldName("id", true, DbType = (TargetedDbType.WPP))]
        [DBFieldName("Idx", true, DbType = (TargetedDbType.TRINITY))]
        public uint? Idx;

        [DBFieldName("item_id", DbType = (TargetedDbType.WPP))]
        [DBFieldName("ItemId", DbType = (TargetedDbType.TRINITY))]
        public uint? ItemId;

        [DBFieldName("sniff_build", DbType = (TargetedDbType.WPP))]
        [DBFieldName("VerifiedBuild", DbType = (TargetedDbType.TRINITY))]
        public int? VerifiedBuild = ClientVersion.BuildInt;
    }

    [DBTableName("creature_template_wdb", TargetedDbType.WPP)]
    [DBTableName("creature_template", TargetedDbType.TRINITY | TargetedDbType.VMANGOS | TargetedDbType.CMANGOS)]
    public sealed class CreatureTemplate : IDataModel
    {
        [DBFieldName("entry", true, DbType = (TargetedDbType.WPP | TargetedDbType.TRINITY | TargetedDbType.VMANGOS))]
        [DBFieldName("Entry", true, DbType = (TargetedDbType.CMANGOS))]
        public uint? Entry;

        [DBFieldName("kill_credit", 2, DbType = (TargetedDbType.WPP))]
        [DBFieldName("KillCredit", 2, DbType = (TargetedDbType.TRINITY | TargetedDbType.CMANGOS))]
        public uint?[] KillCredits;

        [DBFieldName("display_total_count", DbType = (TargetedDbType.WPP | TargetedDbType.VMANGOS))]
        public uint DisplayTotalCount;

        [DBFieldName("display_total_probability", DbType = (TargetedDbType.WPP | TargetedDbType.VMANGOS))]
        public float DisplayTotalProbability;

        [DBFieldName("display_id", TargetedDbExpansion.Classic, TargetedDbExpansion.BattleForAzeroth, 4, DbType = (TargetedDbType.WPP | TargetedDbType.VMANGOS))]
        [DBFieldName("modelid", TargetedDbExpansion.Classic, TargetedDbExpansion.BattleForAzeroth, 4, DbType = (TargetedDbType.TRINITY))]
        [DBFieldName("ModelId", TargetedDbExpansion.Classic, TargetedDbExpansion.BattleForAzeroth, 4, DbType = (TargetedDbType.CMANGOS))]
        public uint?[] DisplayIDs;

        [DBFieldName("display_scale1", DbType = (TargetedDbType.WPP | TargetedDbType.VMANGOS))]
        public float DisplayScale1;

        [DBFieldName("display_scale2", DbType = (TargetedDbType.WPP | TargetedDbType.VMANGOS))]
        public float DisplayScale2;

        [DBFieldName("display_scale3", DbType = (TargetedDbType.WPP | TargetedDbType.VMANGOS))]
        public float DisplayScale3;

        [DBFieldName("display_scale4", DbType = (TargetedDbType.WPP | TargetedDbType.VMANGOS))]
        public float DisplayScale4;

        [DBFieldName("display_probability1", DbType = (TargetedDbType.WPP | TargetedDbType.VMANGOS))]
        public float DisplayProbability1;

        [DBFieldName("display_probability2", DbType = (TargetedDbType.WPP | TargetedDbType.VMANGOS))]
        public float DisplayProbability2;

        [DBFieldName("display_probability3", DbType = (TargetedDbType.WPP | TargetedDbType.VMANGOS))]
        public float DisplayProbability3;

        [DBFieldName("display_probability4", DbType = (TargetedDbType.WPP | TargetedDbType.VMANGOS))]
        public float DisplayProbability4;

        [DBFieldName("name", DbType = (TargetedDbType.WPP | TargetedDbType.TRINITY | TargetedDbType.VMANGOS))]
        [DBFieldName("Name", DbType = (TargetedDbType.CMANGOS))]
        public string Name;

        [DBFieldName("female_name", TargetedDbExpansion.Classic, TargetedDbExpansion.Zero, DbType = (TargetedDbType.WPP))]
        [DBFieldName("female_name", TargetedDbExpansion.Cataclysm, DbType = (TargetedDbType.WPP))]
        [DBFieldName("femaleName", TargetedDbExpansion.Classic, TargetedDbExpansion.Zero, DbType = (TargetedDbType.TRINITY))]
        [DBFieldName("femaleName", TargetedDbExpansion.Cataclysm, DbType = (TargetedDbType.TRINITY))]
        public string FemaleName;

        [DBFieldName("subname", nullable: true, DbType = (TargetedDbType.WPP | TargetedDbType.TRINITY | TargetedDbType.VMANGOS))]
        [DBFieldName("SubName", nullable: true, DbType = (TargetedDbType.CMANGOS))]
        public string SubName;

        [DBFieldName("title_alt", TargetedDbExpansion.Classic, TargetedDbExpansion.Zero, nullable: true, DbType = (TargetedDbType.WPP))]
        [DBFieldName("title_alt", TargetedDbExpansion.WarlordsOfDraenor /*Mists of Pandaria*/, nullable: true, DbType = (TargetedDbType.WPP))]
        [DBFieldName("TitleAlt", TargetedDbExpansion.Classic, TargetedDbExpansion.Zero, nullable: true, DbType = (TargetedDbType.TRINITY))]
        [DBFieldName("TitleAlt", TargetedDbExpansion.WarlordsOfDraenor /*Mists of Pandaria*/, nullable: true, DbType = (TargetedDbType.TRINITY))]
        public string TitleAlt;

        [DBFieldName("icon_name", nullable: true, DbType = (TargetedDbType.WPP))]
        [DBFieldName("IconName", nullable: true, DbType = (TargetedDbType.TRINITY | TargetedDbType.CMANGOS))]
        public string IconName;

        [DBFieldName("health_scaling_expansion", TargetedDbExpansion.Classic, TargetedDbExpansion.Zero, DbType = (TargetedDbType.WPP))]
        [DBFieldName("health_scaling_expansion", TargetedDbExpansion.WarlordsOfDraenor, DbType = (TargetedDbType.TRINITY))]
        [DBFieldName("HealthScalingExpansion", TargetedDbExpansion.Classic, TargetedDbExpansion.Zero, DbType = (TargetedDbType.TRINITY))]
        [DBFieldName("HealthScalingExpansion", TargetedDbExpansion.WarlordsOfDraenor, DbType = (TargetedDbType.TRINITY))]
        public ClientType? HealthScalingExpansion;

        [DBFieldName("required_expansion", TargetedDbExpansion.Classic, TargetedDbExpansion.Zero, DbType = (TargetedDbType.WPP))]
        [DBFieldName("required_expansion", TargetedDbExpansion.Cataclysm, DbType = (TargetedDbType.WPP))]
        [DBFieldName("RequiredExpansion", TargetedDbExpansion.Classic, TargetedDbExpansion.Zero, DbType = (TargetedDbType.TRINITY))]
        [DBFieldName("RequiredExpansion", TargetedDbExpansion.Cataclysm, DbType = (TargetedDbType.TRINITY))]
        public ClientType? RequiredExpansion;

        [DBFieldName("vignette_id", TargetedDbExpansion.Classic, TargetedDbExpansion.Zero, DbType = (TargetedDbType.WPP))]
        [DBFieldName("vignette_id", TargetedDbExpansion.Legion, DbType = (TargetedDbType.WPP))]
        [DBFieldName("VignetteID", TargetedDbExpansion.Classic, TargetedDbExpansion.Zero, DbType = (TargetedDbType.TRINITY))]
        [DBFieldName("VignetteID", TargetedDbExpansion.Legion, DbType = (TargetedDbType.TRINITY))]
        public uint? VignetteID;

        [DBFieldName("unit_class", TargetedDbExpansion.Classic, TargetedDbExpansion.Zero, DbType = (TargetedDbType.WPP | TargetedDbType.TRINITY | TargetedDbType.VMANGOS))]
        [DBFieldName("unit_class", TargetedDbExpansion.BattleForAzeroth, DbType = (TargetedDbType.WPP | TargetedDbType.TRINITY | TargetedDbType.VMANGOS))]
        [DBFieldName("UnitClass", TargetedDbExpansion.Classic, TargetedDbExpansion.Zero, DbType = (TargetedDbType.CMANGOS))]
        [DBFieldName("UnitClass", TargetedDbExpansion.BattleForAzeroth, DbType = (TargetedDbType.CMANGOS))]
        public uint? UnitClass;

        [DBFieldName("FadeRegionRadius", TargetedDbExpansion.BattleForAzeroth, TargetedDbExpansion.Shadowlands, DbType = (TargetedDbType.TRINITY))]
        public float? FadeRegionRadius;

        [DBFieldName("WidgetSetID", TargetedDbExpansion.BattleForAzeroth, DbType = (TargetedDbType.TRINITY))]
        public int? WidgetSetID;

        [DBFieldName("WidgetSetUnitConditionID", TargetedDbExpansion.BattleForAzeroth, DbType = (TargetedDbType.TRINITY))]
        public int? WidgetSetUnitConditionID;

        [DBFieldName("rank", DbType = (TargetedDbType.WPP | TargetedDbType.TRINITY | TargetedDbType.VMANGOS))]
        [DBFieldName("Rank", DbType = (TargetedDbType.CMANGOS))]
        public CreatureRank? Rank;

        [DBFieldName("beast_family", DbType = (TargetedDbType.WPP | TargetedDbType.VMANGOS))]
        [DBFieldName("family", DbType = (TargetedDbType.TRINITY))]
        [DBFieldName("Family", DbType = (TargetedDbType.CMANGOS))]
        public CreatureFamily? Family;

        [DBFieldName("type", DbType = (TargetedDbType.WPP | TargetedDbType.TRINITY | TargetedDbType.VMANGOS))]
        [DBFieldName("CreatureType", DbType = (TargetedDbType.CMANGOS))]
        public CreatureType? Type;

        [DBFieldName("type_flags", DbType = (TargetedDbType.WPP | TargetedDbType.TRINITY | TargetedDbType.VMANGOS))]
        [DBFieldName("CreatureTypeFlags", DbType = (TargetedDbType.CMANGOS))]
        public CreatureTypeFlag? TypeFlags;

        [DBFieldName("type_flags2", TargetedDbExpansion.Classic, TargetedDbExpansion.Zero, DbType = (TargetedDbType.WPP | TargetedDbType.TRINITY))]
        [DBFieldName("type_flags2", TargetedDbExpansion.Cataclysm, DbType = (TargetedDbType.WPP | TargetedDbType.TRINITY))]
        public uint? TypeFlags2;

        [DBFieldName("pet_spell_list_id", TargetedDbExpansion.Classic, TargetedDbExpansion.Cataclysm, DbType = (TargetedDbType.WPP | TargetedDbType.VMANGOS))]
        [DBFieldName("PetSpellDataId", TargetedDbExpansion.Classic, TargetedDbExpansion.Cataclysm, DbType = (TargetedDbType.TRINITY | TargetedDbType.CMANGOS))]
        public uint? PetSpellDataID;

        [DBFieldName("health_multiplier", DbType = (TargetedDbType.WPP))]
        [DBFieldName("HealthModifier", DbType = (TargetedDbType.TRINITY))]
        [DBFieldName("HealthMultiplier", DbType = (TargetedDbType.CMANGOS))]
        public float? HealthMultiplier;

        [DBFieldName("mana_multiplier", DbType = (TargetedDbType.WPP))]
        [DBFieldName("ManaModifier", DbType = (TargetedDbType.TRINITY))]
        [DBFieldName("PowerMultiplier", DbType = (TargetedDbType.CMANGOS))]
        public float? ManaMultiplier;

        [DBFieldName("civilian", DbType = (TargetedDbType.WPP | TargetedDbType.VMANGOS))]
        public bool? Civilian;

        [DBFieldName("racial_leader", DbType = (TargetedDbType.WPP | TargetedDbType.VMANGOS))]
        [DBFieldName("RacialLeader", DbType = (TargetedDbType.TRINITY | TargetedDbType.CMANGOS))]
        public bool? RacialLeader;

        [DBFieldName("movement_id", DbType = (TargetedDbType.WPP))]
        [DBFieldName("movementId", DbType = (TargetedDbType.TRINITY))]
        public uint? MovementID;

        [DBFieldName("sniff_build", DbType = (TargetedDbType.WPP))]
        [DBFieldName("VerifiedBuild", DbType = (TargetedDbType.TRINITY))]
        public int? SniffBuild = ClientVersion.BuildInt;
    }
}
