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
        public IActivePlayerData ActivePlayerData;
        public IActivePlayerData ActivePlayerDataOriginal;

        public Player() : base(false)
        {
            DbGuid = ++PlayerGuidCounter;
            PlayerData = new PlayerData(this);
            PlayerDataOriginal = new OriginalPlayerData(this);
            ActivePlayerData = new ActivePlayerData(this, false);
            ActivePlayerDataOriginal = new ActivePlayerData(this, true);
        }

        public uint GetItemInSlot(EquipmentSlotType slot)
        {
            if (ClientVersion.IsUsingNewUpdateFieldSystem())
                return (uint)PlayerData.VisibleItems[(int)slot].ItemID;

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
                return 0;

            UpdateField value;
            UpdateFields.TryGetValue(field + offset * (int)slot, out value);
            return value.UInt32Value;
        }

        public void GetSkill(ushort skillId, out ushort currentSkill, out ushort maxSkill)
        {
            const uint PLAYER_MAX_SKILLS = 256;
            ISkillInfo skillData = ActivePlayerData.Skill;

            for (uint i = 0; i < PLAYER_MAX_SKILLS; ++i)
            {
                if (skillId == skillData.SkillLineID[i])
                {
                    currentSkill = skillData.SkillRank[i];
                    maxSkill = skillData.SkillMaxRank[i];
                    return;
                }
            }

            currentSkill = 0;
            maxSkill = 0;
        }
    }
}
