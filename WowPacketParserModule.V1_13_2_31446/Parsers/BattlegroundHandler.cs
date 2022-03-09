using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;
using WowPacketParserModule.V6_0_2_19033.Parsers;

namespace WowPacketParserModule.V1_13_2_31446.Parsers
{
    public static class BattlegroundHandler
    {
        [Parser(Opcode.SMSG_BATTLEFIELD_STATUS_QUEUED)]
        [Parser(Opcode.SMSG_BATTLEFIELD_STATUS_NEED_CONFIRMATION)]
        public static void HandleBattlefieldStatus(Packet packet) // probably not correct
        {
            LfgHandler.ReadCliRideTicket(packet);
            long queueId = packet.ReadInt64("QueueID");
            long battleFieldListId = queueId & ~0x1F10000000000000;
            packet.WriteLine($"BattlemasterListID: {battleFieldListId}");
            packet.ReadInt32("Unknown");
            packet.ReadInt32("BattlefieldInstanceID");
            packet.ReadInt32("AverageWaitTime");
            packet.ReadInt32("WaitTime");
            packet.ReadByte("TeamSize");
        }

        [Parser(Opcode.SMSG_BATTLEFIELD_LIST)]
        public static void HandleBattlefieldList(Packet packet)
        {
            packet.ReadPackedGuid128("BattlemasterGuid");
            packet.ReadInt32("Verification");
            packet.ReadInt32("BattlemasterListID");
            packet.ReadByte("MinLevel");
            packet.ReadByte("MaxLevel");
            var battlefieldsCount = packet.ReadUInt32("BattlefieldInstanceCount");
            for (var i = 0; i < battlefieldsCount; ++i)
                packet.ReadInt32("BattlefieldInstance");

            packet.ResetBitReader();
            packet.ReadBit("PvpAnywhere");
            packet.ReadBit("HasRandomWinToday");
        }

        [Parser(Opcode.CMSG_BATTLEMASTER_JOIN)]
        public static void HandleBattlemasterJoin(Packet packet)
        {
            long queueId = packet.ReadInt64("QueueID");
            long battleFieldListId = queueId & ~0x1F10000000000000;
            packet.WriteLine($"BattlemasterListID: {battleFieldListId}");
            packet.ReadByte("Roles");

            for (int i = 0; i < 2; i++)
                packet.ReadInt32("BlacklistMap", i);

            packet.ReadPackedGuid128("BattlemasterGuid");
            packet.ReadInt32("Verification");
            packet.ReadInt32("BattlefieldInstanceID");
            packet.ReadBit("JoinAsGroup");
        }
    }
}
