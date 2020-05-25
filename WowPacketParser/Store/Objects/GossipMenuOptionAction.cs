using WowPacketParser.SQL;

namespace WowPacketParser.Store.Objects
{
    [DBTableName("gossip_menu_option_action")]
    public class GossipMenuOptionAction : IDataModel
    {
        [DBFieldName("menu_id", true)]
        public uint? MenuId;

        [DBFieldName("id", true)]
        public uint? OptionIndex;

        [DBFieldName("action_menu_id")]
        public uint? ActionMenuId;

        [DBFieldName("action_poi_id", false, true)]
        public object ActionPoiId;
    }
}
