using WowPacketParser.Enums;
using WowPacketParser.SQL;

namespace WowPacketParser.Store.Objects
{
    [DBTableName("object_names")]
    public sealed class ObjectName : IDataModel
    {
        [DBFieldName("object_type", true)]
        public StoreNameType? ObjectType;

        [DBFieldName("id", true)]
        public int? ID;

        [DBFieldName("name")]
        public string Name;
    }
}
