using System;
using System.Collections.Generic;
using System.Globalization;
using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;
using WowPacketParser.SQL;
using WowPacketParser.Store;
using WowPacketParser.Store.Objects;
using CoreParsers = WowPacketParser.Parsing.Parsers;

namespace WowPacketParserModule.V1_14_1_40487.Parsers
{
    public static class NpcHandler
    {
        [Parser(Opcode.SMSG_VENDOR_INVENTORY)]
        public static void HandleVendorInventory(Packet packet)
        {
            WowPacketParserModule.V8_0_1_27101.Parsers.NpcHandler.HandleVendorInventory(packet);
        }

        public static void ReadGossipOptionsData(uint menuId, Packet packet, params object[] idx)
        {
            GossipMenuOption gossipOption = new GossipMenuOption
            {
                MenuId = menuId
            };
            GossipMenuOptionBox gossipMenuOptionBox = new GossipMenuOptionBox
            {
                MenuId = menuId
            };

            gossipOption.OptionIndex = gossipMenuOptionBox.OptionIndex = (uint)packet.ReadInt32("ClientOption", idx);
            gossipOption.OptionIcon = (GossipOptionIcon?)packet.ReadByte("OptionNPC", idx);
            gossipMenuOptionBox.BoxCoded = packet.ReadByte("OptionFlags", idx) != 0;
            gossipMenuOptionBox.BoxMoney = (uint)packet.ReadInt32("OptionCost", idx);

            if (ClientVersion.AddedInVersion(ClientVersionBuild.V1_14_1_40666))
                packet.ReadUInt32("Unk", idx);

            packet.ResetBitReader();
            uint textLen = packet.ReadBits("TextLength", 12, idx);
            uint confirmLen = packet.ReadBits("ConfirmLength", 12, idx);
            bool hasSpellId = false;
            if (ClientVersion.AddedInVersion(ClientType.Shadowlands))
            {
                packet.ReadBits("Status", 2, idx);
                if (ClientVersion.AddedInVersion(ClientVersionBuild.V9_0_2_36639))
                    hasSpellId = packet.ReadBit("HasSpellId", idx);

                uint rewardsCount = packet.ReadUInt32("RewardsCount", idx);
                for (uint i = 0; i < rewardsCount; ++i)
                {
                    packet.ResetBitReader();
                    packet.ReadBits("Type", 1, idx, "TreasureItem", i);
                    packet.ReadInt32("ID", idx, "TreasureItem", i);
                    packet.ReadInt32("Quantity", idx, "TreasureItem", i);
                }
            }

            gossipOption.OptionText = packet.ReadWoWString("Text", textLen, idx);
            gossipMenuOptionBox.BoxText = packet.ReadWoWString("Confirm", confirmLen, idx);

            if (hasSpellId)
                packet.ReadInt32("SpellID", idx);

            List<int> boxTextList;
            List<int> optionTextList;

            if (gossipMenuOptionBox.BoxText != string.Empty && SQLDatabase.BroadcastTexts.TryGetValue(gossipMenuOptionBox.BoxText, out boxTextList))
            {
                if (boxTextList.Count == 1)
                    gossipMenuOptionBox.BoxBroadcastTextId = boxTextList[0];
                else
                {
                    gossipMenuOptionBox.BroadcastTextIdHelper += "BoxBroadcastTextID: ";
                    gossipMenuOptionBox.BroadcastTextIdHelper += string.Join(" - ", boxTextList);
                }
            }
            else
                gossipMenuOptionBox.BoxBroadcastTextId = 0;

            if (gossipOption.OptionText != string.Empty && SQLDatabase.BroadcastTexts.TryGetValue(gossipOption.OptionText, out optionTextList))
            {
                if (optionTextList.Count == 1)
                    gossipOption.OptionBroadcastTextId = optionTextList[0];
                else
                {
                    gossipOption.BroadcastTextIDHelper += "OptionBroadcastTextID: ";
                    gossipOption.BroadcastTextIDHelper += string.Join(" - ", optionTextList);
                }
            }
            else
                gossipOption.OptionBroadcastTextId = 0;


            Storage.GossipMenuOptions.Add(gossipOption, packet.TimeSpan);
            if (!gossipMenuOptionBox.IsEmpty)
                Storage.GossipMenuOptionBoxes.Add(gossipMenuOptionBox, packet.TimeSpan);
        }

        [HasSniffData]
        [Parser(Opcode.SMSG_GOSSIP_MESSAGE)]
        public static void HandleNpcGossip(Packet packet)
        {
            GossipMenu gossip = new GossipMenu();

            WowGuid guid = packet.ReadPackedGuid128("GossipGUID");

            gossip.ObjectType = guid.GetObjectType();
            gossip.ObjectEntry = guid.GetEntry();

            int menuId = packet.ReadInt32("GossipID");
            gossip.Entry = (uint)menuId;

            packet.ReadInt32("FriendshipFactionID");

            gossip.TextID = (uint)packet.ReadInt32("TextID");

            int int44 = packet.ReadInt32("GossipOptions");
            int int60 = packet.ReadInt32("GossipText");

            for (int i = 0; i < int44; ++i)
                ReadGossipOptionsData((uint)menuId, packet, i, "GossipOptions");

            for (int i = 0; i < int60; ++i)
                V7_0_3_22248.Parsers.NpcHandler.ReadGossipQuestTextData(packet, i, "GossipQuestText");

            Storage.StoreCreatureGossip(guid, (uint)menuId, packet);
            Storage.Gossips.Add(gossip, packet.TimeSpan);
            CoreParsers.NpcHandler.CanBeDefaultGossipMenu = false;
            var lastGossipOption = CoreParsers.NpcHandler.LastGossipOption;
            var tempGossipOptionPOI = CoreParsers.NpcHandler.TempGossipOptionPOI;

            if (lastGossipOption.HasSelection)
            {
                if ((packet.TimeSpan - lastGossipOption.TimeSpan).Duration() <= TimeSpan.FromMilliseconds(2500))
                {
                    Storage.GossipMenuOptionActions.Add(new GossipMenuOptionAction { MenuId = lastGossipOption.MenuId, OptionIndex = lastGossipOption.OptionIndex, ActionMenuId = gossip.Entry, ActionPoiId = lastGossipOption.ActionPoiId }, packet.TimeSpan);

                    //keep temp data (for case SMSG_GOSSIP_POI is delayed)
                    tempGossipOptionPOI.Guid = lastGossipOption.Guid;
                    tempGossipOptionPOI.MenuId = lastGossipOption.MenuId;
                    tempGossipOptionPOI.OptionIndex = lastGossipOption.OptionIndex;
                    tempGossipOptionPOI.ActionMenuId = gossip.Entry;
                    tempGossipOptionPOI.TimeSpan = lastGossipOption.TimeSpan;

                    // clear lastgossip so no faulty linkings appear
                    lastGossipOption.Reset();
                }
                else
                {
                    lastGossipOption.Reset();
                    tempGossipOptionPOI.Reset();

                }
            }

            packet.AddSniffData(StoreNameType.Gossip, menuId, guid.GetEntry().ToString(CultureInfo.InvariantCulture));
        }

        [Parser(Opcode.SMSG_GOSSIP_POI)]
        public static void HandleGossipPoi(Packet packet)
        {
            PointsOfInterest gossipPOI = new PointsOfInterest();

            gossipPOI.ID = packet.ReadInt32("ID");
            Vector3 pos = packet.ReadVector3("Coordinates");
            gossipPOI.PositionX = pos.X;
            gossipPOI.PositionY = pos.Y;
            gossipPOI.PositionZ = pos.Z;
            gossipPOI.Icon = packet.ReadInt32E<GossipPOIIcon>("Icon");
            gossipPOI.Importance = (uint)packet.ReadInt32("Importance");
            packet.ReadInt32("Unk");

            packet.ResetBitReader();
            gossipPOI.Flags = packet.ReadBits("Flags", 14);
            uint bit84 = packet.ReadBits(6);
            gossipPOI.Name = packet.ReadWoWString("Name", bit84);

            var lastGossipOption = CoreParsers.NpcHandler.LastGossipOption;
            var tempGossipOptionPOI = CoreParsers.NpcHandler.TempGossipOptionPOI;

            lastGossipOption.ActionPoiId = gossipPOI.ID;
            tempGossipOptionPOI.ActionPoiId = gossipPOI.ID;

            Storage.GossipPOIs.Add(gossipPOI, packet.TimeSpan);

            if (tempGossipOptionPOI.HasSelection)
            {
                if ((packet.TimeSpan - tempGossipOptionPOI.TimeSpan).Duration() <= TimeSpan.FromMilliseconds(2500))
                {
                    if (tempGossipOptionPOI.ActionMenuId != null)
                    {
                        Storage.GossipMenuOptionActions.Add(new GossipMenuOptionAction { MenuId = tempGossipOptionPOI.MenuId, OptionIndex = tempGossipOptionPOI.OptionIndex, ActionMenuId = tempGossipOptionPOI.ActionMenuId, ActionPoiId = gossipPOI.ID }, packet.TimeSpan);
                        //clear temp
                        tempGossipOptionPOI.Reset();
                    }
                }
                else
                {
                    lastGossipOption.Reset();
                    tempGossipOptionPOI.Reset();
                }
            }
        }
    }
}

