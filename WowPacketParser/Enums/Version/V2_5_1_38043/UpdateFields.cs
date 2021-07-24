namespace WowPacketParserModule.V2_5_1_38043.Enums
{
    public enum ObjectField
    {
        OBJECT_FIELD_GUID                                                 = 0x0000,      // Size: 4, Flags: (All)
        OBJECT_FIELD_ENTRY_ID                                             = 0x0004,      // Size: 1, Flags: (ViewerDependent)
        OBJECT_FIELD_DYNAMIC_FLAGS                                        = 0x0005,      // Size: 1, Flags: (ViewerDependent, Urgent)
        OBJECT_FIELD_SCALE                                                = 0x0006,      // Size: 1, Flags: (All)
        OBJECT_FIELD_END                                                  = 0x0007,
    }

    public enum ObjectDynamicField
    {
        OBJECT_DYNAMIC_FIELD_END                                          = 0x0000,
    }

    public enum ItemField
    {
        ITEM_FIELD_OWNER                                                  = ObjectField.OBJECT_FIELD_END + 0x0000,      // Size: 4, Flags: (All)
        ITEM_FIELD_CONTAINED_IN                                           = ObjectField.OBJECT_FIELD_END + 0x0004,      // Size: 4, Flags: (All)
        ITEM_FIELD_CREATOR                                                = ObjectField.OBJECT_FIELD_END + 0x0008,      // Size: 4, Flags: (All)
        ITEM_FIELD_GIFT_CREATOR                                           = ObjectField.OBJECT_FIELD_END + 0x000C,      // Size: 4, Flags: (All)
        ITEM_FIELD_STACK_COUNT                                            = ObjectField.OBJECT_FIELD_END + 0x0010,      // Size: 1, Flags: (Owner)
        ITEM_FIELD_EXPIRATION                                             = ObjectField.OBJECT_FIELD_END + 0x0011,      // Size: 1, Flags: (Owner)
        ITEM_FIELD_SPELL_CHARGES                                          = ObjectField.OBJECT_FIELD_END + 0x0012,      // Size: 5, Flags: (Owner)
        ITEM_FIELD_DYNAMIC_FLAGS                                          = ObjectField.OBJECT_FIELD_END + 0x0017,      // Size: 1, Flags: (All)
        ITEM_FIELD_ENCHANTMENT                                            = ObjectField.OBJECT_FIELD_END + 0x0018,      // Size: 39, Flags: (All)
        ITEM_FIELD_PROPERTY_SEED                                          = ObjectField.OBJECT_FIELD_END + 0x003F,      // Size: 1, Flags: (All)
        ITEM_FIELD_RANDOM_PROPERTIES_ID                                   = ObjectField.OBJECT_FIELD_END + 0x0040,      // Size: 1, Flags: (All)
        ITEM_FIELD_DURABILITY                                             = ObjectField.OBJECT_FIELD_END + 0x0041,      // Size: 1, Flags: (Owner)
        ITEM_FIELD_MAX_DURABILITY                                         = ObjectField.OBJECT_FIELD_END + 0x0042,      // Size: 1, Flags: (Owner)
        ITEM_FIELD_CREATE_PLAYED_TIME                                     = ObjectField.OBJECT_FIELD_END + 0x0043,      // Size: 1, Flags: (All)
        ITEM_FIELD_MODIFIERS_MASK                                         = ObjectField.OBJECT_FIELD_END + 0x0044,      // Size: 1, Flags: (Owner)
        ITEM_FIELD_CONTEXT                                                = ObjectField.OBJECT_FIELD_END + 0x0045,      // Size: 1, Flags: (All)
        ITEM_FIELD_ARTIFACT_XP                                            = ObjectField.OBJECT_FIELD_END + 0x0046,      // Size: 2, Flags: (Owner)
        ITEM_FIELD_ITEM_APPEARANCE_MOD_ID                                 = ObjectField.OBJECT_FIELD_END + 0x0048,      // Size: 1, Flags: (Owner)
        ITEM_FIELD_END                                                    = ObjectField.OBJECT_FIELD_END + 0x0049,
    }

    public enum ItemDynamicField
    {
        ITEM_DYNAMIC_FIELD_MODIFIERS                                      = ObjectDynamicField.OBJECT_DYNAMIC_FIELD_END + 0x0000,      // Flags: (Owner)
        ITEM_DYNAMIC_FIELD_BONUS_LIST_IDS                                 = ObjectDynamicField.OBJECT_DYNAMIC_FIELD_END + 0x0001,      // Flags: (Owner, Unk0x100)
        ITEM_DYNAMIC_FIELD_ARTIFACT_POWERS                                = ObjectDynamicField.OBJECT_DYNAMIC_FIELD_END + 0x0002,      // Flags: (Owner)
        ITEM_DYNAMIC_FIELD_GEMS                                           = ObjectDynamicField.OBJECT_DYNAMIC_FIELD_END + 0x0003,      // Flags: (Owner)
        ITEM_DYNAMIC_FIELD_END                                            = ObjectDynamicField.OBJECT_DYNAMIC_FIELD_END + 0x0004,
    }

    public enum ContainerField
    {
        CONTAINER_FIELD_SLOTS                                             = ItemField.ITEM_FIELD_END + 0x0000,      // Size: 144, Flags: (All)
        CONTAINER_FIELD_NUM_SLOTS                                         = ItemField.ITEM_FIELD_END + 0x0090,      // Size: 1, Flags: (All)
        CONTAINER_FIELD_END                                               = ItemField.ITEM_FIELD_END + 0x0091,
    }

    public enum ContainerDynamicField
    {
        CONTAINER_DYNAMIC_FIELD_END                                       = ItemDynamicField.ITEM_DYNAMIC_FIELD_END + 0x0000,
    }

    public enum UnitField
    {
        UNIT_FIELD_CHARM                                                  = ObjectField.OBJECT_FIELD_END + 0x0000,      // Size: 4, Flags: (All)
        UNIT_FIELD_SUMMON                                                 = ObjectField.OBJECT_FIELD_END + 0x0004,      // Size: 4, Flags: (All)
        UNIT_FIELD_CRITTER                                                = ObjectField.OBJECT_FIELD_END + 0x0008,      // Size: 4, Flags: (Self)
        UNIT_FIELD_CHARMED_BY                                             = ObjectField.OBJECT_FIELD_END + 0x000C,      // Size: 4, Flags: (All)
        UNIT_FIELD_SUMMONED_BY                                            = ObjectField.OBJECT_FIELD_END + 0x0010,      // Size: 4, Flags: (All)
        UNIT_FIELD_CREATED_BY                                             = ObjectField.OBJECT_FIELD_END + 0x0014,      // Size: 4, Flags: (All)
        UNIT_FIELD_DEMON_CREATOR                                          = ObjectField.OBJECT_FIELD_END + 0x0018,      // Size: 4, Flags: (All)
        UNIT_FIELD_LOOK_AT_CONTROLLER_TARGET                              = ObjectField.OBJECT_FIELD_END + 0x001C,      // Size: 4, Flags: (All)
        UNIT_FIELD_TARGET                                                 = ObjectField.OBJECT_FIELD_END + 0x0020,      // Size: 4, Flags: (All)
        UNIT_FIELD_BATTLE_PET_COMPANION_GUID                              = ObjectField.OBJECT_FIELD_END + 0x0024,      // Size: 4, Flags: (All)
        UNIT_FIELD_BATTLE_PET_DBID                                        = ObjectField.OBJECT_FIELD_END + 0x0028,      // Size: 2, Flags: (All)
        UNIT_FIELD_CHANNEL_DATA                                           = ObjectField.OBJECT_FIELD_END + 0x002A,      // Size: 2, Flags: (All, Urgent)
        UNIT_FIELD_SUMMONED_BY_HOME_REALM                                 = ObjectField.OBJECT_FIELD_END + 0x002C,      // Size: 1, Flags: (All)
        UNIT_FIELD_BYTES_1                                                = ObjectField.OBJECT_FIELD_END + 0x002D,      // Size: 1, Flags: (All) Nested: (Race, ClassId, PlayerClassId, Sex)
        UNIT_FIELD_DISPLAY_POWER                                          = ObjectField.OBJECT_FIELD_END + 0x002E,      // Size: 1, Flags: (All)
        UNIT_FIELD_OVERRIDE_DISPLAY_POWER_ID                              = ObjectField.OBJECT_FIELD_END + 0x002F,      // Size: 1, Flags: (All)
        UNIT_FIELD_HEALTH                                                 = ObjectField.OBJECT_FIELD_END + 0x0030,      // Size: 2, Flags: (ViewerDependent)
        UNIT_FIELD_POWER                                                  = ObjectField.OBJECT_FIELD_END + 0x0032,      // Size: 6, Flags: (All, UrgentSelfOnly)
        UNIT_FIELD_MAX_HEALTH                                             = ObjectField.OBJECT_FIELD_END + 0x0038,      // Size: 2, Flags: (ViewerDependent)
        UNIT_FIELD_MAX_POWER                                              = ObjectField.OBJECT_FIELD_END + 0x003A,      // Size: 6, Flags: (All)
        UNIT_FIELD_MOD_POWER_REGEN                                        = ObjectField.OBJECT_FIELD_END + 0x0040,      // Size: 6, Flags: (Self, Owner, UnitAll)
        UNIT_FIELD_LEVEL                                                  = ObjectField.OBJECT_FIELD_END + 0x0046,      // Size: 1, Flags: (All)
        UNIT_FIELD_EFFECTIVE_LEVEL                                        = ObjectField.OBJECT_FIELD_END + 0x0047,      // Size: 1, Flags: (All)
        UNIT_FIELD_CONTENT_TUNING_ID                                      = ObjectField.OBJECT_FIELD_END + 0x0048,      // Size: 1, Flags: (All)
        UNIT_FIELD_SCALING_LEVEL_MIN                                      = ObjectField.OBJECT_FIELD_END + 0x0049,      // Size: 1, Flags: (All)
        UNIT_FIELD_SCALING_LEVEL_MAX                                      = ObjectField.OBJECT_FIELD_END + 0x004A,      // Size: 1, Flags: (All)
        UNIT_FIELD_SCALING_LEVEL_DELTA                                    = ObjectField.OBJECT_FIELD_END + 0x004B,      // Size: 1, Flags: (All)
        UNIT_FIELD_SCALING_FACTION_GROUP                                  = ObjectField.OBJECT_FIELD_END + 0x004C,      // Size: 1, Flags: (All)
        UNIT_FIELD_SCALING_HEALTH_ITEM_LEVEL_CURVE_ID                     = ObjectField.OBJECT_FIELD_END + 0x004D,      // Size: 1, Flags: (All)
        UNIT_FIELD_SCALING_DAMAGE_ITEM_LEVEL_CURVE_ID                     = ObjectField.OBJECT_FIELD_END + 0x004E,      // Size: 1, Flags: (All)
        UNIT_FIELD_FACTION_TEMPLATE                                       = ObjectField.OBJECT_FIELD_END + 0x004F,      // Size: 1, Flags: (All)
        UNIT_FIELD_VIRTUAL_ITEMS                                          = ObjectField.OBJECT_FIELD_END + 0x0050,      // Size: 6, Flags: (All)
        UNIT_FIELD_FLAGS                                                  = ObjectField.OBJECT_FIELD_END + 0x0056,      // Size: 1, Flags: (All, Urgent)
        UNIT_FIELD_FLAGS_2                                                = ObjectField.OBJECT_FIELD_END + 0x0057,      // Size: 1, Flags: (All, Urgent)
        UNIT_FIELD_FLAGS_3                                                = ObjectField.OBJECT_FIELD_END + 0x0058,      // Size: 1, Flags: (All, Urgent)
        UNIT_FIELD_AURA_STATE                                             = ObjectField.OBJECT_FIELD_END + 0x0059,      // Size: 1, Flags: (All)
        UNIT_FIELD_ATTACK_ROUND_BASE_TIME                                 = ObjectField.OBJECT_FIELD_END + 0x005A,      // Size: 2, Flags: (All)
        UNIT_FIELD_RANGED_ATTACK_ROUND_BASE_TIME                          = ObjectField.OBJECT_FIELD_END + 0x005C,      // Size: 1, Flags: (Self)
        UNIT_FIELD_BOUNDING_RADIUS                                        = ObjectField.OBJECT_FIELD_END + 0x005D,      // Size: 1, Flags: (All)
        UNIT_FIELD_COMBAT_REACH                                           = ObjectField.OBJECT_FIELD_END + 0x005E,      // Size: 1, Flags: (All)
        UNIT_FIELD_DISPLAY_ID                                             = ObjectField.OBJECT_FIELD_END + 0x005F,      // Size: 1, Flags: (ViewerDependent, Urgent)
        UNIT_FIELD_DISPLAY_SCALE                                          = ObjectField.OBJECT_FIELD_END + 0x0060,      // Size: 1, Flags: (ViewerDependent, Urgent)
        UNIT_FIELD_NATIVE_DISPLAY_ID                                      = ObjectField.OBJECT_FIELD_END + 0x0061,      // Size: 1, Flags: (All, Urgent)
        UNIT_FIELD_NATIVE_X_DISPLAY_SCALE                                 = ObjectField.OBJECT_FIELD_END + 0x0062,      // Size: 1, Flags: (All, Urgent)
        UNIT_FIELD_MOUNT_DISPLAY_ID                                       = ObjectField.OBJECT_FIELD_END + 0x0063,      // Size: 1, Flags: (All, Urgent)
        UNIT_FIELD_MIN_DAMAGE                                             = ObjectField.OBJECT_FIELD_END + 0x0064,      // Size: 1, Flags: (Self, Owner, Empath)
        UNIT_FIELD_MAX_DAMAGE                                             = ObjectField.OBJECT_FIELD_END + 0x0065,      // Size: 1, Flags: (Self, Owner, Empath)
        UNIT_FIELD_MIN_OFF_HAND_DAMAGE                                    = ObjectField.OBJECT_FIELD_END + 0x0066,      // Size: 1, Flags: (Self, Owner, Empath)
        UNIT_FIELD_MAX_OFF_HAND_DAMAGE                                    = ObjectField.OBJECT_FIELD_END + 0x0067,      // Size: 1, Flags: (Self, Owner, Empath)
        UNIT_FIELD_BYTES_2                                                = ObjectField.OBJECT_FIELD_END + 0x0068,      // Size: 1, Flags: (All) Nested: (StandState, PetLoyaltyIndex, VisFlags, AnimTier)
        UNIT_FIELD_PET_NUMBER                                             = ObjectField.OBJECT_FIELD_END + 0x0069,      // Size: 1, Flags: (All)
        UNIT_FIELD_PET_NAME_TIMESTAMP                                     = ObjectField.OBJECT_FIELD_END + 0x006A,      // Size: 1, Flags: (All)
        UNIT_FIELD_PET_EXPERIENCE                                         = ObjectField.OBJECT_FIELD_END + 0x006B,      // Size: 1, Flags: (Owner)
        UNIT_FIELD_PET_NEXT_LEVEL_EXPERIENCE                              = ObjectField.OBJECT_FIELD_END + 0x006C,      // Size: 1, Flags: (Owner)
        UNIT_FIELD_MOD_CASTING_SPEED                                      = ObjectField.OBJECT_FIELD_END + 0x006D,      // Size: 1, Flags: (All)
        UNIT_FIELD_MOD_SPELL_HASTE                                        = ObjectField.OBJECT_FIELD_END + 0x006E,      // Size: 1, Flags: (All)
        UNIT_FIELD_MOD_HASTE                                              = ObjectField.OBJECT_FIELD_END + 0x006F,      // Size: 1, Flags: (All)
        UNIT_FIELD_MOD_RANGED_HASTE                                       = ObjectField.OBJECT_FIELD_END + 0x0070,      // Size: 1, Flags: (All)
        UNIT_FIELD_MOD_HASTE_REGEN                                        = ObjectField.OBJECT_FIELD_END + 0x0071,      // Size: 1, Flags: (All)
        UNIT_FIELD_MOD_TIME_RATE                                          = ObjectField.OBJECT_FIELD_END + 0x0072,      // Size: 1, Flags: (All)
        UNIT_FIELD_CREATED_BY_SPELL                                       = ObjectField.OBJECT_FIELD_END + 0x0073,      // Size: 1, Flags: (All)
        UNIT_FIELD_NPC_FLAGS                                              = ObjectField.OBJECT_FIELD_END + 0x0074,      // Size: 2, Flags: (All, ViewerDependent)
        UNIT_FIELD_EMOTE_STATE                                            = ObjectField.OBJECT_FIELD_END + 0x0076,      // Size: 1, Flags: (All)
        UNIT_FIELD_BYTES_3                                                = ObjectField.OBJECT_FIELD_END + 0x0077,      // Size: 1, Flags: (Owner) Nested: (TrainingPointsUsed, TrainingPointsTotal)
        UNIT_FIELD_STATS                                                  = ObjectField.OBJECT_FIELD_END + 0x0078,      // Size: 5, Flags: (Self, Owner)
        UNIT_FIELD_STAT_POS_BUFF                                          = ObjectField.OBJECT_FIELD_END + 0x007D,      // Size: 5, Flags: (Self, Owner)
        UNIT_FIELD_STAT_NEG_BUFF                                          = ObjectField.OBJECT_FIELD_END + 0x0082,      // Size: 5, Flags: (Self, Owner)
        UNIT_FIELD_RESISTANCES                                            = ObjectField.OBJECT_FIELD_END + 0x0087,      // Size: 7, Flags: (Self, Owner, Empath)
        UNIT_FIELD_RESISTANCE_BUFF_MODS_POSITIVE                          = ObjectField.OBJECT_FIELD_END + 0x008E,      // Size: 7, Flags: (Self, Owner)
        UNIT_FIELD_RESISTANCE_BUFF_MODS_NEGATIVE                          = ObjectField.OBJECT_FIELD_END + 0x0095,      // Size: 7, Flags: (Self, Owner)
        UNIT_FIELD_BASE_MANA                                              = ObjectField.OBJECT_FIELD_END + 0x009C,      // Size: 1, Flags: (All)
        UNIT_FIELD_BASE_HEALTH                                            = ObjectField.OBJECT_FIELD_END + 0x009D,      // Size: 1, Flags: (Self, Owner)
        UNIT_FIELD_BYTES_4                                                = ObjectField.OBJECT_FIELD_END + 0x009E,      // Size: 1, Flags: (All) Nested: (SheatheState, PvpFlags, PetFlags, ShapeshiftForm)
        UNIT_FIELD_ATTACK_POWER                                           = ObjectField.OBJECT_FIELD_END + 0x009F,      // Size: 1, Flags: (Self, Owner)
        UNIT_FIELD_ATTACK_POWER_MOD_POS                                   = ObjectField.OBJECT_FIELD_END + 0x00A0,      // Size: 1, Flags: (Self, Owner)
        UNIT_FIELD_ATTACK_POWER_MOD_NEG                                   = ObjectField.OBJECT_FIELD_END + 0x00A1,      // Size: 1, Flags: (Self, Owner)
        UNIT_FIELD_ATTACK_POWER_MULTIPLIER                                = ObjectField.OBJECT_FIELD_END + 0x00A2,      // Size: 1, Flags: (Self, Owner)
        UNIT_FIELD_RANGED_ATTACK_POWER                                    = ObjectField.OBJECT_FIELD_END + 0x00A3,      // Size: 1, Flags: (Self, Owner)
        UNIT_FIELD_RANGED_ATTACK_POWER_MOD_POS                            = ObjectField.OBJECT_FIELD_END + 0x00A4,      // Size: 1, Flags: (Self, Owner)
        UNIT_FIELD_RANGED_ATTACK_POWER_MOD_NEG                            = ObjectField.OBJECT_FIELD_END + 0x00A5,      // Size: 1, Flags: (Self, Owner)
        UNIT_FIELD_RANGED_ATTACK_POWER_MULTIPLIER                         = ObjectField.OBJECT_FIELD_END + 0x00A6,      // Size: 1, Flags: (Self, Owner)
        UNIT_FIELD_MAIN_HAND_WEAPON_ATTACK_POWER                          = ObjectField.OBJECT_FIELD_END + 0x00A7,      // Size: 1, Flags: (Self, Owner)
        UNIT_FIELD_OFF_HAND_WEAPON_ATTACK_POWER                           = ObjectField.OBJECT_FIELD_END + 0x00A8,      // Size: 1, Flags: (Self, Owner)
        UNIT_FIELD_RANGED_WEAPON_ATTACK_POWER                             = ObjectField.OBJECT_FIELD_END + 0x00A9,      // Size: 1, Flags: (Self, Owner)
        UNIT_FIELD_SET_ATTACK_SPEED_AURA                                  = ObjectField.OBJECT_FIELD_END + 0x00AA,      // Size: 1, Flags: (Self, Owner)
        UNIT_FIELD_LIFESTEAL                                              = ObjectField.OBJECT_FIELD_END + 0x00AB,      // Size: 1, Flags: (Self, Owner)
        UNIT_FIELD_MIN_RANGED_DAMAGE                                      = ObjectField.OBJECT_FIELD_END + 0x00AC,      // Size: 1, Flags: (Self, Owner)
        UNIT_FIELD_MAX_RANGED_DAMAGE                                      = ObjectField.OBJECT_FIELD_END + 0x00AD,      // Size: 1, Flags: (Self, Owner)
        UNIT_FIELD_POWER_COST_MODIFIER                                    = ObjectField.OBJECT_FIELD_END + 0x00AE,      // Size: 7, Flags: (Self, Owner)
        UNIT_FIELD_POWER_COST_MULTIPLIER                                  = ObjectField.OBJECT_FIELD_END + 0x00B5,      // Size: 7, Flags: (Self, Owner)
        UNIT_FIELD_MAX_HEALTH_MODIFIER                                    = ObjectField.OBJECT_FIELD_END + 0x00BC,      // Size: 1, Flags: (Self, Owner)
        UNIT_FIELD_HOVER_HEIGHT                                           = ObjectField.OBJECT_FIELD_END + 0x00BD,      // Size: 1, Flags: (All)
        UNIT_FIELD_MIN_ITEM_LEVEL_CUTOFF                                  = ObjectField.OBJECT_FIELD_END + 0x00BE,      // Size: 1, Flags: (All)
        UNIT_FIELD_MIN_ITEM_LEVEL                                         = ObjectField.OBJECT_FIELD_END + 0x00BF,      // Size: 1, Flags: (All)
        UNIT_FIELD_MAX_ITEM_LEVEL                                         = ObjectField.OBJECT_FIELD_END + 0x00C0,      // Size: 1, Flags: (All)
        UNIT_FIELD_WILD_BATTLE_PET_LEVEL                                  = ObjectField.OBJECT_FIELD_END + 0x00C1,      // Size: 1, Flags: (All)
        UNIT_FIELD_BATTLE_PET_COMPANION_NAME_TIMESTAMP                    = ObjectField.OBJECT_FIELD_END + 0x00C2,      // Size: 1, Flags: (All)
        UNIT_FIELD_INTERACT_SPELL_ID                                      = ObjectField.OBJECT_FIELD_END + 0x00C3,      // Size: 1, Flags: (All)
        UNIT_FIELD_STATE_SPELL_VISUAL_ID                                  = ObjectField.OBJECT_FIELD_END + 0x00C4,      // Size: 1, Flags: (ViewerDependent, Urgent)
        UNIT_FIELD_STATE_ANIM_ID                                          = ObjectField.OBJECT_FIELD_END + 0x00C5,      // Size: 1, Flags: (ViewerDependent, Urgent)
        UNIT_FIELD_STATE_ANIM_KIT_ID                                      = ObjectField.OBJECT_FIELD_END + 0x00C6,      // Size: 1, Flags: (ViewerDependent, Urgent)
        UNIT_FIELD_STATE_WORLD_EFFECT_ID                                  = ObjectField.OBJECT_FIELD_END + 0x00C7,      // Size: 4, Flags: (ViewerDependent, Urgent)
        UNIT_FIELD_SCALE_DURATION                                         = ObjectField.OBJECT_FIELD_END + 0x00CB,      // Size: 1, Flags: (All)
        UNIT_FIELD_LOOKS_LIKE_MOUNT_ID                                    = ObjectField.OBJECT_FIELD_END + 0x00CC,      // Size: 1, Flags: (All)
        UNIT_FIELD_LOOKS_LIKE_CREATURE_ID                                 = ObjectField.OBJECT_FIELD_END + 0x00CD,      // Size: 1, Flags: (All)
        UNIT_FIELD_LOOK_AT_CONTROLLER_ID                                  = ObjectField.OBJECT_FIELD_END + 0x00CE,      // Size: 1, Flags: (All)
        UNIT_FIELD_GUILD_GUID                                             = ObjectField.OBJECT_FIELD_END + 0x00CF,      // Size: 4, Flags: (All)
        UNIT_FIELD_END                                                    = ObjectField.OBJECT_FIELD_END + 0x00D3,
    }

    public enum UnitDynamicField
    {
        UNIT_DYNAMIC_FIELD_PASSIVE_SPELLS                                 = ObjectDynamicField.OBJECT_DYNAMIC_FIELD_END + 0x0000,      // Flags: (All, Urgent)
        UNIT_DYNAMIC_FIELD_WORLD_EFFECTS                                  = ObjectDynamicField.OBJECT_DYNAMIC_FIELD_END + 0x0001,      // Flags: (All, Urgent)
        UNIT_DYNAMIC_FIELD_CHANNEL_OBJECTS                                = ObjectDynamicField.OBJECT_DYNAMIC_FIELD_END + 0x0002,      // Flags: (All, Urgent)
        UNIT_DYNAMIC_FIELD_END                                            = ObjectDynamicField.OBJECT_DYNAMIC_FIELD_END + 0x0003,
    }

    public enum PlayerField
    {
        PLAYER_FIELD_DUEL_ARBITER                                         = UnitField.UNIT_FIELD_END + 0x0000,      // Size: 4, Flags: (All)
        PLAYER_FIELD_WOW_ACCOUNT                                          = UnitField.UNIT_FIELD_END + 0x0004,      // Size: 4, Flags: (All)
        PLAYER_FIELD_LOOT_TARGET_GUID                                     = UnitField.UNIT_FIELD_END + 0x0008,      // Size: 4, Flags: (All)
        PLAYER_FIELD_PLAYER_FLAGS                                         = UnitField.UNIT_FIELD_END + 0x000C,      // Size: 1, Flags: (All)
        PLAYER_FIELD_PLAYER_FLAGS_EX                                      = UnitField.UNIT_FIELD_END + 0x000D,      // Size: 1, Flags: (All)
        PLAYER_FIELD_GUILD_RANK_ID                                        = UnitField.UNIT_FIELD_END + 0x000E,      // Size: 1, Flags: (All)
        PLAYER_FIELD_GUILD_DELETE_DATE                                    = UnitField.UNIT_FIELD_END + 0x000F,      // Size: 1, Flags: (All)
        PLAYER_FIELD_GUILD_LEVEL                                          = UnitField.UNIT_FIELD_END + 0x0010,      // Size: 1, Flags: (All)
        PLAYER_FIELD_BYTES_1                                              = UnitField.UNIT_FIELD_END + 0x0011,      // Size: 1, Flags: (All) Nested: (PartyType, NumBankSlots, NativeSex, Inebriation)
        PLAYER_FIELD_BYTES_2                                              = UnitField.UNIT_FIELD_END + 0x0012,      // Size: 1, Flags: (All) Nested: (PvpTitle, ArenaFaction, PvpRank)
        PLAYER_FIELD_DUEL_TEAM                                            = UnitField.UNIT_FIELD_END + 0x0013,      // Size: 1, Flags: (All)
        PLAYER_FIELD_GUILD_TIME_STAMP                                     = UnitField.UNIT_FIELD_END + 0x0014,      // Size: 1, Flags: (All)
        PLAYER_FIELD_QUEST_LOG                                            = UnitField.UNIT_FIELD_END + 0x0015,      // Size: 320, Flags: (Party)
        PLAYER_FIELD_VISIBLE_ITEMS                                        = UnitField.UNIT_FIELD_END + 0x0155,      // Size: 38, Flags: (All)
        PLAYER_FIELD_PLAYER_TITLE                                         = UnitField.UNIT_FIELD_END + 0x017B,      // Size: 1, Flags: (All)
        PLAYER_FIELD_FAKE_INEBRIATION                                     = UnitField.UNIT_FIELD_END + 0x017C,      // Size: 1, Flags: (All)
        PLAYER_FIELD_VIRTUAL_PLAYER_REALM                                 = UnitField.UNIT_FIELD_END + 0x017D,      // Size: 1, Flags: (All)
        PLAYER_FIELD_CURRENT_SPEC_ID                                      = UnitField.UNIT_FIELD_END + 0x017E,      // Size: 1, Flags: (All)
        PLAYER_FIELD_TAXI_MOUNT_ANIM_KIT_ID                               = UnitField.UNIT_FIELD_END + 0x017F,      // Size: 1, Flags: (All)
        PLAYER_FIELD_AVG_ITEM_LEVEL                                       = UnitField.UNIT_FIELD_END + 0x0180,      // Size: 4, Flags: (All)
        PLAYER_FIELD_CURRENT_BATTLE_PET_BREED_QUALITY                     = UnitField.UNIT_FIELD_END + 0x0184,      // Size: 1, Flags: (All)
        PLAYER_FIELD_HONOR_LEVEL                                          = UnitField.UNIT_FIELD_END + 0x0185,      // Size: 1, Flags: (All)
        PLAYER_FIELD_CUSTOMIZATION_CHOICES                                = UnitField.UNIT_FIELD_END + 0x0186,      // Size: 72, Flags: (All)
        PLAYER_FIELD_END                                                  = UnitField.UNIT_FIELD_END + 0x01CE,
    }

    public enum PlayerDynamicField
    {
        PLAYER_DYNAMIC_FIELD_ARENA_COOLDOWNS                              = UnitDynamicField.UNIT_DYNAMIC_FIELD_END + 0x0000,      // Flags: (All)
        PLAYER_DYNAMIC_FIELD_END                                          = UnitDynamicField.UNIT_DYNAMIC_FIELD_END + 0x0001,
    }

    public enum ActivePlayerField
    {
        ACTIVE_PLAYER_FIELD_INV_SLOTS                                     = PlayerField.PLAYER_FIELD_END + 0x0000,      // Size: 520, Flags: (All)
        ACTIVE_PLAYER_FIELD_FARSIGHT_OBJECT                               = PlayerField.PLAYER_FIELD_END + 0x0208,      // Size: 4, Flags: (All)
        ACTIVE_PLAYER_FIELD_COMBO_TARGET                                  = PlayerField.PLAYER_FIELD_END + 0x020C,      // Size: 4, Flags: (All)
        ACTIVE_PLAYER_FIELD_SUMMONED_BATTLE_PET_GUID                      = PlayerField.PLAYER_FIELD_END + 0x0210,      // Size: 4, Flags: (All)
        ACTIVE_PLAYER_FIELD_KNOWN_TITLES                                  = PlayerField.PLAYER_FIELD_END + 0x0214,      // Size: 12, Flags: (All)
        ACTIVE_PLAYER_FIELD_COINAGE                                       = PlayerField.PLAYER_FIELD_END + 0x0220,      // Size: 2, Flags: (All)
        ACTIVE_PLAYER_FIELD_XP                                            = PlayerField.PLAYER_FIELD_END + 0x0222,      // Size: 1, Flags: (All)
        ACTIVE_PLAYER_FIELD_NEXT_LEVEL_XP                                 = PlayerField.PLAYER_FIELD_END + 0x0223,      // Size: 1, Flags: (All)
        ACTIVE_PLAYER_FIELD_TRIAL_XP                                      = PlayerField.PLAYER_FIELD_END + 0x0224,      // Size: 1, Flags: (All)
        ACTIVE_PLAYER_FIELD_SKILL                                         = PlayerField.PLAYER_FIELD_END + 0x0225,      // Size: 896, Flags: (All)
        ACTIVE_PLAYER_FIELD_CHARACTER_POINTS                              = PlayerField.PLAYER_FIELD_END + 0x05A5,      // Size: 1, Flags: (All)
        ACTIVE_PLAYER_FIELD_MAX_TALENT_TIERS                              = PlayerField.PLAYER_FIELD_END + 0x05A6,      // Size: 1, Flags: (All)
        ACTIVE_PLAYER_FIELD_TRACK_CREATURE_MASK                           = PlayerField.PLAYER_FIELD_END + 0x05A7,      // Size: 1, Flags: (All)
        ACTIVE_PLAYER_FIELD_TRACK_RESOURCE_MASK                           = PlayerField.PLAYER_FIELD_END + 0x05A8,      // Size: 2, Flags: (All)
        ACTIVE_PLAYER_FIELD_MAINHAND_EXPERTISE                            = PlayerField.PLAYER_FIELD_END + 0x05AA,      // Size: 1, Flags: (All)
        ACTIVE_PLAYER_FIELD_OFFHAND_EXPERTISE                             = PlayerField.PLAYER_FIELD_END + 0x05AB,      // Size: 1, Flags: (All)
        ACTIVE_PLAYER_FIELD_RANGED_EXPERTISE                              = PlayerField.PLAYER_FIELD_END + 0x05AC,      // Size: 1, Flags: (All)
        ACTIVE_PLAYER_FIELD_COMBAT_RATING_EXPERTISE                       = PlayerField.PLAYER_FIELD_END + 0x05AD,      // Size: 1, Flags: (All)
        ACTIVE_PLAYER_FIELD_BLOCK_PERCENTAGE                              = PlayerField.PLAYER_FIELD_END + 0x05AE,      // Size: 1, Flags: (All)
        ACTIVE_PLAYER_FIELD_DODGE_PERCENTAGE                              = PlayerField.PLAYER_FIELD_END + 0x05AF,      // Size: 1, Flags: (All)
        ACTIVE_PLAYER_FIELD_DODGE_PERCENTAGE_FROM_ATTRIBUTE               = PlayerField.PLAYER_FIELD_END + 0x05B0,      // Size: 1, Flags: (All)
        ACTIVE_PLAYER_FIELD_PARRY_PERCENTAGE                              = PlayerField.PLAYER_FIELD_END + 0x05B1,      // Size: 1, Flags: (All)
        ACTIVE_PLAYER_FIELD_PARRY_PERCENTAGE_FROM_ATTRIBUTE               = PlayerField.PLAYER_FIELD_END + 0x05B2,      // Size: 1, Flags: (All)
        ACTIVE_PLAYER_FIELD_CRIT_PERCENTAGE                               = PlayerField.PLAYER_FIELD_END + 0x05B3,      // Size: 1, Flags: (All)
        ACTIVE_PLAYER_FIELD_RANGED_CRIT_PERCENTAGE                        = PlayerField.PLAYER_FIELD_END + 0x05B4,      // Size: 1, Flags: (All)
        ACTIVE_PLAYER_FIELD_OFFHAND_CRIT_PERCENTAGE                       = PlayerField.PLAYER_FIELD_END + 0x05B5,      // Size: 1, Flags: (All)
        ACTIVE_PLAYER_FIELD_SPELL_CRIT_PERCENTAGE                         = PlayerField.PLAYER_FIELD_END + 0x05B6,      // Size: 1, Flags: (All)
        ACTIVE_PLAYER_FIELD_SHIELD_BLOCK                                  = PlayerField.PLAYER_FIELD_END + 0x05B7,      // Size: 1, Flags: (All)
        ACTIVE_PLAYER_FIELD_MASTERY                                       = PlayerField.PLAYER_FIELD_END + 0x05B8,      // Size: 1, Flags: (All)
        ACTIVE_PLAYER_FIELD_SPEED                                         = PlayerField.PLAYER_FIELD_END + 0x05B9,      // Size: 1, Flags: (All)
        ACTIVE_PLAYER_FIELD_AVOIDANCE                                     = PlayerField.PLAYER_FIELD_END + 0x05BA,      // Size: 1, Flags: (All)
        ACTIVE_PLAYER_FIELD_STURDINESS                                    = PlayerField.PLAYER_FIELD_END + 0x05BB,      // Size: 1, Flags: (All)
        ACTIVE_PLAYER_FIELD_VERSATILITY                                   = PlayerField.PLAYER_FIELD_END + 0x05BC,      // Size: 1, Flags: (All)
        ACTIVE_PLAYER_FIELD_VERSATILITY_BONUS                             = PlayerField.PLAYER_FIELD_END + 0x05BD,      // Size: 1, Flags: (All)
        ACTIVE_PLAYER_FIELD_PVP_POWER_DAMAGE                              = PlayerField.PLAYER_FIELD_END + 0x05BE,      // Size: 1, Flags: (All)
        ACTIVE_PLAYER_FIELD_PVP_POWER_HEALING                             = PlayerField.PLAYER_FIELD_END + 0x05BF,      // Size: 1, Flags: (All)
        ACTIVE_PLAYER_FIELD_EXPLORED_ZONES                                = PlayerField.PLAYER_FIELD_END + 0x05C0,      // Size: 384, Flags: (All)
        ACTIVE_PLAYER_FIELD_REST_INFO                                     = PlayerField.PLAYER_FIELD_END + 0x0740,      // Size: 4, Flags: (All)
        ACTIVE_PLAYER_FIELD_MOD_DAMAGE_DONE_POS                           = PlayerField.PLAYER_FIELD_END + 0x0744,      // Size: 7, Flags: (All)
        ACTIVE_PLAYER_FIELD_MOD_DAMAGE_DONE_NEG                           = PlayerField.PLAYER_FIELD_END + 0x074B,      // Size: 7, Flags: (All)
        ACTIVE_PLAYER_FIELD_MOD_DAMAGE_DONE_PERCENT                       = PlayerField.PLAYER_FIELD_END + 0x0752,      // Size: 7, Flags: (All)
        ACTIVE_PLAYER_FIELD_MOD_HEALING_DONE_POS                          = PlayerField.PLAYER_FIELD_END + 0x0759,      // Size: 1, Flags: (All)
        ACTIVE_PLAYER_FIELD_MOD_HEALING_PERCENT                           = PlayerField.PLAYER_FIELD_END + 0x075A,      // Size: 1, Flags: (All)
        ACTIVE_PLAYER_FIELD_MOD_HEALING_DONE_PERCENT                      = PlayerField.PLAYER_FIELD_END + 0x075B,      // Size: 1, Flags: (All)
        ACTIVE_PLAYER_FIELD_MOD_PERIODIC_HEALING_DONE_PERCENT             = PlayerField.PLAYER_FIELD_END + 0x075C,      // Size: 1, Flags: (All)
        ACTIVE_PLAYER_FIELD_WEAPON_DMG_MULTIPLIERS                        = PlayerField.PLAYER_FIELD_END + 0x075D,      // Size: 3, Flags: (All)
        ACTIVE_PLAYER_FIELD_WEAPON_ATK_SPEED_MULTIPLIERS                  = PlayerField.PLAYER_FIELD_END + 0x0760,      // Size: 3, Flags: (All)
        ACTIVE_PLAYER_FIELD_MOD_SPELL_POWER_PERCENT                       = PlayerField.PLAYER_FIELD_END + 0x0763,      // Size: 1, Flags: (All)
        ACTIVE_PLAYER_FIELD_MOD_RESILIENCE_PERCENT                        = PlayerField.PLAYER_FIELD_END + 0x0764,      // Size: 1, Flags: (All)
        ACTIVE_PLAYER_FIELD_OVERRIDE_SPELL_POWER_BY_AP_PERCENT            = PlayerField.PLAYER_FIELD_END + 0x0765,      // Size: 1, Flags: (All)
        ACTIVE_PLAYER_FIELD_OVERRIDE_AP_BY_SPELL_POWER_PERCENT            = PlayerField.PLAYER_FIELD_END + 0x0766,      // Size: 1, Flags: (All)
        ACTIVE_PLAYER_FIELD_MOD_TARGET_RESISTANCE                         = PlayerField.PLAYER_FIELD_END + 0x0767,      // Size: 1, Flags: (All)
        ACTIVE_PLAYER_FIELD_MOD_TARGET_PHYSICAL_RESISTANCE                = PlayerField.PLAYER_FIELD_END + 0x0768,      // Size: 1, Flags: (All)
        ACTIVE_PLAYER_FIELD_LOCAL_FLAGS                                   = PlayerField.PLAYER_FIELD_END + 0x0769,      // Size: 1, Flags: (All)
        ACTIVE_PLAYER_FIELD_BYTES_1                                       = PlayerField.PLAYER_FIELD_END + 0x076A,      // Size: 1, Flags: (All) Nested: (GrantableLevels, MultiActionBars, LifetimeMaxRank, NumRespecs)
        ACTIVE_PLAYER_FIELD_AMMO_ID                                       = PlayerField.PLAYER_FIELD_END + 0x076B,      // Size: 1, Flags: (All)
        ACTIVE_PLAYER_FIELD_PVP_MEDALS                                    = PlayerField.PLAYER_FIELD_END + 0x076C,      // Size: 1, Flags: (All)
        ACTIVE_PLAYER_FIELD_BUYBACK_PRICE                                 = PlayerField.PLAYER_FIELD_END + 0x076D,      // Size: 12, Flags: (All)
        ACTIVE_PLAYER_FIELD_BUYBACK_TIMESTAMP                             = PlayerField.PLAYER_FIELD_END + 0x0779,      // Size: 12, Flags: (All)
        ACTIVE_PLAYER_FIELD_SESSION_HONORABLE_KILLS                       = PlayerField.PLAYER_FIELD_END + 0x0785,      // Size: 1, Flags: (All)
        ACTIVE_PLAYER_FIELD_BYTES_2                                       = PlayerField.PLAYER_FIELD_END + 0x0786,      // Size: 1, Flags: (All) Nested: (YesterdayHonorableKills, LastWeekHonorableKills)
        ACTIVE_PLAYER_FIELD_THIS_WEEK_HONORABLE_KILLS                     = PlayerField.PLAYER_FIELD_END + 0x0787,      // Size: 1, Flags: (All)
        ACTIVE_PLAYER_FIELD_THIS_WEEK_CONTRIBUTION                        = PlayerField.PLAYER_FIELD_END + 0x0788,      // Size: 1, Flags: (All)
        ACTIVE_PLAYER_FIELD_LIFETIME_HONORABLE_KILLS                      = PlayerField.PLAYER_FIELD_END + 0x0789,      // Size: 1, Flags: (All)
        ACTIVE_PLAYER_FIELD_YESTERDAY_CONTRIBUTION                        = PlayerField.PLAYER_FIELD_END + 0x078A,      // Size: 1, Flags: (All)
        ACTIVE_PLAYER_FIELD_LAST_WEEK_CONTRIBUTION                        = PlayerField.PLAYER_FIELD_END + 0x078B,      // Size: 1, Flags: (All)
        ACTIVE_PLAYER_FIELD_LAST_WEEK_RANK                                = PlayerField.PLAYER_FIELD_END + 0x078C,      // Size: 1, Flags: (All)
        ACTIVE_PLAYER_FIELD_WATCHED_FACTION_INDEX                         = PlayerField.PLAYER_FIELD_END + 0x078D,      // Size: 1, Flags: (All)
        ACTIVE_PLAYER_FIELD_COMBAT_RATINGS                                = PlayerField.PLAYER_FIELD_END + 0x078E,      // Size: 32, Flags: (All)
        ACTIVE_PLAYER_FIELD_PVP_INFO                                      = PlayerField.PLAYER_FIELD_END + 0x07AE,      // Size: 54, Flags: (All)
        ACTIVE_PLAYER_FIELD_MAX_LEVEL                                     = PlayerField.PLAYER_FIELD_END + 0x07E4,      // Size: 1, Flags: (All)
        ACTIVE_PLAYER_FIELD_SCALING_PLAYER_LEVEL_DELTA                    = PlayerField.PLAYER_FIELD_END + 0x07E5,      // Size: 1, Flags: (All)
        ACTIVE_PLAYER_FIELD_MAX_CREATURE_SCALING_LEVEL                    = PlayerField.PLAYER_FIELD_END + 0x07E6,      // Size: 1, Flags: (All)
        ACTIVE_PLAYER_FIELD_NO_REAGENT_COST_MASK                          = PlayerField.PLAYER_FIELD_END + 0x07E7,      // Size: 4, Flags: (All)
        ACTIVE_PLAYER_FIELD_PET_SPELL_POWER                               = PlayerField.PLAYER_FIELD_END + 0x07EB,      // Size: 1, Flags: (All)
        ACTIVE_PLAYER_FIELD_PROFESSION_SKILL_LINE                         = PlayerField.PLAYER_FIELD_END + 0x07EC,      // Size: 2, Flags: (All)
        ACTIVE_PLAYER_FIELD_UI_HIT_MODIFIER                               = PlayerField.PLAYER_FIELD_END + 0x07EE,      // Size: 1, Flags: (All)
        ACTIVE_PLAYER_FIELD_UI_SPELL_HIT_MODIFIER                         = PlayerField.PLAYER_FIELD_END + 0x07EF,      // Size: 1, Flags: (All)
        ACTIVE_PLAYER_FIELD_HOME_REALM_TIME_OFFSET                        = PlayerField.PLAYER_FIELD_END + 0x07F0,      // Size: 1, Flags: (All)
        ACTIVE_PLAYER_FIELD_MOD_PET_HASTE                                 = PlayerField.PLAYER_FIELD_END + 0x07F1,      // Size: 1, Flags: (All)
        ACTIVE_PLAYER_FIELD_BYTES_3                                       = PlayerField.PLAYER_FIELD_END + 0x07F2,      // Size: 1, Flags: (All) Nested: (LocalRegenFlags, AuraVision, NumBackpackSlots)
        ACTIVE_PLAYER_FIELD_OVERRIDE_SPELLS_ID                            = PlayerField.PLAYER_FIELD_END + 0x07F3,      // Size: 1, Flags: (All, UrgentSelfOnly)
        ACTIVE_PLAYER_FIELD_LFG_BONUS_FACTION_ID                          = PlayerField.PLAYER_FIELD_END + 0x07F4,      // Size: 1, Flags: (All)
        ACTIVE_PLAYER_FIELD_LOOT_SPEC_ID                                  = PlayerField.PLAYER_FIELD_END + 0x07F5,      // Size: 1, Flags: (All)
        ACTIVE_PLAYER_FIELD_OVERRIDE_ZONE_PVP_TYPE                        = PlayerField.PLAYER_FIELD_END + 0x07F6,      // Size: 1, Flags: (All, UrgentSelfOnly)
        ACTIVE_PLAYER_FIELD_BAG_SLOT_FLAGS                                = PlayerField.PLAYER_FIELD_END + 0x07F7,      // Size: 4, Flags: (All)
        ACTIVE_PLAYER_FIELD_BANK_BAG_SLOT_FLAGS                           = PlayerField.PLAYER_FIELD_END + 0x07FB,      // Size: 7, Flags: (All)
        ACTIVE_PLAYER_FIELD_QUEST_COMPLETED                               = PlayerField.PLAYER_FIELD_END + 0x0802,      // Size: 1750, Flags: (All)
        ACTIVE_PLAYER_FIELD_HONOR                                         = PlayerField.PLAYER_FIELD_END + 0x0ED8,      // Size: 1, Flags: (All)
        ACTIVE_PLAYER_FIELD_HONOR_NEXT_LEVEL                              = PlayerField.PLAYER_FIELD_END + 0x0ED9,      // Size: 1, Flags: (All)
        ACTIVE_PLAYER_FIELD_PVP_TIER_MAX_FROM_WINS                        = PlayerField.PLAYER_FIELD_END + 0x0EDA,      // Size: 1, Flags: (All)
        ACTIVE_PLAYER_FIELD_PVP_LAST_WEEKS_TIER_MAX_FROM_WINS             = PlayerField.PLAYER_FIELD_END + 0x0EDB,      // Size: 1, Flags: (All)
        ACTIVE_PLAYER_FIELD_BYTES_4                                       = PlayerField.PLAYER_FIELD_END + 0x0EDC,      // Size: 1, Flags: (All) Nested: (InsertItemsLeftToRight, PvpRankProgress)
        ACTIVE_PLAYER_FIELD_END                                           = PlayerField.PLAYER_FIELD_END + 0x0EDD,
    }

    public enum ActivePlayerDynamicField
    {
        ACTIVE_PLAYER_DYNAMIC_FIELD_RESEARCH                              = PlayerDynamicField.PLAYER_DYNAMIC_FIELD_END + 0x0000,      // Flags: (All)
        ACTIVE_PLAYER_DYNAMIC_FIELD_RESEARCH_SITES                        = PlayerDynamicField.PLAYER_DYNAMIC_FIELD_END + 0x0001,      // Flags: (All)
        ACTIVE_PLAYER_DYNAMIC_FIELD_RESEARCH_SITE_PROGRESS                = PlayerDynamicField.PLAYER_DYNAMIC_FIELD_END + 0x0002,      // Flags: (All)
        ACTIVE_PLAYER_DYNAMIC_FIELD_DAILY_QUESTS_COMPLETED                = PlayerDynamicField.PLAYER_DYNAMIC_FIELD_END + 0x0003,      // Flags: (All)
        ACTIVE_PLAYER_DYNAMIC_FIELD_AVAILABLE_QUEST_LINE_X_QUEST_IDS      = PlayerDynamicField.PLAYER_DYNAMIC_FIELD_END + 0x0004,      // Flags: (All)
        ACTIVE_PLAYER_DYNAMIC_FIELD_HEIRLOOMS                             = PlayerDynamicField.PLAYER_DYNAMIC_FIELD_END + 0x0005,      // Flags: (All)
        ACTIVE_PLAYER_DYNAMIC_FIELD_HEIRLOOM_FLAGS                        = PlayerDynamicField.PLAYER_DYNAMIC_FIELD_END + 0x0006,      // Flags: (All)
        ACTIVE_PLAYER_DYNAMIC_FIELD_TOYS                                  = PlayerDynamicField.PLAYER_DYNAMIC_FIELD_END + 0x0007,      // Flags: (All)
        ACTIVE_PLAYER_DYNAMIC_FIELD_TRANSMOG                              = PlayerDynamicField.PLAYER_DYNAMIC_FIELD_END + 0x0008,      // Flags: (All)
        ACTIVE_PLAYER_DYNAMIC_FIELD_CONDITIONAL_TRANSMOG                  = PlayerDynamicField.PLAYER_DYNAMIC_FIELD_END + 0x0009,      // Flags: (All)
        ACTIVE_PLAYER_DYNAMIC_FIELD_SELF_RES_SPELLS                       = PlayerDynamicField.PLAYER_DYNAMIC_FIELD_END + 0x000A,      // Flags: (All)
        ACTIVE_PLAYER_DYNAMIC_FIELD_CHARACTER_RESTRICTIONS                = PlayerDynamicField.PLAYER_DYNAMIC_FIELD_END + 0x000B,      // Flags: (All)
        ACTIVE_PLAYER_DYNAMIC_FIELD_SPELL_FLAT_MOD_BY_LABEL               = PlayerDynamicField.PLAYER_DYNAMIC_FIELD_END + 0x000C,      // Flags: (All)
        ACTIVE_PLAYER_DYNAMIC_FIELD_SPELL_PCT_MOD_BY_LABEL                = PlayerDynamicField.PLAYER_DYNAMIC_FIELD_END + 0x000D,      // Flags: (All)
        ACTIVE_PLAYER_DYNAMIC_FIELD_END                                   = PlayerDynamicField.PLAYER_DYNAMIC_FIELD_END + 0x000E,
    }

    public enum GameObjectField
    {
        GAME_OBJECT_FIELD_CREATED_BY                                      = ObjectField.OBJECT_FIELD_END + 0x0000,      // Size: 4, Flags: (All)
        GAME_OBJECT_FIELD_GUILD_GUID                                      = ObjectField.OBJECT_FIELD_END + 0x0004,      // Size: 4, Flags: (All)
        GAME_OBJECT_FIELD_DISPLAY_ID                                      = ObjectField.OBJECT_FIELD_END + 0x0008,      // Size: 1, Flags: (ViewerDependent, Urgent)
        GAME_OBJECT_FIELD_FLAGS                                           = ObjectField.OBJECT_FIELD_END + 0x0009,      // Size: 1, Flags: (All, Urgent)
        GAME_OBJECT_FIELD_PARENT_ROTATION                                 = ObjectField.OBJECT_FIELD_END + 0x000A,      // Size: 4, Flags: (All)
        GAME_OBJECT_FIELD_FACTION_TEMPLATE                                = ObjectField.OBJECT_FIELD_END + 0x000E,      // Size: 1, Flags: (All)
        GAME_OBJECT_FIELD_LEVEL                                           = ObjectField.OBJECT_FIELD_END + 0x000F,      // Size: 1, Flags: (All)
        GAME_OBJECT_FIELD_BYTES_1                                         = ObjectField.OBJECT_FIELD_END + 0x0010,      // Size: 1, Flags: (All, Urgent) Nested: (State, TypeID, ArtKit, PercentHealth)
        GAME_OBJECT_FIELD_SPELL_VISUAL_ID                                 = ObjectField.OBJECT_FIELD_END + 0x0011,      // Size: 1, Flags: (All, ViewerDependent, Urgent)
        GAME_OBJECT_FIELD_STATE_SPELL_VISUAL_ID                           = ObjectField.OBJECT_FIELD_END + 0x0012,      // Size: 1, Flags: (ViewerDependent, Urgent)
        GAME_OBJECT_FIELD_SPAWN_TRACKING_STATE_ANIM_ID                    = ObjectField.OBJECT_FIELD_END + 0x0013,      // Size: 1, Flags: (ViewerDependent, Urgent)
        GAME_OBJECT_FIELD_SPAWN_TRACKING_STATE_ANIM_KIT_ID                = ObjectField.OBJECT_FIELD_END + 0x0014,      // Size: 1, Flags: (ViewerDependent, Urgent)
        GAME_OBJECT_FIELD_STATE_WORLD_EFFECT_ID                           = ObjectField.OBJECT_FIELD_END + 0x0015,      // Size: 4, Flags: (ViewerDependent, Urgent)
        GAME_OBJECT_FIELD_CUSTOM_PARAM                                    = ObjectField.OBJECT_FIELD_END + 0x0019,      // Size: 1, Flags: (All, Urgent)
        GAME_OBJECT_FIELD_END                                             = ObjectField.OBJECT_FIELD_END + 0x001A,
    }

    public enum GameObjectDynamicField
    {
        GAME_OBJECT_DYNAMIC_FIELD_ENABLE_DOODAD_SETS                      = ObjectDynamicField.OBJECT_DYNAMIC_FIELD_END + 0x0000,      // Flags: (All)
        GAME_OBJECT_DYNAMIC_FIELD_END                                     = ObjectDynamicField.OBJECT_DYNAMIC_FIELD_END + 0x0001,
    }

    public enum DynamicObjectField
    {
        DYNAMIC_OBJECT_FIELD_CASTER                                       = ObjectField.OBJECT_FIELD_END + 0x0000,      // Size: 4, Flags: (All)
        DYNAMIC_OBJECT_FIELD_TYPE                                         = ObjectField.OBJECT_FIELD_END + 0x0004,      // Size: 1, Flags: (All)
        DYNAMIC_OBJECT_FIELD_SPELL_X_SPELL_VISUAL_ID                      = ObjectField.OBJECT_FIELD_END + 0x0005,      // Size: 1, Flags: (All)
        DYNAMIC_OBJECT_FIELD_SPELL_ID                                     = ObjectField.OBJECT_FIELD_END + 0x0006,      // Size: 1, Flags: (All)
        DYNAMIC_OBJECT_FIELD_RADIUS                                       = ObjectField.OBJECT_FIELD_END + 0x0007,      // Size: 1, Flags: (All)
        DYNAMIC_OBJECT_FIELD_CAST_TIME                                    = ObjectField.OBJECT_FIELD_END + 0x0008,      // Size: 1, Flags: (All)
        DYNAMIC_OBJECT_FIELD_END                                          = ObjectField.OBJECT_FIELD_END + 0x0009,
    }

    public enum DynamicObjectDynamicField
    {
        DYNAMIC_OBJECT_DYNAMIC_FIELD_END                                  = ObjectDynamicField.OBJECT_DYNAMIC_FIELD_END + 0x0000,
    }

    public enum CorpseField
    {
        CORPSE_FIELD_OWNER                                                = ObjectField.OBJECT_FIELD_END + 0x0000,      // Size: 4, Flags: (All)
        CORPSE_FIELD_PARTY_GUID                                           = ObjectField.OBJECT_FIELD_END + 0x0004,      // Size: 4, Flags: (All)
        CORPSE_FIELD_GUILD_GUID                                           = ObjectField.OBJECT_FIELD_END + 0x0008,      // Size: 4, Flags: (All)
        CORPSE_FIELD_DISPLAY_ID                                           = ObjectField.OBJECT_FIELD_END + 0x000C,      // Size: 1, Flags: (All)
        CORPSE_FIELD_ITEMS                                                = ObjectField.OBJECT_FIELD_END + 0x000D,      // Size: 19, Flags: (All)
        CORPSE_FIELD_BYTES_1                                              = ObjectField.OBJECT_FIELD_END + 0x0020,      // Size: 1, Flags: (All) Nested: (RaceID, Sex, ClassID, Padding)
        CORPSE_FIELD_FLAGS                                                = ObjectField.OBJECT_FIELD_END + 0x0021,      // Size: 1, Flags: (All)
        CORPSE_FIELD_DYNAMIC_FLAGS                                        = ObjectField.OBJECT_FIELD_END + 0x0022,      // Size: 1, Flags: (ViewerDependent)
        CORPSE_FIELD_FACTION_TEMPLATE                                     = ObjectField.OBJECT_FIELD_END + 0x0023,      // Size: 1, Flags: (All)
        CORPSE_FIELD_CUSTOMIZATION_CHOICES                                = ObjectField.OBJECT_FIELD_END + 0x0024,      // Size: 72, Flags: (All)
        CORPSE_FIELD_END                                                  = ObjectField.OBJECT_FIELD_END + 0x006C,
    }

    public enum CorpseDynamicField
    {
        CORPSE_DYNAMIC_FIELD_END                                          = ObjectDynamicField.OBJECT_DYNAMIC_FIELD_END + 0x0000,
    }

    public enum AreaTriggerField
    {
        AREA_TRIGGER_FIELD_OVERRIDE_SCALE_CURVE                           = ObjectField.OBJECT_FIELD_END + 0x0000,      // Size: 7, Flags: (All, Urgent)
        AREA_TRIGGER_FIELD_EXTRA_SCALE_CURVE                              = ObjectField.OBJECT_FIELD_END + 0x0007,      // Size: 7, Flags: (All, Urgent)
        AREA_TRIGGER_FIELD_CASTER                                         = ObjectField.OBJECT_FIELD_END + 0x000E,      // Size: 4, Flags: (All)
        AREA_TRIGGER_FIELD_DURATION                                       = ObjectField.OBJECT_FIELD_END + 0x0012,      // Size: 1, Flags: (All)
        AREA_TRIGGER_FIELD_TIME_TO_TARGET                                 = ObjectField.OBJECT_FIELD_END + 0x0013,      // Size: 1, Flags: (All, Urgent)
        AREA_TRIGGER_FIELD_TIME_TO_TARGET_SCALE                           = ObjectField.OBJECT_FIELD_END + 0x0014,      // Size: 1, Flags: (All, Urgent)
        AREA_TRIGGER_FIELD_TIME_TO_TARGET_EXTRA_SCALE                     = ObjectField.OBJECT_FIELD_END + 0x0015,      // Size: 1, Flags: (All, Urgent)
        AREA_TRIGGER_FIELD_SPELL_ID                                       = ObjectField.OBJECT_FIELD_END + 0x0016,      // Size: 1, Flags: (All)
        AREA_TRIGGER_FIELD_SPELL_FOR_VISUALS                              = ObjectField.OBJECT_FIELD_END + 0x0017,      // Size: 1, Flags: (All)
        AREA_TRIGGER_FIELD_SPELL_X_SPELL_VISUAL_ID                        = ObjectField.OBJECT_FIELD_END + 0x0018,      // Size: 1, Flags: (All)
        AREA_TRIGGER_FIELD_BOUNDS_RADIUS_2D                               = ObjectField.OBJECT_FIELD_END + 0x0019,      // Size: 1, Flags: (ViewerDependent, Urgent)
        AREA_TRIGGER_FIELD_DECAL_PROPERTIES_ID                            = ObjectField.OBJECT_FIELD_END + 0x001A,      // Size: 1, Flags: (All)
        AREA_TRIGGER_FIELD_CREATING_EFFECT_GUID                           = ObjectField.OBJECT_FIELD_END + 0x001B,      // Size: 4, Flags: (All)
        AREA_TRIGGER_FIELD_END                                            = ObjectField.OBJECT_FIELD_END + 0x001F,
    }

    public enum AreaTriggerDynamicField
    {
        AREA_TRIGGER_DYNAMIC_FIELD_END                                    = ObjectDynamicField.OBJECT_DYNAMIC_FIELD_END + 0x0000,
    }

    public enum SceneObjectField
    {
        SCENE_OBJECT_FIELD_SCRIPT_PACKAGE_ID                              = ObjectField.OBJECT_FIELD_END + 0x0000,      // Size: 1, Flags: (All)
        SCENE_OBJECT_FIELD_RND_SEED_VAL                                   = ObjectField.OBJECT_FIELD_END + 0x0001,      // Size: 1, Flags: (All)
        SCENE_OBJECT_FIELD_CREATED_BY                                     = ObjectField.OBJECT_FIELD_END + 0x0002,      // Size: 4, Flags: (All)
        SCENE_OBJECT_FIELD_SCENE_TYPE                                     = ObjectField.OBJECT_FIELD_END + 0x0006,      // Size: 1, Flags: (All)
        SCENE_OBJECT_FIELD_END                                            = ObjectField.OBJECT_FIELD_END + 0x0007,
    }

    public enum SceneObjectDynamicField
    {
        SCENE_OBJECT_DYNAMIC_FIELD_END                                    = ObjectDynamicField.OBJECT_DYNAMIC_FIELD_END + 0x0000,
    }

    public enum ConversationField
    {
        CONVERSATION_FIELD_LAST_LINE_END_TIME                             = ObjectField.OBJECT_FIELD_END + 0x0000,      // Size: 1, Flags: (ViewerDependent)
        CONVERSATION_FIELD_END                                            = ObjectField.OBJECT_FIELD_END + 0x0001,
    }

    public enum ConversationDynamicField
    {
        CONVERSATION_DYNAMIC_FIELD_ACTORS                                 = ObjectDynamicField.OBJECT_DYNAMIC_FIELD_END + 0x0000,      // Flags: (All)
        CONVERSATION_DYNAMIC_FIELD_LINES                                  = ObjectDynamicField.OBJECT_DYNAMIC_FIELD_END + 0x0001,      // Flags: (Unk0x100)
        CONVERSATION_DYNAMIC_FIELD_END                                    = ObjectDynamicField.OBJECT_DYNAMIC_FIELD_END + 0x0002,
    }
}
