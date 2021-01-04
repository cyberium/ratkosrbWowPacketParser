using WowPacketParser.Enums;
using WowPacketParser.SQL;

namespace WowPacketParser.Store.Objects
{
    [DBTableName("gossip_menu_option_box")]
    public class GossipMenuOptionBox : IDataModel
    {
        [DBFieldName("menu_id", true, DbType = (TargetedDbType.WPP | TargetedDbType.VMANGOS | TargetedDbType.CMANGOS))]
        [DBFieldName("MenuID", true, DbType = (TargetedDbType.TRINITY))]
        public uint? MenuId;

        [DBFieldName("id", true, DbType = (TargetedDbType.WPP | TargetedDbType.VMANGOS | TargetedDbType.CMANGOS))]
        [DBFieldName("OptionIndex", true, DbType = (TargetedDbType.TRINITY))]
        public uint? OptionIndex;

        [DBFieldName("box_coded", DbType = (TargetedDbType.WPP | TargetedDbType.VMANGOS | TargetedDbType.CMANGOS))]
        [DBFieldName("BoxCoded", DbType = (TargetedDbType.TRINITY))]
        public bool? BoxCoded;

        [DBFieldName("box_money", DbType = (TargetedDbType.WPP | TargetedDbType.VMANGOS | TargetedDbType.CMANGOS))]
        [DBFieldName("BoxMoney", DbType = (TargetedDbType.TRINITY))]
        public uint? BoxMoney;

        [DBFieldName("box_text", DbType = (TargetedDbType.WPP | TargetedDbType.VMANGOS | TargetedDbType.CMANGOS))]
        [DBFieldName("BoxText", DbType = (TargetedDbType.TRINITY))]
        public string BoxText;

        [DBFieldName("box_broadcast_text", DbType = (TargetedDbType.WPP | TargetedDbType.VMANGOS | TargetedDbType.CMANGOS))]
        [DBFieldName("BoxBroadcastTextId", DbType = (TargetedDbType.TRINITY))]
        public int? BoxBroadcastTextId = 0;

        public bool IsEmpty { get { return !BoxCoded.HasValue || !BoxCoded.Value; } }

        public string BroadcastTextIdHelper;
    }
}
