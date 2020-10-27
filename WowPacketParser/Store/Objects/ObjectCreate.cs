using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WowPacketParser.Store.Objects
{
    public sealed class ObjectCreate
    {
        public float? PositionX;

        public float? PositionY;

        public float? PositionZ;

        public float? Orientation;

        public ulong UnixTimeMs;
    }
}
