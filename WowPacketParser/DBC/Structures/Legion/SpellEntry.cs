namespace WowPacketParser.DBC.Structures.Legion
{
    [DBFile("Spell")]

    public sealed class SpellEntry
    {
        public string Name;
        public string NameSubtext;
        public string Description;
        public string AuraDescription;
        public int MiscID;
        public int ID;
        public int DescriptionVariablesID;
    }
}
