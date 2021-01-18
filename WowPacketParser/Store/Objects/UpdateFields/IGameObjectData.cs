using WowPacketParser.Misc;

namespace WowPacketParser.Store.Objects.UpdateFields
{
    public interface IGameObjectData
    {
        WowGuid CreatedBy { get; }
        int DisplayID { get; }
        uint Flags { get; }
        uint DynamicFlags { get; }
        int Level { get; }
        Quaternion ParentRotation { get; }
        int FactionTemplate { get; }
        sbyte State { get; }
        sbyte TypeID { get; }
        uint ArtKit { get; }
        byte PercentHealth { get; }
        byte AnimProgress { get; }
        uint CustomParam { get; }

        IGameObjectData Clone();
    }
}
