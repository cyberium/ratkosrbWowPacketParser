using System.Collections.Generic;
using System.Linq;
using WowPacketParser.Enums;
using WowPacketParser.Misc;

namespace WowPacketParser.Store.Objects.UpdateFields.LegacyImplementation
{
    public class PlayerData : IPlayerData
    {
        private WoWObject Object { get; }
        private Dictionary<int, UpdateField> UpdateFields => Object.UpdateFields;

        public PlayerData(WoWObject obj)
        {
            Object = obj;
        }

        private WowGuid GetGuidValue(PlayerField field)
        {
            if (!ClientVersion.AddedInVersion(ClientType.WarlordsOfDraenor))
            {
                var parts = UpdateFields.GetArray<PlayerField, uint>(field, 2);
                return new WowGuid64(Utilities.MAKE_PAIR64(parts[0], parts[1]));
            }
            else
            {
                var parts = UpdateFields.GetArray<PlayerField, uint>(field, 4);
                return new WowGuid128(Utilities.MAKE_PAIR64(parts[0], parts[1]), Utilities.MAKE_PAIR64(parts[2], parts[3]));
            }
        }

        public WowGuid WowAccount => GetGuidValue(PlayerField.PLAYER_WOW_ACCOUNT);
        public uint Experience => UpdateFields.GetValue<PlayerField, uint>(PlayerField.PLAYER_XP);
        public uint Money => UpdateFields.GetValue<PlayerField, uint>(PlayerField.PLAYER_FIELD_COINAGE);
        public uint PlayerBytes1 => UpdateFields.GetValue<PlayerField, uint>(PlayerField.PLAYER_FIELD_BYTES);
        public uint PlayerBytes2 => UpdateFields.GetValue<PlayerField, uint>(PlayerField.PLAYER_FIELD_BYTES2);
        public uint PlayerFlags => UpdateFields.GetValue<PlayerField, uint>(PlayerField.PLAYER_FLAGS);
    }

    public class OriginalPlayerData : IPlayerData
    {
        private WoWObject Object { get; }
        private Dictionary<int, UpdateField> UpdateFields => Object.UpdateFields;

        public OriginalPlayerData(WoWObject obj)
        {
            Object = obj;
        }

        private WowGuid GetGuidValue(PlayerField field)
        {
            if (!ClientVersion.AddedInVersion(ClientType.WarlordsOfDraenor))
            {
                var parts = UpdateFields.GetArray<PlayerField, uint>(field, 2);
                return new WowGuid64(Utilities.MAKE_PAIR64(parts[0], parts[1]));
            }
            else
            {
                var parts = UpdateFields.GetArray<PlayerField, uint>(field, 4);
                return new WowGuid128(Utilities.MAKE_PAIR64(parts[0], parts[1]), Utilities.MAKE_PAIR64(parts[2], parts[3]));
            }
        }

        public WowGuid WowAccount => GetGuidValue(PlayerField.PLAYER_WOW_ACCOUNT);
        public uint Experience => UpdateFields.GetValue<PlayerField, uint>(PlayerField.PLAYER_XP);
        public uint Money => UpdateFields.GetValue<PlayerField, uint>(PlayerField.PLAYER_FIELD_COINAGE);
        public uint PlayerBytes1 => UpdateFields.GetValue<PlayerField, uint>(PlayerField.PLAYER_FIELD_BYTES);
        public uint PlayerBytes2 => UpdateFields.GetValue<PlayerField, uint>(PlayerField.PLAYER_FIELD_BYTES2);
        public uint PlayerFlags => UpdateFields.GetValue<PlayerField, uint>(PlayerField.PLAYER_FLAGS);
    }
}

