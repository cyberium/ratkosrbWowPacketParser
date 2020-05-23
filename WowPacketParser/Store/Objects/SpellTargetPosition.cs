using WowPacketParser.Misc;
using WowPacketParser.SQL;

namespace WowPacketParser.Store.Objects
{
    public sealed class SpellTargetPosition : IDataModel
    {
        [DBFieldName("ID", true)]
        public uint? ID;

        [DBFieldName("EffectIndex", true)]
        public byte? EffectIndex;

        [DBFieldName("MapID")]
        public ushort? MapID;

        [DBFieldName("PositionX")]
        public float? PositionX;

        [DBFieldName("PositionY")]
        public float? PositionY;

        [DBFieldName("PositionZ")]
        public float? PositionZ;

        [DBFieldName("VerifiedBuild")]
        public int? VerifiedBuild = ClientVersion.BuildInt;

        public string EffectHelper;
    }

    public sealed class SpellCastData : IDataModel
    {
        public const uint MAX_SPELL_HIT_TARGETS_DB = 8;

        [DBFieldName("CasterId")]
        public uint? CasterID;

        [DBFieldName("CasterType")]
        public string CasterType;

        [DBFieldName("SpellId")]
        public uint? SpellID;

        [DBFieldName("CastFlags")]
        public uint? CastFlags;

        [DBFieldName("CastFlagsEx")]
        public uint? CastFlagsEx;

        [DBFieldName("MainTargetId")]
        public uint MainTargetID = 0;

        [DBFieldName("MainTargetType")]
        public string MainTargetType = "";

        public uint HitTargetsCount = 0;
        public uint[] HitTargetID = { 0, 0, 0, 0, 0, 0, 0, 0 };
        public string[] HitTargetType = { "", "", "", "", "", "", "", "" };

        [DBFieldName("verifiedBuild")]
        public int? VerifiedBuild = ClientVersion.BuildInt;
    }

    public sealed class SpellPetCooldown : IDataModel
    {
        [DBFieldName("CasterId", true)]
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
        [DBFieldName("CasterId", true)]
        public uint? CasterID;

        [DBFieldName("SpellID", 10)]
        public uint[] SpellID = new uint[10];

        [DBFieldName("VerifiedBuild")]
        public int? VerifiedBuild = ClientVersion.BuildInt;
    }
}
