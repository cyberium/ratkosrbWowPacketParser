using System;
using System.Collections.Generic;
using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.SQL;

namespace WowPacketParser.Store.Objects
{
    public sealed class ChatPacketData
    {
        public string Text;
        public uint TypeOriginal;
        public ChatMessageType? TypeNormalized;
        public Language? Language;
        public WowGuid SenderGUID;
        public string SenderName;
        public WowGuid ReceiverGUID;
        public string ReceiverName;
        public string ChannelName;
        public DateTime Time;
    }

    [DBTableName("player_chat")]
    public sealed class CharacterChat : IDataModel
    {
        [DBFieldName("unixtimems", true)]
        public ulong UnixTimeMs;

        [DBFieldName("guid", true, true)]
        public string Guid;

        [DBFieldName("sender_name", true)]
        public string SenderName;

        [DBFieldName("text")]
        public string Text;

        [DBFieldName("chat_type")]
        public uint Type;

        [DBFieldName("channel_name")]
        public string ChannelName;

        public WowGuid SenderGUID;
    }

    [DBTableName("creature_text")]
    public sealed class CreatureText : IDataModel
    {
        [DBFieldName("unixtimems", true)]
        public ulong UnixTimeMs;

        [DBFieldName("guid", true, true)]
        public string Guid;

        [DBFieldName("entry", true)]
        public uint? Entry;

        [DBFieldName("idx", true)]
        public uint Idx;

        [DBFieldName("target_guid", false, true)]
        public string TargetGuid = "0";

        [DBFieldName("target_id")]
        public uint TargetId;

        [DBFieldName("target_type")]
        public string TargetType;

        public WowGuid SenderGUID;
        public WowGuid ReceiverGUID;
        public string Text;
    }

    [DBTableName("creature_text_template", TargetedDbType.WPP)]
    [DBTableName("creature_text", TargetedDbType.TRINITY)]
    public sealed class CreatureTextTemplate : ITableWithSniffIdList
    {
        [DBFieldName("entry", true, DbType = TargetedDbType.WPP)]
        [DBFieldName("CreatureID", true, DbType = TargetedDbType.TRINITY)]
        public uint? Entry;

        //[DBFieldName("idx", true, DbType = TargetedDbType.WPP)] using auto increment grouped by entry
        [DBFieldName("GroupID", true, DbType = TargetedDbType.TRINITY)]
        public uint GroupId;

        [DBFieldName("text", true, DbType = TargetedDbType.WPP)]
        [DBFieldName("Text", DbType = TargetedDbType.TRINITY)]
        public string Text;

        [DBFieldName("chat_type", DbType = TargetedDbType.WPP)]
        [DBFieldName("Type", DbType = TargetedDbType.TRINITY)]
        public uint Type;

        [DBFieldName("language", DbType = TargetedDbType.WPP)]
        [DBFieldName("Language", DbType = TargetedDbType.TRINITY)]
        public Language? Language;

        [DBFieldName("emote", DbType = TargetedDbType.WPP)]
        [DBFieldName("Emote", DbType = TargetedDbType.TRINITY)]
        public EmoteType? Emote;

        [DBFieldName("sound", DbType = TargetedDbType.WPP)]
        [DBFieldName("Sound", DbType = TargetedDbType.TRINITY)]
        public uint? Sound;

        [DBFieldName("broadcast_text_id", false, true, DbType = TargetedDbType.WPP)]
        [DBFieldName("BroadcastTextId", false, true, DbType = TargetedDbType.TRINITY)]
        public object BroadcastTextID = 0;

        [DBFieldName("health_percent", false, false, true)]
        public float? HealthPercent;

        [DBFieldName("comment")]
        public string Comment = String.Empty;

        public WowGuid SenderGUID;
        public string SenderName;
        public WowGuid ReceiverGUID;
        public string ReceiverName;
        public DateTime Time;
        public int SniffId;
        public HashSet<int> SniffIdList;

        public string BroadcastTextIDHelper;

        public HashSet<int> GetSniffIdList
        {
            get
            {
                return SniffIdList;
            }
        }

        public CreatureTextTemplate() { }
        public CreatureTextTemplate(ChatPacketData textTemplate, int sniffId)
        {
            Text = textTemplate.Text;
            Type = textTemplate.TypeOriginal;
            Language = textTemplate.Language;
            SenderGUID = textTemplate.SenderGUID;
            SenderName = textTemplate.SenderName;
            ReceiverGUID = textTemplate.ReceiverGUID;
            ReceiverName = textTemplate.ReceiverName;
            Time = textTemplate.Time;
            SniffId = sniffId;
        }
    }

    [DBTableName("world_text")]
    public sealed class WorldText : IDataModel
    {
        [DBFieldName("unixtimems", true)]
        public ulong UnixTimeMs;

        [DBFieldName("text")]
        public string Text;

        [DBFieldName("chat_type")]
        public uint Type;

        [DBFieldName("language")]
        public Language? Language;
    }
}
