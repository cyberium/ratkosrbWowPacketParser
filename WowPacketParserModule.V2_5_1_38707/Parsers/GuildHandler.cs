using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Store;
using WowPacketParser.Store.Objects;
using WowPacketParser.Parsing;

namespace WowPacketParserModule.V2_5_1_38707.Parsers
{
    public static class GuildHandler
    {
        [Parser(Opcode.SMSG_QUERY_GUILD_INFO_RESPONSE)]
        public static void HandleGuildQueryResponse(Packet packet)
        {
            WowPacketParserModule.V8_0_1_27101.Parsers.GuildHandler.HandleGuildQueryResponse(packet);
        }

        [Parser(Opcode.SMSG_GUILD_ROSTER)]
        public static void HandleGuildRoster(Packet packet)
        {
            WowPacketParserModule.V8_0_1_27101.Parsers.GuildHandler.HandleGuildRoster(packet);
        }


        [Parser(Opcode.CMSG_PETITION_BUY)]
        public static void HandlePetitionBuy(Packet packet)
        {
            int length = (int)packet.ReadBits(7);

            packet.ReadPackedGuid128("Unit");
            packet.ReadUInt32("Unused910");
            packet.ReadWoWString("Name", length);
        }

        [Parser(Opcode.CMSG_OFFER_PETITION)]
        public static void HandlePetitionOffer(Packet packet)
        {
            packet.ReadUInt32("Junk"); // uninitialized variable
            packet.ReadPackedGuid128("Item GUID");
            packet.ReadPackedGuid128("Target GUID");
        }
    }
}
