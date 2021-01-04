using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.SQL;

namespace WowPacketParser.Store.Objects
{
    [DBTableName("gossip_menu_option")]
    public class GossipMenuOption : IDataModel
    {
        [DBFieldName("menu_id", true, DbType = (TargetedDbType.WPP | TargetedDbType.VMANGOS | TargetedDbType.CMANGOS))]
        [DBFieldName("MenuID", true, DbType = (TargetedDbType.TRINITY))]
        public uint? MenuId;

        [DBFieldName("id", true, DbType = (TargetedDbType.WPP | TargetedDbType.VMANGOS | TargetedDbType.CMANGOS))]
        [DBFieldName("OptionID", true, DbType = (TargetedDbType.TRINITY))]
        public uint? OptionIndex;

        [DBFieldName("option_icon", DbType = (TargetedDbType.WPP | TargetedDbType.VMANGOS | TargetedDbType.CMANGOS))]
        [DBFieldName("OptionIcon", DbType = (TargetedDbType.TRINITY))]
        public GossipOptionIcon? OptionIcon;

        [DBFieldName("option_text", DbType = (TargetedDbType.WPP | TargetedDbType.VMANGOS | TargetedDbType.CMANGOS))]
        [DBFieldName("OptionText", DbType = (TargetedDbType.TRINITY))]
        public string OptionText;

        [DBFieldName("option_broadcast_text", DbType = (TargetedDbType.WPP | TargetedDbType.VMANGOS | TargetedDbType.CMANGOS))]
        [DBFieldName("OptionBroadcastTextID", DbType = (TargetedDbType.TRINITY))]
        public int? OptionBroadcastTextId = 0;

        [DBFieldName("option_id", DbType = (TargetedDbType.WPP | TargetedDbType.VMANGOS | TargetedDbType.CMANGOS))]
        [DBFieldName("OptionType", DbType = (TargetedDbType.TRINITY))]
        public uint OptionId = 0;

        [DBFieldName("npc_option_npcflag", DbType = (TargetedDbType.WPP | TargetedDbType.VMANGOS | TargetedDbType.CMANGOS))]
        [DBFieldName("OptionNpcFlag", DbType = (TargetedDbType.TRINITY))]
        public uint NpcOptionNpcFlag = 0;

        [DBFieldName("sniff_build", DbType = (TargetedDbType.WPP))]
        [DBFieldName("VerifiedBuild", DbType = (TargetedDbType.TRINITY))]
        public int? SniffBuild = ClientVersion.BuildInt;

        public string BroadcastTextIDHelper;
    }
}
