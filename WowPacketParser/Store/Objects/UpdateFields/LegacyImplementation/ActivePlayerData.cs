using System.Collections.Generic;
using System.Linq;
using WowPacketParser.Enums;
using WowPacketParser.Misc;

namespace WowPacketParser.Store.Objects.UpdateFields.LegacyImplementation
{
    public class ActivePlayerData : IActivePlayerData
    {
        private WoWObject Object { get; }
        private Dictionary<int, UpdateField> UpdateFields => (UseOriginalData ? Object.OriginalUpdateFields : Object.UpdateFields);
        private bool UseOriginalData;

        public ActivePlayerData(WoWObject obj, bool useOriginal)
        {
            Object = obj;
            UseOriginalData = useOriginal;
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

        private WowGuid GetGuidValue(ActivePlayerField field)
        {
            if (!ClientVersion.AddedInVersion(ClientType.WarlordsOfDraenor))
            {
                var parts = UpdateFields.GetArray<ActivePlayerField, uint>(field, 2);
                return new WowGuid64(Utilities.MAKE_PAIR64(parts[0], parts[1]));
            }
            else
            {
                var parts = UpdateFields.GetArray<ActivePlayerField, uint>(field, 4);
                return new WowGuid128(Utilities.MAKE_PAIR64(parts[0], parts[1]), Utilities.MAKE_PAIR64(parts[2], parts[3]));
            }
        }

        public int XP => (ClientVersion.AddedInVersion(ClientVersionBuild.V8_0_1_27101) ?
            UpdateFields.GetValue<ActivePlayerField, int>(ActivePlayerField.ACTIVE_PLAYER_FIELD_XP) :
            UpdateFields.GetValue<PlayerField, int>(PlayerField.PLAYER_XP));

        public ulong Coinage => (ClientVersion.AddedInVersion(ClientVersionBuild.V8_0_1_27101) ?
            UpdateFields.GetValue<ActivePlayerField, uint>(ActivePlayerField.ACTIVE_PLAYER_FIELD_COINAGE) :
            UpdateFields.GetValue<PlayerField, uint>(PlayerField.PLAYER_FIELD_COINAGE));

        public float DodgePercentage => (ClientVersion.AddedInVersion(ClientVersionBuild.V8_0_1_27101) ?
            UpdateFields.GetValue<ActivePlayerField, float>(ActivePlayerField.ACTIVE_PLAYER_FIELD_DODGE_PERCENTAGE) :
            UpdateFields.GetValue<PlayerField, float>(PlayerField.PLAYER_DODGE_PERCENTAGE));

        public float CritPercentage => (ClientVersion.AddedInVersion(ClientVersionBuild.V8_0_1_27101) ?
            UpdateFields.GetValue<ActivePlayerField, float>(ActivePlayerField.ACTIVE_PLAYER_FIELD_CRIT_PERCENTAGE) :
            UpdateFields.GetValue<PlayerField, float>(PlayerField.PLAYER_CRIT_PERCENTAGE));

        public class SkillInfo : ISkillInfo
        {
            public ushort[] SkillLineID { get; } = new ushort[256];
            public ushort[] SkillStep { get; } = new ushort[256];
            public ushort[] SkillRank { get; } = new ushort[256];
            public ushort[] SkillMaxRank { get; } = new ushort[256];
        }

        public ISkillInfo Skill
        {
            get
            {
                SkillInfo skillData = new SkillInfo();
                if (ClientVersion.IsClassicClientVersionBuild(ClientVersion.Build))
                {
                    int skillsField = WowPacketParser.Enums.Version.UpdateFields.GetUpdateField(ActivePlayerField.ACTIVE_PLAYER_FIELD_SKILL_LINEID);
                    if (skillsField > 0)
                    {
                        const uint PLAYER_MAX_SKILLS = 256;
                        const uint SKILL_FIELD_ARRAY_SIZE = 256 / 4 * 2;
                        const uint SKILL_ID_OFFSET = 0;
                        const uint SKILL_STEP_OFFSET = SKILL_ID_OFFSET + SKILL_FIELD_ARRAY_SIZE;
                        const uint SKILL_RANK_OFFSET = SKILL_STEP_OFFSET + SKILL_FIELD_ARRAY_SIZE;
                        const uint SUBSKILL_START_RANK_OFFSET = SKILL_RANK_OFFSET + SKILL_FIELD_ARRAY_SIZE;
                        const uint SKILL_MAX_RANK_OFFSET = SUBSKILL_START_RANK_OFFSET + SKILL_FIELD_ARRAY_SIZE;

                        for (uint i = 0; i < PLAYER_MAX_SKILLS; ++i)
                        {
                            uint field = i / 2;
                            uint offset = i & 1; // i % 2

                            UpdateField value;
                            if (UpdateFields.TryGetValue((int)(skillsField + SKILL_ID_OFFSET + field), out value))
                                skillData.SkillLineID[i] = (ushort)((value.UInt32Value >> (offset == 1 ? 16 : 0)) & 0xFFFF);

                            if (UpdateFields.TryGetValue((int)(skillsField + SKILL_STEP_OFFSET + field), out value))
                                skillData.SkillStep[i] = (ushort)((value.UInt32Value >> (offset == 1 ? 16 : 0)) & 0xFFFF);

                            if (UpdateFields.TryGetValue((int)(skillsField + SKILL_RANK_OFFSET + field), out value))
                                skillData.SkillRank[i] = (ushort)((value.UInt32Value >> (offset == 1 ? 16 : 0)) & 0xFFFF);

                            if (UpdateFields.TryGetValue((int)(skillsField + SKILL_MAX_RANK_OFFSET + field), out value))
                                skillData.SkillMaxRank[i] = (ushort)((value.UInt32Value >> (offset == 1 ? 16 : 0)) & 0xFFFF);
                        }
                    }
                }
                return skillData;
            }
        }
    }
}

