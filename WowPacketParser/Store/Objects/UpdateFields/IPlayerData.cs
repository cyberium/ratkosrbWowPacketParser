using WowPacketParser.Misc;

namespace WowPacketParser.Store.Objects.UpdateFields
{
    public interface IPlayerData
    {
        WowGuid WowAccount { get; }
        uint Experience { get; }
        uint Money { get; }
        uint PlayerBytes1 { get; }
        uint PlayerBytes2 { get; }
        uint PlayerFlags { get; }
        uint PvPRank { get; }
        IVisibleItem[] VisibleItems { get; }
        uint GuildRankID { get; }
        IPlayerData Clone();
    }
}
