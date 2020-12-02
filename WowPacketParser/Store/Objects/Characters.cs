using System;
using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.SQL;

namespace WowPacketParser.Store.Objects
{
    [DBTableName("characters")]
    public sealed class CharacterTemplate : IDataModel
    {
        [DBFieldName("guid", true, true)]
        public string Guid;

        [DBFieldName("account", false, true)]
        public string Account;

        [DBFieldName("name")]
        public string Name;

        [DBFieldName("race")]
        public uint? Race;

        [DBFieldName("class")]
        public uint Class;

        [DBFieldName("gender")]
        public uint Gender;

        [DBFieldName("level")]
        public uint Level;

        [DBFieldName("xp")]
        public uint XP = 0;

        [DBFieldName("money")]
        public uint Money = 0;

        [DBFieldName("playerBytes")]
        public uint PlayerBytes;

        [DBFieldName("playerBytes2")]
        public uint PlayerBytes2;

        [DBFieldName("playerFlags")]
        public uint PlayerFlags;

        [DBFieldName("position_x")]
        public float PositionX;

        [DBFieldName("position_y")]
        public float PositionY;

        [DBFieldName("position_z")]
        public float PositionZ;

        [DBFieldName("map")]
        public uint Map;

        [DBFieldName("orientation")]
        public float Orientation;

        [DBFieldName("health")]
        public uint Health = 1;

        [DBFieldName("power1")]
        public uint Power1 = 0;

        [DBFieldName("equipmentCache")]
        public string EquipmentCache = "";
    }
    [DBTableName("character_inventory")]
    public sealed class CharacterInventory : IDataModel
    {
        [DBFieldName("guid", true, true)]
        public string Guid;

        [DBFieldName("bag")]
        public uint Bag;

        [DBFieldName("slot")]
        public uint Slot;

        [DBFieldName("item", true, true)]
        public string ItemGuid;

        [DBFieldName("item_template")]
        public uint ItemTemplate;
    }
    [DBTableName("item_instance")]
    public sealed class CharacterItemInstance : IDataModel
    {
        [DBFieldName("guid", true, true)]
        public string Guid;

        [DBFieldName("itemEntry")]
        public uint ItemEntry;

        [DBFieldName("owner_guid", false, true)]
        public string OwnerGuid;

        [DBFieldName("count")]
        public uint Count = 1;

        [DBFieldName("charges")]
        public string Charges = "0 0 0 0 0 ";

        [DBFieldName("enchantments")]
        public string Enchantments = "0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 ";

        [DBFieldName("durability")]
        public uint Durability = 1;
    }
    [DBTableName("character_movement_client")]
    public sealed class ClientSideMovement : IDataModel
    {
        [DBFieldName("guid", true, true)]
        public string Guid;

        [DBFieldName("opcode")]
        public string Opcode;

        [DBFieldName("move_time")]
        public uint MoveTime;

        [DBFieldName("move_flags")]
        public uint MoveFlags;

        [DBFieldName("map")]
        public uint Map;

        [DBFieldName("position_x")]
        public float PositionX;

        [DBFieldName("position_y")]
        public float PositionY;

        [DBFieldName("position_z")]
        public float PositionZ;

        [DBFieldName("orientation")]
        public float Orientation;

        [DBFieldName("unixtimems", true)]
        public ulong UnixTimeMs;
    }
    public sealed class PlayerMovement
    {
        public WowGuid guid;
        public Vector4 Position;
        public uint Map;
        public uint MoveTime;
        public uint MoveFlags;
        public int Opcode;
        public Direction OpcodeDirection;
        public DateTime Time;
    }
    [DBTableName("character_active_player")]
    public sealed class CharacterActivePlayer : IDataModel
    {
        [DBFieldName("guid", true, true)]
        public string Guid;

        [DBFieldName("unixtime", true)]
        public uint UnixTime;
    }
    public sealed class ActivePlayerCreateTime
    {
        public WowGuid Guid;
        public DateTime Time;
    }
}
