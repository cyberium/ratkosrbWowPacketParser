using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Store.Objects.UpdateFields;
using WowPacketParser.Store.Objects.UpdateFields.LegacyImplementation;

namespace WowPacketParser.Store.Objects
{
    public sealed class Player : Unit
    {
        public static uint PlayerGuidCounter = 0;

        public Race Race;

        public Class Class;

        public string Name;

        public bool FirstLogin;

        public int Level;

        public bool IsActivePlayer = false;

        public IPlayerData PlayerData;
        public IPlayerData PlayerDataOriginal;

        public Player() : base(false)
        {
            DbGuid = ++PlayerGuidCounter;
            PlayerData = new PlayerData(this);
            PlayerDataOriginal = new OriginalPlayerData(this);
        }

        public bool HasMainHandWeapon()
        {
            if (ClientVersion.IsUsingNewUpdateFieldSystem())
                return PlayerData.VisibleItems[(int)EquipmentSlotType.MainHand].ItemID != 0;

            // Optimization for old UF system.
            // Don't call PlayerData cause it creates a whole array.
            int offset = 2;
            if (ClientVersion.RemovedInVersion(ClientVersionBuild.V3_0_2_9056))
                offset = ClientVersion.AddedInVersion(ClientVersionBuild.V2_0_1_6180) ? 16 : 12;

            int field = Enums.Version.UpdateFields.GetUpdateField(PlayerField.PLAYER_VISIBLE_ITEM_1_0);
            if (field <= 0)
                field = Enums.Version.UpdateFields.GetUpdateField(PlayerField.PLAYER_VISIBLE_ITEM);
            if (field <= 0)
                field = Enums.Version.UpdateFields.GetUpdateField(PlayerField.PLAYER_VISIBLE_ITEM_1_ENTRYID);
            if (field <= 0)
                return true;

            UpdateField value;
            UpdateFields.TryGetValue(field + offset * (int)EquipmentSlotType.MainHand, out value);
            return value.UInt32Value != 0;
        }
    }
}
