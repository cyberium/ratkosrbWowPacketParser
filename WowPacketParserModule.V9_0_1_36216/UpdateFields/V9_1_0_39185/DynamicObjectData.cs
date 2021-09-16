using WowPacketParser.Misc;
using WowPacketParser.Store.Objects.UpdateFields;

// This file is automatically generated, DO NOT EDIT

namespace WowPacketParserModule.V9_0_1_36216.UpdateFields.V9_1_0_39185
{
    public class DynamicObjectData : IDynamicObjectData
    {
        public WowGuid Caster { get; set; }
        public ISpellCastVisual SpellVisual { get; set; }
        public int SpellXSpellVisualID => ((SpellCastVisual)SpellVisual).SpellXSpellVisualID;
        public int SpellID { get; set; }
        public float Radius { get; set; }
        public uint CastTime { get; set; }
        public byte Type { get; set; }
    }
}

