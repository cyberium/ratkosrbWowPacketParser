using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;

namespace WowPacketParserModule.V1_14_1_40487.Parsers
{
    public static class ItemHandler
    {
        [Parser(Opcode.CMSG_USE_ITEM)]
        public static void HandleUseItem(Packet packet)
        {
            packet.ReadByte("PackSlot");
            packet.ReadByte("Slot");
            packet.ReadPackedGuid128("CastItem");

            SpellHandler.ReadSpellCastRequest(packet, "Cast");
        }
    }
}
