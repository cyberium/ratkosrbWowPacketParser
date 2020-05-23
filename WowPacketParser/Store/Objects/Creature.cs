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

        [DBFieldName("movement_type")]
        public uint? MovementType;

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

    [DBTableName("creature_movement")]
    public sealed class CreatureMovement : IDataModel
    {
        [DBFieldName("id", true, true)]
        public string GUID;

        [DBFieldName("point", true)]
        public uint Point;

        [DBFieldName("position_x")]
        public float? PositionX;

        [DBFieldName("position_y")]
        public float? PositionY;

        [DBFieldName("position_z")]
        public float? PositionZ;

        [DBFieldName("orientation")]
        public float? Orientation;

        public static float GetAngle(float x1, float y1, float x2, float y2)
        {
            float dx = x1 - x2;
            float dy = y1 - y2;

            double ang = System.Math.Atan2(dy, dx);
            ang = (ang >= 0) ? ang : 2 * System.Math.PI + ang;
            return (float)ang;
        }
    }
}
