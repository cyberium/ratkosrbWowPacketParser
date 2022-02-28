using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using WowPacketParser.Enums;
using WowPacketParser.Enums.Version;
using WowPacketParser.Misc;
using WowPacketParser.Store;
using WowPacketParser.Store.Objects;

namespace WowPacketParser.Parsing.Parsers
{
    public static class MovementHandler
    {
        [ThreadStatic]
        public static uint CurrentMapId;

        public static uint CurrentDifficultyID = 1;
        public static int CurrentPhaseMask = 1;

        // this is a dictionary because ConcurrentSet does not exist
        public static readonly IDictionary<ushort, bool> ActivePhases = new ConcurrentDictionary<ushort, bool>();

        public static MovementInfo ReadMovementInfo(Packet packet, WowGuid guid, object index = null)
        {
            if (ClientVersion.AddedInVersion(ClientVersionBuild.V4_2_0_14333))
                return ReadMovementInfo420(packet, index);

            return ReadMovementInfoGen(packet, guid, index);
        }

        public static MovementFlag ConvertVanillaMovementFlags(MovementFlagVanilla flags)
        {
            MovementFlag newFlags = MovementFlag.None;

            if (flags.HasAnyFlag(MovementFlagVanilla.Forward))
                newFlags |= MovementFlag.Forward;
            if (flags.HasAnyFlag(MovementFlagVanilla.Backward))
                newFlags |= MovementFlag.Backward;
            if (flags.HasAnyFlag(MovementFlagVanilla.StrafeLeft))
                newFlags |= MovementFlag.StrafeLeft;
            if (flags.HasAnyFlag(MovementFlagVanilla.StrafeRight))
                newFlags |= MovementFlag.StrafeRight;
            if (flags.HasAnyFlag(MovementFlagVanilla.TurnLeft))
                newFlags |= MovementFlag.TurnLeft;
            if (flags.HasAnyFlag(MovementFlagVanilla.TurnRight))
                newFlags |= MovementFlag.TurnRight;
            if (flags.HasAnyFlag(MovementFlagVanilla.PitchUp))
                newFlags |= MovementFlag.PitchUp;
            if (flags.HasAnyFlag(MovementFlagVanilla.PitchDown))
                newFlags |= MovementFlag.PitchDown;
            if (flags.HasAnyFlag(MovementFlagVanilla.WalkMode))
                newFlags |= MovementFlag.WalkMode;
            if (flags.HasAnyFlag(MovementFlagVanilla.OnTransport))
                newFlags |= MovementFlag.OnTransport;
            if (flags.HasAnyFlag(MovementFlagVanilla.Levitating))
                newFlags |= MovementFlag.DisableGravity;
            if (flags.HasAnyFlag(MovementFlagVanilla.Root))
                newFlags |= MovementFlag.Root;
            if (flags.HasAnyFlag(MovementFlagVanilla.Falling))
                newFlags |= MovementFlag.Falling;
            if (flags.HasAnyFlag(MovementFlagVanilla.FallingFar))
                newFlags |= MovementFlag.FallingFar;
            if (flags.HasAnyFlag(MovementFlagVanilla.Swimming))
                newFlags |= MovementFlag.Swimming;
            if (flags.HasAnyFlag(MovementFlagVanilla.SplineEnabled))
                newFlags |= MovementFlag.SplineEnabled;
            if (flags.HasAnyFlag(MovementFlagVanilla.CanFly))
                newFlags |= MovementFlag.CanFly;
            if (flags.HasAnyFlag(MovementFlagVanilla.Flying))
                newFlags |= MovementFlag.Flying;
            if (flags.HasAnyFlag(MovementFlagVanilla.SplineElevation))
                newFlags |= MovementFlag.SplineElevation;
            if (flags.HasAnyFlag(MovementFlagVanilla.Waterwalking))
                newFlags |= MovementFlag.Waterwalking;
            if (flags.HasAnyFlag(MovementFlagVanilla.CanSafeFall))
                newFlags |= MovementFlag.CanSafeFall;
            if (flags.HasAnyFlag(MovementFlagVanilla.Hover))
                newFlags |= MovementFlag.Hover;

            return newFlags;
        }

        public static MovementFlag ConvertTBCMovementFlags(MovementFlagTBC flags)
        {
            MovementFlag newFlags = MovementFlag.None;

            if (flags.HasAnyFlag(MovementFlagTBC.Forward))
                newFlags |= MovementFlag.Forward;
            if (flags.HasAnyFlag(MovementFlagTBC.Backward))
                newFlags |= MovementFlag.Backward;
            if (flags.HasAnyFlag(MovementFlagTBC.StrafeLeft))
                newFlags |= MovementFlag.StrafeLeft;
            if (flags.HasAnyFlag(MovementFlagTBC.StrafeRight))
                newFlags |= MovementFlag.StrafeRight;
            if (flags.HasAnyFlag(MovementFlagTBC.TurnLeft))
                newFlags |= MovementFlag.TurnLeft;
            if (flags.HasAnyFlag(MovementFlagTBC.TurnRight))
                newFlags |= MovementFlag.TurnRight;
            if (flags.HasAnyFlag(MovementFlagTBC.PitchUp))
                newFlags |= MovementFlag.PitchUp;
            if (flags.HasAnyFlag(MovementFlagTBC.PitchDown))
                newFlags |= MovementFlag.PitchDown;
            if (flags.HasAnyFlag(MovementFlagTBC.WalkMode))
                newFlags |= MovementFlag.WalkMode;
            if (flags.HasAnyFlag(MovementFlagTBC.OnTransport))
                newFlags |= MovementFlag.OnTransport;
            if (flags.HasAnyFlag(MovementFlagTBC.DisableGravity))
                newFlags |= MovementFlag.DisableGravity;
            if (flags.HasAnyFlag(MovementFlagTBC.Root))
                newFlags |= MovementFlag.Root;
            if (flags.HasAnyFlag(MovementFlagTBC.Falling))
                newFlags |= MovementFlag.Falling;
            if (flags.HasAnyFlag(MovementFlagTBC.FallingFar))
                newFlags |= MovementFlag.FallingFar;
            if (flags.HasAnyFlag(MovementFlagTBC.Swimming))
                newFlags |= MovementFlag.Swimming;
            if (flags.HasAnyFlag(MovementFlagTBC.SplineEnabled))
                newFlags |= MovementFlag.SplineEnabled;
            if (flags.HasAnyFlag(MovementFlagTBC.CanFly))
                newFlags |= MovementFlag.CanFly;
            if (flags.HasAnyFlag(MovementFlagTBC.Flying) || flags.HasAnyFlag(MovementFlagTBC.Flying2))
                newFlags |= MovementFlag.Flying;
            if (flags.HasAnyFlag(MovementFlagTBC.SplineElevation))
                newFlags |= MovementFlag.SplineElevation;
            if (flags.HasAnyFlag(MovementFlagTBC.Waterwalking))
                newFlags |= MovementFlag.Waterwalking;
            if (flags.HasAnyFlag(MovementFlagTBC.CanSafeFall))
                newFlags |= MovementFlag.CanSafeFall;
            if (flags.HasAnyFlag(MovementFlagTBC.Hover))
                newFlags |= MovementFlag.Hover;

            return newFlags;
        }

        private static MovementInfo ReadMovementInfoGen(Packet packet, WowGuid guid, object index)
        {
            var info = new MovementInfo();

            if (ClientVersion.AddedInVersion(ClientVersionBuild.V3_0_2_9056))
                info.Flags = (uint)packet.ReadInt32E<MovementFlag>("Movement Flags", index);
            else if (ClientVersion.AddedInVersion(ClientVersionBuild.V2_0_1_6180))
                info.Flags = (uint)ConvertTBCMovementFlags(packet.ReadInt32E<MovementFlagTBC>("Movement Flags", index));
            else
                info.Flags = (uint)ConvertVanillaMovementFlags(packet.ReadInt32E<MovementFlagVanilla>("Movement Flags", index));

            if (ClientVersion.AddedInVersion(ClientVersionBuild.V3_0_2_9056))
                info.FlagsExtra = (uint)packet.ReadInt16E<MovementFlagExtra>("Extra Movement Flags", index);
            else if (ClientVersion.AddedInVersion(ClientVersionBuild.V2_0_1_6180))
                info.FlagsExtra = (uint)packet.ReadByteE<MovementFlagExtra>("Extra Movement Flags", index);

            if (ClientVersion.AddedInVersion(ClientVersionBuild.V4_2_2_14545))
                if (packet.ReadGuid("Guid 2", index) != guid)
                    throw new InvalidDataException("Guids are not equal.");

            info.MoveTime = packet.ReadUInt32("Time", index);

            info.Position = packet.ReadVector3("Position", index);
            info.Orientation = packet.ReadSingle("Orientation", index);

            if (info.Flags.HasAnyFlag(MovementFlag.OnTransport))
            {
                if (ClientVersion.AddedInVersion(ClientVersionBuild.V3_1_0_9767))
                    info.TransportGuid = packet.ReadPackedGuid("Transport GUID", index);
                else
                    info.TransportGuid = packet.ReadGuid("Transport GUID", index);

                info.TransportOffset = packet.ReadVector4("Transport Position", index);
                info.TransportTime = packet.ReadUInt32("Transport Time", index);

                if (ClientVersion.AddedInVersion(ClientType.WrathOfTheLichKing))
                    info.TransportSeat = packet.ReadSByte("Transport Seat", index);

                if (info.FlagsExtra.HasAnyFlag(MovementFlagExtra.InterpolateMove))
                    packet.ReadInt32("Transport Time", index);
            }

            if (info.Flags.HasAnyFlag(MovementFlag.Swimming | MovementFlag.Flying) ||
                info.FlagsExtra.HasAnyFlag(MovementFlagExtra.AlwaysAllowPitching))
                info.SwimPitch = packet.ReadSingle("Swim Pitch", index);

            if (ClientVersion.AddedInVersion(ClientType.Cataclysm))
            {
                if (info.FlagsExtra.HasAnyFlag(MovementFlagExtra.InterpolateTurning))
                {
                    info.FallTime = packet.ReadUInt32("Jump Fall Time", index);
                    info.JumpVerticalSpeed = packet.ReadSingle("Jump Vertical Speed", index);

                    if (info.Flags.HasAnyFlag(MovementFlag.Falling))
                    {
                        info.JumpSinAngle = packet.ReadSingle("Jump Sin Angle", index);
                        info.JumpCosAngle = packet.ReadSingle("Jump Cos Angle", index);
                        info.JumpHorizontalSpeed = packet.ReadSingle("Jump Horizontal Speed", index);
                    }
                }
            }
            else
            {
                info.FallTime = packet.ReadUInt32("Jump Fall Time", index);
                if (info.Flags.HasAnyFlag(MovementFlag.Falling))
                {
                    info.JumpVerticalSpeed = packet.ReadSingle("Jump Vertical Speed", index);
                    info.JumpSinAngle = packet.ReadSingle("Jump Sin Angle", index);
                    info.JumpCosAngle = packet.ReadSingle("Jump Cos Angle", index);
                    info.JumpHorizontalSpeed = packet.ReadSingle("Jump Horizontal Speed", index);
                }
            }

            // HACK: "generic" movement flags are wrong for 4.2.2
            if (info.Flags.HasAnyFlag(MovementFlag.SplineElevation) && ClientVersion.Build != ClientVersionBuild.V4_2_2_14545)
                info.SplineElevation = packet.ReadSingle("Spline Elevation", index);

            return info;
        }

        private static MovementInfo ReadMovementInfo420(Packet packet, object index)
        {
            var info = new MovementInfo
            {
                Flags = (uint)packet.ReadBitsE<MovementFlag>("Movement Flags", 30, index),
                FlagsExtra = (uint)packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 12, index)
            };

            var onTransport = packet.ReadBit("OnTransport", index);
            var hasInterpolatedMovement = false;
            var time3 = false;
            if (onTransport)
            {
                hasInterpolatedMovement = packet.ReadBit("HasInterpolatedMovement", index);
                time3 = packet.ReadBit("Time3", index);
            }

            var swimming = packet.ReadBit("Swimming", index);
            var interPolatedTurning = packet.ReadBit("InterPolatedTurning", index);

            var jumping = false;
            if (interPolatedTurning)
                jumping = packet.ReadBit("Jumping", index);

            var splineElevation = packet.ReadBit("SplineElevation", index);

            info.HasSplineData = packet.ReadBit("HasSplineData", index);

            packet.ResetBitReader(); // reset bitreader

            packet.ReadGuid("GUID 2", index);

            info.MoveTime = packet.ReadUInt32("Time", index);

            info.Position = packet.ReadVector3("Position", index);
            info.Orientation = packet.ReadSingle("Orientation", index);

            if (onTransport)
            {
                info.TransportGuid = packet.ReadGuid("Transport GUID", index);
                info.TransportOffset = packet.ReadVector4("Transport Position", index);
                info.TransportSeat = packet.ReadSByte("Transport Seat", index);
                info.TransportTime = packet.ReadUInt32("Transport Time", index);
                if (hasInterpolatedMovement)
                    packet.ReadInt32("Transport Time 2", index);
                if (time3)
                    packet.ReadInt32("Transport Time 3", index);
            }
            if (swimming)
                info.SwimPitch = packet.ReadSingle("Swim Pitch", index);

            if (interPolatedTurning)
            {
                info.FallTime = packet.ReadUInt32("Jump Fall Time", index);
                info.JumpVerticalSpeed = packet.ReadSingle("Jump Vertical Speed", index);
                if (jumping)
                {
                    info.JumpSinAngle = packet.ReadSingle("Jump Sin Angle", index);
                    info.JumpCosAngle = packet.ReadSingle("Jump Cos Angle", index);
                    info.JumpHorizontalSpeed = packet.ReadSingle("Jump Horizontal Speed", index);

                }
            }
            if (splineElevation)
                info.SplineElevation = packet.ReadSingle("Spline Elevation", index);

            return info;
        }

        [Parser(Opcode.SMSG_ON_MONSTER_MOVE)]
        [Parser(Opcode.SMSG_MONSTER_MOVE_TRANSPORT)]
        public static void HandleMonsterMove(Packet packet)
        {
            WowGuid guid = packet.ReadPackedGuid("GUID");
            Unit obj = null;
            ServerSideMovement monsterMove = null;
            if (Storage.Objects != null && Storage.Objects.ContainsKey(guid))
            {
                obj = Storage.Objects[guid].Item1 as Unit;
                if (obj.UpdateFields != null)
                {
                    obj.Movement.HasWpsOrRandMov = true;
                    if (Settings.SaveTransports || packet.Opcode == Opcodes.GetOpcode(Opcode.SMSG_ON_MONSTER_MOVE, Direction.ServerToClient))
                        monsterMove = new ServerSideMovement();
                }   
            }

            if (packet.Opcode == Opcodes.GetOpcode(Opcode.SMSG_MONSTER_MOVE_TRANSPORT, Direction.ServerToClient))
            {
                WowGuid transportGuid = packet.ReadPackedGuid("Transport GUID");
                if (monsterMove != null)
                    monsterMove.TransportGuid = transportGuid;

                sbyte seat = 0;
                if (ClientVersion.AddedInVersion(ClientVersionBuild.V3_1_0_9767)) // no idea when this was added exactly
                    seat = packet.ReadSByte("Transport Seat");
                if (monsterMove != null)
                    monsterMove.TransportSeat = seat;

                if (transportGuid.HasEntry() && transportGuid.GetHighType() == HighGuidType.Vehicle &&
                    guid.HasEntry() && guid.GetHighType() == HighGuidType.Creature)
                {
                    VehicleTemplateAccessory vehicleAccessory = new VehicleTemplateAccessory
                    {
                        Entry = transportGuid.GetEntry(),
                        AccessoryEntry = guid.GetEntry(),
                        SeatId = seat
                    };
                    Storage.VehicleTemplateAccessories.Add(vehicleAccessory, packet.TimeSpan);
                }
            }

            if (ClientVersion.AddedInVersion(ClientVersionBuild.V3_1_0_9767)) // no idea when this was added exactly
                packet.ReadBool("Toggle AnimTierInTrans");

            var pos = packet.ReadVector3("Position");

            packet.ReadInt32("Move Ticks");

            var type = packet.ReadByteE<SplineType>("Spline Type");

            float orientation = 100;
            switch (type)
            {
                case SplineType.FacingSpot:
                {
                    var faceSpot = packet.ReadVector3("Facing Spot");
                    orientation = Utilities.GetAngle(pos.X, pos.Y, faceSpot.X, faceSpot.Y);
                    break;
                }
                case SplineType.FacingTarget:
                {
                    packet.ReadGuid("Facing GUID");
                    break;
                }
                case SplineType.FacingAngle:
                {
                    orientation = packet.ReadSingle("Facing Angle");
                    break;
                }
                case SplineType.Stop:
                    return;
            }
            if (monsterMove != null)
                monsterMove.Orientation = orientation;

            if (ClientVersion.AddedInVersion(ClientVersionBuild.V5_1_0_16309))
            {
                // Not the best way
                ReadSplineMovement510(monsterMove, packet, pos);
                if (monsterMove != null)
                    obj.AddWaypoint(monsterMove, pos, packet.Time);
                return;
            }

            if (ClientVersion.AddedInVersion(ClientVersionBuild.V4_2_2_14545))
            {
                // Not the best way
                ReadSplineMovement422(monsterMove, packet, pos);
                if (monsterMove != null)
                    obj.AddWaypoint(monsterMove, pos, packet.Time);
                return;
            }

            var flags = packet.ReadInt32E<SplineFlag>("Spline Flags");
            if (monsterMove != null)
                monsterMove.SplineFlags = (uint)flags;

            if (flags.HasAnyFlag(SplineFlag.AnimationTier))
            {
                packet.ReadByteE<MovementAnimationState>("Animation State");
                packet.ReadInt32("Async-time in ms");
            }

            int moveTime = packet.ReadInt32("Move Time");
            if (monsterMove != null)
                monsterMove.MoveTime = (uint)moveTime;

            if (flags.HasAnyFlag(SplineFlag.Trajectory))
            {
                packet.ReadSingle("Vertical Speed");
                packet.ReadInt32("Async-time in ms");
            }

            var waypoints = packet.ReadInt32("Waypoints");
            if (monsterMove != null)
            {
                monsterMove.SplineCount = (uint)waypoints;
                if (waypoints > 0)
                    monsterMove.SplinePoints = new List<Vector3>();
            }

            if (flags.HasAnyFlag(SplineFlag.Flying | SplineFlag.CatmullRom))
            {
                for (var i = 0; i < waypoints; i++)
                {
                    Vector3 vec = packet.ReadVector3("Waypoint", i);
                    if (monsterMove != null)
                        monsterMove.SplinePoints.Add(vec);
                }
            }
            else
            {
                var newpos = packet.ReadVector3("Waypoint Endpoint");

                Vector3 mid = (pos + newpos) * 0.5f;

                for (var i = 1; i < waypoints; i++)
                {
                    var vec = packet.ReadPackedVector3();

                    if (ClientVersion.AddedInVersion(ClientVersionBuild.V2_0_1_6180))
                        vec = mid - vec;
                    else
                        vec = newpos - vec;

                    if (monsterMove != null)
                        monsterMove.SplinePoints.Add(vec);

                    packet.AddValue("Waypoint", vec, i);
                }

                if (monsterMove != null && monsterMove.SplinePoints != null)
                    monsterMove.SplinePoints.Add(newpos);
            }

            if (monsterMove != null)
                obj.AddWaypoint(monsterMove, pos, packet.Time);
        }

        private static void ReadSplineMovement510(ServerSideMovement monsterMove, Packet packet, Vector3 pos)
        {
            var flags = packet.ReadInt32E<SplineFlag434>("Spline Flags");
            if (monsterMove != null)
                monsterMove.SplineFlags = (uint)flags;

            if (flags.HasAnyFlag(SplineFlag434.Animation))
            {
                packet.ReadByteE<MovementAnimationState>("Animation State");
                packet.ReadInt32("Asynctime in ms"); // Async-time in ms
            }

            int moveTime = packet.ReadInt32("Move Time");
            if (monsterMove != null)
                monsterMove.MoveTime = (uint)moveTime;

            if (flags.HasAnyFlag(SplineFlag434.Parabolic))
            {
                packet.ReadSingle("Vertical Speed");
                packet.ReadInt32("Async-time in ms");
            }

            var waypoints = packet.ReadInt32("Waypoints");
            if (monsterMove != null)
            {
                monsterMove.SplineCount = (uint)waypoints;
                if (waypoints > 0)
                    monsterMove.SplinePoints = new List<Vector3>();
            }

            if (flags.HasAnyFlag(SplineFlag434.UncompressedPath))
            {
                for (var i = 0; i < waypoints; i++)
                {
                    Vector3 point = packet.ReadVector3("Waypoint", i);
                    if (monsterMove != null)
                        monsterMove.SplinePoints.Add(point);
                }
            }
            else
            {
                var newpos = packet.ReadVector3("Waypoint Endpoint");

                var mid = new Vector3
                {
                    X = (pos.X + newpos.X)*0.5f,
                    Y = (pos.Y + newpos.Y)*0.5f,
                    Z = (pos.Z + newpos.Z)*0.5f
                };

                if (waypoints != 1)
                {
                    var vec = packet.ReadPackedVector3();
                    vec.X += mid.X;
                    vec.Y += mid.Y;
                    vec.Z += mid.Z;
                    packet.AddValue("Waypoint: ", vec, 0);
                    if (monsterMove != null)
                        monsterMove.SplinePoints.Add(vec);

                    if (waypoints > 2)
                    {
                        for (var i = 1; i < waypoints - 1; ++i)
                        {
                            vec = packet.ReadPackedVector3();
                            vec.X += mid.X;
                            vec.Y += mid.Y;
                            vec.Z += mid.Z;

                            packet.AddValue("Waypoint", vec, i);
                            if (monsterMove != null)
                                monsterMove.SplinePoints.Add(vec);
                        }
                    }
                }

                if (monsterMove != null && monsterMove.SplinePoints != null)
                    monsterMove.SplinePoints.Add(newpos);
            }

            var unkLoopCounter = packet.ReadUInt16("Unk UInt16");
            if (unkLoopCounter > 1)
            {
                packet.ReadSingle("Unk Float 1");
                packet.ReadUInt16("Unk UInt16 1");
                packet.ReadUInt16("Unk UInt16 2");
                packet.ReadSingle("Unk Float 2");
                packet.ReadUInt16("Unk UInt16 3");

                for (var i = 0; i < unkLoopCounter; i++)
                {
                    packet.ReadUInt16("Unk UInt16 1", i);
                    packet.ReadUInt16("Unk UInt16 2", i);
                }
            }
        }

        private static void ReadSplineMovement422(ServerSideMovement monsterMove, Packet packet, Vector3 pos)
        {
            var flags = packet.ReadInt32E<SplineFlag422>("Spline Flags");
            if (monsterMove != null)
                monsterMove.SplineFlags = (uint)flags;

            if (flags.HasAnyFlag(SplineFlag422.AnimationTier))
            {
                packet.ReadByteE<MovementAnimationState>("Animation State");
                packet.ReadInt32("Asynctime in ms"); // Async-time in ms
            }

            int moveTime = packet.ReadInt32("Move Time");
            if (monsterMove != null)
                monsterMove.MoveTime = (uint)moveTime;

            if (flags.HasAnyFlag(SplineFlag422.Trajectory))
            {
                packet.ReadSingle("Vertical Speed");
                packet.ReadInt32("Unk Int32 2");
            }

            var waypoints = packet.ReadInt32("Waypoints");
            if (monsterMove != null)
            {
                monsterMove.SplineCount = (uint)waypoints;
                if (waypoints > 0)
                    monsterMove.SplinePoints = new List<Vector3>();
            }

            if (flags.HasAnyFlag(SplineFlag422.UsePathSmoothing))
            {
                for (var i = 0; i < waypoints; i++)
                {
                    Vector3 point = packet.ReadVector3("Waypoint", i);
                    if (monsterMove != null)
                        monsterMove.SplinePoints.Add(point);
                }
            }
            else
            {
                var newpos = packet.ReadVector3("Waypoint Endpoint");

                var mid = new Vector3
                {
                    X = (pos.X + newpos.X)*0.5f,
                    Y = (pos.Y + newpos.Y)*0.5f,
                    Z = (pos.Z + newpos.Z)*0.5f
                };

                for (var i = 1; i < waypoints; i++)
                {
                    var vec = packet.ReadPackedVector3();
                    vec.X += mid.X;
                    vec.Y += mid.Y;
                    vec.Z += mid.Z;

                    if (monsterMove != null)
                        monsterMove.SplinePoints.Add(vec);
                    packet.AddValue("Waypoint", vec);
                }

                if (monsterMove != null && monsterMove.SplinePoints != null)
                    monsterMove.SplinePoints.Add(newpos);
            }
        }

        [HasSniffData]
        [Parser(Opcode.SMSG_NEW_WORLD, ClientVersionBuild.Zero, ClientVersionBuild.V4_2_2_14545)]
        [Parser(Opcode.SMSG_LOGIN_VERIFY_WORLD)]
        public static void HandleEnterWorld(Packet packet)
        {
            CurrentMapId = (uint) packet.ReadInt32<MapId>("Map ID");
            packet.ReadVector4("Position");

            Storage.ClearDataOnMapChange();
            packet.AddSniffData(StoreNameType.Map, (int) CurrentMapId, "NEW_WORLD");
        }

        [HasSniffData]
        [Parser(Opcode.SMSG_NEW_WORLD, ClientVersionBuild.V4_2_2_14545, ClientVersionBuild.V4_3_4_15595)]
        public static void HandleNewWorld422(Packet packet)
        {
            packet.ReadVector3("Position");
            CurrentMapId = (uint) packet.ReadInt32<MapId>("Map");
            packet.ReadSingle("Orientation");

            Storage.ClearDataOnMapChange();
            packet.AddSniffData(StoreNameType.Map, (int)CurrentMapId, "NEW_WORLD");
        }

        [HasSniffData]
        [Parser(Opcode.SMSG_NEW_WORLD, ClientVersionBuild.V5_1_0_16309)]
        public static void HandleNewWorld510(Packet packet)
        {
            CurrentMapId = (uint)packet.ReadInt32<MapId>("Map");
            packet.ReadSingle("Y");
            packet.ReadSingle("Orientation");
            packet.ReadSingle("X");
            packet.ReadSingle("Z");

            Storage.ClearDataOnMapChange();
            packet.AddSniffData(StoreNameType.Map, (int)CurrentMapId, "NEW_WORLD");
        }

        [Parser(Opcode.SMSG_LOGIN_SET_TIME_SPEED)]
        public static void HandleLoginSetTimeSpeed(Packet packet)
        {
            packet.ReadPackedTime("Game Time");
            packet.ReadSingle("New Speed");

            if (ClientVersion.AddedInVersion(ClientVersionBuild.V3_1_2_9901))
                packet.ReadInt32("Game Time Holiday Offset");
        }

        [Parser(Opcode.SMSG_BIND_POINT_UPDATE)]
        public static void HandleBindPointUpdate(Packet packet)
        {
            packet.ReadVector3("Position");
            packet.ReadInt32<MapId>("Map Id");
            packet.ReadInt32<ZoneId>("Zone Id");
        }

        [Parser(Opcode.CMSG_UPDATE_MISSILE_TRAJECTORY)]
        public static void HandleUpdateMissileTrajectory(Packet packet)
        {
            packet.ReadGuid("GUID");
            packet.ReadInt32<SpellId>("Spell ID");
            packet.ReadSingle("Elevation");
            packet.ReadSingle("Missile speed");
            packet.ReadVector3("Current Position");
            packet.ReadVector3("Targeted Position");

            // Boolean if it will send MSG_MOVE_STOP
            if (!packet.ReadBoolean())
                return;

            var opcode = packet.ReadInt32();
            // None length is recieved, so we have to calculate the remaining bytes.
            var remainingLength = packet.Length - packet.Position;
            var bytes = packet.ReadBytes((int)remainingLength);

            using (var newpacket = new Packet(bytes, opcode, packet.Time, packet.Direction, packet.Number, packet.Writer, packet.FileName))
                Handler.Parse(newpacket, true);
        }

        [Parser(Opcode.MSG_MOVE_TELEPORT_ACK, ClientVersionBuild.Zero, ClientVersionBuild.V2_0_1_6180)]
        public static void HandleTeleportAckVanilla(Packet packet)
        {
            if (packet.Direction == Direction.ServerToClient)
            {
                var guid = packet.ReadPackedGuid("Guid");
                packet.ReadInt32("Movement Counter");
                ReadMovementInfo(packet, guid);
            }
            else
            {
                packet.ReadGuid("Guid");
                packet.ReadInt32("Movement Counter");
                packet.ReadUInt32("Time");
            }
        }

        [Parser(Opcode.MSG_MOVE_TELEPORT_ACK, ClientVersionBuild.V2_0_1_6180, ClientVersionBuild.V4_3_4_15595)]
        public static void HandleTeleportAck(Packet packet)
        {
            var guid = packet.ReadPackedGuid("Guid");
            packet.ReadInt32("Movement Counter");

            if (packet.Direction == Direction.ServerToClient)
            {
                ReadMovementInfo(packet, guid);
            }
            else
            {
                packet.ReadUInt32("Time");
            }
        }

        [Parser(Opcode.MSG_MOVE_HEARTBEAT, ClientVersionBuild.V4_2_2_14545, ClientVersionBuild.V4_3_0_15005)]
        public static void HandleMovementHeartbeat422(Packet packet)
        {
            MovementInfo info = new MovementInfo();
            info.Flags = (uint)packet.ReadBitsE<MovementFlag>("Movement flags", 30);

            packet.ReadBit("HasSplineData");

            var guidBytes = new byte[8];
            guidBytes[0] = packet.ReadBit();
            guidBytes[6] = packet.ReadBit();
            guidBytes[1] = packet.ReadBit();
            guidBytes[7] = packet.ReadBit();
            guidBytes[2] = packet.ReadBit();
            guidBytes[4] = packet.ReadBit();
            guidBytes[3] = packet.ReadBit();

            info.FlagsExtra = (uint)packet.ReadBitsE<MovementFlagExtra>("Movement flags extra", 12);

            guidBytes[5] = packet.ReadBit();
            var splineElevation = packet.ReadBit("SplineElevation"); // OR Swimming
            var onTransport = packet.ReadBit("OnTransport");

            var transportBytes = new byte[8];
            var hasInterpolatedMovement = false;
            var time3 = false;
            if (onTransport)
            {
                transportBytes = packet.StartBitStream(0, 6, 2, 5, 4, 1, 3, 7);
                hasInterpolatedMovement = packet.ReadBit("HasInterpolatedMovement");
                time3 = packet.ReadBit("Time3");
            }

            var swimming = packet.ReadBit("Swimming");  // OR SplineElevation
            var interPolatedTurning = packet.ReadBit("InterPolatedTurning");
            var jumping = false;
            if (interPolatedTurning)
                jumping = packet.ReadBit("Jumping");

            info.MoveTime = packet.ReadUInt32("Time");
            info.Position = packet.ReadVector3("Position");
            info.Orientation = packet.ReadSingle("Orientation");

            packet.ReadXORByte(guidBytes, 7);
            packet.ReadXORByte(guidBytes, 5);

            if (splineElevation)
                info.SplineElevation = packet.ReadSingle("Spline Elevation");

            packet.ReadXORByte(guidBytes, 1);
            packet.ReadXORByte(guidBytes, 6);
            packet.ReadXORByte(guidBytes, 4);
            packet.ReadXORByte(guidBytes, 3);

            if (onTransport)
            {
                info.TransportSeat = packet.ReadSByte("Transport Seat");
                info.TransportOffset.O = packet.ReadSingle("Transport Orientation");
                Vector3 transportPos = packet.ReadVector3("Transport Position");
                info.TransportOffset.X = transportPos.X;
                info.TransportOffset.Y = transportPos.Y;
                info.TransportOffset.Z = transportPos.Z;
                info.TransportTime = (uint)packet.ReadInt32("Transport Time");
                if (hasInterpolatedMovement)
                    packet.ReadInt32("Transport Time 2");

                packet.ReadXORByte(transportBytes, 3);
                packet.ReadXORByte(transportBytes, 6);

                if (time3)
                    packet.ReadInt32("Transport Time 3");

                packet.ReadXORByte(transportBytes, 7);
                packet.ReadXORByte(transportBytes, 5);
                packet.ReadXORByte(transportBytes, 2);
                packet.ReadXORByte(transportBytes, 1);
                packet.ReadXORByte(transportBytes, 0);
                packet.ReadXORByte(transportBytes, 4);

                info.TransportGuid = packet.WriteGuid("Transport Guid", transportBytes);
            }

            if (swimming)
                info.SwimPitch = packet.ReadSingle("Swim Pitch");

            if (interPolatedTurning)
            {
                info.FallTime = (uint)packet.ReadInt32("Jump Fall Time");
                info.JumpVerticalSpeed = packet.ReadSingle("Fall Vertical Speed");
                if (jumping)
                {
                    info.JumpHorizontalSpeed = packet.ReadSingle("Jump Horizontal Speed");
                    info.JumpCosAngle = packet.ReadSingle("Jump Cos Angle");
                    info.JumpSinAngle = packet.ReadSingle("Jump Sin Angle");
                }
            }

            packet.ReadXORByte(guidBytes, 2);
            packet.ReadXORByte(guidBytes, 0);

            WowGuid moverGuid = packet.WriteGuid("Guid", guidBytes);
            Storage.StorePlayerMovement(moverGuid, info, packet);
        }

        [Parser(Opcode.MSG_MOVE_HEARTBEAT, ClientVersionBuild.V4_3_3_15354, ClientVersionBuild.V4_3_4_15595)]
        public static void HandleMovementHeartbeat433(Packet packet)
        {
            MovementInfo info = new MovementInfo();
            info.Flags = (uint)packet.ReadBitsE<MovementFlag>("Movement flags", 30);

            packet.ReadBit("HasSplineData");

            var guidBytes = new byte[8];
            guidBytes[0] = packet.ReadBit();
            guidBytes[6] = packet.ReadBit();
            guidBytes[1] = packet.ReadBit();
            guidBytes[7] = packet.ReadBit();
            guidBytes[2] = packet.ReadBit();
            guidBytes[4] = packet.ReadBit();
            guidBytes[3] = packet.ReadBit();

            info.FlagsExtra = (uint)packet.ReadBitsE<MovementFlagExtra>("Movement flags extra", 12);

            guidBytes[5] = packet.ReadBit();
            var splineElevation = packet.ReadBit("SplineElevation"); // OR Swimming
            var onTransport = packet.ReadBit("OnTransport");

            var transportBytes = new byte[8];
            var hasInterpolatedMovement = false;
            var time3 = false;
            if (onTransport)
            {
                transportBytes = packet.StartBitStream(0, 6, 2, 5, 4, 1, 3, 7);
                hasInterpolatedMovement = packet.ReadBit("HasInterpolatedMovement");
                time3 = packet.ReadBit("Time3");
            }

            var swimming = packet.ReadBit("Swimming");  // OR SplineElevation
            var interPolatedTurning = packet.ReadBit("InterPolatedTurning");
            var jumping = false;
            if (interPolatedTurning)
                jumping = packet.ReadBit("Jumping");

            info.MoveTime = packet.ReadUInt32("Time");
            info.Position = packet.ReadVector3("Position");
            info.Orientation = packet.ReadSingle("Orientation");

            packet.ReadXORByte(guidBytes, 7);
            packet.ReadXORByte(guidBytes, 5);

            if (splineElevation)
                info.SplineElevation = packet.ReadSingle("Spline Elevation");

            packet.ReadXORByte(guidBytes, 1);
            packet.ReadXORByte(guidBytes, 6);
            packet.ReadXORByte(guidBytes, 4);
            packet.ReadXORByte(guidBytes, 3);

            if (onTransport)
            {
                info.TransportSeat = packet.ReadSByte("Transport Seat");
                info.TransportOffset.O = packet.ReadSingle("Transport Orientation");
                Vector3 transportPos = packet.ReadVector3("Transport Position");
                info.TransportOffset.X = transportPos.X;
                info.TransportOffset.Y = transportPos.Y;
                info.TransportOffset.Z = transportPos.Z;
                info.TransportTime = (uint)packet.ReadInt32("Transport Time");
                if (hasInterpolatedMovement)
                    packet.ReadInt32("Transport Time 2");

                packet.ReadXORByte(transportBytes, 3);
                packet.ReadXORByte(transportBytes, 6);

                if (time3)
                    packet.ReadInt32("Transport Time 3");

                packet.ReadXORByte(transportBytes, 7);
                packet.ReadXORByte(transportBytes, 5);
                packet.ReadXORByte(transportBytes, 2);
                packet.ReadXORByte(transportBytes, 1);
                packet.ReadXORByte(transportBytes, 0);
                packet.ReadXORByte(transportBytes, 4);

                info.TransportGuid = packet.WriteGuid("Transport Guid", transportBytes);
            }

            if (swimming)
                info.SwimPitch = packet.ReadSingle("Swim Pitch");

            if (interPolatedTurning)
            {
                info.FallTime = (uint)packet.ReadInt32("Jump Fall Time");
                info.JumpVerticalSpeed = packet.ReadSingle("Jump Vertical SPeed");
                if (jumping)
                {
                    info.JumpHorizontalSpeed = packet.ReadSingle("Jump Horizontal Speed");
                    info.JumpCosAngle = packet.ReadSingle("Jump Cos Angle");
                    info.JumpSinAngle = packet.ReadSingle("Jump Sin Angle");
                }
            }

            packet.ReadXORByte(guidBytes, 2);

            packet.ReadXORByte(guidBytes, 0);

            WowGuid moverGuid = packet.WriteGuid("Guid", guidBytes);
            Storage.StorePlayerMovement(moverGuid, info, packet);
        }

        [Parser(Opcode.MSG_MOVE_SET_PITCH, ClientVersionBuild.V4_2_2_14545, ClientVersionBuild.V4_3_0_15005)]
        public static void HandleMovementSetPitch422(Packet packet)
        {
            var guidBytes = new byte[8];
            guidBytes[1] = packet.ReadBit();
            guidBytes[6] = packet.ReadBit();
            guidBytes[7] = packet.ReadBit();
            guidBytes[3] = packet.ReadBit();

            MovementInfo info = new MovementInfo();
            info.Flags = (uint)packet.ReadBitsE<MovementFlag>("Movement flags", 30);

            guidBytes[5] = packet.ReadBit();
            guidBytes[2] = packet.ReadBit();
            guidBytes[0] = packet.ReadBit();

            packet.ReadBit("HasSplineData");

            guidBytes[4] = packet.ReadBit();

            info.FlagsExtra = (uint)packet.ReadBitsE<MovementFlagExtra>("Movement flags extra", 12);

            var splineElevation = packet.ReadBit("SplineElevation"); // OR Swimming
            var onTransport = packet.ReadBit("OnTransport");

            var transportBytes = new byte[8];
            var hasInterpolatedMovement = false;
            var time3 = false;
            if (onTransport)
            {
                transportBytes = packet.StartBitStream(0, 6, 2, 5, 4, 1, 3, 7);
                hasInterpolatedMovement = packet.ReadBit("HasInterpolatedMovement");
                time3 = packet.ReadBit("HasTime3");
            }

            var swimming = packet.ReadBit("HasPitch");  // OR SplineElevation
            var interPolatedTurning = packet.ReadBit("HasFallData");
            var jumping = false;
            if (interPolatedTurning)
                jumping = packet.ReadBit("HasFallDirection");

            info.Position = packet.ReadVector3("Position");
            info.MoveTime = packet.ReadUInt32("Time");
            info.Orientation = packet.ReadSingle("Orientation");

            packet.ReadXORByte(guidBytes, 1);
            packet.ReadXORByte(guidBytes, 4);

            if (splineElevation)
                info.SplineElevation = packet.ReadSingle("Spline Elevation");

            if (onTransport)
            {
                info.TransportSeat = packet.ReadSByte("Transport Seat");
                info.TransportOffset.O = packet.ReadSingle("Transport Orientation");
                Vector3 transportPos = packet.ReadVector3("Transport Position");
                info.TransportOffset.X = transportPos.X;
                info.TransportOffset.Y = transportPos.Y;
                info.TransportOffset.Z = transportPos.Z;
                info.TransportTime = (uint)packet.ReadInt32("Transport Time");
                if (hasInterpolatedMovement)
                    packet.ReadInt32("Transport Time 2");

                packet.ReadXORByte(transportBytes, 3);
                packet.ReadXORByte(transportBytes, 6);

                if (time3)
                    packet.ReadInt32("Transport Time 3");

                packet.ReadXORByte(transportBytes, 7);
                packet.ReadXORByte(transportBytes, 5);
                packet.ReadXORByte(transportBytes, 2);
                packet.ReadXORByte(transportBytes, 1);
                packet.ReadXORByte(transportBytes, 0);
                packet.ReadXORByte(transportBytes, 4);

                info.TransportGuid = packet.WriteGuid("Transport Guid", transportBytes);
            }

            if (swimming)
                info.SwimPitch = packet.ReadSingle("Swim Pitch");

            packet.ReadXORByte(guidBytes, 5);

            if (interPolatedTurning)
            {
                info.FallTime = (uint)packet.ReadInt32("Jump Fall Time");
                info.JumpVerticalSpeed = packet.ReadSingle("Jump Vertical Speed");
                if (jumping)
                {
                    info.JumpHorizontalSpeed = packet.ReadSingle("Jump Horizontal Speed");
                    info.JumpSinAngle = packet.ReadSingle("Jump Sin Angle");
                    info.JumpCosAngle = packet.ReadSingle("Jump Cos Angle");
                }
            }

            packet.ReadXORByte(guidBytes, 0);
            packet.ReadXORByte(guidBytes, 3);
            packet.ReadXORByte(guidBytes, 6);
            packet.ReadXORByte(guidBytes, 7);
            packet.ReadXORByte(guidBytes, 2);

            WowGuid moverGuid = packet.WriteGuid("Guid", guidBytes);
            Storage.StorePlayerMovement(moverGuid, info, packet);
        }

        [Parser(Opcode.MSG_MOVE_SET_FACING, ClientVersionBuild.V4_2_2_14545, ClientVersionBuild.V4_3_0_15005)]
        public static void HandleMovementSetFacing422(Packet packet)
        {
            var info = new MovementInfo();
            var guidBytes = new byte[8];
            var transportGuidBytes = new byte[8];

            info.Flags = (uint)packet.ReadBitsE<MovementFlag>("Movement Flags", 30);

            guidBytes[4] = packet.ReadBit();
            guidBytes[2] = packet.ReadBit();

            info.HasSplineData = packet.ReadBit("HasSplineData");

            guidBytes[3] = packet.ReadBit();
            guidBytes[5] = packet.ReadBit();

            info.FlagsExtra = (uint)packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 12);

            guidBytes[0] = packet.ReadBit();
            guidBytes[7] = packet.ReadBit();
            guidBytes[6] = packet.ReadBit();
            guidBytes[1] = packet.ReadBit();

            var splineElevation = packet.ReadBit("HaveSplineElevation");

            var havePitch = packet.ReadBit("HavePitch");
            var haveFallData = packet.ReadBit("HaveFallData");
            var haveFallDirection = false;

            if (haveFallData)
                haveFallDirection = packet.ReadBit("HaveFallDirection");

            var haveTransportData = packet.ReadBit("HaveTransportData");

            var haveTransportTime2 = false;
            var haveTransportTime3 = false;

            if (haveTransportData)
            {
                transportGuidBytes = packet.StartBitStream(0, 6, 2, 5, 4, 1, 3, 7);
                haveTransportTime2 = packet.ReadBit("HaveTransportTime2");
                haveTransportTime3 = packet.ReadBit("HaveTransportTime3");
            }

            info.Orientation = packet.ReadSingle("Orientation");

            info.MoveTime = packet.ReadUInt32("Timestamp");

            info.Position = packet.ReadVector3("Position");

            packet.ParseBitStream(guidBytes, 7, 5);

            if (splineElevation)
                info.SplineElevation = packet.ReadSingle("Spline Elevation");

            packet.ParseBitStream(guidBytes, 4, 1, 2);

            if (havePitch)
                info.SwimPitch = packet.ReadSingle("Pitch");

            if (haveFallData)
            {
                info.FallTime = packet.ReadUInt32("Jump Fall Time");
                info.JumpVerticalSpeed = packet.ReadSingle("Jump Vertical Speed");
                info.JumpHorizontalSpeed = packet.ReadSingle("Jump Horizontal Speed");

                if (haveFallDirection)
                {
                    info.JumpCosAngle = packet.ReadSingle("Jump Cos Angle");
                    info.JumpSinAngle = packet.ReadSingle("Jump Sin Angle");
                }
            }

            packet.ParseBitStream(guidBytes, 6, 0);

            if (haveTransportData)
            {
                info.TransportSeat = packet.ReadSByte("Transport Seat");
                info.TransportOffset.O = packet.ReadSingle("Transport Orientation");
                Vector3 transportPos = packet.ReadVector3("Transport Position");
                info.TransportOffset.X = transportPos.X;
                info.TransportOffset.Y = transportPos.Y;
                info.TransportOffset.Z = transportPos.Z;
                info.TransportTime = packet.ReadUInt32("Transport Time");

                if (haveTransportTime2)
                    packet.ReadUInt32("Transport Time 2");

                packet.ParseBitStream(transportGuidBytes, 3, 6);

                if (haveTransportTime3)
                    packet.ReadUInt32("Transport Time 3");

                packet.ParseBitStream(transportGuidBytes, 7, 5, 2, 1, 0, 4);
            }

            packet.ParseBitStream(guidBytes, 3);

            WowGuid moverGuid = packet.WriteGuid("Guid", guidBytes);
            info.TransportGuid = packet.WriteGuid("Transport Guid", transportGuidBytes);
            Storage.StorePlayerMovement(moverGuid, info, packet);
        }

        [Parser(Opcode.MSG_MOVE_TELEPORT, ClientVersionBuild.V4_2_2_14545, ClientVersionBuild.V4_3_4_15595)]
        public static void HandleMoveTeleport422(Packet packet)
        {
            var onTransport = packet.ReadBit("OnTransport");

            var guid = packet.StartBitStream(0, 2, 6, 7, 4, 5, 3, 1);

            var unk2 = packet.ReadBit("Unk Bit Boolean 2");

            MovementInfo info = new MovementInfo();
            info.Position = packet.ReadVector3("Destination Position");

            packet.ReadXORByte(guid, 5);
            packet.ReadXORByte(guid, 4);

            if (onTransport)
                info.TransportGuid = packet.ReadGuid("Transport Guid");

            packet.ReadXORByte(guid, 2);
            packet.ReadXORByte(guid, 7);

            packet.ReadInt32("Unk 1");

            packet.ReadXORByte(guid, 1);
            packet.ReadXORByte(guid, 2);
            packet.ReadXORByte(guid, 6);
            packet.ReadXORByte(guid, 3);

            if (unk2)
                packet.ReadByte("Unk 2");

            info.Orientation = packet.ReadSingle("Arrive Orientation");
            WowGuid moverGuid = packet.WriteGuid("Guid", guid);
            Storage.StorePlayerMovement(moverGuid, info, packet);
        }

        [Parser(Opcode.MSG_MOVE_TELEPORT, ClientVersionBuild.V5_1_0_16309)]
        public static void HandleMoveTeleport510(Packet packet)
        {
            var guid = new byte[8];
            var transGuid = new byte[8];
            var pos = new Vector4();
            MovementInfo info = new MovementInfo();

            packet.ReadUInt32("Unk");

            info.Position = new Vector3
            {
                X = pos.X = packet.ReadSingle(),
                Y = pos.Y = packet.ReadSingle(),
                Z = pos.Z = packet.ReadSingle()
            };
            info.Orientation = pos.O = packet.ReadSingle();
            packet.AddValue("Destination", pos);

            guid[3] = packet.ReadBit();
            guid[1] = packet.ReadBit();
            guid[7] = packet.ReadBit();

            var bit48 = packet.ReadBit();

            guid[6] = packet.ReadBit();

            if (bit48)
            {
                packet.ReadBit("Unk bit 50");
                packet.ReadBit("Unk bit 51");
            }

            guid[0] = packet.ReadBit();
            guid[4] = packet.ReadBit();

            var onTransport = packet.ReadBit("On transport");
            guid[2] = packet.ReadBit();
            if (onTransport)
                transGuid = packet.StartBitStream(7, 5, 2, 1, 0, 4, 3, 6);

            guid[5] = packet.ReadBit();

            if (onTransport)
            {
                packet.ParseBitStream(transGuid, 1, 5, 7, 0, 3, 4, 6, 2);
                info.TransportGuid = packet.WriteGuid("Transport Guid", transGuid);
            }

            packet.ReadXORByte(guid, 3);

            if (bit48)
                packet.ReadUInt32("Unk int");

            packet.ReadXORByte(guid, 2);
            packet.ReadXORByte(guid, 1);
            packet.ReadXORByte(guid, 7);
            packet.ReadXORByte(guid, 5);
            packet.ReadXORByte(guid, 6);
            packet.ReadXORByte(guid, 4);
            packet.ReadXORByte(guid, 0);

            WowGuid moverGuid = packet.WriteGuid("Guid", guid);
            Storage.StorePlayerMovement(moverGuid, info, packet);
        }

        [Parser(Opcode.MSG_MOVE_STOP, ClientVersionBuild.V4_2_2_14545, ClientVersionBuild.V4_3_0_15005)]
        public static void HandleMoveStop422(Packet packet)
        {
            var info = new MovementInfo();
            var guidBytes = new byte[8];
            var transportGuidBytes = new byte[8];

            guidBytes[2] = packet.ReadBit();
            guidBytes[0] = packet.ReadBit();

            info.HasSplineData = packet.ReadBit("HasSplineData");

            info.Flags = (uint)packet.ReadBitsE<MovementFlag>("Movement Flags", 30);

            guidBytes[4] = packet.ReadBit();
            guidBytes[6] = packet.ReadBit();
            guidBytes[3] = packet.ReadBit();
            guidBytes[5] = packet.ReadBit();
            guidBytes[7] = packet.ReadBit();

            info.FlagsExtra = (uint)packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 12);

            guidBytes[1] = packet.ReadBit();

            var havePitch = packet.ReadBit("HavePitch");

            var haveFallData = packet.ReadBit("HaveFallData");

            var haveFallDirection = false;
            if (haveFallData)
                haveFallDirection = packet.ReadBit("HaveFallDirection");

            var haveTransportData = packet.ReadBit("HaveTransportData");

            var haveTransportTime2 = false;
            var haveTransportTime3 = false;

            if (haveTransportData)
            {
                transportGuidBytes = packet.StartBitStream(0, 6, 2, 5, 4, 1, 3, 7);
                haveTransportTime2 = packet.ReadBit("HaveTransportTime2");
                haveTransportTime3 = packet.ReadBit("HaveTransportTime3");
            }

            var splineElevation = packet.ReadBit("HaveSplineElevation");

            info.Orientation = packet.ReadSingle("Orientation");

            info.MoveTime = packet.ReadUInt32("Timestamp");

            info.Position = packet.ReadVector3("Position");

            packet.ReadXORByte(guidBytes, 2);
            packet.ReadXORByte(guidBytes, 3);

            if (havePitch)
                info.SwimPitch = packet.ReadSingle("Pitch");

            if (haveFallData)
            {
                info.FallTime = packet.ReadUInt32("Jump Fall Time");
                info.JumpVerticalSpeed = packet.ReadSingle("Jump Vertical Speed");
                info.JumpHorizontalSpeed = packet.ReadSingle("Jump Horizontal Speed");

                if (haveFallDirection)
                {
                    info.JumpCosAngle = packet.ReadSingle("Jump Cos Angle");
                    info.JumpSinAngle = packet.ReadSingle("Jump Sin Angle");
                }
            }

            packet.ReadXORByte(guidBytes, 5);
            packet.ReadXORByte(guidBytes, 7);

            if (haveTransportData)
            {
                info.TransportSeat = packet.ReadSByte("Transport Seat");
                info.TransportOffset.O = packet.ReadSingle("Transport Orientation");
                Vector3 transportPos = packet.ReadVector3("Transport Position");
                info.TransportOffset.X = transportPos.X;
                info.TransportOffset.Y = transportPos.Y;
                info.TransportOffset.Z = transportPos.Z;
                info.TransportTime = packet.ReadUInt32("Transport Time");

                if (haveTransportTime2)
                    packet.ReadUInt32("Transport Time 2");

                packet.ParseBitStream(transportGuidBytes, 3, 6);

                if (haveTransportTime3)
                    packet.ReadUInt32("Transport Time 3");

                packet.ParseBitStream(transportGuidBytes, 7, 5, 2, 1, 0, 4);
            }

            packet.ReadXORByte(guidBytes, 1);
            packet.ReadXORByte(guidBytes, 0);

            if (splineElevation)
                info.SplineElevation = packet.ReadSingle("Spline Elevation");

            packet.ReadXORByte(guidBytes, 6);
            packet.ReadXORByte(guidBytes, 4);

            WowGuid moverGuid = packet.WriteGuid("Guid", guidBytes);
            info.TransportGuid = packet.WriteGuid("Transport Guid", transportGuidBytes);
            Storage.StorePlayerMovement(moverGuid, info, packet);
        }

        [Parser(Opcode.SMSG_MOVE_UPDATE, ClientVersionBuild.V4_2_2_14545, ClientVersionBuild.V4_3_0_15005)]
        public static void HandlePlayerMove422(Packet packet)
        {
            var info = new MovementInfo();
            var guidBytes = new byte[8];
            var transportGuidBytes = new byte[8];

            var splineElevation = packet.ReadBit("HaveSplineElevation");
            var haveTransportData = packet.ReadBit("HaveTransportData");
            guidBytes[5] = packet.ReadBit();

            var haveTransportTime2 = false;
            var haveTransportTime3 = false;
            if (haveTransportData)
            {
                transportGuidBytes[2] = packet.ReadBit();
                transportGuidBytes[4] = packet.ReadBit();
                transportGuidBytes[1] = packet.ReadBit();
                transportGuidBytes[3] = packet.ReadBit();
                transportGuidBytes[0] = packet.ReadBit();
                haveTransportTime2 = packet.ReadBit("HaveTransportTime2");
                transportGuidBytes[7] = packet.ReadBit();
                haveTransportTime3 = packet.ReadBit("HaveTransportTime3");
                transportGuidBytes[6] = packet.ReadBit();
                transportGuidBytes[5] = packet.ReadBit();
            }

            guidBytes[7] = packet.ReadBit();
            guidBytes[3] = packet.ReadBit();
            guidBytes[1] = packet.ReadBit();
            guidBytes[4] = packet.ReadBit();
            guidBytes[0] = packet.ReadBit();
            info.Flags = (uint)packet.ReadBitsE<MovementFlag>("Movement Flags", 30);
            var havePitch = packet.ReadBit("HavePitch");
            guidBytes[2] = packet.ReadBit();
            info.FlagsExtra = (uint)packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 12);
            guidBytes[6] = packet.ReadBit();

            var haveFallData = packet.ReadBit("HaveFallData");
            var haveFallDirection = false;
            if (haveFallData)
                haveFallDirection = packet.ReadBit("HaveFallDirection");

            info.HasSplineData = packet.ReadBit("HasSplineData");

            packet.ReadXORByte(guidBytes, 4);
            packet.ReadXORByte(guidBytes, 0);

            info.Orientation = packet.ReadSingle("Orientation");

            packet.ReadXORByte(guidBytes, 6);
            packet.ReadXORByte(guidBytes, 7);

            if (splineElevation)
                info.SplineElevation = packet.ReadSingle("Spline Elevation");

            if (haveTransportData)
            {
                packet.ReadXORByte(transportGuidBytes, 4);
                packet.ReadXORByte(transportGuidBytes, 2);
                info.TransportOffset.O = packet.ReadSingle("Transport Orientation");
                info.TransportTime = packet.ReadUInt32("Transport Time");
                info.TransportSeat = packet.ReadSByte("Transport Seat");
                packet.ReadXORByte(transportGuidBytes, 3);
                Vector3 transportPos = packet.ReadVector3("Transport Position");
                info.TransportOffset.X = transportPos.X;
                info.TransportOffset.Y = transportPos.Y;
                info.TransportOffset.Z = transportPos.Z;
                packet.ReadXORByte(transportGuidBytes, 1);

                if (haveTransportTime2)
                    packet.ReadUInt32("Transport Time 2");

                if (haveTransportTime3)
                    packet.ReadUInt32("Transport Time 3");

                packet.ReadXORByte(transportGuidBytes, 5);
                packet.ReadXORByte(transportGuidBytes, 0);
                packet.ReadXORByte(transportGuidBytes, 6);
                packet.ReadXORByte(transportGuidBytes, 7);
            }

            packet.ReadXORByte(guidBytes, 2);
            info.MoveTime = packet.ReadUInt32("Timestamp");
            packet.ReadXORByte(guidBytes, 1);

            if (havePitch)
                info.SwimPitch = packet.ReadSingle("Pitch");

            info.Position = packet.ReadVector3("Position");
            packet.ReadXORByte(guidBytes, 5);
            packet.ReadXORByte(guidBytes, 3);

            if (haveFallData)
            {
                info.JumpHorizontalSpeed = packet.ReadSingle("Jump Horizontal Speed");

                if (haveFallDirection)
                {
                    info.JumpCosAngle = packet.ReadSingle("Jump Cos Angle");
                    info.JumpSinAngle = packet.ReadSingle("Jump Sin Angle");
                }

                info.JumpVerticalSpeed = packet.ReadSingle("Jump Vertical Speed");
                info.FallTime = packet.ReadUInt32("Jump Fall Time");
            }

            WowGuid moverGuid = packet.WriteGuid("Guid", guidBytes);
            info.TransportGuid = packet.WriteGuid("Transport Guid", transportGuidBytes);
            Storage.StorePlayerMovement(moverGuid, info, packet);
        }

        [Parser(Opcode.SMSG_MOVE_UPDATE, ClientVersionBuild.Zero, ClientVersionBuild.V4_2_2_14545)]
        public static void HandlePlayerMove(Packet packet)
        {
        }

        [Parser(Opcode.MSG_MOVE_START_FORWARD, ClientVersionBuild.Zero, ClientVersionBuild.V4_3_4_15595)]
        [Parser(Opcode.MSG_MOVE_START_BACKWARD, ClientVersionBuild.Zero, ClientVersionBuild.V4_3_4_15595)]
        [Parser(Opcode.MSG_MOVE_STOP, ClientVersionBuild.Zero, ClientVersionBuild.V4_2_2_14545)]
        [Parser(Opcode.MSG_MOVE_START_STRAFE_LEFT, ClientVersionBuild.Zero, ClientVersionBuild.V4_3_4_15595)]
        [Parser(Opcode.MSG_MOVE_START_STRAFE_RIGHT, ClientVersionBuild.Zero, ClientVersionBuild.V4_3_4_15595)]
        [Parser(Opcode.MSG_MOVE_STOP_STRAFE, ClientVersionBuild.Zero, ClientVersionBuild.V4_3_4_15595)]
        [Parser(Opcode.MSG_MOVE_START_ASCEND, ClientVersionBuild.Zero, ClientVersionBuild.V4_3_4_15595)]
        [Parser(Opcode.MSG_MOVE_START_DESCEND, ClientVersionBuild.Zero, ClientVersionBuild.V4_3_4_15595)]
        [Parser(Opcode.MSG_MOVE_STOP_ASCEND, ClientVersionBuild.Zero, ClientVersionBuild.V4_3_4_15595)]
        [Parser(Opcode.MSG_MOVE_JUMP, ClientVersionBuild.Zero, ClientVersionBuild.V4_3_4_15595)]
        [Parser(Opcode.MSG_MOVE_START_TURN_LEFT, ClientVersionBuild.Zero, ClientVersionBuild.V4_3_4_15595)]
        [Parser(Opcode.MSG_MOVE_START_TURN_RIGHT, ClientVersionBuild.Zero, ClientVersionBuild.V4_3_4_15595)]
        [Parser(Opcode.MSG_MOVE_STOP_TURN, ClientVersionBuild.Zero, ClientVersionBuild.V4_3_4_15595)]
        [Parser(Opcode.MSG_MOVE_START_PITCH_UP, ClientVersionBuild.Zero, ClientVersionBuild.V4_3_4_15595)]
        [Parser(Opcode.MSG_MOVE_START_PITCH_DOWN, ClientVersionBuild.Zero, ClientVersionBuild.V4_3_4_15595)]
        [Parser(Opcode.MSG_MOVE_STOP_PITCH, ClientVersionBuild.Zero, ClientVersionBuild.V4_3_4_15595)]
        [Parser(Opcode.MSG_MOVE_SET_RUN_MODE, ClientVersionBuild.Zero, ClientVersionBuild.V4_3_4_15595)]
        [Parser(Opcode.MSG_MOVE_SET_WALK_MODE, ClientVersionBuild.Zero, ClientVersionBuild.V4_3_4_15595)]
        [Parser(Opcode.MSG_MOVE_TELEPORT, ClientVersionBuild.Zero, ClientVersionBuild.V4_2_2_14545)]
        [Parser(Opcode.MSG_MOVE_SET_FACING, ClientVersionBuild.Zero, ClientVersionBuild.V4_2_2_14545)]
        [Parser(Opcode.MSG_MOVE_SET_PITCH, ClientVersionBuild.Zero, ClientVersionBuild.V4_2_2_14545)]
        [Parser(Opcode.MSG_MOVE_TOGGLE_COLLISION_CHEAT)]
        [Parser(Opcode.MSG_MOVE_GRAVITY_CHNG)]
        [Parser(Opcode.MSG_MOVE_ROOT)]
        [Parser(Opcode.MSG_MOVE_UNROOT)]
        [Parser(Opcode.MSG_MOVE_START_SWIM, ClientVersionBuild.Zero, ClientVersionBuild.V4_3_4_15595)]
        [Parser(Opcode.MSG_MOVE_STOP_SWIM, ClientVersionBuild.Zero, ClientVersionBuild.V4_3_4_15595)]
        [Parser(Opcode.MSG_MOVE_START_SWIM_CHEAT)]
        [Parser(Opcode.MSG_MOVE_STOP_SWIM_CHEAT)]
        [Parser(Opcode.MSG_MOVE_HEARTBEAT, ClientVersionBuild.Zero, ClientVersionBuild.V4_2_2_14545)]
        [Parser(Opcode.MSG_MOVE_FALL_LAND, ClientVersionBuild.Zero, ClientVersionBuild.V4_3_4_15595)]
        [Parser(Opcode.MSG_MOVE_UPDATE_CAN_FLY)]
        [Parser(Opcode.MSG_MOVE_UPDATE_CAN_TRANSITION_BETWEEN_SWIM_AND_FLY)]
        [Parser(Opcode.MSG_MOVE_KNOCK_BACK)]
        [Parser(Opcode.MSG_MOVE_HOVER)]
        [Parser(Opcode.MSG_MOVE_FEATHER_FALL)]
        [Parser(Opcode.MSG_MOVE_WATER_WALK)]
        [Parser(Opcode.CMSG_MOVE_FALL_RESET, ClientVersionBuild.Zero, ClientVersionBuild.V4_3_4_15595)]
        [Parser(Opcode.CMSG_MOVE_SET_FLY)]
        [Parser(Opcode.CMSG_MOVE_CHANGE_TRANSPORT, ClientVersionBuild.Zero, ClientVersionBuild.V4_3_4_15595)]
        [Parser(Opcode.CMSG_MOVE_NOT_ACTIVE_MOVER, ClientVersionBuild.Zero, ClientVersionBuild.V4_3_4_15595)]
        [Parser(Opcode.CMSG_DISMISS_CONTROLLED_VEHICLE, ClientVersionBuild.Zero, ClientVersionBuild.V4_3_4_15595)]
        public static void HandleMovementMessages(Packet packet)
        {
            WowGuid moverGuid;
            if ((ClientVersion.AddedInVersion(ClientVersionBuild.V3_2_0_10192) ||
                packet.Direction == Direction.ServerToClient) && ClientVersion.Build != ClientVersionBuild.V4_2_2_14545)
                moverGuid = packet.ReadPackedGuid("Guid");
            else
                moverGuid = Storage.CurrentActivePlayer != null ? Storage.CurrentActivePlayer : WowGuid64.Empty;

            MovementInfo moveInfo = ReadMovementInfo(packet, moverGuid);

            Storage.StorePlayerMovement(moverGuid, moveInfo, packet);

            if (packet.Opcode != Opcodes.GetOpcode(Opcode.MSG_MOVE_KNOCK_BACK, Direction.Bidirectional))
                return;

            packet.ReadSingle("Sin Angle");
            packet.ReadSingle("Cos Angle");
            packet.ReadSingle("Horizontal Speed");
            packet.ReadSingle("Vertical Speed");
        }

        [Parser(Opcode.CMSG_MOVE_SPLINE_DONE, ClientVersionBuild.Zero, ClientVersionBuild.V4_3_4_15595)]
        public static void HandleMoveSplineDone(Packet packet)
        {
            var guid = ClientVersion.AddedInVersion(ClientVersionBuild.V3_0_2_9056) ? packet.ReadPackedGuid("Guid") : Storage.CurrentActivePlayer;
            ReadMovementInfo(packet, guid);
            packet.ReadInt32("Move Ticks"); // Possibly
            if (ClientVersion.RemovedInVersion(ClientVersionBuild.V2_0_1_6180))
                packet.ReadSingle("Spline Type");
        }

        [Parser(Opcode.SMSG_MOVE_SPLINE_SET_RUN_SPEED, ClientVersionBuild.V4_2_2_14545, ClientVersionBuild.V4_3_0_15005)]
        public static void HandleSplineMovementSetRunSpeed422(Packet packet)
        {
            var guid = packet.StartBitStream(7, 2, 1, 3, 5, 6, 4, 0);
            packet.ParseBitStream(guid, 6, 7, 4, 3, 2, 5, 0, 1);
            CreatureSpeedUpdate speedUpdate = new CreatureSpeedUpdate();
            speedUpdate.SpeedType = SpeedType.Run;
            speedUpdate.SpeedRate = packet.ReadSingle("Speed") / MovementInfo.DEFAULT_RUN_SPEED;
            WowGuid moverGuid = packet.WriteGuid("Guid", guid);
            speedUpdate.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(packet.Time);
            Storage.StoreUnitSpeedUpdate(moverGuid, speedUpdate);
        }

        [Parser(Opcode.SMSG_MOVE_SPLINE_SET_RUN_SPEED, ClientVersionBuild.V4_3_0_15005, ClientVersionBuild.V4_3_4_15595)]
        public static void HandleSplineMovementSetRunSpeed430(Packet packet)
        {
            var guid = packet.StartBitStream(2, 6, 4, 1, 3, 0, 7, 5);
            packet.ParseBitStream(guid, 2, 4, 3);
            CreatureSpeedUpdate speedUpdate = new CreatureSpeedUpdate();
            speedUpdate.SpeedType = SpeedType.Run;
            speedUpdate.SpeedRate = packet.ReadSingle("Speed") / MovementInfo.DEFAULT_RUN_SPEED;
            packet.ParseBitStream(guid, 0, 6, 5, 1, 7);
            WowGuid moverGuid = packet.WriteGuid("Guid", guid);
            speedUpdate.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(packet.Time);
            Storage.StoreUnitSpeedUpdate(moverGuid, speedUpdate);
        }

        [Parser(Opcode.SMSG_MOVE_SET_RUN_SPEED, ClientVersionBuild.V5_1_0_16309)]
        public static void HandleMoveSetRunSpeed510(Packet packet)
        {
            var guid = packet.StartBitStream(0, 4, 1, 6, 3, 5, 7, 2);
            CreatureSpeedUpdate speedUpdate = new CreatureSpeedUpdate();
            speedUpdate.SpeedType = SpeedType.Run;
            speedUpdate.SpeedRate = packet.ReadSingle("Speed") / MovementInfo.DEFAULT_RUN_SPEED;
            packet.ReadXORByte(guid, 7);
            packet.ReadInt32("Unk Int32");
            packet.ParseBitStream(guid, 3, 6, 0, 4, 1, 5, 2);
            WowGuid moverGuid = packet.WriteGuid("Guid", guid);
            speedUpdate.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(packet.Time);
            Storage.StoreUnitSpeedUpdate(moverGuid, speedUpdate);
        }

        [Parser(Opcode.MSG_MOVE_SET_RUN_SPEED, ClientVersionBuild.V4_2_2_14545)]
        public static void HandleMovementSetRunSpeed422(Packet packet)
        {
            var guid = packet.StartBitStream(1, 0, 7, 5, 2, 4, 3, 6);
            packet.ParseBitStream(guid, 1);
            CreatureSpeedUpdate speedUpdate = new CreatureSpeedUpdate();
            speedUpdate.SpeedType = SpeedType.Run;
            speedUpdate.SpeedRate = packet.ReadSingle("Speed") / MovementInfo.DEFAULT_RUN_SPEED;
            packet.ParseBitStream(guid, 6, 2, 3, 7, 4, 0, 5);
            packet.ReadUInt32("Move Event");
            WowGuid moverGuid = packet.WriteGuid("Guid", guid);
            speedUpdate.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(packet.Time);
            Storage.StoreUnitSpeedUpdate(moverGuid, speedUpdate);
        }

        [Parser(Opcode.MSG_MOVE_SET_WALK_SPEED)]
        public static void HandleMovementSetWalkSpeed(Packet packet)
        {
            var guid = packet.ReadPackedGuid("GUID");
            ReadMovementInfo(packet, guid);
            CreatureSpeedUpdate speedUpdate = new CreatureSpeedUpdate();
            speedUpdate.SpeedType = SpeedType.Walk;
            speedUpdate.SpeedRate = packet.ReadSingle("Speed") / MovementInfo.DEFAULT_WALK_SPEED;
            speedUpdate.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(packet.Time);
            Storage.StoreUnitSpeedUpdate(guid, speedUpdate);
        }
        [Parser(Opcode.MSG_MOVE_SET_RUN_SPEED, ClientVersionBuild.Zero, ClientVersionBuild.V4_2_2_14545)]
        public static void HandleMovementSetRunSpeed(Packet packet)
        {
            var guid = packet.ReadPackedGuid("GUID");
            ReadMovementInfo(packet, guid);
            CreatureSpeedUpdate speedUpdate = new CreatureSpeedUpdate();
            speedUpdate.SpeedType = SpeedType.Run;
            speedUpdate.SpeedRate = packet.ReadSingle("Speed") / MovementInfo.DEFAULT_RUN_SPEED;
            speedUpdate.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(packet.Time);
            Storage.StoreUnitSpeedUpdate(guid, speedUpdate);
        }
        [Parser(Opcode.MSG_MOVE_SET_RUN_BACK_SPEED)]
        public static void HandleMovementSetRunBackSpeed(Packet packet)
        {
            var guid = packet.ReadPackedGuid("GUID");
            ReadMovementInfo(packet, guid);
            CreatureSpeedUpdate speedUpdate = new CreatureSpeedUpdate();
            speedUpdate.SpeedType = SpeedType.RunBack;
            speedUpdate.SpeedRate = packet.ReadSingle("Speed") / MovementInfo.DEFAULT_RUN_BACK_SPEED;
            speedUpdate.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(packet.Time);
            Storage.StoreUnitSpeedUpdate(guid, speedUpdate);
        }
        [Parser(Opcode.MSG_MOVE_SET_SWIM_SPEED)]
        public static void HandleMovementSetSwimSpeed(Packet packet)
        {
            var guid = packet.ReadPackedGuid("GUID");
            ReadMovementInfo(packet, guid);
            CreatureSpeedUpdate speedUpdate = new CreatureSpeedUpdate();
            speedUpdate.SpeedType = SpeedType.Swim;
            speedUpdate.SpeedRate = packet.ReadSingle("Speed") / MovementInfo.DEFAULT_SWIM_SPEED;
            speedUpdate.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(packet.Time);
            Storage.StoreUnitSpeedUpdate(guid, speedUpdate);
        }
        [Parser(Opcode.MSG_MOVE_SET_SWIM_BACK_SPEED)]
        public static void HandleMovementSetSwimBackSpeed(Packet packet)
        {
            var guid = packet.ReadPackedGuid("GUID");
            ReadMovementInfo(packet, guid);
            CreatureSpeedUpdate speedUpdate = new CreatureSpeedUpdate();
            speedUpdate.SpeedType = SpeedType.SwimBack;
            speedUpdate.SpeedRate = packet.ReadSingle("Speed") / MovementInfo.DEFAULT_SWIM_BACK_SPEED;
            speedUpdate.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(packet.Time);
            Storage.StoreUnitSpeedUpdate(guid, speedUpdate);
        }
        [Parser(Opcode.MSG_MOVE_SET_TURN_RATE)]
        public static void HandleMovementSetTurnRate(Packet packet)
        {
            var guid = packet.ReadPackedGuid("GUID");
            ReadMovementInfo(packet, guid);
            CreatureSpeedUpdate speedUpdate = new CreatureSpeedUpdate();
            speedUpdate.SpeedType = SpeedType.Turn;
            speedUpdate.SpeedRate = packet.ReadSingle("Speed") / MovementInfo.DEFAULT_TURN_RATE;
            speedUpdate.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(packet.Time);
            Storage.StoreUnitSpeedUpdate(guid, speedUpdate);
        }
        [Parser(Opcode.MSG_MOVE_SET_FLIGHT_SPEED)]
        public static void HandleMovementSetFlightSpeed(Packet packet)
        {
            var guid = packet.ReadPackedGuid("GUID");
            ReadMovementInfo(packet, guid);
            CreatureSpeedUpdate speedUpdate = new CreatureSpeedUpdate();
            speedUpdate.SpeedType = SpeedType.Fly;
            speedUpdate.SpeedRate = packet.ReadSingle("Speed") / MovementInfo.DEFAULT_FLY_SPEED;
            speedUpdate.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(packet.Time);
            Storage.StoreUnitSpeedUpdate(guid, speedUpdate);
        }
        [Parser(Opcode.MSG_MOVE_SET_FLIGHT_BACK_SPEED)]
        public static void HandleMovementSetFlightBackSpeed(Packet packet)
        {
            var guid = packet.ReadPackedGuid("GUID");
            ReadMovementInfo(packet, guid);
            CreatureSpeedUpdate speedUpdate = new CreatureSpeedUpdate();
            speedUpdate.SpeedType = SpeedType.FlyBack;
            speedUpdate.SpeedRate = packet.ReadSingle("Speed") / MovementInfo.DEFAULT_FLY_BACK_SPEED;
            speedUpdate.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(packet.Time);
            Storage.StoreUnitSpeedUpdate(guid, speedUpdate);
        }
        [Parser(Opcode.MSG_MOVE_SET_PITCH_RATE)]
        public static void HandleMovementSetPitchRate(Packet packet)
        {
            var guid = packet.ReadPackedGuid("GUID");
            ReadMovementInfo(packet, guid);
            CreatureSpeedUpdate speedUpdate = new CreatureSpeedUpdate();
            speedUpdate.SpeedType = SpeedType.Pitch;
            speedUpdate.SpeedRate = packet.ReadSingle("Speed") / MovementInfo.DEFAULT_PITCH_RATE;
            speedUpdate.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(packet.Time);
            Storage.StoreUnitSpeedUpdate(guid, speedUpdate);
        }

        [Parser(Opcode.SMSG_FORCE_WALK_SPEED_CHANGE)]
        public static void HandleForceWalkSpeedChange(Packet packet)
        {
            WowGuid guid = packet.ReadPackedGuid("Guid");
            packet.ReadUInt32("Movement Counter");
            CreatureSpeedUpdate speedUpdate = new CreatureSpeedUpdate();
            speedUpdate.SpeedType = SpeedType.Walk;
            speedUpdate.SpeedRate = packet.ReadSingle("New Speed") / MovementInfo.DEFAULT_WALK_SPEED;
            speedUpdate.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(packet.Time);
            Storage.StoreUnitSpeedUpdate(guid, speedUpdate);
        }
        [Parser(Opcode.SMSG_FORCE_RUN_SPEED_CHANGE)]
        public static void HandleForceRunSpeedChange(Packet packet)
        {
            WowGuid guid = packet.ReadPackedGuid("Guid");
            packet.ReadUInt32("Movement Counter");

            if (ClientVersion.AddedInVersion(ClientVersionBuild.V2_0_1_6180))
                packet.ReadByte("Unk Byte");

            CreatureSpeedUpdate speedUpdate = new CreatureSpeedUpdate();
            speedUpdate.SpeedType = SpeedType.Run;
            speedUpdate.SpeedRate = packet.ReadSingle("New Speed") / MovementInfo.DEFAULT_RUN_SPEED;
            speedUpdate.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(packet.Time);
            Storage.StoreUnitSpeedUpdate(guid, speedUpdate);
        }
        [Parser(Opcode.SMSG_FORCE_RUN_BACK_SPEED_CHANGE)]
        public static void HandleForceRunBackSpeedChange(Packet packet)
        {
            WowGuid guid = packet.ReadPackedGuid("Guid");
            packet.ReadUInt32("Movement Counter");
            CreatureSpeedUpdate speedUpdate = new CreatureSpeedUpdate();
            speedUpdate.SpeedType = SpeedType.RunBack;
            speedUpdate.SpeedRate = packet.ReadSingle("New Speed") / MovementInfo.DEFAULT_RUN_BACK_SPEED;
            speedUpdate.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(packet.Time);
            Storage.StoreUnitSpeedUpdate(guid, speedUpdate);
        }
        [Parser(Opcode.SMSG_FORCE_SWIM_SPEED_CHANGE)]
        public static void HandleForceSwimSpeedChange(Packet packet)
        {
            WowGuid guid = packet.ReadPackedGuid("Guid");
            packet.ReadUInt32("Movement Counter");
            CreatureSpeedUpdate speedUpdate = new CreatureSpeedUpdate();
            speedUpdate.SpeedType = SpeedType.Swim;
            speedUpdate.SpeedRate = packet.ReadSingle("New Speed") / MovementInfo.DEFAULT_SWIM_SPEED;
            speedUpdate.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(packet.Time);
            Storage.StoreUnitSpeedUpdate(guid, speedUpdate);
        }
        [Parser(Opcode.SMSG_FORCE_SWIM_BACK_SPEED_CHANGE)]
        public static void HandleForceSwimBackSpeedChange(Packet packet)
        {
            WowGuid guid = packet.ReadPackedGuid("Guid");
            packet.ReadUInt32("Movement Counter");
            CreatureSpeedUpdate speedUpdate = new CreatureSpeedUpdate();
            speedUpdate.SpeedType = SpeedType.SwimBack;
            speedUpdate.SpeedRate = packet.ReadSingle("New Speed") / MovementInfo.DEFAULT_SWIM_BACK_SPEED;
            speedUpdate.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(packet.Time);
            Storage.StoreUnitSpeedUpdate(guid, speedUpdate);
        }
        [Parser(Opcode.SMSG_FORCE_TURN_RATE_CHANGE)]
        public static void HandleForceTurnRateChange(Packet packet)
        {
            WowGuid guid = packet.ReadPackedGuid("Guid");
            packet.ReadUInt32("Movement Counter");
            CreatureSpeedUpdate speedUpdate = new CreatureSpeedUpdate();
            speedUpdate.SpeedType = SpeedType.Turn;
            speedUpdate.SpeedRate = packet.ReadSingle("New Speed") / MovementInfo.DEFAULT_TURN_RATE;
            speedUpdate.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(packet.Time);
            Storage.StoreUnitSpeedUpdate(guid, speedUpdate);
        }
        [Parser(Opcode.SMSG_FORCE_FLIGHT_SPEED_CHANGE)]
        public static void HandleForceFlightSpeedChange(Packet packet)
        {
            WowGuid guid = packet.ReadPackedGuid("Guid");
            packet.ReadUInt32("Movement Counter");
            CreatureSpeedUpdate speedUpdate = new CreatureSpeedUpdate();
            speedUpdate.SpeedType = SpeedType.Fly;
            speedUpdate.SpeedRate = packet.ReadSingle("New Speed") / MovementInfo.DEFAULT_FLY_SPEED;
            speedUpdate.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(packet.Time);
            Storage.StoreUnitSpeedUpdate(guid, speedUpdate);
        }
        [Parser(Opcode.SMSG_FORCE_FLIGHT_BACK_SPEED_CHANGE)]
        public static void HandleForceFlightBackSpeedChange(Packet packet)
        {
            WowGuid guid = packet.ReadPackedGuid("Guid");
            packet.ReadUInt32("Movement Counter");
            CreatureSpeedUpdate speedUpdate = new CreatureSpeedUpdate();
            speedUpdate.SpeedType = SpeedType.FlyBack;
            speedUpdate.SpeedRate = packet.ReadSingle("New Speed") / MovementInfo.DEFAULT_FLY_BACK_SPEED;
            speedUpdate.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(packet.Time);
            Storage.StoreUnitSpeedUpdate(guid, speedUpdate);
        }
        [Parser(Opcode.SMSG_FORCE_PITCH_RATE_CHANGE)]
        public static void HandleForcePitchRateChange(Packet packet)
        {
            WowGuid guid = packet.ReadPackedGuid("Guid");
            packet.ReadUInt32("Movement Counter");
            CreatureSpeedUpdate speedUpdate = new CreatureSpeedUpdate();
            speedUpdate.SpeedType = SpeedType.Pitch;
            speedUpdate.SpeedRate = packet.ReadSingle("New Speed") / MovementInfo.DEFAULT_PITCH_RATE;
            speedUpdate.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(packet.Time);
            Storage.StoreUnitSpeedUpdate(guid, speedUpdate);
        }

        [Parser(Opcode.CMSG_FORCE_RUN_SPEED_CHANGE_ACK)]
        [Parser(Opcode.CMSG_FORCE_RUN_BACK_SPEED_CHANGE_ACK)]
        [Parser(Opcode.CMSG_FORCE_SWIM_SPEED_CHANGE_ACK)]
        [Parser(Opcode.CMSG_FORCE_SWIM_BACK_SPEED_CHANGE_ACK)]
        [Parser(Opcode.CMSG_FORCE_WALK_SPEED_CHANGE_ACK)]
        [Parser(Opcode.CMSG_FORCE_TURN_RATE_CHANGE_ACK)]
        [Parser(Opcode.CMSG_FORCE_FLIGHT_SPEED_CHANGE_ACK)]
        [Parser(Opcode.CMSG_FORCE_FLIGHT_BACK_SPEED_CHANGE_ACK)]
        [Parser(Opcode.CMSG_FORCE_PITCH_RATE_CHANGE_ACK)]
        public static void HandleSpeedChangeMessage(Packet packet)
        {
            var guid = ClientVersion.AddedInVersion(ClientVersionBuild.V2_0_1_6180) ? packet.ReadPackedGuid("Guid") : packet.ReadGuid("Guid");
            packet.ReadInt32("Movement Counter");

            ReadMovementInfo(packet, guid);

            packet.ReadSingle("New Speed");
        }

        [Parser(Opcode.MSG_MOVE_SET_COLLISION_HGT)]
        [Parser(Opcode.SMSG_MOVE_SET_COLLISION_HGT)]
        [Parser(Opcode.CMSG_MOVE_SET_COLLISION_HGT_ACK)]
        public static void HandleCollisionMovements(Packet packet)
        {
            var guid = packet.ReadPackedGuid("Guid");

            if (packet.Opcode != Opcodes.GetOpcode(Opcode.MSG_MOVE_SET_COLLISION_HGT, Direction.Bidirectional))
                packet.ReadInt32("Movement Counter");

            if (packet.Opcode != Opcodes.GetOpcode(Opcode.SMSG_MOVE_SET_COLLISION_HGT, Direction.ServerToClient))
                ReadMovementInfo(packet, guid);

            packet.ReadSingle("Collision Height");
        }

        [Parser(Opcode.CMSG_SET_ACTIVE_MOVER, ClientVersionBuild.Zero, ClientVersionBuild.V4_3_0_15005)]
        [Parser(Opcode.SMSG_MOUNT_SPECIAL_ANIM)]
        public static void HandleSetActiveMover(Packet packet)
        {
            packet.ReadGuid("GUID");
        }

        [Parser(Opcode.CMSG_SET_ACTIVE_MOVER, ClientVersionBuild.V4_3_0_15005, ClientVersionBuild.V4_3_4_15595)]
        public static void HandleSetActiveMover430(Packet packet)
        {
            var guid = packet.StartBitStream(7, 2, 0, 4, 3, 5, 6, 1);
            packet.ParseBitStream(guid, 1, 3, 2, 6, 0, 5, 4, 7);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_FORCE_MOVE_ROOT)]
        [Parser(Opcode.SMSG_FORCE_MOVE_UNROOT)]
        [Parser(Opcode.SMSG_MOVE_SET_WATER_WALK)]
        [Parser(Opcode.SMSG_MOVE_SET_LAND_WALK)]
        [Parser(Opcode.SMSG_MOVE_SET_HOVERING)]
        [Parser(Opcode.SMSG_MOVE_UNSET_HOVERING)]
        [Parser(Opcode.SMSG_MOVE_SET_CAN_FLY)]
        [Parser(Opcode.SMSG_MOVE_UNSET_CAN_FLY)]
        [Parser(Opcode.SMSG_MOVE_ENABLE_TRANSITION_BETWEEN_SWIM_AND_FLY)]
        [Parser(Opcode.SMSG_MOVE_DISABLE_TRANSITION_BETWEEN_SWIM_AND_FLY)]
        [Parser(Opcode.SMSG_MOVE_DISABLE_GRAVITY)]
        [Parser(Opcode.SMSG_MOVE_ENABLE_GRAVITY)]
        [Parser(Opcode.SMSG_MOVE_SET_FEATHER_FALL)]
        [Parser(Opcode.SMSG_MOVE_SET_NORMAL_FALL, ClientVersionBuild.Zero, ClientVersionBuild.V4_3_4_15595)]
        public static void HandleSetMovementMessages(Packet packet)
        {
            packet.ReadPackedGuid("Guid");
            packet.ReadInt32("Movement Counter");
        }

        [Parser(Opcode.CMSG_MOVE_WATER_WALK_ACK, ClientVersionBuild.Zero, ClientVersionBuild.V4_3_4_15595)]
        [Parser(Opcode.CMSG_MOVE_FEATHER_FALL_ACK, ClientVersionBuild.Zero, ClientVersionBuild.V4_3_4_15595)]
        [Parser(Opcode.CMSG_MOVE_HOVER_ACK, ClientVersionBuild.Zero, ClientVersionBuild.V4_3_4_15595)]
        [Parser(Opcode.CMSG_MOVE_SET_CAN_FLY_ACK, ClientVersionBuild.Zero, ClientVersionBuild.V4_3_4_15595)]
        [Parser(Opcode.CMSG_MOVE_SET_CAN_TRANSITION_BETWEEN_SWIM_AND_FLY_ACK, ClientVersionBuild.Zero, ClientVersionBuild.V4_3_4_15595)]
        public static void HandleSpecialMoveAckMessages(Packet packet)
        {
            WowGuid guid;
            if (ClientVersion.RemovedInVersion(ClientVersionBuild.V3_0_2_9056))
                guid = packet.ReadGuid("Guid");
            else
                guid = packet.ReadPackedGuid("Guid");

            packet.ReadInt32("Movement Counter");
            ReadMovementInfo(packet, guid);
            packet.ReadInt32("Apply");
        }

        [Parser(Opcode.CMSG_MOVE_KNOCK_BACK_ACK, ClientVersionBuild.Zero, ClientVersionBuild.V4_3_4_15595)]
        [Parser(Opcode.CMSG_FORCE_MOVE_UNROOT_ACK, ClientVersionBuild.Zero, ClientVersionBuild.V4_3_4_15595)]
        [Parser(Opcode.CMSG_FORCE_MOVE_ROOT_ACK, ClientVersionBuild.Zero, ClientVersionBuild.V4_3_4_15595)]
        [Parser(Opcode.CMSG_MOVE_GRAVITY_ENABLE_ACK, ClientVersionBuild.Zero, ClientVersionBuild.V4_3_4_15595)]
        [Parser(Opcode.CMSG_MOVE_GRAVITY_DISABLE_ACK, ClientVersionBuild.Zero, ClientVersionBuild.V4_3_4_15595)]
        public static void HandleSpecialMoveAckMessages2(Packet packet)
        {
            WowGuid guid;
            if (ClientVersion.Build < ClientVersionBuild.V3_0_2_9056)
                guid = packet.ReadGuid("Guid");
            else
                guid = packet.ReadPackedGuid("Guid");

            packet.ReadInt32("Movement Counter");

            ReadMovementInfo(packet, guid);
        }

        [HasSniffData]
        [Parser(Opcode.SMSG_PHASE_SHIFT_CHANGE, ClientVersionBuild.Zero, ClientVersionBuild.V4_0_6a_13623)]
        public static void HandlePhaseShift(Packet packet)
        {
            CurrentPhaseMask = packet.ReadInt32("Phase Mask");

            packet.AddSniffData(StoreNameType.Phase, CurrentPhaseMask, "PHASEMASK");
        }

        [HasSniffData]
        [Parser(Opcode.SMSG_PHASE_SHIFT_CHANGE, ClientVersionBuild.V4_0_6a_13623, ClientVersionBuild.V4_1_0_13914)]
        public static void HandlePhaseShift406(Packet packet)
        {
            packet.ReadGuid("GUID");
            var i = 0;
            int count = packet.ReadInt32("Count");
            for (var j = 0; j < count / 2; ++j)
                packet.ReadInt16<MapId>("Unk", i, j);

            i++;
            count = packet.ReadInt32();
            for (var j = 0; j < count / 2; ++j)
                packet.ReadInt16<MapId>("Terrain Swap 1", i, j);

            i++;
            count = packet.ReadInt32();
            var phaseMask = 0;
            for (var j = 0; j < count / 2; ++j)
                phaseMask = packet.ReadInt16("Phases", ++i, j);

            i++;
            count = packet.ReadInt32();
            for (var j = 0; j < count / 2; ++j)
                packet.ReadInt16<MapId>("Terrain Swap 2", i, j);

            packet.ReadUInt32("Flag"); // can be 0, 4 or 8, 8 = normal world, others are unknown

            //CurrentPhaseMask = phaseMask;
            packet.AddSniffData(StoreNameType.Phase, phaseMask, "PHASEMASK 406");
        }

        [HasSniffData]
        [Parser(Opcode.SMSG_PHASE_SHIFT_CHANGE, ClientVersionBuild.V4_2_2_14545, ClientVersionBuild.V4_3_0_15005)]
        public static void HandlePhaseShift422(Packet packet)
        {
            var guid = new byte[8];

            guid[6] = packet.ReadBit();//0
            guid[1] = packet.ReadBit();//1
            guid[7] = packet.ReadBit();//2
            guid[4] = packet.ReadBit();//3
            guid[2] = packet.ReadBit();//4
            guid[3] = packet.ReadBit();//5
            guid[0] = packet.ReadBit();//6
            guid[5] = packet.ReadBit();//7

            packet.ReadXORByte(guid, 0);
            packet.ReadXORByte(guid, 4);

            var i = 0;
            var count = packet.ReadInt32();
            for (var j = 0; j < count / 2; ++j)
                packet.ReadInt16<MapId>("Map Swap 1", i, j);

            packet.ReadXORByte(guid, 3);

            packet.ReadUInt32("Mask");

            packet.ReadXORByte(guid, 2);

            var phaseMask = -1;
            count = packet.ReadInt32();
            for (var j = 0; j < count / 2; ++j)
                phaseMask = packet.ReadUInt16("Current Mask", i, j);

            packet.ReadXORByte(guid, 6);

            i++;
            count = packet.ReadInt32();
            for (var j = 0; j < count / 2; ++j)
                packet.ReadInt16<MapId>("Map Swap 1", i, j);

            packet.ReadXORByte(guid, 7);

            i++;
            count = packet.ReadInt32();
            for (var j = 0; j < count / 2; ++j)
                packet.ReadInt16<MapId>("Map Swap 3", i, j);

            packet.ReadXORByte(guid, 5);
            packet.ReadXORByte(guid, 1);

            packet.WriteGuid("Guid", guid);

            if (phaseMask != -1)
            {
                CurrentPhaseMask = phaseMask;
                packet.AddSniffData(StoreNameType.Phase, phaseMask, "PHASEMASK 422");
            }
        }

        [Parser(Opcode.SMSG_PHASE_SHIFT_CHANGE, ClientVersionBuild.V5_0_5_16048, ClientVersionBuild.V5_1_0_16309)]
        public static void HandlePhaseShift505(Packet packet)
        {
            ActivePhases.Clear();

            var count = packet.ReadUInt32() / 2;
            packet.AddValue("Inactive Terrain swap count", count);
            for (var i = 0; i < count; ++i)
                packet.ReadInt16<MapId>("Inactive Terrain swap", i);

            packet.ReadUInt32("UInt32");

            count = packet.ReadUInt32() / 2;
            packet.AddValue("Active Terrain swap count", count);
            for (var i = 0; i < count; ++i)
                packet.ReadInt16<MapId>("Active Terrain swap", i);

            count = packet.ReadUInt32() / 2;
            packet.AddValue("Phases count", count);
            for (var i = 0; i < count; ++i)
                ActivePhases.Add(packet.ReadUInt16("Phase id", i), true); // Phase.dbc

            count = packet.ReadUInt32() / 2;
            packet.AddValue("WorldMapArea swap count", count);
            for (var i = 0; i < count; ++i)
                packet.ReadUInt16("WorldMapArea swap", i);

            var guid = packet.StartBitStream(3, 7, 1, 6, 0, 4, 5, 2);
            packet.ParseBitStream(guid, 4, 3, 0, 6, 2, 7, 5, 1);

            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_PHASE_SHIFT_CHANGE, ClientVersionBuild.V5_1_0_16309)]
        public static void HandlePhaseShift510(Packet packet)
        {
            ActivePhases.Clear();

            var guid = packet.StartBitStream(6, 4, 7, 2, 0, 1, 3, 5);
            packet.ReadXORByte(guid, 4);

            var count = packet.ReadUInt32() / 2;
            packet.AddValue("WorldMapArea swap count", count);
            for (var i = 0; i < count; ++i)
                packet.ReadUInt16("WorldMapArea swap", i);

            packet.ReadXORByte(guid, 2);
            packet.ReadXORByte(guid, 3);

            count = packet.ReadUInt32() / 2;
            packet.AddValue("Phases count", count);
            for (var i = 0; i < count; ++i)
                ActivePhases.Add(packet.ReadUInt16("Phase id", i), true); // Phase.dbc

            packet.ReadXORByte(guid, 1);
            packet.ReadXORByte(guid, 6);

            count = packet.ReadUInt32() / 2;
            packet.AddValue("Active Terrain swap count", count);
            for (var i = 0; i < count; ++i)
                packet.ReadInt16<MapId>("Active Terrain swap", i);

            packet.ReadUInt32("UInt32");
            packet.ReadXORByte(guid, 0);
            packet.ReadXORByte(guid, 7);
            packet.ReadXORByte(guid, 5);

            count = packet.ReadUInt32() / 2;
            packet.AddValue("Inactive Terrain swap count", count);
            for (var i = 0; i < count; ++i)
                packet.ReadInt16<MapId>("Inactive Terrain swap", i);

            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_TRANSFER_PENDING, ClientVersionBuild.Zero, ClientVersionBuild.V4_3_0_15005)]
        public static void HandleTransferPending(Packet packet)
        {
            packet.ReadInt32<MapId>("Map ID");

            if (!packet.CanRead())
                return;

            packet.ReadInt32("Transport Entry");
            packet.ReadInt32<MapId>("Transport Map ID");
        }

        [Parser(Opcode.SMSG_TRANSFER_PENDING, ClientVersionBuild.V4_3_0_15005, ClientVersionBuild.V4_3_4_15595)]
        public static void HandleTransferPending430(Packet packet)
        {
            var bit1 = packet.ReadBit();
            var hasTransport = packet.ReadBit();

            if (bit1)
                packet.ReadUInt32("Unk int");

            if (hasTransport)
            {
                packet.ReadInt32<MapId>("Transport Map ID");
                packet.ReadInt32("Transport Entry");
            }

            packet.ReadInt32<MapId>("Map ID");
        }

        [Parser(Opcode.SMSG_TRANSFER_ABORTED)]
        public static void HandleTransferAborted(Packet packet)
        {
            packet.ReadInt32<MapId>("Map ID");

            var reason = packet.ReadByteE<TransferAbortReason>("Reason");

            switch (reason)
            {
                case TransferAbortReason.DifficultyUnavailable:
                {
                    packet.ReadByteE<MapDifficulty>("Difficulty");
                    break;
                }
                case TransferAbortReason.InsufficientExpansion:
                {
                    packet.ReadByteE<ClientType>("Expansion");
                    break;
                }
                case TransferAbortReason.UniqueMessage:
                {
                    packet.ReadByte("Message ID");
                    break;
                }
                default:
                    packet.ReadByte(); // Does nothing
                    break;
            }
        }

        [Parser(Opcode.SMSG_FLIGHT_SPLINE_SYNC)]
        public static void HandleFlightSplineSync(Packet packet)
        {
            packet.ReadSingle("Duration modifier");
            packet.ReadPackedGuid("GUID");
        }

        [Parser(Opcode.SMSG_CONTROL_UPDATE)]
        public static void HandleClientControlUpdate(Packet packet)
        {
            packet.ReadPackedGuid("GUID");
            packet.ReadByte("AllowMove");
        }

        [Parser(Opcode.SMSG_MOVE_KNOCK_BACK, ClientVersionBuild.Zero, ClientVersionBuild.V4_2_2_14545)]
        public static void HandleMoveKnockBack(Packet packet)
        {
            packet.ReadPackedGuid("GUID");
            packet.ReadUInt32("Movement Counter");
            packet.ReadSingle("X direction");
            packet.ReadSingle("Y direction");
            packet.ReadSingle("Horizontal Speed");
            packet.ReadSingle("Vertical Speed");
        }

        [Parser(Opcode.MSG_MOVE_TIME_SKIPPED)]
        public static void HandleMoveTimeSkippedMsg(Packet packet)
        {
            packet.ReadPackedGuid("Guid");
            packet.ReadUInt32("Time");
        }

        [Parser(Opcode.CMSG_MOVE_TIME_SKIPPED, ClientVersionBuild.Zero, ClientVersionBuild.V4_3_4_15595)]
        public static void HandleMoveTimeSkipped(Packet packet)
        {
            if (ClientVersion.Build < ClientVersionBuild.V3_0_2_9056)
                packet.ReadGuid("GUID");
            else
                packet.ReadPackedGuid("GUID");
            packet.ReadUInt32("Time");
        }

        [Parser(Opcode.SMSG_MOVE_SPLINE_ROOT, ClientVersionBuild.Zero, ClientVersionBuild.V4_3_4_15595)]
        [Parser(Opcode.SMSG_MOVE_SPLINE_UNROOT, ClientVersionBuild.Zero, ClientVersionBuild.V4_3_4_15595)]
        [Parser(Opcode.SMSG_MOVE_SPLINE_ENABLE_GRAVITY, ClientVersionBuild.Zero, ClientVersionBuild.V4_3_4_15595)]
        [Parser(Opcode.SMSG_MOVE_SPLINE_DISABLE_GRAVITY, ClientVersionBuild.Zero, ClientVersionBuild.V4_3_4_15595)]
        [Parser(Opcode.SMSG_MOVE_SPLINE_SET_FEATHER_FALL, ClientVersionBuild.Zero, ClientVersionBuild.V4_3_4_15595)]
        [Parser(Opcode.SMSG_MOVE_SPLINE_SET_NORMAL_FALL, ClientVersionBuild.Zero, ClientVersionBuild.V4_3_4_15595)]
        [Parser(Opcode.SMSG_MOVE_SPLINE_SET_HOVER, ClientVersionBuild.Zero, ClientVersionBuild.V4_3_4_15595)]
        [Parser(Opcode.SMSG_MOVE_SPLINE_UNSET_HOVER, ClientVersionBuild.Zero, ClientVersionBuild.V4_3_4_15595)]
        [Parser(Opcode.SMSG_MOVE_SPLINE_SET_WATER_WALK, ClientVersionBuild.Zero, ClientVersionBuild.V4_3_4_15595)]
        [Parser(Opcode.SMSG_MOVE_SPLINE_SET_LAND_WALK)]
        [Parser(Opcode.SMSG_MOVE_SPLINE_START_SWIM, ClientVersionBuild.Zero, ClientVersionBuild.V4_3_4_15595)]
        [Parser(Opcode.SMSG_MOVE_SPLINE_STOP_SWIM, ClientVersionBuild.Zero, ClientVersionBuild.V4_3_4_15595)]
        [Parser(Opcode.SMSG_MOVE_SPLINE_SET_RUN_MODE, ClientVersionBuild.Zero, ClientVersionBuild.V4_3_4_15595)]
        [Parser(Opcode.SMSG_MOVE_SPLINE_SET_WALK_MODE, ClientVersionBuild.Zero, ClientVersionBuild.V4_3_4_15595)]
        [Parser(Opcode.SMSG_MOVE_SPLINE_SET_FLYING, ClientVersionBuild.Zero, ClientVersionBuild.V4_3_4_15595)]
        [Parser(Opcode.SMSG_MOVE_SPLINE_UNSET_FLYING, ClientVersionBuild.Zero, ClientVersionBuild.V4_3_4_15595)]
        public static void HandleSplineMovementMessages(Packet packet)
        {
            packet.ReadPackedGuid("GUID");
        }

        [Parser(Opcode.SMSG_MOVE_SPLINE_SET_WALK_BACK_SPEED, ClientVersionBuild.Zero, ClientVersionBuild.V4_3_4_15595)]
        public static void HandleSplineMovementSetWalkBackSpeed(Packet packet)
        {
            WowGuid guid = packet.ReadPackedGuid("GUID");
            CreatureSpeedUpdate speedUpdate = new CreatureSpeedUpdate();
            speedUpdate.SpeedType = SpeedType.Walk;
            speedUpdate.SpeedRate = packet.ReadSingle("Amount") / MovementInfo.DEFAULT_WALK_SPEED;
            speedUpdate.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(packet.Time);
            Storage.StoreUnitSpeedUpdate(guid, speedUpdate);
        }
        [Parser(Opcode.SMSG_MOVE_SPLINE_SET_RUN_SPEED, ClientVersionBuild.Zero, ClientVersionBuild.V4_2_2_14545)]
        public static void HandleSplineMovementSetRunSpeed(Packet packet)
        {
            WowGuid guid = packet.ReadPackedGuid("GUID");
            CreatureSpeedUpdate speedUpdate = new CreatureSpeedUpdate();
            speedUpdate.SpeedType = SpeedType.Run;
            speedUpdate.SpeedRate = packet.ReadSingle("Amount") / MovementInfo.DEFAULT_RUN_SPEED;
            speedUpdate.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(packet.Time);
            Storage.StoreUnitSpeedUpdate(guid, speedUpdate);
        }
        [Parser(Opcode.SMSG_MOVE_SPLINE_SET_SWIM_SPEED, ClientVersionBuild.Zero, ClientVersionBuild.V4_3_4_15595)]
        public static void HandleSplineMovementSetSwimSpeed(Packet packet)
        {
            WowGuid guid = packet.ReadPackedGuid("GUID");
            CreatureSpeedUpdate speedUpdate = new CreatureSpeedUpdate();
            speedUpdate.SpeedType = SpeedType.Swim;
            speedUpdate.SpeedRate = packet.ReadSingle("Amount") / MovementInfo.DEFAULT_SWIM_SPEED;
            speedUpdate.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(packet.Time);
            Storage.StoreUnitSpeedUpdate(guid, speedUpdate);
        }
        [Parser(Opcode.SMSG_MOVE_SPLINE_SET_FLIGHT_SPEED, ClientVersionBuild.Zero, ClientVersionBuild.V4_3_4_15595)]
        public static void HandleSplineMovementSetFlightSpeed(Packet packet)
        {
            WowGuid guid = packet.ReadPackedGuid("GUID");
            CreatureSpeedUpdate speedUpdate = new CreatureSpeedUpdate();
            speedUpdate.SpeedType = SpeedType.Fly;
            speedUpdate.SpeedRate = packet.ReadSingle("Amount") / MovementInfo.DEFAULT_FLY_SPEED;
            speedUpdate.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(packet.Time);
            Storage.StoreUnitSpeedUpdate(guid, speedUpdate);
        }
        [Parser(Opcode.SMSG_MOVE_SPLINE_SET_RUN_BACK_SPEED, ClientVersionBuild.Zero, ClientVersionBuild.V4_3_4_15595)]
        public static void HandleSplineMovementSetRunBackSpeed(Packet packet)
        {
            WowGuid guid = packet.ReadPackedGuid("GUID");
            CreatureSpeedUpdate speedUpdate = new CreatureSpeedUpdate();
            speedUpdate.SpeedType = SpeedType.RunBack;
            speedUpdate.SpeedRate = packet.ReadSingle("Amount") / MovementInfo.DEFAULT_RUN_BACK_SPEED;
            speedUpdate.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(packet.Time);
            Storage.StoreUnitSpeedUpdate(guid, speedUpdate);
        }
        [Parser(Opcode.SMSG_MOVE_SPLINE_SET_SWIM_BACK_SPEED, ClientVersionBuild.Zero, ClientVersionBuild.V4_3_4_15595)]
        public static void HandleSplineMovementSetSwimBackSpeed(Packet packet)
        {
            WowGuid guid = packet.ReadPackedGuid("GUID");
            CreatureSpeedUpdate speedUpdate = new CreatureSpeedUpdate();
            speedUpdate.SpeedType = SpeedType.SwimBack;
            speedUpdate.SpeedRate = packet.ReadSingle("Amount") / MovementInfo.DEFAULT_SWIM_BACK_SPEED;
            speedUpdate.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(packet.Time);
            Storage.StoreUnitSpeedUpdate(guid, speedUpdate);
        }
        [Parser(Opcode.SMSG_MOVE_SPLINE_SET_FLIGHT_BACK_SPEED, ClientVersionBuild.Zero, ClientVersionBuild.V4_3_4_15595)]
        public static void HandleSplineMovementSetFlightBackSpeed(Packet packet)
        {
            WowGuid guid = packet.ReadPackedGuid("GUID");
            CreatureSpeedUpdate speedUpdate = new CreatureSpeedUpdate();
            speedUpdate.SpeedType = SpeedType.FlyBack;
            speedUpdate.SpeedRate = packet.ReadSingle("Amount") / MovementInfo.DEFAULT_FLY_BACK_SPEED;
            speedUpdate.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(packet.Time);
            Storage.StoreUnitSpeedUpdate(guid, speedUpdate);
        }
        [Parser(Opcode.SMSG_MOVE_SPLINE_SET_TURN_RATE, ClientVersionBuild.Zero, ClientVersionBuild.V4_3_4_15595)]
        public static void HandleSplineMovementSetTurnRate(Packet packet)
        {
            WowGuid guid = packet.ReadPackedGuid("GUID");
            CreatureSpeedUpdate speedUpdate = new CreatureSpeedUpdate();
            speedUpdate.SpeedType = SpeedType.Turn;
            speedUpdate.SpeedRate = packet.ReadSingle("Amount") / MovementInfo.DEFAULT_TURN_RATE;
            speedUpdate.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(packet.Time);
            Storage.StoreUnitSpeedUpdate(guid, speedUpdate);
        }
        [Parser(Opcode.SMSG_MOVE_SPLINE_SET_PITCH_RATE, ClientVersionBuild.Zero, ClientVersionBuild.V4_3_4_15595)]
        public static void HandleSplineMovementSetPitchRate(Packet packet)
        {
            WowGuid guid = packet.ReadPackedGuid("GUID");
            CreatureSpeedUpdate speedUpdate = new CreatureSpeedUpdate();
            speedUpdate.SpeedType = SpeedType.Pitch;
            speedUpdate.SpeedRate = packet.ReadSingle("Amount") / MovementInfo.DEFAULT_PITCH_RATE;
            speedUpdate.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(packet.Time);
            Storage.StoreUnitSpeedUpdate(guid, speedUpdate);
        }

        [Parser(Opcode.SMSG_COMPRESSED_MOVES)]
        [Parser(Opcode.SMSG_MULTIPLE_MOVES)]
        public static void HandleCompressedMoves(Packet packet)
        {
            var uncompressedSize = packet.ReadInt32("Data Size");

            packet.WriteLine("{"); // To be able to see what is inside this packet.
            packet.WriteLine();

            Packet pkt;
            if (packet.Opcode == Opcodes.GetOpcode(Opcode.SMSG_COMPRESSED_MOVES, Direction.ServerToClient))
                pkt = packet.Inflate(uncompressedSize);
            else
                pkt = packet;

            while (pkt.CanRead())
            {
                var size = pkt.ReadByte();
                var opc = pkt.ReadInt16();
                var data = pkt.ReadBytes(size - 2);

                using (var newPacket = new Packet(data, opc, pkt.Time, pkt.Direction, pkt.Number, packet.Writer, packet.FileName))
                    Handler.Parse(newPacket, true);
                packet.WriteLine();
            }

            packet.WriteLine("}");
            packet.ReadToEnd();
        }

        [Parser(Opcode.SMSG_MOVE_KNOCK_BACK, ClientVersionBuild.V4_2_2_14545, ClientVersionBuild.V4_3_4_15595)]
        public static void HandleMoveKnockBack422(Packet packet)
        {
            var guid = packet.StartBitStream(5, 2, 6, 3, 1, 4, 0, 7);

            packet.ReadXORByte(guid, 0);

            packet.ReadSingle("Jump Horizontal Speed");
            packet.ReadUInt32("Jump Fall Time");
            packet.ReadSingle("Jump Vertical Speed");

            packet.ReadXORByte(guid, 6);

            packet.ReadSingle("Jump Cos");
            packet.ReadSingle("Jump Sin");

            packet.ReadXORByte(guid, 3);
            packet.ReadXORByte(guid, 1);
            packet.ReadXORByte(guid, 2);
            packet.ReadXORByte(guid, 4);
            packet.ReadXORByte(guid, 7);
            packet.ReadXORByte(guid, 5);

            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_SET_PLAY_HOVER_ANIM)]
        public static void HandlePlayHoverAnim(Packet packet)
        {
            var guid = new byte[8];
            guid[4] = packet.ReadBit();
            guid[0] = packet.ReadBit();
            guid[1] = packet.ReadBit();
            packet.ReadBit("unk");
            guid[3] = packet.ReadBit();
            guid[7] = packet.ReadBit();
            guid[5] = packet.ReadBit();
            guid[2] = packet.ReadBit();
            guid[6] = packet.ReadBit();

            packet.ParseBitStream(guid, 3, 2, 1, 7, 0, 5, 4, 6);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_MOVE_SET_ACTIVE_MOVER, ClientVersionBuild.V4_3_0_15005, ClientVersionBuild.V4_3_4_15595)]
        public static void HandleMoveSetActiveMover430(Packet packet)
        {
            var guid = packet.StartBitStream(6, 2, 7, 0, 3, 5, 4, 1);
            packet.ParseBitStream(guid, 3, 5, 6, 7, 2, 0, 1, 4);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.CMSG_MOUNT_SPECIAL_ANIM)]
        public static void HandleMovementNull(Packet packet)
        {
        }
    }
}
