using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.SQL;

namespace WowPacketParser.Store.Objects
{
    [DBTableName("broadcast_text")]
    public sealed class BroadcastText : IDataModel
    {
        public ushort?[] EmoteID;
        public ushort?[] EmoteDelay;
        public uint?[] SoundEntriesID;

        public void ConvertToDBStruct()
        {
            EmoteID1 = EmoteID[0];
            EmoteID2 = EmoteID[1];
            EmoteID3 = EmoteID[2];

            EmoteDelay1 = EmoteDelay[0];
            EmoteDelay2 = EmoteDelay[1];
            EmoteDelay3 = EmoteDelay[2];

            SoundEntriesID1 = SoundEntriesID[0];
            SoundEntriesID2 = SoundEntriesID[1];
        }

        [DBFieldName("male_text", LocaleConstant.enUS, DbType = (TargetedDbType.WPP | TargetedDbType.VMANGOS))]
        [DBFieldName("Text", LocaleConstant.enUS, DbType = (TargetedDbType.TRINITY | TargetedDbType.CMANGOS))]
        public string Text;

        [DBFieldName("female_text", LocaleConstant.enUS, DbType = (TargetedDbType.WPP | TargetedDbType.VMANGOS))]
        [DBFieldName("Text1", LocaleConstant.enUS, DbType = (TargetedDbType.TRINITY | TargetedDbType.CMANGOS))]
        public string Text1;

        [DBFieldName("entry", true, DbType = (TargetedDbType.WPP | TargetedDbType.VMANGOS))]
        [DBFieldName("ID", true, DbType = (TargetedDbType.TRINITY | TargetedDbType.CMANGOS))]
        public uint? ID;

        [DBFieldName("language_id", DbType = (TargetedDbType.WPP | TargetedDbType.VMANGOS))]
        [DBFieldName("LanguageID", DbType = (TargetedDbType.TRINITY | TargetedDbType.CMANGOS))]
        public int? LanguageID;

        [DBFieldName("condition_id", DbType = (TargetedDbType.WPP))]
        [DBFieldName("ConditionID", DbType = (TargetedDbType.TRINITY | TargetedDbType.CMANGOS))]
        public uint? ConditionID;

        [DBFieldName("emotes_id", DbType = (TargetedDbType.WPP))]
        [DBFieldName("EmotesID", DbType = (TargetedDbType.TRINITY | TargetedDbType.CMANGOS))]
        public ushort? EmotesID;

        [DBFieldName("flags", DbType = (TargetedDbType.WPP))]
        [DBFieldName("Flags", DbType = (TargetedDbType.TRINITY | TargetedDbType.CMANGOS))]
        public byte? Flags;

        [DBFieldName("chat_bubble_duration", TargetedDbExpansion.BattleForAzeroth, DbType = (TargetedDbType.WPP))]
        [DBFieldName("ChatBubbleDurationMs", TargetedDbExpansion.BattleForAzeroth, DbType = (TargetedDbType.TRINITY))]
        public uint? ChatBubbleDurationMs;

        [DBFieldName("sound_id1", DbType = (TargetedDbType.WPP))]
        [DBFieldName("sound_id", DbType = (TargetedDbType.VMANGOS))]
        [DBFieldName("SoundEntriesID1", DbType = (TargetedDbType.TRINITY | TargetedDbType.CMANGOS))]
        public uint? SoundEntriesID1;

        [DBFieldName("sound_id2", DbType = (TargetedDbType.WPP))]
        [DBFieldName("SoundEntriesID2", DbType = (TargetedDbType.TRINITY | TargetedDbType.CMANGOS))]
        public uint? SoundEntriesID2;

        [DBFieldName("emote_id1", DbType = (TargetedDbType.WPP | TargetedDbType.VMANGOS))]
        [DBFieldName("EmoteID1", DbType = (TargetedDbType.TRINITY | TargetedDbType.CMANGOS))]
        public ushort? EmoteID1;

        [DBFieldName("emote_id2", DbType = (TargetedDbType.WPP | TargetedDbType.VMANGOS))]
        [DBFieldName("EmoteID2", DbType = (TargetedDbType.TRINITY | TargetedDbType.CMANGOS))]
        public ushort? EmoteID2;

        [DBFieldName("emote_id3", DbType = (TargetedDbType.WPP | TargetedDbType.VMANGOS))]
        [DBFieldName("EmoteID3", DbType = (TargetedDbType.TRINITY | TargetedDbType.CMANGOS))]
        public ushort? EmoteID3;

        [DBFieldName("emote_delay1", DbType = (TargetedDbType.WPP | TargetedDbType.VMANGOS))]
        [DBFieldName("EmoteDelay1", DbType = (TargetedDbType.TRINITY | TargetedDbType.CMANGOS))]
        public ushort? EmoteDelay1;

        [DBFieldName("emote_delay2", DbType = (TargetedDbType.WPP | TargetedDbType.VMANGOS))]
        [DBFieldName("EmoteDelay2", DbType = (TargetedDbType.TRINITY | TargetedDbType.CMANGOS))]
        public ushort? EmoteDelay2;

        [DBFieldName("emote_delay3", DbType = (TargetedDbType.WPP | TargetedDbType.VMANGOS))]
        [DBFieldName("EmoteDelay3", DbType = (TargetedDbType.TRINITY | TargetedDbType.CMANGOS))]
        public ushort? EmoteDelay3;

        [DBFieldName("sniff_build", DbType = (TargetedDbType.WPP))]
        [DBFieldName("VerifiedBuild", DbType = (TargetedDbType.TRINITY | TargetedDbType.CMANGOS))]
        public int? VerifiedBuild = ClientVersion.BuildInt;
    }
}