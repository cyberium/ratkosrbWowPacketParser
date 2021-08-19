using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.SQL;

namespace WowPacketParser.Store.Objects
{
    [DBTableName("broadcast_text_locale")]
    public sealed class BroadcastTextLocale : IDataModel
    {
        [DBFieldName("entry", true, DbType = (TargetedDbType.WPP | TargetedDbType.VMANGOS))]
        [DBFieldName("ID", true, DbType = (TargetedDbType.TRINITY | TargetedDbType.CMANGOS))]
        public uint? ID;

        [DBFieldName("locale", true)]
        public string Locale = ClientLocale.PacketLocaleString;

        [DBFieldName("male_text", DbType = (TargetedDbType.WPP | TargetedDbType.VMANGOS))]
        [DBFieldName("Text_lang", DbType = (TargetedDbType.TRINITY | TargetedDbType.CMANGOS))]
        public string TextLang;

        [DBFieldName("female_text", DbType = (TargetedDbType.WPP | TargetedDbType.VMANGOS))]
        [DBFieldName("Text1_lang", DbType = (TargetedDbType.TRINITY | TargetedDbType.CMANGOS))]
        public string Text1Lang;

        [DBFieldName("sniff_build", DbType = (TargetedDbType.WPP | TargetedDbType.VMANGOS))]
        [DBFieldName("VerifiedBuild", DbType = (TargetedDbType.TRINITY | TargetedDbType.CMANGOS))]
        public int? VerifiedBuild = ClientVersion.BuildInt;
    }
}
