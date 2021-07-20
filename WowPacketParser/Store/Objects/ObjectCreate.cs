using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WowPacketParser.Misc;

namespace WowPacketParser.Store.Objects
{
    public sealed class ObjectCreate
    {
        public uint Map;

        public MovementInfo MoveInfo;

        public ulong UnixTimeMs;
    }
}
