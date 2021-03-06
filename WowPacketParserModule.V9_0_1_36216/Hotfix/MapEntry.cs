using WowPacketParser.Enums;
using WowPacketParser.Hotfix;

namespace WowPacketParserModule.V9_0_1_36216.Hotfix
{
    [HotfixStructure(DB2Hash.Map, HasIndexInData = false)]
    public class MapEntry
    {
        public string Directory { get; set; }
        public string MapName { get; set; }
        [HotfixVersion(ClientVersionBuild.V9_1_0_39185, true)]
        public string InternalName { get; set; }
        public string MapDescription0 { get; set; }
        public string MapDescription1 { get; set; }
        public string PvpShortDescription { get; set; }
        public string PvpLongDescription { get; set; }
        [HotfixArray(2, true)]
        public float[] Corpse { get; set; }
        public byte MapType { get; set; }
        public sbyte InstanceType { get; set; }
        public byte ExpansionID { get; set; }
        public ushort AreaTableID { get; set; }
        public short LoadingScreenID { get; set; }
        public short TimeOfDayOverride { get; set; }
        public short ParentMapID { get; set; }
        public short CosmeticParentMapID { get; set; }
        public byte TimeOffset { get; set; }
        public float MinimapIconScale { get; set; }
        public short CorpseMapID { get; set; }
        public byte MaxPlayers { get; set; }
        public short WindSettingsID { get; set; }
        public int ZmpFileDataID { get; set; }
        public int WdtFileDataID { get; set; }
        [HotfixArray(2)]
        public int[] Flags { get; set; }
    }
}
