using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.SQL;

namespace WowPacketParser.Store.Objects
{
    [DBTableName("creature_equip_template")]
    public sealed class CreatureEquipment : IDataModel
    {
        [DBFieldName("entry", true, DbType = (TargetedDbType.WPP | TargetedDbType.VMANGOS | TargetedDbType.CMANGOS))]
        [DBFieldName("CreatureID", true, DbType = (TargetedDbType.TRINITY))]
        public uint? CreatureID;

        [DBFieldName("id", true, DbType = (TargetedDbType.WPP))]
        [DBFieldName("ID", true, DbType = (TargetedDbType.TRINITY))]
        public uint? ID;

        [DBFieldName("item_id1", DbType = (TargetedDbType.WPP))]
        [DBFieldName("equipentry1", DbType = (TargetedDbType.VMANGOS | TargetedDbType.CMANGOS))]
        [DBFieldName("ItemID1", DbType = (TargetedDbType.TRINITY))]
        public uint? ItemID1;

        [DBFieldName("appearance_mod1", TargetedDbExpansion.Legion, DbType = (TargetedDbType.WPP))]
        [DBFieldName("AppearanceModID1", TargetedDbExpansion.Legion, DbType = (TargetedDbType.TRINITY))]
        public ushort? AppearanceModID1;

        [DBFieldName("item_visual1", TargetedDbExpansion.Legion, DbType = (TargetedDbType.WPP))]
        [DBFieldName("ItemVisual1", TargetedDbExpansion.Legion, DbType = (TargetedDbType.TRINITY))]
        public ushort? ItemVisual1;

        [DBFieldName("item_id2", DbType = (TargetedDbType.WPP))]
        [DBFieldName("equipentry2", DbType = (TargetedDbType.VMANGOS | TargetedDbType.CMANGOS))]
        [DBFieldName("ItemID2", DbType = (TargetedDbType.TRINITY))]
        public uint? ItemID2;

        [DBFieldName("appearance_mod2", TargetedDbExpansion.Legion, DbType = (TargetedDbType.WPP))]
        [DBFieldName("AppearanceModID2", TargetedDbExpansion.Legion, DbType = (TargetedDbType.TRINITY))]
        public ushort? AppearanceModID2;

        [DBFieldName("item_visual2", TargetedDbExpansion.Legion, DbType = (TargetedDbType.WPP))]
        [DBFieldName("ItemVisual2", TargetedDbExpansion.Legion, DbType = (TargetedDbType.TRINITY))]
        public ushort? ItemVisual2;

        [DBFieldName("item_id3", DbType = (TargetedDbType.WPP))]
        [DBFieldName("equipentry3", DbType = (TargetedDbType.VMANGOS | TargetedDbType.CMANGOS))]
        [DBFieldName("ItemID3", DbType = (TargetedDbType.TRINITY))]
        public uint? ItemID3;

        [DBFieldName("appearance_mod3", TargetedDbExpansion.Legion, DbType = (TargetedDbType.WPP))]
        [DBFieldName("AppearanceModID3", TargetedDbExpansion.Legion, DbType = (TargetedDbType.TRINITY))]
        public ushort? AppearanceModID3;

        [DBFieldName("item_visual3", TargetedDbExpansion.Legion, DbType = (TargetedDbType.WPP))]
        [DBFieldName("ItemVisual3", TargetedDbExpansion.Legion, DbType = (TargetedDbType.TRINITY))]
        public ushort? ItemVisual3;

        [DBFieldName("sniff_build", DbType = (TargetedDbType.WPP))]
        [DBFieldName("VerifiedBuild", DbType = (TargetedDbType.TRINITY))]
        public int? VerifiedBuild = ClientVersion.BuildInt;
    }
}
