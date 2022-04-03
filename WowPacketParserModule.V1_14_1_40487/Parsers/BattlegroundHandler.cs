using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;
using WowPacketParserModule.V6_0_2_19033.Parsers;

namespace WowPacketParserModule.V1_14_1_40487.Parsers
{
    public static class BattlegroundHandler
    {
        public static void ReadBattlefieldStatusHeader(Packet packet)
        {
            LfgHandler.ReadCliRideTicket(packet);

            if (ClientVersion.AddedInVersion(1, 14, 2))
                packet.ReadByte("Unk254");

            uint queueCount = packet.ReadUInt32("QueueCount");
            packet.ReadByte("RangeMin");
            packet.ReadByte("RangeMax");
            packet.ReadByte("ArenaTeamSize");
            packet.ReadInt32("BattlefieldInstanceID");
            for (uint i = 0; i < queueCount; i++)
            {
                long queueId = packet.ReadInt64("QueueID", i);
                long battleFieldListId = queueId & ~0x1F10000000000000;
                packet.WriteLine($"[{i}] BattlemasterListID: {battleFieldListId}");
            }
            packet.ReadBit("IsArena");
            packet.ReadBit("TournamentRules");
            packet.ResetBitReader();
        }

        [Parser(Opcode.SMSG_BATTLEFIELD_STATUS_QUEUED)]
        public static void HandleBattlefieldStatusQueued(Packet packet)
        {
            ReadBattlefieldStatusHeader(packet);
            packet.ReadInt32("AverageWaitTime");
            packet.ReadInt32("WaitTime");

            if (ClientVersion.AddedInVersion(1, 14, 2))
                packet.ReadUInt32("Unk254");

            packet.ReadBit("AsGroup");
            packet.ReadBit("EligibleForMatchmaking");
        }

        [Parser(Opcode.SMSG_BATTLEFIELD_STATUS_NEED_CONFIRMATION)]
        public static void HandleBattlefieldStatusNeedConfirmation(Packet packet)
        {
            ReadBattlefieldStatusHeader(packet);
            packet.ReadInt32("MapId");
            packet.ReadInt32("Timeout");

            if (ClientVersion.AddedInVersion(1, 14, 2))
                packet.ReadUInt32("Unk254");

            packet.ReadByte("Role");
        }

        [Parser(Opcode.SMSG_BATTLEFIELD_STATUS_FAILED)]
        public static void HandleBattlefieldStatus_Failed(Packet packet)
        {
            LfgHandler.ReadCliRideTicket(packet);
            packet.ReadByte("Unk");
            packet.ReadInt64("QueueID");
            packet.ReadInt32("Reason");
            packet.ReadPackedGuid128("ClientID");
        }

        [Parser(Opcode.SMSG_PVP_MATCH_INITIALIZE)]
        public static void HandlePvPMatchInitialize(Packet packet)
        {
            packet.ReadUInt32("MapID");
            packet.ReadByte("UnkByte");
            packet.ReadTime64("Time");
            packet.ReadInt64("UnkInt64");
            packet.ReadByte("UnkByte");
            packet.ReadUInt32("UnkUInt32");
            packet.ReadBit("UnkBit");
            packet.ReadBit("UnkBit");
        }

        [Parser(Opcode.SMSG_BATTLEGROUND_INIT)]
        public static void HandleBattlegroundInit(Packet packet)
        {
            packet.ReadUInt32("Milliseconds"); // dword_14387B250 = OsGetAsyncTimeMs() - **(_DWORD **)(a1 + 32);
            packet.ReadUInt16("BattlegroundPoints");
        }

        public static void ReadPvPMatchPlayerStatistics(Packet packet, params object[] idx)
        {
            packet.ReadPackedGuid128("PlayerGUID", idx);
            packet.ReadUInt32("Kills", idx);
            packet.ReadUInt32("DamageDone", idx);
            packet.ReadUInt32("HealingDone", idx);
            var statsCount = packet.ReadUInt32("StatsCount", idx);
            packet.ReadInt32("PrimaryTalentTree", idx);
            packet.ReadInt32E<Gender>("Sex", idx);
            packet.ReadInt32E<Race>("Race", idx);
            packet.ReadInt32E<Class>("Class", idx);
            packet.ReadInt32("CreatureID", idx);
            packet.ReadInt32("HonorLevel", idx);
            packet.ReadInt32("Rank", idx); // @TODO only confirmed for 2.5.3

            for (int i = 0; i < statsCount; i++)
                packet.ReadUInt32("Stats", i, idx);

            packet.ResetBitReader();

            packet.ReadBit("Faction", idx);
            packet.ReadBit("IsInWorld", idx);

            var hasHonor = packet.ReadBit("HasHonor", idx);
            var hasPreMatchRating = packet.ReadBit("HasPreMatchRating", idx);
            var hasRatingChange = packet.ReadBit("HasRatingChange", idx);
            var hasPreMatchMMR = packet.ReadBit("HasPreMatchMMR", idx);
            var hasMmrChange = packet.ReadBit("HasMmrChange", idx);

            packet.ResetBitReader();

            if (hasHonor)
                V6_0_2_19033.Parsers.BattlegroundHandler.ReadHonorData(packet, "Honor", idx);

            if (hasPreMatchRating)
                packet.ReadUInt32("PreMatchRating", idx);

            if (hasRatingChange)
                packet.ReadInt32("RatingChange", idx);

            if (hasPreMatchMMR)
                packet.ReadUInt32("PreMatchMMR", idx);

            if (hasMmrChange)
                packet.ReadInt32("MmrChange", idx);
        }

        [Parser(Opcode.SMSG_PVP_LOG_DATA)]
        [Parser(Opcode.SMSG_PVP_MATCH_STATISTICS)]
        public static void HandlePvPLogData(Packet packet)
        {
            var hasRatings = packet.ReadBit("HasRatings");
            var hasUnk = packet.ReadBit("HasUnk");
            var hasWinner = packet.ReadBit("HasWinner");

            if (hasUnk)
            {
                var strLens = new uint[2];
                for (int i = 0; i < 2; i++)
                    strLens[i] = packet.ReadBits("UnkBits", 7, i);

                for (int i = 0; i < 2; i++)
                {
                    packet.ReadPackedGuid128("UnkGuid128", i);
                    packet.ReadWoWString("UnkString", (int)strLens[i], i);
                }
            }

            var statisticsCount = packet.ReadUInt32();
            for (int i = 0; i < 2; i++)
                packet.ReadSByte("PlayerCount", i);

            if (hasRatings)
                V8_0_1_27101.Parsers.BattlegroundHandler.ReadRatingData820(packet, "Ratings");

            if (hasWinner)
                packet.ReadByte("Winner");

            for (int i = 0; i < statisticsCount; i++)
                ReadPvPMatchPlayerStatistics(packet, "Statistics", i);
        }
    }
}
