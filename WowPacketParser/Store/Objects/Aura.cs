using System;
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
    }
}
