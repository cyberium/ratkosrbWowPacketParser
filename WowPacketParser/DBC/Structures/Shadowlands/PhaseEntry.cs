using WowPacketParser.DBC.Structures.BattleForAzeroth;

namespace WowPacketParser.DBC.Structures.Shadowlands
{
    [DBFile("Phase")]
    public sealed class PhaseEntry
    {
        public uint ID;
        public short Flags;
    }
}
