using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;
using WowPacketParser.Store;
using WowPacketParser.Store.Objects;

namespace WowPacketParserModule.V7_0_3_22248.Parsers
{
    public static class PetHandler
    {
        public static void ReadPetFlags(Packet packet, params object[] idx)
        {
            var petModeFlag = packet.ReadUInt16();
            var reactState = packet.ReadByte();
            var flag = petModeFlag >> 16;
            var commandState = (petModeFlag & flag);

            packet.AddValue("ReactState", (ReactState)reactState, idx);
            packet.AddValue("CommandState", (CommandState)commandState, idx);
            packet.AddValue("Flag", flag, idx);
        }

        public static void ReadPetSpellCooldownData(Packet packet, CreaturePetRemainingCooldown cooldown, params object[] idx)
        {
            cooldown.SpellID = (uint)packet.ReadInt32("SpellID", idx);
            cooldown.Cooldown = (uint)packet.ReadInt32("Duration", idx);
            cooldown.CategoryCooldown = (uint)packet.ReadInt32("CategoryDuration", idx);
            cooldown.ModRate = (uint)packet.ReadSingle("ModRate", idx);
            cooldown.Category = (uint)packet.ReadInt16("Category", idx);
        }

        public static void ReadPetSpellHistoryData(Packet packet, params object[] idx)
        {
            packet.ReadInt32("CategoryID", idx);
            packet.ReadInt32("RecoveryTime", idx);
            packet.ReadSingle("ChargeModRate", idx);
            packet.ReadSByte("ConsumedCharges", idx);
        }

        [Parser(Opcode.SMSG_PET_SPELLS_MESSAGE)]
        public static void HandlePetSpells(Packet packet)
        {
            WowGuid petGuid = packet.ReadPackedGuid128("PetGUID");
            packet.ReadInt16("CreatureFamily");
            packet.ReadInt16("Specialization");
            packet.ReadInt32("TimeLimit");

            ReadPetFlags(packet, "PetModeAndOrders");

            CreaturePetActions petActions = new CreaturePetActions();
            petActions.CasterGUID = petGuid;
            petActions.CasterID = petGuid.GetEntry();
            const int maxCreatureSpells = 10;
            for (var i = 0; i < maxCreatureSpells; i++) // Read pet / vehicle spell ids
                petActions.SpellID[i] = V6_0_2_19033.Parsers.PetHandler.ReadPetAction(packet, "ActionButtons", i);
            petActions.SniffId = packet.SniffIdString;
            if (petGuid.GetHighType() == HighGuidType.Creature)
                Storage.CreaturePetActions.Add(petActions);

            var actionsCount = packet.ReadInt32("ActionsCount");
            var cooldownsCount = packet.ReadUInt32("CooldownsCount");
            var spellHistoryCount = packet.ReadUInt32("SpellHistoryCount");

            for (int i = 0; i < actionsCount; i++)
                V6_0_2_19033.Parsers.PetHandler.ReadPetAction(packet, i, "Actions");

            if (cooldownsCount > 0)
            {
                CreaturePetRemainingCooldown cooldown = new CreaturePetRemainingCooldown();
                
                for (int i = 0; i < cooldownsCount; i++)
                {
                    if (ClientVersion.AddedInVersion(ClientVersionBuild.V7_1_0_22900))
                        ReadPetSpellCooldownData(packet, cooldown, i, "PetSpellCooldown");
                    else
                        V6_0_2_19033.Parsers.PetHandler.ReadPetSpellCooldownData(packet, cooldown, i, "PetSpellCooldown");
                }

                if (petGuid.GetHighType() == HighGuidType.Creature)
                {
                    cooldown.CasterID = petGuid.GetEntry();
                    cooldown.TimeSinceCast = Utilities.GetTimeDiffInMs(Storage.GetLastCastGoTimeForCreature(petGuid, (uint)cooldown.SpellID), packet.Time);
                    cooldown.SniffId = packet.SniffIdString;
                    Storage.CreaturePetRemainingCooldown.Add(cooldown);
                }
            }

            for (int i = 0; i < spellHistoryCount; i++)
            {
                if (ClientVersion.AddedInVersion(ClientVersionBuild.V7_1_0_22900))
                    ReadPetSpellHistoryData(packet, i, "PetSpellHistory");
                else
                    V6_0_2_19033.Parsers.PetHandler.ReadPetSpellHistoryData(packet, i, "PetSpellHistory");
            }
        }

        [Parser(Opcode.SMSG_PET_MODE)]
        public static void HandlePetMode(Packet packet)
        {
            packet.ReadPackedGuid128("PetGUID");
            ReadPetFlags(packet, "PetModeAndOrders");
        }

        [Parser(Opcode.SMSG_PET_NAME_INVALID)]
        public static void HandlePetNameInvalid(Packet packet)
        {
            V6_0_2_19033.Parsers.PetHandler.ReadPetRenameData(packet);
        }

        [Parser(Opcode.CMSG_PET_SPELL_AUTOCAST)]
        public static void HandlePetSpellAutocast(Packet packet)
        {
            packet.ReadPackedGuid128("PetGUID");
            packet.ReadUInt32<SpellId>("SpellID");
            packet.ResetBitReader();
            packet.ReadBit("AutocastEnabled");
        }

        [Parser(Opcode.SMSG_PET_NEWLY_TAMED)]
        public static void HandlePetNewlyTamed(Packet packet)
        {
            packet.ReadPackedGuid128("GUID");
            packet.ResetBitReader();
            packet.ReadBit("SuppressLevelUpAnim");
        }
    }
}
