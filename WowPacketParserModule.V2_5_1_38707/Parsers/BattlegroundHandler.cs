using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;
using WowPacketParserModule.V6_0_2_19033.Parsers;

namespace WowPacketParserModule.V2_5_1_38707.Parsers
{
    public static class BattlegroundHandler
    {
        [Parser(Opcode.SMSG_BATTLEFIELD_STATUS_QUEUED)]
        [Parser(Opcode.SMSG_BATTLEFIELD_STATUS_NEED_CONFIRMATION)]
        public static void HandleBattlefieldStatusd(Packet packet) // probably not correct
        {
            LfgHandler.ReadCliRideTicket(packet);

            packet.ReadInt32("Unknown");
            packet.ReadInt32("Unknown2");
            packet.ReadInt16("Unknown3");
            packet.ReadByte("Unknown4");
            packet.ReadInt32("BattlefieldInstanceID");
            long queueId = packet.ReadInt64("QueueID");
            long battleFieldListId = queueId & ~0x1F10000000000000;
            packet.WriteLine($"BattlemasterListID: {battleFieldListId}");
            packet.ReadByte("UnkByte");
            packet.ReadInt32("AverageWaitTime");
            packet.ReadInt32("WaitTime");

            packet.ReadByte("TeamSize");
        }
    }
}
