using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;

namespace WowPacketParserModule.V1_14_1_40487.Parsers
{
    public static class PetHandler
    {
        public static void ReadPetStableInfo(Packet packet, params object[] indexes)
        {
            packet.ReadUInt32("PetSlot", indexes);
            packet.ReadUInt32("PetNumber", indexes);
            packet.ReadUInt32("CreatureID", indexes);
            packet.ReadUInt32("DisplayID", indexes);
            packet.ReadByte("ExperienceLevel", indexes);
            packet.ReadByte("PetFlags", indexes);

            packet.ResetBitReader();

            var len = packet.ReadBits(8);
            packet.ReadWoWString("PetName", len, indexes);
        }

        [Parser(Opcode.SMSG_PET_STABLE_LIST)]
        public static void HandlePetStableList(Packet packet)
        {
            packet.ReadPackedGuid128("StableMaster");
            var petCount = packet.ReadInt32("PetCount");
            packet.ReadByte("NumStableSlots");

            for (int i = 0; i < petCount; i++)
                ReadPetStableInfo(packet, i, "PetStableInfo");
        }
    }
}