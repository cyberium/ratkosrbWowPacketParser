using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;
using WowPacketParser.Store;
using WowPacketParser.Store.Objects;

namespace WowPacketParserModule.V2_5_1_38707.Parsers
{
    public static class ChannelHandler
    {
        [Parser(Opcode.CMSG_CHAT_JOIN_CHANNEL)]
        public static void HandleChannelJoin(Packet packet)
        {
            packet.ReadInt32("ChatChannelId");

            var channelLength = packet.ReadBits("ChannelLength", 7);
            var passwordLength = packet.ReadBits("PasswordLength", 7);

            packet.ResetBitReader();

            packet.ReadWoWString("ChannelName", channelLength);
            packet.ReadWoWString("Password", passwordLength);
        }

        [Parser(Opcode.SMSG_CHANNEL_NOTIFY_JOINED)]
        public static void HandleChannelNotifyJoined(Packet packet)
        {
            var channelLen = packet.ReadBits(7);
            uint channelWelcomeMsgLen = packet.ReadBits(11);

            packet.ReadUInt32E<ChannelFlag>("ChannelFlags");
            packet.ReadInt32("ChatChannelID");
            packet.ReadUInt64("InstanceID");
            packet.ReadPackedGuid128("ChannelGUID");
            packet.ReadWoWString("Channel", channelLen);
            packet.ReadWoWString("ChannelWelcomeMsg", channelWelcomeMsgLen);
        }
        [Parser(Opcode.SMSG_CHANNEL_NOTIFY_LEFT)]
        public static void HandleChannelNotifyLeft(Packet packet)
        {
            var bits20 = packet.ReadBits(7);
            packet.ReadBit("Suspended");
            packet.ReadInt32("ChatChannelID");
            packet.ReadWoWString("Channel", bits20);
        }
    }
}
