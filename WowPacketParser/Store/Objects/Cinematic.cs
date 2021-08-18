using System;
using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.SQL;

namespace WowPacketParser.Store.Objects
{
    [DBTableName("cinematic_begin")]
    public sealed class CinematicBegin : IDataModel
    {
        [DBFieldName("unixtimems", true)]
        public ulong UnixTimeMs;

        [DBFieldName("cinematic_id")]
        public uint CinematicId;
    }

    [DBTableName("cinematic_end")]
    public sealed class CinematicEnd : IDataModel
    {
        [DBFieldName("unixtimems", true)]
        public ulong UnixTimeMs;
    }
}
