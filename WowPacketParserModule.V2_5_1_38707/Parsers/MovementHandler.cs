using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;

namespace WowPacketParserModule.V2_5_1_38707.Parsers
{
    public static class MovementHandler
    {
        [Parser(Opcode.SMSG_LOGIN_SET_TIME_SPEED)]
        public static void HandleLoginSetTimeSpeed(Packet packet)
        {
            WowPacketParserModule.V6_0_2_19033.Parsers.MovementHandler.HandleLoginSetTimeSpeed(packet);
        }

        [HasSniffData]
        [Parser(Opcode.SMSG_NEW_WORLD)]
        public static void HandleNewWorld(Packet packet)
        {
            V7_0_3_22248.Parsers.MovementHandler.HandleNewWorld(packet);
        }
    }
}
