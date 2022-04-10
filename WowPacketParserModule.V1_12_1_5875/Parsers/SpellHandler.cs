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
    [SuppressMessage("ReSharper", "UseObjectOrCollectionInitializer")]
    public static class SpellHandler
    {
        [Parser(Opcode.SMSG_CAST_FAILED)]
        public static void HandleCastFailed(Packet packet)
        {
            packet.ReadUInt32<SpellId>("Spell ID");
            var status = packet.ReadByte("Status");
            if (status != 2)
                return;

            // i cba to add the whole enum
            var result = packet.ReadByte("Reason");
            switch (result)
            {
                case 94: // SPELL_FAILED_REQUIRES_SPELL_FOCUS
                {
                    packet.ReadUInt32("Required Spell Focus");
                    break;
                }
                case 93: // SPELL_FAILED_REQUIRES_AREA
                {
                    packet.ReadUInt32("Required Area");
                    break;
                }
                case 25: // SPELL_FAILED_EQUIPPED_ITEM_CLASS
                {
                    packet.ReadUInt32("Equipped Item Class");
                    packet.ReadUInt32("Equipped Item Sub Class Mask");
                    packet.ReadUInt32("Equipped Item Inventory Type Mask");
                    break;
                }
            }
        }

        [Parser(Opcode.SMSG_SPELL_FAILURE)]
        [Parser(Opcode.SMSG_SPELL_FAILED_OTHER)]
        public static void HandleSpellFailedOther(Packet packet)
        {
            SpellCastFailed failData = new SpellCastFailed();
            failData.Guid = packet.ReadGuid("Guid");
            failData.SpellId = packet.ReadUInt32<SpellId>("Spell ID");
            failData.Time = packet.Time;
            Storage.SpellCastFailed.Add(failData);
        }

        [Parser(Opcode.SMSG_INIT_EXTRA_AURA_INFO_OBSOLETE)] // 2.4.3
        public static void HandleInitExtraAuraInfo(Packet packet)
        {
            packet.ReadPackedGuid("GUID");
            int index = 0;
            while (packet.CanRead())
            {
                packet.ReadByte("Slot", index);
                packet.ReadUInt32<SpellId>("SpellID", index);
                packet.ReadInt32("Max Duration", index);
                packet.ReadUInt32("Duration", index);
                index++;
            }
        }

        [Parser(Opcode.SMSG_SET_EXTRA_AURA_INFO_OBSOLETE)] // 2.4.3
        [Parser(Opcode.SMSG_SET_EXTRA_AURA_INFO_NEED_UPDATE_OBSOLETE)] // 2.4.3
        public static void HandleSetExtraAuraInfo(Packet packet)
        {
            packet.ReadPackedGuid();
            packet.ReadByte("Slot");
            packet.ReadUInt32<SpellId>("SpellID");
            if (packet.CanRead())
            {
                packet.ReadInt32("Max Duration");
                packet.ReadUInt32("Duration");
            }
        }

        [Parser(Opcode.SMSG_CLEAR_EXTRA_AURA_INFO_OBSOLETE)] // 2.4.3
        public static void HandleClearExtraAuraInfo(Packet packet)
        {
            packet.ReadPackedGuid();
            packet.ReadUInt32<SpellId>("SpellID");
        }
    }
}
