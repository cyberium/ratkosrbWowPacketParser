using WowPacketParser.Enums;
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
    }
}
