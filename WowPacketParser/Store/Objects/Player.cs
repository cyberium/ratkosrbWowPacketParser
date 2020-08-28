using WowPacketParser.Enums;
using WowPacketParser.Store.Objects.UpdateFields;
using WowPacketParser.Store.Objects.UpdateFields.LegacyImplementation;

namespace WowPacketParser.Store.Objects
{
    public sealed class Player : WoWObject
    {
        public static uint PlayerGuidCounter = 0;
        public uint DbGuid;

        public Race Race;

        public Class Class;

        public string Name;

        public bool FirstLogin;

        public int Level;

        public IUnitData UnitData;
        public bool IsActivePlayer = false;

        public Player() : base()
        {
            DbGuid = ++PlayerGuidCounter;

            UnitData = new UnitData(this);
        }

        // Used when inserting data from SMSG_ENUM_CHARACTERS_RESULT into the Objects container
        public static WoWObject UpdatePlayerInfo(Player oldPlayer, Player newPlayer)
        {
            oldPlayer.Race = newPlayer.Race;
            oldPlayer.Class = newPlayer.Class;
            oldPlayer.Name = newPlayer.Name;
            oldPlayer.FirstLogin = newPlayer.FirstLogin;
            oldPlayer.Level = newPlayer.Level;

            return oldPlayer;
        }
    }
}
