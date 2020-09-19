using System;
using System.Collections.Generic;
using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.SQL;

namespace WowPacketParser.Store.Objects
{
    [DBTableName("play_spell_visual_kit")]
    public sealed class PlaySpellVisualKit : IDataModel
    {
        [DBFieldName("caster_guid", true, true)]
        public string CasterGuid;

        [DBFieldName("caster_id")]
        public uint CasterId;

        [DBFieldName("caster_type", true)]
        public string CasterType;

        [DBFieldName("kit_id", true)]
        public uint KitId;

        [DBFieldName("kit_type", false, false, true)]
        public uint? KitType;

        [DBFieldName("duration", false, false, true)]
        public uint? Duration;

        [DBFieldName("unixtime", true)]
        public uint UnixTime;

        public WowGuid Guid;
        public DateTime Time;
    }

    [DBTableName("spell_cast_failed")]
    public sealed class SpellCastFailed : IDataModel
    {
        [DBFieldName("caster_guid", true, true)]
        public string CasterGuid;

        [DBFieldName("caster_id")]
        public uint CasterId;

        [DBFieldName("caster_type", true)]
        public string CasterType;

        [DBFieldName("spell_id", true)]
        public uint SpellId;

        [DBFieldName("visual_id", false, false, true)]
        public uint? VisualId;

        [DBFieldName("reason", false, false, true)]
        public uint? Reason;

        [DBFieldName("unixtime", true)]
        public uint UnixTime;

        public WowGuid Guid;
        public DateTime Time;
    }

    [DBTableName("spell_cast_start")]
    public sealed class SpellCastStart : IDataModel
    {
        [DBFieldName("unixtime", true)]
        public uint UnixTime;

        [DBFieldName("caster_guid", true, true)]
        public string CasterGuid;

        [DBFieldName("caster_id")]
        public uint CasterId;

        [DBFieldName("caster_type", true)]
        public string CasterType;

        [DBFieldName("spell_id", true)]
        public uint SpellId;

        [DBFieldName("cast_flags")]
        public uint CastFlags;

        [DBFieldName("cast_flags_ex")]
        public uint CastFlagsEx;

        [DBFieldName("target_guid", false, true)]
        public string TargetGuid;

        [DBFieldName("target_id")]
        public uint TargetId;

        [DBFieldName("target_type")]
        public string TargetType;
    }

    [DBTableName("spell_cast_go_target")]
    public sealed class SpellCastGoTarget : IDataModel
    {
        [DBFieldName("list_id", true)]
        public uint ListId;

        [DBFieldName("target_guid", true, true)]
        public string TargetGuid;

        [DBFieldName("target_id")]
        public uint TargetId;

        [DBFieldName("target_type", true)]
        public string TargetType;
    }

    [DBTableName("spell_cast_go")]
    public sealed class SpellCastGo : IDataModel
    {
        [DBFieldName("unixtime", true)]
        public uint UnixTime;

        [DBFieldName("caster_guid", true, true)]
        public string CasterGuid;

        [DBFieldName("caster_id")]
        public uint CasterId;

        [DBFieldName("caster_type", true)]
        public string CasterType;

        [DBFieldName("spell_id", true)]
        public uint SpellId;

        [DBFieldName("cast_flags")]
        public uint CastFlags;

        [DBFieldName("cast_flags_ex")]
        public uint CastFlagsEx;

        [DBFieldName("main_target_guid", false, true)]
        public string MainTargetGuid;

        [DBFieldName("main_target_id")]
        public uint MainTargetId;

        [DBFieldName("main_target_type")]
        public string MainTargetType;

        [DBFieldName("hit_targets_count")]
        public uint HitTargetsCount;

        [DBFieldName("hit_targets_list_id")]
        public uint HitTargetsListId;

        [DBFieldName("miss_targets_count")]
        public uint MissTargetsCount;

        [DBFieldName("miss_targets_list_id")]
        public uint MissTargetsListId;
    }

    public sealed class SpellCastData : IDataModel
    {
        public uint UnixTime = 0;

        public WowGuid CasterGuid;

        public uint SpellID;

        public uint CastFlags;

        public uint CastFlagsEx;

        public WowGuid MainTargetGuid;

        public uint HitTargetsCount = 0;
        public List<WowGuid> HitTargetsList;
        public void AddHitTarget(WowGuid guid)
        {
            if (HitTargetsList == null)
                HitTargetsList = new List<WowGuid>();

            HitTargetsList.Add(guid);
        }
        public uint MissTargetsCount = 0;
        public List<WowGuid> MissTargetsList;
        public void AddMissTarget(WowGuid guid)
        {
            if (MissTargetsList == null)
                MissTargetsList = new List<WowGuid>();

            MissTargetsList.Add(guid);
        }
    }

    public sealed class SpellPetCooldown : IDataModel
    {
        [DBFieldName("creature_id", true)]
        public uint? CasterID;

        [DBFieldName("flags")]
        public byte? Flags;

        [DBFieldName("index", true)]
        public byte? Index;

        [DBFieldName("spell_id", true)]
        public uint? SpellID;

        [DBFieldName("cooldown", true)]
        public uint? Cooldown;

        [DBFieldName("mod_rate")]
        public float? ModRate;
    }

    public sealed class SpellPetActions : IDataModel
    {
        [DBFieldName("creature_id", true)]
        public uint? CasterID;

        [DBFieldName("slot", 10)]
        public uint[] SpellID = new uint[10];
    }
}
