using WowPacketParser.Enums;
using WowPacketParser.Hotfix;

namespace WowPacketParserModule.V9_0_1_36216.Hotfix
{
    [HotfixStructure(DB2Hash.ItemSearchName, ClientVersionBuild.V9_0_1_36216, ClientVersionBuild.V9_1_0_39185)]
    public class ItemSearchNameEntry
    {
        public long AllowableRace { get; set; }
        public string Display { get; set; }
        public uint ID { get; set; }
        public byte OverallQualityID { get; set; }
        public byte ExpansionID { get; set; }
        public ushort MinFactionID { get; set; }
        public byte MinReputation { get; set; }
        public int AllowableClass { get; set; }
        public sbyte RequiredLevel { get; set; }
        public ushort RequiredSkill { get; set; }
        public ushort RequiredSkillRank { get; set; }
        public uint RequiredAbility { get; set; }
        public ushort ItemLevel { get; set; }
        [HotfixArray(4)]
        public int[] Flags { get; set; }
    }
}
