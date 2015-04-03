using WowPacketParser.Misc;

namespace WowPacketParser.Enums.Version.V6_0_2_19033
{
    public static class Opcodes_6_0_2
    {
        public static BiDictionary<Opcode, int> Opcodes(Direction direction)
        {
            if (direction == Direction.ClientToServer || direction == Direction.BNClientToServer)
                return ClientOpcodes;
            if (direction == Direction.ServerToClient || direction == Direction.BNServerToClient)
                return ServerOpcodes;
            return MiscOpcodes;
        }

        private static readonly BiDictionary<Opcode, int> ClientOpcodes = new BiDictionary<Opcode, int>
        {
            {Opcode.CMSG_ACCEPT_TRADE, 0x1C85},
            {Opcode.CMSG_ADDON_REGISTERED_PREFIXES, 0x07CC},
            {Opcode.CMSG_AUCTION_HELLO_REQUEST, 0x1074},
            {Opcode.CMSG_AUTH_SESSION, 0x1B05},
            {Opcode.CMSG_BANKER_ACTIVATE, 0x0204},
            {Opcode.CMSG_CAST_SPELL, 0x10C8},
            {Opcode.CMSG_CREATE_CHARACTER, 0x10F3},
            {Opcode.CMSG_CHAR_DELETE, 0x08FC},
            {Opcode.CMSG_ENUM_CHARACTERS, 0x01EC},
            {Opcode.CMSG_CHAR_RACE_OR_FACTION_CHANGE, 0x08B7},
            {Opcode.CMSG_QUERY_CREATURE, 0x14D6},
            {Opcode.CMSG_DB_QUERY_BULK, 0x09AC},
            {Opcode.CMSG_EQUIPMENT_SET_SAVE, 0x0114},
            {Opcode.CMSG_QUERY_GAME_OBJECT, 0x0D97},
            {Opcode.CMSG_GARRISON_MISSION_BONUS_ROLL, 0x14C6},
            {Opcode.CMSG_GOSSIP_SELECT_OPTION, 0x1143},
            {Opcode.CMSG_GUILD_BANK_BUY_TAB, 0x1238},
            {Opcode.CMSG_QUERY_GUILD_INFO, 0x00E4},
            {Opcode.CMSG_GUILD_GET_RANKS, 0x018E},
            {Opcode.CMSG_REQUEST_GUILD_PARTY_STATE, 0x1183},
            {Opcode.CMSG_LIST_INVENTORY, 0x1037},
            {Opcode.CMSG_LOADING_SCREEN_NOTIFY, 0x09B8},
            {Opcode.CMSG_LOGOUT_REQUEST, 0x0513},
            {Opcode.CMSG_CANCEL_TRADE, 0x0591},
            {Opcode.CMSG_LOG_DISCONNECT, 0x1856},
            {Opcode.CMSG_GET_ITEM_PURCHASE_DATA, 0x0154},
            {Opcode.CMSG_CHAT_JOIN_CHANNEL, 0x0EC3},
            {Opcode.CMSG_CHAT_MESSAGE_AFK, 0x0EEF},
            {Opcode.CMSG_CHAT_MESSAGE_DND, 0x12C7},
            {Opcode.CMSG_CHAT_MESSAGE_CHANNEL, 0x0288},
            {Opcode.CMSG_CHAT_MESSAGE_EMOTE, 0x12D4},
            {Opcode.CMSG_CHAT_MESSAGE_GUILD, 0x039B},
            {Opcode.CMSG_CHAT_MESSAGE_PARTY, 0x06EF},
            {Opcode.CMSG_CHAT_MESSAGE_SAY, 0x07B3},
            {Opcode.CMSG_CHAT_MESSAGE_YELL, 0x1288},
            {Opcode.CMSG_MOVE_HEARTBEAT, 0x0E36},
            {Opcode.CMSG_MOVE_TIME_SKIPPED, 0x0F46},
            {Opcode.CMSG_NAME_QUERY, 0x0BA4},
            {Opcode.CMSG_QUERY_NPC_TEXT, 0x0CC3},
            {Opcode.CMSG_QUERY_PAGE_TEXT, 0x0B98},
            {Opcode.CMSG_PING, 0x1B75},
            {Opcode.CMSG_QUERY_PET_NAME, 0x05A7},
            {Opcode.CMSG_PLAYER_LOGIN, 0x03A8},
            {Opcode.CMSG_QUERY_QUEST_COMPLETION_NPCS, 0x02A7},
            {Opcode.CMSG_QUEST_POI_QUERY, 0x0A90},
            {Opcode.CMSG_QUERY_QUEST_INFO, 0x0A94},
            {Opcode.CMSG_QUEST_GIVER_QUERY_QUEST, 0x0358},
            {Opcode.CMSG_QUEST_GIVER_COMPLETE_QUEST, 0x1243},
            {Opcode.CMSG_QUEST_GIVER_STATUS_QUERY, 0x0704},
            {Opcode.CMSG_QUEST_GIVER_STATUS_MULTIPLE_QUERY, 0x0633},
            {Opcode.CMSG_AUTH_CONTINUED_SESSION, 0x1806},
            {Opcode.CMSG_RESET_FACTION_CHEAT, 0x1876},
            {Opcode.CMSG_SET_SELECTION, 0x1038},
            {Opcode.CMSG_GOSSIP_HELLO, 0x0647},
            {Opcode.CMSG_SAVE_CUF_PROFILES, 0x0CC4},
            {Opcode.CMSG_SET_RAID_DIFFICULTY, 0x1A76},
            {Opcode.CMSG_SUSPEND_TOKEN_RESPONSE, 0x0845},
            {Opcode.CMSG_TIME_SYNC_RESPONSE, 0x0A02},
            {Opcode.CMSG_UPDATE_ACCOUNT_DATA, 0x10EF},
            {Opcode.CMSG_VIOLENCE_LEVEL, 0x00D4},
            {Opcode.CMSG_WARDEN_DATA, 0x00F3},
            {Opcode.CMSG_WHO, 0x11AF}
        };

        private static readonly BiDictionary<Opcode, int> ServerOpcodes = new BiDictionary<Opcode, int>
        {
            {Opcode.SMSG_ACCOUNT_CRITERIA_UPDATE, 0x0727},
            {Opcode.SMSG_ACCOUNT_DATA_TIMES, 0x11AC},
            {Opcode.SMSG_ADDON_INFO, 0x1400},
            {Opcode.SMSG_ALL_ACCOUNT_CRITERIA, 0x1603},
            {Opcode.SMSG_ALL_ACHIEVEMENT_DATA, 0x01A4},
            {Opcode.SMSG_ALL_GUILD_ACHIEVEMENTS, 0x027A},
            {Opcode.SMSG_ATTACKER_STATE_UPDATE, 0x11BC},
            {Opcode.SMSG_ATTACK_START, 0x13E4},
            {Opcode.SMSG_ATTACK_STOP, 0x10E7},
            {Opcode.SMSG_AUCTION_COMMAND_RESULT, 0x1554},
            {Opcode.SMSG_AUCTION_HELLO_RESPONSE, 0x0417},
            {Opcode.SMSG_AUCTION_LIST_RESULT, 0x13B4},
            {Opcode.SMSG_AURA_UPDATE, 0x128B},
            {Opcode.SMSG_AUTH_CHALLENGE, 0x10AA},
            {Opcode.SMSG_AUTH_RESPONSE, 0x0564},
            {Opcode.SMSG_BATTLE_PET_JOURNAL, 0x00EF},
            {Opcode.SMSG_BIND_POINT_UPDATE, 0x1428},
            {Opcode.SMSG_CHANNEL_NOTIFY, 0x0C4D},
            {Opcode.SMSG_CHANNEL_NOTIFY_JOINED, 0x1C0A},
            {Opcode.SMSG_CHAT, 0x0E09},
            {Opcode.SMSG_CREATE_CHAR, 0x0637},
            {Opcode.SMSG_DELETE_CHAR, 0x12A4},
            {Opcode.SMSG_CHAR_FACTION_CHANGE_RESULT, 0x1743},
            {Opcode.SMSG_ENUM_CHARACTERS_RESULT, 0x1154},
            {Opcode.SMSG_CACHE_VERSION, 0x10EF},
            {Opcode.SMSG_QUERY_CREATURE_RESPONSE, 0x0203},
            {Opcode.SMSG_CRITERIA_UPDATE, 0x0AEC},
            {Opcode.SMSG_CORPSE_RECLAIM_DELAY, 0x11C0},
            {Opcode.SMSG_DB_REPLY, 0x1574},
            {Opcode.SMSG_EMOTE, 0x03F8},
            {Opcode.SMSG_FACTION_BONUS_INFO, 0x01C0},
            {Opcode.SMSG_FEATURE_SYSTEM_STATUS, 0x0177},
            {Opcode.SMSG_FEATURE_SYSTEM_STATUS_GLUE_SCREEN, 0x0577},
            {Opcode.SMSG_FLIGHT_SPLINE_SYNC, 0x10C4},
            {Opcode.SMSG_QUERY_GAME_OBJECT_RESPONSE, 0x08E3},
            {Opcode.SMSG_GOSSIP_COMPLETE, 0x0292},
            {Opcode.SMSG_GOSSIP_POI, 0x08E8},
            {Opcode.SMSG_GOSSIP_MESSAGE, 0x01EE},
            {Opcode.SMSG_GUILD_EVENT, 0x0229},
            {Opcode.SMSG_GUILD_EVENT_PRESENCE_CHANGE, 0x0649},
            {Opcode.SMSG_GUILD_MOTD, 0x125A},
            {Opcode.SMSG_QUERY_GUILD_INFO_RESPONSE, 0x034A},
            {Opcode.SMSG_GUILD_RANKS, 0x035A},
            {Opcode.SMSG_GUILD_KNOWN_RECIPES, 0x0679},
            {Opcode.SMSG_GUILD_ROSTER, 0x0779},
            {Opcode.SMSG_GUILD_PARTY_STATE, 0x127A},
            {Opcode.SMSG_HIGHEST_THREAT_UPDATE, 0x0604},
            {Opcode.SMSG_HOTFIX_NOTIFY_BLOB, 0x0AA8},
            {Opcode.SMSG_SETUP_CURRENCY, 0x00A4},
            {Opcode.SMSG_INIT_WORLD_STATES, 0x0BB7},
            {Opcode.SMSG_INITIAL_SETUP, 0x12E8},
            {Opcode.SMSG_SEND_KNOWN_SPELLS, 0x0297},
            {Opcode.SMSG_INITIALIZE_FACTIONS, 0x0AAB},
            {Opcode.SMSG_VENDOR_INVENTORY, 0x0103},
            {Opcode.SMSG_ITEM_ENCHANT_TIME_UPDATE, 0x01BB},
            {Opcode.SMSG_LEARNED_SPELLS, 0x02D8},
            {Opcode.SMSG_LFG_PLAYER_INFO, 0x0005},
            {Opcode.SMSG_LOAD_CUF_PROFILES, 0x09B3},
            {Opcode.SMSG_LOAD_EQUIPMENT_SET, 0x01E7},
            {Opcode.SMSG_LOGIN_SET_TIME_SPEED, 0x0528},
            {Opcode.SMSG_LOGIN_VERIFY_WORLD, 0x1044},
            {Opcode.SMSG_LOGOUT_COMPLETE, 0x1077},
            {Opcode.SMSG_LOGOUT_RESPONSE, 0x1408},
            {Opcode.SMSG_MAIL_LIST_RESULT, 0x08AF},
            {Opcode.SMSG_MOTD, 0x0E5D},
            {Opcode.SMSG_MOVE_SPLINE_SET_FLIGHT_SPEED, 0x05D8},
            {Opcode.SMSG_MOVE_SPLINE_SET_RUN_SPEED, 0x1B97},
            {Opcode.SMSG_MOVE_SPLINE_SET_RUN_BACK_SPEED, 0x118B},
            {Opcode.SMSG_MOVE_SPLINE_SET_SWIM_SPEED, 0x01CB},
            {Opcode.SMSG_MOVE_SPLINE_SET_WALK_BACK_SPEED, 0x1AA0},
            {Opcode.SMSG_ON_MONSTER_MOVE, 0x04A4},
            {Opcode.SMSG_QUERY_PLAYER_NAME_RESPONSE, 0x1667},
            {Opcode.SMSG_NEW_WORLD, 0x0003},
            {Opcode.SMSG_QUERY_NPC_TEXT_RESPONSE, 0x0BE8},
            {Opcode.SMSG_QUERY_PAGE_TEXT_RESPONSE, 0x1163},
            {Opcode.SMSG_QUERY_PET_NAME_RESPONSE, 0x03B4},
            {Opcode.SMSG_SPELL_PERIODIC_AURA_LOG, 0x039B},
            {Opcode.SMSG_MOVE_UPDATE, 0x019C},
            {Opcode.SMSG_PONG, 0x1881},
            {Opcode.SMSG_POWER_UPDATE, 0x1424},
            {Opcode.SMSG_PVP_SEASON, 0x0618},
            {Opcode.SMSG_QUERY_TIME_RESPONSE, 0x0224},
            {Opcode.SMSG_QUEST_GIVER_QUEST_DETAILS, 0x02B6},
            {Opcode.SMSG_QUEST_GIVER_STATUS, 0x02A1},
            {Opcode.SMSG_QUEST_GIVER_STATUS_MULTIPLE, 0x01CA},
            {Opcode.SMSG_QUEST_POI_QUERY_RESPONSE, 0x03DE},
            {Opcode.SMSG_QUERY_QUEST_INFO_RESPONSE, 0x00D5},
            {Opcode.SMSG_QUEST_COMPLETION_NPC_RESPONSE, 0x01C6},
            {Opcode.SMSG_GENERATE_RANDOM_CHARACTER_NAME_RESULT, 0x0653},
            {Opcode.SMSG_CONNECT_TO, 0x1082},
            {Opcode.SMSG_RESUME_COMMS, 0x128A},
            {Opcode.SMSG_MAIL_COMMAND_RESULT, 0x0035},
            {Opcode.SMSG_SET_TIME_ZONE_INFORMATION, 0x1257},
            {Opcode.SMSG_SET_FLAT_SPELL_MODIFIER, 0x07B3},
            {Opcode.SMSG_SET_PCT_SPELL_MODIFIER, 0x12D4},
            {Opcode.SMSG_PHASE_SHIFT_CHANGE, 0x0567},
            {Opcode.SMSG_SET_PROFICIENCY, 0x12AF},
            {Opcode.SMSG_CATEGORY_COOLDOWN, 0x07A7},
            {Opcode.SMSG_SPELL_GO, 0x1288},
            {Opcode.SMSG_SPELL_START, 0x0FCB},
            {Opcode.SMSG_SPELL_ENERGIZE_LOG, 0x07E7},
            {Opcode.SMSG_SPELL_HEAL_LOG, 0x0298},
            {Opcode.SMSG_SPELL_EXECUTE_LOG, 0x07D0},
            {Opcode.SMSG_SPELL_NON_MELEE_DAMAGE_LOG, 0x0EC8},
            {Opcode.SMSG_SUSPEND_COMMS, 0x1882},
            {Opcode.SMSG_SUSPEND_TOKEN, 0x0464},
            {Opcode.SMSG_UPDATE_TALENT_DATA, 0x10FF},
            {Opcode.SMSG_TEXT_EMOTE, 0x1458},
            {Opcode.SMSG_TRAINER_LIST, 0x0678},
            {Opcode.SMSG_TRANSFER_PENDING, 0x0BC0},
            {Opcode.SMSG_TIME_SYNC_REQUEST, 0x0CA8},
            {Opcode.SMSG_TUTORIAL_FLAGS, 0x0617},
            {Opcode.SMSG_SEND_UNLEARN_SPELLS, 0x07DB},
            {Opcode.SMSG_UPDATE_ACTION_BUTTONS, 0x03F4},
            {Opcode.SMSG_UPDATE_ACCOUNT_DATA, 0x1427},
            {Opcode.SMSG_UPDATE_OBJECT, 0x03EF},
            {Opcode.SMSG_UPDATE_WORLD_STATE, 0x1368},
            {Opcode.SMSG_VIGNETTE_UPDATE, 0x1613},
            {Opcode.SMSG_VOID_STORAGE_CONTENTS, 0x0137},
            {Opcode.SMSG_WARDEN_DATA, 0x12EF},
            {Opcode.SMSG_WEATHER, 0x01BF},
            {Opcode.SMSG_WEEKLY_SPELL_USAGE, 0x0E8C},
            {Opcode.SMSG_WHO, 0x080A},
            {Opcode.SMSG_WORLD_SERVER_INFO, 0x1164}
        };

        private static readonly BiDictionary<Opcode, int> MiscOpcodes = new BiDictionary<Opcode, int>();
    }
}
