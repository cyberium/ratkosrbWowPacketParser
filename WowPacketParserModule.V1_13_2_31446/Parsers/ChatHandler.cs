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
            var text = new CreatureTextTemplate
            {
                Type = (ChatMessageType)packet.ReadByteE<ChatMessageTypeNew>("SlashCmd"),
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
            packet.ReadWoWString("Channel Name", channelLen);

            text.Text = packet.ReadWoWString("Text", textLen);
            if (unk801bit)
                packet.ReadUInt32("Unk801");

            uint entry = 0;
            if (text.SenderGUID.GetObjectType() == ObjectType.Unit)
                entry = text.SenderGUID.GetEntry();
            else if (text.ReceiverGUID.GetObjectType() == ObjectType.Unit)
                entry = text.ReceiverGUID.GetEntry();

            if (entry != 0)
            {
                text.Time = packet.Time;
                Storage.CreatureTextTemplates.Add(entry, text, packet.TimeSpan);
                CreatureText textEntry = new CreatureText();
                textEntry.Entry = entry;
                textEntry.Text = text.Text;
                textEntry.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(packet.Time);
                textEntry.SenderGUID = text.SenderGUID;
                if (Storage.Objects.ContainsKey(text.SenderGUID))
                {
                    var obj = Storage.Objects[text.SenderGUID].Item1 as Unit;
                    textEntry.HealthPercent = obj.UnitData.HealthPercent;
                }
                Storage.CreatureTexts.Add(textEntry);
            }
            else if (text.SenderGUID.IsEmpty() && (text.ReceiverGUID == null || text.ReceiverGUID.IsEmpty()) &&
                    (text.Type == ChatMessageType.BattlegroundNeutral))
            {
                var worldText = new WorldText
                {
                    UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(packet.Time),
                    Type = text.Type,
                    Language = text.Language,
                    Text = text.Text
                };
                Storage.WorldTexts.Add(worldText);
            }
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
