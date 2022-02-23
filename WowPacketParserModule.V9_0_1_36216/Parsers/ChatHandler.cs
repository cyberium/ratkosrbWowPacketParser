using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;
using WowPacketParser.Store;
using WowPacketParser.Store.Objects;

namespace WowPacketParserModule.V9_0_1_36216.Parsers
{
    public static class ChatHandler
    {
        [Parser(Opcode.SMSG_CHAT)]
        public static void HandleServerChatMessage(Packet packet)
        {
            ChatMessageTypeNew chatType = packet.ReadByteE<ChatMessageTypeNew>("SlashCmd");
            var text = new ChatPacketData
            {
                TypeNormalized = (ChatMessageType)chatType,
                TypeOriginal = (uint)chatType,
                Language = packet.ReadUInt32E<Language>("Language"),
                SenderGUID = packet.ReadPackedGuid128("SenderGUID")
            };

            packet.ReadPackedGuid128("SenderGuildGUID");
            packet.ReadPackedGuid128("WowAccountGUID");
            text.ReceiverGUID = packet.ReadPackedGuid128("TargetGUID");
            packet.ReadUInt32("TargetVirtualAddress");
            packet.ReadUInt32("SenderVirtualAddress");
            packet.ReadPackedGuid128("PartyGUID");
            packet.ReadInt32("AchievementID");
            packet.ReadSingle("DisplayTime");

            var senderNameLen = packet.ReadBits(11);
            var receiverNameLen = packet.ReadBits(11);
            var prefixLen = packet.ReadBits(5);
            var channelLen = packet.ReadBits(7);
            var textLen = packet.ReadBits(12);
            packet.ReadBits("ChatFlags", 14);

            packet.ReadBit("HideChatLog");
            packet.ReadBit("FakeSenderName");
            bool unk801bit = packet.ReadBit("Unk801_Bit");
            bool hasChannelGuid = false;
            if (ClientVersion.AddedInVersion(ClientVersionBuild.V9_1_0_39185))
                hasChannelGuid = packet.ReadBit("HasChannelGUID");

            text.SenderName = packet.ReadWoWString("Sender Name", senderNameLen);
            text.ReceiverName = packet.ReadWoWString("Receiver Name", receiverNameLen);
            packet.ReadWoWString("Addon Message Prefix", prefixLen);
            text.ChannelName = packet.ReadWoWString("Channel Name", channelLen);

            text.Text = packet.ReadWoWString("Text", textLen);
            if (unk801bit)
                packet.ReadUInt32("Unk801");

            if (hasChannelGuid)
                packet.ReadPackedGuid128("ChannelGUID");

            Storage.StoreText(text, packet);
        }

        [Parser(Opcode.SMSG_EMOTE)]
        public static void HandleEmote(Packet packet)
        {
            var guid = packet.ReadPackedGuid128("GUID");
            var emote = packet.ReadUInt32E<EmoteType>("Emote ID");

            if (ClientVersion.AddedInVersion(ClientVersionBuild.V9_0_5_37503))
            {
                var count = packet.ReadUInt32("SpellVisualKitCount");
                if (ClientVersion.AddedInVersion(9, 2, 0, 1, 14, 2, 2, 5, 3))
                    packet.ReadInt32("SequenceVariation");

                for (var i = 0; i < count; ++i)
                    packet.ReadUInt32("SpellVisualKitID", i);
            }

            Storage.StoreUnitEmote(guid, emote, packet);
        }

        [Parser(Opcode.CMSG_SEND_TEXT_EMOTE)]
        public static void HandleSendTextEmote(Packet packet)
        {
            packet.ReadPackedGuid128("Target");
            packet.ReadInt32E<EmoteTextType>("EmoteID");
            packet.ReadInt32("SoundIndex");

            if (ClientVersion.AddedInVersion(ClientVersionBuild.V9_0_5_37503))
            {
                var count = packet.ReadUInt32("SpellVisualKitCount");
                if (ClientVersion.AddedInVersion(9, 2, 0, 1, 14, 2, 2, 5, 3))
                    packet.ReadInt32("SequenceVariation");

                for (var i = 0; i < count; ++i)
                    packet.ReadUInt32("SpellVisualKitID", i);
            }
        }
    }
}
