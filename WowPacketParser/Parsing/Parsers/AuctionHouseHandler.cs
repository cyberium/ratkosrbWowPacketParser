using System;
using WowPacketParser.Enums;
using WowPacketParser.Enums.Version;
using WowPacketParser.Misc;

namespace WowPacketParser.Parsing.Parsers
{
    public static class AuctionHouseHandler
    {
        // TODO: Use this in more places
        private static readonly TypeCode AuctionSize = ClientVersion.AddedInVersion(ClientVersionBuild.V4_0_6a_13623) ? TypeCode.UInt64 : TypeCode.UInt32;

        [Parser(Opcode.MSG_AUCTION_HELLO)]
        public static void HandleAuctionHello(Packet packet)
        {
            packet.ReadGuid("GUID");
            if (packet.Direction == Direction.ClientToServer)
                return;

            packet.ReadUInt32("AuctionHouse ID");

            if (ClientVersion.AddedInVersion(ClientVersionBuild.V3_3_0_10958))
                packet.ReadBool("Enabled");

            NpcHandler.LastGossipOption.Reset();
            NpcHandler.TempGossipOptionPOI.Reset();
        }

        [Parser(Opcode.CMSG_AUCTION_SELL_ITEM)]
        public static void HandleAuctionSellItem(Packet packet)
        {
            packet.ReadGuid("Auctioneer GUID");

            if (ClientVersion.RemovedInVersion(ClientVersionBuild.V3_2_2a_10505))
            {
                packet.ReadGuid("Item Guid");
                packet.ReadUInt32("Item Count");
            }
            else
            {
                if (!packet.CanRead()) // dword_F4955C <= (unsigned int)dword_F49578[v13]
                    return;

                var count = packet.ReadUInt32("Count");
                for (int i = 0; i < count; ++i)
                {
                    packet.ReadGuid("Item Guid", i);
                    packet.ReadInt32("", i);
                }
            }

            if (ClientVersion.AddedInVersion(ClientType.Cataclysm))
            {
                packet.ReadUInt64("Bid");
                packet.ReadUInt64("Buyout");
            }
            else
            {
                packet.ReadUInt32("Bid");
                packet.ReadUInt32("Buyout");
            }

            packet.ReadUInt32("Expire Time");
        }

        [Parser(Opcode.CMSG_AUCTION_REMOVE_ITEM)]
        public static void HandleAuctionRemoveItem(Packet packet)
        {
            packet.ReadGuid("Auctioneer GUID");
            packet.ReadUInt32("Auction Id");
        }

        [Parser(Opcode.CMSG_AUCTION_LIST_ITEMS)]
        public static void HandleAuctionListItems(Packet packet)
        {
            packet.ReadGuid("Auctioneer GUID");
            packet.ReadUInt32("Auction House Id");
            packet.ReadCString("Search Pattern");
            packet.ReadByte("Min Level");
            packet.ReadByte("Max Level");
            packet.ReadInt32("Slot ID");
            packet.ReadInt32("Category");
            packet.ReadInt32("Sub Category");
            packet.ReadInt32("Quality");
            packet.ReadByte("Only Usable");
            if (ClientVersion.AddedInVersion(ClientVersionBuild.V2_0_1_6180))
                packet.ReadBool("Exact Match");
            if (ClientVersion.AddedInVersion(ClientVersionBuild.V4_2_2_14545))
                packet.ReadByte("Unk Byte");

            if(ClientVersion.AddedInVersion(ClientVersionBuild.V2_0_1_6180))
            {
                var count = packet.ReadByte("Sort Count");
                for (var i = 0; i < count; ++i)
                {
                    packet.ReadByte("Type", i);
                    packet.ReadByte("Direction", i);
                }
            }
        }

        [Parser(Opcode.CMSG_AUCTION_PLACE_BID)]
        public static void HandleAuctionPlaceBid(Packet packet)
        {
            packet.ReadGuid("Auctioneer GUID");
            packet.ReadUInt32("Auction Id");

            // I think Blizz got this wrong. Auction Id should be 64 on 4.x, not price.
            packet.ReadValue("Price", AuctionSize);
        }

        [Parser(Opcode.SMSG_AUCTION_COMMAND_RESULT)]
        public static void HandleAuctionCommandResult(Packet packet)
        {
            packet.ReadUInt32("AuctionID");
            var action = packet.ReadUInt32E<AuctionHouseAction>("Action");
            var error = packet.ReadUInt32E<AuctionHouseError>("Error");

            switch (error)
            {
                case AuctionHouseError.Ok:
                    if (action == AuctionHouseAction.Bid)
                        packet.ReadValue("MinIncrement", AuctionSize);
                    break;
                case AuctionHouseError.Inventory:
                    packet.ReadUInt32E<InventoryResult>("EquipError");
                    break;
                case AuctionHouseError.HigherBid:
                    packet.ReadGuid("Bidder");
                    packet.ReadValue("BidAmount", AuctionSize);
                    packet.ReadValue("MinIncrement", AuctionSize);
                    break;
            }
        }

        [Parser(Opcode.SMSG_AUCTION_BIDDER_NOTIFICATION)]
        public static void HandleAuctionBidderNotification(Packet packet)
        {
            packet.ReadUInt32("AuctionHouseID");
            packet.ReadUInt32("AuctionID");
            packet.ReadGuid("BidderGUID");
            packet.ReadValue("BidAmount", AuctionSize);
            packet.ReadValue("MinIncrement", AuctionSize);
            packet.ReadUInt32<ItemId>("ItemEntry");
            packet.ReadUInt32("RandomPropertyID");
        }

        [Parser(Opcode.SMSG_AUCTION_OWNER_NOTIFICATION)]
        public static void HandleAuctionOwnerNotification(Packet packet)
        {
            packet.ReadUInt32("AuctionID");
            packet.ReadValue("BidAmount", AuctionSize);
            packet.ReadValue("MinIncrement", AuctionSize);
            packet.ReadGuid("BidderGUID");
            packet.ReadUInt32<ItemId>("ItemEntry");
            packet.ReadUInt32("RandomPropertyID");
            if (ClientVersion.AddedInVersion(ClientVersionBuild.V3_0_2_9056))
                packet.ReadSingle("TimeLeft");
        }

        [Parser(Opcode.CMSG_AUCTION_LIST_BIDDED_ITEMS)]
        [Parser(Opcode.CMSG_AUCTION_LIST_OWNED_ITEMS)]
        public static void HandleAuctionListBidderItems(Packet packet)
        {
            packet.ReadGuid("Auctioneer GUID");
            packet.ReadUInt32("List From");
            if (packet.Opcode == Opcodes.GetOpcode(Opcode.CMSG_AUCTION_LIST_OWNED_ITEMS, Direction.ClientToServer))
                return;

            var count = packet.ReadUInt32("Outbidded Count");
            for (var i = 0; i < count; ++i)
                packet.ReadUInt32("Auction Id", i);
        }

        [Parser(Opcode.SMSG_AUCTION_LIST_BIDDED_ITEMS_RESULT)]
        [Parser(Opcode.SMSG_AUCTION_LIST_OWNED_ITEMS_RESULT)]
        [Parser(Opcode.SMSG_AUCTION_LIST_ITEMS_RESULT)]
        public static void HandleAuctionListItemsResult(Packet packet)
        {
            var count = packet.ReadUInt32("Count");
            for (var i = 0; i < count; ++i)
            {
                packet.ReadUInt32("Auction Id", i);
                packet.ReadUInt32<ItemId>("Item Entry", i);

                int enchantmentCount = ClientVersion.AddedInVersion(ClientVersionBuild.V4_3_0_15005) ? 10 : ClientVersion.AddedInVersion(ClientVersionBuild.V4_2_2_14545) ? 9 : ClientVersion.AddedInVersion(ClientType.WrathOfTheLichKing) ? 7 : 6;
                if (ClientVersion.RemovedInVersion(ClientVersionBuild.V2_0_1_6180))
                    enchantmentCount = 1;

                for (var j = 0; j < enchantmentCount; ++j)
                {
                    packet.ReadUInt32("Item Enchantment ID", i, j);
                    if (ClientVersion.AddedInVersion(ClientVersionBuild.V2_0_1_6180))
                    {
                        packet.ReadUInt32("Item Enchantment Duration", i, j);
                        packet.ReadUInt32("Item Enchantment Charges", i, j);
                    }
                }

                packet.ReadInt32("Item Random Property ID", i);
                packet.ReadUInt32("Item Suffix", i);
                packet.ReadUInt32("Item Count", i);
                packet.ReadInt32("Item Spell Charges", i);

                if (ClientVersion.AddedInVersion(ClientVersionBuild.V2_0_1_6180))
                    packet.ReadUInt32("Flags", i);

                packet.ReadGuid("Owner", i);
                packet.ReadValue("Start Bid", AuctionSize, i);
                packet.ReadValue("Out Bid", AuctionSize, i);
                packet.ReadValue("Buyout ", AuctionSize, i);
                packet.ReadUInt32("Time Left", i);
                packet.ReadGuid("Bidder", i);
                packet.ReadValue("Bid", AuctionSize, i);
            }

            packet.ReadUInt32("Total item count");
            if (ClientVersion.AddedInVersion(ClientVersionBuild.V2_3_0_7561 ))
                packet.ReadUInt32("Desired delay time");
        }

        [Parser(Opcode.SMSG_AUCTION_REMOVED_NOTIFICATION)]
        public static void HandleAuctionRemovedNotification(Packet packet)
        {
            packet.ReadInt32("Auction ID");
            packet.ReadUInt32<ItemId>("Item Entry");
            packet.ReadInt32("Item Random Property ID");
        }

        [Parser(Opcode.CMSG_AUCTION_LIST_PENDING_SALES)]
        public static void HandleAuctionListPendingSales(Packet packet)
        {
            packet.ReadGuid("GUID");
        }

        [Parser(Opcode.SMSG_AUCTION_LIST_PENDING_SALES)]
        public static void HandleAuctionListPendingSalesResult(Packet packet)
        {
            var count = packet.ReadUInt32("Pending Sales Count");
            for (var i = 0; i < count; ++i)
            {
                packet.ReadCString("Unk String 1", i);
                packet.ReadCString("Unk String 2", i);
                if (ClientVersion.AddedInVersion(ClientVersionBuild.V4_3_4_15595))
                    packet.ReadUInt64("Unk UInt32 1", i);
                else
                    packet.ReadUInt32("Unk UInt32 1", i);
                packet.ReadUInt32("Unk UInt32 2", i);
                packet.ReadSingle("Unk Single", i);
            }
        }
    }
}
