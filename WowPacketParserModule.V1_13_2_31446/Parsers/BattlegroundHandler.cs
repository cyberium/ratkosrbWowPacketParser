using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;
using WowPacketParserModule.V6_0_2_19033.Parsers;

namespace WowPacketParserModule.V1_13_2_31446.Parsers
{
    public static class BattlegroundHandler
    {
        [Parser(Opcode.SMSG_BATTLEFIELD_STATUS_QUEUED)]
        public static void HandleBattlefieldStatus_Queued(Packet packet) // probably not correct
        {
            LfgHandler.ReadCliRideTicket(packet);
            
            var queueIdCount = packet.ReadUInt32("QueueIdCount");
            packet.ReadByte("TeamSize");
            packet.ReadInt32("InstanceID");
            //for (var i = 0u; i < queueIdCount; ++i)
                packet.ReadUInt64("QueueID");
            packet.ReadInt32("AverageWaitTime");
            packet.ReadInt32("WaitTime");
        }

        [Parser(Opcode.SMSG_BATTLEFIELD_STATUS_NEED_CONFIRMATION)]
        public static void HandleBattlefieldStatus_NeedConfirmation(Packet packet) // probably not correct
        {
            LfgHandler.ReadCliRideTicket(packet);

            var queueIdCount = packet.ReadUInt32("QueueIdCount");
            packet.ReadByte("TeamSize");
            packet.ReadInt32("InstanceID");
            //for (var i = 0u; i < queueIdCount; ++i)
            packet.ReadUInt64("QueueID");
            packet.ReadInt32("AverageWaitTime");
            packet.ReadInt32("WaitTime");
        }
    }
}
