﻿using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;

namespace WowPacketParserModule.V6_0_2_19033.Parsers
{
    public static class TradeHandler
    {
        [Parser(Opcode.CMSG_ACCEPT_TRADE)]
        public static void HandleAcceptTrade(Packet packet)
        {
            packet.ReadUInt32("State Index");
        }

        [Parser(Opcode.SMSG_TRADE_UPDATED)]
        public static void HandleTradeUpdated(Packet packet)
        {
            packet.ReadByte("WhichPlayer");

            packet.ReadInt32("ID");
            packet.ReadInt32("CurrentStateIndex");
            packet.ReadInt32("ClientStateIndex");

            packet.ReadInt64("Gold");

            // Order guessed
            packet.ReadInt32("CurrencyType");
            packet.ReadInt32("CurrencyQuantity");
            packet.ReadInt32("ProposedEnchantment");

            var count = packet.ReadInt32("ItemCount");

            for (int i = 0; i < count; i++)
            {
                packet.ReadByte("Slot", i);
                packet.ReadInt32("EntryID", i);
                packet.ReadInt32("StackCount", i);

                packet.ReadPackedGuid128("GiftCreator", i);

                packet.ResetBitReader();

                var bit32 = packet.ReadBit("HasUnwrapped", i);
                if (bit32)
                {
                    ItemHandler.ReadItemInstance(packet, i);

                    packet.ReadInt32("EnchantID", i);
                    packet.ReadInt32("OnUseEnchantmentID", i);

                    for (int j = 0; j < 3; j++)
                        packet.ReadInt32("SocketEnchant", i, j);

                    packet.ReadPackedGuid128("Creator", i);

                    packet.ReadInt32("MaxDurability", i);
                    packet.ReadInt32("Durability", i);
                    packet.ReadInt32("Charges", i);

                    packet.ResetBitReader();

                    packet.ReadBit("Lock", i);
                }
            }
        }
    }
}