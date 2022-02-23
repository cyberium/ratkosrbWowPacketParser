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
        public GossipOptionType OptionId = 0;

        [DBFieldName("npc_option_npcflag", DbType = (TargetedDbType.WPP | TargetedDbType.VMANGOS | TargetedDbType.CMANGOS))]
        [DBFieldName("OptionNpcFlag", DbType = (TargetedDbType.TRINITY))]
        public uint NpcOptionNpcFlag = 0;

        [DBFieldName("Language", TargetedDbExpansion.Shadowlands, DbType = (TargetedDbType.TRINITY))]
        public Language? Language;

        [DBFieldName("sniff_build", DbType = (TargetedDbType.WPP))]
        [DBFieldName("VerifiedBuild", DbType = (TargetedDbType.TRINITY))]
        public int SniffBuild = ClientVersion.BuildInt;

        public string BroadcastTextIDHelper;

        public void FillOptionType()
        {
            var gossipOption = this;
            if (Settings.TargetedDbExpansion == TargetedDbExpansion.Zero ||
                Settings.TargetedDbExpansion == TargetedDbExpansion.Classic)
            {
                switch (gossipOption.OptionIcon)
                {
                    case GossipOptionIcon.Gossip:
                        gossipOption.OptionId = GossipOptionType.Gossip;
                        gossipOption.NpcOptionNpcFlag = 1;
                        break;
                    case GossipOptionIcon.Vendor:
                        gossipOption.OptionId = GossipOptionType.Vendor;
                        gossipOption.NpcOptionNpcFlag = 4;
                        break;
                    case GossipOptionIcon.Taxi:
                        gossipOption.OptionId = GossipOptionType.Taxivendor;
                        gossipOption.NpcOptionNpcFlag = 8;
                        break;
                    case GossipOptionIcon.Trainer:
                        gossipOption.OptionId = GossipOptionType.Trainer;
                        gossipOption.NpcOptionNpcFlag = 16;
                        break;
                    case GossipOptionIcon.SpiritHealer:
                        gossipOption.OptionId = GossipOptionType.Spirithealer;
                        gossipOption.NpcOptionNpcFlag = 32;
                        break;
                    case GossipOptionIcon.Innkeeper:
                        gossipOption.OptionId = GossipOptionType.Innkeeper;
                        gossipOption.NpcOptionNpcFlag = 128;
                        break;
                    case GossipOptionIcon.Banker:
                        gossipOption.OptionId = GossipOptionType.Banker;
                        gossipOption.NpcOptionNpcFlag = 256;
                        break;
                    case GossipOptionIcon.Petition:
                        gossipOption.OptionId = GossipOptionType.Petitioner;
                        gossipOption.NpcOptionNpcFlag = 512;
                        break;
                    case GossipOptionIcon.Tabard:
                        gossipOption.OptionId = GossipOptionType.TabardDesigner;
                        gossipOption.NpcOptionNpcFlag = 1024;
                        break;
                    case GossipOptionIcon.Battlemaster:
                        gossipOption.OptionId = GossipOptionType.Battlefield;
                        gossipOption.NpcOptionNpcFlag = 2048;
                        break;
                    case GossipOptionIcon.Auctioneer:
                        gossipOption.OptionId = GossipOptionType.Auctioneer;
                        gossipOption.NpcOptionNpcFlag = 4096;
                        break;
                }
            }
            else // tbc+
            {
                switch (gossipOption.OptionIcon)
                {
                    case GossipOptionIcon.Gossip:
                        gossipOption.OptionId = GossipOptionType.Gossip;
                        gossipOption.NpcOptionNpcFlag = 1;
                        break;
                    case GossipOptionIcon.Vendor:
                        gossipOption.OptionId = GossipOptionType.Vendor;
                        gossipOption.NpcOptionNpcFlag = 128;
                        break;
                    case GossipOptionIcon.Taxi:
                        gossipOption.OptionId = GossipOptionType.Taxivendor;
                        gossipOption.NpcOptionNpcFlag = 8192;
                        break;
                    case GossipOptionIcon.Trainer:
                        gossipOption.OptionId = GossipOptionType.Trainer;
                        gossipOption.NpcOptionNpcFlag = 16;
                        break;
                    case GossipOptionIcon.SpiritHealer:
                        gossipOption.OptionId = GossipOptionType.Spirithealer;
                        gossipOption.NpcOptionNpcFlag = 16384;
                        break;
                    case GossipOptionIcon.Innkeeper:
                        gossipOption.OptionId = GossipOptionType.Innkeeper;
                        gossipOption.NpcOptionNpcFlag = 65536;
                        break;
                    case GossipOptionIcon.Banker:
                        gossipOption.OptionId = GossipOptionType.Banker;
                        gossipOption.NpcOptionNpcFlag = 131072;
                        break;
                    case GossipOptionIcon.Petition:
                        gossipOption.OptionId = GossipOptionType.Petitioner;
                        gossipOption.NpcOptionNpcFlag = 262144;
                        break;
                    case GossipOptionIcon.Tabard:
                        gossipOption.OptionId = GossipOptionType.TabardDesigner;
                        gossipOption.NpcOptionNpcFlag = 524288;
                        break;
                    case GossipOptionIcon.Battlemaster:
                        gossipOption.OptionId = GossipOptionType.Battlefield;
                        gossipOption.NpcOptionNpcFlag = 1048576;
                        break;
                    case GossipOptionIcon.Auctioneer:
                        gossipOption.OptionId = GossipOptionType.Auctioneer;
                        gossipOption.NpcOptionNpcFlag = 2097152;
                        break;
                }
            }
        }
    }
}
