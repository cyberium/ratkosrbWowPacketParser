using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using WowPacketParser.Enums;
using WowPacketParser.Enums.Version;
using WowPacketParser.Misc;
using WowPacketParser.Store;
using WowPacketParser.Store.Objects;


namespace WowPacketParser.Parsing.Parsers
{
    public static class GroupHandler
    {
        [Parser(Opcode.SMSG_GROUP_LIST)]
        public static void HandleGroupList(Packet packet)
        {
            var grouptype = packet.ReadByteE<GroupTypeFlag>("Group Type");;
            packet.ReadByteE<GroupUpdateFlag>("Flags");

            var numFields = packet.ReadInt32("Member Count");
            for (var i = 0; i < numFields; i++)
            {
                var name = packet.ReadCString("Name", i);
                var guid = packet.ReadGuid("GUID", i);
                StoreGetters.AddName(guid, name);
                packet.ReadByteE<GroupMemberStatusFlag>("Status", i);
                packet.ReadByteE<GroupUpdateFlag>("Update Flags", i);
            }

            packet.ReadGuid("Leader GUID");

            if (numFields <= 0)
                return;

            packet.ReadByteE<LootMethod>("Loot Method");
            packet.ReadGuid("Looter GUID");
            packet.ReadByteE<ItemQuality>("Loot Threshold");
        }

        [Parser(Opcode.SMSG_PARTY_MEMBER_PARTIAL_STATE)]
        [Parser(Opcode.SMSG_PARTY_MEMBER_FULL_STATE)]
        public static void HandlePartyMemberStats(Packet packet)
        {
            packet.ReadPackedGuid("GUID");
            var updateFlags = packet.ReadUInt32E<GroupUpdateFlagVanilla>("Update Flags");

            if (updateFlags.HasFlag(GroupUpdateFlagVanilla.Status))
                packet.ReadByteE<GroupMemberStatusFlag>("Status");

            if (updateFlags.HasFlag(GroupUpdateFlagVanilla.CurrentHealth))
                packet.ReadUInt16("Current Health");

            if (updateFlags.HasFlag(GroupUpdateFlagVanilla.MaxHealth))
                packet.ReadUInt16("Max Health");

            if (updateFlags.HasFlag(GroupUpdateFlagVanilla.PowerType))
                packet.ReadByteE<PowerType>("Power type");

            if (updateFlags.HasFlag(GroupUpdateFlagVanilla.CurrentPower))
                packet.ReadInt16("Current Power");

            if (updateFlags.HasFlag(GroupUpdateFlagVanilla.MaxPower))
                packet.ReadInt16("Max Power");

            if (updateFlags.HasFlag(GroupUpdateFlagVanilla.Level))
                packet.ReadInt16("Level");

            if (updateFlags.HasFlag(GroupUpdateFlagVanilla.Zone))
                packet.ReadInt16<ZoneId>("Zone Id");

            if (updateFlags.HasFlag(GroupUpdateFlagVanilla.Position))
            {
                packet.ReadInt16("Position X");
                packet.ReadInt16("Position Y");
            }

            if (updateFlags.HasFlag(GroupUpdateFlagVanilla.Auras))
            {
                var auraMask = packet.ReadUInt32("Positive Aura Mask");

                var maxAura = 32;

                for (var i = 0; i < maxAura; ++i)
                {
                    if ((auraMask & (1ul << i)) == 0)
                        continue;

                    packet.ReadUInt16<SpellId>("Spell Id", i);
                }
            }

            if (updateFlags.HasFlag(GroupUpdateFlagVanilla.AurasNegative))
            {
                var auraMask = packet.ReadUInt16("Negative Aura Mask");

                var maxAura = 48;

                for (var i = 0; i < maxAura; ++i)
                {
                    if ((auraMask & (1ul << i)) == 0)
                        continue;

                    packet.ReadUInt16<SpellId>("Spell Id", i);
                }
            }

            if (updateFlags.HasFlag(GroupUpdateFlagVanilla.PetGuid))
                packet.ReadGuid("Pet GUID");

            if (updateFlags.HasFlag(GroupUpdateFlagVanilla.PetName))
                packet.ReadCString("Pet Name");

            if (updateFlags.HasFlag(GroupUpdateFlagVanilla.PetModelId))
                packet.ReadInt16("Pet Display Id");

            if (updateFlags.HasFlag(GroupUpdateFlagVanilla.PetCurrentHealth))
                packet.ReadUInt16("Pet Current Health");

            if (updateFlags.HasFlag(GroupUpdateFlagVanilla.PetMaxHealth))
                packet.ReadUInt16("Pet Max Health");

            if (updateFlags.HasFlag(GroupUpdateFlagVanilla.PetPowerType))
                packet.ReadByteE<PowerType>("Pet Power type");

            if (updateFlags.HasFlag(GroupUpdateFlagVanilla.PetCurrentPower))
                packet.ReadInt16("Pet Current Power");

            if (updateFlags.HasFlag(GroupUpdateFlagVanilla.PetMaxPower))
                packet.ReadInt16("Pet Max Power");

            if (updateFlags.HasFlag(GroupUpdateFlagVanilla.PetAuras))
            {
                var auraMask = packet.ReadUInt32("Pet Positive Aura Mask");

                var maxAura = 32;
                for (var i = 0; i < maxAura; ++i)
                {
                    if ((auraMask & (1ul << i)) == 0)
                        continue;
                    packet.ReadUInt16<SpellId>("Spell Id", i); ;
                }
            }

            if (updateFlags.HasFlag(GroupUpdateFlagVanilla.PetAurasNegative))
            {
                var auraMask = packet.ReadUInt16("Pet Negative Aura Mask");

                var maxAura = 48;
                for (var i = 0; i < maxAura; ++i)
                {
                    if ((auraMask & (1ul << i)) == 0)
                        continue;
                    packet.ReadUInt16<SpellId>("Spell Id", i); ;
                }
            }
        }
    }
}
