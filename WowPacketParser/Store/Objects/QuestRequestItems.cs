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

        [DBFieldName("Emote", false, false, true)]
        public uint? EmoteOnComplete;

        [DBFieldName("EmoteDelay", TargetedDbExpansion.WarlordsOfDraenor)]
        public uint? EmoteOnCompleteDelay;

        [DBFieldName("CompletionText")]
        public string CompletionText;

        [DBFieldName("VerifiedBuild")]
        public int? VerifiedBuild = ClientVersion.BuildInt;
    }
}
