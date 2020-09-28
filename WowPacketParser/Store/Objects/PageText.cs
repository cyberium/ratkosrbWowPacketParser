using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.SQL;

namespace WowPacketParser.Store.Objects
{
    [DBTableName("page_text")]
    public sealed class PageText : IDataModel
    {
        [DBFieldName("entry", true)]
        public uint? ID;

        [DBFieldName("text")]
        public string Text;

        [DBFieldName("next_page")]
        public uint? NextPageID;

        [DBFieldName("PlayerConditionID", TargetedDatabase.Legion)]
        public int? PlayerConditionID;

        [DBFieldName("Flags", TargetedDatabase.Legion)]
        public byte? Flags;

        [DBFieldName("VerifiedBuild")]
        public int? VerifiedBuild = ClientVersion.BuildInt;
    }
}
