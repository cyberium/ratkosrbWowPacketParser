using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.SQL;

namespace WowPacketParser.Store.Objects
{
    [DBTableName("gossip_menu_option")]
    public class GossipMenuOption : IDataModel
    {
        [DBFieldName("menu_id", true)]
        public uint? MenuId;

        [DBFieldName("id", true)]
        public uint? OptionIndex;

        [DBFieldName("option_icon")]
        public GossipOptionIcon? OptionIcon;

        [DBFieldName("option_text")]
        public string OptionText;

        [DBFieldName("option_broadcast_text")]
        public int? OptionBroadcastTextId;

        [DBFieldName("VerifiedBuild")]
        public int? VerifiedBuild = ClientVersion.BuildInt;

        public string BroadcastTextIDHelper;
    }
}
