using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;
using WowPacketParser.Store;
using WowPacketParser.Store.Objects;

namespace WowPacketParserModule.V1_13_2_31446.Parsers
{
    public static class ChatHandler
    {
        [Parser(Opcode.SMSG_CHANNEL_NOTIFY_JOINED, ClientVersionBuild.V1_13_2_31446)]
        public static void HandleChannelNotifyJoined(Packet packet)
        {
            var channelLen = packet.ReadBits(8);
            uint channelWelcomeMsgLen = 0;
            if (ClientVersion.AddedInVersion(ClientVersionBuild.V8_2_0_30898))
                channelWelcomeMsgLen = packet.ReadBits(11);
            else
                channelWelcomeMsgLen = packet.ReadBits(10);

            packet.ReadUInt32E<ChannelFlag>("ChannelFlags");
            packet.ReadInt32("ChatChannelID");
            packet.ReadUInt32("InstanceID");

            if (ClientVersion.AddedInVersion(ClientVersionBuild.V8_2_5_31921))
                packet.ReadPackedGuid128("ChannelGUID");

            packet.ReadWoWString("Channel", channelLen);
            packet.ReadWoWString("ChannelWelcomeMsg", channelWelcomeMsgLen);
        }

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
            uint channelLen = packet.ReadBits(7);
            var textLen = packet.ReadBits(12);
            packet.ReadBits("ChatFlags", 11);

            packet.ReadBit("HideChatLog");
            packet.ReadBit("FakeSenderName");
            bool unk801bit = packet.ReadBit("Unk801_Bit");

            text.SenderName = packet.ReadWoWString("Sender Name", senderNameLen);
            text.ReceiverName = packet.ReadWoWString("Receiver Name", receiverNameLen);
            packet.ReadWoWString("Addon Message Prefix", prefixLen);
            text.ChannelName = packet.ReadWoWString("Channel Name", channelLen);

            text.Text = packet.ReadWoWString("Text", textLen);
            if (unk801bit)
                packet.ReadUInt32("Unk801");

            Storage.StoreText(text, packet);
        }

        [Parser(Opcode.CMSG_CHAT_ADDON_MESSAGE)]
        public static void HandleAddonMessage810(Packet packet)
        {
            var prefixLen = packet.ReadBits(5);
            var testLen = packet.ReadBits(9);
            packet.ReadBit("IsLogged");

            packet.ReadInt32("Type");
            packet.ReadWoWString("Prefix", prefixLen);
            packet.ReadWoWString("Text", testLen);
        }
    }
}
