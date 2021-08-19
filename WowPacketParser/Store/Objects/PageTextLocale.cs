using WowPacketParser.Misc;
using WowPacketParser.SQL;
using WowPacketParser.Enums;

namespace WowPacketParser.Store.Objects
{
    [DBTableName("page_text_locale")]
    public sealed class PageTextLocale : IDataModel
    {
        [DBFieldName("entry", true, DbType = (TargetedDbType.WPP | TargetedDbType.VMANGOS | TargetedDbType.CMANGOS))]
        [DBFieldName("ID", true, DbType = (TargetedDbType.TRINITY))]
        public uint? ID;

        [DBFieldName("locale", true)]
        public string Locale = ClientLocale.PacketLocaleString;

        [DBFieldName("text", DbType = (TargetedDbType.WPP | TargetedDbType.VMANGOS | TargetedDbType.CMANGOS))]
        [DBFieldName("Text", DbType = (TargetedDbType.TRINITY))]
        public string Text;

        [DBFieldName("sniff_build", DbType = (TargetedDbType.WPP))]
        [DBFieldName("VerifiedBuild", DbType = (TargetedDbType.TRINITY))]
        public int? VerifiedBuild = ClientVersion.BuildInt;
    }
}