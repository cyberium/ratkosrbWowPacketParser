using System;
using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.SQL;

namespace WowPacketParser.Store.Objects
{
    [DBTableName("creature_text")]
    public sealed class CreatureText : IDataModel
    {
        [DBFieldName("guid", true, true)]
        public string Guid;

        [DBFieldName("creature_id", true)]
        public uint? Entry;

        [DBFieldName("group_id", true)]
        public uint GroupId;

        [DBFieldName("health_percent", false, false, true)]
        public float? HealthPercent;

        [DBFieldName("unixtimems", true)]
        public ulong UnixTimeMs;

        public WowGuid SenderGUID;
        public string Text;
    }

    [DBTableName("creature_text_template")]
    public sealed class CreatureTextTemplate : IDataModel
    {
        [DBFieldName("creature_id", true)]
        public uint? Entry;

        [DBFieldName("group_id", true)]
        public uint GroupId;

        [DBFieldName("text")]
        public string Text;

        [DBFieldName("chat_type")]
        public ChatMessageType? Type;

        [DBFieldName("language")]
        public Language? Language;

        [DBFieldName("emote")]
        public EmoteType? Emote;

        [DBFieldName("sound")]
        public uint? Sound;

        [DBFieldName("broadcast_text_id", false, true)]
        public object BroadcastTextID = 0;

        [DBFieldName("comment")]
        public string Comment;

        public WowGuid SenderGUID;
        public string SenderName;
        public WowGuid ReceiverGUID;
        public string ReceiverName;
        public DateTime Time;

        public string BroadcastTextIDHelper;
    }

    [DBTableName("world_text")]
    public sealed class WorldText : IDataModel
    {
        [DBFieldName("unixtimems", true)]
        public ulong UnixTimeMs;

        [DBFieldName("text")]
        public string Text;

        [DBFieldName("chat_type")]
        public ChatMessageType? Type;

        [DBFieldName("language")]
        public Language? Language;
    }
}
