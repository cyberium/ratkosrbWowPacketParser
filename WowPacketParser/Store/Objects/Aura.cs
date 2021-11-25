using System;
using WowPacketParser.Enums;
using WowPacketParser.Misc;

namespace WowPacketParser.Store.Objects
{
    public class Aura
    {
        public uint? Slot;

        public uint SpellId;

        public uint VisualId;

        public uint AuraFlags;

        public uint ActiveFlags;

        public uint Level;

        public uint Charges;

        public int ContentTuningId;

        public WowGuid CasterGuid;

        public int MaxDuration;

        public int Duration;

        public Aura Clone()
        {
            Aura aura = new Aura();
            aura.Slot = Slot;
            aura.SpellId = SpellId;
            aura.VisualId = VisualId;
            aura.AuraFlags = AuraFlags;
            aura.ActiveFlags = ActiveFlags;
            aura.Level = Level;
            aura.Charges = Charges;
            aura.ContentTuningId = ContentTuningId;
            aura.CasterGuid = CasterGuid;
            aura.MaxDuration = MaxDuration;
            aura.Duration = Duration;
            return aura;
        }

        public bool HasDuration()
        {
            if (ClientVersion.AddedInVersion(ClientType.MistsOfPandaria) ? AuraFlags.HasAnyFlag(AuraFlagMoP.Duration) : AuraFlags.HasAnyFlag(AuraFlag.Duration))
                return true;

            return (Duration > 0) || (MaxDuration > 0);
        }
    }
}
