using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.SQL;

namespace WowPacketParser.Store.Objects
{
    [DBTableName("sniff_data")]
    public sealed class SniffData : IDataModel
    {
        [DBFieldName("sniff_build")]
        public int Build = ClientVersion.BuildInt;

        [DBFieldName("sniff_id", true, true)]
        public string SniffId;

        [DBFieldName("object_type", true)]
        public StoreNameType ObjectType;

        [DBFieldName("id", true)]
        public int Id;

        [DBFieldName("data", true)]
        public string Data;
    }
}
