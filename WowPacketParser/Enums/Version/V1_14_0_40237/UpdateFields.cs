﻿namespace WowPacketParser.Enums.Version.V1_14_0_40237
{
    // ReSharper disable InconsistentNaming
    // 1.14.0.40237
    public enum ObjectField
    {
        OBJECT_FIELD_GUID                                           = 0x000, // Size: 4, Flags: PUBLIC
        OBJECT_FIELD_ENTRY                                          = 0x004, // Size: 1, Flags: DYNAMIC
        OBJECT_DYNAMIC_FLAGS                                        = 0x005, // Size: 1, Flags: DYNAMIC, URGENT
        OBJECT_FIELD_SCALE_X                                        = 0x006, // Size: 1, Flags: PUBLIC
        OBJECT_END                                                  = 0x007,
    }

    public enum ObjectDynamicField
    {
        OBJECT_DYNAMIC_END                                          = 0x000,
    }

    public enum ItemField
    {
        ITEM_FIELD_OWNER                                            = ObjectField.OBJECT_END + 0x000, // Size: 4, Flags: PUBLIC
        ITEM_FIELD_CONTAINED                                        = ObjectField.OBJECT_END + 0x004, // Size: 4, Flags: PUBLIC
        ITEM_FIELD_CREATOR                                          = ObjectField.OBJECT_END + 0x008, // Size: 4, Flags: PUBLIC
        ITEM_FIELD_GIFTCREATOR                                      = ObjectField.OBJECT_END + 0x00C, // Size: 4, Flags: PUBLIC
        ITEM_FIELD_STACK_COUNT                                      = ObjectField.OBJECT_END + 0x010, // Size: 1, Flags: OWNER
        ITEM_FIELD_DURATION                                         = ObjectField.OBJECT_END + 0x011, // Size: 1, Flags: OWNER
        ITEM_FIELD_SPELL_CHARGES                                    = ObjectField.OBJECT_END + 0x012, // Size: 5, Flags: OWNER
        ITEM_FIELD_FLAGS                                            = ObjectField.OBJECT_END + 0x017, // Size: 1, Flags: PUBLIC
        ITEM_FIELD_ENCHANTMENT                                      = ObjectField.OBJECT_END + 0x018, // Size: 39, Flags: PUBLIC
        ITEM_FIELD_PROPERTY_SEED                                    = ObjectField.OBJECT_END + 0x03F, // Size: 1, Flags: PUBLIC
        ITEM_FIELD_RANDOM_PROPERTIES_ID                             = ObjectField.OBJECT_END + 0x040, // Size: 1, Flags: PUBLIC
        ITEM_FIELD_DURABILITY                                       = ObjectField.OBJECT_END + 0x041, // Size: 1, Flags: OWNER
        ITEM_FIELD_MAXDURABILITY                                    = ObjectField.OBJECT_END + 0x042, // Size: 1, Flags: OWNER
        ITEM_FIELD_CREATE_PLAYED_TIME                               = ObjectField.OBJECT_END + 0x043, // Size: 1, Flags: PUBLIC
        ITEM_FIELD_MODIFIERS_MASK                                   = ObjectField.OBJECT_END + 0x044, // Size: 1, Flags: OWNER
        ITEM_FIELD_CONTEXT                                          = ObjectField.OBJECT_END + 0x045, // Size: 1, Flags: PUBLIC
        ITEM_FIELD_ARTIFACT_XP                                      = ObjectField.OBJECT_END + 0x046, // Size: 2, Flags: OWNER
        ITEM_FIELD_APPEARANCE_MOD_ID                                = ObjectField.OBJECT_END + 0x048, // Size: 1, Flags: OWNER
        ITEM_END                                                    = ObjectField.OBJECT_END + 0x049,
    }

    public enum ItemDynamicField
    {
        ITEM_DYNAMIC_FIELD_MODIFIERS                                = ObjectDynamicField.OBJECT_DYNAMIC_END + 0x000, // Flags: OWNER
        ITEM_DYNAMIC_FIELD_BONUSLIST_IDS                            = ObjectDynamicField.OBJECT_DYNAMIC_END + 0x001, // Flags: OWNER, 0x100
        ITEM_DYNAMIC_FIELD_ARTIFACT_POWERS                          = ObjectDynamicField.OBJECT_DYNAMIC_END + 0x002, // Flags: OWNER
        ITEM_DYNAMIC_FIELD_GEMS                                     = ObjectDynamicField.OBJECT_DYNAMIC_END + 0x003, // Flags: OWNER
        ITEM_DYNAMIC_END                                            = ObjectDynamicField.OBJECT_DYNAMIC_END + 0x004,
    }

    public enum ContainerField
    {
        CONTAINER_FIELD_SLOT_1                                      = ItemField.ITEM_END + 0x000, // Size: 144, Flags: PUBLIC
        CONTAINER_FIELD_NUM_SLOTS                                   = ItemField.ITEM_END + 0x090, // Size: 1, Flags: PUBLIC
        CONTAINER_END                                               = ItemField.ITEM_END + 0x091,
    }

    public enum ContainerDynamicField
    {
        CONTAINER_DYNAMIC_END                                       = ItemDynamicField.ITEM_DYNAMIC_END + 0x000,
    }

    public enum UnitField
    {
        UNIT_FIELD_CHARM                                            = ObjectField.OBJECT_END + 0x000, // Size: 4, Flags: PUBLIC
        UNIT_FIELD_SUMMON                                           = ObjectField.OBJECT_END + 0x004, // Size: 4, Flags: PUBLIC
        UNIT_FIELD_CRITTER                                          = ObjectField.OBJECT_END + 0x008, // Size: 4, Flags: PRIVATE
        UNIT_FIELD_CHARMEDBY                                        = ObjectField.OBJECT_END + 0x00C, // Size: 4, Flags: PUBLIC
        UNIT_FIELD_SUMMONEDBY                                       = ObjectField.OBJECT_END + 0x010, // Size: 4, Flags: PUBLIC
        UNIT_FIELD_CREATEDBY                                        = ObjectField.OBJECT_END + 0x014, // Size: 4, Flags: PUBLIC
        UNIT_FIELD_DEMON_CREATOR                                    = ObjectField.OBJECT_END + 0x018, // Size: 4, Flags: PUBLIC
        UNIT_FIELD_LOOK_AT_CONTROLLER_TARGET                        = ObjectField.OBJECT_END + 0x01C, // Size: 4, Flags: PUBLIC
        UNIT_FIELD_TARGET                                           = ObjectField.OBJECT_END + 0x020, // Size: 4, Flags: PUBLIC
        UNIT_FIELD_BATTLE_PET_COMPANION_GUID                        = ObjectField.OBJECT_END + 0x024, // Size: 4, Flags: PUBLIC
        UNIT_FIELD_BATTLE_PET_DB_ID                                 = ObjectField.OBJECT_END + 0x028, // Size: 2, Flags: PUBLIC
        UNIT_FIELD_CHANNEL_DATA                                     = ObjectField.OBJECT_END + 0x02A, // Size: 2, Flags: PUBLIC, URGENT
        UNIT_FIELD_SUMMONED_BY_HOME_REALM                           = ObjectField.OBJECT_END + 0x02C, // Size: 1, Flags: PUBLIC
        UNIT_FIELD_BYTES_0                                          = ObjectField.OBJECT_END + 0x02D, // Size: 1, Flags: PUBLIC Nested: (Race, ClassId, PlayerClassId, Sex)
        UNIT_FIELD_DISPLAY_POWER                                    = ObjectField.OBJECT_END + 0x02E, // Size: 1, Flags: PUBLIC
        UNIT_FIELD_OVERRIDE_DISPLAY_POWER_ID                        = ObjectField.OBJECT_END + 0x02F, // Size: 1, Flags: PUBLIC
        UNIT_FIELD_HEALTH                                           = ObjectField.OBJECT_END + 0x030, // Size: 2, Flags: DYNAMIC
        UNIT_FIELD_POWER                                            = ObjectField.OBJECT_END + 0x032, // Size: 6, Flags: PUBLIC, URGENT_SELF_ONLY
        UNIT_FIELD_MAXHEALTH                                        = ObjectField.OBJECT_END + 0x038, // Size: 2, Flags: DYNAMIC
        UNIT_FIELD_MAXPOWER                                         = ObjectField.OBJECT_END + 0x03A, // Size: 6, Flags: PUBLIC
        UNIT_FIELD_MOD_POWER_REGEN                                  = ObjectField.OBJECT_END + 0x040, // Size: 6, Flags: PRIVATE, OWNER, UNIT_ALL
        UNIT_FIELD_LEVEL                                            = ObjectField.OBJECT_END + 0x046, // Size: 1, Flags: PUBLIC
        UNIT_FIELD_EFFECTIVE_LEVEL                                  = ObjectField.OBJECT_END + 0x047, // Size: 1, Flags: PUBLIC
        UNIT_FIELD_CONTENT_TUNING_ID                                = ObjectField.OBJECT_END + 0x048, // Size: 1, Flags: PUBLIC
        UNIT_FIELD_SCALING_LEVEL_MIN                                = ObjectField.OBJECT_END + 0x049, // Size: 1, Flags: PUBLIC
        UNIT_FIELD_SCALING_LEVEL_MAX                                = ObjectField.OBJECT_END + 0x04A, // Size: 1, Flags: PUBLIC
        UNIT_FIELD_SCALING_LEVEL_DELTA                              = ObjectField.OBJECT_END + 0x04B, // Size: 1, Flags: PUBLIC
        UNIT_FIELD_SCALING_FACTION_GROUP                            = ObjectField.OBJECT_END + 0x04C, // Size: 1, Flags: PUBLIC
        UNIT_FIELD_SCALING_HEALTH_ITEM_LEVEL_CURVE_ID               = ObjectField.OBJECT_END + 0x04D, // Size: 1, Flags: PUBLIC
        UNIT_FIELD_SCALING_DAMAGE_ITEM_LEVEL_CURVE_ID               = ObjectField.OBJECT_END + 0x04E, // Size: 1, Flags: PUBLIC
        UNIT_FIELD_FACTIONTEMPLATE                                  = ObjectField.OBJECT_END + 0x04F, // Size: 1, Flags: PUBLIC
        UNIT_VIRTUAL_ITEM_SLOT_ID                                   = ObjectField.OBJECT_END + 0x050, // Size: 6, Flags: PUBLIC
        UNIT_FIELD_FLAGS                                            = ObjectField.OBJECT_END + 0x056, // Size: 1, Flags: PUBLIC, URGENT
        UNIT_FIELD_FLAGS_2                                          = ObjectField.OBJECT_END + 0x057, // Size: 1, Flags: PUBLIC, URGENT
        UNIT_FIELD_FLAGS_3                                          = ObjectField.OBJECT_END + 0x058, // Size: 1, Flags: PUBLIC, URGENT
        UNIT_FIELD_AURASTATE                                        = ObjectField.OBJECT_END + 0x059, // Size: 1, Flags: PUBLIC
        UNIT_FIELD_BASEATTACKTIME                                   = ObjectField.OBJECT_END + 0x05A, // Size: 2, Flags: PUBLIC
        UNIT_FIELD_RANGEDATTACKTIME                                 = ObjectField.OBJECT_END + 0x05C, // Size: 1, Flags: PRIVATE
        UNIT_FIELD_BOUNDINGRADIUS                                   = ObjectField.OBJECT_END + 0x05D, // Size: 1, Flags: PUBLIC
        UNIT_FIELD_COMBATREACH                                      = ObjectField.OBJECT_END + 0x05E, // Size: 1, Flags: PUBLIC
        UNIT_FIELD_DISPLAYID                                        = ObjectField.OBJECT_END + 0x05F, // Size: 1, Flags: DYNAMIC, URGENT
        UNIT_FIELD_DISPLAYSCALE                                     = ObjectField.OBJECT_END + 0x060, // Size: 1, Flags: DYNAMIC, URGENT
        UNIT_FIELD_NATIVEDISPLAYID                                  = ObjectField.OBJECT_END + 0x061, // Size: 1, Flags: PUBLIC, URGENT
        UNIT_FIELD_NATIVEXDISPLAYSCALE                              = ObjectField.OBJECT_END + 0x062, // Size: 1, Flags: PUBLIC, URGENT
        UNIT_FIELD_MOUNTDISPLAYID                                   = ObjectField.OBJECT_END + 0x063, // Size: 1, Flags: PUBLIC, URGENT
        UNIT_FIELD_MINDAMAGE                                        = ObjectField.OBJECT_END + 0x064, // Size: 1, Flags: PRIVATE, OWNER, SPECIAL_INFO
        UNIT_FIELD_MAXDAMAGE                                        = ObjectField.OBJECT_END + 0x065, // Size: 1, Flags: PRIVATE, OWNER, SPECIAL_INFO
        UNIT_FIELD_MINOFFHANDDAMAGE                                 = ObjectField.OBJECT_END + 0x066, // Size: 1, Flags: PRIVATE, OWNER, SPECIAL_INFO
        UNIT_FIELD_MAXOFFHANDDAMAGE                                 = ObjectField.OBJECT_END + 0x067, // Size: 1, Flags: PRIVATE, OWNER, SPECIAL_INFO
        UNIT_FIELD_BYTES_1                                          = ObjectField.OBJECT_END + 0x068, // Size: 1, Flags: PUBLIC Nested: (StandState, PetLoyaltyIndex, VisFlags, AnimTier)
        UNIT_FIELD_PETNUMBER                                        = ObjectField.OBJECT_END + 0x069, // Size: 1, Flags: PUBLIC
        UNIT_FIELD_PET_NAME_TIMESTAMP                               = ObjectField.OBJECT_END + 0x06A, // Size: 1, Flags: PUBLIC
        UNIT_FIELD_PETEXPERIENCE                                    = ObjectField.OBJECT_END + 0x06B, // Size: 1, Flags: OWNER
        UNIT_FIELD_PETNEXTLEVELEXPERIENCE                           = ObjectField.OBJECT_END + 0x06C, // Size: 1, Flags: OWNER
        UNIT_MOD_CAST_SPEED                                         = ObjectField.OBJECT_END + 0x06D, // Size: 1, Flags: PUBLIC
        UNIT_MOD_CAST_HASTE                                         = ObjectField.OBJECT_END + 0x06E, // Size: 1, Flags: PUBLIC
        UNIT_FIELD_MOD_HASTE                                        = ObjectField.OBJECT_END + 0x06F, // Size: 1, Flags: PUBLIC
        UNIT_FIELD_MOD_RANGED_HASTE                                 = ObjectField.OBJECT_END + 0x070, // Size: 1, Flags: PUBLIC
        UNIT_FIELD_MOD_HASTE_REGEN                                  = ObjectField.OBJECT_END + 0x071, // Size: 1, Flags: PUBLIC
        UNIT_FIELD_MOD_TIME_RATE                                    = ObjectField.OBJECT_END + 0x072, // Size: 1, Flags: PUBLIC
        UNIT_CREATED_BY_SPELL                                       = ObjectField.OBJECT_END + 0x073, // Size: 1, Flags: PUBLIC
        UNIT_NPC_FLAGS                                              = ObjectField.OBJECT_END + 0x074, // Size: 2, Flags: PUBLIC, DYNAMIC
        UNIT_NPC_EMOTESTATE                                         = ObjectField.OBJECT_END + 0x076, // Size: 1, Flags: PUBLIC
        UNIT_FIELD_TRAINING_POINTS_TOTAL                            = ObjectField.OBJECT_END + 0x077, // Size: 1, Flags: OWNER Nested: (TrainingPointsUsed, TrainingPointsTotal)
        UNIT_FIELD_STAT                                             = ObjectField.OBJECT_END + 0x078, // Size: 5, Flags: PRIVATE, OWNER
        UNIT_FIELD_POSSTAT                                          = ObjectField.OBJECT_END + 0x07D, // Size: 5, Flags: PRIVATE, OWNER
        UNIT_FIELD_NEGSTAT                                          = ObjectField.OBJECT_END + 0x082, // Size: 5, Flags: PRIVATE, OWNER
        UNIT_FIELD_RESISTANCES                                      = ObjectField.OBJECT_END + 0x087, // Size: 7, Flags: PRIVATE, OWNER, SPECIAL_INFO
        UNIT_FIELD_RESISTANCEBUFFMODSPOSITIVE                       = ObjectField.OBJECT_END + 0x08E, // Size: 7, Flags: PRIVATE, OWNER
        UNIT_FIELD_RESISTANCEBUFFMODSNEGATIVE                       = ObjectField.OBJECT_END + 0x095, // Size: 7, Flags: PRIVATE, OWNER
        UNIT_FIELD_BASE_MANA                                        = ObjectField.OBJECT_END + 0x09C, // Size: 1, Flags: PUBLIC
        UNIT_FIELD_BASE_HEALTH                                      = ObjectField.OBJECT_END + 0x09D, // Size: 1, Flags: PRIVATE, OWNER
        UNIT_FIELD_BYTES_2                                          = ObjectField.OBJECT_END + 0x09E, // Size: 1, Flags: PUBLIC  Nested: (SheatheState, PvpFlags, PetFlags, ShapeshiftForm)
        UNIT_FIELD_ATTACK_POWER                                     = ObjectField.OBJECT_END + 0x09F, // Size: 1, Flags: PRIVATE, OWNER
        UNIT_FIELD_ATTACK_POWER_MOD_POS                             = ObjectField.OBJECT_END + 0x0A0, // Size: 1, Flags: PRIVATE, OWNER
        UNIT_FIELD_ATTACK_POWER_MOD_NEG                             = ObjectField.OBJECT_END + 0x0A1, // Size: 1, Flags: PRIVATE, OWNER
        UNIT_FIELD_ATTACK_POWER_MULTIPLIER                          = ObjectField.OBJECT_END + 0x0A2, // Size: 1, Flags: PRIVATE, OWNER
        UNIT_FIELD_RANGED_ATTACK_POWER                              = ObjectField.OBJECT_END + 0x0A3, // Size: 1, Flags: PRIVATE, OWNER
        UNIT_FIELD_RANGED_ATTACK_POWER_MOD_POS                      = ObjectField.OBJECT_END + 0x0A4, // Size: 1, Flags: PRIVATE, OWNER
        UNIT_FIELD_RANGED_ATTACK_POWER_MOD_NEG                      = ObjectField.OBJECT_END + 0x0A5, // Size: 1, Flags: PRIVATE, OWNER
        UNIT_FIELD_RANGED_ATTACK_POWER_MULTIPLIER                   = ObjectField.OBJECT_END + 0x0A6, // Size: 1, Flags: PRIVATE, OWNER
        UNIT_FIELD_ATTACK_SPEED_AURA                                = ObjectField.OBJECT_END + 0x0A7, // Size: 1, Flags: PRIVATE, OWNER
        UNIT_FIELD_LIFESTEAL                                        = ObjectField.OBJECT_END + 0x0A8, // Size: 1, Flags: PRIVATE, OWNER
        UNIT_FIELD_MINRANGEDDAMAGE                                  = ObjectField.OBJECT_END + 0x0A9, // Size: 1, Flags: PRIVATE, OWNER
        UNIT_FIELD_MAXRANGEDDAMAGE                                  = ObjectField.OBJECT_END + 0x0AA, // Size: 1, Flags: PRIVATE, OWNER
        UNIT_FIELD_POWER_COST_MODIFIER                              = ObjectField.OBJECT_END + 0x0AB, // Size: 7, Flags: PRIVATE, OWNER
        UNIT_FIELD_POWER_COST_MULTIPLIER                            = ObjectField.OBJECT_END + 0x0B2, // Size: 7, Flags: PRIVATE, OWNER
        UNIT_FIELD_MAXHEALTHMODIFIER                                = ObjectField.OBJECT_END + 0x0B9, // Size: 1, Flags: PRIVATE, OWNER
        UNIT_FIELD_HOVERHEIGHT                                      = ObjectField.OBJECT_END + 0x0BA, // Size: 1, Flags: PUBLIC
        UNIT_FIELD_MIN_ITEM_LEVEL_CUTOFF                            = ObjectField.OBJECT_END + 0x0BB, // Size: 1, Flags: PUBLIC
        UNIT_FIELD_MIN_ITEM_LEVEL                                   = ObjectField.OBJECT_END + 0x0BC, // Size: 1, Flags: PUBLIC
        UNIT_FIELD_MAXITEMLEVEL                                     = ObjectField.OBJECT_END + 0x0BD, // Size: 1, Flags: PUBLIC
        UNIT_FIELD_WILD_BATTLE_PET_LEVEL                            = ObjectField.OBJECT_END + 0x0BE, // Size: 1, Flags: PUBLIC
        UNIT_FIELD_BATTLEPET_COMPANION_NAME_TIMESTAMP               = ObjectField.OBJECT_END + 0x0BF, // Size: 1, Flags: PUBLIC
        UNIT_FIELD_INTERACT_SPELL_ID                                = ObjectField.OBJECT_END + 0x0C0, // Size: 1, Flags: PUBLIC
        UNIT_FIELD_STATE_SPELL_VISUAL_ID                            = ObjectField.OBJECT_END + 0x0C1, // Size: 1, Flags: DYNAMIC, URGENT
        UNIT_FIELD_STATE_ANIM_ID                                    = ObjectField.OBJECT_END + 0x0C2, // Size: 1, Flags: DYNAMIC, URGENT
        UNIT_FIELD_STATE_ANIM_KIT_ID                                = ObjectField.OBJECT_END + 0x0C3, // Size: 1, Flags: DYNAMIC, URGENT
        UNIT_FIELD_STATE_WORLD_EFFECT_ID                            = ObjectField.OBJECT_END + 0x0C4, // Size: 4, Flags: DYNAMIC, URGENT
        UNIT_FIELD_SCALE_DURATION                                   = ObjectField.OBJECT_END + 0x0C8, // Size: 1, Flags: PUBLIC
        UNIT_FIELD_LOOKS_LIKE_MOUNT_ID                              = ObjectField.OBJECT_END + 0x0C9, // Size: 1, Flags: PUBLIC
        UNIT_FIELD_LOOKS_LIKE_CREATURE_ID                           = ObjectField.OBJECT_END + 0x0CA, // Size: 1, Flags: PUBLIC
        UNIT_FIELD_LOOK_AT_CONTROLLER_ID                            = ObjectField.OBJECT_END + 0x0CB, // Size: 1, Flags: PUBLIC
        UNIT_FIELD_GUILD_GUID                                       = ObjectField.OBJECT_END + 0x0CC, // Size: 4, Flags: PUBLIC
        UNIT_END                                                    = ObjectField.OBJECT_END + 0x0D0,
    }

    public enum UnitDynamicField
    {
        UNIT_DYNAMIC_FIELD_PASSIVE_SPELLS                                 = ObjectDynamicField.OBJECT_DYNAMIC_END + 0x0000, // Flags: PUBLIC, URGENT
        UNIT_DYNAMIC_FIELD_WORLD_EFFECTS                                  = ObjectDynamicField.OBJECT_DYNAMIC_END + 0x0001, // Flags: PUBLIC, URGENT
        UNIT_DYNAMIC_FIELD_CHANNEL_OBJECTS                                = ObjectDynamicField.OBJECT_DYNAMIC_END + 0x0002, // Flags: PUBLIC, URGENT
        UNIT_DYNAMIC_END                                                  = ObjectDynamicField.OBJECT_DYNAMIC_END + 0x0003,
    }

    public enum PlayerField
    {
        PLAYER_DUEL_ARBITER                                               = UnitField.UNIT_END + 0x0000, // Size: 4, Flags: PUBLIC
        PLAYER_WOW_ACCOUNT                                                = UnitField.UNIT_END + 0x0004, // Size: 4, Flags: PUBLIC
        PLAYER_LOOT_TARGET_GUID                                           = UnitField.UNIT_END + 0x0008, // Size: 4, Flags: PUBLIC
        PLAYER_FLAGS                                                      = UnitField.UNIT_END + 0x000C, // Size: 1, Flags: PUBLIC
        PLAYER_FLAGS_EX                                                   = UnitField.UNIT_END + 0x000D, // Size: 1, Flags: PUBLIC
        PLAYER_GUILDRANK                                                  = UnitField.UNIT_END + 0x000E, // Size: 1, Flags: PUBLIC
        PLAYER_GUILDDELETE_DATE                                           = UnitField.UNIT_END + 0x000F, // Size: 1, Flags: PUBLIC
        PLAYER_GUILDLEVEL                                                 = UnitField.UNIT_END + 0x0010, // Size: 1, Flags: PUBLIC
        PLAYER_BYTES                                                      = UnitField.UNIT_END + 0x0011, // Size: 1, Flags: PUBLIC Nested: (PartyType, NumBankSlots, NativeSex, Inebriation)
        PLAYER_BYTES_2                                                    = UnitField.UNIT_END + 0x0012, // Size: 1, Flags: PUBLIC Nested: (PvpTitle, ArenaFaction, PvpRank)
        PLAYER_DUEL_TEAM                                                  = UnitField.UNIT_END + 0x0013, // Size: 1, Flags: PUBLIC
        PLAYER_GUILD_TIMESTAMP                                            = UnitField.UNIT_END + 0x0014, // Size: 1, Flags: PUBLIC
        PLAYER_QUEST_LOG                                                  = UnitField.UNIT_END + 0x0015, // Size: 400, Flags: GROUP_ONLY
        PLAYER_VISIBLE_ITEM                                               = UnitField.UNIT_END + 0x01A5, // Size: 38, Flags: PUBLIC
        PLAYER_CHOSEN_TITLE                                               = UnitField.UNIT_END + 0x01CB, // Size: 1, Flags: PUBLIC
        PLAYER_FAKE_INEBRIATION                                           = UnitField.UNIT_END + 0x01CC, // Size: 1, Flags: PUBLIC
        PLAYER_FIELD_VIRTUAL_PLAYER_REALM                                 = UnitField.UNIT_END + 0x01CD, // Size: 1, Flags: PUBLIC
        PLAYER_FIELD_CURRENT_SPEC_ID                                      = UnitField.UNIT_END + 0x01CE, // Size: 1, Flags: PUBLIC
        PLAYER_FIELD_TAXI_MOUNT_ANIM_KIT_ID                               = UnitField.UNIT_END + 0x01CF, // Size: 1, Flags: PUBLIC
        PLAYER_FIELD_AVG_ITEM_LEVEL                                       = UnitField.UNIT_END + 0x01D0, // Size: 6, Flags: PUBLIC
        PLAYER_FIELD_CURRENT_BATTLE_PET_BREED_QUALITY                     = UnitField.UNIT_END + 0x01D6, // Size: 1, Flags: PUBLIC
        PLAYER_FIELD_HONOR_LEVEL                                          = UnitField.UNIT_END + 0x01D7, // Size: 1, Flags: PUBLIC
        PLAYER_FIELD_CUSTOMIZATION_CHOICES                                = UnitField.UNIT_END + 0x01D8, // Size: 72, Flags: PUBLIC
        PLAYER_END                                                        = UnitField.UNIT_END + 0x220,
    }

    public enum PlayerDynamicField
    {
        PLAYER_DYNAMIC_FIELD_ARENA_COOLDOWNS                              = UnitDynamicField.UNIT_DYNAMIC_END + 0x0000, // Flags: PUBLIC
        PLAYER_DYNAMIC_END                                                = UnitDynamicField.UNIT_DYNAMIC_END + 0x0001,
    }

    public enum ActivePlayerField
    {
        ACTIVE_PLAYER_FIELD_INV_SLOT_HEAD                                 = PlayerField.PLAYER_END + 0x0000, // Size: 516, Flags: PUBLIC
        ACTIVE_PLAYER_FIELD_FARSIGHT                                      = PlayerField.PLAYER_END + 0x0204, // Size: 4, Flags: PUBLIC
        ACTIVE_PLAYER_FIELD_COMBO_TARGET                                  = PlayerField.PLAYER_END + 0x0208, // Size: 4, Flags: PUBLIC
        ACTIVE_PLAYER_FIELD_SUMMONED_BATTLE_PET_ID                        = PlayerField.PLAYER_END + 0x020C, // Size: 4, Flags: PUBLIC
        ACTIVE_PLAYER_FIELD_KNOWN_TITLES                                  = PlayerField.PLAYER_END + 0x0210, // Size: 12, Flags: PUBLIC
        ACTIVE_PLAYER_FIELD_COINAGE                                       = PlayerField.PLAYER_END + 0x021C, // Size: 2, Flags: PUBLIC
        ACTIVE_PLAYER_FIELD_XP                                            = PlayerField.PLAYER_END + 0x021E, // Size: 1, Flags: PUBLIC
        ACTIVE_PLAYER_FIELD_NEXT_LEVEL_XP                                 = PlayerField.PLAYER_END + 0x021F, // Size: 1, Flags: PUBLIC
        ACTIVE_PLAYER_FIELD_TRIAL_XP                                      = PlayerField.PLAYER_END + 0x0220, // Size: 1, Flags: PUBLIC
        ACTIVE_PLAYER_FIELD_SKILL_LINEID                                  = PlayerField.PLAYER_END + 0x0221, // Size: 896, Flags: PUBLIC
        ACTIVE_PLAYER_FIELD_CHARACTER_POINTS                              = PlayerField.PLAYER_END + 0x05A1, // Size: 1, Flags: PUBLIC
        ACTIVE_PLAYER_FIELD_MAX_TALENT_TIERS                              = PlayerField.PLAYER_END + 0x05A2, // Size: 1, Flags: PUBLIC
        ACTIVE_PLAYER_FIELD_TRACK_CREATURES                               = PlayerField.PLAYER_END + 0x05A3, // Size: 1, Flags: PUBLIC
        ACTIVE_PLAYER_FIELD_TRACK_RESOURCES                               = PlayerField.PLAYER_END + 0x05A4, // Size: 2, Flags: PUBLIC
        ACTIVE_PLAYER_FIELD_EXPERTISE                                     = PlayerField.PLAYER_END + 0x05A6, // Size: 1, Flags: PUBLIC
        ACTIVE_PLAYER_FIELD_OFFHAND_EXPERTISE                             = PlayerField.PLAYER_END + 0x05A7, // Size: 1, Flags: PUBLIC
        ACTIVE_PLAYER_FIELD_RANGED_EXPERTISE                              = PlayerField.PLAYER_END + 0x05A8, // Size: 1, Flags: PUBLIC
        ACTIVE_PLAYER_FIELD_COMBAT_RATING_EXPERTISE                       = PlayerField.PLAYER_END + 0x05A9, // Size: 1, Flags: PUBLIC
        ACTIVE_PLAYER_FIELD_BLOCK_PERCENTAGE                              = PlayerField.PLAYER_END + 0x05AA, // Size: 1, Flags: PUBLIC
        ACTIVE_PLAYER_FIELD_DODGE_PERCENTAGE                              = PlayerField.PLAYER_END + 0x05AB, // Size: 1, Flags: PUBLIC
        ACTIVE_PLAYER_FIELD_DODGE_PERCENTAGE_FROM_ATTRIBUTE               = PlayerField.PLAYER_END + 0x05AC, // Size: 1, Flags: PUBLIC
        ACTIVE_PLAYER_FIELD_PARRY_PERCENTAGE                              = PlayerField.PLAYER_END + 0x05AD, // Size: 1, Flags: PUBLIC
        ACTIVE_PLAYER_FIELD_PARRY_PERCENTAGE_FROM_ATTRIBUTE               = PlayerField.PLAYER_END + 0x05AE, // Size: 1, Flags: PUBLIC
        ACTIVE_PLAYER_FIELD_CRIT_PERCENTAGE                               = PlayerField.PLAYER_END + 0x05AF, // Size: 1, Flags: PUBLIC
        ACTIVE_PLAYER_FIELD_RANGED_CRIT_PERCENTAGE                        = PlayerField.PLAYER_END + 0x05B0, // Size: 1, Flags: PUBLIC
        ACTIVE_PLAYER_FIELD_OFFHAND_CRIT_PERCENTAGE                       = PlayerField.PLAYER_END + 0x05B1, // Size: 1, Flags: PUBLIC
        ACTIVE_PLAYER_FIELD_SPELL_CRIT_PERCENTAGE1                        = PlayerField.PLAYER_END + 0x05B2, // Size: 7, Flags: PUBLIC
        ACTIVE_PLAYER_FIELD_SHIELD_BLOCK                                  = PlayerField.PLAYER_END + 0x05B9, // Size: 1, Flags: PUBLIC
        ACTIVE_PLAYER_FIELD_MASTERY                                       = PlayerField.PLAYER_END + 0x05BA, // Size: 1, Flags: PUBLIC
        ACTIVE_PLAYER_FIELD_SPEED                                         = PlayerField.PLAYER_END + 0x05BB, // Size: 1, Flags: PUBLIC
        ACTIVE_PLAYER_FIELD_AVOIDANCE                                     = PlayerField.PLAYER_END + 0x05BC, // Size: 1, Flags: PUBLIC
        ACTIVE_PLAYER_FIELD_STURDINESS                                    = PlayerField.PLAYER_END + 0x05BD, // Size: 1, Flags: PUBLIC
        ACTIVE_PLAYER_FIELD_VERSATILITY                                   = PlayerField.PLAYER_END + 0x05BE, // Size: 1, Flags: PUBLIC
        ACTIVE_PLAYER_FIELD_VERSATILITY_BONUS                             = PlayerField.PLAYER_END + 0x05BF, // Size: 1, Flags: PUBLIC
        ACTIVE_PLAYER_FIELD_PVP_POWER_DAMAGE                              = PlayerField.PLAYER_END + 0x05C0, // Size: 1, Flags: PUBLIC
        ACTIVE_PLAYER_FIELD_PVP_POWER_HEALING                             = PlayerField.PLAYER_END + 0x05C1, // Size: 1, Flags: PUBLIC
        ACTIVE_PLAYER_FIELD_EXPLORED_ZONES                                = PlayerField.PLAYER_END + 0x05C2, // Size: 480, Flags: PUBLIC
        ACTIVE_PLAYER_FIELD_REST_INFO                                     = PlayerField.PLAYER_END + 0x07A2, // Size: 4, Flags: PUBLIC
        ACTIVE_PLAYER_FIELD_MOD_DAMAGE_DONE_POS                           = PlayerField.PLAYER_END + 0x07A6, // Size: 7, Flags: PUBLIC
        ACTIVE_PLAYER_FIELD_MOD_DAMAGE_DONE_NEG                           = PlayerField.PLAYER_END + 0x07AD, // Size: 7, Flags: PUBLIC
        ACTIVE_PLAYER_FIELD_MOD_DAMAGE_DONE_PCT                           = PlayerField.PLAYER_END + 0x07B4, // Size: 7, Flags: PUBLIC
        ACTIVE_PLAYER_FIELD_MOD_HEALING_DONE_POS                          = PlayerField.PLAYER_END + 0x07BB, // Size: 1, Flags: PUBLIC
        ACTIVE_PLAYER_FIELD_MOD_HEALING_PCT                               = PlayerField.PLAYER_END + 0x07BC, // Size: 1, Flags: PUBLIC
        ACTIVE_PLAYER_FIELD_MOD_HEALING_DONE_PCT                          = PlayerField.PLAYER_END + 0x07BD, // Size: 1, Flags: PUBLIC
        ACTIVE_PLAYER_FIELD_MOD_PERIODIC_HEALING_DONE_PERCENT             = PlayerField.PLAYER_END + 0x07BE, // Size: 1, Flags: PUBLIC
        ACTIVE_PLAYER_FIELD_WEAPON_DMG_MULTIPLIERS                        = PlayerField.PLAYER_END + 0x07BF, // Size: 3, Flags: PUBLIC
        ACTIVE_PLAYER_FIELD_WEAPON_ATK_SPEED_MULTIPLIERS                  = PlayerField.PLAYER_END + 0x07C2, // Size: 3, Flags: PUBLIC
        ACTIVE_PLAYER_FIELD_MOD_SPELL_POWER_PCT                           = PlayerField.PLAYER_END + 0x07C5, // Size: 1, Flags: PUBLIC
        ACTIVE_PLAYER_FIELD_MOD_RESILIENCE_PERCENT                        = PlayerField.PLAYER_END + 0x07C6, // Size: 1, Flags: PUBLIC
        ACTIVE_PLAYER_FIELD_OVERRIDE_SPELL_POWER_BY_AP_PCT                = PlayerField.PLAYER_END + 0x07C7, // Size: 1, Flags: PUBLIC
        ACTIVE_PLAYER_FIELD_OVERRIDE_AP_BY_SPELL_POWER_PERCENT            = PlayerField.PLAYER_END + 0x07C8, // Size: 1, Flags: PUBLIC
        ACTIVE_PLAYER_FIELD_MOD_TARGET_RESISTANCE                         = PlayerField.PLAYER_END + 0x07C9, // Size: 1, Flags: PUBLIC
        ACTIVE_PLAYER_FIELD_MOD_TARGET_PHYSICAL_RESISTANCE                = PlayerField.PLAYER_END + 0x07CA, // Size: 1, Flags: PUBLIC
        ACTIVE_PLAYER_FIELD_LOCAL_FLAGS                                   = PlayerField.PLAYER_END + 0x07CB, // Size: 1, Flags: PUBLIC
        ACTIVE_PLAYER_FIELD_BYTES                                         = PlayerField.PLAYER_END + 0x07CC, // Size: 1, Flags: PUBLIC Nested: (GrantableLevels, MultiActionBars, LifetimeMaxRank, NumRespecs)
        ACTIVE_PLAYER_FIELD_AMMO_ID                                       = PlayerField.PLAYER_END + 0x07CD, // Size: 1, Flags: PUBLIC
        ACTIVE_PLAYER_FIELD_PVP_MEDALS                                    = PlayerField.PLAYER_END + 0x07CE, // Size: 1, Flags: PUBLIC
        ACTIVE_PLAYER_FIELD_BUYBACK_PRICE                                 = PlayerField.PLAYER_END + 0x07CF, // Size: 12, Flags: PUBLIC
        ACTIVE_PLAYER_FIELD_BUYBACK_TIMESTAMP                             = PlayerField.PLAYER_END + 0x07DB, // Size: 12, Flags: PUBLIC
        ACTIVE_PLAYER_FIELD_BYTES_2                                       = PlayerField.PLAYER_END + 0x07E7, // Size: 1, Flags: (All) Nested: (TodayHonorableKills, TodayDishonorableKills)
        ACTIVE_PLAYER_FIELD_BYTES_3                                       = PlayerField.PLAYER_END + 0x07E8, // Size: 1, Flags: (All) Nested: (YesterdayHonorableKills, YesterdayDishonorableKills)
        ACTIVE_PLAYER_FIELD_BYTES_4                                       = PlayerField.PLAYER_END + 0x07E9, // Size: 1, Flags: (All) Nested: (LastWeekHonorableKills, LastWeekDishonorableKills)
        ACTIVE_PLAYER_FIELD_BYTES_5                                       = PlayerField.PLAYER_END + 0x07EA, // Size: 1, Flags: (All) Nested: (ThisWeekHonorableKills, ThisWeekDishonorableKills)
        ACTIVE_PLAYER_FIELD_THIS_WEEK_CONTRIBUTION                        = PlayerField.PLAYER_END + 0x07EB, // Size: 1, Flags: PUBLIC
        ACTIVE_PLAYER_FIELD_LIFETIME_HONORABLE_KILLS                      = PlayerField.PLAYER_END + 0x07EC, // Size: 1, Flags: PUBLIC
        ACTIVE_PLAYER_FIELD_LIFETIME_DISHONORABLE_KILLS                   = PlayerField.PLAYER_END + 0x07ED, // Size: 1, Flags: PUBLIC
        ACTIVE_PLAYER_FIELD_YESTERDAY_CONTRIBUTION                        = PlayerField.PLAYER_END + 0x07EE, // Size: 1, Flags: PUBLIC
        ACTIVE_PLAYER_FIELD_LAST_WEEK_CONTRIBUTION                        = PlayerField.PLAYER_END + 0x07EF, // Size: 1, Flags: PUBLIC
        ACTIVE_PLAYER_FIELD_LAST_WEEK_RANK                                = PlayerField.PLAYER_END + 0x07F0, // Size: 1, Flags: PUBLIC
        ACTIVE_PLAYER_FIELD_WATCHED_FACTION_INDEX                         = PlayerField.PLAYER_END + 0x07F1, // Size: 1, Flags: PUBLIC
        ACTIVE_PLAYER_FIELD_COMBAT_RATINGS                                = PlayerField.PLAYER_END + 0x07F2, // Size: 32, Flags: PUBLIC
        ACTIVE_PLAYER_FIELD_PVP_INFO                                      = PlayerField.PLAYER_END + 0x0812, // Size: 72, Flags: PUBLIC
        ACTIVE_PLAYER_FIELD_MAX_LEVEL                                     = PlayerField.PLAYER_END + 0x085A, // Size: 1, Flags: PUBLIC
        ACTIVE_PLAYER_FIELD_SCALING_PLAYER_LEVEL_DELTA                    = PlayerField.PLAYER_END + 0x085B, // Size: 1, Flags: PUBLIC
        ACTIVE_PLAYER_FIELD_MAX_CREATURE_SCALING_LEVEL                    = PlayerField.PLAYER_END + 0x085C, // Size: 1, Flags: PUBLIC
        ACTIVE_PLAYER_FIELD_NO_REAGENT_COST_MASK                          = PlayerField.PLAYER_END + 0x085D, // Size: 4, Flags: PUBLIC
        ACTIVE_PLAYER_FIELD_PET_SPELL_POWER                               = PlayerField.PLAYER_END + 0x0861, // Size: 1, Flags: PUBLIC
        ACTIVE_PLAYER_FIELD_PROFESSION_SKILL_LINE                         = PlayerField.PLAYER_END + 0x0862, // Size: 2, Flags: PUBLIC
        ACTIVE_PLAYER_FIELD_UI_HIT_MODIFIER                               = PlayerField.PLAYER_END + 0x0864, // Size: 1, Flags: PUBLIC
        ACTIVE_PLAYER_FIELD_UI_SPELL_HIT_MODIFIER                         = PlayerField.PLAYER_END + 0x0865, // Size: 1, Flags: PUBLIC
        ACTIVE_PLAYER_FIELD_HOME_REALM_TIME_OFFSET                        = PlayerField.PLAYER_END + 0x0866, // Size: 1, Flags: PUBLIC
        ACTIVE_PLAYER_FIELD_MOD_PET_HASTE                                 = PlayerField.PLAYER_END + 0x0867, // Size: 1, Flags: PUBLIC
        ACTIVE_PLAYER_FIELD_BYTES_6                                       = PlayerField.PLAYER_END + 0x0868, // Size: 1, Flags: PUBLIC Nested: (LocalRegenFlags, AuraVision, NumBackpackSlots)
        ACTIVE_PLAYER_FIELD_OVERRIDE_SPELLS_ID                            = PlayerField.PLAYER_END + 0x0869, // Size: 1, Flags: PUBLIC, URGENT_SELF_ONLY
        ACTIVE_PLAYER_FIELD_LFG_BONUS_FACTION_ID                          = PlayerField.PLAYER_END + 0x086A, // Size: 1, Flags: PUBLIC
        ACTIVE_PLAYER_FIELD_LOOT_SPEC_ID                                  = PlayerField.PLAYER_END + 0x086B, // Size: 1, Flags: PUBLIC
        ACTIVE_PLAYER_FIELD_OVERRIDE_ZONE_PVP_TYPE                        = PlayerField.PLAYER_END + 0x086C, // Size: 1, Flags: PUBLIC, URGENT_SELF_ONLY
        ACTIVE_PLAYER_FIELD_BAG_SLOT_FLAGS                                = PlayerField.PLAYER_END + 0x086D, // Size: 4, Flags: PUBLIC
        ACTIVE_PLAYER_FIELD_BANK_BAG_SLOT_FLAGS                           = PlayerField.PLAYER_END + 0x0871, // Size: 6, Flags: PUBLIC
        ACTIVE_PLAYER_FIELD_QUEST_COMPLETED                               = PlayerField.PLAYER_END + 0x0877, // Size: 1750, Flags: PUBLIC
        ACTIVE_PLAYER_FIELD_HONOR                                         = PlayerField.PLAYER_END + 0x0F4D, // Size: 1, Flags: PUBLIC
        ACTIVE_PLAYER_FIELD_HONOR_NEXT_LEVEL                              = PlayerField.PLAYER_END + 0x0F4E, // Size: 1, Flags: PUBLIC
        ACTIVE_PLAYER_FIELD_PVP_TIER_MAX_FROM_WINS                        = PlayerField.PLAYER_END + 0x0F4F, // Size: 1, Flags: PUBLIC
        ACTIVE_PLAYER_FIELD_PVP_LAST_WEEKS_TIER_MAX_FROM_WINS             = PlayerField.PLAYER_END + 0x0F50, // Size: 1, Flags: PUBLIC
        ACTIVE_PLAYER_FIELD_BYTES_7                                       = PlayerField.PLAYER_END + 0x0F51, // Size: 1, Flags: PUBLIC Nested: (InsertItemsLeftToRight, PvpRankProgress)
        ACTIVE_PLAYER_END                                                 = PlayerField.PLAYER_END + 0x0F52,
    }

    public enum ActivePlayerDynamicField
    {
        ACTIVE_PLAYER_DYNAMIC_FIELD_RESEARCH                              = PlayerDynamicField.PLAYER_DYNAMIC_END + 0x0000, // Flags: PUBLIC
        ACTIVE_PLAYER_DYNAMIC_FIELD_RESEARCH_SITES                        = PlayerDynamicField.PLAYER_DYNAMIC_END + 0x0001, // Flags: PUBLIC
        ACTIVE_PLAYER_DYNAMIC_FIELD_RESEARCH_SITE_PROGRESS                = PlayerDynamicField.PLAYER_DYNAMIC_END + 0x0002, // Flags: PUBLIC
        ACTIVE_PLAYER_DYNAMIC_FIELD_DAILY_QUESTS_COMPLETED                = PlayerDynamicField.PLAYER_DYNAMIC_END + 0x0003, // Flags: PUBLIC
        ACTIVE_PLAYER_DYNAMIC_FIELD_AVAILABLE_QUEST_LINE_X_QUEST_IDS      = PlayerDynamicField.PLAYER_DYNAMIC_END + 0x0004, // Flags: PUBLIC
        ACTIVE_PLAYER_DYNAMIC_FIELD_HEIRLOOMS                             = PlayerDynamicField.PLAYER_DYNAMIC_END + 0x0005, // Flags: PUBLIC
        ACTIVE_PLAYER_DYNAMIC_FIELD_HEIRLOOM_FLAGS                        = PlayerDynamicField.PLAYER_DYNAMIC_END + 0x0006, // Flags: PUBLIC
        ACTIVE_PLAYER_DYNAMIC_FIELD_TOYS                                  = PlayerDynamicField.PLAYER_DYNAMIC_END + 0x0007, // Flags: PUBLIC
        ACTIVE_PLAYER_DYNAMIC_FIELD_TRANSMOG                              = PlayerDynamicField.PLAYER_DYNAMIC_END + 0x0008, // Flags: PUBLIC
        ACTIVE_PLAYER_DYNAMIC_FIELD_CONDITIONAL_TRANSMOG                  = PlayerDynamicField.PLAYER_DYNAMIC_END + 0x0009, // Flags: PUBLIC
        ACTIVE_PLAYER_DYNAMIC_FIELD_SELF_RES_SPELLS                       = PlayerDynamicField.PLAYER_DYNAMIC_END + 0x000A, // Flags: PUBLIC
        ACTIVE_PLAYER_DYNAMIC_FIELD_CHARACTER_RESTRICTIONS                = PlayerDynamicField.PLAYER_DYNAMIC_END + 0x000B, // Flags: PUBLIC
        ACTIVE_PLAYER_DYNAMIC_FIELD_SPELL_FLAT_MOD_BY_LABEL               = PlayerDynamicField.PLAYER_DYNAMIC_END + 0x000C, // Flags: PUBLIC
        ACTIVE_PLAYER_DYNAMIC_FIELD_SPELL_PCT_MOD_BY_LABEL                = PlayerDynamicField.PLAYER_DYNAMIC_END + 0x000D, // Flags: PUBLIC
        ACTIVE_PLAYER_DYNAMIC_END                                         = PlayerDynamicField.PLAYER_DYNAMIC_END + 0x000E,
    }

    public enum GameObjectField
    {
        GAMEOBJECT_FIELD_CREATED_BY                                 = ObjectField.OBJECT_END + 0x000, // Size: 4, Flags: PUBLIC
        GAMEOBJECT_GUILD                                            = ObjectField.OBJECT_END + 0x004, // Size: 4, Flags: PUBLIC
        GAMEOBJECT_DISPLAYID                                        = ObjectField.OBJECT_END + 0x008, // Size: 1, Flags: DYNAMIC, URGENT
        GAMEOBJECT_FLAGS                                            = ObjectField.OBJECT_END + 0x009, // Size: 1, Flags: PUBLIC, URGENT
        GAMEOBJECT_PARENTROTATION                                   = ObjectField.OBJECT_END + 0x00A, // Size: 4, Flags: PUBLIC
        GAMEOBJECT_FACTION                                          = ObjectField.OBJECT_END + 0x00E, // Size: 1, Flags: PUBLIC
        GAMEOBJECT_LEVEL                                            = ObjectField.OBJECT_END + 0x00F, // Size: 1, Flags: PUBLIC
        GAMEOBJECT_BYTES_1                                          = ObjectField.OBJECT_END + 0x010, // Size: 1, Flags: PUBLIC, URGENT
        GAMEOBJECT_SPELL_VISUAL_ID                                  = ObjectField.OBJECT_END + 0x011, // Size: 1, Flags: PUBLIC, DYNAMIC, URGENT
        GAMEOBJECT_STATE_SPELL_VISUAL_ID                            = ObjectField.OBJECT_END + 0x012, // Size: 1, Flags: DYNAMIC, URGENT
        GAMEOBJECT_STATE_ANIM_ID                                    = ObjectField.OBJECT_END + 0x013, // Size: 1, Flags: DYNAMIC, URGENT
        GAMEOBJECT_STATE_ANIM_KIT_ID                                = ObjectField.OBJECT_END + 0x014, // Size: 1, Flags: DYNAMIC, URGENT
        GAMEOBJECT_STATE_WORLD_EFFECT_ID                            = ObjectField.OBJECT_END + 0x015, // Size: 4, Flags: DYNAMIC, URGENT
        GAMEOBJECT_CUSTOM_PARAM                                     = ObjectField.OBJECT_END + 0x019, // Size: 1, Flags: PUBLIC, URGENT
        GAMEOBJECT_END                                              = ObjectField.OBJECT_END + 0x01A,
    }

    public enum GameObjectDynamicField
    {
        GAMEOBJECT_DYNAMIC_ENABLE_DOODAD_SETS                       = ObjectDynamicField.OBJECT_DYNAMIC_END + 0x000, // Flags: PUBLIC
        GAMEOBJECT_DYNAMIC_END                                      = ObjectDynamicField.OBJECT_DYNAMIC_END + 0x001,
    }

    public enum DynamicObjectField
    {
        DYNAMICOBJECT_CASTER                                        = ObjectField.OBJECT_END + 0x000, // Size: 4, Flags: PUBLIC
        DYNAMICOBJECT_TYPE                                          = ObjectField.OBJECT_END + 0x004, // Size: 1, Flags: PUBLIC
        DYNAMICOBJECT_SPELL_X_SPELL_VISUAL_ID                       = ObjectField.OBJECT_END + 0x005, // Size: 1, Flags: PUBLIC
        DYNAMICOBJECT_SPELLID                                       = ObjectField.OBJECT_END + 0x006, // Size: 1, Flags: PUBLIC
        DYNAMICOBJECT_RADIUS                                        = ObjectField.OBJECT_END + 0x007, // Size: 1, Flags: PUBLIC
        DYNAMICOBJECT_CASTTIME                                      = ObjectField.OBJECT_END + 0x008, // Size: 1, Flags: PUBLIC
        DYNAMICOBJECT_END                                           = ObjectField.OBJECT_END + 0x009,
    }

    public enum DynamicObjectDynamicField
    {
        DYNAMICOBJECT_DYNAMIC_END                                   = ObjectDynamicField.OBJECT_DYNAMIC_END + 0x000,
    }

    public enum CorpseField
    {
        CORPSE_FIELD_OWNER                                          = ObjectField.OBJECT_END + 0x0000, // Size: 4, Flags: PUBLIC
        CORPSE_FIELD_PARTY_GUID                                     = ObjectField.OBJECT_END + 0x0004, // Size: 4, Flags: PUBLIC
        CORPSE_FIELD_GUILD_GUID                                     = ObjectField.OBJECT_END + 0x0008, // Size: 4, Flags: PUBLIC
        CORPSE_FIELD_DISPLAY_ID                                     = ObjectField.OBJECT_END + 0x000C, // Size: 1, Flags: PUBLIC
        CORPSE_FIELD_ITEMS                                          = ObjectField.OBJECT_END + 0x000D, // Size: 19, Flags: PUBLIC
        CORPSE_FIELD_BYTES_1                                        = ObjectField.OBJECT_END + 0x0020, // Size: 1, Flags: PUBLIC Nested: (RaceID, Sex, ClassID, Padding)
        CORPSE_FIELD_FLAGS                                          = ObjectField.OBJECT_END + 0x0021, // Size: 1, Flags: PUBLIC
        CORPSE_FIELD_DYNAMIC_FLAGS                                  = ObjectField.OBJECT_END + 0x0022, // Size: 1, Flags: DYNAMIC
        CORPSE_FIELD_FACTION_TEMPLATE                               = ObjectField.OBJECT_END + 0x0023, // Size: 1, Flags: PUBLIC
        CORPSE_FIELD_CUSTOMIZATION_CHOICES                          = ObjectField.OBJECT_END + 0x0024, // Size: 72, Flags: PUBLIC
        CORPSE_END                                                  = ObjectField.OBJECT_END + 0x006C,
    }

    public enum CorpseDynamicField
    {
        CORPSE_DYNAMIC_END                                          = ObjectDynamicField.OBJECT_DYNAMIC_END + 0x000,
    }

    public enum AreaTriggerField
    {
        AREATRIGGER_OVERRIDE_SCALE_CURVE                            = ObjectField.OBJECT_END + 0x000, // Size: 7, Flags: PUBLIC, URGENT
        AREATRIGGER_EXTRA_SCALE_CURVE                               = ObjectField.OBJECT_END + 0x007, // Size: 7, Flags: PUBLIC, URGENT
        AREATRIGGER_CASTER                                          = ObjectField.OBJECT_END + 0x00E, // Size: 4, Flags: PUBLIC
        AREATRIGGER_DURATION                                        = ObjectField.OBJECT_END + 0x012, // Size: 1, Flags: PUBLIC
        AREATRIGGER_TIME_TO_TARGET                                  = ObjectField.OBJECT_END + 0x013, // Size: 1, Flags: PUBLIC, URGENT
        AREATRIGGER_TIME_TO_TARGET_SCALE                            = ObjectField.OBJECT_END + 0x014, // Size: 1, Flags: PUBLIC, URGENT
        AREATRIGGER_TIME_TO_TARGET_EXTRA_SCALE                      = ObjectField.OBJECT_END + 0x015, // Size: 1, Flags: PUBLIC, URGENT
        AREATRIGGER_SPELLID                                         = ObjectField.OBJECT_END + 0x016, // Size: 1, Flags: PUBLIC
        AREATRIGGER_SPELL_FOR_VISUALS                               = ObjectField.OBJECT_END + 0x017, // Size: 1, Flags: PUBLIC
        AREATRIGGER_SPELL_X_SPELL_VISUAL_ID                         = ObjectField.OBJECT_END + 0x018, // Size: 1, Flags: PUBLIC
        AREATRIGGER_BOUNDS_RADIUS_2D                                = ObjectField.OBJECT_END + 0x019, // Size: 1, Flags: DYNAMIC, URGENT
        AREATRIGGER_DECAL_PROPERTIES_ID                             = ObjectField.OBJECT_END + 0x01A, // Size: 1, Flags: PUBLIC
        AREATRIGGER_CREATING_EFFECT_GUID                            = ObjectField.OBJECT_END + 0x01B, // Size: 4, Flags: PUBLIC
        AREATRIGGER_END                                             = ObjectField.OBJECT_END + 0x01F,
    }

    public enum AreaTriggerDynamicField
    {
        AREATRIGGER_DYNAMIC_END                                     = ObjectDynamicField.OBJECT_DYNAMIC_END + 0x000,
    }

    public enum SceneObjectField
    {
        SCENEOBJECT_FIELD_SCRIPT_PACKAGE_ID                         = ObjectField.OBJECT_END + 0x000, // Size: 1, Flags: PUBLIC
        SCENEOBJECT_FIELD_RND_SEED_VAL                              = ObjectField.OBJECT_END + 0x001, // Size: 1, Flags: PUBLIC
        SCENEOBJECT_FIELD_CREATEDBY                                 = ObjectField.OBJECT_END + 0x002, // Size: 4, Flags: PUBLIC
        SCENEOBJECT_FIELD_SCENE_TYPE                                = ObjectField.OBJECT_END + 0x006, // Size: 1, Flags: PUBLIC
        SCENEOBJECT_END                                             = ObjectField.OBJECT_END + 0x007,
    }

    public enum SceneObjectDynamicField
    {
        SCENEOBJECT_DYNAMIC_END                                     = ObjectDynamicField.OBJECT_DYNAMIC_END + 0x000,
    }

    public enum ConversationField
    {
        CONVERSATION_LAST_LINE_END_TIME                             = ObjectField.OBJECT_END + 0x000, // Size: 1, Flags: DYNAMIC
        CONVERSATION_END                                            = ObjectField.OBJECT_END + 0x001,
    }

    public enum ConversationDynamicField
    {
        CONVERSATION_DYNAMIC_FIELD_ACTORS                           = ObjectDynamicField.OBJECT_DYNAMIC_END + 0x000, // Flags: PUBLIC
        CONVERSATION_DYNAMIC_FIELD_LINES                            = ObjectDynamicField.OBJECT_DYNAMIC_END + 0x001, // Flags: 0x100
        CONVERSATION_DYNAMIC_END                                    = ObjectDynamicField.OBJECT_DYNAMIC_END + 0x002,
    }

    // ReSharper restore InconsistentNaming
}
