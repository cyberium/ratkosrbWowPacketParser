namespace WowPacketParser.Enums.Version.V1_12_1_5875
{
    // ReSharper disable InconsistentNaming
    // 1.12.1
    public enum ObjectField
    {
        OBJECT_FIELD_GUID = 0x0000, // Size: 2, Type: LONG, Flags: PUBLIC
        OBJECT_FIELD_TYPE = 0x0002, // Size: 1, Type: INT, Flags: PUBLIC
        OBJECT_FIELD_ENTRY = 0x0003, // Size: 1, Type: INT, Flags: PUBLIC
        OBJECT_FIELD_SCALE_X = 0x0004, // Size: 1, Type: FLOAT, Flags: PUBLIC
        OBJECT_FIELD_PADDING = 0x0005, // Size: 1, Type: INT, Flags: NONE
        OBJECT_END = 0x0006
    }

    public enum ItemField
    {
        ITEM_FIELD_OWNER                           = ObjectField.OBJECT_END + 0x00, // Size:2
        ITEM_FIELD_CONTAINED                       = ObjectField.OBJECT_END + 0x02, // Size:2
        ITEM_FIELD_CREATOR                         = ObjectField.OBJECT_END + 0x04, // Size:2
        ITEM_FIELD_GIFTCREATOR                     = ObjectField.OBJECT_END + 0x06, // Size:2
        ITEM_FIELD_STACK_COUNT                     = ObjectField.OBJECT_END + 0x08, // Size:1
        ITEM_FIELD_DURATION                        = ObjectField.OBJECT_END + 0x09, // Size:1
        ITEM_FIELD_SPELL_CHARGES                   = ObjectField.OBJECT_END + 0x0A, // Size:5
        ITEM_FIELD_SPELL_CHARGES_01                = ObjectField.OBJECT_END + 0x0B,
        ITEM_FIELD_SPELL_CHARGES_02                = ObjectField.OBJECT_END + 0x0C,
        ITEM_FIELD_SPELL_CHARGES_03                = ObjectField.OBJECT_END + 0x0D,
        ITEM_FIELD_SPELL_CHARGES_04                = ObjectField.OBJECT_END + 0x0E,
        ITEM_FIELD_FLAGS                           = ObjectField.OBJECT_END + 0x0F, // Size:1
        ITEM_FIELD_ENCHANTMENT                     = ObjectField.OBJECT_END + 0x10, // count=21
        ITEM_FIELD_PROPERTY_SEED                   = ObjectField.OBJECT_END + 0x25, // Size:1
        ITEM_FIELD_RANDOM_PROPERTIES_ID            = ObjectField.OBJECT_END + 0x26, // Size:1
        ITEM_FIELD_ITEM_TEXT_ID                    = ObjectField.OBJECT_END + 0x27, // Size:1
        ITEM_FIELD_DURABILITY                      = ObjectField.OBJECT_END + 0x28, // Size:1
        ITEM_FIELD_MAXDURABILITY                   = ObjectField.OBJECT_END + 0x29, // Size:1
        ITEM_END                                   = ObjectField.OBJECT_END + 0x2A,
    }

    public enum ContainerField
    {
        CONTAINER_FIELD_NUM_SLOTS = ItemField.ITEM_END + 0x0000, // Size: 1, Type: INT, Flags: PUBLIC
        CONTAINER_ALIGN_PAD = ItemField.ITEM_END + 0x0001, // Size: 1, Type: BYTES, Flags: NONE
        CONTAINER_FIELD_SLOT_1 = ItemField.ITEM_END + 0x0002, // Size: 72, Type: LONG, Flags: PUBLIC
        CONTAINER_END = ItemField.ITEM_END + 0x004A
    }

    public enum UnitField
    {
        UNIT_FIELD_CHARM                           = 0x00 + ObjectField.OBJECT_END, // Size:2
        UNIT_FIELD_SUMMON                          = 0x02 + ObjectField.OBJECT_END, // Size:2
        UNIT_FIELD_CHARMEDBY                       = 0x04 + ObjectField.OBJECT_END, // Size:2
        UNIT_FIELD_SUMMONEDBY                      = 0x06 + ObjectField.OBJECT_END, // Size:2
        UNIT_FIELD_CREATEDBY                       = 0x08 + ObjectField.OBJECT_END, // Size:2
        UNIT_FIELD_TARGET                          = 0x0A + ObjectField.OBJECT_END, // Size:2
        UNIT_FIELD_PERSUADED                       = 0x0C + ObjectField.OBJECT_END, // Size:2
        UNIT_FIELD_CHANNEL_OBJECT                  = 0x0E + ObjectField.OBJECT_END, // Size:2
        UNIT_FIELD_HEALTH                          = 0x10 + ObjectField.OBJECT_END, // Size:1
        UNIT_FIELD_POWER1                          = 0x11 + ObjectField.OBJECT_END, // Size:1
        UNIT_FIELD_POWER2                          = 0x12 + ObjectField.OBJECT_END, // Size:1
        UNIT_FIELD_POWER3                          = 0x13 + ObjectField.OBJECT_END, // Size:1
        UNIT_FIELD_POWER4                          = 0x14 + ObjectField.OBJECT_END, // Size:1
        UNIT_FIELD_POWER5                          = 0x15 + ObjectField.OBJECT_END, // Size:1
        UNIT_FIELD_MAXHEALTH                       = 0x16 + ObjectField.OBJECT_END, // Size:1
        UNIT_FIELD_MAXPOWER1                       = 0x17 + ObjectField.OBJECT_END, // Size:1
        UNIT_FIELD_MAXPOWER2                       = 0x18 + ObjectField.OBJECT_END, // Size:1
        UNIT_FIELD_MAXPOWER3                       = 0x19 + ObjectField.OBJECT_END, // Size:1
        UNIT_FIELD_MAXPOWER4                       = 0x1A + ObjectField.OBJECT_END, // Size:1
        UNIT_FIELD_MAXPOWER5                       = 0x1B + ObjectField.OBJECT_END, // Size:1
        UNIT_FIELD_LEVEL                           = 0x1C + ObjectField.OBJECT_END, // Size:1
        UNIT_FIELD_FACTIONTEMPLATE                 = 0x1D + ObjectField.OBJECT_END, // Size:1
        UNIT_FIELD_BYTES_0                         = 0x1E + ObjectField.OBJECT_END, // Size:1
        UNIT_VIRTUAL_ITEM_SLOT_DISPLAY             = 0x1F + ObjectField.OBJECT_END, // Size:3
        UNIT_VIRTUAL_ITEM_INFO                     = 0x22 + ObjectField.OBJECT_END, // Size:6
        UNIT_FIELD_FLAGS                           = 0x28 + ObjectField.OBJECT_END, // Size:1
        UNIT_FIELD_AURA                            = 0x29 + ObjectField.OBJECT_END, // Size:48
        UNIT_FIELD_AURA_LAST                       = 0x58 + ObjectField.OBJECT_END,
        UNIT_FIELD_AURAFLAGS                       = 0x59 + ObjectField.OBJECT_END, // Size:6
        UNIT_FIELD_AURAFLAGS_01                    = 0x5a + ObjectField.OBJECT_END,
        UNIT_FIELD_AURAFLAGS_02                    = 0x5b + ObjectField.OBJECT_END,
        UNIT_FIELD_AURAFLAGS_03                    = 0x5c + ObjectField.OBJECT_END,
        UNIT_FIELD_AURAFLAGS_04                    = 0x5d + ObjectField.OBJECT_END,
        UNIT_FIELD_AURAFLAGS_05                    = 0x5e + ObjectField.OBJECT_END,
        UNIT_FIELD_AURALEVELS                      = 0x5f + ObjectField.OBJECT_END, // Size:12
        UNIT_FIELD_AURALEVELS_LAST                 = 0x6a + ObjectField.OBJECT_END,
        UNIT_FIELD_AURAAPPLICATIONS                = 0x6b + ObjectField.OBJECT_END, // Size:12
        UNIT_FIELD_AURAAPPLICATIONS_LAST           = 0x76 + ObjectField.OBJECT_END,
        UNIT_FIELD_AURASTATE                       = 0x77 + ObjectField.OBJECT_END, // Size:1
        UNIT_FIELD_BASEATTACKTIME                  = 0x78 + ObjectField.OBJECT_END, // Size:2
        UNIT_FIELD_OFFHANDATTACKTIME               = 0x79 + ObjectField.OBJECT_END, // Size:2
        UNIT_FIELD_RANGEDATTACKTIME                = 0x7a + ObjectField.OBJECT_END, // Size:1
        UNIT_FIELD_BOUNDINGRADIUS                  = 0x7b + ObjectField.OBJECT_END, // Size:1
        UNIT_FIELD_COMBATREACH                     = 0x7c + ObjectField.OBJECT_END, // Size:1
        UNIT_FIELD_DISPLAYID                       = 0x7d + ObjectField.OBJECT_END, // Size:1
        UNIT_FIELD_NATIVEDISPLAYID                 = 0x7e + ObjectField.OBJECT_END, // Size:1
        UNIT_FIELD_MOUNTDISPLAYID                  = 0x7f + ObjectField.OBJECT_END, // Size:1
        UNIT_FIELD_MINDAMAGE                       = 0x80 + ObjectField.OBJECT_END, // Size:1
        UNIT_FIELD_MAXDAMAGE                       = 0x81 + ObjectField.OBJECT_END, // Size:1
        UNIT_FIELD_MINOFFHANDDAMAGE                = 0x82 + ObjectField.OBJECT_END, // Size:1
        UNIT_FIELD_MAXOFFHANDDAMAGE                = 0x83 + ObjectField.OBJECT_END, // Size:1
        UNIT_FIELD_BYTES_1                         = 0x84 + ObjectField.OBJECT_END, // Size:1
        UNIT_FIELD_PETNUMBER                       = 0x85 + ObjectField.OBJECT_END, // Size:1
        UNIT_FIELD_PET_NAME_TIMESTAMP              = 0x86 + ObjectField.OBJECT_END, // Size:1
        UNIT_FIELD_PETEXPERIENCE                   = 0x87 + ObjectField.OBJECT_END, // Size:1
        UNIT_FIELD_PETNEXTLEVELEXP                 = 0x88 + ObjectField.OBJECT_END, // Size:1
        UNIT_DYNAMIC_FLAGS                         = 0x89 + ObjectField.OBJECT_END, // Size:1
        UNIT_CHANNEL_SPELL                         = 0x8a + ObjectField.OBJECT_END, // Size:1
        UNIT_MOD_CAST_SPEED                        = 0x8b + ObjectField.OBJECT_END, // Size:1
        UNIT_CREATED_BY_SPELL                      = 0x8c + ObjectField.OBJECT_END, // Size:1
        UNIT_NPC_FLAGS                             = 0x8d + ObjectField.OBJECT_END, // Size:1
        UNIT_NPC_EMOTESTATE                        = 0x8e + ObjectField.OBJECT_END, // Size:1
        UNIT_TRAINING_POINTS                       = 0x8f + ObjectField.OBJECT_END, // Size:1
        UNIT_FIELD_STAT0                           = 0x90 + ObjectField.OBJECT_END, // Size:1
        UNIT_FIELD_STAT1                           = 0x91 + ObjectField.OBJECT_END, // Size:1
        UNIT_FIELD_STAT2                           = 0x92 + ObjectField.OBJECT_END, // Size:1
        UNIT_FIELD_STAT3                           = 0x93 + ObjectField.OBJECT_END, // Size:1
        UNIT_FIELD_STAT4                           = 0x94 + ObjectField.OBJECT_END, // Size:1
        UNIT_FIELD_RESISTANCES                     = 0x95 + ObjectField.OBJECT_END, // Size:7
        UNIT_FIELD_RESISTANCES_01                  = 0x96 + ObjectField.OBJECT_END,
        UNIT_FIELD_RESISTANCES_02                  = 0x97 + ObjectField.OBJECT_END,
        UNIT_FIELD_RESISTANCES_03                  = 0x98 + ObjectField.OBJECT_END,
        UNIT_FIELD_RESISTANCES_04                  = 0x99 + ObjectField.OBJECT_END,
        UNIT_FIELD_RESISTANCES_05                  = 0x9a + ObjectField.OBJECT_END,
        UNIT_FIELD_RESISTANCES_06                  = 0x9b + ObjectField.OBJECT_END,
        UNIT_FIELD_BASE_MANA                       = 0x9c + ObjectField.OBJECT_END, // Size:1
        UNIT_FIELD_BASE_HEALTH                     = 0x9d + ObjectField.OBJECT_END, // Size:1
        UNIT_FIELD_BYTES_2                         = 0x9e + ObjectField.OBJECT_END, // Size:1
        UNIT_FIELD_ATTACK_POWER                    = 0x9f + ObjectField.OBJECT_END, // Size:1
        UNIT_FIELD_ATTACK_POWER_MODS               = 0xa0 + ObjectField.OBJECT_END, // Size:1
        UNIT_FIELD_ATTACK_POWER_MULTIPLIER         = 0xa1 + ObjectField.OBJECT_END, // Size:1
        UNIT_FIELD_RANGED_ATTACK_POWER             = 0xa2 + ObjectField.OBJECT_END, // Size:1
        UNIT_FIELD_RANGED_ATTACK_POWER_MODS        = 0xa3 + ObjectField.OBJECT_END, // Size:1
        UNIT_FIELD_RANGED_ATTACK_POWER_MULTIPLIER  = 0xa4 + ObjectField.OBJECT_END, // Size:1
        UNIT_FIELD_MINRANGEDDAMAGE                 = 0xa5 + ObjectField.OBJECT_END, // Size:1
        UNIT_FIELD_MAXRANGEDDAMAGE                 = 0xa6 + ObjectField.OBJECT_END, // Size:1
        UNIT_FIELD_POWER_COST_MODIFIER             = 0xa7 + ObjectField.OBJECT_END, // Size:7
        UNIT_FIELD_POWER_COST_MODIFIER_01          = 0xa8 + ObjectField.OBJECT_END,
        UNIT_FIELD_POWER_COST_MODIFIER_02          = 0xa9 + ObjectField.OBJECT_END,
        UNIT_FIELD_POWER_COST_MODIFIER_03          = 0xaa + ObjectField.OBJECT_END,
        UNIT_FIELD_POWER_COST_MODIFIER_04          = 0xab + ObjectField.OBJECT_END,
        UNIT_FIELD_POWER_COST_MODIFIER_05          = 0xac + ObjectField.OBJECT_END,
        UNIT_FIELD_POWER_COST_MODIFIER_06          = 0xad + ObjectField.OBJECT_END,
        UNIT_FIELD_POWER_COST_MULTIPLIER           = 0xae + ObjectField.OBJECT_END, // Size:7
        UNIT_FIELD_POWER_COST_MULTIPLIER_01        = 0xaf + ObjectField.OBJECT_END,
        UNIT_FIELD_POWER_COST_MULTIPLIER_02        = 0xb0 + ObjectField.OBJECT_END,
        UNIT_FIELD_POWER_COST_MULTIPLIER_03        = 0xb1 + ObjectField.OBJECT_END,
        UNIT_FIELD_POWER_COST_MULTIPLIER_04        = 0xb2 + ObjectField.OBJECT_END,
        UNIT_FIELD_POWER_COST_MULTIPLIER_05        = 0xb3 + ObjectField.OBJECT_END,
        UNIT_FIELD_POWER_COST_MULTIPLIER_06        = 0xb4 + ObjectField.OBJECT_END,
        UNIT_FIELD_PADDING                         = 0xb5 + ObjectField.OBJECT_END,
        UNIT_END                                   = 0xb6 + ObjectField.OBJECT_END,
    }

    public enum PlayerField
    {
        PLAYER_DUEL_ARBITER                        = 0x00 + UnitField.UNIT_END, // Size:2
        PLAYER_FLAGS                               = 0x02 + UnitField.UNIT_END, // Size:1
        PLAYER_GUILDID                             = 0x03 + UnitField.UNIT_END, // Size:1
        PLAYER_GUILDRANK                           = 0x04 + UnitField.UNIT_END, // Size:1
        PLAYER_BYTES                               = 0x05 + UnitField.UNIT_END, // Size:1
        PLAYER_BYTES_2                             = 0x06 + UnitField.UNIT_END, // Size:1
        PLAYER_BYTES_3                             = 0x07 + UnitField.UNIT_END, // Size:1
        PLAYER_DUEL_TEAM                           = 0x08 + UnitField.UNIT_END, // Size:1
        PLAYER_GUILD_TIMESTAMP                     = 0x09 + UnitField.UNIT_END, // Size:1
        PLAYER_QUEST_LOG_1_1                       = 0x0A + UnitField.UNIT_END, // count = 20
        PLAYER_QUEST_LOG_1_2                       = 0x0B + UnitField.UNIT_END,
        PLAYER_QUEST_LOG_1_3                       = 0x0C + UnitField.UNIT_END,
        PLAYER_QUEST_LOG_LAST_1                    = 0x43 + UnitField.UNIT_END,
        PLAYER_QUEST_LOG_LAST_2                    = 0x44 + UnitField.UNIT_END,
        PLAYER_QUEST_LOG_LAST_3                    = 0x45 + UnitField.UNIT_END,
        PLAYER_VISIBLE_ITEM_1_CREATOR              = 0x46 + UnitField.UNIT_END, // Size:2, count = 19
        PLAYER_VISIBLE_ITEM_1_0                    = 0x48 + UnitField.UNIT_END, // Size:8
        PLAYER_VISIBLE_ITEM_1_PROPERTIES           = 0x50 + UnitField.UNIT_END, // Size:1
        PLAYER_VISIBLE_ITEM_1_PAD                  = 0x51 + UnitField.UNIT_END, // Size:1
        PLAYER_VISIBLE_ITEM_19_CREATOR             = 0x11e + UnitField.UNIT_END,
        PLAYER_VISIBLE_ITEM_19_0                   = 0x120 + UnitField.UNIT_END,
        PLAYER_VISIBLE_ITEM_19_PROPERTIES          = 0x128 + UnitField.UNIT_END,
        PLAYER_VISIBLE_ITEM_19_PAD                 = 0x129 + UnitField.UNIT_END,
        PLAYER_FIELD_INV_SLOT_HEAD                 = 0x12a + UnitField.UNIT_END, // Size:46
        PLAYER_FIELD_PACK_SLOT_1                   = 0x158 + UnitField.UNIT_END, // Size:32
        PLAYER_FIELD_PACK_SLOT_LAST                = 0x176 + UnitField.UNIT_END,
        PLAYER_FIELD_BANK_SLOT_1                   = 0x178 + UnitField.UNIT_END, // Size:48
        PLAYER_FIELD_BANK_SLOT_LAST                = 0x1a6 + UnitField.UNIT_END,
        PLAYER_FIELD_BANKBAG_SLOT_1                = 0x1a8 + UnitField.UNIT_END, // Size:12
        PLAYER_FIELD_BANKBAG_SLOT_LAST             = 0x1b2 + UnitField.UNIT_END,
        PLAYER_FIELD_VENDORBUYBACK_SLOT_1          = 0x1b4 + UnitField.UNIT_END, // Size:24
        PLAYER_FIELD_VENDORBUYBACK_SLOT_LAST       = 0x1ca + UnitField.UNIT_END,
        PLAYER_FIELD_KEYRING_SLOT_1                = 0x1cc + UnitField.UNIT_END, // Size:64
        PLAYER_FIELD_KEYRING_SLOT_LAST             = 0x20a + UnitField.UNIT_END,
        PLAYER_FARSIGHT                            = 0x20c + UnitField.UNIT_END, // Size:2
        PLAYER_FIELD_COMBO_TARGET                  = 0x20e + UnitField.UNIT_END, // Size:2
        PLAYER_XP                                  = 0x210 + UnitField.UNIT_END, // Size:1
        PLAYER_NEXT_LEVEL_XP                       = 0x211 + UnitField.UNIT_END, // Size:1
        PLAYER_SKILL_INFO_1_1                      = 0x212 + UnitField.UNIT_END, // Size:384
        PLAYER_CHARACTER_POINTS1                   = 0x392 + UnitField.UNIT_END, // Size:1
        PLAYER_CHARACTER_POINTS2                   = 0x393 + UnitField.UNIT_END, // Size:1
        PLAYER_TRACK_CREATURES                     = 0x394 + UnitField.UNIT_END, // Size:1
        PLAYER_TRACK_RESOURCES                     = 0x395 + UnitField.UNIT_END, // Size:1
        PLAYER_BLOCK_PERCENTAGE                    = 0x396 + UnitField.UNIT_END, // Size:1
        PLAYER_DODGE_PERCENTAGE                    = 0x397 + UnitField.UNIT_END, // Size:1
        PLAYER_PARRY_PERCENTAGE                    = 0x398 + UnitField.UNIT_END, // Size:1
        PLAYER_CRIT_PERCENTAGE                     = 0x399 + UnitField.UNIT_END, // Size:1
        PLAYER_RANGED_CRIT_PERCENTAGE              = 0x39a + UnitField.UNIT_END, // Size:1
        PLAYER_EXPLORED_ZONES_1                    = 0x39b + UnitField.UNIT_END, // Size:64
        PLAYER_REST_STATE_EXPERIENCE               = 0x3db + UnitField.UNIT_END, // Size:1
        PLAYER_FIELD_COINAGE                       = 0x3dc + UnitField.UNIT_END, // Size:1
        PLAYER_FIELD_POSSTAT0                      = 0x3DD + UnitField.UNIT_END, // Size:1
        PLAYER_FIELD_POSSTAT1                      = 0x3DE + UnitField.UNIT_END, // Size:1
        PLAYER_FIELD_POSSTAT2                      = 0x3DF + UnitField.UNIT_END, // Size:1
        PLAYER_FIELD_POSSTAT3                      = 0x3E0 + UnitField.UNIT_END, // Size:1
        PLAYER_FIELD_POSSTAT4                      = 0x3E1 + UnitField.UNIT_END, // Size:1
        PLAYER_FIELD_NEGSTAT0                      = 0x3E2 + UnitField.UNIT_END, // Size:1
        PLAYER_FIELD_NEGSTAT1                      = 0x3E3 + UnitField.UNIT_END, // Size:1
        PLAYER_FIELD_NEGSTAT2                      = 0x3E4 + UnitField.UNIT_END, // Size:1
        PLAYER_FIELD_NEGSTAT3                      = 0x3E5 + UnitField.UNIT_END, // Size:1,
        PLAYER_FIELD_NEGSTAT4                      = 0x3E6 + UnitField.UNIT_END, // Size:1
        PLAYER_FIELD_RESISTANCEBUFFMODSPOSITIVE    = 0x3E7 + UnitField.UNIT_END, // Size:7
        PLAYER_FIELD_RESISTANCEBUFFMODSNEGATIVE    = 0x3EE + UnitField.UNIT_END, // Size:7
        PLAYER_FIELD_MOD_DAMAGE_DONE_POS           = 0x3F5 + UnitField.UNIT_END, // Size:7
        PLAYER_FIELD_MOD_DAMAGE_DONE_NEG           = 0x3FC + UnitField.UNIT_END, // Size:7
        PLAYER_FIELD_MOD_DAMAGE_DONE_PCT           = 0x403 + UnitField.UNIT_END, // Size:7
        PLAYER_FIELD_BYTES                         = 0x40A + UnitField.UNIT_END, // Size:1
        PLAYER_AMMO_ID                             = 0x40B + UnitField.UNIT_END, // Size:1
        PLAYER_SELF_RES_SPELL                      = 0x40C + UnitField.UNIT_END, // Size:1
        PLAYER_FIELD_PVP_MEDALS                    = 0x40D + UnitField.UNIT_END, // Size:1
        PLAYER_FIELD_BUYBACK_PRICE_1               = 0x40E + UnitField.UNIT_END, // count=12
        PLAYER_FIELD_BUYBACK_PRICE_LAST            = 0x419 + UnitField.UNIT_END,
        PLAYER_FIELD_BUYBACK_TIMESTAMP_1           = 0x41A + UnitField.UNIT_END, // count=12
        PLAYER_FIELD_BUYBACK_TIMESTAMP_LAST        = 0x425 + UnitField.UNIT_END,
        PLAYER_FIELD_SESSION_KILLS                 = 0x426 + UnitField.UNIT_END, // Size:1
        PLAYER_FIELD_YESTERDAY_KILLS               = 0x427 + UnitField.UNIT_END, // Size:1
        PLAYER_FIELD_LAST_WEEK_KILLS               = 0x428 + UnitField.UNIT_END, // Size:1
        PLAYER_FIELD_THIS_WEEK_KILLS               = 0x429 + UnitField.UNIT_END, // Size:1
        PLAYER_FIELD_THIS_WEEK_CONTRIBUTION        = 0x42a + UnitField.UNIT_END, // Size:1
        PLAYER_FIELD_LIFETIME_HONORABLE_KILLS      = 0x42b + UnitField.UNIT_END, // Size:1
        PLAYER_FIELD_LIFETIME_DISHONORABLE_KILLS   = 0x42c + UnitField.UNIT_END, // Size:1
        PLAYER_FIELD_YESTERDAY_CONTRIBUTION        = 0x42d + UnitField.UNIT_END, // Size:1
        PLAYER_FIELD_LAST_WEEK_CONTRIBUTION        = 0x42e + UnitField.UNIT_END, // Size:1
        PLAYER_FIELD_LAST_WEEK_RANK                = 0x42f + UnitField.UNIT_END, // Size:1
        PLAYER_FIELD_BYTES2                        = 0x430 + UnitField.UNIT_END, // Size:1
        PLAYER_FIELD_WATCHED_FACTION_INDEX         = 0x431 + UnitField.UNIT_END, // Size:1
        PLAYER_FIELD_COMBAT_RATING_1               = 0x432 + UnitField.UNIT_END, // Size:20

        PLAYER_END                                 = 0x446 + UnitField.UNIT_END
    }

    public enum GameObjectField
    {
        GAMEOBJECT_FIELD_CREATED_BY = ObjectField.OBJECT_END + 0x0000, // Size: 2, Type: LONG, Flags: PUBLIC
        GAMEOBJECT_DISPLAYID = ObjectField.OBJECT_END + 0x0002, // Size: 1, Type: INT, Flags: PUBLIC
        GAMEOBJECT_FLAGS = ObjectField.OBJECT_END + 0x0003, // Size: 1, Type: INT, Flags: PUBLIC
        GAMEOBJECT_ROTATION = ObjectField.OBJECT_END + 0x0004, // Size: 4, Type: FLOAT, Flags: PUBLIC
        GAMEOBJECT_STATE = ObjectField.OBJECT_END + 0x0008, // Size: 1, Type: INT, Flags: PUBLIC
        GAMEOBJECT_POS_X = ObjectField.OBJECT_END + 0x0009, // Size: 1, Type: FLOAT, Flags: PUBLIC
        GAMEOBJECT_POS_Y = ObjectField.OBJECT_END + 0x000A, // Size: 1, Type: FLOAT, Flags: PUBLIC
        GAMEOBJECT_POS_Z = ObjectField.OBJECT_END + 0x000B, // Size: 1, Type: FLOAT, Flags: PUBLIC
        GAMEOBJECT_FACING = ObjectField.OBJECT_END + 0x000C, // Size: 1, Type: FLOAT, Flags: PUBLIC
        GAMEOBJECT_DYNAMIC = ObjectField.OBJECT_END + 0x000D, // Size: 1, Type: INT, Flags: DYNAMIC
        GAMEOBJECT_FACTION = ObjectField.OBJECT_END + 0x000E, // Size: 1, Type: INT, Flags: PUBLIC
        GAMEOBJECT_TYPE_ID = ObjectField.OBJECT_END + 0x000F, // Size: 1, Type: INT, Flags: PUBLIC
        GAMEOBJECT_LEVEL = ObjectField.OBJECT_END + 0x0010, // Size: 1, Type: INT, Flags: PUBLIC
        GAMEOBJECT_ARTKIT = ObjectField.OBJECT_END + 0x0011, // Size: 1, Type: INT, Flags: PUBLIC
        GAMEOBJECT_ANIMPROGRESS = ObjectField.OBJECT_END + 0x0012, // Size: 1, Type: INT, Flags: PUBLIC
        GAMEOBJECT_PADDING = ObjectField.OBJECT_END + 0x0013, // Size: 1, Type: INT, Flags: PUBLIC
        GAMEOBJECT_END = ObjectField.OBJECT_END + 0x0014
    }

    public enum DynamicObjectField
    {
        DYNAMICOBJECT_CASTER = ObjectField.OBJECT_END + 0x0000, // Size: 2, Type: LONG, Flags: PUBLIC
        DYNAMICOBJECT_BYTES = ObjectField.OBJECT_END + 0x0002, // Size: 1, Type: BYTES, Flags: PUBLIC
        DYNAMICOBJECT_SPELLID = ObjectField.OBJECT_END + 0x0003, // Size: 1, Type: INT, Flags: PUBLIC
        DYNAMICOBJECT_RADIUS = ObjectField.OBJECT_END + 0x0004, // Size: 1, Type: FLOAT, Flags: PUBLIC
        DYNAMICOBJECT_POS_X = ObjectField.OBJECT_END + 0x0005, // Size: 1, Type: FLOAT, Flags: PUBLIC
        DYNAMICOBJECT_POS_Y = ObjectField.OBJECT_END + 0x0006, // Size: 1, Type: FLOAT, Flags: PUBLIC
        DYNAMICOBJECT_POS_Z = ObjectField.OBJECT_END + 0x0007, // Size: 1, Type: FLOAT, Flags: PUBLIC
        DYNAMICOBJECT_FACING = ObjectField.OBJECT_END + 0x0008, // Size: 1, Type: FLOAT, Flags: PUBLIC
        DYNAMICOBJECT_CASTTIME = ObjectField.OBJECT_END + 0x0009, // Size: 1, Type: INT, Flags: PUBLIC
        DYNAMICOBJECT_END = ObjectField.OBJECT_END + 0x000A
    }

    public enum CorpseField
    {
        CORPSE_FIELD_OWNER = ObjectField.OBJECT_END + 0x0000, // Size: 2, Type: LONG, Flags: PUBLIC
        CORPSE_FIELD_FACING = ObjectField.OBJECT_END + 0x0002, // Size: 1, Type: FLOAT, Flags: PUBLIC
        CORPSE_FIELD_POS_X = ObjectField.OBJECT_END + 0x0003, // Size: 1, Type: FLOAT, Flags: PUBLIC
        CORPSE_FIELD_POS_Y = ObjectField.OBJECT_END + 0x0004, // Size: 1, Type: FLOAT, Flags: PUBLIC
        CORPSE_FIELD_POS_Z = ObjectField.OBJECT_END + 0x0005, // Size: 1, Type: FLOAT, Flags: PUBLIC
        CORPSE_FIELD_DISPLAY_ID = ObjectField.OBJECT_END + 0x0006, // Size: 1, Type: INT, Flags: PUBLIC
        CORPSE_FIELD_ITEM = ObjectField.OBJECT_END + 0x0007, // Size: 19, Type: INT, Flags: PUBLIC
        CORPSE_FIELD_BYTES_1 = ObjectField.OBJECT_END + 0x001A, // Size: 1, Type: BYTES, Flags: PUBLIC
        CORPSE_FIELD_BYTES_2 = ObjectField.OBJECT_END + 0x001B, // Size: 1, Type: BYTES, Flags: PUBLIC
        CORPSE_FIELD_GUILD = ObjectField.OBJECT_END + 0x001C, // Size: 1, Type: INT, Flags: PUBLIC
        CORPSE_FIELD_FLAGS = ObjectField.OBJECT_END + 0x001D, // Size: 1, Type: INT, Flags: PUBLIC
        CORPSE_FIELD_DYNAMIC_FLAGS = ObjectField.OBJECT_END + 0x001E, // Size: 1, Type: INT, Flags: DYNAMIC
        CORPSE_FIELD_PAD = ObjectField.OBJECT_END + 0x001F, // Size: 1, Type: INT, Flags: NONE
        CORPSE_END = ObjectField.OBJECT_END + 0x0020
    }
}
