using WowPacketParser.Enums;

namespace WowPacketParser.Misc
{
    public sealed class MovementInfo
    {
        public const float DEFAULT_WALK_SPEED = 2.5f;
        public const float DEFAULT_RUN_SPEED = 7.0f;
        public const float DEFAULT_RUN_BACK_SPEED = 4.5f;
        public const float DEFAULT_SWIM_SPEED = 4.72222f;
        public const float DEFAULT_SWIM_BACK_SPEED = 2.5f;
        public const float DEFAULT_FLY_SPEED = 7.0f;
        public const float DEFAULT_FLY_BACK_SPEED = 4.5f;
        public const float DEFAULT_TURN_RATE = 3.141593f;
        public const float DEFAULT_PITCH_RATE = 3.141593f;

        // NOTE: Do not use flag fields in a generic way to handle anything for producing spawns - different versions have different flags
        public uint Flags;

        public uint Flags2;

        public uint Flags3;

        public uint MoveTime;

        public float SwimPitch;

        public uint FallTime;

        public float JumpHorizontalSpeed;

        public float JumpVerticalSpeed;

        public float JumpCosAngle;

        public float JumpSinAngle;

        public float SplineElevation;

        public bool HasSplineData;

        public Vector3 Position;

        public float Orientation;

        public WowGuid TransportGuid;

        public Vector4 TransportOffset;

        public uint TransportTime;

        public sbyte TransportSeat;

        public Quaternion Rotation;

        public float WalkSpeed;

        public float RunSpeed;

        public float RunBackSpeed;

        public float SwimSpeed;

        public float SwimBackSpeed;

        public float FlightSpeed;

        public float FlightBackSpeed;

        public float TurnRate;

        public float PitchRate;

        public bool Hover;

        public float VehicleOrientation;

        public uint VehicleId; // Not exactly related to movement but it is read in ReadMovementUpdateBlock

        public uint TransportPathTimer; // only set for transports

        public bool HasWpsOrRandMov; // waypoints or random movement

        public Vector4 PositionAsVector4 => new Vector4(Position, Orientation);

        public MovementInfo CopyFromMe()
        {
            MovementInfo copy = new MovementInfo();
            copy.Flags = this.Flags;
            copy.Flags2 = this.Flags2;
            copy.Flags3 = this.Flags3;
            copy.SwimPitch = this.SwimPitch;
            copy.FallTime = this.FallTime;
            copy.JumpHorizontalSpeed = this.JumpHorizontalSpeed;
            copy.JumpVerticalSpeed = this.JumpVerticalSpeed;
            copy.JumpCosAngle = this.JumpCosAngle;
            copy.JumpSinAngle = this.JumpSinAngle;
            copy.SplineElevation = this.SplineElevation;
            copy.HasSplineData = this.HasSplineData;
            copy.Position = this.Position;
            copy.Orientation = this.Orientation;
            copy.TransportGuid = this.TransportGuid;
            copy.TransportOffset = this.TransportOffset;
            copy.TransportTime = this.TransportTime;
            copy.TransportSeat = this.TransportSeat;
            copy.Rotation = this.Rotation;
            copy.WalkSpeed = this.WalkSpeed;
            copy.RunSpeed = this.RunSpeed;
            copy.RunBackSpeed = this.RunBackSpeed;
            copy.SwimSpeed = this.SwimSpeed;
            copy.SwimBackSpeed = this.SwimBackSpeed;
            copy.FlightSpeed = this.FlightSpeed;
            copy.FlightBackSpeed = this.FlightBackSpeed;
            copy.TurnRate = this.TurnRate;
            copy.PitchRate = this.PitchRate;
            copy.Hover = this.Hover;
            copy.VehicleId = this.VehicleId;
            copy.VehicleOrientation = this.VehicleOrientation;
            copy.TransportPathTimer = this.TransportPathTimer;
            copy.HasWpsOrRandMov = this.HasWpsOrRandMov;
            return copy;
        }
    }
}
