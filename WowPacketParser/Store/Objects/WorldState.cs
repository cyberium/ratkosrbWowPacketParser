using System;
using System.Collections.Generic;
using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.SQL;

namespace WowPacketParser.Store.Objects
{
    [DBTableName("world_state_init")]
    public sealed class WorldStateInit : IDataModel
    {
        [DBFieldName("unixtime", true)]
        public uint UnixTime;

        [DBFieldName("map", true)]
        public uint Map;

        [DBFieldName("zone_id", true)]
        public int ZoneId;

        [DBFieldName("area_id", true)]
        public int AreaId;

        [DBFieldName("variable", true)]
        public int Variable;

        [DBFieldName("value", true)]
        public int Value;
    }

    [DBTableName("world_state_update")]
    public sealed class WorldStateUpdate : IDataModel
    {
        [DBFieldName("unixtime", true)]
        public uint UnixTime;

        [DBFieldName("variable", true)]
        public int Variable;

        [DBFieldName("value", true)]
        public int Value;
    }
}
