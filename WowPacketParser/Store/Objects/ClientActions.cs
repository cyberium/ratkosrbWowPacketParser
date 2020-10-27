using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.SQL;

namespace WowPacketParser.Store.Objects
{
    [DBTableName("quest_client_accept")]
    public sealed class QuestClientAccept : IDataModel
    {
        [DBFieldName("unixtimems", true)]
        public ulong UnixTimeMs;

        [DBFieldName("object_guid", false, true)]
        public string ObjectGuid;

        [DBFieldName("object_id")]
        public uint ObjectId;

        [DBFieldName("object_type")]
        public string ObjectType;

        [DBFieldName("quest_id")]
        public uint QuestId;
    }

    [DBTableName("quest_client_complete")]
    public sealed class QuestClientComplete : IDataModel
    {
        [DBFieldName("unixtimems", true)]
        public ulong UnixTimeMs;

        [DBFieldName("object_guid", false, true)]
        public string ObjectGuid;

        [DBFieldName("object_id")]
        public uint ObjectId;

        [DBFieldName("object_type")]
        public string ObjectType;

        [DBFieldName("quest_id")]
        public uint QuestId;
    }

    [DBTableName("creature_client_interact")]
    public sealed class CreatureClientInteract : IDataModel
    {
        [DBFieldName("guid", true, true)]
        public string GUID;

        [DBFieldName("unixtimems", true)]
        public ulong UnixTimeMs;
    }

    [DBTableName("gameobject_client_use")]
    public sealed class GameObjectClientUse : IDataModel
    {
        [DBFieldName("guid", true, true)]
        public string GUID;

        [DBFieldName("unixtimems", true)]
        public ulong UnixTimeMs;
    }

    [DBTableName("item_client_use")]
    public sealed class ItemClientUse : IDataModel
    {
        [DBFieldName("entry", true, true)]
        public uint Entry;

        [DBFieldName("unixtimems", true)]
        public ulong UnixTimeMs;
    }
    [DBTableName("client_release_spirit")]
    public sealed class ClientReleaseSpirit : IDataModel
    {
        [DBFieldName("unixtimems", true)]
        public ulong UnixTimeMs;
    }
    [DBTableName("client_reclaim_corpse")]
    public sealed class ClientReclaimCorpse : IDataModel
    {
        [DBFieldName("unixtimems", true)]
        public ulong UnixTimeMs;
    }
}
