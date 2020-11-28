using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;

namespace WowPacketParserModule.V1_13_2_31446.Parsers
{
    public static class PetHandler
    {
        [Parser(Opcode.SMSG_PET_SLOT_UPDATED)]
        public static void HandlePetSlotUpdated(Packet packet)
        {
            packet.ReadInt16("PetNumberA");
            packet.ReadInt16("PetSlotA");
        }
    }
}