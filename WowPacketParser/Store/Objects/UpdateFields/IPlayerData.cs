using WowPacketParser.Misc;

namespace WowPacketParser.Store.Objects.UpdateFields
{
    public interface IPlayerData
    {
        WowGuid WowAccount { get; }
        uint PlayerBytes1 { get; }
        uint PlayerBytes2 { get; }
        uint PlayerFlags { get; }
        byte PvPRank { get; }
        IVisibleItem[] VisibleItems { get; }
        uint GuildRankID { get; }
        ChrCustomizationChoice[] GetCustomizations();
        IPlayerData Clone();
    }
}
