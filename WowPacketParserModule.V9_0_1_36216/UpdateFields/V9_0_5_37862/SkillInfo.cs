using WowPacketParser.Store.Objects.UpdateFields;

// This file is automatically generated, DO NOT EDIT

namespace WowPacketParserModule.V9_0_1_36216.UpdateFields.V9_0_5_37862
{
    public class SkillInfo : ISkillInfo
    {
        public ushort[] SkillLineID { get; } = new ushort[256];
        public ushort[] SkillStep { get; } = new ushort[256];
        public ushort[] SkillRank { get; } = new ushort[256];
        public ushort[] SkillStartingRank { get; } = new ushort[256];
        public ushort[] SkillMaxRank { get; } = new ushort[256];
        public short[] SkillTempBonus { get; } = new short[256];
        public ushort[] SkillPermBonus { get; } = new ushort[256];
    }
}

