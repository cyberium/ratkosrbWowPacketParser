using System;
using System.Collections.Generic;
using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.SQL;

namespace WowPacketParser.Store.Objects
{
    [DBTableName("creature")]
    public sealed class Creature : IDataModel
    {
        [DBFieldName("guid", true, true)]
        public string GUID;

        [DBFieldName("id")]
        public uint? ID;

        [DBFieldName("map")]
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
        public int? PhaseGroup;

        [DBFieldName("position_x")]
        public float? PositionX;

        [DBFieldName("position_y")]
        public float? PositionY;

        [DBFieldName("position_z")]
        public float? PositionZ;

        [DBFieldName("orientation")]
        public float? Orientation;

        [DBFieldName("wander_distance")]
        public float? WanderDistance;

        [DBFieldName("movement_type")]
        public uint? MovementType;

        [DBFieldName("temp")]
        public byte? TemporarySpawn;

        [DBFieldName("creator")]
        public uint? CreatedBy;

        [DBFieldName("summoner")]
        public uint? SummonedBy;

        [DBFieldName("summon_spell")]
        public uint? SummonSpell;

        [DBFieldName("display_id")]
        public uint? DisplayID;

        [DBFieldName("faction")]
        public uint? FactionTemplate;

        [DBFieldName("level")]
        public uint? Level;

        [DBFieldName("current_health")]
        public uint? CurHealth;

        [DBFieldName("max_health")]
        public uint? MaxHealth;

        [DBFieldName("current_mana")]
        public uint? CurMana;

        [DBFieldName("max_mana")]
        public uint? MaxMana;

        [DBFieldName("speed_walk")]
        public float? SpeedWalk;

        [DBFieldName("speed_run")]
        public float? SpeedRun;

        [DBFieldName("scale")]
        public float? Scale;

        [DBFieldName("base_attack_time")]
        public uint? BaseAttackTime;

        [DBFieldName("ranged_attack_time")]
        public uint? RangedAttackTime;

        [DBFieldName("npc_flags")]
        public uint? NpcFlag;

        [DBFieldName("unit_flags")]
        public uint? UnitFlag;

        [DBFieldName("dynamic_flags")]
        public uint? DynamicFlag;

        [DBFieldName("VerifiedBuild")]
        public int? VerifiedBuild = ClientVersion.BuildInt;
    }

    [DBTableName("creature_movement_spline")]
    public sealed class CreatureMovementSpline : IDataModel
    {
        [DBFieldName("guid", true, true)]
        public string GUID;

        [DBFieldName("parent_point", true)]
        public uint ParentPoint;

        [DBFieldName("spline_point", true)]
        public uint SplinePoint;

        [DBFieldName("global_point")]
        public uint GlobalPoint;

        [DBFieldName("position_x")]
        public float PositionX;

        [DBFieldName("position_y")]
        public float PositionY;

        [DBFieldName("position_z")]
        public float PositionZ;
    }

    [DBTableName("creature_movement")]
    public sealed class CreatureMovement : IDataModel
    {
        [DBFieldName("id", true, true)]
        public string GUID;

        [DBFieldName("point", true)]
        public uint Point;

        [DBFieldName("move_time")]
        public uint MoveTime;

        [DBFieldName("spline_flags")]
        public uint SplineFlags;

        [DBFieldName("spline_count")]
        public uint SplineCount;

        [DBFieldName("start_position_x")]
        public float StartPositionX;

        [DBFieldName("start_position_y")]
        public float StartPositionY;

        [DBFieldName("start_position_z")]
        public float StartPositionZ;  

        [DBFieldName("end_position_x")]
        public float EndPositionX;

        [DBFieldName("end_position_y")]
        public float EndPositionY;

        [DBFieldName("end_position_z")]
        public float EndPositionZ;

        [DBFieldName("orientation")]
        public float? Orientation;

        [DBFieldName("unixtime")]
        public uint? UnixTime;

        public List<Vector3> SplinePoints = null;
    }

    public sealed class CreatureEmote
    {
        public CreatureEmote(EmoteType emote_, DateTime time_)
        {
            emote = emote_;
            time = time_;
        }
        public EmoteType emote;
        public DateTime time;
    }
}
