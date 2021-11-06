using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;

namespace WowPacketParserModule.V1_14_1_40487.Parsers
{
    public static class LootHandler
    {
        [Parser(Opcode.SMSG_LOOT_MONEY_NOTIFY)]
        public static void HandleLootMoneyNotify(Packet packet)
        {
            V8_0_1_27101.Parsers.LootHandler.HandleLootMoneyNotify(packet);
        }
    }
}
