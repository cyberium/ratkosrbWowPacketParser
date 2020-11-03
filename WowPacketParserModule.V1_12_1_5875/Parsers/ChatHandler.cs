using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using WowPacketParser.Enums;
using WowPacketParser.Enums.Version;
using WowPacketParser.Misc;
using WowPacketParser.Store;
using WowPacketParser.Store.Objects;


namespace WowPacketParser.Parsing.Parsers
{
    public static class ChatHandler
    {
        public static ChatMessageType ConvertVanillaMessageType(ChatMessageTypeVanilla type)
        {
            switch (type)
            {
                case ChatMessageTypeVanilla.System:
                    return ChatMessageType.System;
                case ChatMessageTypeVanilla.Say:
                    return ChatMessageType.Say;
                case ChatMessageTypeVanilla.Party:
                    return ChatMessageType.Party;
                case ChatMessageTypeVanilla.Raid:
                    return ChatMessageType.Raid;
                case ChatMessageTypeVanilla.Guild:
                    return ChatMessageType.Guild;
                case ChatMessageTypeVanilla.Officer:
                    return ChatMessageType.Officer;
                case ChatMessageTypeVanilla.Yell:
                    return ChatMessageType.Yell;
                case ChatMessageTypeVanilla.Whisper:
                    return ChatMessageType.Whisper;
                case ChatMessageTypeVanilla.WhisperInform:
                    return ChatMessageType.WhisperInform;
                case ChatMessageTypeVanilla.Emote:
                    return ChatMessageType.Emote;
                case ChatMessageTypeVanilla.TextEmote:
                    return ChatMessageType.TextEmote;
                case ChatMessageTypeVanilla.MonsterSay:
                    return ChatMessageType.MonsterSay;
                case ChatMessageTypeVanilla.MonsterYell:
                    return ChatMessageType.MonsterYell;
                case ChatMessageTypeVanilla.MonsterEmote:
                    return ChatMessageType.MonsterEmote;
                case ChatMessageTypeVanilla.Channel:
                    return ChatMessageType.Channel;
                case ChatMessageTypeVanilla.ChannelJoin:
                    return ChatMessageType.ChannelJoin;
                case ChatMessageTypeVanilla.ChannelLeave:
                    return ChatMessageType.ChannelLeave;
                case ChatMessageTypeVanilla.ChannelList:
                    return ChatMessageType.ChannelList;
                case ChatMessageTypeVanilla.ChannelNotice:
                    return ChatMessageType.ChannelNotice;
                case ChatMessageTypeVanilla.ChannelNoticeUser:
                    return ChatMessageType.ChannelNoticeUser;
                case ChatMessageTypeVanilla.Afk:
                    return ChatMessageType.Afk;
                case ChatMessageTypeVanilla.Dnd:
                    return ChatMessageType.Dnd;
                case ChatMessageTypeVanilla.Ignored:
                    return ChatMessageType.Ignored;
                case ChatMessageTypeVanilla.Skill:
                    return ChatMessageType.Skill;
                case ChatMessageTypeVanilla.Loot:
                    return ChatMessageType.Loot;
                case ChatMessageTypeVanilla.MonsterWhisper:
                    return ChatMessageType.MonsterWhisper;
                case ChatMessageTypeVanilla.MonsterParty:
                    return ChatMessageType.MonsterParty;
                case ChatMessageTypeVanilla.BattlegroundNeutral:
                    return ChatMessageType.BattlegroundNeutral;
                case ChatMessageTypeVanilla.BattlegroundAlliance:
                    return ChatMessageType.BattlegroundAlliance;
                case ChatMessageTypeVanilla.BattlegroundHorde:
                    return ChatMessageType.BattlegroundHorde;
                case ChatMessageTypeVanilla.RaidLeader:
                    return ChatMessageType.RaidLeader;
                case ChatMessageTypeVanilla.RaidWarning:
                    return ChatMessageType.RaidWarning;
                case ChatMessageTypeVanilla.RaidBossEmote:
                    return ChatMessageType.RaidBossEmote;
                case ChatMessageTypeVanilla.RaidBossWhisper:
                    return ChatMessageType.RaidBossWhisper;
                case ChatMessageTypeVanilla.Battleground:
                    return ChatMessageType.Battleground;
                case ChatMessageTypeVanilla.BattlegroundLeader:
                    return ChatMessageType.BattlegroundLeader;

            }
            return ChatMessageType.System;
        }

        [Parser(Opcode.SMSG_CHAT)]
        [Parser(Opcode.SMSG_GM_MESSAGECHAT)]
        public static void HandleServerChatMessage(Packet packet)
        {
            var text = new CreatureTextTemplate
            {
                Type = ConvertVanillaMessageType(packet.ReadByteE<ChatMessageTypeVanilla>("Type")),
                Language = packet.ReadInt32E<Language>("Language"),
                SenderGUID = WowGuid.Empty
            };

            switch (text.Type)
            {
                case ChatMessageType.MonsterWhisper:
                //case CHAT_MSG_RAID_BOSS_WHISPER:
                case ChatMessageType.RaidBossEmote:
                case ChatMessageType.MonsterEmote:
                    packet.ReadUInt32("Sender Name Length");
                    text.SenderName = packet.ReadCString("Sender Name");
                    text.ReceiverGUID = packet.ReadGuid("Target Guid");
                    break;
                case ChatMessageType.Say:
                case ChatMessageType.Party:
                case ChatMessageType.Yell:
                    text.SenderGUID = packet.ReadGuid("Sender Guid");
                    packet.ReadGuid("Sender Guid");
                    break;
                case ChatMessageType.MonsterSay:
                case ChatMessageType.MonsterYell:
                    text.SenderGUID = packet.ReadGuid("Sender Guid");
                    packet.ReadUInt32("Sender Name Length");
                    text.SenderName = packet.ReadCString("Sender Name");
                    text.ReceiverGUID = packet.ReadGuid("Target Guid");
                    break;

                case ChatMessageType.Channel:
                    packet.ReadCString("Channel Name");
                    packet.ReadUInt32("Player Rank");
                    text.SenderGUID = packet.ReadGuid("Sender Guid");
                    break;

                default:
                    text.SenderGUID = packet.ReadGuid("Sender Guid");
                    break;
            }
            
            packet.ReadInt32("Text Length");
            text.Text = packet.ReadCString("Text");
            packet.ReadByteE<ChatTag>("Chat Tag");

            uint entry = 0;
            if (text.SenderGUID.GetObjectType() == ObjectType.Unit)
                entry = text.SenderGUID.GetEntry();
            else if (text.ReceiverGUID != null && text.ReceiverGUID.GetObjectType() == ObjectType.Unit)
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
    }
}
