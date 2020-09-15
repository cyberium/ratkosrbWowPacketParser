using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.SQL;

namespace WowPacketParser.Store.Objects
{
    [DBTableName("gameobject")]
    public sealed class GameObjectModel : IDataModel
    {
        [DBFieldName("guid", true, true)]
        public string GUID;

        [DBFieldName("id")]
        public uint? ID;

        [DBFieldName("map", false, false, true)]
        public uint? Map;

        [DBFieldName("zone_id")]
        public uint? ZoneID;

        [DBFieldName("area_id")]
        public uint? AreaID;

        [DBFieldName("spawn_mask", TargetedDatabase.Zero, TargetedDatabase.Legion)]
        public uint? SpawnMask;

        [DBFieldName("spawn_difficulties", TargetedDatabase.Legion)]
        public string spawnDifficulties;

        [DBFieldName("phase_mask", TargetedDatabase.Zero, TargetedDatabase.Cataclysm)]
        public uint? PhaseMask;

        [DBFieldName("phase_id", TargetedDatabase.Cataclysm)]
        public string PhaseID;

        [DBFieldName("phase_group")]
        public uint? PhaseGroup;

        [DBFieldName("position_x")]
        public float? PositionX;

        [DBFieldName("position_y")]
        public float? PositionY;

        [DBFieldName("position_z")]
        public float? PositionZ;

        [DBFieldName("orientation")]
        public float? Orientation;

        [DBFieldName("rotation", 4, true)]
        public float?[] Rotation;

        [DBFieldName("temp")]
        public byte? TemporarySpawn;

        [DBFieldName("creator")]
        public uint? CreatedBy;

        [DBFieldName("animprogress")]
        public uint? AnimProgress;

        [DBFieldName("state")]
        public uint? State;

        [DBFieldName("flags")]
        public uint? Flags;

        [DBFieldName("SniffId", false, false, false, true)]
        public int? SniffId;

        [DBFieldName("VerifiedBuild")]
        public int? VerifiedBuild = ClientVersion.BuildInt;
    }

    [DBTableName("gameobject_create1_time")]
    public sealed class GameObjectCreate1 : IDataModel
    {
        [DBFieldName("guid", true, true)]
        public string GUID;

        [DBFieldName("position_x")]
        public float? PositionX;

        [DBFieldName("position_y")]
        public float? PositionY;

        [DBFieldName("position_z")]
        public float? PositionZ;

        [DBFieldName("orientation")]
        public float? Orientation;

        [DBFieldName("unixtime", true)]
        public uint UnixTime;
    }

    [DBTableName("gameobject_create2_time")]
    public sealed class GameObjectCreate2 : IDataModel
    {
        [DBFieldName("guid", true, true)]
        public string GUID;

        [DBFieldName("position_x")]
        public float? PositionX;

        [DBFieldName("position_y")]
        public float? PositionY;

        [DBFieldName("position_z")]
        public float? PositionZ;

        [DBFieldName("orientation")]
        public float? Orientation;

        [DBFieldName("unixtime", true)]
        public uint UnixTime;
    }

    [DBTableName("gameobject_destroy_time")]
    public sealed class GameObjectDestroy : IDataModel
    {
        [DBFieldName("guid", true, true)]
        public string GUID;

        [DBFieldName("unixtime", true)]
        public uint UnixTime;
    }

    [DBTableName("gameobject_update")]
    public sealed class GameObjectUpdate : IDataModel
    {
        [DBFieldName("guid", true, true)]
        public string GUID;

        [DBFieldName("unixtime", true)]
        public uint UnixTime;

        [DBFieldName("flags", false, false, true)]
        public uint? Flags;

        [DBFieldName("state", false, false, true)]
        public uint? State;

        [DBFieldName("animprogress", false, false, true)]
        public uint? AnimProgress;
    }

    [DBTableName("gameobject_custom_anim")]
    public sealed class GameObjectCustomAnim : IDataModel
    {
        [DBFieldName("guid", true, true)]
        public string GUID;

        [DBFieldName("anim_id")]
        public int AnimId;

        [DBFieldName("as_despawn", false, false, true)]
        public bool? AsDespawn;

        [DBFieldName("unixtime", true)]
        public uint UnixTime;
    }

    [DBTableName("gameobject_despawn_anim")]
    public sealed class GameObjectDespawnAnim : IDataModel
    {
        [DBFieldName("guid", true, true)]
        public string GUID;

        [DBFieldName("unixtime", true)]
        public uint UnixTime;
    }
}
