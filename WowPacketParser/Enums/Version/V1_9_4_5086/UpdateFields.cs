namespace WowPacketParser.Enums.Version.V1_9_4_5086
{
    // ReSharper disable InconsistentNaming
    // 1.9.4
    public enum ObjectField
    {
        OBJECT_FIELD_GUID = 0x0,                                                    // 0x000 - Size: 2 - Type: GUID - Flags: PUBLIC
        OBJECT_FIELD_TYPE = 0x2,                                                    // 0x002 - Size: 1 - Type: INT - Flags: PUBLIC
        OBJECT_FIELD_ENTRY = 0x3,                                                   // 0x003 - Size: 1 - Type: INT - Flags: PUBLIC
        OBJECT_FIELD_SCALE_X = 0x4,                                                 // 0x004 - Size: 1 - Type: FLOAT - Flags: PUBLIC
        OBJECT_FIELD_PADDING = 0x5,                                                 // 0x005 - Size: 1 - Type: INT - Flags: NONE
        OBJECT_END = 0x6
    }

    public enum ItemField
    {
        ITEM_FIELD_OWNER = ObjectField.OBJECT_END + 0x0,                            // 0x006 - Size: 2 - Type: GUID - Flags: PUBLIC
        ITEM_FIELD_CONTAINED = ObjectField.OBJECT_END + 0x2,                        // 0x008 - Size: 2 - Type: GUID - Flags: PUBLIC
        ITEM_FIELD_CREATOR = ObjectField.OBJECT_END + 0x4,                          // 0x00A - Size: 2 - Type: GUID - Flags: PUBLIC
        ITEM_FIELD_GIFTCREATOR = ObjectField.OBJECT_END + 0x6,                      // 0x00C - Size: 2 - Type: GUID - Flags: PUBLIC
        ITEM_FIELD_STACK_COUNT = ObjectField.OBJECT_END + 0x8,                      // 0x00E - Size: 1 - Type: INT - Flags: OWNER_ONLY + UNK2
        ITEM_FIELD_DURATION = ObjectField.OBJECT_END + 0x9,                         // 0x00F - Size: 1 - Type: INT - Flags: OWNER_ONLY + UNK2
        ITEM_FIELD_SPELL_CHARGES = ObjectField.OBJECT_END + 0xA,                    // 0x010 - Size: 5 - Type: INT - Flags: OWNER_ONLY + UNK2
        ITEM_FIELD_FLAGS = ObjectField.OBJECT_END + 0xF,                            // 0x015 - Size: 1 - Type: INT - Flags: PUBLIC
        ITEM_FIELD_ENCHANTMENT = ObjectField.OBJECT_END + 0x10,                     // 0x016 - Size: 21 - Type: INT - Flags: PUBLIC
        ITEM_FIELD_PROPERTY_SEED = ObjectField.OBJECT_END + 0x25,                   // 0x02B - Size: 1 - Type: INT - Flags: PUBLIC
        ITEM_FIELD_RANDOM_PROPERTIES_ID = ObjectField.OBJECT_END + 0x26,            // 0x02C - Size: 1 - Type: INT - Flags: PUBLIC
        ITEM_FIELD_ITEM_TEXT_ID = ObjectField.OBJECT_END + 0x27,                    // 0x02D - Size: 1 - Type: INT - Flags: OWNER_ONLY
        ITEM_FIELD_DURABILITY = ObjectField.OBJECT_END + 0x28,                      // 0x02E - Size: 1 - Type: INT - Flags: OWNER_ONLY + UNK2
        ITEM_FIELD_MAXDURABILITY = ObjectField.OBJECT_END + 0x29,                   // 0x02F - Size: 1 - Type: INT - Flags: OWNER_ONLY + UNK2
        ITEM_END = ObjectField.OBJECT_END + 0x2A                                    // 0x030
    }

    public enum ContainerField
    {
        CONTAINER_FIELD_NUM_SLOTS = ItemField.ITEM_END + 0x0,                       // 0x02A - Size: 1 - Type: INT - Flags: PUBLIC
        CONTAINER_ALIGN_PAD = ItemField.ITEM_END + 0x1,                             // 0x02B - Size: 1 - Type: BYTES - Flags: NONE
        CONTAINER_FIELD_SLOT_1 = ItemField.ITEM_END + 0x2,                          // 0x02C - Size: 72 - Type: GUID - Flags: PUBLIC
        CONTAINER_END = ItemField.ITEM_END + 0x4A                                   // 0x074
    }

    public enum UnitField
    {
        UNIT_FIELD_CHARM = ObjectField.OBJECT_END + 0x0,                            // 0x006 - Size: 2 - Type: GUID - Flags: PUBLIC
        UNIT_FIELD_SUMMON = ObjectField.OBJECT_END + 0x2,                           // 0x008 - Size: 2 - Type: GUID - Flags: PUBLIC
        UNIT_FIELD_CHARMEDBY = ObjectField.OBJECT_END + 0x4,                        // 0x00A - Size: 2 - Type: GUID - Flags: PUBLIC
        UNIT_FIELD_SUMMONEDBY = ObjectField.OBJECT_END + 0x6,                       // 0x00C - Size: 2 - Type: GUID - Flags: PUBLIC
        UNIT_FIELD_CREATEDBY = ObjectField.OBJECT_END + 0x8,                        // 0x00E - Size: 2 - Type: GUID - Flags: PUBLIC
        UNIT_FIELD_TARGET = ObjectField.OBJECT_END + 0xA,                           // 0x010 - Size: 2 - Type: GUID - Flags: PUBLIC
        UNIT_FIELD_PERSUADED = ObjectField.OBJECT_END + 0xC,                        // 0x012 - Size: 2 - Type: GUID - Flags: PUBLIC
        UNIT_FIELD_CHANNEL_OBJECT = ObjectField.OBJECT_END + 0xE,                   // 0x014 - Size: 2 - Type: GUID - Flags: PUBLIC
        UNIT_FIELD_HEALTH = ObjectField.OBJECT_END + 0x10,                          // 0x016 - Size: 1 - Type: INT - Flags: DYNAMIC
        UNIT_FIELD_POWER1 = ObjectField.OBJECT_END + 0x11,                          // 0x017 - Size: 1 - Type: INT - Flags: PUBLIC
        UNIT_FIELD_POWER2 = ObjectField.OBJECT_END + 0x12,                          // 0x018 - Size: 1 - Type: INT - Flags: PUBLIC
        UNIT_FIELD_POWER3 = ObjectField.OBJECT_END + 0x13,                          // 0x019 - Size: 1 - Type: INT - Flags: PUBLIC
        UNIT_FIELD_POWER4 = ObjectField.OBJECT_END + 0x14,                          // 0x01A - Size: 1 - Type: INT - Flags: PUBLIC
        UNIT_FIELD_POWER5 = ObjectField.OBJECT_END + 0x15,                          // 0x01B - Size: 1 - Type: INT - Flags: PUBLIC
        UNIT_FIELD_MAXHEALTH = ObjectField.OBJECT_END + 0x16,                       // 0x01C - Size: 1 - Type: INT - Flags: DYNAMIC
        UNIT_FIELD_MAXPOWER1 = ObjectField.OBJECT_END + 0x17,                       // 0x01D - Size: 1 - Type: INT - Flags: PUBLIC
        UNIT_FIELD_MAXPOWER2 = ObjectField.OBJECT_END + 0x18,                       // 0x01E - Size: 1 - Type: INT - Flags: PUBLIC
        UNIT_FIELD_MAXPOWER3 = ObjectField.OBJECT_END + 0x19,                       // 0x01F - Size: 1 - Type: INT - Flags: PUBLIC
        UNIT_FIELD_MAXPOWER4 = ObjectField.OBJECT_END + 0x1A,                       // 0x020 - Size: 1 - Type: INT - Flags: PUBLIC
        UNIT_FIELD_MAXPOWER5 = ObjectField.OBJECT_END + 0x1B,                       // 0x021 - Size: 1 - Type: INT - Flags: PUBLIC
        UNIT_FIELD_LEVEL = ObjectField.OBJECT_END + 0x1C,                           // 0x022 - Size: 1 - Type: INT - Flags: PUBLIC
        UNIT_FIELD_FACTIONTEMPLATE = ObjectField.OBJECT_END + 0x1D,                 // 0x023 - Size: 1 - Type: INT - Flags: PUBLIC
        UNIT_FIELD_BYTES_0 = ObjectField.OBJECT_END + 0x1E,                         // 0x024 - Size: 1 - Type: BYTES - Flags: PUBLIC
        UNIT_VIRTUAL_ITEM_SLOT_DISPLAY = ObjectField.OBJECT_END + 0x1F,             // 0x025 - Size: 3 - Type: INT - Flags: PUBLIC
        UNIT_VIRTUAL_ITEM_INFO = ObjectField.OBJECT_END + 0x22,                     // 0x028 - Size: 6 - Type: BYTES - Flags: PUBLIC
        UNIT_FIELD_FLAGS = ObjectField.OBJECT_END + 0x28,                           // 0x02E - Size: 1 - Type: INT - Flags: PUBLIC
        UNIT_FIELD_AURA = ObjectField.OBJECT_END + 0x29,                            // 0x02F - Size: 64 - Type: INT - Flags: PUBLIC
        UNIT_FIELD_AURAFLAGS = ObjectField.OBJECT_END + 0x69,                       // 0x06F - Size: 8 - Type: BYTES - Flags: PUBLIC
        UNIT_FIELD_AURALEVELS = ObjectField.OBJECT_END + 0x71,                      // 0x077 - Size: 12 - Type: BYTES - Flags: PUBLIC
        UNIT_FIELD_AURAAPPLICATIONS = ObjectField.OBJECT_END + 0x7D,                // 0x083 - Size: 12 - Type: BYTES - Flags: PUBLIC
        UNIT_FIELD_AURASTATE = ObjectField.OBJECT_END + 0x89,                       // 0x08F - Size: 1 - Type: INT - Flags: PUBLIC
        UNIT_FIELD_BASEATTACKTIME = ObjectField.OBJECT_END + 0x8A,                  // 0x090 - Size: 2 - Type: INT - Flags: PUBLIC
        UNIT_FIELD_RANGEDATTACKTIME = ObjectField.OBJECT_END + 0x8C,                // 0x092 - Size: 1 - Type: INT - Flags: PRIVATE
        UNIT_FIELD_BOUNDINGRADIUS = ObjectField.OBJECT_END + 0x8D,                  // 0x093 - Size: 1 - Type: FLOAT - Flags: PUBLIC
        UNIT_FIELD_COMBATREACH = ObjectField.OBJECT_END + 0x8E,                     // 0x094 - Size: 1 - Type: FLOAT - Flags: PUBLIC
        UNIT_FIELD_DISPLAYID = ObjectField.OBJECT_END + 0x8F,                       // 0x095 - Size: 1 - Type: INT - Flags: PUBLIC
        UNIT_FIELD_NATIVEDISPLAYID = ObjectField.OBJECT_END + 0x90,                 // 0x096 - Size: 1 - Type: INT - Flags: PUBLIC
        UNIT_FIELD_MOUNTDISPLAYID = ObjectField.OBJECT_END + 0x91,                  // 0x097 - Size: 1 - Type: INT - Flags: PUBLIC
        UNIT_FIELD_MINDAMAGE = ObjectField.OBJECT_END + 0x92,                       // 0x098 - Size: 1 - Type: FLOAT - Flags: PRIVATE + OWNER_ONLY + UNK3
        UNIT_FIELD_MAXDAMAGE = ObjectField.OBJECT_END + 0x93,                       // 0x099 - Size: 1 - Type: FLOAT - Flags: PRIVATE + OWNER_ONLY + UNK3
        UNIT_FIELD_MINOFFHANDDAMAGE = ObjectField.OBJECT_END + 0x94,                // 0x09A - Size: 1 - Type: FLOAT - Flags: PRIVATE + OWNER_ONLY + UNK3
        UNIT_FIELD_MAXOFFHANDDAMAGE = ObjectField.OBJECT_END + 0x95,                // 0x09B - Size: 1 - Type: FLOAT - Flags: PRIVATE + OWNER_ONLY + UNK3
        UNIT_FIELD_BYTES_1 = ObjectField.OBJECT_END + 0x96,                         // 0x09C - Size: 1 - Type: BYTES - Flags: PUBLIC
        UNIT_FIELD_PETNUMBER = ObjectField.OBJECT_END + 0x97,                       // 0x09D - Size: 1 - Type: INT - Flags: PUBLIC
        UNIT_FIELD_PET_NAME_TIMESTAMP = ObjectField.OBJECT_END + 0x98,              // 0x09E - Size: 1 - Type: INT - Flags: PUBLIC
        UNIT_FIELD_PETEXPERIENCE = ObjectField.OBJECT_END + 0x99,                   // 0x09F - Size: 1 - Type: INT - Flags: OWNER_ONLY
        UNIT_FIELD_PETNEXTLEVELEXP = ObjectField.OBJECT_END + 0x9A,                 // 0x0A0 - Size: 1 - Type: INT - Flags: OWNER_ONLY
        UNIT_DYNAMIC_FLAGS = ObjectField.OBJECT_END + 0x9B,                         // 0x0A1 - Size: 1 - Type: INT - Flags: DYNAMIC
        UNIT_CHANNEL_SPELL = ObjectField.OBJECT_END + 0x9C,                         // 0x0A2 - Size: 1 - Type: INT - Flags: PUBLIC
        UNIT_MOD_CAST_SPEED = ObjectField.OBJECT_END + 0x9D,                        // 0x0A3 - Size: 1 - Type: INT - Flags: PUBLIC
        UNIT_CREATED_BY_SPELL = ObjectField.OBJECT_END + 0x9E,                      // 0x0A4 - Size: 1 - Type: INT - Flags: PUBLIC
        UNIT_NPC_FLAGS = ObjectField.OBJECT_END + 0x9F,                             // 0x0A5 - Size: 1 - Type: INT - Flags: PUBLIC
        UNIT_NPC_EMOTESTATE = ObjectField.OBJECT_END + 0xA0,                        // 0x0A6 - Size: 1 - Type: INT - Flags: PUBLIC
        UNIT_TRAINING_POINTS = ObjectField.OBJECT_END + 0xA1,                       // 0x0A7 - Size: 1 - Type: TWO_SHORT - Flags: OWNER_ONLY
        UNIT_FIELD_STAT0 = ObjectField.OBJECT_END + 0xA2,                           // 0x0A8 - Size: 1 - Type: INT - Flags: PRIVATE + OWNER_ONLY
        UNIT_FIELD_STAT1 = ObjectField.OBJECT_END + 0xA3,                           // 0x0A9 - Size: 1 - Type: INT - Flags: PRIVATE + OWNER_ONLY
        UNIT_FIELD_STAT2 = ObjectField.OBJECT_END + 0xA4,                           // 0x0AA - Size: 1 - Type: INT - Flags: PRIVATE + OWNER_ONLY
        UNIT_FIELD_STAT3 = ObjectField.OBJECT_END + 0xA5,                           // 0x0AB - Size: 1 - Type: INT - Flags: PRIVATE + OWNER_ONLY
        UNIT_FIELD_STAT4 = ObjectField.OBJECT_END + 0xA6,                           // 0x0AC - Size: 1 - Type: INT - Flags: PRIVATE + OWNER_ONLY
        UNIT_FIELD_RESISTANCES = ObjectField.OBJECT_END + 0xA7,                     // 0x0AD - Size: 7 - Type: INT - Flags: PRIVATE + OWNER_ONLY + UNK3
        UNIT_FIELD_BASE_MANA = ObjectField.OBJECT_END + 0xAE,                       // 0x0B4 - Size: 1 - Type: INT - Flags: PRIVATE + OWNER_ONLY
        UNIT_FIELD_BASE_HEALTH = ObjectField.OBJECT_END + 0xAF,                     // 0x0B5 - Size: 1 - Type: INT - Flags: PRIVATE + OWNER_ONLY
        UNIT_FIELD_BYTES_2 = ObjectField.OBJECT_END + 0xB0,                         // 0x0B6 - Size: 1 - Type: BYTES - Flags: PUBLIC
        UNIT_FIELD_ATTACK_POWER = ObjectField.OBJECT_END + 0xB1,                    // 0x0B7 - Size: 1 - Type: INT - Flags: PRIVATE + OWNER_ONLY
        UNIT_FIELD_ATTACK_POWER_MODS = ObjectField.OBJECT_END + 0xB2,               // 0x0B8 - Size: 1 - Type: TWO_SHORT - Flags: PRIVATE + OWNER_ONLY
        UNIT_FIELD_ATTACK_POWER_MULTIPLIER = ObjectField.OBJECT_END + 0xB3,         // 0x0B9 - Size: 1 - Type: FLOAT - Flags: PRIVATE + OWNER_ONLY
        UNIT_FIELD_RANGED_ATTACK_POWER = ObjectField.OBJECT_END + 0xB4,             // 0x0BA - Size: 1 - Type: INT - Flags: PRIVATE + OWNER_ONLY
        UNIT_FIELD_RANGED_ATTACK_POWER_MODS = ObjectField.OBJECT_END + 0xB5,        // 0x0BB - Size: 1 - Type: TWO_SHORT - Flags: PRIVATE + OWNER_ONLY
        UNIT_FIELD_RANGED_ATTACK_POWER_MULTIPLIER = ObjectField.OBJECT_END + 0xB6,  // 0x0BC - Size: 1 - Type: FLOAT - Flags: PRIVATE + OWNER_ONLY
        UNIT_FIELD_MINRANGEDDAMAGE = ObjectField.OBJECT_END + 0xB7,                 // 0x0BD - Size: 1 - Type: FLOAT - Flags: PRIVATE + OWNER_ONLY
        UNIT_FIELD_MAXRANGEDDAMAGE = ObjectField.OBJECT_END + 0xB8,                 // 0x0BE - Size: 1 - Type: FLOAT - Flags: PRIVATE + OWNER_ONLY
        UNIT_FIELD_POWER_COST_MODIFIER = ObjectField.OBJECT_END + 0xB9,             // 0x0BF - Size: 7 - Type: INT - Flags: PRIVATE + OWNER_ONLY
        UNIT_FIELD_POWER_COST_MULTIPLIER = ObjectField.OBJECT_END + 0xC0,           // 0x0C6 - Size: 7 - Type: FLOAT - Flags: PRIVATE + OWNER_ONLY
        UNIT_FIELD_PADDING = ObjectField.OBJECT_END + 0xC7,                         // 0x0CD - Size: 1 - Type: INT - Flags: NONE
        UNIT_END = ObjectField.OBJECT_END + 0xC8                                    // 0x0CE
    }

    public enum PlayerField
    {
        PLAYER_SELECTION = UnitField.UNIT_END + 0x0,                                // 0x0C8 - Size: 2 - Type: GUID - Flags: PUBLIC
        PLAYER_DUEL_ARBITER = UnitField.UNIT_END + 0x2,                             // 0x0CA - Size: 2 - Type: GUID - Flags: PUBLIC
        PLAYER_FLAGS = UnitField.UNIT_END + 0x4,                                    // 0x0CC - Size: 1 - Type: INT - Flags: PUBLIC
        PLAYER_GUILDID = UnitField.UNIT_END + 0x5,                                  // 0x0CD - Size: 1 - Type: INT - Flags: PUBLIC
        PLAYER_GUILDRANK = UnitField.UNIT_END + 0x6,                                // 0x0CE - Size: 1 - Type: INT - Flags: PUBLIC
        PLAYER_BYTES = UnitField.UNIT_END + 0x7,                                    // 0x0CF - Size: 1 - Type: BYTES - Flags: PUBLIC
        PLAYER_BYTES_2 = UnitField.UNIT_END + 0x8,                                  // 0x0D0 - Size: 1 - Type: BYTES - Flags: PUBLIC
        PLAYER_BYTES_3 = UnitField.UNIT_END + 0x9,                                  // 0x0D1 - Size: 1 - Type: BYTES - Flags: PUBLIC
        PLAYER_DUEL_TEAM = UnitField.UNIT_END + 0xA,                                // 0x0D2 - Size: 1 - Type: INT - Flags: PUBLIC
        PLAYER_GUILD_TIMESTAMP = UnitField.UNIT_END + 0xB,                          // 0x0D3 - Size: 1 - Type: INT - Flags: PUBLIC
        PLAYER_QUEST_LOG_1_1 = UnitField.UNIT_END + 0xC,                            // 0x0D4 - Size: 1 - Type: INT - Flags: GROUP_ONLY
        PLAYER_QUEST_LOG_1_2 = UnitField.UNIT_END + 0xD,                            // 0x0D5 - Size: 2 - Type: INT - Flags: PRIVATE
        PLAYER_QUEST_LOG_2_1 = UnitField.UNIT_END + 0xF,                            // 0x0D7 - Size: 1 - Type: INT - Flags: GROUP_ONLY
        PLAYER_QUEST_LOG_2_2 = UnitField.UNIT_END + 0x10,                           // 0x0D8 - Size: 2 - Type: INT - Flags: PRIVATE
        PLAYER_QUEST_LOG_3_1 = UnitField.UNIT_END + 0x12,                           // 0x0DA - Size: 1 - Type: INT - Flags: GROUP_ONLY
        PLAYER_QUEST_LOG_3_2 = UnitField.UNIT_END + 0x13,                           // 0x0DB - Size: 2 - Type: INT - Flags: PRIVATE
        PLAYER_QUEST_LOG_4_1 = UnitField.UNIT_END + 0x15,                           // 0x0DD - Size: 1 - Type: INT - Flags: GROUP_ONLY
        PLAYER_QUEST_LOG_4_2 = UnitField.UNIT_END + 0x16,                           // 0x0DE - Size: 2 - Type: INT - Flags: PRIVATE
        PLAYER_QUEST_LOG_5_1 = UnitField.UNIT_END + 0x18,                           // 0x0E0 - Size: 1 - Type: INT - Flags: GROUP_ONLY
        PLAYER_QUEST_LOG_5_2 = UnitField.UNIT_END + 0x19,                           // 0x0E1 - Size: 2 - Type: INT - Flags: PRIVATE
        PLAYER_QUEST_LOG_6_1 = UnitField.UNIT_END + 0x1B,                           // 0x0E3 - Size: 1 - Type: INT - Flags: GROUP_ONLY
        PLAYER_QUEST_LOG_6_2 = UnitField.UNIT_END + 0x1C,                           // 0x0E4 - Size: 2 - Type: INT - Flags: PRIVATE
        PLAYER_QUEST_LOG_7_1 = UnitField.UNIT_END + 0x1E,                           // 0x0E6 - Size: 1 - Type: INT - Flags: GROUP_ONLY
        PLAYER_QUEST_LOG_7_2 = UnitField.UNIT_END + 0x1F,                           // 0x0E7 - Size: 2 - Type: INT - Flags: PRIVATE
        PLAYER_QUEST_LOG_8_1 = UnitField.UNIT_END + 0x21,                           // 0x0E9 - Size: 1 - Type: INT - Flags: GROUP_ONLY
        PLAYER_QUEST_LOG_8_2 = UnitField.UNIT_END + 0x22,                           // 0x0EA - Size: 2 - Type: INT - Flags: PRIVATE
        PLAYER_QUEST_LOG_9_1 = UnitField.UNIT_END + 0x24,                           // 0x0EC - Size: 1 - Type: INT - Flags: GROUP_ONLY
        PLAYER_QUEST_LOG_9_2 = UnitField.UNIT_END + 0x25,                           // 0x0ED - Size: 2 - Type: INT - Flags: PRIVATE
        PLAYER_QUEST_LOG_10_1 = UnitField.UNIT_END + 0x27,                          // 0x0EF - Size: 1 - Type: INT - Flags: GROUP_ONLY
        PLAYER_QUEST_LOG_10_2 = UnitField.UNIT_END + 0x28,                          // 0x0F0 - Size: 2 - Type: INT - Flags: PRIVATE
        PLAYER_QUEST_LOG_11_1 = UnitField.UNIT_END + 0x2A,                          // 0x0F2 - Size: 1 - Type: INT - Flags: GROUP_ONLY
        PLAYER_QUEST_LOG_11_2 = UnitField.UNIT_END + 0x2B,                          // 0x0F3 - Size: 2 - Type: INT - Flags: PRIVATE
        PLAYER_QUEST_LOG_12_1 = UnitField.UNIT_END + 0x2D,                          // 0x0F5 - Size: 1 - Type: INT - Flags: GROUP_ONLY
        PLAYER_QUEST_LOG_12_2 = UnitField.UNIT_END + 0x2E,                          // 0x0F6 - Size: 2 - Type: INT - Flags: PRIVATE
        PLAYER_QUEST_LOG_13_1 = UnitField.UNIT_END + 0x30,                          // 0x0F8 - Size: 1 - Type: INT - Flags: GROUP_ONLY
        PLAYER_QUEST_LOG_13_2 = UnitField.UNIT_END + 0x31,                          // 0x0F9 - Size: 2 - Type: INT - Flags: PRIVATE
        PLAYER_QUEST_LOG_14_1 = UnitField.UNIT_END + 0x33,                          // 0x0FB - Size: 1 - Type: INT - Flags: GROUP_ONLY
        PLAYER_QUEST_LOG_14_2 = UnitField.UNIT_END + 0x34,                          // 0x0FC - Size: 2 - Type: INT - Flags: PRIVATE
        PLAYER_QUEST_LOG_15_1 = UnitField.UNIT_END + 0x36,                          // 0x0FE - Size: 1 - Type: INT - Flags: GROUP_ONLY
        PLAYER_QUEST_LOG_15_2 = UnitField.UNIT_END + 0x37,                          // 0x0FF - Size: 2 - Type: INT - Flags: PRIVATE
        PLAYER_QUEST_LOG_16_1 = UnitField.UNIT_END + 0x39,                          // 0x101 - Size: 1 - Type: INT - Flags: GROUP_ONLY
        PLAYER_QUEST_LOG_16_2 = UnitField.UNIT_END + 0x3A,                          // 0x102 - Size: 2 - Type: INT - Flags: PRIVATE
        PLAYER_QUEST_LOG_17_1 = UnitField.UNIT_END + 0x3C,                          // 0x104 - Size: 1 - Type: INT - Flags: GROUP_ONLY
        PLAYER_QUEST_LOG_17_2 = UnitField.UNIT_END + 0x3D,                          // 0x105 - Size: 2 - Type: INT - Flags: PRIVATE
        PLAYER_QUEST_LOG_18_1 = UnitField.UNIT_END + 0x3F,                          // 0x107 - Size: 1 - Type: INT - Flags: GROUP_ONLY
        PLAYER_QUEST_LOG_18_2 = UnitField.UNIT_END + 0x40,                          // 0x108 - Size: 2 - Type: INT - Flags: PRIVATE
        PLAYER_QUEST_LOG_19_1 = UnitField.UNIT_END + 0x42,                          // 0x10A - Size: 1 - Type: INT - Flags: GROUP_ONLY
        PLAYER_QUEST_LOG_19_2 = UnitField.UNIT_END + 0x43,                          // 0x10B - Size: 2 - Type: INT - Flags: PRIVATE
        PLAYER_QUEST_LOG_20_1 = UnitField.UNIT_END + 0x45,                          // 0x10D - Size: 1 - Type: INT - Flags: GROUP_ONLY
        PLAYER_QUEST_LOG_20_2 = UnitField.UNIT_END + 0x46,                          // 0x10E - Size: 2 - Type: INT - Flags: PRIVATE
        PLAYER_VISIBLE_ITEM_1_CREATOR = UnitField.UNIT_END + 0x48,                  // 0x110 - Size: 2 - Type: GUID - Flags: PUBLIC
        PLAYER_VISIBLE_ITEM_1_0 = UnitField.UNIT_END + 0x4A,                        // 0x112 - Size: 8 - Type: INT - Flags: PUBLIC
        PLAYER_VISIBLE_ITEM_1_PROPERTIES = UnitField.UNIT_END + 0x52,               // 0x11A - Size: 1 - Type: TWO_SHORT - Flags: PUBLIC
        PLAYER_VISIBLE_ITEM_1_PAD = UnitField.UNIT_END + 0x53,                      // 0x11B - Size: 1 - Type: INT - Flags: PUBLIC
        PLAYER_VISIBLE_ITEM_2_CREATOR = UnitField.UNIT_END + 0x54,                  // 0x11C - Size: 2 - Type: GUID - Flags: PUBLIC
        PLAYER_VISIBLE_ITEM_2_0 = UnitField.UNIT_END + 0x56,                        // 0x11E - Size: 8 - Type: INT - Flags: PUBLIC
        PLAYER_VISIBLE_ITEM_2_PROPERTIES = UnitField.UNIT_END + 0x5E,               // 0x126 - Size: 1 - Type: TWO_SHORT - Flags: PUBLIC
        PLAYER_VISIBLE_ITEM_2_PAD = UnitField.UNIT_END + 0x5F,                      // 0x127 - Size: 1 - Type: INT - Flags: PUBLIC
        PLAYER_VISIBLE_ITEM_3_CREATOR = UnitField.UNIT_END + 0x60,                  // 0x128 - Size: 2 - Type: GUID - Flags: PUBLIC
        PLAYER_VISIBLE_ITEM_3_0 = UnitField.UNIT_END + 0x62,                        // 0x12A - Size: 8 - Type: INT - Flags: PUBLIC
        PLAYER_VISIBLE_ITEM_3_PROPERTIES = UnitField.UNIT_END + 0x6A,               // 0x132 - Size: 1 - Type: TWO_SHORT - Flags: PUBLIC
        PLAYER_VISIBLE_ITEM_3_PAD = UnitField.UNIT_END + 0x6B,                      // 0x133 - Size: 1 - Type: INT - Flags: PUBLIC
        PLAYER_VISIBLE_ITEM_4_CREATOR = UnitField.UNIT_END + 0x6C,                  // 0x134 - Size: 2 - Type: GUID - Flags: PUBLIC
        PLAYER_VISIBLE_ITEM_4_0 = UnitField.UNIT_END + 0x6E,                        // 0x136 - Size: 8 - Type: INT - Flags: PUBLIC
        PLAYER_VISIBLE_ITEM_4_PROPERTIES = UnitField.UNIT_END + 0x76,               // 0x13E - Size: 1 - Type: TWO_SHORT - Flags: PUBLIC
        PLAYER_VISIBLE_ITEM_4_PAD = UnitField.UNIT_END + 0x77,                      // 0x13F - Size: 1 - Type: INT - Flags: PUBLIC
        PLAYER_VISIBLE_ITEM_5_CREATOR = UnitField.UNIT_END + 0x78,                  // 0x140 - Size: 2 - Type: GUID - Flags: PUBLIC
        PLAYER_VISIBLE_ITEM_5_0 = UnitField.UNIT_END + 0x7A,                        // 0x142 - Size: 8 - Type: INT - Flags: PUBLIC
        PLAYER_VISIBLE_ITEM_5_PROPERTIES = UnitField.UNIT_END + 0x82,               // 0x14A - Size: 1 - Type: TWO_SHORT - Flags: PUBLIC
        PLAYER_VISIBLE_ITEM_5_PAD = UnitField.UNIT_END + 0x83,                      // 0x14B - Size: 1 - Type: INT - Flags: PUBLIC
        PLAYER_VISIBLE_ITEM_6_CREATOR = UnitField.UNIT_END + 0x84,                  // 0x14C - Size: 2 - Type: GUID - Flags: PUBLIC
        PLAYER_VISIBLE_ITEM_6_0 = UnitField.UNIT_END + 0x86,                        // 0x14E - Size: 8 - Type: INT - Flags: PUBLIC
        PLAYER_VISIBLE_ITEM_6_PROPERTIES = UnitField.UNIT_END + 0x8E,               // 0x156 - Size: 1 - Type: TWO_SHORT - Flags: PUBLIC
        PLAYER_VISIBLE_ITEM_6_PAD = UnitField.UNIT_END + 0x8F,                      // 0x157 - Size: 1 - Type: INT - Flags: PUBLIC
        PLAYER_VISIBLE_ITEM_7_CREATOR = UnitField.UNIT_END + 0x90,                  // 0x158 - Size: 2 - Type: GUID - Flags: PUBLIC
        PLAYER_VISIBLE_ITEM_7_0 = UnitField.UNIT_END + 0x92,                        // 0x15A - Size: 8 - Type: INT - Flags: PUBLIC
        PLAYER_VISIBLE_ITEM_7_PROPERTIES = UnitField.UNIT_END + 0x9A,               // 0x162 - Size: 1 - Type: TWO_SHORT - Flags: PUBLIC
        PLAYER_VISIBLE_ITEM_7_PAD = UnitField.UNIT_END + 0x9B,                      // 0x163 - Size: 1 - Type: INT - Flags: PUBLIC
        PLAYER_VISIBLE_ITEM_8_CREATOR = UnitField.UNIT_END + 0x9C,                  // 0x164 - Size: 2 - Type: GUID - Flags: PUBLIC
        PLAYER_VISIBLE_ITEM_8_0 = UnitField.UNIT_END + 0x9E,                        // 0x166 - Size: 8 - Type: INT - Flags: PUBLIC
        PLAYER_VISIBLE_ITEM_8_PROPERTIES = UnitField.UNIT_END + 0xA6,               // 0x16E - Size: 1 - Type: TWO_SHORT - Flags: PUBLIC
        PLAYER_VISIBLE_ITEM_8_PAD = UnitField.UNIT_END + 0xA7,                      // 0x16F - Size: 1 - Type: INT - Flags: PUBLIC
        PLAYER_VISIBLE_ITEM_9_CREATOR = UnitField.UNIT_END + 0xA8,                  // 0x170 - Size: 2 - Type: GUID - Flags: PUBLIC
        PLAYER_VISIBLE_ITEM_9_0 = UnitField.UNIT_END + 0xAA,                        // 0x172 - Size: 8 - Type: INT - Flags: PUBLIC
        PLAYER_VISIBLE_ITEM_9_PROPERTIES = UnitField.UNIT_END + 0xB2,               // 0x17A - Size: 1 - Type: TWO_SHORT - Flags: PUBLIC
        PLAYER_VISIBLE_ITEM_9_PAD = UnitField.UNIT_END + 0xB3,                      // 0x17B - Size: 1 - Type: INT - Flags: PUBLIC
        PLAYER_VISIBLE_ITEM_10_CREATOR = UnitField.UNIT_END + 0xB4,                 // 0x17C - Size: 2 - Type: GUID - Flags: PUBLIC
        PLAYER_VISIBLE_ITEM_10_0 = UnitField.UNIT_END + 0xB6,                       // 0x17E - Size: 8 - Type: INT - Flags: PUBLIC
        PLAYER_VISIBLE_ITEM_10_PROPERTIES = UnitField.UNIT_END + 0xBE,              // 0x186 - Size: 1 - Type: TWO_SHORT - Flags: PUBLIC
        PLAYER_VISIBLE_ITEM_10_PAD = UnitField.UNIT_END + 0xBF,                     // 0x187 - Size: 1 - Type: INT - Flags: PUBLIC
        PLAYER_VISIBLE_ITEM_11_CREATOR = UnitField.UNIT_END + 0xC0,                 // 0x188 - Size: 2 - Type: GUID - Flags: PUBLIC
        PLAYER_VISIBLE_ITEM_11_0 = UnitField.UNIT_END + 0xC2,                       // 0x18A - Size: 8 - Type: INT - Flags: PUBLIC
        PLAYER_VISIBLE_ITEM_11_PROPERTIES = UnitField.UNIT_END + 0xCA,              // 0x192 - Size: 1 - Type: TWO_SHORT - Flags: PUBLIC
        PLAYER_VISIBLE_ITEM_11_PAD = UnitField.UNIT_END + 0xCB,                     // 0x193 - Size: 1 - Type: INT - Flags: PUBLIC
        PLAYER_VISIBLE_ITEM_12_CREATOR = UnitField.UNIT_END + 0xCC,                 // 0x194 - Size: 2 - Type: GUID - Flags: PUBLIC
        PLAYER_VISIBLE_ITEM_12_0 = UnitField.UNIT_END + 0xCE,                       // 0x196 - Size: 8 - Type: INT - Flags: PUBLIC
        PLAYER_VISIBLE_ITEM_12_PROPERTIES = UnitField.UNIT_END + 0xD6,              // 0x19E - Size: 1 - Type: TWO_SHORT - Flags: PUBLIC
        PLAYER_VISIBLE_ITEM_12_PAD = UnitField.UNIT_END + 0xD7,                     // 0x19F - Size: 1 - Type: INT - Flags: PUBLIC
        PLAYER_VISIBLE_ITEM_13_CREATOR = UnitField.UNIT_END + 0xD8,                 // 0x1A0 - Size: 2 - Type: GUID - Flags: PUBLIC
        PLAYER_VISIBLE_ITEM_13_0 = UnitField.UNIT_END + 0xDA,                       // 0x1A2 - Size: 8 - Type: INT - Flags: PUBLIC
        PLAYER_VISIBLE_ITEM_13_PROPERTIES = UnitField.UNIT_END + 0xE2,              // 0x1AA - Size: 1 - Type: TWO_SHORT - Flags: PUBLIC
        PLAYER_VISIBLE_ITEM_13_PAD = UnitField.UNIT_END + 0xE3,                     // 0x1AB - Size: 1 - Type: INT - Flags: PUBLIC
        PLAYER_VISIBLE_ITEM_14_CREATOR = UnitField.UNIT_END + 0xE4,                 // 0x1AC - Size: 2 - Type: GUID - Flags: PUBLIC
        PLAYER_VISIBLE_ITEM_14_0 = UnitField.UNIT_END + 0xE6,                       // 0x1AE - Size: 8 - Type: INT - Flags: PUBLIC
        PLAYER_VISIBLE_ITEM_14_PROPERTIES = UnitField.UNIT_END + 0xEE,              // 0x1B6 - Size: 1 - Type: TWO_SHORT - Flags: PUBLIC
        PLAYER_VISIBLE_ITEM_14_PAD = UnitField.UNIT_END + 0xEF,                     // 0x1B7 - Size: 1 - Type: INT - Flags: PUBLIC
        PLAYER_VISIBLE_ITEM_15_CREATOR = UnitField.UNIT_END + 0xF0,                 // 0x1B8 - Size: 2 - Type: GUID - Flags: PUBLIC
        PLAYER_VISIBLE_ITEM_15_0 = UnitField.UNIT_END + 0xF2,                       // 0x1BA - Size: 8 - Type: INT - Flags: PUBLIC
        PLAYER_VISIBLE_ITEM_15_PROPERTIES = UnitField.UNIT_END + 0xFA,              // 0x1C2 - Size: 1 - Type: TWO_SHORT - Flags: PUBLIC
        PLAYER_VISIBLE_ITEM_15_PAD = UnitField.UNIT_END + 0xFB,                     // 0x1C3 - Size: 1 - Type: INT - Flags: PUBLIC
        PLAYER_VISIBLE_ITEM_16_CREATOR = UnitField.UNIT_END + 0xFC,                 // 0x1C4 - Size: 2 - Type: GUID - Flags: PUBLIC
        PLAYER_VISIBLE_ITEM_16_0 = UnitField.UNIT_END + 0xFE,                       // 0x1C6 - Size: 8 - Type: INT - Flags: PUBLIC
        PLAYER_VISIBLE_ITEM_16_PROPERTIES = UnitField.UNIT_END + 0x106,             // 0x1CE - Size: 1 - Type: TWO_SHORT - Flags: PUBLIC
        PLAYER_VISIBLE_ITEM_16_PAD = UnitField.UNIT_END + 0x107,                    // 0x1CF - Size: 1 - Type: INT - Flags: PUBLIC
        PLAYER_VISIBLE_ITEM_17_CREATOR = UnitField.UNIT_END + 0x108,                // 0x1D0 - Size: 2 - Type: GUID - Flags: PUBLIC
        PLAYER_VISIBLE_ITEM_17_0 = UnitField.UNIT_END + 0x10A,                      // 0x1D2 - Size: 8 - Type: INT - Flags: PUBLIC
        PLAYER_VISIBLE_ITEM_17_PROPERTIES = UnitField.UNIT_END + 0x112,             // 0x1DA - Size: 1 - Type: TWO_SHORT - Flags: PUBLIC
        PLAYER_VISIBLE_ITEM_17_PAD = UnitField.UNIT_END + 0x113,                    // 0x1DB - Size: 1 - Type: INT - Flags: PUBLIC
        PLAYER_VISIBLE_ITEM_18_CREATOR = UnitField.UNIT_END + 0x114,                // 0x1DC - Size: 2 - Type: GUID - Flags: PUBLIC
        PLAYER_VISIBLE_ITEM_18_0 = UnitField.UNIT_END + 0x116,                      // 0x1DE - Size: 8 - Type: INT - Flags: PUBLIC
        PLAYER_VISIBLE_ITEM_18_PROPERTIES = UnitField.UNIT_END + 0x11E,             // 0x1E6 - Size: 1 - Type: TWO_SHORT - Flags: PUBLIC
        PLAYER_VISIBLE_ITEM_18_PAD = UnitField.UNIT_END + 0x11F,                    // 0x1E7 - Size: 1 - Type: INT - Flags: PUBLIC
        PLAYER_VISIBLE_ITEM_19_CREATOR = UnitField.UNIT_END + 0x120,                // 0x1E8 - Size: 2 - Type: GUID - Flags: PUBLIC
        PLAYER_VISIBLE_ITEM_19_0 = UnitField.UNIT_END + 0x122,                      // 0x1EA - Size: 8 - Type: INT - Flags: PUBLIC
        PLAYER_VISIBLE_ITEM_19_PROPERTIES = UnitField.UNIT_END + 0x12A,             // 0x1F2 - Size: 1 - Type: TWO_SHORT - Flags: PUBLIC
        PLAYER_VISIBLE_ITEM_19_PAD = UnitField.UNIT_END + 0x12B,                    // 0x1F3 - Size: 1 - Type: INT - Flags: PUBLIC
        PLAYER_FIELD_INV_SLOT_HEAD = UnitField.UNIT_END + 0x12C,                    // 0x1F4 - Size: 46 - Type: GUID - Flags: PRIVATE
        PLAYER_FIELD_PACK_SLOT_1 = UnitField.UNIT_END + 0x15A,                      // 0x222 - Size: 32 - Type: GUID - Flags: PRIVATE
        PLAYER_FIELD_BANK_SLOT_1 = UnitField.UNIT_END + 0x17A,                      // 0x242 - Size: 48 - Type: GUID - Flags: PRIVATE
        PLAYER_FIELD_BANKBAG_SLOT_1 = UnitField.UNIT_END + 0x1AA,                   // 0x272 - Size: 12 - Type: GUID - Flags: PRIVATE
        PLAYER_FIELD_VENDORBUYBACK_SLOT_1 = UnitField.UNIT_END + 0x1B6,             // 0x27E - Size: 24 - Type: GUID - Flags: PRIVATE
        PLAYER_FARSIGHT = UnitField.UNIT_END + 0x1CE,                               // 0x296 - Size: 2 - Type: GUID - Flags: PRIVATE
        PLAYER__FIELD_COMBO_TARGET = UnitField.UNIT_END + 0x1D0,                    // 0x298 - Size: 2 - Type: GUID - Flags: PRIVATE
        PLAYER_XP = UnitField.UNIT_END + 0x1D2,                                     // 0x29A - Size: 1 - Type: INT - Flags: PRIVATE
        PLAYER_NEXT_LEVEL_XP = UnitField.UNIT_END + 0x1D3,                          // 0x29B - Size: 1 - Type: INT - Flags: PRIVATE
        PLAYER_SKILL_INFO_1_1 = UnitField.UNIT_END + 0x1D4,                         // 0x29C - Size: 384 - Type: TWO_SHORT - Flags: PRIVATE
        PLAYER_CHARACTER_POINTS1 = UnitField.UNIT_END + 0x354,                      // 0x41C - Size: 1 - Type: INT - Flags: PRIVATE
        PLAYER_CHARACTER_POINTS2 = UnitField.UNIT_END + 0x355,                      // 0x41D - Size: 1 - Type: INT - Flags: PRIVATE
        PLAYER_TRACK_CREATURES = UnitField.UNIT_END + 0x356,                        // 0x41E - Size: 1 - Type: INT - Flags: PRIVATE
        PLAYER_TRACK_RESOURCES = UnitField.UNIT_END + 0x357,                        // 0x41F - Size: 1 - Type: INT - Flags: PRIVATE
        PLAYER_BLOCK_PERCENTAGE = UnitField.UNIT_END + 0x358,                       // 0x420 - Size: 1 - Type: FLOAT - Flags: PRIVATE
        PLAYER_DODGE_PERCENTAGE = UnitField.UNIT_END + 0x359,                       // 0x421 - Size: 1 - Type: FLOAT - Flags: PRIVATE
        PLAYER_PARRY_PERCENTAGE = UnitField.UNIT_END + 0x35A,                       // 0x422 - Size: 1 - Type: FLOAT - Flags: PRIVATE
        PLAYER_CRIT_PERCENTAGE = UnitField.UNIT_END + 0x35B,                        // 0x423 - Size: 1 - Type: FLOAT - Flags: PRIVATE
        PLAYER_RANGED_CRIT_PERCENTAGE = UnitField.UNIT_END + 0x35C,                 // 0x424 - Size: 1 - Type: FLOAT - Flags: PRIVATE
        PLAYER_EXPLORED_ZONES_1 = UnitField.UNIT_END + 0x35D,                       // 0x425 - Size: 64 - Type: BYTES - Flags: PRIVATE
        PLAYER_REST_STATE_EXPERIENCE = UnitField.UNIT_END + 0x39D,                  // 0x465 - Size: 1 - Type: INT - Flags: PRIVATE
        PLAYER_FIELD_COINAGE = UnitField.UNIT_END + 0x39E,                          // 0x466 - Size: 1 - Type: INT - Flags: PRIVATE
        PLAYER_FIELD_POSSTAT0 = UnitField.UNIT_END + 0x39F,                         // 0x467 - Size: 1 - Type: INT - Flags: PRIVATE
        PLAYER_FIELD_POSSTAT1 = UnitField.UNIT_END + 0x3A0,                         // 0x468 - Size: 1 - Type: INT - Flags: PRIVATE
        PLAYER_FIELD_POSSTAT2 = UnitField.UNIT_END + 0x3A1,                         // 0x469 - Size: 1 - Type: INT - Flags: PRIVATE
        PLAYER_FIELD_POSSTAT3 = UnitField.UNIT_END + 0x3A2,                         // 0x46A - Size: 1 - Type: INT - Flags: PRIVATE
        PLAYER_FIELD_POSSTAT4 = UnitField.UNIT_END + 0x3A3,                         // 0x46B - Size: 1 - Type: INT - Flags: PRIVATE
        PLAYER_FIELD_NEGSTAT0 = UnitField.UNIT_END + 0x3A4,                         // 0x46C - Size: 1 - Type: INT - Flags: PRIVATE
        PLAYER_FIELD_NEGSTAT1 = UnitField.UNIT_END + 0x3A5,                         // 0x46D - Size: 1 - Type: INT - Flags: PRIVATE
        PLAYER_FIELD_NEGSTAT2 = UnitField.UNIT_END + 0x3A6,                         // 0x46E - Size: 1 - Type: INT - Flags: PRIVATE
        PLAYER_FIELD_NEGSTAT3 = UnitField.UNIT_END + 0x3A7,                         // 0x46F - Size: 1 - Type: INT - Flags: PRIVATE
        PLAYER_FIELD_NEGSTAT4 = UnitField.UNIT_END + 0x3A8,                         // 0x470 - Size: 1 - Type: INT - Flags: PRIVATE
        PLAYER_FIELD_RESISTANCEBUFFMODSPOSITIVE = UnitField.UNIT_END + 0x3A9,       // 0x471 - Size: 7 - Type: INT - Flags: PRIVATE
        PLAYER_FIELD_RESISTANCEBUFFMODSNEGATIVE = UnitField.UNIT_END + 0x3B0,       // 0x478 - Size: 7 - Type: INT - Flags: PRIVATE
        PLAYER_FIELD_MOD_DAMAGE_DONE_POS = UnitField.UNIT_END + 0x3B7,              // 0x47F - Size: 7 - Type: INT - Flags: PRIVATE
        PLAYER_FIELD_MOD_DAMAGE_DONE_NEG = UnitField.UNIT_END + 0x3BE,              // 0x486 - Size: 7 - Type: INT - Flags: PRIVATE
        PLAYER_FIELD_MOD_DAMAGE_DONE_PCT = UnitField.UNIT_END + 0x3C5,              // 0x48D - Size: 7 - Type: INT - Flags: PRIVATE
        PLAYER_FIELD_BYTES = UnitField.UNIT_END + 0x3CC,                            // 0x494 - Size: 1 - Type: BYTES - Flags: PRIVATE
        PLAYER_AMMO_ID = UnitField.UNIT_END + 0x3CD,                                // 0x495 - Size: 1 - Type: INT - Flags: PRIVATE
        PLAYER_SELF_RES_SPELL = UnitField.UNIT_END + 0x3CE,                         // 0x496 - Size: 1 - Type: INT - Flags: PRIVATE
        PLAYER_FIELD_PVP_MEDALS = UnitField.UNIT_END + 0x3CF,                       // 0x497 - Size: 1 - Type: INT - Flags: PRIVATE
        PLAYER_FIELD_BUYBACK_PRICE_1 = UnitField.UNIT_END + 0x3D0,                  // 0x498 - Size: 12 - Type: INT - Flags: PRIVATE
        PLAYER_FIELD_BUYBACK_TIMESTAMP_1 = UnitField.UNIT_END + 0x3DC,              // 0x4A4 - Size: 12 - Type: INT - Flags: PRIVATE
        PLAYER_FIELD_SESSION_KILLS = UnitField.UNIT_END + 0x3E8,                    // 0x4B0 - Size: 1 - Type: TWO_SHORT - Flags: PRIVATE
        PLAYER_FIELD_YESTERDAY_KILLS = UnitField.UNIT_END + 0x3E9,                  // 0x4B1 - Size: 1 - Type: TWO_SHORT - Flags: PRIVATE
        PLAYER_FIELD_LAST_WEEK_KILLS = UnitField.UNIT_END + 0x3EA,                  // 0x4B2 - Size: 1 - Type: TWO_SHORT - Flags: PRIVATE
        PLAYER_FIELD_THIS_WEEK_KILLS = UnitField.UNIT_END + 0x3EB,                  // 0x4B3 - Size: 1 - Type: TWO_SHORT - Flags: PRIVATE
        PLAYER_FIELD_THIS_WEEK_CONTRIBUTION = UnitField.UNIT_END + 0x3EC,           // 0x4B4 - Size: 1 - Type: INT - Flags: PRIVATE
        PLAYER_FIELD_LIFETIME_HONORBALE_KILLS = UnitField.UNIT_END + 0x3ED,         // 0x4B5 - Size: 1 - Type: INT - Flags: PRIVATE
        PLAYER_FIELD_LIFETIME_DISHONORBALE_KILLS = UnitField.UNIT_END + 0x3EE,      // 0x4B6 - Size: 1 - Type: INT - Flags: PRIVATE
        PLAYER_FIELD_YESTERDAY_CONTRIBUTION = UnitField.UNIT_END + 0x3EF,           // 0x4B7 - Size: 1 - Type: INT - Flags: PRIVATE
        PLAYER_FIELD_LAST_WEEK_CONTRIBUTION = UnitField.UNIT_END + 0x3F0,           // 0x4B8 - Size: 1 - Type: INT - Flags: PRIVATE
        PLAYER_FIELD_LAST_WEEK_RANK = UnitField.UNIT_END + 0x3F1,                   // 0x4B9 - Size: 1 - Type: INT - Flags: PRIVATE
        PLAYER_FIELD_BYTES2 = UnitField.UNIT_END + 0x3F2,                           // 0x4BA - Size: 1 - Type: BYTES - Flags: PRIVATE
        PLAYER_FIELD_PADDING = UnitField.UNIT_END + 0x3F3,                          // 0x4BB - Size: 1 - Type: INT - Flags: NONE
        PLAYER_END = UnitField.UNIT_END + 0x3F4                                     // 0x4BC
    }

    public enum GameObjectField
    {
        GAMEOBJECT_FIELD_CREATED_BY = ObjectField.OBJECT_END + 0x0,                 // 0x006 - Size: 2 - Type: GUID - Flags: PUBLIC
        GAMEOBJECT_DISPLAYID = ObjectField.OBJECT_END + 0x2,                        // 0x008 - Size: 1 - Type: INT - Flags: PUBLIC
        GAMEOBJECT_FLAGS = ObjectField.OBJECT_END + 0x3,                            // 0x009 - Size: 1 - Type: INT - Flags: PUBLIC
        GAMEOBJECT_ROTATION = ObjectField.OBJECT_END + 0x4,                         // 0x00A - Size: 4 - Type: FLOAT - Flags: PUBLIC
        GAMEOBJECT_STATE = ObjectField.OBJECT_END + 0x8,                            // 0x00E - Size: 1 - Type: INT - Flags: PUBLIC
        GAMEOBJECT_TIMESTAMP = ObjectField.OBJECT_END + 0x9,                        // 0x00F - Size: 1 - Type: INT - Flags: PUBLIC
        GAMEOBJECT_POS_X = ObjectField.OBJECT_END + 0xA,                            // 0x010 - Size: 1 - Type: FLOAT - Flags: PUBLIC
        GAMEOBJECT_POS_Y = ObjectField.OBJECT_END + 0xB,                            // 0x011 - Size: 1 - Type: FLOAT - Flags: PUBLIC
        GAMEOBJECT_POS_Z = ObjectField.OBJECT_END + 0xC,                            // 0x012 - Size: 1 - Type: FLOAT - Flags: PUBLIC
        GAMEOBJECT_FACING = ObjectField.OBJECT_END + 0xD,                           // 0x013 - Size: 1 - Type: FLOAT - Flags: PUBLIC
        GAMEOBJECT_DYN_FLAGS = ObjectField.OBJECT_END + 0xE,                        // 0x014 - Size: 1 - Type: INT - Flags: DYNAMIC
        GAMEOBJECT_FACTION = ObjectField.OBJECT_END + 0xF,                          // 0x015 - Size: 1 - Type: INT - Flags: PUBLIC
        GAMEOBJECT_TYPE_ID = ObjectField.OBJECT_END + 0x10,                         // 0x016 - Size: 1 - Type: INT - Flags: PUBLIC
        GAMEOBJECT_LEVEL = ObjectField.OBJECT_END + 0x11,                           // 0x017 - Size: 1 - Type: INT - Flags: PUBLIC
        GAMEOBJECT_END = ObjectField.OBJECT_END + 0x12                              // 0x018
    }

    public enum DynamicObjectField
    {
        DYNAMICOBJECT_CASTER = ObjectField.OBJECT_END + 0x0,                        // 0x006 - Size: 2 - Type: GUID - Flags: PUBLIC
        DYNAMICOBJECT_BYTES = ObjectField.OBJECT_END + 0x2,                         // 0x008 - Size: 1 - Type: BYTES - Flags: PUBLIC
        DYNAMICOBJECT_SPELLID = ObjectField.OBJECT_END + 0x3,                       // 0x009 - Size: 1 - Type: INT - Flags: PUBLIC
        DYNAMICOBJECT_RADIUS = ObjectField.OBJECT_END + 0x4,                        // 0x00A - Size: 1 - Type: FLOAT - Flags: PUBLIC
        DYNAMICOBJECT_POS_X = ObjectField.OBJECT_END + 0x5,                         // 0x00B - Size: 1 - Type: FLOAT - Flags: PUBLIC
        DYNAMICOBJECT_POS_Y = ObjectField.OBJECT_END + 0x6,                         // 0x00C - Size: 1 - Type: FLOAT - Flags: PUBLIC
        DYNAMICOBJECT_POS_Z = ObjectField.OBJECT_END + 0x7,                         // 0x00D - Size: 1 - Type: FLOAT - Flags: PUBLIC
        DYNAMICOBJECT_FACING = ObjectField.OBJECT_END + 0x8,                        // 0x00E - Size: 1 - Type: FLOAT - Flags: PUBLIC
        DYNAMICOBJECT_PAD = ObjectField.OBJECT_END + 0x9,                           // 0x00F - Size: 1 - Type: BYTES - Flags: PUBLIC
        DYNAMICOBJECT_END = ObjectField.OBJECT_END + 0xA                            // 0x010
    }

    public enum CorpseField
    {
        CORPSE_FIELD_OWNER = ObjectField.OBJECT_END + 0x0,                          // 0x006 - Size: 2 - Type: GUID - Flags: PUBLIC
        CORPSE_FIELD_FACING = ObjectField.OBJECT_END + 0x2,                         // 0x008 - Size: 1 - Type: FLOAT - Flags: PUBLIC
        CORPSE_FIELD_POS_X = ObjectField.OBJECT_END + 0x3,                          // 0x009 - Size: 1 - Type: FLOAT - Flags: PUBLIC
        CORPSE_FIELD_POS_Y = ObjectField.OBJECT_END + 0x4,                          // 0x00A - Size: 1 - Type: FLOAT - Flags: PUBLIC
        CORPSE_FIELD_POS_Z = ObjectField.OBJECT_END + 0x5,                          // 0x00B - Size: 1 - Type: FLOAT - Flags: PUBLIC
        CORPSE_FIELD_DISPLAY_ID = ObjectField.OBJECT_END + 0x6,                     // 0x00C - Size: 1 - Type: INT - Flags: PUBLIC
        CORPSE_FIELD_ITEM = ObjectField.OBJECT_END + 0x7,                           // 0x00D - Size: 19 - Type: INT - Flags: PUBLIC
        CORPSE_FIELD_BYTES_1 = ObjectField.OBJECT_END + 0x1A,                       // 0x020 - Size: 1 - Type: BYTES - Flags: PUBLIC
        CORPSE_FIELD_BYTES_2 = ObjectField.OBJECT_END + 0x1B,                       // 0x021 - Size: 1 - Type: BYTES - Flags: PUBLIC
        CORPSE_FIELD_GUILD = ObjectField.OBJECT_END + 0x1C,                         // 0x022 - Size: 1 - Type: INT - Flags: PUBLIC
        CORPSE_FIELD_FLAGS = ObjectField.OBJECT_END + 0x1D,                         // 0x023 - Size: 1 - Type: INT - Flags: PUBLIC
        CORPSE_FIELD_DYNAMIC_FLAGS = ObjectField.OBJECT_END + 0x1E,                 // 0x024 - Size: 1 - Type: INT - Flags: DYNAMIC
        CORPSE_FIELD_PAD = ObjectField.OBJECT_END + 0x1F,                           // 0x025 - Size: 1 - Type: INT - Flags: NONE
        CORPSE_END = ObjectField.OBJECT_END + 0x20                                  // 0x026
    }
}
