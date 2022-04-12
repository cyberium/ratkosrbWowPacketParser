using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;
using WowPacketParser.Store;

namespace WowPacketParserModule.V2_5_1_38835.Parsers
{
    public static class CharacterHandler
    {
        public static void ReadAccountCharacterList(Packet packet, params object[] idx)
        {
            packet.ReadPackedGuid128("WowAccountGUID", idx);
            packet.ReadPackedGuid128("CharacterGUID", idx);
            packet.ReadUInt32("VirtualRealmAddress", idx);
            packet.ReadByteE<Race>("Race", idx);
            packet.ReadByteE<Class>("Class", idx);
            packet.ReadByteE<Gender>("Gender", idx);
            packet.ReadByte("Level", idx);
            packet.ReadTime64("LastLogin", idx);

            if (ClientVersion.AddedInVersion(ClientVersionBuild.V2_5_3_41531))
                packet.ReadUInt32("Unk", idx);

            packet.ResetBitReader();

            uint characterNameLength = packet.ReadBits(6);
            uint realmNameLength = packet.ReadBits(9);

            packet.ReadWoWString("CharacterName", characterNameLength, idx);
            packet.ReadWoWString("RealmName", realmNameLength, idx);
        }

        [Parser(Opcode.SMSG_GET_ACCOUNT_CHARACTER_LIST_RESULT)]
        public static void HandleGetAccountCharacterListResult(Packet packet)
        {
            packet.ReadUInt32("Token");
            uint count = packet.ReadUInt32("CharacterCount");

            packet.ResetBitReader();

            packet.ReadBit("UnkBit");

            for (var i = 0; i < count; ++i)
            {
                ReadAccountCharacterList(packet, i);
            }
        }

        [Parser(Opcode.CMSG_PLAYER_LOGIN)]
        public static void HandlePlayerLogin(Packet packet)
        {
            Storage.CurrentActivePlayer = packet.ReadPackedGuid128("PlayerGUID");
            packet.ReadSingle("FarClip");
            packet.ReadBit("UnkBit");
        }

        [Parser(Opcode.CMSG_INSPECT_PVP)]
        [Parser(Opcode.CMSG_INSPECT_HONOR_STATS)]
        public static void HandleInspectPvP(Packet packet)
        {
            packet.ReadPackedGuid128("PlayerGUID");
        }

        public static void ReadAzeriteEssenceData(Packet packet, params object[] idx)
        {
            packet.ReadUInt32("Index", idx);
            packet.ReadUInt32("AzeriteEssenceID", idx);
            packet.ReadUInt32("Rank", idx);
            packet.ResetBitReader();
            packet.ReadBit("SlotUnlocked", idx);
        }

        public static void ReadInspectItemData(Packet packet, params object[] idx)
        {
            packet.ReadPackedGuid128("CreatorGUID", idx);
            packet.ReadByte("Index", idx);

            var azeritePowerCount = packet.ReadUInt32("AzeritePowersCount", idx);
            var azeriteEssenceCount = packet.ReadUInt32();

            for (int i = 0; i < azeritePowerCount; i++)
                packet.ReadInt32("AzeritePowerId", idx, i);

            Substructures.ItemHandler.ReadItemInstance(packet, idx, "ItemInstance");

            packet.ResetBitReader();
            packet.ReadBit("Usable", idx);
            var enchantsCount = packet.ReadBits("EnchantsCount", 4, idx);
            var gemsCount = packet.ReadBits("GemsCount", 2, idx);

            for (int i = 0; i < azeriteEssenceCount; i++)
                ReadAzeriteEssenceData(packet, "AzeriteEssence", i);

            for (int i = 0; i < enchantsCount; i++)
            {
                packet.ReadUInt32("Id", idx, "EnchantData", i);
                packet.ReadByte("Index", idx, "EnchantData", i);
            }

            for (int i = 0; i < gemsCount; i++)
            {
                packet.ReadByte("Slot", idx, "GemData", i);
                Substructures.ItemHandler.ReadItemInstance(packet, idx, "GemData", "ItemInstance", i);
            }
        }

        public static void ReadPlayerModelDisplayInfo(Packet packet, params object[] idx)
        {
            packet.ReadPackedGuid128("InspecteeGUID", idx);
            packet.ReadInt32("SpecializationID", idx);
            var itemCount = packet.ReadUInt32();

            packet.ResetBitReader();
            var nameLen = packet.ReadBits(6);

            packet.ReadByteE<Gender>("GenderID", idx);
            packet.ReadByteE<Race>("Race", idx);
            packet.ReadByteE<Class>("ClassID", idx);
            var customizationCount = packet.ReadUInt32();
            packet.ReadWoWString("Name", nameLen, idx);

            for (var i = 0; i < customizationCount; ++i)
                V9_0_1_36216.Parsers.CharacterHandler.ReadChrCustomizationChoice(packet, idx, "Customizations", i);

            for (int i = 0; i < itemCount; i++)
                ReadInspectItemData(packet, idx, "InspectItemData", i);
        }

        [Parser(Opcode.SMSG_INSPECT_RESULT)]
        public static void HandleInspectResult(Packet packet)
        {
            ReadPlayerModelDisplayInfo(packet, "DisplayInfo");
            var glyphCount = packet.ReadUInt32("GlyphsCount");
            var talentCount = packet.ReadUInt32("TalentsCount");
            packet.ReadInt32("ItemLevel");
            packet.ReadByte("LifetimeMaxRank");
            packet.ReadUInt16("TodayHK");
            packet.ReadUInt16("YesterdayHK");
            packet.ReadUInt32("LifetimeHK");
            packet.ReadUInt32("HonorLevel");

            for (int i = 0; i < glyphCount; i++)
                packet.ReadUInt16("Glyphs", i);

            for (int i = 0; i < talentCount; i++)
                packet.ReadByte("Talents", i);

            packet.ResetBitReader();
            var hasGuildData = packet.ReadBit("HasGuildData");
            var hasAzeriteLevel = packet.ReadBit("HasAzeriteLevel");

            for (int i = 0; i < 6; i++)
            {
                packet.ReadByte("Bracket", i, "PvpData");
                packet.ReadInt32("Rating", i, "PvpData");
                packet.ReadInt32("Rank", i, "PvpData");
                packet.ReadInt32("WeeklyPlayed", i, "PvpData");
                packet.ReadInt32("WeeklyWon", i, "PvpData");
                packet.ReadInt32("SeasonPlayed", i, "PvpData");
                packet.ReadInt32("SeasonWon", i, "PvpData");
                packet.ReadInt32("WeeklyBestRating", i, "PvpData");
                packet.ReadInt32("Unk710", i, "PvpData");
                packet.ReadInt32("Unk801_1", i, "PvpData");
                packet.ReadInt32("Unk252_1", i, "PvpData");
                packet.ResetBitReader();
                packet.ReadBit("Unk801_2", i, "PvpData");
            }

            if (hasGuildData)
            {
                packet.ReadPackedGuid128("GuildGUID");
                packet.ReadInt32("NumGuildMembers");
                packet.ReadInt32("GuildAchievementPoints");
            }
            if (hasAzeriteLevel)
                packet.ReadInt32("AzeriteLevel");
        }

        [Parser(Opcode.SMSG_INSPECT_HONOR_STATS)]
        public static void HandleInspectHonorStats(Packet packet)
        {
            packet.ReadPackedGuid128("PlayerGuid");
            packet.ReadByte("LifetimeHighestRank");
            packet.ReadUInt16("TodayHonorableKills");
            packet.ReadUInt16("TodayDishonorableKills");
            packet.ReadUInt16("YesterdayHonorableKills");
            packet.ReadUInt16("YesterdayDishonorableKills");
            if (ClientVersion.AddedInVersion(2, 5, 4))
            {
                packet.ReadUInt16("LastWeekHonorableKills");
                packet.ReadUInt16("LastWeekDishonorableKills");
                packet.ReadUInt16("ThisWeekHonorableKills");
                packet.ReadUInt16("ThisWeekDishonorableKills");
                packet.ReadUInt32("LifetimeHonorableKills");
                packet.ReadUInt32("LifetimeDishonorableKills");
                packet.ReadUInt32("YesterdayHonor");
                packet.ReadUInt32("LastWeekHonor");
                packet.ReadUInt32("ThisWeekHonor");
                packet.ReadUInt32("Standing");
                packet.ReadByte("RankProgress");
            }
            else
            {
                packet.ReadUInt32("LastWeekHonorableKills");
                packet.ReadUInt32("ThisWeekHonorableKills");
                packet.ReadUInt32("LifetimeHonorableKills");
                packet.ReadUInt32("LifetimeDishonorableKills");
                packet.ReadUInt32("YesterdayHonor");
                packet.ReadByte("RankProgress");
            }
        }

        public static void ReadPVPBracketData(Packet packet, params object[] idx)
        {
            packet.ReadByte("Bracket", idx);
            packet.ReadInt32("Rating", idx);
            packet.ReadInt32("Rank", idx);
            packet.ReadInt32("WeeklyPlayed", idx);
            packet.ReadInt32("WeeklyWon", idx);
            packet.ReadInt32("SeasonPlayed", idx);
            packet.ReadInt32("SeasonWon", idx);
            packet.ReadInt32("WeeklyBestRating", idx);
            packet.ReadInt32("SeasonBestRating", idx);
            packet.ReadInt32("PvpTierID", idx);
            packet.ReadInt32("WeeklyBestWinPvpTierID ", idx);
            packet.ResetBitReader();
            packet.ReadBit("Disqualified", idx);
        }

        public static void ReadArenaTeamData(Packet packet, params object[] idx)
        {
            packet.ReadPackedGuid128("TeamGuid", idx);
            packet.ReadInt32("TeamRating", idx);
            packet.ReadInt32("TeamGamesPlayed", idx);
            packet.ReadInt32("TeamGamesWon", idx);
            packet.ReadInt32("PersonalGamesPlayed", idx);
            packet.ReadInt32("PersonalRating", idx);
        }

        [Parser(Opcode.SMSG_INSPECT_PVP)]
        public static void HandleInspectPVP(Packet packet)
        {
            packet.ReadPackedGuid128("ClientGUID");
            var bracketCount = packet.ReadBits("BracketDataCount", 3);
            var arenaTeamsCount = packet.ReadBits("ArenaTeamCount", 2);

            for (var i = 0; i < bracketCount; i++)
                ReadPVPBracketData(packet, i, "PVPBracketData");
            for (var i = 0; i < arenaTeamsCount; i++)
                ReadArenaTeamData(packet, i, "ArenaTeamData");
        }

        [Parser(Opcode.SMSG_ENUM_CHARACTERS_RESULT)]
        public static void HandleEnumCharactersResult(Packet packet)
        {
            WowPacketParserModule.V9_0_1_36216.Parsers.CharacterHandler.HandleEnumCharactersResult(packet);
        }
    }
}