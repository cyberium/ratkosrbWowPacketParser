using System;
using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.SQL;

namespace WowPacketParser.Store.Objects
{
    [DBTableName("creature_text")]
    public sealed class CreatureText : IDataModel
    {
        [DBFieldName("CreatureID", true)]
        public uint? Entry;

        [DBFieldName("GroupID", true)]
        public uint GroupId;

        [DBFieldName("HealthPercent")]
        public float HealthPercent;

        [DBFieldName("UnixTime", true)]
        public uint UnixTime;

        public string Text;
    }

    [DBTableName("creature_text_template")]
    public sealed class CreatureTextTemplate : IDataModel
    {
        [DBFieldName("CreatureID", true)]
        public uint? Entry;

        [DBFieldName("GroupID", true)]
        public uint GroupId;

        [DBFieldName("Text")]
        public string Text;

        [DBFieldName("Type")]
        public ChatMessageType? Type;

        [DBFieldName("Language", TargetedDatabase.Zero, TargetedDatabase.BattleForAzeroth)]
        public Language? Language;

        [DBFieldName("Language", TargetedDatabase.BattleForAzeroth)]
        public Language801? Language801;

        [DBFieldName("Emote")]
        public EmoteType? Emote;

        [DBFieldName("Sound")]
        public uint? Sound;

        [DBFieldName("BroadcastTextId", false, true)]
        public object BroadcastTextID = 0;

        [DBFieldName("Comment")]
        public string Comment;

        public WowGuid SenderGUID;
        public string SenderName;
        public WowGuid ReceiverGUID;
        public string ReceiverName;
        public DateTime Time;

        public string BroadcastTextIDHelper;
    }
}
