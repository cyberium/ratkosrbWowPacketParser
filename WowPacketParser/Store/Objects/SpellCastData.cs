using System;
using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.SQL;

namespace WowPacketParser.Store.Objects
{
    [DBTableName("spell_cast_start")]
    public sealed class SpellCastStart : IDataModel
    {
        [DBFieldName("UnixTime", true)]
        public uint UnixTime;

        [DBFieldName("CasterGuid", true, true)]
        public string CasterGuid;

        [DBFieldName("CasterId", true)]
        public uint CasterId;

        [DBFieldName("CasterType", true)]
        public string CasterType;

        [DBFieldName("SpellId", true)]
        public uint SpellId;

        [DBFieldName("CastFlags")]
        public uint CastFlags;

        [DBFieldName("CastFlagsEx")]
        public uint CastFlagsEx;

        [DBFieldName("TargetGuid", false, true)]
        public string TargetGuid;

        [DBFieldName("TargetId")]
        public uint TargetId;

        [DBFieldName("TargetType")]
        public string TargetType;
    }

    [DBTableName("spell_cast_go")]
    public sealed class SpellCastGo : IDataModel
    {
        [DBFieldName("UnixTime", true)]
        public uint UnixTime;

        [DBFieldName("CasterGuid", true, true)]
        public string CasterGuid;

        [DBFieldName("CasterId", true)]
        public uint CasterId;

        [DBFieldName("CasterType", true)]
        public string CasterType;

        [DBFieldName("SpellId", true)]
        public uint SpellId;

        [DBFieldName("CastFlags")]
        public uint CastFlags;

        [DBFieldName("CastFlagsEx")]
        public uint CastFlagsEx;

        [DBFieldName("MainTargetGuid", false, true)]
        public string MainTargetGuid;

        [DBFieldName("MainTargetId")]
        public uint MainTargetId;

        [DBFieldName("MainTargetType")]
        public string MainTargetType;

        [DBFieldName("HitTargetsCount")]
        public uint HitTargetsCount;

        [DBFieldName("HitTargetId", 8)]
        public uint[] HitTargetId = { 0, 0, 0, 0, 0, 0, 0, 0 };

        [DBFieldName("HitTargetType", 8)]
        public string[] HitTargetType = { "", "", "", "", "", "", "", "" };
    }

    public sealed class SpellCastData : IDataModel
    {
        public const uint MAX_SPELL_HIT_TARGETS_DB = 8;

        public uint UnixTime = 0;

        public WowGuid CasterGuid;

        public uint SpellID;

        public uint CastFlags;

        public uint CastFlagsEx;

        public WowGuid MainTargetGuid;

        public uint HitTargetsCount = 0;
        public uint[] HitTargetID = { 0, 0, 0, 0, 0, 0, 0, 0 };
        public string[] HitTargetType = { "", "", "", "", "", "", "", "" };

        [DBFieldName("VerifiedBuild")]
        public int? VerifiedBuild = ClientVersion.BuildInt;
    }

    public sealed class SpellPetCooldown : IDataModel
    {
        [DBFieldName("CreatureId", true)]
        public uint? CasterID;

        [DBFieldName("Flags")]
        public byte? Flags;

        [DBFieldName("Index", true)]
        public byte? Index;

        [DBFieldName("SpellId", true)]
        public uint? SpellID;

        [DBFieldName("Cooldown", true)]
        public uint? Cooldown;

        [DBFieldName("ModRate")]
        public float? ModRate;

        [DBFieldName("VerifiedBuild")]
        public int? VerifiedBuild = ClientVersion.BuildInt;
    }

    public sealed class SpellPetActions : IDataModel
    {
        [DBFieldName("CreatureId", true)]
        public uint? CasterID;

        [DBFieldName("SpellId", 10)]
        public uint[] SpellID = new uint[10];

        [DBFieldName("VerifiedBuild")]
        public int? VerifiedBuild = ClientVersion.BuildInt;
    }
}
