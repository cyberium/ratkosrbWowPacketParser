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

        public uint UnixTime = 0;

        public WowGuid CasterGuid;

        public uint? SpellID;

        public uint? CastFlags;

        public uint? CastFlagsEx;

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
