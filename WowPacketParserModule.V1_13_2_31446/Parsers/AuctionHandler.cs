using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;
using CoreParsers = WowPacketParser.Parsing.Parsers;

namespace WowPacketParserModule.V1_13_2_31446.Parsers
{
    public static class AuctionHandler
    {
        [Parser(Opcode.SMSG_AUCTION_HELLO_RESPONSE)]
        public static void HandleServerAuctionHello(Packet packet)
        {
            packet.ReadPackedGuid128("Guid");
            packet.ReadUInt32("AuctionHouseID"); // Unsure
            packet.ReadBit("OpenForBusiness");

            CoreParsers.NpcHandler.LastGossipOption.Reset();
            CoreParsers.NpcHandler.TempGossipOptionPOI.Reset();
        }

        [Parser(Opcode.SMSG_AUCTION_OUTBID_NOTIFICATION)]
        public static void HandleAuctionOutbitNotification(Packet packet)
        {
            packet.ReadUInt32("AuctionHouseID");
            packet.ReadUInt32("AuctionItemID");
            packet.ReadPackedGuid128("Bidder");
            packet.ReadInt32<ItemId>("ItemID");
            packet.ReadUInt32("UnkInt2");
            packet.ReadUInt32("UnkInt3");
            packet.ReadByte("UnkByte");
            packet.ReadUInt64("BidAmount");
            packet.ReadUInt64("MinIncrement");
        }
    }
}
