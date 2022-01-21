using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.SQL;

namespace WowPacketParser.Store.Objects
{
    [DBTableName("mail_template")]
    public sealed class MailTemplate : IDataModel
    {
        [DBFieldName("entry", true)]
        public uint Entry;

        [DBFieldName("sniff_build", true)]
        public int SniffBuild = ClientVersion.BuildInt;

        [DBFieldName("stationery_id")]
        public uint StationeryId;

        [DBFieldName("sender_id")]
        public uint SenderId;

        [DBFieldName("sender_type")]
        public byte SenderType;

        [DBFieldName("money")]
        public ulong Money;

        [DBFieldName("items_count")]
        public uint ItemsCount;

        [DBFieldName("subject", LocaleConstant.enUS)]
        public string Subject;

        [DBFieldName("body", LocaleConstant.enUS)]
        public string Body;
    }

    [DBTableName("mail_template_item")]
    public sealed class MailTemplateItem : IDataModel
    {
        [DBFieldName("entry", true)]
        public uint Entry;

        [DBFieldName("slot", true)]
        public byte Slot;

        [DBFieldName("item_id", true)]
        public uint ItemId;

        [DBFieldName("count", true)]
        public byte Count;

        [DBFieldName("sniff_build")]
        public int SniffBuild = ClientVersion.BuildInt;
    }
}
