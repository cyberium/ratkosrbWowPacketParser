using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.SQL;

namespace WowPacketParser.Store.Objects
{
    [DBTableName("gossip_menu")]
    public class GossipMenu : IDataModel
    {
        [DBFieldName("MenuID", true, DbType = (TargetedDbType.TRINITY))]
        [DBFieldName("entry", true, DbType = (TargetedDbType.WPP | TargetedDbType.VMANGOS | TargetedDbType.CMANGOS))]
        public uint? Entry;

        [DBFieldName("TextID", true, DbType = (TargetedDbType.TRINITY))]
        [DBFieldName("text_id", true, DbType = (TargetedDbType.WPP | TargetedDbType.VMANGOS | TargetedDbType.CMANGOS))]
        public uint? TextID;

        [DBFieldName("VerifiedBuild", DbType = (TargetedDbType.TRINITY))]
        [DBFieldName("sniff_build", DbType = (TargetedDbType.WPP))]
        public int SniffBuild = ClientVersion.BuildInt;

        public ObjectType ObjectType;

        public uint ObjectEntry;

        //public ICollection<GossipMenuOption> GossipOptions;
    }
    [DBTableName("creature_gossip")]
    public class CreatureGossip : IDataModel
    {
        [DBFieldName("entry", true)]
        public uint? CreatureId;

        [DBFieldName("gossip_menu_id", true)]
        public uint? GossipMenuId;

        [DBFieldName("is_default", true)]
        public bool? IsDefault;

        [DBFieldName("sniff_build")]
        public int SniffBuild = ClientVersion.BuildInt;
    }
}
