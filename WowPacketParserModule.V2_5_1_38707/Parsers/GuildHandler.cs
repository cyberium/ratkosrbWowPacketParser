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

        [Parser(Opcode.CMSG_TURN_IN_PETITION)]
        public static void HandlePetitionTurnIn(Packet packet)
        {
            packet.ReadPackedGuid128("Petition GUID");
            if (packet.CanRead())
            {
                packet.ReadUInt32("Background Color");
                packet.ReadInt32("Emblem Style");
                packet.ReadUInt32("Emblem Color");
                packet.ReadInt32("Emblem Border Style");
                packet.ReadUInt32("Emblem Border Color");
            }
        }

        [Parser(Opcode.SMSG_PETITION_SHOW_LIST)]
        public static void HandlePetitionShowList(Packet packet)
        {
            packet.ReadPackedGuid128("GUID");
            var counter = packet.ReadUInt32("Count");
            for (var i = 0; i < counter; i++)
            {
                packet.ReadUInt32("Index", i);
                packet.ReadUInt32("Charter Cost", i);
                packet.ReadUInt32("Charter Entry", i);
                packet.ReadUInt32("Is Arena", i);
                packet.ReadUInt32("Required Signatures", i);
            }
        }
    }
}
