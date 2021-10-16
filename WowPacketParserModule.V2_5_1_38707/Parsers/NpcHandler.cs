using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;
using WowPacketParser.Store;
using WowPacketParser.Store.Objects;
using CoreParsers = WowPacketParser.Parsing.Parsers;

namespace WowPacketParserModule.V2_5_1_38707.Parsers
{
    public static class NpcHandler
    {
        [Parser(Opcode.SMSG_VENDOR_INVENTORY)]
        public static void HandleVendorInventory(Packet packet)
        {
            WowPacketParserModule.V8_0_1_27101.Parsers.NpcHandler.HandleVendorInventory(packet);
        }
    }
}

