using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;
using WowPacketParser.Store;
using WowPacketParser.Store.Objects;

namespace WowPacketParserModule.V7_0_3_22248.Parsers
{
    public static class LootHandler
    {
        public static void ReadLootItem(LootEntry loot, Packet packet, params object[] indexes)
        {
            packet.ResetBitReader();

            packet.ReadBits("ItemType", 2, indexes);
            packet.ReadBits("ItemUiType", 3, indexes);
            packet.ReadBit("CanTradeToTapList", indexes);

            ItemInstance itemInstance = Substructures.ItemHandler.ReadItemInstance(packet, indexes, "ItemInstance");
            uint count = packet.ReadUInt32("Quantity", indexes);
            if (loot != null)
            {
                LootItem lootItem = new LootItem();
                lootItem.ItemId = (uint)itemInstance.ItemID;
                lootItem.Count = count;
                loot.ItemsList.Add(lootItem);
            }

            packet.ReadByte("LootItemType", indexes);
            packet.ReadByte("LootListID", indexes);
        }

        [Parser(Opcode.SMSG_LOOT_RESPONSE)]
        public static void HandleLootResponse(Packet packet)
        {
            WowGuid lootOwner = packet.ReadPackedGuid128("Owner");
            WowGuid lootObject = packet.ReadPackedGuid128("LootObj");
            packet.ReadByteE<LootError>("FailureReason");
            packet.ReadByteE<LootType>("AcquireReason");
            packet.ReadByteE<LootMethod>("LootMethod");
            packet.ReadByteE<ItemQuality>("Threshold");

            LootEntry loot = new LootEntry();
            loot.Entry = lootOwner.GetEntry();
            loot.Money = packet.ReadUInt32("Coins");

            uint itemCount = packet.ReadUInt32("ItemCount");
            loot.ItemsCount = itemCount;
            var currencyCount = packet.ReadUInt32("CurrencyCount");

            packet.ResetBitReader();

            packet.ReadBit("Acquired");
            packet.ReadBit("AELooting");
            if (ClientVersion.RemovedInVersion(ClientVersionBuild.V7_2_0_23826))
                packet.ReadBit("PersonalLooting");

            for (var i = 0; i < itemCount; ++i)
                ReadLootItem(loot, packet, i, "LootItem");

            for (var i = 0; i < currencyCount; ++i)
                V6_0_2_19033.Parsers.LootHandler.ReadCurrenciesData(packet, i, "Currencies");

            Storage.StoreLoot(loot, lootOwner, lootObject);
        }

        [Parser(Opcode.SMSG_START_LOOT_ROLL)]
        public static void HandleLootStartRoll(Packet packet)
        {
            packet.ReadPackedGuid128("LootObj");
            packet.ReadInt32<MapId>("MapID");
            packet.ReadUInt32("RollTime");
            packet.ReadByte("ValidRolls");
            packet.ReadByteE<LootMethod>("Method");
            ReadLootItem(null, packet, "LootItem");
        }

        [Parser(Opcode.SMSG_LOOT_ROLL)]
        public static void HandleLootRollServer(Packet packet)
        {
            packet.ReadPackedGuid128("LootObj");
            packet.ReadPackedGuid128("Winner");
            packet.ReadInt32("Roll");
            packet.ReadByte("RollType");
            ReadLootItem(null, packet, "LootItem");
            packet.ResetBitReader();
            packet.ReadBit("MainSpec");
        }

        [Parser(Opcode.SMSG_LOOT_ROLL_WON)]
        public static void HandleLootRollWon(Packet packet)
        {
            packet.ReadPackedGuid128("LootObj");
            packet.ReadPackedGuid128("Player");
            packet.ReadInt32("Roll");
            packet.ReadByte("RollType");
            ReadLootItem(null, packet, "LootItem");
            packet.ReadBit("MainSpec");
        }

        [Parser(Opcode.SMSG_LOOT_ALL_PASSED)]
        public static void HandleLootAllPassed(Packet packet)
        {
            packet.ReadPackedGuid128("LootObj");
            ReadLootItem(null, packet, "LootItem");
        }

        [Parser(Opcode.SMSG_LOOT_LIST, ClientVersionBuild.V7_2_0_23826)]
        public static void HandleLootList(Packet packet)
        {
            packet.ReadPackedGuid128("Owner");
            packet.ReadPackedGuid128("LootObj");

            var hasMaster = packet.ReadBit("HasMaster");
            var hasRoundRobin = packet.ReadBit("HasRoundRobinWinner");

            if (hasMaster)
                packet.ReadPackedGuid128("Master");

            if (hasRoundRobin)
                packet.ReadPackedGuid128("RoundRobinWinner");
        }
    }
}
