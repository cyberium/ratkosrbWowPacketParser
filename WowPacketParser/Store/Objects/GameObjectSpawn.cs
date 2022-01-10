using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.SQL;

namespace WowPacketParser.Store.Objects
{
    [DBTableName("gameobject")]
    public sealed class GameObjectSpawn : IDataModel
    {
        [DBFieldName("guid", true, true)]
        public string GUID;

        [DBFieldName("original_id")]
        public uint? OriginalID;

        [DBFieldName("id")]
        public uint? ID;

        [DBFieldName("map", false, false, true)]
        public uint? Map;

        [DBFieldName("zone_id", DbType = (TargetedDbType.WPP))]
        [DBFieldName("zoneId", DbType = (TargetedDbType.TRINITY))]
        public uint? ZoneID;

        [DBFieldName("area_id", DbType = (TargetedDbType.WPP))]
        [DBFieldName("areaId", DbType = (TargetedDbType.TRINITY))]
        public uint? AreaID;

        [DBFieldName("spawn_mask", TargetedDbExpansion.WrathOfTheLichKing, TargetedDbExpansion.Legion, DbType = (TargetedDbType.WPP))]
        [DBFieldName("spawnMask", TargetedDbExpansion.WrathOfTheLichKing, TargetedDbExpansion.Legion, DbType = (TargetedDbType.TRINITY | TargetedDbType.CMANGOS))]
        public uint? SpawnMask;

        [DBFieldName("spawn_difficulties", TargetedDbExpansion.Legion, DbType = (TargetedDbType.WPP))]
        [DBFieldName("spawnDifficulties", TargetedDbExpansion.Legion, DbType = (TargetedDbType.TRINITY))]
        public string SpawnDifficulties;

        [DBFieldName("phase_mask", TargetedDbExpansion.WrathOfTheLichKing, TargetedDbExpansion.Cataclysm, DbType = (TargetedDbType.WPP))]
        [DBFieldName("phaseMask", TargetedDbExpansion.WrathOfTheLichKing, TargetedDbExpansion.Cataclysm, DbType = (TargetedDbType.TRINITY | TargetedDbType.CMANGOS))]
        public uint? PhaseMask;

        [DBFieldName("phase_id", TargetedDbExpansion.Cataclysm, DbType = (TargetedDbType.WPP))]
        [DBFieldName("PhaseId", TargetedDbExpansion.Cataclysm, DbType = (TargetedDbType.TRINITY))]
        public string PhaseID;

        [DBFieldName("phase_group", TargetedDbExpansion.Cataclysm, DbType = (TargetedDbType.WPP))]
        [DBFieldName("PhaseGroup", TargetedDbExpansion.Cataclysm, DbType = (TargetedDbType.TRINITY))]
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

        [DBFieldName("is_spawn", DbType = (TargetedDbType.WPP))]
        public ObjectCreateType? IsSpawn;

        [DBFieldName("is_temporary", DbType = (TargetedDbType.WPP))]
        public byte? TemporarySpawn;

        [DBFieldName("is_transport", DbType = (TargetedDbType.WPP))]
        public byte? IsTransport;

        [DBFieldName("creator_guid", false, true, DbType = (TargetedDbType.WPP))]
        public string CreatedByGuid;

        [DBFieldName("creator_id", DbType = (TargetedDbType.WPP))]
        public uint CreatedById;

        [DBFieldName("creator_type", DbType = (TargetedDbType.WPP))]
        public string CreatedByType;

        [DBFieldName("display_id", DbType = (TargetedDbType.WPP))]
        public uint? DisplayID;

        [DBFieldName("level", DbType = (TargetedDbType.WPP))]
        public uint? Level;

        [DBFieldName("faction", DbType = (TargetedDbType.WPP))]
        public uint? Faction;

        [DBFieldName("flags", DbType = (TargetedDbType.WPP))]
        public uint? Flags;

        [DBFieldName("dynamic_flags", DbType = (TargetedDbType.WPP))]
        public uint? DynamicFlags;

        [DBFieldName("path_progress", DbType = (TargetedDbType.WPP))]
        public uint? PathProgress;

        [DBFieldName("state")]
        public uint? State;

        [DBFieldName("type", DbType = (TargetedDbType.WPP))]
        public uint? Type;

        [DBFieldName("art_kit", DbType = (TargetedDbType.WPP))]
        public uint? ArtKit;

        [DBFieldName("anim_progress", DbType = (TargetedDbType.WPP))]
        [DBFieldName("animprogress", DbType = (TargetedDbType.VMANGOS | TargetedDbType.CMANGOS | TargetedDbType.TRINITY))]
        public uint? AnimProgress;

        [DBFieldName("custom_param", TargetedDbExpansion.BurningCrusadeClassic, TargetedDbExpansion.Zero, DbType = (TargetedDbType.WPP))]
        [DBFieldName("custom_param", TargetedDbExpansion.BattleForAzeroth, DbType = (TargetedDbType.WPP))]
        public uint? CustomParam;

        [DBFieldName("sniff_id", false, true, false, true, DbType = (TargetedDbType.WPP))]
        public string SniffId;

        [DBFieldName("sniff_build", DbType = (TargetedDbType.WPP))]
        [DBFieldName("VerifiedBuild", DbType = (TargetedDbType.TRINITY))]
        public int SniffBuild = ClientVersion.BuildInt;
    }

    [DBTableName("gameobject_create1_time")]
    public sealed class GameObjectCreate1 : IDataModel
    {
        [DBFieldName("unixtimems", true)]
        public ulong UnixTimeMs;

        [DBFieldName("guid", true, true)]
        public string GUID;

        [DBFieldName("map")]
        public uint? Map;

        [DBFieldName("position_x")]
        public float? PositionX;

        [DBFieldName("position_y")]
        public float? PositionY;

        [DBFieldName("position_z")]
        public float? PositionZ;

        [DBFieldName("orientation")]
        public float? Orientation;

        [DBFieldName("transport_guid", false, true, OnlyWhenSavingTransports = true)]
        public string TransportGuid = "0";

        [DBFieldName("transport_x", OnlyWhenSavingTransports = true)]
        public float TransportPositionX;

        [DBFieldName("transport_y", OnlyWhenSavingTransports = true)]
        public float TransportPositionY;

        [DBFieldName("transport_z", OnlyWhenSavingTransports = true)]
        public float TransportPositionZ;

        [DBFieldName("transport_o", OnlyWhenSavingTransports = true)]
        public float TransportOrientation;

        [DBFieldName("transport_path_timer", OnlyWhenSavingTransports = true)]
        public float TransportPathTimer;
    }

    [DBTableName("gameobject_create2_time")]
    public sealed class GameObjectCreate2 : IDataModel
    {
        [DBFieldName("unixtimems", true)]
        public ulong UnixTimeMs;

        [DBFieldName("guid", true, true)]
        public string GUID;

        [DBFieldName("map")]
        public uint? Map;

        [DBFieldName("position_x")]
        public float? PositionX;

        [DBFieldName("position_y")]
        public float? PositionY;

        [DBFieldName("position_z")]
        public float? PositionZ;

        [DBFieldName("orientation")]
        public float? Orientation;

        [DBFieldName("transport_guid", false, true, OnlyWhenSavingTransports = true)]
        public string TransportGuid = "0";

        [DBFieldName("transport_x", OnlyWhenSavingTransports = true)]
        public float TransportPositionX;

        [DBFieldName("transport_y", OnlyWhenSavingTransports = true)]
        public float TransportPositionY;

        [DBFieldName("transport_z", OnlyWhenSavingTransports = true)]
        public float TransportPositionZ;

        [DBFieldName("transport_o", OnlyWhenSavingTransports = true)]
        public float TransportOrientation;

        [DBFieldName("transport_path_timer", OnlyWhenSavingTransports = true)]
        public float TransportPathTimer;
    }

    [DBTableName("gameobject_destroy_time")]
    public sealed class GameObjectDestroy : IDataModel
    {
        [DBFieldName("unixtimems", true)]
        public ulong UnixTimeMs;

        [DBFieldName("guid", true, true)]
        public string GUID;
    }

    [DBTableName("gameobject_values_update")]
    public sealed class GameObjectUpdate : IDataModel
    {
        [DBFieldName("unixtimems", true)]
        public ulong UnixTimeMs;

        [DBFieldName("guid", true, true)]
        public string GUID;

        [DBFieldName("flags", true, false, true)]
        public uint? Flags;

        [DBFieldName("dynamic_flags", true, false, true)]
        public uint? DynamicFlags;

        [DBFieldName("path_progress", true, false, true)]
        public uint? PathProgress;

        [DBFieldName("state", true, false, true)]
        public uint? State;

        [DBFieldName("art_kit", true, false, true)]
        public uint? ArtKit;

        [DBFieldName("anim_progress", true, false, true)]
        public uint? AnimProgress;

        [DBFieldName("custom_param", true, false, true)]
        public uint? CustomParam;
    }

    [DBTableName("gameobject_custom_anim")]
    public sealed class GameObjectCustomAnim : IDataModel
    {
        [DBFieldName("unixtimems", true)]
        public ulong UnixTimeMs;

        [DBFieldName("guid", true, true)]
        public string GUID;

        [DBFieldName("anim_id")]
        public int AnimId;

        [DBFieldName("as_despawn", false, false, true)]
        public bool? AsDespawn;
    }

    [DBTableName("gameobject_despawn_anim")]
    public sealed class GameObjectDespawnAnim : IDataModel
    {
        [DBFieldName("unixtimems", true)]
        public ulong UnixTimeMs;

        [DBFieldName("guid", true, true)]
        public string GUID;
    }
}
