using System;
using System.Collections.Generic;
using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.SQL;

namespace WowPacketParser.Store.Objects
{
    public sealed class ObjectSound
    {
        public ObjectSound(uint sound_, DateTime time_, WowGuid guid_)
        {
            sound = sound_;
            time = time_;
            guid = guid_;
        }
        public uint sound;
        public DateTime time;
        public WowGuid guid;
    }

    [DBTableName("play_music")]
    public sealed class PlayMusic : IDataModel
    {
        [DBFieldName("music", true)]
        public uint Music;

        [DBFieldName("unixtime", true)]
        public uint UnixTime;
    }
}
