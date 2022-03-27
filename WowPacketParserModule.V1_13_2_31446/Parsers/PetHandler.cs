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

        public static void ReadPetStableInfo(Packet packet, params object[] indexes)
        {
            packet.ReadUInt32("PetNumber", indexes);
            packet.ReadUInt32("CreatureID", indexes);
            packet.ReadUInt32("DisplayID", indexes);
            packet.ReadUInt32("ExperienceLevel", indexes);
            packet.ReadByte("LoyaltyLevel", indexes);
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

        [Parser(Opcode.CMSG_STABLE_PET)]
        public static void HandleStablePet(Packet packet)
        {
            packet.ReadPackedGuid128("StableMaster");
        }

        [Parser(Opcode.CMSG_UNSTABLE_PET)]
        [Parser(Opcode.CMSG_STABLE_SWAP_PET)]
        public static void HandleUnstablePet(Packet packet)
        {
            packet.ReadUInt32("PetSlot");
            packet.ReadPackedGuid128("StableMaster");
        }

        [Parser(Opcode.CMSG_BUY_STABLE_SLOT)]
        public static void HandleBuyStableSlot(Packet packet)
        {
            packet.ReadPackedGuid128("StableMaster");
        }
    }
}