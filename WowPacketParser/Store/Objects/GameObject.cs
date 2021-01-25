using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.SQL;
using WowPacketParser.Store.Objects.UpdateFields;
using WowPacketParser.Store.Objects.UpdateFields.LegacyImplementation;

namespace WowPacketParser.Store.Objects
{
    public sealed class GameObject : WoWObject, IDataModel
    {
        public static uint GameObjectGuidCounter = 0;
        public uint DbGuid;

        public IGameObjectData GameObjectData;
        public IGameObjectData GameObjectDataOriginal;
        public uint? WorldEffectID;
        public uint? AIAnimKitID;

        public GameObject() : base()
        {
            DbGuid = ++GameObjectGuidCounter;

            GameObjectData = new GameObjectData(this);
            GameObjectDataOriginal = new OriginalGameObjectData(this);
        }

        public override bool IsTemporarySpawn()
        {
            if (ForceTemporarySpawn)
                return true;

            // If our gameobject got the following update field set,
            // it's probably a temporary spawn
            return !GameObjectData.CreatedBy.IsEmpty();
        }

        public uint GetDynamicFlags()
        {
            if (ClientVersion.AddedInVersion(ClientType.WarlordsOfDraenor))
                return ObjectData.DynamicFlags & 0x0000FFFF;

            return GameObjectData.DynamicFlags & 0x0000FFFF;
        }

        public uint GetDynamicFlagsOriginal()
        {
            if (ClientVersion.AddedInVersion(ClientType.WarlordsOfDraenor))
                return ObjectDataOriginal.DynamicFlags & 0x0000FFFF;

            return GameObjectDataOriginal.DynamicFlags & 0x0000FFFF;
        }

        public uint GetPathProgress()
        {
            if (ClientVersion.AddedInVersion(ClientType.WarlordsOfDraenor))
                return (ObjectData.DynamicFlags & 0xFFFF0000) >> 16;

            return (GameObjectData.DynamicFlags & 0xFFFF0000) >> 16;
        }

        public uint GetPathProgressOriginal()
        {
            if (ClientVersion.AddedInVersion(ClientType.WarlordsOfDraenor))
                return (ObjectDataOriginal.DynamicFlags & 0xFFFF0000) >> 16;

            return (GameObjectDataOriginal.DynamicFlags & 0xFFFF0000) >> 16;
        }

        public Quaternion GetStaticRotation()
        {
            if (ClientVersion.AddedInVersion(ClientVersionBuild.V3_1_0_9767))
                return Movement.Rotation;
            else if (ClientVersion.AddedInVersion(ClientVersionBuild.V3_0_2_9056)) // packed quaternion
            {
                var packedQuat = UpdateFields.GetArray<GameObjectField, uint>(GameObjectField.GAMEOBJECT_ROTATION, 2);
                return new Quaternion((((long)packedQuat[1]) << 32) | packedQuat[0]);
            }
            else
            {
                float []rotation = UpdateFields.GetArray<GameObjectField, float>(GameObjectField.GAMEOBJECT_ROTATION, 4);
                return new Quaternion(rotation[0], rotation[1], rotation[2], rotation[3]);
            }
        }

        public Quaternion? GetParentRotation()
        {
            if (ClientVersion.AddedInVersion(ClientVersionBuild.V3_0_2_9056))
                return GameObjectData.ParentRotation;
            else
                return null;
        }

        public bool IsTransport()
        {
            return (GameObjectType)GameObjectData.TypeID == GameObjectType.MOTransport;
        }
    }
}
