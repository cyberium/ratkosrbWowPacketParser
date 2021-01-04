using WowPacketParser.SQL;
using WowPacketParser.Enums;

namespace WowPacketParser.Store.Objects
{
    [DBTableName("gossip_menu_option_action")]
    public class GossipMenuOptionAction : IDataModel
    {
        [DBFieldName("menu_id", true, DbType = (TargetedDbType.WPP | TargetedDbType.VMANGOS | TargetedDbType.CMANGOS))]
        [DBFieldName("MenuID", true, DbType = (TargetedDbType.TRINITY))]
        public uint? MenuId;

        [DBFieldName("id", true, DbType = (TargetedDbType.WPP | TargetedDbType.VMANGOS | TargetedDbType.CMANGOS))]
        [DBFieldName("OptionIndex", true, DbType = (TargetedDbType.TRINITY))]
        public uint? OptionIndex;

        [DBFieldName("action_menu_id", DbType = (TargetedDbType.WPP | TargetedDbType.VMANGOS | TargetedDbType.CMANGOS))]
        [DBFieldName("ActionMenuId", DbType = (TargetedDbType.TRINITY))]
        public uint? ActionMenuId;

        [DBFieldName("action_poi_id", false, true, DbType = (TargetedDbType.WPP | TargetedDbType.VMANGOS | TargetedDbType.CMANGOS))]
        [DBFieldName("ActionPoiId", false, true, DbType = (TargetedDbType.TRINITY))]
        public object ActionPoiId;
    }
}
