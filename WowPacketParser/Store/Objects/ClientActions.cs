using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.SQL;

namespace WowPacketParser.Store.Objects
{
    [DBTableName("quest_client_accept")]
    public sealed class QuestClientAccept : IDataModel
    {
        [DBFieldName("unixtime", true)]
        public uint? UnixTime;

        [DBFieldName("object_guid", false, true)]
        public string ObjectGuid;

        [DBFieldName("object_id")]
        public uint? ObjectId;

        [DBFieldName("object_type")]
        public string ObjectType;

        [DBFieldName("quest_id")]
        public uint? QuestId;
    }

    [DBTableName("quest_client_complete")]
    public sealed class QuestClientComplete : IDataModel
    {
        [DBFieldName("unixtime", true)]
        public uint? UnixTime;

        [DBFieldName("object_guid", false, true)]
        public string ObjectGuid;

        [DBFieldName("object_id")]
        public uint? ObjectId;

        [DBFieldName("object_type")]
        public string ObjectType;

        [DBFieldName("quest_id")]
        public uint? QuestId;
    }

    [DBTableName("gameobject_client_use")]
    public sealed class GameObjectClientUse : IDataModel
    {
        [DBFieldName("guid", true, true)]
        public string GUID;

        [DBFieldName("unixtime", true)]
        public uint UnixTime;
    }

    [DBTableName("item_client_use")]
    public sealed class ItemClientUse : IDataModel
    {
        [DBFieldName("entry", true, true)]
        public uint Entry;

        [DBFieldName("unixtime", true)]
        public uint UnixTime;
    }
    [DBTableName("client_release_spirit")]
    public sealed class ClientReleaseSpirit : IDataModel
    {
        [DBFieldName("unixtime", true)]
        public uint UnixTime;
    }
    [DBTableName("client_reclaim_corpse")]
    public sealed class ClientReclaimCorpse : IDataModel
    {
        [DBFieldName("unixtime", true)]
        public uint UnixTime;
    }
}
