using System;
using WowPacketParser.Enums;

namespace WowPacketParser.Misc
{
    public sealed class MovementInfo
    {
        // NOTE: Do not use flag fields in a generic way to handle anything for producing spawns - different versions have different flags
        public MovementFlag Flags;

        public MovementFlagExtra FlagsExtra;

        public bool HasSplineData;

        public Vector3 Position;

        public float Orientation;

        public WowGuid TransportGuid;

        public Vector4 TransportOffset;

        public Quaternion Rotation;

        public float WalkSpeed;

        public float RunSpeed;

        public uint VehicleId; // Not exactly related to movement but it is read in ReadMovementUpdateBlock

        public bool HasWpsOrRandMov; // waypoints or random movement

        public MovementInfo CopyFromMe()
        {
            MovementInfo copy = new MovementInfo();
            copy.Flags = this.Flags;
            copy.FlagsExtra = this.FlagsExtra;
            copy.HasSplineData = this.HasSplineData;
            copy.Position = this.Position;
            copy.Orientation = this.Orientation;
            copy.TransportGuid = this.TransportGuid;
            copy.TransportOffset = this.TransportOffset;
            copy.Rotation = this.Rotation;
            copy.WalkSpeed = this.WalkSpeed;
            copy.RunSpeed = this.RunSpeed;
            copy.VehicleId = this.VehicleId;
            copy.HasWpsOrRandMov = this.HasWpsOrRandMov;
            return copy;
        }
    }
}
