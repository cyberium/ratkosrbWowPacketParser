using WowPacketParser.Enums;
using WowPacketParser.SQL;

namespace WowPacketParser.Store.Objects
{
    public sealed class CreatureAddon : IDataModel
    {
        [DBFieldName("guid", true, noQuotes: true)]
        public string GUID;

        [DBFieldName("mount")]
        public uint? Mount;

        [DBFieldName("bytes1")]
        public uint? Bytes1;

        [DBFieldName("stand_state")]
        public uint? StandState;

        [DBFieldName("pet_talent_points")]
        public uint? PetTalentPoints;

        [DBFieldName("vis_flags")]
        public uint? VisFlags;

        [DBFieldName("anim_tier")]
        public uint? AnimTier;

        [DBFieldName("bytes2")]
        public uint? Bytes2;

        [DBFieldName("sheath_state")]
        public uint? SheatheState;

        [DBFieldName("pvp_flags")]
        public uint? PvpFlags;

        [DBFieldName("pet_flags")]
        public uint? PetFlags;

        [DBFieldName("shapeshift_form")]
        public uint? ShapeshiftForm;

        [DBFieldName("emote")]
        public uint? Emote;

        [DBFieldName("aiAnimKit", TargetedDatabase.Legion)]
        public ushort? AIAnimKit;

        [DBFieldName("movementAnimKit", TargetedDatabase.Legion)]
        public ushort? MovementAnimKit;

        [DBFieldName("meleeAnimKit", TargetedDatabase.Legion)]
        public ushort? MeleeAnimKit;

        [DBFieldName("auras")]
        public string Auras;
    }
}
