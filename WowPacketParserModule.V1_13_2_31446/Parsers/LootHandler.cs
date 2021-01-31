using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;

namespace WowPacketParserModule.V1_13_2_31446.Parsers
{
    public static class LootHandler
    {
        [Parser(Opcode.SMSG_LOOT_MONEY_NOTIFY)]
        public static void HandleLootMoneyNotify(Packet packet)
        {
            packet.ReadInt32("Money");
            packet.ResetBitReader();
            packet.ReadBit("SoleLooter");
        }

        [Parser(Opcode.SMSG_LOOT_ROLL_WON)]
        public static void HandleLootRollWon(Packet packet)
        {
            packet.ReadPackedGuid128("LootObj");
            packet.ReadPackedGuid128("Player");
            packet.ReadInt32("Roll");
            packet.ReadByte("RollType");
            WowPacketParserModule.V7_0_3_22248.Parsers.LootHandler.ReadLootItem(null, packet, "LootItem");
            packet.ReadByte("MainSpec");
        }
    }
}
