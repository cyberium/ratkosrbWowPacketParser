using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;

namespace WowPacketParserModule.V1_13_2_31446.Parsers
{
    public static class AreaTriggerHandler
    {
        [Parser(Opcode.SMSG_AREA_TRIGGER_MESSAGE)]
        public static void HandleAreaTriggerReShape(Packet packet)
        {
            packet.ReadUInt32("AreaTriggerID");
        }
    }
}
