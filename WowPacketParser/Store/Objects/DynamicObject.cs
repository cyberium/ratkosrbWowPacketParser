using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.SQL;
using WowPacketParser.Store.Objects.UpdateFields;
using WowPacketParser.Store.Objects.UpdateFields.LegacyImplementation;

namespace WowPacketParser.Store.Objects
{
    public sealed class DynamicObject : WoWObject
    {
        public static uint DynamicObjectGuidCounter = 0;
        public uint DbGuid;

        public IDynamicObjectData DynamicObjectData;
        public IDynamicObjectData DynamicObjectDataOriginal;

        public DynamicObject() : base()
        {
            DbGuid = ++DynamicObjectGuidCounter;

            DynamicObjectData = new DynamicObjectData(this);
            DynamicObjectDataOriginal = new OriginalDynamicObjectData(this);
        }
    }
}
