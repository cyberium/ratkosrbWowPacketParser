using System;
using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.SQL;

namespace WowPacketParser.Store.Objects
{
    public sealed class XpGainLog : IDataModel
    {
        [DBFieldName("unixtimems", true)]
        public ulong UnixTimeMs;

        [DBFieldName("victim_guid", false, true)]
        public string VictimGuid;

        [DBFieldName("victim_id")]
        public uint VictimId;

        [DBFieldName("victim_type")]
        public string VictimType;

        [DBFieldName("original_amount")]
        public uint OriginalAmount;

        [DBFieldName("reason")]
        public uint Reason;

        [DBFieldName("amount")]
        public uint Amount;

        [DBFieldName("group_bonus")]
        public float GroupBonus;

        [DBFieldName("raf_bonus")]
        public bool RAFBonus;

        public WowGuid GUID;
        public DateTime Time;
    }
}
