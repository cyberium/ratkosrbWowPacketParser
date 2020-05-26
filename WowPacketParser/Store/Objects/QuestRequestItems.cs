using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.SQL;

namespace WowPacketParser.Store.Objects
{
    [DBTableName("quest_request_items")]
    public sealed class QuestRequestItems : IDataModel
    {
        [DBFieldName("ID", true)]
        public uint? ID;

        [DBFieldName("EmoteOnComplete")]
        public int EmoteOnComplete = -1;

        [DBFieldName("EmoteOnIncomplete")]
        public int EmoteOnIncomplete = -1;

        [DBFieldName("EmoteOnCompleteDelay", TargetedDatabase.WarlordsOfDraenor)]
        public uint? EmoteOnCompleteDelay;

        [DBFieldName("EmoteOnIncompleteDelay", TargetedDatabase.WarlordsOfDraenor)]
        public uint? EmoteOnIncompleteDelay;

        [DBFieldName("CompletionText")]
        public string CompletionText;

        [DBFieldName("VerifiedBuild")]
        public int? VerifiedBuild = ClientVersion.BuildInt;
    }
}
