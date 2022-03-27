using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;

namespace WowPacketParserModule.V2_5_1_38707.Parsers
{
    public static class AuctionHouseHandler
    {
        public static void ReadActionBucketKey(Packet packet, params object[] idx)
        {
            packet.ResetBitReader();
            packet.ReadBits("ItemId", 20, idx);
            bool hasBattlePetSpeciesID = packet.ReadBit();
            packet.ReadBits("ItemLevel", 11, idx);
            bool hasSuffixItemNameDescriptionID = packet.ReadBit();

            if (hasBattlePetSpeciesID)
                packet.ReadUInt16("BattlePetSpeciesID", idx);

            if (hasSuffixItemNameDescriptionID)
                packet.ReadUInt16("SuffixItemNameDescriptionID", idx);
        }
        public static void ReadCliAuctionItem(Packet packet, params object[] idx)
        {
            packet.ResetBitReader();
            bool hasItemInstance = packet.ReadBit("HasItemInstance", idx);
            var enchantmentsCount = packet.ReadBits("EnchantmentsCount", 4, idx);
            var gemsCount = packet.ReadBits("GemsCount", 2, idx);
            bool hasMinBid = packet.ReadBit("HasMinBid", idx);

            bool hasMinIncrement = packet.ReadBit("HasMinIncrement", idx);
            bool hasBuyoutPrice = packet.ReadBit("HasBuyoutPrice", idx);
            bool hasUnitPrice = packet.ReadBit("HasUnitPrice", idx);
            bool censorServerSideInfo = packet.ReadBit("CensorServerSideInfo", idx);
            bool censorBidInfo = packet.ReadBit("CensorBidInfo", idx);
            bool hasAuctionBucketKey = packet.ReadBit("HasAuctionBucketKey", idx);
            bool hasCreator = packet.ReadBit("HasCreator", idx);
            bool hasBidder = false;
            bool hasBidAmount = false;
            if (!censorBidInfo)
            {
                hasBidder = packet.ReadBit("HasBidder", idx);
                hasBidAmount = packet.ReadBit("HasBidAmount", idx);
            }
            packet.ReadBits("UnkBits2", 7, idx);

            if (hasItemInstance)
                Substructures.ItemHandler.ReadItemInstance(packet, idx);

            packet.ReadInt32("Count", idx);
            packet.ReadInt32("Charges", idx);
            packet.ReadInt32("Flags", idx);
            packet.ReadInt32("AuctionItemID", idx);

            packet.ReadPackedGuid128("Owner", idx);
            packet.ReadInt32("DurationLeft", idx);
            packet.ReadByte("DeleteReason", idx);

            for (int i = 0; i < enchantmentsCount; i++)
            {
                packet.ReadInt32("ID", idx, i);
                packet.ReadUInt32("Expiration", idx, i);
                packet.ReadInt32("Charges", idx, i);
                packet.ReadByte("Slot", idx, i);
            }

            if (hasMinBid)
                packet.ReadUInt64("MinBid", idx);

            if (hasMinIncrement)
                packet.ReadUInt64("MinIncrement", idx);

            if (hasBuyoutPrice)
                packet.ReadUInt64("BuyoutPrice", idx);

            if (hasUnitPrice)
                packet.ReadUInt64("UnitPrice", idx);

            if (!censorServerSideInfo)
            {
                packet.ReadPackedGuid128("ItemGUID", idx);
                packet.ReadPackedGuid128("OwnerAccountID", idx);
                packet.ReadInt32("EndTime", idx);
            }

            if (hasCreator)
                packet.ReadPackedGuid128("Creator", idx);

            if (hasBidder)
                packet.ReadPackedGuid128("Bidder", idx);

            if (hasBidAmount)
                packet.ReadInt64("BidAmount", idx);

            for (int i = 0; i < gemsCount; i++)
            {
                WowPacketParserModule.V7_0_3_22248.Parsers.ItemHandler.ReadItemGemInstanceData(packet, "Gems", idx, i);
            }

            if (hasAuctionBucketKey)
                ReadActionBucketKey(packet, idx);
        }
        [Parser(Opcode.SMSG_AUCTION_LIST_ITEMS_RESULT)]
        public static void HandleListItemsResult(Packet packet)
        {
            var itemsCount = packet.ReadInt32("ItemsCount");

            packet.ReadInt32("TotalCount");
            packet.ReadInt32("DesiredDelay");

            if (itemsCount > 0)
                packet.ReadBool("OnlyUsable");

            for (var i = 0; i < itemsCount; i++)
                ReadCliAuctionItem(packet, i);
        }

        [Parser(Opcode.CMSG_AUCTION_SELL_ITEM)]
        public static void HandleAuctionSellItem(Packet packet)
        {
            packet.ReadPackedGuid128("Auctioneer");
            packet.ReadInt64("BidPrice");
            packet.ReadInt64("BuyoutPrice");
            packet.ReadInt32("RunTime");
            bool hasAddonInfo = ClientVersion.AddedInVersion(2, 5, 4) ? (bool)packet.ReadBit("HasAddonInfo") : false;
            var count = packet.ReadBits("ItemsCount", 6);
            packet.ResetBitReader();

            if (hasAddonInfo)
                WowPacketParserModule.V9_0_1_36216.Parsers.AuctionHouseHandler.ReadAddonInfo(packet);

            for (var i = 0; i < count; ++i)
            {
                packet.ReadPackedGuid128("Guid", i);
                packet.ReadInt32("UseCount");
            }
        }

        [Parser(Opcode.CMSG_AUCTION_REMOVE_ITEM)]
        public static void HandleAuctionRemoveItem(Packet packet)
        {
            packet.ReadPackedGuid128("Auctioneer");
            packet.ReadInt32("AuctionItemID");
            bool hasAddonInfo = packet.ReadBit("HasAddonInfo");

            if (hasAddonInfo)
                WowPacketParserModule.V9_0_1_36216.Parsers.AuctionHouseHandler.ReadAddonInfo(packet);
        }

        [Parser(Opcode.CMSG_AUCTION_PLACE_BID)]
        public static void HandleAuctionPlaceBid(Packet packet)
        {
            packet.ReadPackedGuid128("Auctioneer");
            packet.ReadInt32("AuctionItemID");
            packet.ReadInt64("BidAmount");
            bool hasAddonInfo = packet.ReadBit("HasAddonInfo");

            if (hasAddonInfo)
                WowPacketParserModule.V9_0_1_36216.Parsers.AuctionHouseHandler.ReadAddonInfo(packet);
        }

        [Parser(Opcode.SMSG_AUCTION_COMMAND_RESULT)]
        public static void HandleAuctionCommandResult(Packet packet)
        {
            packet.ReadUInt32("AuctionItemID");
            packet.ReadUInt32E<AuctionHouseAction>("Command");
            packet.ReadUInt32E<AuctionHouseError>("ErrorCode");
            packet.ReadUInt32("BagResult");
            packet.ReadPackedGuid128("Guid");

            // One of the following is MinIncrement and the other is Money, order still unknown
            packet.ReadUInt64("MinIncrement");
            packet.ReadUInt64("Money");
            packet.ReadUInt32("DesiredDelay");
        }

        [Parser(Opcode.SMSG_AUCTION_LIST_BIDDED_ITEMS_RESULT)]
        [Parser(Opcode.SMSG_AUCTION_LIST_OWNED_ITEMS_RESULT)]
        public static void HandleAuctionListOwnedItemsResult(Packet packet)
        {
            int itemsCount = packet.ReadInt32("ItemsCount");
            int soldItemsCount = packet.ReadInt32("SoldItemsCount");
            packet.ReadUInt32("DesiredDelay");

            for (int i = 0; i < itemsCount; i++)
                ReadCliAuctionItem(packet, "Items", i);
        }

        [Parser(Opcode.SMSG_AUCTION_OUTBID_NOTIFICATION)]
        public static void HandleAuctionOutbitNotification(Packet packet)
        {
            packet.ReadUInt32("AuctionItemID");
            packet.ReadUInt64("BidAmount");
            Substructures.ItemHandler.ReadItemInstance(packet);
            packet.ReadByte("Unk");
            packet.ReadUInt32("Unk2");
        }

        [Parser(Opcode.SMSG_AUCTION_OWNER_BID_NOTIFICATION)]
        public static void HandleAuctionOwnerBidNotification(Packet packet)
        {
            var mailListCount = packet.ReadUInt32("MailListCount");
            packet.ReadInt32("Field_04");

            for (var i = 0; i < mailListCount; i++)
                V7_0_3_22248.Parsers.MailHandler.ReadMailListEntry(packet, "MailListEntry", i);
        }
    }
}
