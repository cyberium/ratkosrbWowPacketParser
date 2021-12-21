using System;
using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.SQL;

namespace WowPacketParser.Store.Objects
{
    [DBTableName("faction_standing_update")]
    public sealed class FactionStandingUpdate : IDataModel
    {
        [DBFieldName("unixtimems", true)]
        public ulong UnixTimeMs;

        [DBFieldName("reputation_list_id", true)]
        public int ReputationListId;

        [DBFieldName("standing", true)]
        public int Standing;

        [DBFieldName("raf_bonus")]
        public float RAFBonus;

        [DBFieldName("achievement_bonus")]
        public float AchievementBonus;

        [DBFieldName("show_visual")]
        public bool ShowVisual;
    }
}