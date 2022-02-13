using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;


namespace WowPacketParserModule.V2_5_1_38707.Parsers
{
    public static class MiscellaneousHandler
    {
        [Parser(Opcode.SMSG_UNK_DEBUG1)]
        public static void HandleLootMoneyNotify(Packet packet)
        {
            packet.ReadPackedGuid128("Guid1");
            packet.ReadPackedGuid128("Guid2");
            packet.ReadInt32("UnkInt3");
            packet.ReadSingle("UnkFloat4");
            packet.ReadSingle("UnkFloat5");
            packet.ReadInt32("UnkInt6");
            packet.ReadByte("UnkByte7");
        }
    }
}
