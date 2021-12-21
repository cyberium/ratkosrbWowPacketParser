using System.Collections.Generic;
﻿using System.Linq;
using WowPacketParser.DBC;
using WowPacketParser.Enums;
using WowPacketParser.Enums.Version;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;
using WowPacketParser.Store;
using WowPacketParser.Store.Objects;
using CoreParsers = WowPacketParser.Parsing.Parsers;
using MovementFlag = WowPacketParserModule.V4_3_4_15595.Enums.MovementFlag;
using MovementFlagExtra = WowPacketParserModule.V4_3_4_15595.Enums.MovementFlagExtra;
using SetCollisionHeightReason = WowPacketParserModule.V4_3_4_15595.Enums.SetCollisionHeightReason;
using SplineFlag = WowPacketParserModule.V4_3_4_15595.Enums.SplineFlag;

namespace WowPacketParserModule.V4_3_4_15595.Parsers
{
    public static class MovementHandler
    {
        [Parser(Opcode.SMSG_ON_MONSTER_MOVE)]
        [Parser(Opcode.SMSG_MONSTER_MOVE_TRANSPORT)]
        public static void HandleMonsterMove(Packet packet)
        {
            var guid = packet.ReadPackedGuid("MoverGUID");

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
                WowGuid transportGuid = packet.ReadPackedGuid("TransportGUID");
                if (monsterMove != null)
                    monsterMove.TransportGuid = transportGuid;

                sbyte seat = packet.ReadSByte("VehicleSeat");
                if (monsterMove != null)
                    monsterMove.TransportSeat = seat;
            }

            packet.ReadSByte("VehicleExitVoluntary");
            var pos = packet.ReadVector3("Position");

            ReadMovementMonsterSpline(monsterMove, packet, pos, "MovementMonsterSpline");

            if (obj != null && monsterMove != null)
                obj.AddWaypoint(monsterMove, pos, packet.Time);
        }

        public static void ReadMovementMonsterSpline(ServerSideMovement monsterMove, Packet packet, Vector3 pos, params object[] indexes)
        {
            packet.ReadInt32("Id", indexes);
            ReadMovementSpline(monsterMove, packet, pos, indexes, "MovementSpline");
        }

        public static void ReadMovementSpline(ServerSideMovement monsterMove, Packet packet, Vector3 pos, params object[] indexes)
        {
            var type = packet.ReadSByteE<SplineType>("Face", indexes);

            float orientation = 100;
            switch (type)
            {
                case SplineType.FacingSpot:
                {
                    var faceSpot = packet.ReadVector3("FaceSpot", indexes);
                    orientation = Utilities.GetAngle(pos.X, pos.Y, faceSpot.X, faceSpot.Y);
                    break;
                }
                case SplineType.FacingTarget:
                    packet.ReadGuid("FacingGUID", indexes);
                    break;
                case SplineType.FacingAngle:
                {
                    orientation = packet.ReadSingle("FaceDirection", indexes);
                    break;
                }
                case SplineType.Stop:
                    return;
            }
            if (monsterMove != null)
                monsterMove.Orientation = orientation;

            var flags = packet.ReadInt32E<SplineFlag>("Flags", indexes);
            if (monsterMove != null)
                monsterMove.SplineFlags = (uint)flags;

            if (flags.HasAnyFlag(SplineFlag.Animation))
            {
                packet.ReadSByteE<MovementAnimationState>("AnimTier", indexes);
                packet.ReadInt32("TierTransStartTime", indexes); // Async-time in ms
            }

            int movetime = packet.ReadInt32("MoveTime", indexes);
            if (monsterMove != null)
                monsterMove.MoveTime = (uint)movetime;

            if (flags.HasAnyFlag(SplineFlag.Parabolic))
            {
                packet.ReadSingle("JumpGravity", indexes);
                packet.ReadInt32("SpecialTime", indexes);
            }

            var pointsCount = packet.ReadInt32("PointsCount", indexes);
            if (monsterMove != null)
            {
                monsterMove.SplineCount = (uint)pointsCount;
                if (pointsCount > 0)
                    monsterMove.SplinePoints = new List<Vector3>();
            }

            if (flags.HasAnyFlag(SplineFlag.UncompressedPath))
            {
                for (var i = 0; i < pointsCount; i++)
                {
                    Vector3 vec = packet.ReadVector3("Waypoints", indexes, i);
                    if (monsterMove != null)
                        monsterMove.SplinePoints.Add(vec);
                }   
            }
            else
            {
                var newpos = packet.ReadVector3("Points", indexes);

                if (pointsCount > 1)
                {
                    var mid = new Vector3
                    {
                        X = (pos.X + newpos.X) * 0.5f,
                        Y = (pos.Y + newpos.Y) * 0.5f,
                        Z = (pos.Z + newpos.Z) * 0.5f
                    };

                    for (var i = 0; i < pointsCount - 1; ++i)
                    {
                        var vec = packet.ReadPackedVector3();
                        vec.X = mid.X - vec.X;
                        vec.Y = mid.Y - vec.Y;
                        vec.Z = mid.Z - vec.Z;

                        if (monsterMove != null)
                            monsterMove.SplinePoints.Add(vec);

                        packet.AddValue("Waypoints", vec, indexes, i);
                    }
                }

                if (monsterMove != null && monsterMove.SplinePoints != null)
                    monsterMove.SplinePoints.Add(newpos);
            }
        }

        [HasSniffData]
        [Parser(Opcode.SMSG_NEW_WORLD)]
        public static void HandleNewWorld434(Packet packet)
        {
            packet.ReadSingle("X");
            packet.ReadSingle("Orientation");
            packet.ReadSingle("Y");
            CoreParsers.MovementHandler.CurrentMapId = (uint)packet.ReadInt32<MapId>("MapID");
            packet.ReadSingle("Z"); // seriously...

            Storage.ClearDataOnMapChange();
            packet.AddSniffData(StoreNameType.Map, (int)CoreParsers.MovementHandler.CurrentMapId, "NEW_WORLD");
        }

        [Parser(Opcode.MSG_MOVE_TELEPORT_ACK)]
        public static void HandleMoveTeleportAck434(Packet packet)
        {
            packet.ReadInt32("AckIndex");
            packet.ReadInt32("MoveTime");
            var guid = packet.StartBitStream(5, 0, 1, 6, 3, 7, 2, 4);
            packet.ParseBitStream(guid, 4, 2, 7, 6, 5, 1, 3, 0);
            packet.WriteGuid("MoverGUID", guid);
        }

        [Parser(Opcode.MSG_MOVE_HEARTBEAT)]
        public static void HandleMoveHeartbeat434(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();
            MovementInfo info = new MovementInfo();

            info.Position.Z = pos.Z = packet.ReadSingle();
            info.Position.X = pos.X = packet.ReadSingle();
            info.Position.Y = pos.Y = packet.ReadSingle();
            var hasPitch = !packet.ReadBit("Has pitch");
            var hasTime = !packet.ReadBit("Has timestamp");
            var hasFallData = packet.ReadBit("Has fall data");
            var hasMovementFlags2 = !packet.ReadBit();
            var hasTrans = packet.ReadBit("Has transport");
            guid[7] = packet.ReadBit();
            guid[1] = packet.ReadBit();
            guid[0] = packet.ReadBit();
            guid[4] = packet.ReadBit();
            guid[2] = packet.ReadBit();
            var hasO = !packet.ReadBit();
            guid[5] = packet.ReadBit();
            guid[3] = packet.ReadBit();
            var hasSplineElev = !packet.ReadBit("Has Spline Elevation");
            packet.ReadBit("Has Spline");
            packet.ReadBit();
            guid[6] = packet.ReadBit();
            var hasMovementFlags = !packet.ReadBit();

            if (hasTrans)
            {
                hasTransTime3 = packet.ReadBit();
                transportGuid[4] = packet.ReadBit();
                transportGuid[2] = packet.ReadBit();
                hasTransTime2 = packet.ReadBit();
                transportGuid[5] = packet.ReadBit();
                transportGuid[7] = packet.ReadBit();
                transportGuid[6] = packet.ReadBit();
                transportGuid[0] = packet.ReadBit();
                transportGuid[3] = packet.ReadBit();
                transportGuid[1] = packet.ReadBit();
            }

            if (hasFallData)
                hasFallDirection = packet.ReadBit();

            if (hasMovementFlags)
                info.Flags = (uint)packet.ReadBitsE<MovementFlag>("Movement Flags", 30);

            if (hasMovementFlags2)
                info.FlagsExtra = (uint)packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 12);

            packet.ParseBitStream(guid, 3, 6, 1, 7, 2, 5, 0, 4);

            if (hasTrans)
            {
                var tpos = new Vector4();
                tpos.Z = packet.ReadSingle();
                info.TransportSeat = packet.ReadSByte("Transport Seat");
                tpos.O = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 4);
                tpos.Y = packet.ReadSingle();
                info.TransportTime = packet.ReadUInt32("Transport Time");
                tpos.X = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 5);
                packet.ReadXORByte(transportGuid, 1);
                packet.ReadXORByte(transportGuid, 3);
                packet.ReadXORByte(transportGuid, 7);

                if (hasTransTime3)
                    packet.ReadUInt32("Transport Time 3");

                if (hasTransTime2)
                    packet.ReadUInt32("Transport Time 2");

                packet.ReadXORByte(transportGuid, 2);
                packet.ReadXORByte(transportGuid, 0);
                packet.ReadXORByte(transportGuid, 6);

                info.TransportGuid = packet.WriteGuid("Transport Guid", transportGuid);
                info.TransportOffset = packet.AddValue("Transport Position", tpos);
            }

            if (hasO)
                info.Orientation = pos.O = packet.ReadSingle();

            if (hasFallData)
            {
                info.JumpVerticalSpeed = packet.ReadSingle("Vertical Speed");
                info.FallTime = packet.ReadUInt32("Fall time");
                if (hasFallDirection)
                {
                    info.JumpHorizontalSpeed = packet.ReadSingle("Horizontal Speed");
                    info.JumpSinAngle = packet.ReadSingle("Fall Sin");
                    info.JumpCosAngle = packet.ReadSingle("Fall Cos");
                }
            }

            if (hasPitch)
                info.SwimPitch = packet.ReadSingle("Pitch");
            if (hasSplineElev)
                info.SplineElevation = packet.ReadSingle("Spline elevation");
            if (hasTime)
                info.MoveTime = packet.ReadUInt32("Timestamp");

            WowGuid moverGuid = packet.WriteGuid("Guid", guid);
            packet.AddValue("Position", pos);
            Storage.StorePlayerMovement(moverGuid, info, packet);
        }

        [Parser(Opcode.MSG_MOVE_SET_PITCH)]
        public static void HandleMoveSetPitch434(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();
            MovementInfo info = new MovementInfo();

            info.Position.X = pos.X = packet.ReadSingle();
            info.Position.Z = pos.Z = packet.ReadSingle();
            info.Position.Y = pos.Y = packet.ReadSingle();
            var hasFallData = packet.ReadBit("Has fall data");
            var hasMovementFlags = !packet.ReadBit();
            guid[1] = packet.ReadBit();
            var hasO = !packet.ReadBit();
            guid[7] = packet.ReadBit();
            guid[3] = packet.ReadBit();
            var hasMovementFlags2 = !packet.ReadBit();
            var hasTrans = packet.ReadBit("Has transport");
            var hasSplineElev = !packet.ReadBit("Has Spline Elevation");
            guid[6] = packet.ReadBit();
            var hasPitch = !packet.ReadBit("Has pitch");
            guid[4] = packet.ReadBit();
            packet.ReadBit("Has Spline");
            guid[2] = packet.ReadBit();
            packet.ReadBit();
            var hasTime = !packet.ReadBit("Has timestamp");
            guid[0] = packet.ReadBit();
            guid[5] = packet.ReadBit();

            if (hasTrans)
            {
                transportGuid[3] = packet.ReadBit();
                transportGuid[6] = packet.ReadBit();
                transportGuid[2] = packet.ReadBit();
                transportGuid[4] = packet.ReadBit();
                transportGuid[0] = packet.ReadBit();
                transportGuid[1] = packet.ReadBit();
                transportGuid[5] = packet.ReadBit();
                hasTransTime3 = packet.ReadBit();
                transportGuid[7] = packet.ReadBit();
                hasTransTime2 = packet.ReadBit();
            }

            if (hasMovementFlags2)
                info.FlagsExtra = (uint)packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 12);

            if (hasMovementFlags)
                info.Flags = (uint)packet.ReadBitsE<MovementFlag>("Movement Flags", 30);

            if (hasFallData)
                hasFallDirection = packet.ReadBit();

            packet.ParseBitStream(guid, 3, 7, 1, 6, 0, 5, 2, 4);

            if (hasTrans)
            {
                var tpos = new Vector4();

                packet.ReadXORByte(transportGuid, 4);
                packet.ReadXORByte(transportGuid, 3);
                packet.ReadXORByte(transportGuid, 6);
                tpos.O = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 1);

                if (hasTransTime3)
                    packet.ReadUInt32("Transport Time 3");

                info.TransportTime = packet.ReadUInt32("Transport Time");
                packet.ReadXORByte(transportGuid, 2);
                tpos.Z = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 0);

                if (hasTransTime2)
                    packet.ReadUInt32("Transport Time 2");

                tpos.Y = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 5);
                info.TransportSeat = packet.ReadSByte("Transport Seat");
                packet.ReadXORByte(transportGuid, 7);
                tpos.X = packet.ReadSingle();

                info.TransportGuid = packet.WriteGuid("Transport Guid", transportGuid);
                info.TransportOffset = packet.AddValue("Transport Position", tpos);
            }

            if (hasFallData)
            {
                info.JumpVerticalSpeed = packet.ReadSingle("Vertical Speed");
                info.FallTime = packet.ReadUInt32("Fall time");

                if (hasFallDirection)
                {
                    info.JumpHorizontalSpeed = packet.ReadSingle("Horizontal Speed");
                    info.JumpCosAngle = packet.ReadSingle("Fall Cos");
                    info.JumpSinAngle = packet.ReadSingle("Fall Sin");
                }
            }

            if (hasPitch)
                info.SwimPitch = packet.ReadSingle("Pitch");
            if (hasSplineElev)
                info.SplineElevation = packet.ReadSingle("Spline elevation");
            if (hasTime)
                info.MoveTime = packet.ReadUInt32("Timestamp");
            if (hasO)
                info.Orientation = pos.O = packet.ReadSingle();

            WowGuid moverGuid = packet.WriteGuid("Guid", guid);
            packet.AddValue("Position", pos);
            Storage.StorePlayerMovement(moverGuid, info, packet);
        }

        [Parser(Opcode.MSG_MOVE_SET_FACING)]
        public static void HandleMoveSetFacing434(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();
            MovementInfo info = new MovementInfo();

            info.Position.X = pos.X = packet.ReadSingle();
            info.Position.Y = pos.Y = packet.ReadSingle();
            info.Position.Z = pos.Z = packet.ReadSingle();
            guid[6] = packet.ReadBit();
            var hasTrans = packet.ReadBit("Has transport");
            guid[4] = packet.ReadBit();
            packet.ReadBit("Has Spline");
            guid[0] = packet.ReadBit();
            var hasO = !packet.ReadBit();
            var hasTime = !packet.ReadBit("Has timestamp");
            var hasPitch = !packet.ReadBit("Has pitch");
            var hasMovementFlags2 = !packet.ReadBit();
            guid[5] = packet.ReadBit();
            guid[7] = packet.ReadBit();
            guid[2] = packet.ReadBit();
            var hasFallData = packet.ReadBit("Has fall data");
            var hasSplineElev = !packet.ReadBit("Has Spline Elevation");
            var hasMovementFlags = !packet.ReadBit();
            guid[3] = packet.ReadBit();
            packet.ReadBit();
            guid[1] = packet.ReadBit();

            if (hasTrans)
            {
                hasTransTime3 = packet.ReadBit();
                transportGuid[3] = packet.ReadBit();
                transportGuid[7] = packet.ReadBit();
                transportGuid[5] = packet.ReadBit();
                transportGuid[2] = packet.ReadBit();
                transportGuid[6] = packet.ReadBit();
                transportGuid[0] = packet.ReadBit();
                hasTransTime2 = packet.ReadBit();
                transportGuid[4] = packet.ReadBit();
                transportGuid[1] = packet.ReadBit();
            }

            if (hasFallData)
                hasFallDirection = packet.ReadBit();

            if (hasMovementFlags2)
                info.FlagsExtra = (uint)packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 12);

            if (hasMovementFlags)
                info.Flags = (uint)packet.ReadBitsE<MovementFlag>("Movement Flags", 30);

            packet.ParseBitStream(guid, 6, 7, 2, 0, 4, 1, 5, 3);

            if (hasTrans)
            {
                var tpos = new Vector4();
                packet.ReadXORByte(transportGuid, 0);
                tpos.X = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 1);

                if (hasTransTime2)
                    packet.ReadUInt32("Transport Time 2");

                packet.ReadXORByte(transportGuid, 4);
                tpos.Z = packet.ReadSingle();
                tpos.O = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 2);

                if (hasTransTime3)
                    packet.ReadUInt32("Transport Time 3");

                packet.ReadXORByte(transportGuid, 3);
                info.TransportSeat = packet.ReadSByte("Transport Seat");
                tpos.Y = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 5);
                packet.ReadXORByte(transportGuid, 6);
                info.TransportTime = packet.ReadUInt32("Transport Time");
                packet.ReadXORByte(transportGuid, 7);

                info.TransportGuid = packet.WriteGuid("Transport Guid", transportGuid);
                info.TransportOffset = packet.AddValue("Transport Position", tpos);
            }

            if (hasFallData)
            {
                if (hasFallDirection)
                {
                    info.JumpHorizontalSpeed = packet.ReadSingle("Horizontal Speed");
                    info.JumpSinAngle = packet.ReadSingle("Fall Sin");
                    info.JumpCosAngle = packet.ReadSingle("Fall Cos");
                }

                info.FallTime = packet.ReadUInt32("Fall time");
                info.JumpVerticalSpeed = packet.ReadSingle("Vertical Speed");
            }

            if (hasSplineElev)
                info.SplineElevation = packet.ReadSingle("Spline elevation");
            if (hasO)
                info.Orientation = pos.O = packet.ReadSingle();
            if (hasTime)
                info.MoveTime = packet.ReadUInt32("Timestamp");
            if (hasPitch)
                info.SwimPitch = packet.ReadSingle("Pitch");

            WowGuid moverGuid = packet.WriteGuid("Guid", guid);
            packet.AddValue("Position", pos);
            Storage.StorePlayerMovement(moverGuid, info, packet);
        }

        [Parser(Opcode.MSG_MOVE_TELEPORT)]
        public static void HandleMoveTeleport434(Packet packet)
        {
            var guid = new byte[8];
            var transGuid = new byte[8];
            var pos = new Vector4();
            MovementInfo info = new MovementInfo();

            guid[6] = packet.ReadBit();
            guid[0] = packet.ReadBit();
            guid[3] = packet.ReadBit();
            guid[2] = packet.ReadBit();
            var hasVehicle = packet.ReadBit("HasVehicle");
            if (hasVehicle)
            {
                packet.ReadBit("VehicleExitVoluntary");
                packet.ReadBit("VehicleExitTeleport");
            }

            var onTransport = packet.ReadBit("HasTransport");
            guid[1] = packet.ReadBit();
            if (onTransport)
                transGuid = packet.StartBitStream(1, 3, 2, 5, 0, 7, 6, 4);

            guid[4] = packet.ReadBit();
            guid[7] = packet.ReadBit();
            guid[5] = packet.ReadBit();
            if (onTransport)
            {
                packet.ParseBitStream(transGuid, 5, 6, 1, 7, 0, 2, 4, 3);
                info.TransportGuid = packet.WriteGuid("TransportGUID", transGuid);
            }

            packet.ReadUInt32("SequenceIndex");
            packet.ReadXORByte(guid, 1);
            packet.ReadXORByte(guid, 2);
            packet.ReadXORByte(guid, 3);
            packet.ReadXORByte(guid, 5);
            info.Position.X = pos.X = packet.ReadSingle();
            packet.ReadXORByte(guid, 4);
            info.Orientation = pos.O = packet.ReadSingle();
            packet.ReadXORByte(guid, 7);
            info.Position.Z = pos.Z = packet.ReadSingle();
            if (hasVehicle)
                info.TransportSeat = (sbyte)packet.ReadUInt32("VehicleSeatIndex");

            packet.ReadXORByte(guid, 0);
            packet.ReadXORByte(guid, 6);
            info.Position.Y = pos.Y = packet.ReadSingle();

            packet.AddValue("Pos", pos);
            WowGuid moverGuid = packet.WriteGuid("MoverGUID", guid);
            Storage.StorePlayerMovement(moverGuid, info, packet);
        }

        [Parser(Opcode.MSG_MOVE_STOP)]
        public static void HandleMoveStop434(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();
            MovementInfo info = new MovementInfo();

            info.Position.X = pos.X = packet.ReadSingle();
            info.Position.Y = pos.Y = packet.ReadSingle();
            info.Position.Z = pos.Z = packet.ReadSingle();
            guid[3] = packet.ReadBit();
            guid[6] = packet.ReadBit();
            var hasSplineElev = !packet.ReadBit("Has Spline Elevation");
            packet.ReadBit("Has Spline");
            var hasO = !packet.ReadBit();
            guid[7] = packet.ReadBit();
            var hasMovementFlags = !packet.ReadBit();
            guid[5] = packet.ReadBit();
            var hasFallData = packet.ReadBit("Has fall data");
            var hasMovementFlags2 = !packet.ReadBit();
            var hasTrans = packet.ReadBit("Has transport");
            var hasTime = !packet.ReadBit("Has timestamp");
            guid[4] = packet.ReadBit();
            guid[1] = packet.ReadBit();
            packet.ReadBit();
            guid[2] = packet.ReadBit();
            guid[0] = packet.ReadBit();
            var hasPitch = !packet.ReadBit("Has pitch");

            if (hasTrans)
            {
                transportGuid[7] = packet.ReadBit();
                transportGuid[4] = packet.ReadBit();
                transportGuid[1] = packet.ReadBit();
                transportGuid[5] = packet.ReadBit();
                hasTransTime2 = packet.ReadBit();
                hasTransTime3 = packet.ReadBit();
                transportGuid[3] = packet.ReadBit();
                transportGuid[6] = packet.ReadBit();
                transportGuid[0] = packet.ReadBit();
                transportGuid[2] = packet.ReadBit();
            }

            if (hasMovementFlags)
                info.Flags = (uint)packet.ReadBitsE<MovementFlag>("Movement Flags", 30);

            if (hasMovementFlags2)
                info.FlagsExtra = (uint)packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 12);

            if (hasFallData)
                hasFallDirection = packet.ReadBit();

            packet.ParseBitStream(guid, 6, 3, 0, 4, 2, 1, 5, 7);

            if (hasTrans)
            {
                var tpos = new Vector4();
                packet.ReadXORByte(transportGuid, 4);
                packet.ReadXORByte(transportGuid, 7);
                info.TransportTime = packet.ReadUInt32("Transport Time");
                info.TransportSeat = packet.ReadSByte("Transport Seat");
                tpos.Z = packet.ReadSingle();

                if (hasTransTime3)
                    packet.ReadUInt32("Transport Time 3");

                packet.ReadXORByte(transportGuid, 2);
                packet.ReadXORByte(transportGuid, 0);
                tpos.Y = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 1);
                packet.ReadXORByte(transportGuid, 3);

                if (hasTransTime2)
                    packet.ReadUInt32("Transport Time 2");

                tpos.X = packet.ReadSingle();
                tpos.O = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 5);
                packet.ReadXORByte(transportGuid, 6);

                info.TransportGuid = packet.WriteGuid("Transport Guid", transportGuid);
                info.TransportOffset = packet.AddValue("Transport Position", tpos);
            }

            if (hasTime)
                info.MoveTime = packet.ReadUInt32("Timestamp");
            if (hasO)
                info.Orientation = pos.O = packet.ReadSingle();
            if (hasPitch)
                info.SwimPitch = packet.ReadSingle("Pitch");
            if (hasSplineElev)
                info.SplineElevation = packet.ReadSingle("Spline elevation");

            if (hasFallData)
            {
                if (hasFallDirection)
                {
                    info.JumpCosAngle = packet.ReadSingle("Fall Cos");
                    info.JumpSinAngle = packet.ReadSingle("Fall Sin");
                    info.JumpHorizontalSpeed = packet.ReadSingle("Horizontal Speed");
                }

                info.JumpVerticalSpeed = packet.ReadSingle("Vertical Speed");
                info.FallTime = packet.ReadUInt32("Fall time");
            }

            WowGuid moverGuid = packet.WriteGuid("Guid", guid);
            packet.AddValue("Position", pos);
            Storage.StorePlayerMovement(moverGuid, info, packet);
        }

        [Parser(Opcode.CMSG_MOVE_CHANGE_TRANSPORT)]
        public static void HandleMoveChngTransport434(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();
            MovementInfo info = new MovementInfo();

            info.Position.Y = pos.Y = packet.ReadSingle();
            info.Position.X = pos.X = packet.ReadSingle();
            info.Position.Z = pos.Z = packet.ReadSingle();
            guid[4] = packet.ReadBit();
            guid[0] = packet.ReadBit();
            guid[2] = packet.ReadBit();
            var hasTrans = packet.ReadBit("Has transport");
            packet.ReadBit("Has Spline");
            var hasO = !packet.ReadBit();
            guid[6] = packet.ReadBit();
            var hasTime = !packet.ReadBit("Has timestamp");
            guid[5] = packet.ReadBit();
            guid[7] = packet.ReadBit();
            var hasPitch = !packet.ReadBit("Has pitch");
            packet.ReadBit();
            guid[3] = packet.ReadBit();
            var hasSplineElev = !packet.ReadBit("Has Spline Elevation");
            guid[1] = packet.ReadBit();
            var hasFallData = packet.ReadBit("Has fall data");
            var hasMovementFlags = !packet.ReadBit();
            var hasMovementFlags2 = !packet.ReadBit();

            if (hasTrans)
            {
                transportGuid[3] = packet.ReadBit();
                hasTransTime3 = packet.ReadBit();
                transportGuid[4] = packet.ReadBit();
                transportGuid[1] = packet.ReadBit();
                transportGuid[6] = packet.ReadBit();
                transportGuid[2] = packet.ReadBit();
                hasTransTime2 = packet.ReadBit();
                transportGuid[0] = packet.ReadBit();
                transportGuid[7] = packet.ReadBit();
                transportGuid[5] = packet.ReadBit();
            }

            if (hasMovementFlags)
                info.Flags = (uint)packet.ReadBitsE<MovementFlag>("Movement Flags", 30);

            if (hasMovementFlags2)
                info.FlagsExtra = (uint)packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 12);

            if (hasFallData)
                hasFallDirection = packet.ReadBit();

            packet.ParseBitStream(guid, 7, 5, 1, 2, 6, 4, 0, 3);

            if (hasTrans)
            {
                var tpos = new Vector4();

                tpos.Y = packet.ReadSingle();
                info.TransportSeat = packet.ReadSByte("Transport Seat");
                packet.ReadXORByte(transportGuid, 1);
                packet.ReadXORByte(transportGuid, 0);
                packet.ReadXORByte(transportGuid, 2);
                packet.ReadXORByte(transportGuid, 3);
                packet.ReadXORByte(transportGuid, 6);
                tpos.Z = packet.ReadSingle();
                tpos.X = packet.ReadSingle();

                if (hasTransTime3)
                    packet.ReadUInt32("Transport Time 3");

                info.TransportTime = packet.ReadUInt32("Transport Time");
                packet.ReadXORByte(transportGuid, 7);
                packet.ReadXORByte(transportGuid, 5);
                packet.ReadXORByte(transportGuid, 4);
                tpos.O = packet.ReadSingle();

                if (hasTransTime2)
                    packet.ReadUInt32("Transport Time 2");

                info.TransportGuid = packet.WriteGuid("Transport Guid", transportGuid);
                info.TransportOffset = packet.AddValue("Transport Position", tpos);
            }

            if (hasFallData)
            {
                info.FallTime = packet.ReadUInt32("Fall time");

                if (hasFallDirection)
                {
                    info.JumpHorizontalSpeed = packet.ReadSingle("Horizontal Speed");
                    info.JumpCosAngle = packet.ReadSingle("Fall Cos");
                    info.JumpSinAngle = packet.ReadSingle("Fall Sin");
                }

                info.JumpVerticalSpeed = packet.ReadSingle("Vertical Speed");
            }

            if (hasO)
                info.Orientation = pos.O = packet.ReadSingle();
            if (hasPitch)
                info.SwimPitch = packet.ReadSingle("Pitch");
            if (hasSplineElev)
                info.SplineElevation = packet.ReadSingle("Spline elevation");
            if (hasTime)
                info.MoveTime = packet.ReadUInt32("Timestamp");

            WowGuid moverGuid = packet.WriteGuid("Guid", guid);
            packet.AddValue("Position", pos);
            Storage.StorePlayerMovement(moverGuid, info, packet);
        }

        [Parser(Opcode.MSG_MOVE_START_ASCEND)]
        public static void HandleMoveStartAscend434(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();
            MovementInfo info = new MovementInfo();

            info.Position.X = pos.X = packet.ReadSingle();
            info.Position.Y = pos.Y = packet.ReadSingle();
            info.Position.Z = pos.Z = packet.ReadSingle();
            guid[0] = packet.ReadBit();
            guid[1] = packet.ReadBit();
            guid[7] = packet.ReadBit();
            packet.ReadBit();
            guid[5] = packet.ReadBit();
            var hasTrans = packet.ReadBit("Has transport");
            guid[2] = packet.ReadBit();
            var hasSplineElev = !packet.ReadBit("Has Spline Elevation");
            var hasFallData = packet.ReadBit("Has fall data");
            packet.ReadBit("Has Spline");
            guid[3] = packet.ReadBit();
            var hasMovementFlags2 = !packet.ReadBit();
            guid[6] = packet.ReadBit();
            var hasMovementFlags = !packet.ReadBit();
            var hasPitch = !packet.ReadBit("Has pitch");
            var hasTime = !packet.ReadBit("Has timestamp");
            var hasO = !packet.ReadBit();
            guid[4] = packet.ReadBit();

            if (hasMovementFlags)
                info.Flags = (uint)packet.ReadBitsE<MovementFlag>("Movement Flags", 30);

            if (hasTrans)
            {
                transportGuid[0] = packet.ReadBit();
                transportGuid[2] = packet.ReadBit();
                hasTransTime3 = packet.ReadBit();
                transportGuid[6] = packet.ReadBit();
                transportGuid[1] = packet.ReadBit();
                transportGuid[4] = packet.ReadBit();
                hasTransTime2 = packet.ReadBit();
                transportGuid[5] = packet.ReadBit();
                transportGuid[3] = packet.ReadBit();
                transportGuid[7] = packet.ReadBit();
            }

            if (hasMovementFlags2)
                info.FlagsExtra = (uint)packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 12);

            if (hasFallData)
                hasFallDirection = packet.ReadBit();

            packet.ParseBitStream(guid, 6, 3, 1, 4, 2, 0, 5, 7);

            if (hasTrans)
            {
                var tpos = new Vector4();

                packet.ReadXORByte(transportGuid, 3);
                packet.ReadXORByte(transportGuid, 5);
                packet.ReadXORByte(transportGuid, 4);
                info.TransportSeat = packet.ReadSByte("Transport Seat");
                packet.ReadXORByte(transportGuid, 2);
                tpos.O = packet.ReadSingle();
                tpos.Y = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 7);

                if (hasTransTime2)
                    packet.ReadUInt32("Transport Time 2");

                tpos.Z = packet.ReadSingle();

                if (hasTransTime3)
                    packet.ReadUInt32("Transport Time 3");

                packet.ReadXORByte(transportGuid, 6);
                info.TransportTime = packet.ReadUInt32("Transport Time");
                packet.ReadXORByte(transportGuid, 0);
                packet.ReadXORByte(transportGuid, 1);
                tpos.X = packet.ReadSingle();

                info.TransportGuid = packet.WriteGuid("Transport Guid", transportGuid);
                info.TransportOffset = packet.AddValue("Transport Position", tpos);
            }

            if (hasFallData)
            {
                if (hasFallDirection)
                {
                    info.JumpCosAngle = packet.ReadSingle("Fall Cos");
                    info.JumpSinAngle = packet.ReadSingle("Fall Sin");
                    info.JumpHorizontalSpeed = packet.ReadSingle("Horizontal Speed");
                }

                info.FallTime = packet.ReadUInt32("Fall time");
                info.JumpVerticalSpeed = packet.ReadSingle("Vertical Speed");
            }

            if (hasO)
                info.Orientation = pos.O = packet.ReadSingle();
            if (hasPitch)
                info.SwimPitch = packet.ReadSingle("Pitch");
            if (hasTime)
                info.MoveTime = packet.ReadUInt32("Timestamp");
            if (hasSplineElev)
                info.SplineElevation = packet.ReadSingle("Spline elevation");

            WowGuid moverGuid = packet.WriteGuid("Guid", guid);
            packet.AddValue("Position", pos);
            Storage.StorePlayerMovement(moverGuid, info, packet);
        }

        [Parser(Opcode.MSG_MOVE_START_DESCEND)]
        public static void HandleMoveStartDescend434(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();
            MovementInfo info = new MovementInfo();

            info.Position.Y = pos.Y = packet.ReadSingle();
            info.Position.Z = pos.Z = packet.ReadSingle();
            info.Position.X = pos.X = packet.ReadSingle();
            guid[0] = packet.ReadBit();
            var hasPitch = !packet.ReadBit("Has pitch");
            var hasFallData = packet.ReadBit("Has fall data");
            guid[4] = packet.ReadBit();
            var hasO = !packet.ReadBit();
            var hasTime = !packet.ReadBit("Has timestamp");
            var hasMovementFlags2 = !packet.ReadBit();
            var hasMovementFlags = !packet.ReadBit();
            guid[6] = packet.ReadBit();
            packet.ReadBit();
            guid[1] = packet.ReadBit();
            var hasTrans = packet.ReadBit("Has transport");
            packet.ReadBit("Has Spline");
            var hasSplineElev = !packet.ReadBit("Has Spline Elevation");
            guid[5] = packet.ReadBit();
            guid[3] = packet.ReadBit();
            guid[7] = packet.ReadBit();
            guid[2] = packet.ReadBit();

            if (hasTrans)
            {
                transportGuid[0] = packet.ReadBit();
                transportGuid[1] = packet.ReadBit();
                transportGuid[2] = packet.ReadBit();
                hasTransTime3 = packet.ReadBit();
                transportGuid[7] = packet.ReadBit();
                transportGuid[4] = packet.ReadBit();
                transportGuid[6] = packet.ReadBit();
                hasTransTime2 = packet.ReadBit();
                transportGuid[3] = packet.ReadBit();
                transportGuid[5] = packet.ReadBit();
            }

            if (hasMovementFlags2)
                info.FlagsExtra = (uint)packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 12);

            if (hasMovementFlags)
                info.Flags = (uint)packet.ReadBitsE<MovementFlag>("Movement Flags", 30);

            if (hasFallData)
                hasFallDirection = packet.ReadBit();

            packet.ParseBitStream(guid, 2, 7, 6, 0, 1, 5, 4, 3);

            if (hasPitch)
                info.SwimPitch = packet.ReadSingle("Pitch");

            if (hasTrans)
            {
                var tpos = new Vector4();

                tpos.Z = packet.ReadSingle();
                info.TransportSeat = packet.ReadSByte("Transport Seat");

                if (hasTransTime3)
                    packet.ReadUInt32("Transport Time 3");

                packet.ReadXORByte(transportGuid, 6);

                if (hasTransTime2)
                    packet.ReadUInt32("Transport Time 2");

                packet.ReadXORByte(transportGuid, 1);
                info.TransportTime = packet.ReadUInt32("Transport Time");
                packet.ReadXORByte(transportGuid, 4);
                tpos.Y = packet.ReadSingle();
                tpos.X = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 7);
                packet.ReadXORByte(transportGuid, 3);
                packet.ReadXORByte(transportGuid, 5);
                packet.ReadXORByte(transportGuid, 2);
                tpos.O = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 0);

                info.TransportGuid = packet.WriteGuid("Transport Guid", transportGuid);
                info.TransportOffset = packet.AddValue("Transport Position", tpos);
            }

            if (hasFallData)
            {
                info.FallTime = packet.ReadUInt32("Fall time");

                if (hasFallDirection)
                {
                    info.JumpSinAngle = packet.ReadSingle("Fall Sin");
                    info.JumpCosAngle = packet.ReadSingle("Fall Cos");
                    info.JumpHorizontalSpeed = packet.ReadSingle("Horizontal Speed");
                }

                info.JumpVerticalSpeed = packet.ReadSingle("Vertical Speed");
            }

            if (hasTime)
                info.MoveTime = packet.ReadUInt32("Timestamp");
            if (hasO)
                info.Orientation = pos.O = packet.ReadSingle();
            if (hasSplineElev)
                info.SplineElevation = packet.ReadSingle("Spline elevation");

            WowGuid moverGuid = packet.WriteGuid("Guid", guid);
            packet.AddValue("Position", pos);
            Storage.StorePlayerMovement(moverGuid, info, packet);
        }

        [Parser(Opcode.MSG_MOVE_STOP_ASCEND)]
        public static void HandleMoveStopAscend434(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();
            MovementInfo info = new MovementInfo();

            info.Position.Z = pos.Z = packet.ReadSingle();
            info.Position.Y = pos.Y = packet.ReadSingle();
            info.Position.X = pos.X = packet.ReadSingle();
            guid[3] = packet.ReadBit();
            guid[2] = packet.ReadBit();
            var hasFallData = packet.ReadBit("Has fall data");
            guid[7] = packet.ReadBit();
            var hasO = !packet.ReadBit();
            var hasPitch = !packet.ReadBit("Has pitch");
            packet.ReadBit("Has Spline");
            packet.ReadBit();
            guid[1] = packet.ReadBit();
            guid[4] = packet.ReadBit();
            var hasMovementFlags = !packet.ReadBit();
            guid[0] = packet.ReadBit();
            guid[6] = packet.ReadBit();
            var hasSplineElev = !packet.ReadBit("Has Spline Elevation");
            var hasTrans = packet.ReadBit("Has transport");
            var hasMovementFlags2 = !packet.ReadBit();
            var hasTime = !packet.ReadBit("Has timestamp");
            guid[5] = packet.ReadBit();

            if (hasTrans)
            {
                hasTransTime2 = packet.ReadBit();
                hasTransTime3 = packet.ReadBit();
                transportGuid = packet.StartBitStream(1, 3, 2, 5, 7, 4, 6, 0);
            }

            if (hasMovementFlags2)
                info.FlagsExtra = (uint)packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 12);

            if (hasMovementFlags)
                info.Flags = (uint)packet.ReadBitsE<MovementFlag>("Movement Flags", 30);

            if (hasFallData)
                hasFallDirection = packet.ReadBit();

            packet.ParseBitStream(guid, 7, 5, 4, 3, 2, 1, 0, 6);

            if (hasTrans)
            {
                var tpos = new Vector4();

                packet.ReadXORByte(transportGuid, 5);
                tpos.Y = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 7);
                packet.ReadXORByte(transportGuid, 2);
                packet.ReadXORByte(transportGuid, 6);

                if (hasTransTime2)
                    packet.ReadUInt32("Transport Time 2");

                if (hasTransTime3)
                    packet.ReadUInt32("Transport Time 3");

                tpos.O = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 3);
                packet.ReadXORByte(transportGuid, 4);
                info.TransportSeat = packet.ReadSByte("Transport Seat");
                packet.ReadXORByte(transportGuid, 1);
                info.TransportTime = packet.ReadUInt32("Transport Time");
                packet.ReadXORByte(transportGuid, 0);
                tpos.X = packet.ReadSingle();
                tpos.Z = packet.ReadSingle();

                info.TransportGuid = packet.WriteGuid("Transport Guid", transportGuid);
                info.TransportOffset = packet.AddValue("Transport Position", tpos);
            }

            if (hasFallData)
            {
                info.FallTime = packet.ReadUInt32("Fall time");
                info.JumpVerticalSpeed = packet.ReadSingle("Vertical Speed");

                if (hasFallDirection)
                {
                    info.JumpCosAngle = packet.ReadSingle("Fall Cos");
                    info.JumpSinAngle = packet.ReadSingle("Fall Sin");
                    info.JumpHorizontalSpeed = packet.ReadSingle("Horizontal Speed");
                }
            }

            if (hasPitch)
                info.SwimPitch = packet.ReadSingle("Pitch");
            if (hasSplineElev)
                info.SplineElevation = packet.ReadSingle("Spline elevation");
            if (hasTime)
                info.MoveTime = packet.ReadUInt32("Timestamp");
            if (hasO)
                info.Orientation = pos.O = packet.ReadSingle();

            WowGuid moverGuid = packet.WriteGuid("Guid", guid);
            packet.AddValue("Position", pos);
            Storage.StorePlayerMovement(moverGuid, info, packet);
        }

        [Parser(Opcode.MSG_MOVE_START_PITCH_DOWN)]
        public static void HandleMoveStartPitchDown434(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();
            MovementInfo info = new MovementInfo();

            info.Position.X = pos.X = packet.ReadSingle();
            info.Position.Z = pos.Z = packet.ReadSingle();
            info.Position.Y = pos.Y = packet.ReadSingle();
            var hasMovementFlags = !packet.ReadBit();
            guid[7] = packet.ReadBit();
            guid[6] = packet.ReadBit();
            var hasPitch = !packet.ReadBit("Has pitch");
            packet.ReadBit();
            guid[1] = packet.ReadBit();
            guid[4] = packet.ReadBit();
            var hasTime = !packet.ReadBit("Has timestamp");
            var hasSplineElev = !packet.ReadBit("Has Spline Elevation");
            var hasMovementFlags2 = !packet.ReadBit();
            var hasTrans = packet.ReadBit("Has transport");
            guid[0] = packet.ReadBit();
            guid[5] = packet.ReadBit();
            guid[3] = packet.ReadBit();
            packet.ReadBit("Has Spline");
            var hasO = !packet.ReadBit();
            var hasFallData = packet.ReadBit("Has fall data");
            guid[2] = packet.ReadBit();

            if (hasTrans)
            {
                transportGuid[1] = packet.ReadBit();
                transportGuid[5] = packet.ReadBit();
                hasTransTime3 = packet.ReadBit();
                transportGuid[7] = packet.ReadBit();
                transportGuid[4] = packet.ReadBit();
                transportGuid[6] = packet.ReadBit();
                hasTransTime2 = packet.ReadBit();
                transportGuid[0] = packet.ReadBit();
                transportGuid[3] = packet.ReadBit();
                transportGuid[2] = packet.ReadBit();
            }

            if (hasMovementFlags2)
                info.FlagsExtra = (uint)packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 12);

            if (hasFallData)
                hasFallDirection = packet.ReadBit();

            if (hasMovementFlags)
                info.Flags = (uint)packet.ReadBitsE<MovementFlag>("Movement Flags", 30);

            packet.ParseBitStream(guid, 3, 7, 0, 5, 2, 6, 4, 1);

            if (hasTrans)
            {
                var tpos = new Vector4();

                if (hasTransTime3)
                    packet.ReadUInt32("Transport Time 3");

                packet.ReadXORByte(transportGuid, 3);

                if (hasTransTime2)
                    packet.ReadUInt32("Transport Time 2");

                packet.ReadXORByte(transportGuid, 5);
                packet.ReadXORByte(transportGuid, 1);
                tpos.Y = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 6);
                packet.ReadXORByte(transportGuid, 4);
                info.TransportTime = packet.ReadUInt32("Transport Time");
                tpos.Z = packet.ReadSingle();
                tpos.X = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 0);
                info.TransportSeat = packet.ReadSByte("Transport Seat");
                packet.ReadXORByte(transportGuid, 2);
                packet.ReadXORByte(transportGuid, 7);
                tpos.O = packet.ReadSingle();

                info.TransportGuid = packet.WriteGuid("Transport Guid", transportGuid);
                info.TransportOffset = packet.AddValue("Transport Position", tpos);
            }

            if (hasFallData)
            {
                if (hasFallDirection)
                {
                    info.JumpSinAngle = packet.ReadSingle("Fall Sin");
                    info.JumpHorizontalSpeed = packet.ReadSingle("Horizontal Speed");
                    info.JumpCosAngle = packet.ReadSingle("Fall Cos");
                }

                info.JumpVerticalSpeed = packet.ReadSingle("Vertical Speed");
                info.FallTime = packet.ReadUInt32("Fall time");
            }

            if (hasPitch)
                info.SwimPitch = packet.ReadSingle("Pitch");
            if (hasO)
                info.Orientation = pos.O = packet.ReadSingle();
            if (hasSplineElev)
                info.SplineElevation = packet.ReadSingle("Spline elevation");
            if (hasTime)
                info.MoveTime = packet.ReadUInt32("Timestamp");

            WowGuid moverGuid = packet.WriteGuid("Guid", guid);
            packet.AddValue("Position", pos);
            Storage.StorePlayerMovement(moverGuid, info, packet);
        }

        [Parser(Opcode.MSG_MOVE_START_PITCH_UP)]
        public static void HandleMoveStartPitchUp434(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();
            MovementInfo info = new MovementInfo();

            info.Position.Z = pos.Z = packet.ReadSingle();
            info.Position.Y = pos.Y = packet.ReadSingle();
            info.Position.X = pos.X = packet.ReadSingle();
            guid[4] = packet.ReadBit();
            var hasMovementFlags = !packet.ReadBit();
            var hasMovementFlags2 = !packet.ReadBit();
            packet.ReadBit("Has Spline");
            guid[2] = packet.ReadBit();
            guid[6] = packet.ReadBit();
            guid[3] = packet.ReadBit();
            packet.ReadBit();
            var hasSplineElev = !packet.ReadBit("Has Spline Elevation");
            var hasFallData = packet.ReadBit("Has fall data");
            guid[0] = packet.ReadBit();
            var hasTrans = packet.ReadBit("Has transport");
            guid[1] = packet.ReadBit();
            guid[5] = packet.ReadBit();
            var hasPitch = !packet.ReadBit("Has pitch");
            var hasTime = !packet.ReadBit("Has timestamp");
            var hasO = !packet.ReadBit();
            guid[7] = packet.ReadBit();

            if (hasMovementFlags2)
                info.FlagsExtra = (uint)packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 12);

            if (hasTrans)
            {
                transportGuid[7] = packet.ReadBit();
                transportGuid[3] = packet.ReadBit();
                transportGuid[1] = packet.ReadBit();
                hasTransTime2 = packet.ReadBit();
                transportGuid[4] = packet.ReadBit();
                hasTransTime3 = packet.ReadBit();
                transportGuid[0] = packet.ReadBit();
                transportGuid[6] = packet.ReadBit();
                transportGuid[5] = packet.ReadBit();
                transportGuid[2] = packet.ReadBit();
            }

            if (hasMovementFlags)
                info.Flags = (uint)packet.ReadBitsE<MovementFlag>("Movement Flags", 30);

            if (hasFallData)
                hasFallDirection = packet.ReadBit();

            packet.ParseBitStream(guid, 0, 3, 4, 6, 7, 1, 5, 2);

            if (hasTrans)
            {
                var tpos = new Vector4();

                tpos.X = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 1);
                packet.ReadXORByte(transportGuid, 3);
                packet.ReadXORByte(transportGuid, 2);
                packet.ReadXORByte(transportGuid, 0);
                info.TransportTime = packet.ReadUInt32("Transport Time");
                tpos.Z = packet.ReadSingle();
                tpos.O = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 7);

                if (hasTransTime2)
                    packet.ReadUInt32("Transport Time 2");

                info.TransportSeat = packet.ReadSByte("Transport Seat");
                packet.ReadXORByte(transportGuid, 6);
                packet.ReadXORByte(transportGuid, 5);

                if (hasTransTime3)
                    packet.ReadUInt32("Transport Time 3");

                tpos.Y = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 4);

                info.TransportGuid = packet.WriteGuid("Transport Guid", transportGuid);
                info.TransportOffset = packet.AddValue("Transport Position", tpos);
            }

            if (hasFallData)
            {
                info.FallTime = packet.ReadUInt32("Fall time");

                if (hasFallDirection)
                {
                    info.JumpCosAngle = packet.ReadSingle("Fall Cos");
                    info.JumpSinAngle = packet.ReadSingle("Fall Sin");
                    info.JumpHorizontalSpeed = packet.ReadSingle("Horizontal Speed");
                }

                info.JumpVerticalSpeed = packet.ReadSingle("Vertical Speed");
            }

            if (hasO)
                info.Orientation = pos.O = packet.ReadSingle();
            if (hasSplineElev)
                info.SplineElevation = packet.ReadSingle("Spline elevation");
            if (hasPitch)
                info.SwimPitch = packet.ReadSingle("Pitch");
            if (hasTime)
                info.MoveTime = packet.ReadUInt32("Timestamp");

            WowGuid moverGuid = packet.WriteGuid("Guid", guid);
            packet.AddValue("Position", pos);
            Storage.StorePlayerMovement(moverGuid, info, packet);
        }

        [Parser(Opcode.MSG_MOVE_STOP_PITCH)]
        public static void HandleMoveStopPitch434(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();
            MovementInfo info = new MovementInfo();

            info.Position.Y = pos.Y = packet.ReadSingle();
            info.Position.X = pos.X = packet.ReadSingle();
            info.Position.Z = pos.Z = packet.ReadSingle();
            guid[0] = packet.ReadBit();
            guid[5] = packet.ReadBit();
            guid[3] = packet.ReadBit();
            var hasFallData = packet.ReadBit("Has fall data");
            guid[2] = packet.ReadBit();
            guid[4] = packet.ReadBit();
            guid[7] = packet.ReadBit();
            var hasPitch = !packet.ReadBit("Has pitch");
            packet.ReadBit("Has Spline");
            var hasO = !packet.ReadBit();
            packet.ReadBit();
            var hasMovementFlags2 = !packet.ReadBit();
            var hasSplineElev = !packet.ReadBit("Has Spline Elevation");
            guid[6] = packet.ReadBit();
            guid[1] = packet.ReadBit();
            var hasTime = !packet.ReadBit("Has timestamp");
            var hasTrans = packet.ReadBit("Has transport");
            var hasMovementFlags = !packet.ReadBit();

            if (hasTrans)
            {
                transportGuid[7] = packet.ReadBit();
                transportGuid[0] = packet.ReadBit();
                transportGuid[5] = packet.ReadBit();
                transportGuid[2] = packet.ReadBit();
                hasTransTime3 = packet.ReadBit();
                transportGuid[6] = packet.ReadBit();
                transportGuid[3] = packet.ReadBit();
                hasTransTime2 = packet.ReadBit();
                transportGuid[4] = packet.ReadBit();
                transportGuid[1] = packet.ReadBit();
            }

            if (hasFallData)
                hasFallDirection = packet.ReadBit();

            if (hasMovementFlags2)
                info.FlagsExtra = (uint)packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 12);

            if (hasMovementFlags)
                info.Flags = (uint)packet.ReadBitsE<MovementFlag>("Movement Flags", 30);

            packet.ParseBitStream(guid, 1, 7, 0, 6, 4, 3, 5, 2);

            if (hasTime)
                info.MoveTime = packet.ReadUInt32("Timestamp");

            if (hasFallData)
            {
                if (hasFallDirection)
                {
                    info.JumpHorizontalSpeed = packet.ReadSingle("Horizontal Speed");
                    info.JumpSinAngle = packet.ReadSingle("Fall Sin");
                    info.JumpCosAngle = packet.ReadSingle("Fall Cos");
                }

                info.JumpVerticalSpeed = packet.ReadSingle("Vertical Speed");
                info.FallTime = packet.ReadUInt32("Fall time");
            }

            if (hasTrans)
            {
                var tpos = new Vector4();

                packet.ReadXORByte(transportGuid, 5);
                packet.ReadXORByte(transportGuid, 2);

                if (hasTransTime2)
                    packet.ReadUInt32("Transport Time 2");

                tpos.X = packet.ReadSingle();
                tpos.O = packet.ReadSingle();

                packet.ReadXORByte(transportGuid, 6);
                packet.ReadXORByte(transportGuid, 3);
                packet.ReadXORByte(transportGuid, 1);
                tpos.Z = packet.ReadSingle();

                if (hasTransTime3)
                    packet.ReadUInt32("Transport Time 3");

                packet.ReadXORByte(transportGuid, 0);
                packet.ReadXORByte(transportGuid, 4);
                info.TransportTime = packet.ReadUInt32("Transport Time");
                tpos.Y = packet.ReadSingle();
                info.TransportSeat = packet.ReadSByte("Transport Seat");
                packet.ReadXORByte(transportGuid, 7);

                info.TransportGuid = packet.WriteGuid("Transport Guid", transportGuid);
                info.TransportOffset = packet.AddValue("Transport Position", tpos);
            }

            if (hasSplineElev)
                info.SplineElevation = packet.ReadSingle("Spline elevation");
            if (hasO)
                info.Orientation = pos.O = packet.ReadSingle();
            if (hasPitch)
                info.SwimPitch = packet.ReadSingle("Pitch");

            WowGuid moverGuid = packet.WriteGuid("Guid", guid);
            packet.AddValue("Position", pos);
            Storage.StorePlayerMovement(moverGuid, info, packet);
        }

        [Parser(Opcode.MSG_MOVE_START_SWIM)]
        public static void HandleMoveStartSwim434(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();
            MovementInfo info = new MovementInfo();

            info.Position.Z = pos.Z = packet.ReadSingle();
            info.Position.X = pos.X = packet.ReadSingle();
            info.Position.Y = pos.Y = packet.ReadSingle();
            guid[3] = packet.ReadBit();
            var hasSplineElev = !packet.ReadBit("Has Spline Elevation");
            guid[4] = packet.ReadBit();
            guid[7] = packet.ReadBit();
            packet.ReadBit();
            var hasPitch = !packet.ReadBit("Has pitch");
            guid[0] = packet.ReadBit();
            var hasO = !packet.ReadBit();
            var hasMovementFlags = !packet.ReadBit();
            var hasFallData = packet.ReadBit("Has fall data");
            guid[5] = packet.ReadBit();
            var hasTrans = packet.ReadBit("Has transport");
            var hasMovementFlags2 = !packet.ReadBit();
            var hasTime = !packet.ReadBit("Has timestamp");
            guid[6] = packet.ReadBit();
            guid[1] = packet.ReadBit();
            packet.ReadBit("Has Spline");
            guid[2] = packet.ReadBit();

            if (hasTrans)
            {
                transportGuid[2] = packet.ReadBit();
                transportGuid[3] = packet.ReadBit();
                transportGuid[7] = packet.ReadBit();
                transportGuid[5] = packet.ReadBit();
                transportGuid[4] = packet.ReadBit();
                transportGuid[0] = packet.ReadBit();
                hasTransTime2 = packet.ReadBit();
                transportGuid[6] = packet.ReadBit();
                transportGuid[1] = packet.ReadBit();
                hasTransTime3 = packet.ReadBit();
            }

            if (hasMovementFlags)
                info.Flags = (uint)packet.ReadBitsE<MovementFlag>("Movement Flags", 30);

            if (hasFallData)
                hasFallDirection = packet.ReadBit();

            if (hasMovementFlags2)
                info.FlagsExtra = (uint)packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 12);

            packet.ParseBitStream(guid, 0, 2, 1, 5, 4, 6, 3, 7);

            if (hasPitch)
                info.SwimPitch = packet.ReadSingle("Pitch");

            if (hasTrans)
            {
                var tpos = new Vector4();

                if (hasTransTime2)
                    packet.ReadUInt32("Transport Time 2");

                packet.ReadXORByte(transportGuid, 2);
                tpos.Y = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 3);
                info.TransportTime = packet.ReadUInt32("Transport Time");
                tpos.X = packet.ReadSingle();
                tpos.O = packet.ReadSingle();

                if (hasTransTime3)
                    packet.ReadUInt32("Transport Time 3");

                packet.ReadXORByte(transportGuid, 6);
                packet.ReadXORByte(transportGuid, 5);
                packet.ReadXORByte(transportGuid, 4);
                tpos.Z = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 1);
                packet.ReadXORByte(transportGuid, 7);
                packet.ReadXORByte(transportGuid, 0);
                info.TransportSeat = packet.ReadSByte("Transport Seat");

                info.TransportGuid = packet.WriteGuid("Transport Guid", transportGuid);
                info.TransportOffset = packet.AddValue("Transport Position", tpos);
            }

            if (hasFallData)
            {
                if (hasFallDirection)
                {
                    info.JumpCosAngle = packet.ReadSingle("Fall Cos");
                    info.JumpHorizontalSpeed = packet.ReadSingle("Horizontal Speed");
                    info.JumpSinAngle = packet.ReadSingle("Fall Sin");
                }

                info.JumpVerticalSpeed = packet.ReadSingle("Vertical Speed");
                info.FallTime = packet.ReadUInt32("Fall time");
            }

            if (hasO)
                info.Orientation = pos.O = packet.ReadSingle();
            if (hasTime)
                info.MoveTime = packet.ReadUInt32("Timestamp");
            if (hasSplineElev)
                info.SplineElevation = packet.ReadSingle("Spline elevation");

            WowGuid moverGuid = packet.WriteGuid("Guid", guid);
            packet.AddValue("Position", pos);
            Storage.StorePlayerMovement(moverGuid, info, packet);
        }

        [Parser(Opcode.MSG_MOVE_STOP_SWIM)]
        public static void HandleMoveStopSwim434(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();
            MovementInfo info = new MovementInfo();

            info.Position.X = pos.X = packet.ReadSingle();
            info.Position.Y = pos.Y = packet.ReadSingle();
            info.Position.Z = pos.Z = packet.ReadSingle();
            packet.ReadBit("Has Spline");
            var hasPitch = !packet.ReadBit("Has pitch");
            guid[5] = packet.ReadBit();
            guid[3] = packet.ReadBit();
            guid[7] = packet.ReadBit();
            var hasSplineElev = !packet.ReadBit("Has Spline Elevation");
            var hasMovementFlags = !packet.ReadBit();
            guid[4] = packet.ReadBit();
            var hasMovementFlags2 = !packet.ReadBit();
            guid[2] = packet.ReadBit();
            guid[6] = packet.ReadBit();
            var hasTrans = packet.ReadBit("Has transport");
            var hasO = !packet.ReadBit();
            packet.ReadBit();
            var hasTime = !packet.ReadBit("Has timestamp");
            guid[1] = packet.ReadBit();
            guid[0] = packet.ReadBit();
            var hasFallData = packet.ReadBit("Has fall data");

            if (hasTrans)
            {
                hasTransTime3 = packet.ReadBit();
                transportGuid[4] = packet.ReadBit();
                transportGuid[6] = packet.ReadBit();
                transportGuid[3] = packet.ReadBit();
                transportGuid[7] = packet.ReadBit();
                transportGuid[1] = packet.ReadBit();
                transportGuid[0] = packet.ReadBit();
                hasTransTime2 = packet.ReadBit();
                transportGuid[2] = packet.ReadBit();
                transportGuid[5] = packet.ReadBit();
            }

            if (hasMovementFlags)
                info.Flags = (uint)packet.ReadBitsE<MovementFlag>("Movement Flags", 30);

            if (hasMovementFlags2)
                info.FlagsExtra = (uint)packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 12);

            if (hasFallData)
                hasFallDirection = packet.ReadBit();

            packet.ParseBitStream(guid, 0, 4, 3, 6, 7, 1, 5, 2);

            if (hasTrans)
            {
                var tpos = new Vector4();

                if (hasTransTime3)
                    packet.ReadUInt32("Transport Time 3");

                packet.ReadXORByte(transportGuid, 4);
                info.TransportTime = packet.ReadUInt32("Transport Time");
                tpos.Z = packet.ReadSingle();
                info.TransportSeat = packet.ReadSByte("Transport Seat");
                packet.ReadXORByte(transportGuid, 3);
                packet.ReadXORByte(transportGuid, 2);
                tpos.Y = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 7);
                packet.ReadXORByte(transportGuid, 1);
                packet.ReadXORByte(transportGuid, 6);
                packet.ReadXORByte(transportGuid, 5);

                if (hasTransTime2)
                    packet.ReadUInt32("Transport Time 2");

                tpos.X = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 0);
                tpos.O = packet.ReadSingle();

                info.TransportGuid = packet.WriteGuid("Transport Guid", transportGuid);
                info.TransportOffset = packet.AddValue("Transport Position", tpos);
            }

            if (hasFallData)
            {
                info.JumpVerticalSpeed = packet.ReadSingle("Vertical Speed");

                if (hasFallDirection)
                {
                    info.JumpCosAngle = packet.ReadSingle("Fall Cos");
                    info.JumpHorizontalSpeed = packet.ReadSingle("Horizontal Speed");
                    info.JumpSinAngle = packet.ReadSingle("Fall Sin");
                }

                info.FallTime = packet.ReadUInt32("Fall time");
            }

            if (hasTime)
                info.MoveTime = packet.ReadUInt32("Timestamp");
            if (hasPitch)
                info.SwimPitch = packet.ReadSingle("Pitch");
            if (hasO)
                info.Orientation = pos.O = packet.ReadSingle();
            if (hasSplineElev)
                info.SplineElevation = packet.ReadSingle("Spline elevation");

            WowGuid moverGuid = packet.WriteGuid("Guid", guid);
            packet.AddValue("Position", pos);
            Storage.StorePlayerMovement(moverGuid, info, packet);
        }

        [Parser(Opcode.SMSG_MOVE_SPLINE_SET_RUN_SPEED)]
        public static void HandleSplineSetRunSpeed434(Packet packet)
        {
            var guid = packet.StartBitStream(4, 0, 5, 7, 6, 3, 1, 2);
            packet.ParseBitStream(guid, 0, 7, 6, 5, 3, 4);
            CreatureSpeedUpdate speedUpdate = new CreatureSpeedUpdate();
            speedUpdate.SpeedType = SpeedType.Run;
            speedUpdate.SpeedRate = packet.ReadSingle("Speed") / MovementInfo.DEFAULT_RUN_SPEED;
            packet.ParseBitStream(guid, 2, 1);
            WowGuid moverGuid = packet.WriteGuid("Guid", guid);
            speedUpdate.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(packet.Time);
            Storage.StoreUnitSpeedUpdate(moverGuid, speedUpdate);
        }

        [Parser(Opcode.MSG_MOVE_FALL_LAND)]
        public static void HandleMoveFallLand434(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();
            MovementInfo info = new MovementInfo();

            info.Position.X = pos.X = packet.ReadSingle();
            info.Position.Y = pos.Y = packet.ReadSingle();
            info.Position.Z = pos.Z = packet.ReadSingle();
            var hasTrans = packet.ReadBit("Has transport");
            guid[7] = packet.ReadBit();
            guid[1] = packet.ReadBit();
            var hasMovementFlags2 = !packet.ReadBit();
            guid[3] = packet.ReadBit();
            var hasSplineElev = !packet.ReadBit("Has Spline Elevation");
            var hasO = !packet.ReadBit();
            guid[6] = packet.ReadBit();
            var hasTime = !packet.ReadBit("Has timestamp");
            packet.ReadBit();
            var hasPitch = !packet.ReadBit("Has pitch");
            guid[4] = packet.ReadBit();
            packet.ReadBit("Has Spline");
            guid[5] = packet.ReadBit();
            var hasMovementFlags = !packet.ReadBit();
            var hasFallData = packet.ReadBit("Has fall data");
            guid[0] = packet.ReadBit();
            guid[2] = packet.ReadBit();

            if (hasTrans)
            {
                transportGuid[0] = packet.ReadBit();
                transportGuid[5] = packet.ReadBit();
                transportGuid[3] = packet.ReadBit();
                transportGuid[2] = packet.ReadBit();
                hasTransTime3 = packet.ReadBit();
                hasTransTime2 = packet.ReadBit();
                transportGuid[6] = packet.ReadBit();
                transportGuid[4] = packet.ReadBit();
                transportGuid[1] = packet.ReadBit();
                transportGuid[7] = packet.ReadBit();
            }

            if (hasMovementFlags2)
                info.FlagsExtra = (uint)packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 12);

            if (hasMovementFlags)
                info.Flags = (uint)packet.ReadBitsE<MovementFlag>("Movement Flags", 30);

            if (hasFallData)
                hasFallDirection = packet.ReadBit();

            packet.ParseBitStream(guid, 1, 7, 4, 3, 6, 0, 2, 5);

            if (hasTrans)
            {
                var tpos = new Vector4();
                info.TransportTime = packet.ReadUInt32("Transport Time");
                tpos.Z = packet.ReadSingle();
                tpos.Y = packet.ReadSingle();
                tpos.X = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 5);

                if (hasTransTime3)
                    packet.ReadUInt32("Transport Time 3");

                packet.ReadXORByte(transportGuid, 1);
                info.TransportSeat = packet.ReadSByte("Transport Seat");

                if (hasTransTime2)
                    packet.ReadUInt32("Transport Time 2");

                packet.ReadXORByte(transportGuid, 4);
                tpos.O = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 0);
                packet.ReadXORByte(transportGuid, 7);
                packet.ReadXORByte(transportGuid, 3);
                packet.ReadXORByte(transportGuid, 6);
                packet.ReadXORByte(transportGuid, 2);

                info.TransportGuid = packet.WriteGuid("Transport Guid", transportGuid);
                info.TransportOffset = packet.AddValue("Transport Position", tpos);
            }

            if (hasFallData)
            {
                info.JumpVerticalSpeed = packet.ReadSingle("Vertical Speed");
                info.FallTime = packet.ReadUInt32("Fall time");

                if (hasFallDirection)
                {
                    info.JumpCosAngle = packet.ReadSingle("Fall Cos");
                    info.JumpHorizontalSpeed = packet.ReadSingle("Horizontal Speed");
                    info.JumpSinAngle = packet.ReadSingle("Fall Sin");
                }
            }

            if (hasSplineElev)
                info.SplineElevation = packet.ReadSingle("Spline elevation");
            if (hasTime)
                info.MoveTime = packet.ReadUInt32("Timestamp");
            if (hasPitch)
                info.SwimPitch = packet.ReadSingle("Pitch");
            if (hasO)
                info.Orientation = pos.O = packet.ReadSingle();

            WowGuid moverGuid = packet.WriteGuid("Guid", guid);
            packet.AddValue("Position", pos);
            Storage.StorePlayerMovement(moverGuid, info, packet);
        }

        [Parser(Opcode.MSG_MOVE_JUMP)]
        public static void HandleMoveJump434(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();
            MovementInfo info = new MovementInfo();

            info.Position.Y = pos.Y = packet.ReadSingle();
            info.Position.X = pos.X = packet.ReadSingle();
            info.Position.Z = pos.Z = packet.ReadSingle();
            var hasTime = !packet.ReadBit("Has timestamp");
            guid[5] = packet.ReadBit();
            var hasMovementFlags = !packet.ReadBit();
            guid[4] = packet.ReadBit();
            guid[6] = packet.ReadBit();
            var hasMovementFlags2 = !packet.ReadBit();
            guid[0] = packet.ReadBit();
            var hasTrans = packet.ReadBit("Has transport");
            guid[3] = packet.ReadBit();
            var hasPitch = !packet.ReadBit("Has pitch");
            guid[7] = packet.ReadBit();
            var hasFallData = packet.ReadBit("Has fall data");
            packet.ReadBit("Has Spline");
            var hasO = !packet.ReadBit();
            guid[1] = packet.ReadBit();
            var hasSplineElev = !packet.ReadBit("Has Spline Elevation");
            packet.ReadBit();
            guid[2] = packet.ReadBit();

            if (hasTrans)
            {
                transportGuid[3] = packet.ReadBit();
                transportGuid[7] = packet.ReadBit();
                transportGuid[6] = packet.ReadBit();
                transportGuid[0] = packet.ReadBit();
                transportGuid[5] = packet.ReadBit();
                transportGuid[1] = packet.ReadBit();
                transportGuid[4] = packet.ReadBit();
                hasTransTime3 = packet.ReadBit();
                hasTransTime2 = packet.ReadBit();
                transportGuid[2] = packet.ReadBit();
            }

            if (hasFallData)
                hasFallDirection = packet.ReadBit();

            if (hasMovementFlags)
                info.Flags = (uint)packet.ReadBitsE<MovementFlag>("Movement Flags", 30);

            if (hasMovementFlags2)
                info.FlagsExtra = (uint)packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 12);

            packet.ParseBitStream(guid, 6, 5, 4, 0, 2, 3, 7, 1);

            if (hasTrans)
            {
                var tpos = new Vector4();
                packet.ReadXORByte(transportGuid, 1);
                packet.ReadXORByte(transportGuid, 7);
                tpos.X = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 4);

                if (hasTransTime3)
                    packet.ReadUInt32("Transport Time 3");

                packet.ReadXORByte(transportGuid, 0);
                tpos.O = packet.ReadSingle();

                if (hasTransTime2)
                    packet.ReadUInt32("Transport Time 2");

                info.TransportTime = packet.ReadUInt32("Transport Time");

                packet.ReadXORByte(transportGuid, 6);
                tpos.Z = packet.ReadSingle();
                info.TransportSeat = packet.ReadSByte("Transport Seat");
                tpos.Y = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 3);
                packet.ReadXORByte(transportGuid, 2);
                packet.ReadXORByte(transportGuid, 5);

                info.TransportGuid = packet.WriteGuid("Transport Guid", transportGuid);
                info.TransportOffset = packet.AddValue("Transport Position", tpos);
            }

            if (hasPitch)
                info.SwimPitch = packet.ReadSingle("Pitch");
            if (hasTime)
                info.MoveTime = packet.ReadUInt32("Timestamp");

            if (hasFallData)
            {
                if (hasFallDirection)
                {
                    info.JumpCosAngle = packet.ReadSingle("Fall Cos");
                    info.JumpSinAngle = packet.ReadSingle("Fall Sin");
                    info.JumpHorizontalSpeed = packet.ReadSingle("Horizontal Speed");
                }

                info.JumpVerticalSpeed = packet.ReadSingle("Vertical Speed");
                info.FallTime = packet.ReadUInt32("Fall time");
            }

            if (hasSplineElev)
                info.SplineElevation = packet.ReadSingle("Spline elevation");
            if (hasO)
                info.Orientation = pos.O = packet.ReadSingle();

            WowGuid moverGuid = packet.WriteGuid("Guid", guid);
            packet.AddValue("Position", pos);
            Storage.StorePlayerMovement(moverGuid, info, packet);
        }

        [Parser(Opcode.MSG_MOVE_START_STRAFE_LEFT)]
        public static void HandleMoveStartStrafeLeft434(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();
            MovementInfo info = new MovementInfo();

            info.Position.Z = pos.Z = packet.ReadSingle();
            info.Position.X = pos.X = packet.ReadSingle();
            info.Position.Y = pos.Y = packet.ReadSingle();
            var hasSplineElev = !packet.ReadBit("Has Spline Elevation");
            guid[5] = packet.ReadBit();
            var hasPitch = !packet.ReadBit("Has pitch");
            guid[6] = packet.ReadBit();
            var hasTime = !packet.ReadBit("Has timestamp");
            guid[1] = packet.ReadBit();
            packet.ReadBit();
            guid[4] = packet.ReadBit();
            guid[0] = packet.ReadBit();
            guid[2] = packet.ReadBit();
            var hasFallData = packet.ReadBit("Has fall data");
            var hasO = !packet.ReadBit();
            guid[3] = packet.ReadBit();
            var hasMovementFlags2 = !packet.ReadBit();
            guid[7] = packet.ReadBit();
            packet.ReadBit("Has Spline");
            var hasMovementFlags = !packet.ReadBit();
            var hasTrans = packet.ReadBit("Has transport");

            if (hasFallData)
                hasFallDirection = packet.ReadBit();

            if (hasTrans)
            {
                hasTransTime2 = packet.ReadBit();
                transportGuid[7] = packet.ReadBit();
                hasTransTime3 = packet.ReadBit();
                transportGuid[3] = packet.ReadBit();
                transportGuid[1] = packet.ReadBit();
                transportGuid[6] = packet.ReadBit();
                transportGuid[5] = packet.ReadBit();
                transportGuid[2] = packet.ReadBit();
                transportGuid[4] = packet.ReadBit();
                transportGuid[0] = packet.ReadBit();
            }

            if (hasMovementFlags)
                info.Flags = (uint)packet.ReadBitsE<MovementFlag>("Movement Flags", 30);

            if (hasMovementFlags2)
                info.FlagsExtra = (uint)packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 12);

            packet.ParseBitStream(guid, 2, 6, 3, 1, 0, 7, 4, 5);

            if (hasFallData)
            {
                if (hasFallDirection)
                {
                    info.JumpSinAngle = packet.ReadSingle("Fall Sin");
                    info.JumpHorizontalSpeed = packet.ReadSingle("Horizontal Speed");
                    info.JumpCosAngle = packet.ReadSingle("Fall Cos");
                }

                info.FallTime = packet.ReadUInt32("Fall time");
                info.JumpVerticalSpeed = packet.ReadSingle("Vertical Speed");
            }

            if (hasTrans)
            {
                var tpos = new Vector4();
                info.TransportSeat = packet.ReadSByte("Transport Seat");
                packet.ReadXORByte(transportGuid, 2);

                if (hasTransTime2)
                    packet.ReadUInt32("Transport Time 2");

                packet.ReadXORByte(transportGuid, 3);
                tpos.Z = packet.ReadSingle();

                if (hasTransTime3)
                    packet.ReadUInt32("Transport Time 3");

                packet.ReadXORByte(transportGuid, 0);
                packet.ReadXORByte(transportGuid, 7);
                tpos.Y = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 5);
                packet.ReadXORByte(transportGuid, 1);
                tpos.O = packet.ReadSingle();
                info.TransportTime = packet.ReadUInt32("Transport Time");
                packet.ReadXORByte(transportGuid, 6);
                packet.ReadXORByte(transportGuid, 4);
                tpos.X = packet.ReadSingle();

                info.TransportGuid = packet.WriteGuid("Transport Guid", transportGuid);
                info.TransportOffset = packet.AddValue("Transport Position", tpos);
            }

            if (hasTime)
                info.MoveTime = packet.ReadUInt32("Timestamp");
            if (hasO)
                info.Orientation = pos.O = packet.ReadSingle();
            if (hasPitch)
                info.SwimPitch = packet.ReadSingle("Pitch");
            if (hasSplineElev)
                info.SplineElevation = packet.ReadSingle("Spline elevation");

            WowGuid moverGuid = packet.WriteGuid("Guid", guid);
            packet.AddValue("Position", pos);
            Storage.StorePlayerMovement(moverGuid, info, packet);
        }

        [Parser(Opcode.MSG_MOVE_START_STRAFE_RIGHT)]
        public static void HandleMoveStartStrafeRight434(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();
            MovementInfo info = new MovementInfo();

            info.Position.Y = pos.Y = packet.ReadSingle();
            info.Position.X = pos.X = packet.ReadSingle();
            info.Position.Z = pos.Z = packet.ReadSingle();
            var hasPitch = !packet.ReadBit("Has pitch");
            guid[1] = packet.ReadBit();
            var hasO = !packet.ReadBit();
            guid[4] = packet.ReadBit();
            packet.ReadBit("Has Spline");
            packet.ReadBit();
            guid[5] = packet.ReadBit();
            var hasFallData = packet.ReadBit("Has fall data");
            var hasSplineElev = !packet.ReadBit("Has Spline Elevation");
            var hasTime = !packet.ReadBit("Has timestamp");
            var hasMovementFlags = !packet.ReadBit();
            guid[2] = packet.ReadBit();
            guid[7] = packet.ReadBit();
            guid[6] = packet.ReadBit();
            guid[3] = packet.ReadBit();
            var hasMovementFlags2 = !packet.ReadBit();
            var hasTrans = packet.ReadBit("Has transport");
            guid[0] = packet.ReadBit();

            if (hasTrans)
            {
                transportGuid[7] = packet.ReadBit();
                transportGuid[2] = packet.ReadBit();
                transportGuid[3] = packet.ReadBit();
                transportGuid[5] = packet.ReadBit();
                transportGuid[0] = packet.ReadBit();
                hasTransTime2 = packet.ReadBit();
                transportGuid[6] = packet.ReadBit();
                transportGuid[1] = packet.ReadBit();
                transportGuid[4] = packet.ReadBit();
                hasTransTime3 = packet.ReadBit();
            }

            if (hasMovementFlags2)
                info.FlagsExtra = (uint)packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 12);

            if (hasMovementFlags)
                info.Flags = (uint)packet.ReadBitsE<MovementFlag>("Movement Flags", 30);

            if (hasFallData)
                hasFallDirection = packet.ReadBit();

            packet.ParseBitStream(guid, 7, 5, 3, 1, 2, 4, 6, 0);

            if (hasTrans)
            {
                var tpos = new Vector4();
                packet.ReadXORByte(transportGuid, 5);
                packet.ReadXORByte(transportGuid, 1);
                packet.ReadXORByte(transportGuid, 6);
                tpos.Y = packet.ReadSingle();
                tpos.O = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 0);
                packet.ReadXORByte(transportGuid, 2);
                info.TransportSeat = packet.ReadSByte("Transport Seat");
                tpos.X = packet.ReadSingle();

                if (hasTransTime3)
                    packet.ReadUInt32("Transport Time 3");

                info.TransportTime = packet.ReadUInt32("Transport Time");
                packet.ReadXORByte(transportGuid, 4);
                packet.ReadXORByte(transportGuid, 7);

                if (hasTransTime2)
                    packet.ReadUInt32("Transport Time 2");

                tpos.Z = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 3);

                info.TransportGuid = packet.WriteGuid("Transport Guid", transportGuid);
                info.TransportOffset = packet.AddValue("Transport Position", tpos);
            }

            if (hasPitch)
                info.SwimPitch = packet.ReadSingle("Pitch");
            if (hasO)
                info.Orientation = pos.O = packet.ReadSingle();

            if (hasFallData)
            {
                if (hasFallDirection)
                {
                    info.JumpCosAngle = packet.ReadSingle("Fall Cos");
                    info.JumpSinAngle = packet.ReadSingle("Fall Sin");
                    info.JumpHorizontalSpeed = packet.ReadSingle("Horizontal Speed");
                }

                info.FallTime = packet.ReadUInt32("Fall time");
                info.JumpVerticalSpeed = packet.ReadSingle("Vertical Speed");
            }

            if (hasTime)
                info.MoveTime = packet.ReadUInt32("Timestamp");
            if (hasSplineElev)
                info.SplineElevation = packet.ReadSingle("Spline elevation");

            WowGuid moverGuid = packet.WriteGuid("Guid", guid);
            packet.AddValue("Position", pos);
            Storage.StorePlayerMovement(moverGuid, info, packet);
        }

        [Parser(Opcode.MSG_MOVE_STOP_STRAFE)]
        public static void HandleMoveStopStrafe434(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();
            MovementInfo info = new MovementInfo();

            info.Position.Y = pos.Y = packet.ReadSingle();
            info.Position.Z = pos.Z = packet.ReadSingle();
            info.Position.X = pos.X = packet.ReadSingle();
            var hasPitch = !packet.ReadBit("Has pitch");
            var hasTime = !packet.ReadBit("Has timestamp");
            guid[2] = packet.ReadBit();
            var hasFallData = packet.ReadBit("Has fall data");
            guid[7] = packet.ReadBit();
            var hasSplineElev = !packet.ReadBit("Has Spline Elevation");
            guid[3] = packet.ReadBit();
            var hasO = !packet.ReadBit();
            var hasMovementFlags2 = !packet.ReadBit();
            var hasTrans = packet.ReadBit("Has transport");
            var hasMovementFlags = !packet.ReadBit();
            packet.ReadBit("Has Spline");
            guid[0] = packet.ReadBit();
            packet.ReadBit();
            guid[6] = packet.ReadBit();
            guid[5] = packet.ReadBit();
            guid[1] = packet.ReadBit();
            guid[4] = packet.ReadBit();

            if (hasTrans)
            {
                transportGuid[7] = packet.ReadBit();
                transportGuid[0] = packet.ReadBit();
                transportGuid[4] = packet.ReadBit();
                transportGuid[2] = packet.ReadBit();
                transportGuid[6] = packet.ReadBit();
                transportGuid[5] = packet.ReadBit();
                hasTransTime2 = packet.ReadBit();
                transportGuid[1] = packet.ReadBit();
                transportGuid[3] = packet.ReadBit();
                hasTransTime3 = packet.ReadBit();
            }

            if (hasMovementFlags)
                info.Flags = (uint)packet.ReadBitsE<MovementFlag>("Movement Flags", 30);

            if (hasFallData)
                hasFallDirection = packet.ReadBit();

            if (hasMovementFlags2)
                info.FlagsExtra = (uint)packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 12);

            packet.ParseBitStream(guid, 2, 7, 3, 4, 5, 6, 1, 0);

            if (hasTrans)
            {
                var tpos = new Vector4();
                info.TransportSeat = packet.ReadSByte("Transport Seat");
                packet.ReadXORByte(transportGuid, 6);
                tpos.Z = packet.ReadSingle();

                if (hasTransTime3)
                    packet.ReadUInt32("Transport Time 3");

                packet.ReadXORByte(transportGuid, 1);
                packet.ReadXORByte(transportGuid, 3);
                packet.ReadXORByte(transportGuid, 2);
                packet.ReadXORByte(transportGuid, 4);
                packet.ReadXORByte(transportGuid, 5);
                info.TransportTime = packet.ReadUInt32("Transport Time");
                tpos.O = packet.ReadSingle();
                tpos.X = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 0);
                tpos.Y = packet.ReadSingle();

                if (hasTransTime2)
                    packet.ReadUInt32("Transport Time 2");

                packet.ReadXORByte(transportGuid, 7);

                info.TransportGuid = packet.WriteGuid("Transport Guid", transportGuid);
                info.TransportOffset = packet.AddValue("Transport Position", tpos);
            }

            if (hasFallData)
            {
                if (hasFallDirection)
                {
                    info.JumpCosAngle = packet.ReadSingle("Fall Cos");
                    info.JumpHorizontalSpeed = packet.ReadSingle("Horizontal Speed");
                    info.JumpSinAngle = packet.ReadSingle("Fall Sin");
                }

                info.FallTime = packet.ReadUInt32("Fall time");
                info.JumpVerticalSpeed = packet.ReadSingle("Vertical Speed");
            }

            if (hasSplineElev)
                info.SplineElevation = packet.ReadSingle("Spline elevation");
            if (hasO)
                info.Orientation = pos.O = packet.ReadSingle();
            if (hasPitch)
                info.SwimPitch = packet.ReadSingle("Pitch");
            if (hasTime)
                info.MoveTime = packet.ReadUInt32("Timestamp");

            WowGuid moverGuid = packet.WriteGuid("Guid", guid);
            packet.AddValue("Position", pos);
            Storage.StorePlayerMovement(moverGuid, info, packet);
        }

        [Parser(Opcode.MSG_MOVE_START_BACKWARD)]
        public static void HandleMoveStartBackward434(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();
            MovementInfo info = new MovementInfo();

            info.Position.X = pos.X = packet.ReadSingle();
            info.Position.Z = pos.Z = packet.ReadSingle();
            info.Position.Y = pos.Y = packet.ReadSingle();
            var hasTrans = packet.ReadBit("Has transport");
            guid[3] = packet.ReadBit();
            guid[0] = packet.ReadBit();
            guid[2] = packet.ReadBit();
            var hasTime = !packet.ReadBit("Has timestamp");
            guid[7] = packet.ReadBit();
            var hasPitch = !packet.ReadBit("Has pitch");
            packet.ReadBit();
            var hasMovementFlags = !packet.ReadBit();
            var hasO = !packet.ReadBit();
            packet.ReadBit("Has Spline");
            var hasMovementFlags2 = !packet.ReadBit();
            var hasFallData = packet.ReadBit("Has fall data");
            guid[5] = packet.ReadBit();
            guid[1] = packet.ReadBit();
            guid[4] = packet.ReadBit();
            guid[6] = packet.ReadBit();
            var hasSplineElev = !packet.ReadBit("Has Spline Elevation");

            if (hasTrans)
            {
                transportGuid[3] = packet.ReadBit();
                transportGuid[1] = packet.ReadBit();
                hasTransTime2 = packet.ReadBit();
                transportGuid[2] = packet.ReadBit();
                transportGuid[7] = packet.ReadBit();
                transportGuid[0] = packet.ReadBit();
                transportGuid[6] = packet.ReadBit();
                transportGuid[5] = packet.ReadBit();
                transportGuid[4] = packet.ReadBit();
                hasTransTime3 = packet.ReadBit();
            }

            if (hasMovementFlags)
                info.Flags = (uint)packet.ReadBitsE<MovementFlag>("Movement Flags", 30);

            if (hasMovementFlags2)
                info.FlagsExtra = (uint)packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 12);

            if (hasFallData)
                hasFallDirection = packet.ReadBit();

            packet.ParseBitStream(guid, 6, 7, 4, 1, 5, 0, 2, 3);

            if (hasTrans)
            {
                var tpos = new Vector4();
                tpos.Z = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 2);

                if (hasTransTime3)
                    packet.ReadUInt32("Transport Time 3");

                packet.ReadXORByte(transportGuid, 0);
                packet.ReadXORByte(transportGuid, 5);
                tpos.Y = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 1);
                tpos.X = packet.ReadSingle();

                if (hasTransTime2)
                    packet.ReadUInt32("Transport Time 2");

                packet.ReadXORByte(transportGuid, 4);
                tpos.O = packet.ReadSingle();
                info.TransportSeat = packet.ReadSByte("Transport Seat");
                packet.ReadXORByte(transportGuid, 7);
                info.TransportTime = packet.ReadUInt32("Transport Time");
                packet.ReadXORByte(transportGuid, 6);
                packet.ReadXORByte(transportGuid, 3);

                info.TransportGuid = packet.WriteGuid("Transport Guid", transportGuid);
                info.TransportOffset = packet.AddValue("Transport Position", tpos);
            }

            if (hasPitch)
                info.SwimPitch = packet.ReadSingle("Pitch");
            if (hasTime)
                info.MoveTime = packet.ReadUInt32("Timestamp");

            if (hasFallData)
            {
                if (hasFallDirection)
                {
                    info.JumpHorizontalSpeed = packet.ReadSingle("Horizontal Speed");
                    info.JumpSinAngle = packet.ReadSingle("Fall Sin");
                    info.JumpCosAngle = packet.ReadSingle("Fall Cos");
                }

                info.JumpVerticalSpeed = packet.ReadSingle("Vertical Speed");
                info.FallTime = packet.ReadUInt32("Fall time");
            }

            if (hasO)
                info.Orientation = pos.O = packet.ReadSingle();
            if (hasSplineElev)
                info.SplineElevation = packet.ReadSingle("Spline elevation");

            WowGuid moverGuid = packet.WriteGuid("Guid", guid);
            packet.AddValue("Position", pos);
            Storage.StorePlayerMovement(moverGuid, info, packet);
        }

        [Parser(Opcode.MSG_MOVE_START_TURN_LEFT)]
        public static void HandleMoveStartTurnLeft434(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();
            MovementInfo info = new MovementInfo();

            info.Position.Y = pos.Y = packet.ReadSingle();
            info.Position.X = pos.X = packet.ReadSingle();
            info.Position.Z = pos.Z = packet.ReadSingle();
            packet.ReadBit();
            guid[1] = packet.ReadBit();
            var hasO = !packet.ReadBit();
            packet.ReadBit("Has Spline");
            var hasMovementFlags = !packet.ReadBit();
            guid[4] = packet.ReadBit();
            guid[2] = packet.ReadBit();
            var hasMovementFlags2 = !packet.ReadBit();
            guid[5] = packet.ReadBit();
            guid[7] = packet.ReadBit();
            var hasTrans = packet.ReadBit("Has transport");
            guid[6] = packet.ReadBit();
            var hasSplineElev = !packet.ReadBit("Has Spline Elevation");
            guid[0] = packet.ReadBit();
            guid[3] = packet.ReadBit();
            var hasPitch = !packet.ReadBit("Has pitch");
            var hasTime = !packet.ReadBit("Has timestamp");
            var hasFallData = packet.ReadBit("Has fall data");

            if (hasMovementFlags2)
                info.FlagsExtra = (uint)packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 12);

            if (hasTrans)
            {
                transportGuid[3] = packet.ReadBit();
                transportGuid[1] = packet.ReadBit();
                transportGuid[0] = packet.ReadBit();
                hasTransTime2 = packet.ReadBit();
                transportGuid[2] = packet.ReadBit();
                hasTransTime3 = packet.ReadBit();
                transportGuid[4] = packet.ReadBit();
                transportGuid[7] = packet.ReadBit();
                transportGuid[5] = packet.ReadBit();
                transportGuid[6] = packet.ReadBit();
            }

            if (hasFallData)
                hasFallDirection = packet.ReadBit();

            if (hasMovementFlags)
                info.Flags = (uint)packet.ReadBitsE<MovementFlag>("Movement Flags", 30);

            packet.ParseBitStream(guid, 0, 4, 7, 5, 6, 3, 2, 1);

            if (hasFallData)
            {
                if (hasFallDirection)
                {
                    info.JumpSinAngle = packet.ReadSingle("Fall Sin");
                    info.JumpCosAngle = packet.ReadSingle("Fall Cos");
                    info.JumpHorizontalSpeed = packet.ReadSingle("Horizontal Speed");
                }

                info.JumpVerticalSpeed = packet.ReadSingle("Vertical Speed");
                info.FallTime = packet.ReadUInt32("Fall time");
            }

            if (hasTrans)
            {
                var tpos = new Vector4();
                packet.ReadXORByte(transportGuid, 0);
                tpos.X = packet.ReadSingle();
                info.TransportTime = packet.ReadUInt32("Transport Time");
                info.TransportSeat = packet.ReadSByte("Transport Seat");
                tpos.Z = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 4);
                tpos.O = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 2);
                packet.ReadXORByte(transportGuid, 6);
                packet.ReadXORByte(transportGuid, 1);
                packet.ReadXORByte(transportGuid, 3);
                tpos.Y = packet.ReadSingle();

                if (hasTransTime3)
                    packet.ReadUInt32("Transport Time 3");

                if (hasTransTime2)
                    packet.ReadUInt32("Transport Time 2");

                packet.ReadXORByte(transportGuid, 5);
                packet.ReadXORByte(transportGuid, 7);

                info.TransportGuid = packet.WriteGuid("Transport Guid", transportGuid);
                info.TransportOffset = packet.AddValue("Transport Position", tpos);
            }

            if (hasTime)
                info.MoveTime = packet.ReadUInt32("Timestamp");
            if (hasPitch)
                info.SwimPitch = packet.ReadSingle("Pitch");
            if (hasO)
                info.Orientation = pos.O = packet.ReadSingle();
            if (hasSplineElev)
                info.SplineElevation = packet.ReadSingle("Spline elevation");

            WowGuid moverGuid = packet.WriteGuid("Guid", guid);
            packet.AddValue("Position", pos);
            Storage.StorePlayerMovement(moverGuid, info, packet);
        }

        [Parser(Opcode.MSG_MOVE_START_TURN_RIGHT)]
        public static void HandleMoveStartTurnRight434(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();
            MovementInfo info = new MovementInfo();

            info.Position.X = pos.X = packet.ReadSingle();
            info.Position.Z = pos.Z = packet.ReadSingle();
            info.Position.Y = pos.Y = packet.ReadSingle();
            guid[3] = packet.ReadBit();
            guid[5] = packet.ReadBit();
            var hasMovementFlags = !packet.ReadBit();
            packet.ReadBit("Has Spline");
            guid[0] = packet.ReadBit();
            var hasO = !packet.ReadBit();
            var hasTrans = packet.ReadBit("Has transport");
            guid[7] = packet.ReadBit();
            packet.ReadBit();
            var hasMovementFlags2 = !packet.ReadBit();
            guid[1] = packet.ReadBit();
            var hasTime = !packet.ReadBit("Has timestamp");
            guid[6] = packet.ReadBit();
            guid[2] = packet.ReadBit();
            guid[4] = packet.ReadBit();
            var hasSplineElev = !packet.ReadBit("Has Spline Elevation");
            var hasPitch = !packet.ReadBit("Has pitch");
            var hasFallData = packet.ReadBit("Has fall data");

            if (hasTrans)
            {
                transportGuid[1] = packet.ReadBit();
                hasTransTime2 = packet.ReadBit();
                transportGuid[6] = packet.ReadBit();
                transportGuid[0] = packet.ReadBit();
                transportGuid[3] = packet.ReadBit();
                transportGuid[5] = packet.ReadBit();
                transportGuid[2] = packet.ReadBit();
                transportGuid[4] = packet.ReadBit();
                transportGuid[7] = packet.ReadBit();
                hasTransTime3 = packet.ReadBit();
            }

            if (hasMovementFlags2)
                info.FlagsExtra = (uint)packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 12);

            if (hasMovementFlags)
                info.Flags = (uint)packet.ReadBitsE<MovementFlag>("Movement Flags", 30);

            if (hasFallData)
                hasFallDirection = packet.ReadBit();

            packet.ParseBitStream(guid, 5, 0, 7, 3, 2, 1, 4, 6);

            if (hasTrans)
            {
                var tpos = new Vector4();
                tpos.Y = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 0);
                packet.ReadXORByte(transportGuid, 4);
                packet.ReadXORByte(transportGuid, 1);
                packet.ReadXORByte(transportGuid, 6);
                packet.ReadXORByte(transportGuid, 2);
                info.TransportSeat = packet.ReadSByte("Transport Seat");
                tpos.O = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 5);

                if (hasTransTime3)
                    packet.ReadUInt32("Transport Time 3");

                tpos.Z = packet.ReadSingle();
                tpos.X = packet.ReadSingle();
                info.TransportTime = packet.ReadUInt32("Transport Time");
                packet.ReadXORByte(transportGuid, 7);
                packet.ReadXORByte(transportGuid, 3);

                if (hasTransTime2)
                    packet.ReadUInt32("Transport Time 2");

                info.TransportGuid = packet.WriteGuid("Transport Guid", transportGuid);
                info.TransportOffset = packet.AddValue("Transport Position", tpos);
            }

            if (hasFallData)
            {
                if (hasFallDirection)
                {
                    info.JumpHorizontalSpeed = packet.ReadSingle("Horizontal Speed");
                    info.JumpSinAngle = packet.ReadSingle("Fall Sin");
                    info.JumpCosAngle = packet.ReadSingle("Fall Cos");
                }

                info.FallTime = packet.ReadUInt32("Fall time");
                info.JumpVerticalSpeed = packet.ReadSingle("Vertical Speed");
            }

            if (hasPitch)
                info.SwimPitch = packet.ReadSingle("Pitch");
            if (hasO)
                info.Orientation = pos.O = packet.ReadSingle();
            if (hasSplineElev)
                info.SplineElevation = packet.ReadSingle("Spline elevation");
            if (hasTime)
                info.MoveTime = packet.ReadUInt32("Timestamp");

            WowGuid moverGuid = packet.WriteGuid("Guid", guid);
            packet.AddValue("Position", pos);
            Storage.StorePlayerMovement(moverGuid, info, packet);
        }

        [Parser(Opcode.MSG_MOVE_STOP_TURN)]
        public static void HandleMoveStopTurn434(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();
            MovementInfo info = new MovementInfo();

            info.Position.X = pos.X = packet.ReadSingle();
            info.Position.Z = pos.Z = packet.ReadSingle();
            info.Position.Y = pos.Y = packet.ReadSingle();
            guid[5] = packet.ReadBit();
            guid[4] = packet.ReadBit();
            var hasFallData = packet.ReadBit("Has fall data");
            packet.ReadBit();
            guid[1] = packet.ReadBit();
            guid[0] = packet.ReadBit();
            packet.ReadBit("Has Spline");
            var hasMovementFlags = !packet.ReadBit();
            guid[2] = packet.ReadBit();
            guid[6] = packet.ReadBit();
            var hasPitch = !packet.ReadBit("Has pitch");
            var hasTrans = packet.ReadBit("Has transport");
            guid[3] = packet.ReadBit();
            var hasSplineElev = !packet.ReadBit("Has Spline Elevation");
            var hasTime = !packet.ReadBit("Has timestamp");
            var hasMovementFlags2 = !packet.ReadBit();
            var hasO = !packet.ReadBit();
            guid[7] = packet.ReadBit();

            if (hasTrans)
            {
                transportGuid[2] = packet.ReadBit();
                transportGuid[7] = packet.ReadBit();
                hasTransTime3 = packet.ReadBit();
                transportGuid[4] = packet.ReadBit();
                hasTransTime2 = packet.ReadBit();
                transportGuid[5] = packet.ReadBit();
                transportGuid[0] = packet.ReadBit();
                transportGuid[3] = packet.ReadBit();
                transportGuid[6] = packet.ReadBit();
                transportGuid[1] = packet.ReadBit();
            }

            if (hasFallData)
                hasFallDirection = packet.ReadBit();

            if (hasMovementFlags)
                info.Flags = (uint)packet.ReadBitsE<MovementFlag>("Movement Flags", 30);

            if (hasMovementFlags2)
                info.FlagsExtra = (uint)packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 12);

            packet.ParseBitStream(guid, 3, 2, 6, 4, 0, 7, 1, 5);

            if (hasSplineElev)
                info.SplineElevation = packet.ReadSingle("Spline elevation");

            if (hasTrans)
            {
                var tpos = new Vector4();
                tpos.X = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 5);
                info.TransportSeat = packet.ReadSByte("Transport Seat");
                packet.ReadXORByte(transportGuid, 2);
                packet.ReadXORByte(transportGuid, 3);
                tpos.O = packet.ReadSingle();

                if (hasTransTime2)
                    packet.ReadUInt32("Transport Time 2");

                if (hasTransTime3)
                    packet.ReadUInt32("Transport Time 3");

                packet.ReadXORByte(transportGuid, 7);
                packet.ReadXORByte(transportGuid, 1);
                packet.ReadXORByte(transportGuid, 0);
                packet.ReadXORByte(transportGuid, 4);
                tpos.Y = packet.ReadSingle();
                tpos.Z = packet.ReadSingle();

                info.TransportTime = packet.ReadUInt32("Transport Time");
                packet.ReadXORByte(transportGuid, 6);

                info.TransportGuid = packet.WriteGuid("Transport Guid", transportGuid);
                info.TransportOffset = packet.AddValue("Transport Position", tpos);
            }

            if (hasFallData)
            {
                info.FallTime = packet.ReadUInt32("Fall time");
                if (hasFallDirection)
                {
                    info.JumpHorizontalSpeed = packet.ReadSingle("Horizontal Speed");
                    info.JumpCosAngle = packet.ReadSingle("Fall Cos");
                    info.JumpSinAngle = packet.ReadSingle("Fall Sin");
                }

                info.JumpVerticalSpeed = packet.ReadSingle("Vertical Speed");
            }

            if (hasTime)
                info.MoveTime = packet.ReadUInt32("Timestamp");
            if (hasPitch)
                info.SwimPitch = packet.ReadSingle("Pitch");
            if (hasO)
                info.Orientation = pos.O = packet.ReadSingle();

            WowGuid moverGuid = packet.WriteGuid("Guid", guid);
            packet.AddValue("Position", pos);
            Storage.StorePlayerMovement(moverGuid, info, packet);
        }

        [Parser(Opcode.CMSG_SET_ACTIVE_MOVER)]
        public static void HandleSetActiveMover434(Packet packet)
        {
            var guid = packet.StartBitStream(7, 2, 1, 0, 4, 5, 6, 3);
            packet.ParseBitStream(guid, 3, 2, 4, 0, 5, 1, 6, 7);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_PHASE_SHIFT_CHANGE)]
        public static void HandlePhaseShift434(Packet packet)
        {
            CoreParsers.MovementHandler.ActivePhases.Clear();

            var guid = packet.StartBitStream(2, 3, 1, 6, 4, 5, 0, 7);

            packet.ReadXORByte(guid, 7);
            packet.ReadXORByte(guid, 4);

            var uiWorldMapAreaIDSwapsCount = packet.ReadUInt32("UiWorldMapAreaIDSwap") / 2;
            for (var i = 0; i < uiWorldMapAreaIDSwapsCount; ++i)
                packet.ReadInt16("UiWorldMapAreaIDSwaps", i);

            packet.ReadXORByte(guid, 1);

            packet.ReadUInt32("PhaseShiftFlags");

            packet.ReadXORByte(guid, 2);
            packet.ReadXORByte(guid, 6);

            var preloadMapIDCount = packet.ReadUInt32("PreloadMapIDsCount") / 2;
            for (var i = 0; i < preloadMapIDCount; ++i)
                packet.ReadInt16<MapId>("PreloadMapID", i);

            var count = packet.ReadUInt32("PhaseShiftCount") / 2;
            for (var i = 0; i < count; ++i)
            {
                var id = packet.ReadUInt16("Id", i);
                CoreParsers.MovementHandler.ActivePhases.Add(id, true);
            }

            if (DBC.Phases.Any())
            {
                foreach (var phaseGroup in DBC.GetPhaseGroups(CoreParsers.MovementHandler.ActivePhases.Keys))
                    packet.WriteLine($"PhaseGroup: { phaseGroup } Phases: { string.Join(" - ", DBC.Phases[phaseGroup]) }");
            }

            packet.ReadXORByte(guid, 3);
            packet.ReadXORByte(guid, 0);

            var visibleMapIDsCount = packet.ReadUInt32("VisibleMapIDsCount") / 2;
            for (var i = 0; i < visibleMapIDsCount; ++i)
                packet.ReadInt16<MapId>("VisibleMapID", i);

            packet.ReadXORByte(guid, 5);
            packet.WriteGuid("Client", guid);
        }

        [Parser(Opcode.SMSG_TRANSFER_PENDING)]
        public static void HandleTransferPending434(Packet packet)
        {
            // s_customLoadScreenSpellID
            var customLoadScreenSpell = packet.ReadBit("HasTransferSpellID");
            var hasTransport = packet.ReadBit("HasShip");
            if (hasTransport)
            {
                packet.ReadInt32<MapId>("ShipOriginMapID");
                packet.ReadInt32("ShipID");
            }

            if (customLoadScreenSpell)
                packet.ReadUInt32<SpellId>("TransferSpellID");

            packet.ReadInt32<MapId>("MapID");
        }

        [Parser(Opcode.CMSG_MOVE_TIME_SKIPPED)]
        public static void HandleMoveTimeSkipped434(Packet packet)
        {
            packet.ReadUInt32("Time");
            var guid = packet.StartBitStream(5, 1, 3, 7, 6, 0, 4, 2);
            packet.ParseBitStream(guid, 7, 1, 2, 4, 3, 6, 0, 5);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_MOVE_SPLINE_SET_FLIGHT_SPEED)]
        public static void HandleSplineSetFlightSpeed434(Packet packet)
        {
            var guid = packet.StartBitStream(7, 4, 0, 1, 3, 6, 5, 2);
            packet.ParseBitStream(guid, 0, 5, 4, 7, 3, 2, 1, 6);
            CreatureSpeedUpdate speedUpdate = new CreatureSpeedUpdate();
            speedUpdate.SpeedType = SpeedType.Fly;
            speedUpdate.SpeedRate = packet.ReadSingle("Speed") / MovementInfo.DEFAULT_FLY_SPEED;
            WowGuid moverGuid = packet.WriteGuid("Guid", guid);
            speedUpdate.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(packet.Time);
            Storage.StoreUnitSpeedUpdate(moverGuid, speedUpdate);
        }

        [Parser(Opcode.SMSG_MOVE_SPLINE_SET_SWIM_SPEED)]
        public static void HandleSplineSetSwimSpeed434(Packet packet)
        {
            var guid = packet.StartBitStream(4, 2, 5, 0, 7, 6, 3, 1);
            packet.ReadXORByte(guid, 5);
            packet.ReadXORByte(guid, 6);
            packet.ReadXORByte(guid, 1);
            packet.ReadXORByte(guid, 0);
            packet.ReadXORByte(guid, 2);
            packet.ReadXORByte(guid, 4);
            CreatureSpeedUpdate speedUpdate = new CreatureSpeedUpdate();
            speedUpdate.SpeedType = SpeedType.Swim;
            speedUpdate.SpeedRate = packet.ReadSingle("Speed") / MovementInfo.DEFAULT_SWIM_SPEED;
            packet.ReadXORByte(guid, 7);
            packet.ReadXORByte(guid, 3);
            WowGuid moverGuid = packet.WriteGuid("Guid", guid);
            speedUpdate.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(packet.Time);
            Storage.StoreUnitSpeedUpdate(moverGuid, speedUpdate);
        }

        [Parser(Opcode.SMSG_MOVE_SPLINE_SET_WALK_BACK_SPEED)]
        public static void HandleSplineSetWalkSpeed434(Packet packet)
        {
            var guid = packet.StartBitStream(0, 6, 7, 3, 5, 1, 2, 4);
            packet.ReadXORByte(guid, 0);
            packet.ReadXORByte(guid, 4);
            packet.ReadXORByte(guid, 7);
            packet.ReadXORByte(guid, 1);
            packet.ReadXORByte(guid, 5);
            packet.ReadXORByte(guid, 3);
            packet.ReadSingle("Speed");
            packet.ReadXORByte(guid, 6);
            packet.ReadXORByte(guid, 2);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_MOVE_SPLINE_SET_RUN_BACK_SPEED)]
        public static void HandleSplineSetRunBackSpeed434(Packet packet)
        {
            var guid = packet.StartBitStream(1, 2, 6, 0, 3, 7, 5, 4);
            packet.ReadXORByte(guid, 1);
            CreatureSpeedUpdate speedUpdate = new CreatureSpeedUpdate();
            speedUpdate.SpeedType = SpeedType.RunBack;
            speedUpdate.SpeedRate = packet.ReadSingle("Speed") / MovementInfo.DEFAULT_RUN_BACK_SPEED;
            packet.ReadXORByte(guid, 2);
            packet.ReadXORByte(guid, 4);
            packet.ReadXORByte(guid, 0);
            packet.ReadXORByte(guid, 3);
            packet.ReadXORByte(guid, 6);
            packet.ReadXORByte(guid, 5);
            packet.ReadXORByte(guid, 7);
            WowGuid moverGuid = packet.WriteGuid("Guid", guid);
            speedUpdate.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(packet.Time);
            Storage.StoreUnitSpeedUpdate(moverGuid, speedUpdate);
        }

        [Parser(Opcode.MSG_MOVE_START_FORWARD)]
        public static void HandleMoveStartForward434(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();
            MovementInfo info = new MovementInfo();

            info.Position.Y = pos.Y = packet.ReadSingle();
            info.Position.Z = pos.Z = packet.ReadSingle();
            info.Position.X = pos.X = packet.ReadSingle();
            guid[5] = packet.ReadBit();
            guid[2] = packet.ReadBit();
            guid[0] = packet.ReadBit();
            packet.ReadBit();
            var hasMovementFlags = !packet.ReadBit();
            guid[7] = packet.ReadBit();
            guid[3] = packet.ReadBit();
            guid[1] = packet.ReadBit();
            var hasO = !packet.ReadBit();
            guid[6] = packet.ReadBit();
            packet.ReadBit("Has Spline");
            var hasSplineElev = !packet.ReadBit("Has Spline Elevation");
            guid[4] = packet.ReadBit();
            var hasTrans = packet.ReadBit("Has transport");
            var hasTime = !packet.ReadBit("Has timestamp");
            var hasPitch = !packet.ReadBit("Has pitch");
            var hasMovementFlags2 = !packet.ReadBit();
            var hasFallData = packet.ReadBit("Has fall data");

            if (hasMovementFlags)
                info.Flags = (uint)packet.ReadBitsE<MovementFlag>("Movement Flags", 30);

            if (hasTrans)
            {
                transportGuid = packet.StartBitStream(3, 4, 6, 2, 5, 0, 7, 1);
                hasTransTime3 = packet.ReadBit();
                hasTransTime2 = packet.ReadBit();
            }

            if (hasFallData)
                hasFallDirection = packet.ReadBit();

            if (hasMovementFlags2)
                info.FlagsExtra = (uint)packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 12);

            packet.ParseBitStream(guid, 2, 4, 6, 1, 7, 3, 5, 0);

            if (hasFallData)
            {
                info.JumpVerticalSpeed = packet.ReadSingle("Vertical Speed");
                if (hasFallDirection)
                {
                    info.JumpHorizontalSpeed = packet.ReadSingle("Horizontal Speed");
                    info.JumpCosAngle = packet.ReadSingle("Fall Cos");
                    info.JumpSinAngle = packet.ReadSingle("Fall Sin");
                }

                info.FallTime = packet.ReadUInt32("Fall time");
            }

            if (hasTrans)
            {
                var tpos = new Vector4();
                packet.ReadXORByte(transportGuid, 3);
                tpos.Y = packet.ReadSingle();
                tpos.Z = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 1);
                packet.ReadXORByte(transportGuid, 4);
                packet.ReadXORByte(transportGuid, 7);
                tpos.O = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 2);
                tpos.X = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 5);
                if (hasTransTime3)
                    packet.ReadUInt32("Transport Time 3");

                info.TransportTime = packet.ReadUInt32("Transport Time");
                packet.ReadXORByte(transportGuid, 6);
                packet.ReadXORByte(transportGuid, 0);
                info.TransportSeat = packet.ReadSByte("Transport Seat");
                if (hasTransTime2)
                    packet.ReadUInt32("Transport Time 2");

                info.TransportGuid = packet.WriteGuid("Transport Guid", transportGuid);
                info.TransportOffset = packet.AddValue("Transport Position", tpos);
            }

            if (hasSplineElev)
                info.SplineElevation = packet.ReadSingle("Spline elevation");
            if (hasPitch)
                info.SwimPitch = packet.ReadSingle("Pitch");
            if (hasO)
                info.Orientation = pos.O = packet.ReadSingle();
            if (hasTime)
                info.MoveTime = packet.ReadUInt32("Timestamp");

            WowGuid moverGuid = packet.WriteGuid("Guid", guid);
            packet.AddValue("Position", pos);
            Storage.StorePlayerMovement(moverGuid, info, packet);
        }

        [Parser(Opcode.SMSG_MOVE_UPDATE)]
        public static void HandlePlayerMove434(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasFallDirection = false;
            var pos = new Vector4();
            MovementInfo info = new MovementInfo();

            var hasFallData = packet.ReadBit();
            guid[3] = packet.ReadBit();
            guid[6] = packet.ReadBit();
            var hasMovementFlagsExtra = !packet.ReadBit();
            packet.ReadBit("Has spline data");
            var hasTime = !packet.ReadBit();
            guid[0] = packet.ReadBit();
            guid[1] = packet.ReadBit();
            if (hasMovementFlagsExtra)
                info.FlagsExtra = (uint)packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 12);

            guid[7] = packet.ReadBit();
            var hasMovementFlags = !packet.ReadBit();
            var hasO = !packet.ReadBit();
            guid[2] = packet.ReadBit();
            var hasSplineElevation = !packet.ReadBit();
            packet.ReadBit();
            guid[4] = packet.ReadBit();
            if (hasFallData)
                hasFallDirection = packet.ReadBit();

            guid[5] = packet.ReadBit();
            var hasTransport = packet.ReadBit();
            if (hasMovementFlags)
                info.Flags = (uint)packet.ReadBitsE<MovementFlag>("Movement Flags", 30);

            var hasTransportTime3 = false;
            var hasTransportTime2 = false;
            if (hasTransport)
            {
                transportGuid[3] = packet.ReadBit();
                hasTransportTime3 = packet.ReadBit();
                transportGuid[6] = packet.ReadBit();
                transportGuid[1] = packet.ReadBit();
                transportGuid[7] = packet.ReadBit();
                transportGuid[0] = packet.ReadBit();
                transportGuid[4] = packet.ReadBit();
                hasTransportTime2 = packet.ReadBit();
                transportGuid[5] = packet.ReadBit();
                transportGuid[2] = packet.ReadBit();
            }

            var hasPitch = !packet.ReadBit();
            packet.ReadXORByte(guid, 5);
            if (hasFallData)
            {
                if (hasFallDirection)
                {
                    info.JumpHorizontalSpeed = packet.ReadSingle("Horizontal Speed");
                    info.JumpCosAngle = packet.ReadSingle("Fall Cos");
                    info.JumpSinAngle = packet.ReadSingle("Fall Sin");
                }

                info.JumpVerticalSpeed = packet.ReadSingle("Vertical Speed");
                info.FallTime = packet.ReadUInt32("Fall time");
            }

            if (hasSplineElevation)
                info.SplineElevation = packet.ReadSingle("Spline elevation");

            packet.ReadXORByte(guid, 7);
            info.Position.Y = pos.Y = packet.ReadSingle();
            packet.ReadXORByte(guid, 3);
            if (hasTransport)
            {
                var tpos = new Vector4();
                if (hasTransportTime3)
                    packet.ReadUInt32("Transport Time 3");

                packet.ReadXORByte(transportGuid, 6);
                info.TransportSeat = packet.ReadSByte("Transport Seat");
                packet.ReadXORByte(transportGuid, 5);
                tpos.X = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 1);
                tpos.O = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 2);
                if (hasTransportTime2)
                    packet.ReadUInt32("Transport Time 2");

                packet.ReadXORByte(transportGuid, 0);
                tpos.Z = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 7);
                packet.ReadXORByte(transportGuid, 4);
                packet.ReadXORByte(transportGuid, 3);
                tpos.Y = packet.ReadSingle();
                info.TransportTime = packet.ReadUInt32("Transport Time");

                info.TransportGuid = packet.WriteGuid("Transport Guid", transportGuid);
                info.TransportOffset = packet.AddValue("Transport Position", tpos);
            }

            packet.ReadXORByte(guid, 4);
            info.Position.X = pos.X = packet.ReadSingle();
            packet.ReadXORByte(guid, 6);
            info.Position.Z = pos.Z = packet.ReadSingle();
            if (hasTime)
                info.MoveTime = packet.ReadUInt32("Timestamp");

            packet.ReadXORByte(guid, 2);
            if (hasPitch)
                info.SwimPitch = packet.ReadSingle("Pitch");

            packet.ReadXORByte(guid, 0);
            if (hasO)
                info.Orientation = pos.O = packet.ReadSingle();

            packet.ReadXORByte(guid, 1);
            WowGuid moverGuid = packet.WriteGuid("Guid", guid);
            packet.AddValue("Position", pos);
            Storage.StorePlayerMovement(moverGuid, info, packet);
        }

        [Parser(Opcode.SMSG_MOVE_SET_COLLISION_HEIGHT)]
        public static void HandleSetCollisionHeight434(Packet packet)
        {
            packet.ReadBitsE<SetCollisionHeightReason>("Reason", 2);
            var guid = packet.StartBitStream(6, 1, 4, 7, 5, 2, 0, 3);

            packet.ReadXORByte(guid, 6);
            packet.ReadXORByte(guid, 0);
            packet.ReadXORByte(guid, 4);
            packet.ReadXORByte(guid, 3);
            packet.ReadXORByte(guid, 5);
            packet.ReadUInt32("SequenceIndex");
            packet.ReadXORByte(guid, 1);
            packet.ReadXORByte(guid, 2);
            packet.ReadXORByte(guid, 7);
            packet.ReadSingle("Height");
            packet.WriteGuid("MoverGUID", guid);
        }

        [Parser(Opcode.MSG_MOVE_SET_RUN_MODE)]
        public static void HandleMoveSetRunMode434(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();
            MovementInfo info = new MovementInfo();

            info.Position.Y = pos.Y = packet.ReadSingle();
            info.Position.X = pos.X = packet.ReadSingle();
            info.Position.Z = pos.Z = packet.ReadSingle();
            var hasTime = !packet.ReadBit("Has timestamp");
            var hasMovementFlags2 = !packet.ReadBit();
            guid[1] = packet.ReadBit();
            packet.ReadBit("Has Spline");
            var hasMovementFlags = !packet.ReadBit();
            guid[7] = packet.ReadBit();
            var hasTrans = packet.ReadBit("Has transport");
            packet.ReadBit();
            guid[0] = packet.ReadBit();
            guid[3] = packet.ReadBit();
            var hasSplineElev = !packet.ReadBit("Has Spline Elevation");
            guid[5] = packet.ReadBit();
            var hasPitch = !packet.ReadBit("Has pitch");
            guid[6] = packet.ReadBit();
            guid[4] = packet.ReadBit();
            var hasFallData = packet.ReadBit("Has fall data");
            var hasO = !packet.ReadBit();
            guid[2] = packet.ReadBit();

            if (hasTrans)
            {
                transportGuid[6] = packet.ReadBit();
                transportGuid[4] = packet.ReadBit();
                hasTransTime3 = packet.ReadBit();
                transportGuid[5] = packet.ReadBit();
                transportGuid[3] = packet.ReadBit();
                transportGuid[1] = packet.ReadBit();
                hasTransTime2 = packet.ReadBit();
                transportGuid[7] = packet.ReadBit();
                transportGuid[0] = packet.ReadBit();
                transportGuid[2] = packet.ReadBit();
            }

            if (hasFallData)
                hasFallDirection = packet.ReadBit();

            if (hasMovementFlags2)
                info.FlagsExtra = (uint)packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 12);

            if (hasMovementFlags)
                info.Flags = (uint)packet.ReadBitsE<MovementFlag>("Movement Flags", 30);

            packet.ParseBitStream(guid, 3, 6, 0, 7, 4, 1, 5, 2);

            if (hasPitch)
                info.SwimPitch = packet.ReadSingle("Pitch");

            if (hasTrans)
            {
                var tpos = new Vector4();

                if (hasTransTime2)
                    packet.ReadUInt32("Transport Time 2");

                packet.ReadXORByte(transportGuid, 3);
                tpos.X = packet.ReadSingle();
                info.TransportSeat = packet.ReadSByte("Transport Seat");
                packet.ReadXORByte(transportGuid, 5);
                packet.ReadXORByte(transportGuid, 1);
                tpos.Z = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 2);
                packet.ReadXORByte(transportGuid, 7);
                tpos.O = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 4);
                info.TransportTime = packet.ReadUInt32("Transport Time");

                if (hasTransTime3)
                    packet.ReadUInt32("Transport Time 3");

                packet.ReadXORByte(transportGuid, 0);
                tpos.Y = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 6);

                info.TransportGuid = packet.WriteGuid("Transport Guid", transportGuid);
                info.TransportOffset = packet.AddValue("Transport Position", tpos);
            }

            if (hasFallData)
            {
                if (hasFallDirection)
                {
                    info.JumpSinAngle = packet.ReadSingle("Fall Sin");
                    info.JumpHorizontalSpeed = packet.ReadSingle("Horizontal Speed");
                    info.JumpCosAngle = packet.ReadSingle("Fall Cos");
                }

                info.FallTime = packet.ReadUInt32("Fall time");
                info.JumpVerticalSpeed = packet.ReadSingle("Vertical Speed");
            }

            if (hasSplineElev)
                info.SplineElevation = packet.ReadSingle("Spline elevation");
            if (hasTime)
                info.MoveTime = packet.ReadUInt32("Timestamp");
            if (hasO)
                info.Orientation = pos.O = packet.ReadSingle();

            WowGuid moverGuid = packet.WriteGuid("Guid", guid);
            packet.AddValue("Position", pos);
            Storage.StorePlayerMovement(moverGuid, info, packet);
        }

        [Parser(Opcode.MSG_MOVE_SET_WALK_MODE)]
        public static void HandleMoveSetWalkMode434(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();
            MovementInfo info = new MovementInfo();

            info.Position.Y = pos.Y = packet.ReadSingle();
            info.Position.X = pos.X = packet.ReadSingle();
            info.Position.Z = pos.Z = packet.ReadSingle();
            guid[6] = packet.ReadBit();
            packet.ReadBit("Has Spline");
            var hasTime = !packet.ReadBit("Has timestamp");
            guid[0] = packet.ReadBit();
            guid[1] = packet.ReadBit();
            var hasMovementFlags = !packet.ReadBit();
            var hasPitch = !packet.ReadBit("Has pitch");
            guid[7] = packet.ReadBit();
            var hasSplineElev = !packet.ReadBit("Has Spline Elevation");
            guid[4] = packet.ReadBit();
            var hasO = !packet.ReadBit();
            var hasTrans = packet.ReadBit("Has transport");
            guid[2] = packet.ReadBit();
            guid[5] = packet.ReadBit();
            guid[3] = packet.ReadBit();
            packet.ReadBit();
            var hasMovementFlags2 = !packet.ReadBit();
            var hasFallData = packet.ReadBit("Has fall data");

            if (hasTrans)
            {
                transportGuid[2] = packet.ReadBit();
                transportGuid[0] = packet.ReadBit();
                transportGuid[6] = packet.ReadBit();
                transportGuid[1] = packet.ReadBit();
                transportGuid[3] = packet.ReadBit();
                hasTransTime2 = packet.ReadBit();
                transportGuid[5] = packet.ReadBit();
                hasTransTime3 = packet.ReadBit();
                transportGuid[4] = packet.ReadBit();
                transportGuid[7] = packet.ReadBit();
            }

            if (hasFallData)
                hasFallDirection = packet.ReadBit();

            if (hasMovementFlags)
                info.Flags = (uint)packet.ReadBitsE<MovementFlag>("Movement Flags", 30);

            if (hasMovementFlags2)
                info.FlagsExtra = (uint)packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 12);

            packet.ParseBitStream(guid, 5, 6, 4, 7, 3, 0, 2, 1);

            if (hasTrans)
            {
                var tpos = new Vector4();

                packet.ReadXORByte(transportGuid, 2);
                packet.ReadXORByte(transportGuid, 5);
                info.TransportSeat = packet.ReadSByte("Transport Seat");
                tpos.Z = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 3);
                packet.ReadXORByte(transportGuid, 6);
                packet.ReadXORByte(transportGuid, 0);
                info.TransportTime = packet.ReadUInt32("Transport Time");
                packet.ReadXORByte(transportGuid, 4);

                if (hasTransTime2)
                    packet.ReadUInt32("Transport Time 2");

                tpos.O = packet.ReadSingle();
                tpos.X = packet.ReadSingle();

                if (hasTransTime3)
                    packet.ReadUInt32("Transport Time 3");

                packet.ReadXORByte(transportGuid, 7);
                tpos.Y = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 1);

                info.TransportGuid = packet.WriteGuid("Transport Guid", transportGuid);
                info.TransportOffset = packet.AddValue("Transport Position", tpos);
            }

            if (hasFallData)
            {
                if (hasFallDirection)
                {
                    info.JumpCosAngle = packet.ReadSingle("Fall Cos");
                    info.JumpHorizontalSpeed = packet.ReadSingle("Horizontal Speed");
                    info.JumpSinAngle = packet.ReadSingle("Fall Sin");
                }

                info.JumpVerticalSpeed = packet.ReadSingle("Vertical Speed");
                info.FallTime = packet.ReadUInt32("Fall time");
            }

            if (hasSplineElev)
                info.SplineElevation = packet.ReadSingle("Spline elevation");
            if (hasPitch)
                info.SwimPitch = packet.ReadSingle("Pitch");
            if (hasTime)
                info.MoveTime = packet.ReadUInt32("Timestamp");
            if (hasO)
                info.Orientation = pos.O = packet.ReadSingle();

            WowGuid moverGuid = packet.WriteGuid("Guid", guid);
            packet.AddValue("Position", pos);
            Storage.StorePlayerMovement(moverGuid, info, packet);
        }

        [Parser(Opcode.CMSG_MOVE_SET_CAN_FLY)]
        public static void HandleMoveSetCanFly434(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();
            MovementInfo info = new MovementInfo();

            info.Position.Z = pos.Z = packet.ReadSingle();
            info.Position.X = pos.X = packet.ReadSingle();
            info.Position.Y = pos.Y = packet.ReadSingle();
            var hasTrans = packet.ReadBit("Has transport");
            guid[1] = packet.ReadBit();
            guid[6] = packet.ReadBit();
            guid[4] = packet.ReadBit();
            guid[2] = packet.ReadBit();
            var hasFallData = packet.ReadBit("Has fall data");
            guid[0] = packet.ReadBit();
            var hasMovementFlags = !packet.ReadBit();
            guid[3] = packet.ReadBit();
            var hasMovementFlags2 = !packet.ReadBit();
            guid[5] = packet.ReadBit();
            var hasTime = !packet.ReadBit("Has timestamp");
            var hasSplineElev = !packet.ReadBit("Has Spline Elevation");
            packet.ReadBit("Has Spline");
            var hasO = !packet.ReadBit();
            guid[7] = packet.ReadBit();
            packet.ReadBit();
            var hasPitch = !packet.ReadBit("Has pitch");

            if (hasTrans)
            {
                transportGuid[3] = packet.ReadBit();
                hasTransTime2 = packet.ReadBit();
                transportGuid[6] = packet.ReadBit();
                transportGuid[0] = packet.ReadBit();
                transportGuid[7] = packet.ReadBit();
                transportGuid[4] = packet.ReadBit();
                hasTransTime3 = packet.ReadBit();
                transportGuid[1] = packet.ReadBit();
                transportGuid[2] = packet.ReadBit();
                transportGuid[5] = packet.ReadBit();
            }

            if (hasMovementFlags2)
                info.FlagsExtra = (uint)packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 12);

            if (hasFallData)
                hasFallDirection = packet.ReadBit();

            if (hasMovementFlags)
                info.Flags = (uint)packet.ReadBitsE<MovementFlag>("Movement Flags", 30);

            packet.ParseBitStream(guid, 2, 0, 4, 7, 5, 1, 3, 6);

            if (hasTrans)
            {
                var tpos = new Vector4();

                tpos.Z = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 3);
                packet.ReadXORByte(transportGuid, 5);
                packet.ReadXORByte(transportGuid, 4);
                info.TransportSeat = packet.ReadSByte("Transport Seat");
                tpos.X = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 2);

                if (hasTransTime2)
                    packet.ReadUInt32("Transport Time 2");

                tpos.Y = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 1);
                info.TransportTime = packet.ReadUInt32("Transport Time");
                packet.ReadXORByte(transportGuid, 7);
                packet.ReadXORByte(transportGuid, 0);
                tpos.O = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 6);

                if (hasTransTime3)
                    packet.ReadUInt32("Transport Time 3");

                info.TransportGuid = packet.WriteGuid("Transport Guid", transportGuid);
                info.TransportOffset = packet.AddValue("Transport Position", tpos);
            }

            if (hasSplineElev)
                info.SplineElevation = packet.ReadSingle("Spline elevation");

            if (hasFallData)
            {
                if (hasFallDirection)
                {
                    info.JumpCosAngle = packet.ReadSingle("Fall Cos");
                    info.JumpSinAngle = packet.ReadSingle("Fall Sin");
                    info.JumpHorizontalSpeed = packet.ReadSingle("Horizontal Speed");
                }

                info.JumpVerticalSpeed = packet.ReadSingle("Vertical Speed");
                info.FallTime = packet.ReadUInt32("Fall time");
            }

            if (hasPitch)
                info.SwimPitch = packet.ReadSingle("Pitch");
            if (hasTime)
                info.MoveTime = packet.ReadUInt32("Timestamp");
            if (hasO)
                info.Orientation = pos.O = packet.ReadSingle();

            WowGuid moverGuid = packet.WriteGuid("Guid", guid);
            packet.AddValue("Position", pos);
            Storage.StorePlayerMovement(moverGuid, info, packet);
        }

        [Parser(Opcode.CMSG_DISMISS_CONTROLLED_VEHICLE)]
        public static void HandleDismissControlledVehicle434(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();
            MovementInfo info = new MovementInfo();

            info.Position.Y = pos.Y = packet.ReadSingle();
            info.Position.Z = pos.Z = packet.ReadSingle();
            info.Position.X = pos.X = packet.ReadSingle();
            var hasSplineElev = !packet.ReadBit("Has Spline Elevation");
            var hasFallData = packet.ReadBit("Has fall data");
            var hasTrans = packet.ReadBit("Has transport");
            guid[4] = packet.ReadBit();
            guid[7] = packet.ReadBit();
            var hasMovementFlags2 = !packet.ReadBit();
            guid[2] = packet.ReadBit();
            var hasO = !packet.ReadBit();
            guid[0] = packet.ReadBit();
            guid[6] = packet.ReadBit();
            var hasPitch = !packet.ReadBit("Has pitch");
            var hasTime = !packet.ReadBit("Has timestamp");
            guid[1] = packet.ReadBit();
            packet.ReadBit();
            packet.ReadBit("Has Spline");
            guid[3] = packet.ReadBit();
            var hasMovementFlags = !packet.ReadBit();
            guid[5] = packet.ReadBit();

            if (hasTrans)
            {
                transportGuid[5] = packet.ReadBit();
                hasTransTime3 = packet.ReadBit();
                transportGuid[2] = packet.ReadBit();
                transportGuid[0] = packet.ReadBit();
                transportGuid[7] = packet.ReadBit();
                transportGuid[3] = packet.ReadBit();
                transportGuid[4] = packet.ReadBit();
                transportGuid[1] = packet.ReadBit();
                transportGuid[6] = packet.ReadBit();
                hasTransTime2 = packet.ReadBit();
            }

            if (hasMovementFlags2)
                info.FlagsExtra = (uint)packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 12);

            if (hasFallData)
                hasFallDirection = packet.ReadBit();

            if (hasMovementFlags)
                info.Flags = (uint)packet.ReadBitsE<MovementFlag>("Movement Flags", 30);

            packet.ParseBitStream(guid, 6, 3, 1, 5, 2, 4, 7, 0);

            if (hasTime)
                info.MoveTime = packet.ReadUInt32("Timestamp");

            if (hasTrans)
            {
                var tpos = new Vector4();

                if (hasTransTime3)
                    packet.ReadUInt32("Transport Time 3");

                packet.ReadXORByte(transportGuid, 4);
                packet.ReadXORByte(transportGuid, 7);

                if (hasTransTime2)
                    packet.ReadUInt32("Transport Time 2");

                packet.ReadXORByte(transportGuid, 6);
                info.TransportTime = packet.ReadUInt32("Transport Time");
                tpos.Z = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 0);
                tpos.X = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 3);
                info.TransportSeat = packet.ReadSByte("Transport Seat");
                tpos.Y = packet.ReadSingle();
                tpos.O = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 1);
                packet.ReadXORByte(transportGuid, 5);
                packet.ReadXORByte(transportGuid, 2);

                info.TransportGuid = packet.WriteGuid("Transport Guid", transportGuid);
                info.TransportOffset = packet.AddValue("Transport Position", tpos);
            }

            if (hasFallData)
            {
                info.FallTime = packet.ReadUInt32("Fall time");

                if (hasFallDirection)
                {
                    info.JumpHorizontalSpeed = packet.ReadSingle("Horizontal Speed");
                    info.JumpSinAngle = packet.ReadSingle("Fall Sin");
                    info.JumpCosAngle = packet.ReadSingle("Fall Cos");
                }

                info.JumpVerticalSpeed = packet.ReadSingle("Vertical Speed");
            }

            if (hasO)
                info.Orientation = pos.O = packet.ReadSingle();
            if (hasSplineElev)
                info.SplineElevation = packet.ReadSingle("Spline elevation");
            if (hasPitch)
                info.SwimPitch = packet.ReadSingle("Pitch");

            WowGuid moverGuid = packet.WriteGuid("Guid", guid);
            packet.AddValue("Position", pos);
            Storage.StorePlayerMovement(moverGuid, info, packet);
        }

        [Parser(Opcode.CMSG_MOVE_SPLINE_DONE)]
        public static void HandleMoveSplineDone434(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();
            MovementInfo info = new MovementInfo();

            packet.ReadInt32("Move Ticks");
            info.Position.Y = pos.Y = packet.ReadSingle();
            info.Position.X = pos.X = packet.ReadSingle();
            info.Position.Z = pos.Z = packet.ReadSingle();
            guid[6] = packet.ReadBit();
            var hasO = !packet.ReadBit();
            var hasFallData = packet.ReadBit("Has fall data");
            var hasTime = !packet.ReadBit("Has timestamp");
            guid[2] = packet.ReadBit();
            var hasSplineElev = !packet.ReadBit("Has Spline Elevation");
            guid[4] = packet.ReadBit();
            var hasTrans = packet.ReadBit("Has transport");
            guid[3] = packet.ReadBit();
            var hasMovementFlags = !packet.ReadBit();
            guid[0] = packet.ReadBit();
            packet.ReadBit();
            guid[1] = packet.ReadBit();
            guid[5] = packet.ReadBit();
            var hasPitch = !packet.ReadBit("Has pitch");
            packet.ReadBit("Has Spline");
            var hasMovementFlags2 = !packet.ReadBit();
            guid[7] = packet.ReadBit();

            if (hasTrans)
            {
                transportGuid[1] = packet.ReadBit();
                transportGuid[7] = packet.ReadBit();
                transportGuid[5] = packet.ReadBit();
                transportGuid[3] = packet.ReadBit();
                transportGuid[4] = packet.ReadBit();
                transportGuid[6] = packet.ReadBit();
                hasTransTime2 = packet.ReadBit();
                transportGuid[2] = packet.ReadBit();
                hasTransTime3 = packet.ReadBit();
                transportGuid[0] = packet.ReadBit();
            }

            if (hasFallData)
                hasFallDirection = packet.ReadBit();

            if (hasMovementFlags2)
                info.FlagsExtra = (uint)packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 12);

            if (hasMovementFlags)
                info.Flags = (uint)packet.ReadBitsE<MovementFlag>("Movement Flags", 30);

            packet.ParseBitStream(guid, 7, 4, 5, 6, 0, 1, 2, 3);

            if (hasFallData)
            {
                info.JumpVerticalSpeed = packet.ReadSingle("Vertical Speed");

                if (hasFallDirection)
                {
                    info.JumpSinAngle = packet.ReadSingle("Fall Sin");
                    info.JumpCosAngle = packet.ReadSingle("Fall Cos");
                    info.JumpHorizontalSpeed = packet.ReadSingle("Horizontal Speed");
                }

                info.FallTime = packet.ReadUInt32("Fall time");
            }

            if (hasPitch)
                info.SwimPitch = packet.ReadSingle("Pitch");
            if (hasO)
                info.Orientation = pos.O = packet.ReadSingle();

            if (hasTrans)
            {
                var tpos = new Vector4();

                packet.ReadXORByte(transportGuid, 1);

                if (hasTransTime3)
                    packet.ReadUInt32("Transport Time 3");

                packet.ReadXORByte(transportGuid, 7);
                info.TransportTime = packet.ReadUInt32("Transport Time");

                tpos.Y = packet.ReadSingle();
                tpos.X = packet.ReadSingle();
                tpos.Z = packet.ReadSingle();
                info.TransportSeat = packet.ReadSByte("Transport Seat");
                tpos.O = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 0);

                if (hasTransTime2)
                    packet.ReadUInt32("Transport Time 2");

                packet.ReadXORByte(transportGuid, 2);
                packet.ReadXORByte(transportGuid, 3);
                packet.ReadXORByte(transportGuid, 5);
                packet.ReadXORByte(transportGuid, 6);
                packet.ReadXORByte(transportGuid, 4);

                info.TransportGuid = packet.WriteGuid("Transport Guid", transportGuid);
                info.TransportOffset = packet.AddValue("Transport Position", tpos);
            }

            if (hasTime)
                info.MoveTime = packet.ReadUInt32("Timestamp");
            if (hasSplineElev)
                info.SplineElevation = packet.ReadSingle("Spline elevation");

            WowGuid moverGuid = packet.WriteGuid("Guid", guid);
            packet.AddValue("Position", pos);
            Storage.StorePlayerMovement(moverGuid, info, packet);
        }

        [Parser(Opcode.CMSG_MOVE_SET_CAN_TRANSITION_BETWEEN_SWIM_AND_FLY_ACK)]
        public static void HandleMoveSetCanTransitionBetweenSwimAndFlyAck434(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();
            MovementInfo info = new MovementInfo();

            info.Position.Z = pos.Z = packet.ReadSingle();
            info.Position.Y = pos.Y = packet.ReadSingle();
            packet.ReadInt32("Movement Counter");
            info.Position.X = pos.X = packet.ReadSingle();
            guid[3] = packet.ReadBit();
            var hasO = !packet.ReadBit();
            var hasTrans = packet.ReadBit("Has transport");
            var hasMovementFlags = !packet.ReadBit();
            var hasMovementFlags2 = !packet.ReadBit();
            var hasSplineElev = !packet.ReadBit("Has Spline Elevation");
            var hasTime = !packet.ReadBit("Has timestamp");
            packet.ReadBit("Has Spline");
            packet.ReadBit();
            var hasFallData = packet.ReadBit("Has fall data");
            guid[5] = packet.ReadBit();
            guid[0] = packet.ReadBit();
            guid[1] = packet.ReadBit();
            guid[4] = packet.ReadBit();
            guid[6] = packet.ReadBit();
            guid[2] = packet.ReadBit();
            var hasPitch = !packet.ReadBit("Has pitch");
            guid[7] = packet.ReadBit();

            if (hasTrans)
            {
                transportGuid[4] = packet.ReadBit();
                transportGuid[2] = packet.ReadBit();
                hasTransTime2 = packet.ReadBit();
                transportGuid[0] = packet.ReadBit();
                transportGuid[3] = packet.ReadBit();
                transportGuid[6] = packet.ReadBit();
                transportGuid[5] = packet.ReadBit();
                hasTransTime3 = packet.ReadBit();
                transportGuid[7] = packet.ReadBit();
                transportGuid[1] = packet.ReadBit();
            }

            if (hasMovementFlags2)
                info.FlagsExtra = (uint)packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 12);

            if (hasFallData)
                hasFallDirection = packet.ReadBit();

            if (hasMovementFlags)
                info.Flags = (uint)packet.ReadBitsE<MovementFlag>("Movement Flags", 30);

            packet.ParseBitStream(guid, 3, 2, 0, 4, 1, 5, 7, 6);

            if (hasFallData)
            {
                info.FallTime = packet.ReadUInt32("Fall time");

                if (hasFallDirection)
                {
                    info.JumpHorizontalSpeed = packet.ReadSingle("Horizontal Speed");
                    info.JumpSinAngle = packet.ReadSingle("Fall Sin");
                    info.JumpCosAngle = packet.ReadSingle("Fall Cos");
                }

                info.JumpVerticalSpeed = packet.ReadSingle("Vertical Speed");
            }

            if (hasTrans)
            {
                var tpos = new Vector4();

                tpos.Y = packet.ReadSingle();
                tpos.Z = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 5);
                packet.ReadXORByte(transportGuid, 1);
                packet.ReadXORByte(transportGuid, 4);
                info.TransportTime = packet.ReadUInt32("Transport Time");
                info.TransportSeat = packet.ReadSByte("Transport Seat");
                packet.ReadXORByte(transportGuid, 3);
                packet.ReadXORByte(transportGuid, 0);
                packet.ReadXORByte(transportGuid, 2);
                packet.ReadXORByte(transportGuid, 7);
                tpos.X = packet.ReadSingle();

                if (hasTransTime2)
                    packet.ReadUInt32("Transport Time 2");

                packet.ReadXORByte(transportGuid, 6);
                tpos.O = packet.ReadSingle();

                if (hasTransTime3)
                    packet.ReadUInt32("Transport Time 3");

                info.TransportGuid = packet.WriteGuid("Transport Guid", transportGuid);
                info.TransportOffset = packet.AddValue("Transport Position", tpos);
            }

            if (hasPitch)
                info.SwimPitch = packet.ReadSingle("Pitch");
            if (hasO)
                info.Orientation = pos.O = packet.ReadSingle();
            if (hasTime)
                info.MoveTime = packet.ReadUInt32("Timestamp");
            if (hasSplineElev)
                info.SplineElevation = packet.ReadSingle("Spline elevation");

            WowGuid moverGuid = packet.WriteGuid("Guid", guid);
            packet.AddValue("Position", pos);
            Storage.StorePlayerMovement(moverGuid, info, packet);
        }

        [Parser(Opcode.SMSG_MOVE_UPDATE_SWIM_SPEED)]
        public static void HandleMoveUpdateSwimSpeed434(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasFallDirection = false;
            var pos = new Vector4();
            MovementInfo info = new MovementInfo();

            var hasMovementFlags = !packet.ReadBit();
            guid[2] = packet.ReadBit();
            packet.ReadBit("Has spline data");
            var hasTime = !packet.ReadBit();

            if (hasMovementFlags)
                info.Flags = (uint)packet.ReadBitsE<MovementFlag>("Movement Flags", 30);

            var hasFallData = packet.ReadBit();
            guid[3] = packet.ReadBit();
            var hasTransport = packet.ReadBit();
            packet.ReadBit();
            guid[7] = packet.ReadBit();

            if (hasFallData)
                hasFallDirection = packet.ReadBit();

            var hasTransportTime2 = false;
            var hasTransportTime3 = false;
            if (hasTransport)
            {
                transportGuid[4] = packet.ReadBit();
                transportGuid[5] = packet.ReadBit();
                transportGuid[3] = packet.ReadBit();
                hasTransportTime2 = packet.ReadBit();
                transportGuid[2] = packet.ReadBit();
                hasTransportTime3 = packet.ReadBit();
                transportGuid[0] = packet.ReadBit();
                transportGuid[1] = packet.ReadBit();
                transportGuid[6] = packet.ReadBit();
                transportGuid[7] = packet.ReadBit();
            }

            var hasMovementFlagsExtra = !packet.ReadBit();
            guid[6] = packet.ReadBit();
            var hasSplineElevation = !packet.ReadBit();
            guid[0] = packet.ReadBit();
            guid[4] = packet.ReadBit();
            guid[5] = packet.ReadBit();
            var hasO = !packet.ReadBit();
            var hasPitch = !packet.ReadBit();

            if (hasMovementFlagsExtra)
                info.FlagsExtra = (uint)packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 12);

            guid[1] = packet.ReadBit();

            if (hasTransport)
            {
                var tpos = new Vector4();

                packet.ReadXORByte(transportGuid, 2);
                tpos.X = packet.ReadSingle();

                if (hasTransportTime3)
                    packet.ReadUInt32("Transport Time 3");

                if (hasTransportTime2)
                    packet.ReadUInt32("Transport Time 2");

                packet.ReadXORByte(transportGuid, 6);
                tpos.O = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 0);
                packet.ReadXORByte(transportGuid, 4);
                tpos.Y = packet.ReadSingle();
                info.TransportTime = packet.ReadUInt32("Transport Time");
                packet.ReadXORByte(transportGuid, 1);
                packet.ReadXORByte(transportGuid, 7);
                tpos.Z = packet.ReadSingle();
                info.TransportSeat = packet.ReadSByte("Transport Seat");
                packet.ReadXORByte(transportGuid, 3);
                packet.ReadXORByte(transportGuid, 5);

                info.TransportGuid = packet.WriteGuid("Transport Guid", transportGuid);
                info.TransportOffset = packet.AddValue("Transport Position", tpos);
            }

            info.Position.X = pos.X = packet.ReadSingle();

            if (hasFallData)
            {
                if (hasFallDirection)
                {
                    info.JumpCosAngle = packet.ReadSingle("Fall Cos");
                    info.JumpHorizontalSpeed = packet.ReadSingle("Horizontal Speed");
                    info.JumpSinAngle = packet.ReadSingle("Fall Sin");
                }

                info.FallTime = packet.ReadUInt32("Fall time");
                info.JumpVerticalSpeed = packet.ReadSingle("Vertical Speed");
            }

            packet.ReadXORByte(guid, 7);

            if (hasTime)
                info.MoveTime = packet.ReadUInt32("Timestamp");

            info.Position.Y = pos.Y = packet.ReadSingle();

            if (hasSplineElevation)
                info.SplineElevation = packet.ReadSingle("Spline elevation");

            info.Position.Z = pos.Z = packet.ReadSingle();
            CreatureSpeedUpdate speedUpdate = new CreatureSpeedUpdate();
            speedUpdate.SpeedType = SpeedType.Swim;
            speedUpdate.SpeedRate = packet.ReadSingle("Speed") / MovementInfo.DEFAULT_SWIM_SPEED;
            packet.ReadXORByte(guid, 4);

            if (hasO)
                info.Orientation = pos.O = packet.ReadSingle();

            packet.ReadXORByte(guid, 2);
            packet.ReadXORByte(guid, 3);
            packet.ReadXORByte(guid, 0);
            packet.ReadXORByte(guid, 6);
            packet.ReadXORByte(guid, 1);
            packet.ReadXORByte(guid, 5);

            if (hasPitch)
                info.SwimPitch = packet.ReadSingle("Pitch");

            WowGuid moverGuid = packet.WriteGuid("Guid", guid);
            packet.AddValue("Position", pos);
            speedUpdate.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(packet.Time);
            Storage.StoreUnitSpeedUpdate(moverGuid, speedUpdate);
            Storage.StorePlayerMovement(moverGuid, info, packet);
        }

        [Parser(Opcode.SMSG_MOVE_UPDATE_RUN_SPEED)]
        public static void HandleMoveUpdateRunSpeed434(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasFallDirection = false;
            var pos = new Vector4();
            MovementInfo info = new MovementInfo();

            info.Position.Z = pos.Z = packet.ReadSingle();
            info.Position.X = pos.X = packet.ReadSingle();
            info.Position.Y = pos.Y = packet.ReadSingle();
            CreatureSpeedUpdate speedUpdate = new CreatureSpeedUpdate();
            speedUpdate.SpeedType = SpeedType.Run;
            speedUpdate.SpeedRate = packet.ReadSingle("Speed") / MovementInfo.DEFAULT_RUN_SPEED;
            guid[6] = packet.ReadBit();
            var hasMovementFlagsExtra = !packet.ReadBit();
            var hasPitch = !packet.ReadBit();
            guid[2] = packet.ReadBit();
            guid[5] = packet.ReadBit();
            var hasSplineElevation = !packet.ReadBit();
            packet.ReadBit("Has spline data");
            var hasMovementFlags = !packet.ReadBit();
            var hasTime = !packet.ReadBit();
            guid[1] = packet.ReadBit();

            if (hasMovementFlagsExtra)
                info.FlagsExtra = (uint)packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 12);

            guid[3] = packet.ReadBit();

            if (hasMovementFlags)
                info.Flags = (uint)packet.ReadBitsE<MovementFlag>("Movement Flags", 30);

            guid[7] = packet.ReadBit();
            guid[0] = packet.ReadBit();
            var hasO = !packet.ReadBit();
            var hasTransport = packet.ReadBit();

            var hasTransportTime2 = false;
            var hasTransportTime3 = false;
            if (hasTransport)
            {
                transportGuid[5] = packet.ReadBit();
                hasTransportTime2 = packet.ReadBit();
                hasTransportTime3 = packet.ReadBit();
                transportGuid[7] = packet.ReadBit();
                transportGuid[4] = packet.ReadBit();
                transportGuid[2] = packet.ReadBit();
                transportGuid[3] = packet.ReadBit();
                transportGuid[6] = packet.ReadBit();
                transportGuid[1] = packet.ReadBit();
                transportGuid[0] = packet.ReadBit();
            }

            var hasFallData = packet.ReadBit();

            if (hasFallData)
                hasFallDirection = packet.ReadBit();

            guid[4] = packet.ReadBit();
            packet.ReadBit();

            if (hasTransport)
            {
                var tpos = new Vector4();

                packet.ReadXORByte(transportGuid, 4);
                packet.ReadXORByte(transportGuid, 5);
                tpos.X = packet.ReadSingle();
                tpos.O = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 1);
                packet.ReadXORByte(transportGuid, 0);
                packet.ReadXORByte(transportGuid, 6);
                info.TransportTime = packet.ReadUInt32("Transport Time");
                packet.ReadXORByte(transportGuid, 7);
                info.TransportSeat = packet.ReadSByte("Transport Seat");

                if (hasTransportTime2)
                    packet.ReadUInt32("Transport Time 2");

                tpos.Y = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 3);
                packet.ReadXORByte(transportGuid, 2);

                if (hasTransportTime3)
                    packet.ReadUInt32("Transport Time 3");

                tpos.Z = packet.ReadSingle();

                info.TransportGuid = packet.WriteGuid("Transport Guid", transportGuid);
                info.TransportOffset = packet.AddValue("Transport Position", tpos);
            }

            if (hasTime)
                info.MoveTime = packet.ReadUInt32("Timestamp");

            if (hasFallData)
            {
                if (hasFallDirection)
                {
                    info.JumpSinAngle = packet.ReadSingle("Fall Sin");
                    info.JumpHorizontalSpeed = packet.ReadSingle("Horizontal Speed");
                    info.JumpCosAngle = packet.ReadSingle("Fall Cos");
                }

                info.JumpVerticalSpeed = packet.ReadSingle("Vertical Speed");
                info.FallTime = packet.ReadUInt32("Fall time");
            }

            if (hasPitch)
                info.SwimPitch = packet.ReadSingle("Pitch");

            packet.ReadXORByte(guid, 6);

            if (hasSplineElevation)
                info.SplineElevation = packet.ReadSingle("Spline elevation");

            packet.ReadXORByte(guid, 5);
            packet.ReadXORByte(guid, 7);
            packet.ReadXORByte(guid, 4);

            if (hasO)
                info.Orientation = pos.O = packet.ReadSingle();

            packet.ReadXORByte(guid, 0);
            packet.ReadXORByte(guid, 3);
            packet.ReadXORByte(guid, 2);
            packet.ReadXORByte(guid, 1);


            WowGuid moverGuid = packet.WriteGuid("Guid", guid);
            packet.AddValue("Position", pos);
            speedUpdate.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(packet.Time);
            Storage.StoreUnitSpeedUpdate(moverGuid, speedUpdate);
            Storage.StorePlayerMovement(moverGuid, info, packet);
        }

        [Parser(Opcode.SMSG_MOVE_UPDATE_FLIGHT_SPEED)]
        public static void HandleMoveUpdateFlightSpeed434(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasFallDirection = false;
            var pos = new Vector4();
            MovementInfo info = new MovementInfo();

            info.Position.Y = pos.Y = packet.ReadSingle();
            CreatureSpeedUpdate speedUpdate = new CreatureSpeedUpdate();
            speedUpdate.SpeedType = SpeedType.Fly;
            speedUpdate.SpeedRate = packet.ReadSingle("Speed") / MovementInfo.DEFAULT_FLY_SPEED;
            info.Position.X = pos.X = packet.ReadSingle();
            info.Position.Z = pos.Z = packet.ReadSingle();
            var hasMovementFlags = !packet.ReadBit();
            guid[4] = packet.ReadBit();
            packet.ReadBit();
            guid[5] = packet.ReadBit();
            guid[1] = packet.ReadBit();

            if (hasMovementFlags)
                info.Flags = (uint)packet.ReadBitsE<MovementFlag>("Movement Flags", 30);

            var hasFallData = packet.ReadBit();
            guid[2] = packet.ReadBit();
            var hasO = !packet.ReadBit();
            var hasPitch = !packet.ReadBit();
            var hasSplineElevation = !packet.ReadBit();
            guid[7] = packet.ReadBit();
            var hasTime = !packet.ReadBit();
            var hasTransport = packet.ReadBit();

            var hasTransportTime2 = false;
            var hasTransportTime3 = false;
            if (hasTransport)
            {
                transportGuid[3] = packet.ReadBit();
                transportGuid[5] = packet.ReadBit();
                transportGuid[1] = packet.ReadBit();
                transportGuid[0] = packet.ReadBit();
                transportGuid[6] = packet.ReadBit();
                transportGuid[4] = packet.ReadBit();
                transportGuid[2] = packet.ReadBit();
                hasTransportTime3 = packet.ReadBit();
                transportGuid[7] = packet.ReadBit();
                hasTransportTime2 = packet.ReadBit();
            }

            guid[6] = packet.ReadBit();
            guid[3] = packet.ReadBit();
            var hasMovementFlagsExtra = !packet.ReadBit();
            packet.ReadBit("Has spline data");

            if (hasMovementFlagsExtra)
                info.FlagsExtra = (uint)packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 12);

            if (hasFallData)
                hasFallDirection = packet.ReadBit();

            guid[0] = packet.ReadBit();

            if (hasSplineElevation)
                info.SplineElevation = packet.ReadSingle("Spline elevation");

            if (hasPitch)
                info.SwimPitch = packet.ReadSingle("Pitch");

            packet.ReadXORByte(guid, 0);
            packet.ReadXORByte(guid, 5);

            if (hasTransport)
            {
                var tpos = new Vector4();

                tpos.O = packet.ReadSingle();
                info.TransportTime = packet.ReadUInt32("Transport Time");
                packet.ReadXORByte(transportGuid, 5);
                packet.ReadXORByte(transportGuid, 4);
                packet.ReadXORByte(transportGuid, 6);
                tpos.Z = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 7);
                packet.ReadXORByte(transportGuid, 0);
                packet.ReadXORByte(transportGuid, 2);
                packet.ReadXORByte(transportGuid, 3);
                tpos.Y = packet.ReadSingle();

                if (hasTransportTime3)
                    packet.ReadUInt32("Transport Time 3");

                if (hasTransportTime2)
                    packet.ReadUInt32("Transport Time 2");

                packet.ReadXORByte(transportGuid, 1);
                info.TransportSeat = packet.ReadSByte("Transport Seat");
                tpos.X = packet.ReadSingle();

                info.TransportGuid = packet.WriteGuid("Transport Guid", transportGuid);
                info.TransportOffset = packet.AddValue("Transport Position", tpos);
            }

            if (hasFallData)
            {
                if (hasFallDirection)
                {
                    info.JumpCosAngle = packet.ReadSingle("Fall Cos");
                    info.JumpHorizontalSpeed = packet.ReadSingle("Horizontal Speed");
                    info.JumpSinAngle = packet.ReadSingle("Fall Sin");
                }

                info.JumpVerticalSpeed = packet.ReadSingle("Vertical Speed");
                info.FallTime = packet.ReadUInt32("Fall time");
            }

            packet.ReadXORByte(guid, 1);
            packet.ReadXORByte(guid, 6);

            if (hasTime)
                info.MoveTime = packet.ReadUInt32("Timestamp");

            packet.ReadXORByte(guid, 7);
            packet.ReadXORByte(guid, 2);
            packet.ReadXORByte(guid, 4);

            if (hasO)
                info.Orientation = pos.O = packet.ReadSingle();

            packet.ReadXORByte(guid, 3);

            WowGuid moverGuid = packet.WriteGuid("Guid", guid);
            packet.AddValue("Position", pos);
            speedUpdate.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(packet.Time);
            Storage.StoreUnitSpeedUpdate(moverGuid, speedUpdate);
            Storage.StorePlayerMovement(moverGuid, info, packet);
        }

        [Parser(Opcode.SMSG_MOVE_UPDATE_COLLISION_HEIGHT)]
        public static void HandleMoveUpdateCollisionHeight434(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasFallDirection = false;
            var pos = new Vector4();
            MovementInfo info = new MovementInfo();

            info.Position.Z = pos.Z = packet.ReadSingle();
            packet.ReadSingle("Height");
            info.Position.X = pos.X = packet.ReadSingle();
            info.Position.Y = pos.Y = packet.ReadSingle();
            guid[6] = packet.ReadBit();
            guid[7] = packet.ReadBit();
            var hasSplineElevation = !packet.ReadBit();
            var hasTransport = packet.ReadBit();

            var hasTransportTime2 = false;
            var hasTransportTime3 = false;
            if (hasTransport)
            {
                transportGuid[6] = packet.ReadBit();
                transportGuid[4] = packet.ReadBit();
                transportGuid[3] = packet.ReadBit();
                transportGuid[7] = packet.ReadBit();
                transportGuid[5] = packet.ReadBit();
                hasTransportTime3 = packet.ReadBit();
                transportGuid[1] = packet.ReadBit();
                transportGuid[0] = packet.ReadBit();
                transportGuid[2] = packet.ReadBit();
                hasTransportTime2 = packet.ReadBit();
            }

            guid[3] = packet.ReadBit();
            var hasO = !packet.ReadBit();
            guid[4] = packet.ReadBit();
            guid[5] = packet.ReadBit();
            var hasTime = !packet.ReadBit();
            var hasFallData = packet.ReadBit();
            guid[0] = packet.ReadBit();
            var hasPitch = !packet.ReadBit();
            packet.ReadBit(); // not sure (offset 157);
            guid[2] = packet.ReadBit();
            var hasMovementFlags = !packet.ReadBit();

            if (hasFallData)
                hasFallDirection = packet.ReadBit();

            guid[1] = packet.ReadBit();
            var hasMovementFlagsExtra = !packet.ReadBit();
            packet.ReadBit("Has spline data"); // not sure (offset 156)

            if (hasMovementFlagsExtra)
                info.FlagsExtra = (uint)packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 12);

            if (hasMovementFlags)
                info.Flags = (uint)packet.ReadBitsE<MovementFlag>("Movement Flags", 30);

            packet.ReadXORByte(guid, 3);

            if (hasTransport)
            {
                var tpos = new Vector4();

                packet.ReadXORByte(transportGuid, 7);
                info.TransportTime = packet.ReadUInt32("Transport Time");
                packet.ReadXORByte(transportGuid, 4);

                if (hasTransportTime3)
                    packet.ReadUInt32("Transport Time 3");

                if (hasTransportTime2)
                    packet.ReadUInt32("Transport Time 2");

                packet.ReadXORByte(transportGuid, 5);
                tpos.O = packet.ReadSingle();
                tpos.X = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 6);
                packet.ReadXORByte(transportGuid, 0);
                tpos.Y = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 2);
                tpos.Z = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 3);
                packet.ReadXORByte(transportGuid, 1);
                info.TransportSeat = packet.ReadSByte("Transport Seat");

                info.TransportGuid = packet.WriteGuid("Transport Guid", transportGuid);
                info.TransportOffset = packet.AddValue("Transport Position", tpos);
            }

            if (hasPitch)
                info.SwimPitch = packet.ReadSingle("Pitch");

            packet.ReadXORByte(guid, 6);

            if (hasFallData)
            {
                if (hasFallDirection)
                {
                    info.JumpCosAngle = packet.ReadSingle("Fall Cos");
                    info.JumpSinAngle = packet.ReadSingle("Fall Sin");
                    info.JumpHorizontalSpeed = packet.ReadSingle("Horizontal Speed");
                }

                info.JumpVerticalSpeed = packet.ReadSingle("Vertical Speed");
                info.FallTime = packet.ReadUInt32("Fall time");
            }

            packet.ReadXORByte(guid, 7);

            if (hasO)
                info.Orientation = pos.O = packet.ReadSingle();

            packet.ReadXORByte(guid, 0);
            packet.ReadXORByte(guid, 5);
            packet.ReadXORByte(guid, 4);

            if (hasSplineElevation)
                info.SplineElevation = packet.ReadSingle("Spline elevation");

            if (hasTime)
                info.MoveTime = packet.ReadUInt32("Timestamp");

            packet.ReadXORByte(guid, 2);
            packet.ReadXORByte(guid, 1);

            WowGuid moverGuid = packet.WriteGuid("Guid", guid);
            packet.AddValue("Position", pos);
            Storage.StorePlayerMovement(moverGuid, info, packet);
        }

        [Parser(Opcode.SMSG_MOVE_UPDATE_TELEPORT)]
        public static void HandleMoveUpdateTeleport434(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasFallDirection = false;
            var pos = new Vector4();
            MovementInfo info = new MovementInfo();

            info.Position.Z = pos.Z = packet.ReadSingle();
            info.Position.Y = pos.Y = packet.ReadSingle();
            info.Position.X = pos.X = packet.ReadSingle();
            var hasO = !packet.ReadBit();
            packet.ReadBit("Has spline data");
            var hasMovementFlags = !packet.ReadBit();
            guid[2] = packet.ReadBit();
            guid[4] = packet.ReadBit();
            guid[6] = packet.ReadBit();
            var hasFallData = packet.ReadBit();
            guid[0] = packet.ReadBit();
            var hasTransport = packet.ReadBit();
            guid[5] = packet.ReadBit();

            var hasTransportTime2 = false;
            var hasTransportTime3 = false;
            if (hasTransport)
            {
                transportGuid[1] = packet.ReadBit();
                transportGuid[4] = packet.ReadBit();
                transportGuid[5] = packet.ReadBit();
                transportGuid[3] = packet.ReadBit();
                transportGuid[0] = packet.ReadBit();
                hasTransportTime2 = packet.ReadBit();
                transportGuid[7] = packet.ReadBit();
                transportGuid[6] = packet.ReadBit();
                hasTransportTime3 = packet.ReadBit();
                transportGuid[2] = packet.ReadBit();
            }

            packet.ReadBit();
            guid[7] = packet.ReadBit();
            guid[3] = packet.ReadBit();
            var hasPitch = !packet.ReadBit();
            var hasMovementFlagsExtra = !packet.ReadBit();
            var hasTime = !packet.ReadBit();

            if (hasFallData)
                hasFallDirection = packet.ReadBit();

            if (hasMovementFlagsExtra)
                info.FlagsExtra = (uint)packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 12);

            var hasSplineElevation = !packet.ReadBit();

            if (hasMovementFlags)
                info.Flags = (uint)packet.ReadBitsE<MovementFlag>("Movement Flags", 30);

            guid[1] = packet.ReadBit();
            packet.ReadXORByte(guid, 7);

            if (hasTransport)
            {
                var tpos = new Vector4();

                packet.ReadXORByte(transportGuid, 3);
                packet.ReadXORByte(transportGuid, 4);
                tpos.O = packet.ReadSingle();

                if (hasTransportTime3)
                    packet.ReadUInt32("Transport Time 3");

                packet.ReadXORByte(transportGuid, 1);

                if (hasTransportTime2)
                    packet.ReadUInt32("Transport Time 2");

                tpos.Z = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 7);
                packet.ReadXORByte(transportGuid, 0);
                packet.ReadXORByte(transportGuid, 6);
                packet.ReadXORByte(transportGuid, 5);
                packet.ReadXORByte(transportGuid, 2);
                info.TransportSeat = packet.ReadSByte("Transport Seat");
                info.TransportTime = packet.ReadUInt32("Transport Time");
                tpos.Y = packet.ReadSingle();
                tpos.X = packet.ReadSingle();

                info.TransportGuid = packet.WriteGuid("Transport Guid", transportGuid);
                info.TransportOffset = packet.AddValue("Transport Position", tpos);
            }

            packet.ReadXORByte(guid, 6);

            if (hasPitch)
                info.SwimPitch = packet.ReadSingle("Pitch");

            if (hasSplineElevation)
                info.SplineElevation = packet.ReadSingle("Spline elevation");

            if (hasO)
                info.Orientation = pos.O = packet.ReadSingle();

            packet.ReadXORByte(guid, 2);
            packet.ReadXORByte(guid, 3);
            packet.ReadXORByte(guid, 1);

            if (hasFallData)
            {
                info.FallTime = packet.ReadUInt32("Fall time");

                if (hasFallDirection)
                {
                    info.JumpHorizontalSpeed = packet.ReadSingle("Horizontal Speed");
                    info.JumpSinAngle = packet.ReadSingle("Fall Sin");
                    info.JumpCosAngle = packet.ReadSingle("Fall Cos");
                }

                info.JumpVerticalSpeed = packet.ReadSingle("Vertical Speed");
            }

            packet.ReadXORByte(guid, 5);
            packet.ReadXORByte(guid, 4);

            if (hasTime)
                info.MoveTime = packet.ReadUInt32("Timestamp");

            packet.ReadXORByte(guid, 0);

            WowGuid moverGuid = packet.WriteGuid("Guid", guid);
            packet.AddValue("Position", pos);
            Storage.StorePlayerMovement(moverGuid, info, packet);
        }

        [Parser(Opcode.SMSG_MOVE_SPLINE_SET_SWIM_BACK_SPEED)]
        public static void HandleSplineSetSwimBackSpeed434(Packet packet)
        {
            var guid = packet.StartBitStream(0, 1, 3, 6, 4, 5, 7, 2);
            packet.ReadXORByte(guid, 5);
            packet.ReadXORByte(guid, 3);
            packet.ReadXORByte(guid, 1);
            packet.ReadXORByte(guid, 0);
            packet.ReadXORByte(guid, 7);
            packet.ReadXORByte(guid, 6);
            CreatureSpeedUpdate speedUpdate = new CreatureSpeedUpdate();
            speedUpdate.SpeedType = SpeedType.SwimBack;
            speedUpdate.SpeedRate = packet.ReadSingle("Speed") / MovementInfo.DEFAULT_SWIM_BACK_SPEED;
            packet.ReadXORByte(guid, 4);
            packet.ReadXORByte(guid, 2);
            WowGuid moverGuid = packet.WriteGuid("Guid", guid);
            speedUpdate.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(packet.Time);
            Storage.StoreUnitSpeedUpdate(moverGuid, speedUpdate);
        }

        [Parser(Opcode.SMSG_MOVE_SPLINE_SET_FLIGHT_BACK_SPEED)]
        public static void HandleSplineSetFlightBackSpeed434(Packet packet)
        {
            var guid = packet.StartBitStream(2, 1, 6, 5, 0, 3, 4, 7);
            packet.ReadXORByte(guid, 5);
            CreatureSpeedUpdate speedUpdate = new CreatureSpeedUpdate();
            speedUpdate.SpeedType = SpeedType.FlyBack;
            speedUpdate.SpeedRate = packet.ReadSingle("Speed") / MovementInfo.DEFAULT_FLY_BACK_SPEED;
            packet.ReadXORByte(guid, 6);
            packet.ReadXORByte(guid, 1);
            packet.ReadXORByte(guid, 0);
            packet.ReadXORByte(guid, 2);
            packet.ReadXORByte(guid, 3);
            packet.ReadXORByte(guid, 7);
            packet.ReadXORByte(guid, 4);
            WowGuid moverGuid = packet.WriteGuid("Guid", guid);
            speedUpdate.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(packet.Time);
            Storage.StoreUnitSpeedUpdate(moverGuid, speedUpdate);
        }

        [Parser(Opcode.SMSG_MOVE_SPLINE_SET_TURN_RATE)]
        public static void HandleSplineSetTurnRate434(Packet packet)
        {
            var guid = packet.StartBitStream(2, 4, 6, 1, 3, 5, 7, 0);
            CreatureSpeedUpdate speedUpdate = new CreatureSpeedUpdate();
            speedUpdate.SpeedType = SpeedType.Turn;
            speedUpdate.SpeedRate = packet.ReadSingle("Rate") / MovementInfo.DEFAULT_TURN_RATE;
            packet.ParseBitStream(guid, 1, 5, 3, 2, 7, 4, 6, 0);
            WowGuid moverGuid = packet.WriteGuid("Guid", guid);
            speedUpdate.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(packet.Time);
            Storage.StoreUnitSpeedUpdate(moverGuid, speedUpdate);
        }

        [Parser(Opcode.SMSG_MOVE_SPLINE_SET_PITCH_RATE)]
        public static void HandleSplineSetPitchRate434(Packet packet)
        {
            var guid = packet.StartBitStream(3, 5, 6, 1, 0, 4, 7, 2);
            packet.ReadXORByte(guid, 1);
            packet.ReadXORByte(guid, 5);
            packet.ReadXORByte(guid, 7);
            packet.ReadXORByte(guid, 0);
            packet.ReadXORByte(guid, 6);
            packet.ReadXORByte(guid, 3);
            packet.ReadXORByte(guid, 2);
            CreatureSpeedUpdate speedUpdate = new CreatureSpeedUpdate();
            speedUpdate.SpeedType = SpeedType.Pitch;
            speedUpdate.SpeedRate = packet.ReadSingle("Rate") / MovementInfo.DEFAULT_PITCH_RATE;
            packet.ReadXORByte(guid, 4);
            WowGuid moverGuid = packet.WriteGuid("Guid", guid);
            speedUpdate.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(packet.Time);
            Storage.StoreUnitSpeedUpdate(moverGuid, speedUpdate);
        }

        [Parser(Opcode.SMSG_MOVE_SPLINE_ROOT)]
        public static void HandleSplineMoveRoot434(Packet packet)
        {
            var guid = packet.StartBitStream(5, 4, 6, 1, 3, 7, 2, 0);
            packet.ParseBitStream(guid, 2, 1, 7, 3, 5, 0, 6, 4);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_MOVE_SPLINE_UNROOT)]
        public static void HandleSplineMoveUnroot434(Packet packet)
        {
            var guid = packet.StartBitStream(0, 1, 6, 5, 3, 2, 7, 4);
            packet.ParseBitStream(guid, 6, 3, 1, 5, 2, 0, 7, 4);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_MOVE_SPLINE_ENABLE_GRAVITY)]
        public static void HandleSplineMoveGravityEnable434(Packet packet)
        {
            var guid = packet.StartBitStream(5, 4, 7, 1, 3, 6, 2, 0);
            packet.ParseBitStream(guid, 7, 3, 4, 2, 1, 6, 0, 5);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_MOVE_SPLINE_DISABLE_GRAVITY)]
        public static void HandleSplineMoveGravityDisable434(Packet packet)
        {
            var guid = packet.StartBitStream(7, 3, 4, 2, 5, 1, 0, 6);
            packet.ParseBitStream(guid, 7, 1, 3, 4, 6, 2, 5, 0);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_MOVE_SPLINE_ENABLE_COLLISION)]
        public static void HandleSplineMoveCollisionEnable434(Packet packet)
        {
            var guid = packet.StartBitStream(3, 4, 7, 6, 1, 0, 2, 5);
            packet.ParseBitStream(guid, 1, 3, 7, 2, 0, 6, 4, 5);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_MOVE_SPLINE_DISABLE_COLLISION)]
        public static void HandleSplineMoveCollisionDisable434(Packet packet)
        {
            var guid = packet.StartBitStream(3, 7, 1, 0, 4, 2, 6, 5);
            packet.ParseBitStream(guid, 3, 5, 6, 7, 2, 1, 0, 4);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_MOVE_SPLINE_SET_RUN_MODE)]
        public static void HandleSplineSetRunMode434(Packet packet)
        {
            var guid = packet.StartBitStream(5, 6, 3, 7, 2, 0, 4, 1);
            packet.ParseBitStream(guid, 7, 0, 4, 6, 5, 1, 2, 3);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_MOVE_SPLINE_SET_WALK_MODE)]
        public static void HandleSplineSetWalkMode434(Packet packet)
        {
            var guid = packet.StartBitStream(7, 6, 5, 1, 3, 4, 2, 0);
            packet.ParseBitStream(guid, 4, 2, 1, 6, 5, 0, 7, 3);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_MOVE_SPLINE_SET_HOVER)]
        public static void HandleSplineSetHover434(Packet packet)
        {
            var guid = packet.StartBitStream(3, 7, 0, 1, 4, 6, 2, 5);
            packet.ParseBitStream(guid, 2, 4, 3, 1, 7, 0, 5, 6);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_MOVE_SPLINE_UNSET_HOVER)]
        public static void HandleSplineUnsetHover434(Packet packet)
        {
            var guid = packet.StartBitStream(6, 7, 4, 0, 3, 1, 5, 2);
            packet.ParseBitStream(guid, 4, 5, 3, 0, 2, 7, 6, 1);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_MOVE_SPLINE_SET_WATER_WALK)]
        public static void HandleSplineMoveWaterWalk434(Packet packet)
        {
            var guid = packet.StartBitStream(6, 1, 4, 2, 3, 7, 5, 0);
            packet.ParseBitStream(guid, 0, 6, 3, 7, 4, 2, 5, 1);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_MOVE_SPLINE_START_SWIM)]
        public static void HandleSplineMoveStartSwim434(Packet packet)
        {
            var guid = packet.StartBitStream(1, 6, 0, 7, 3, 5, 2, 4);
            packet.ParseBitStream(guid, 3, 7, 2, 5, 6, 4, 1, 0);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_MOVE_SPLINE_STOP_SWIM)]
        public static void HandleSplineMoveStopSwim434(Packet packet)
        {
            var guid = packet.StartBitStream(4, 1, 5, 3, 0, 7, 2, 6);
            packet.ParseBitStream(guid, 6, 0, 7, 2, 3, 1, 5, 4);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_MOVE_SPLINE_SET_FLYING)]
        public static void HandleSplineMoveSetFlying434(Packet packet)
        {
            var guid = packet.StartBitStream(0, 4, 1, 6, 7, 2, 3, 5);
            packet.ParseBitStream(guid, 7, 0, 5, 6, 4, 1, 3, 2);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_MOVE_SPLINE_UNSET_FLYING)]
        public static void HandleSplineMoveUnsetFlying434(Packet packet)
        {
            var guid = packet.StartBitStream(5, 0, 4, 7, 2, 3, 1, 6);
            packet.ParseBitStream(guid, 7, 2, 3, 4, 5, 1, 6, 0);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_MOVE_SET_RUN_SPEED)]
        public static void HandleMoveSetRunSpeed434(Packet packet)
        {
            var guid = packet.StartBitStream(6, 1, 5, 2, 7, 0, 3, 4);
            packet.ReadXORByte(guid, 5);
            packet.ReadXORByte(guid, 3);
            packet.ReadXORByte(guid, 1);
            packet.ReadXORByte(guid, 4);
            packet.ReadInt32("Movement Counter");
            CreatureSpeedUpdate speedUpdate = new CreatureSpeedUpdate();
            speedUpdate.SpeedType = SpeedType.Run;
            speedUpdate.SpeedRate = packet.ReadSingle("Speed") / MovementInfo.DEFAULT_RUN_SPEED;
            packet.ReadXORByte(guid, 6);
            packet.ReadXORByte(guid, 0);
            packet.ReadXORByte(guid, 7);
            packet.ReadXORByte(guid, 2);
            WowGuid moverGuid = packet.WriteGuid("Guid", guid);
            speedUpdate.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(packet.Time);
            Storage.StoreUnitSpeedUpdate(moverGuid, speedUpdate);
        }

        [Parser(Opcode.SMSG_MOVE_ROOT)]
        public static void HandleMoveRoot434(Packet packet)
        {
            var guid = packet.StartBitStream(2, 7, 6, 0, 5, 4, 1, 3);
            packet.ReadXORByte(guid, 1);
            packet.ReadXORByte(guid, 0);
            packet.ReadXORByte(guid, 2);
            packet.ReadXORByte(guid, 5);
            packet.ReadInt32("Movement Counter");
            packet.ReadXORByte(guid, 3);
            packet.ReadXORByte(guid, 4);
            packet.ReadXORByte(guid, 7);
            packet.ReadXORByte(guid, 6);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_MOVE_UNROOT)]
        public static void HandleMoveUnroot434(Packet packet)
        {
            var guid = packet.StartBitStream(0, 1, 3, 7, 5, 2, 4, 6);
            packet.ReadXORByte(guid, 3);
            packet.ReadXORByte(guid, 6);
            packet.ReadXORByte(guid, 1);
            packet.ReadInt32("Movement Counter");
            packet.ReadXORByte(guid, 2);
            packet.ReadXORByte(guid, 0);
            packet.ReadXORByte(guid, 7);
            packet.ReadXORByte(guid, 4);
            packet.ReadXORByte(guid, 5);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.CMSG_MOVE_FORCE_RUN_SPEED_CHANGE_ACK)]
        public static void HandleMoveForceRunSpeedChangeAck434(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();
            MovementInfo info = new MovementInfo();

            packet.ReadInt32("Movement Counter");
            info.Position.X = pos.X = packet.ReadSingle();
            CreatureSpeedUpdate speedUpdate = new CreatureSpeedUpdate();
            speedUpdate.SpeedType = SpeedType.Run;
            speedUpdate.SpeedRate = packet.ReadSingle("Speed") / MovementInfo.DEFAULT_RUN_SPEED;
            info.Position.Z = pos.Z = packet.ReadSingle();
            info.Position.Y = pos.Y = packet.ReadSingle();
            guid[2] = packet.ReadBit();
            guid[4] = packet.ReadBit();
            guid[1] = packet.ReadBit();
            guid[7] = packet.ReadBit();
            var hasO = !packet.ReadBit();
            var hasFallData = packet.ReadBit("Has fall data");
            guid[0] = packet.ReadBit();
            packet.ReadBit("Has Spline");
            var hasTrans = packet.ReadBit("Has transport");
            var hasTime = !packet.ReadBit("Has timestamp");
            var hasMovementFlags2 = !packet.ReadBit();
            guid[6] = packet.ReadBit();
            packet.ReadBit();
            var hasSplineElev = !packet.ReadBit("Has Spline Elevation");
            var hasPitch = !packet.ReadBit("Has pitch");
            guid[5] = packet.ReadBit();
            var hasMovementFlags = !packet.ReadBit();
            guid[3] = packet.ReadBit();

            if (hasTrans)
            {
                hasTransTime3 = packet.ReadBit();
                transportGuid[5] = packet.ReadBit();
                hasTransTime2 = packet.ReadBit();
                transportGuid[3] = packet.ReadBit();
                transportGuid[2] = packet.ReadBit();
                transportGuid[0] = packet.ReadBit();
                transportGuid[7] = packet.ReadBit();
                transportGuid[6] = packet.ReadBit();
                transportGuid[1] = packet.ReadBit();
                transportGuid[4] = packet.ReadBit();
            }

            if (hasMovementFlags)
                info.Flags = (uint)packet.ReadBitsE<MovementFlag>("Movement Flags", 30);

            if (hasFallData)
                hasFallDirection = packet.ReadBit();

            if (hasMovementFlags2)
                info.FlagsExtra = (uint)packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 12);

            packet.ParseBitStream(guid, 6, 4, 1, 3, 5, 2, 7, 0);

            if (hasTrans)
            {
                var tpos = new Vector4();

                tpos.Z = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 6);
                packet.ReadXORByte(transportGuid, 1);
                tpos.Y = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 0);
                packet.ReadXORByte(transportGuid, 5);

                if (hasTransTime2)
                    packet.ReadUInt32("Transport Time 2");

                tpos.X = packet.ReadSingle();
                info.TransportTime = packet.ReadUInt32("Transport Time");
                packet.ReadXORByte(transportGuid, 7);
                tpos.O = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 3);

                if (hasTransTime3)
                    packet.ReadUInt32("Transport Time 3");

                packet.ReadXORByte(transportGuid, 2);
                info.TransportSeat = packet.ReadSByte("Transport Seat");
                packet.ReadXORByte(transportGuid, 4);

                info.TransportGuid = packet.WriteGuid("Transport Guid", transportGuid);
                info.TransportOffset = packet.AddValue("Transport Position", tpos);
            }

            if (hasFallData)
            {
                info.JumpVerticalSpeed = packet.ReadSingle("Vertical Speed");

                if (hasFallDirection)
                {
                    info.JumpHorizontalSpeed = packet.ReadSingle("Horizontal Speed");
                    info.JumpCosAngle = packet.ReadSingle("Fall Cos");
                    info.JumpSinAngle = packet.ReadSingle("Fall Sin");
                }

                info.FallTime = packet.ReadUInt32("Fall time");
            }

            if (hasSplineElev)
                info.SplineElevation = packet.ReadSingle("Spline elevation");
            if (hasPitch)
                info.SwimPitch = packet.ReadSingle("Pitch");
            if (hasTime)
                info.MoveTime = packet.ReadUInt32("Timestamp");
            if (hasO)
                info.Orientation = pos.O = packet.ReadSingle();

            WowGuid moverGuid = packet.WriteGuid("Guid", guid);
            packet.AddValue("Position", pos);
            speedUpdate.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(packet.Time);
            Storage.StoreUnitSpeedUpdate(moverGuid, speedUpdate);
            Storage.StorePlayerMovement(moverGuid, info, packet);
        }

        [Parser(Opcode.CMSG_MOVE_SET_COLLISION_HEIGHT_ACK)]
        public static void HandleMoveSetCollisionHeightAck434(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();
            MovementInfo info = new MovementInfo();

            packet.ReadSingle("Collision height");
            info.Position.X = pos.X = packet.ReadSingle();
            info.Position.Z = pos.Z = packet.ReadSingle();
            packet.ReadInt32("Movement Counter");
            info.Position.Y = pos.Y = packet.ReadSingle();
            guid[6] = packet.ReadBit();
            guid[4] = packet.ReadBit();
            packet.ReadBits("Reason", 2);
            var hasPitch = !packet.ReadBit("Has pitch");
            guid[5] = packet.ReadBit();
            packet.ReadBit();
            guid[2] = packet.ReadBit();
            guid[1] = packet.ReadBit();
            var hasFallData = packet.ReadBit("Has fall data");
            guid[3] = packet.ReadBit();
            packet.ReadBit("Has Spline");
            guid[7] = packet.ReadBit();
            var hasMovementFlags = !packet.ReadBit();
            var hasTrans = packet.ReadBit("Has transport");
            var hasTime = !packet.ReadBit("Has timestamp");
            var hasSplineElev = !packet.ReadBit("Has Spline Elevation");
            var hasMovementFlags2 = !packet.ReadBit();
            var hasO = !packet.ReadBit();
            guid[0] = packet.ReadBit();

            if (hasTrans)
            {
                transportGuid[4] = packet.ReadBit();
                transportGuid[3] = packet.ReadBit();
                hasTransTime2 = packet.ReadBit();
                hasTransTime3 = packet.ReadBit();
                transportGuid[5] = packet.ReadBit();
                transportGuid[1] = packet.ReadBit();
                transportGuid[7] = packet.ReadBit();
                transportGuid[2] = packet.ReadBit();
                transportGuid[6] = packet.ReadBit();
                transportGuid[0] = packet.ReadBit();
            }

            if (hasMovementFlags2)
                info.FlagsExtra = (uint)packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 12);

            if (hasMovementFlags)
                info.Flags = (uint)packet.ReadBitsE<MovementFlag>("Movement Flags", 30);

            if (hasFallData)
                hasFallDirection = packet.ReadBit();

            packet.ParseBitStream(guid, 0, 3, 1, 5, 7, 6, 2, 4);

            if (hasTrans)
            {
                var tpos = new Vector4();

                tpos.X = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 4);

                if (hasTransTime2)
                    packet.ReadUInt32("Transport Time 2");

                packet.ReadXORByte(transportGuid, 0);
                tpos.O = packet.ReadSingle();
                tpos.Y = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 7);
                info.TransportSeat = packet.ReadSByte("Transport Seat");
                packet.ReadXORByte(transportGuid, 5);
                packet.ReadXORByte(transportGuid, 2);
                info.TransportTime = packet.ReadUInt32("Transport Time");
                packet.ReadXORByte(transportGuid, 6);
                packet.ReadXORByte(transportGuid, 3);
                packet.ReadXORByte(transportGuid, 1);

                if (hasTransTime3)
                    packet.ReadUInt32("Transport Time 3");

                tpos.Z = packet.ReadSingle();

                info.TransportGuid = packet.WriteGuid("Transport Guid", transportGuid);
                info.TransportOffset = packet.AddValue("Transport Position", tpos);
            }

            if (hasFallData)
            {
                info.JumpVerticalSpeed = packet.ReadSingle("Vertical Speed");
                info.FallTime = packet.ReadUInt32("Fall time");

                if (hasFallDirection)
                {
                    info.JumpSinAngle = packet.ReadSingle("Fall Sin");
                    info.JumpCosAngle = packet.ReadSingle("Fall Cos");
                    info.JumpHorizontalSpeed = packet.ReadSingle("Horizontal Speed");
                }
            }

            if (hasTime)
                info.MoveTime = packet.ReadUInt32("Timestamp");
            if (hasSplineElev)
                info.SplineElevation = packet.ReadSingle("Spline elevation");
            if (hasO)
                info.Orientation = pos.O = packet.ReadSingle();
            if (hasPitch)
                info.SwimPitch = packet.ReadSingle("Pitch");

            WowGuid moverGuid = packet.WriteGuid("Guid", guid);
            packet.AddValue("Position", pos);
            Storage.StorePlayerMovement(moverGuid, info, packet);
        }

        [Parser(Opcode.CMSG_MOVE_FORCE_FLIGHT_SPEED_CHANGE_ACK)]
        public static void HandleMoveForceFlightSpeedChangeAck434(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();
            MovementInfo info = new MovementInfo();

            packet.ReadInt32("Movement Counter");
            info.Position.Z = pos.Z = packet.ReadSingle();
            info.Position.X = pos.X = packet.ReadSingle();
            info.Position.Y = pos.Y = packet.ReadSingle();
            CreatureSpeedUpdate speedUpdate = new CreatureSpeedUpdate();
            speedUpdate.SpeedType = SpeedType.Fly;
            speedUpdate.SpeedRate = packet.ReadSingle("Speed") / MovementInfo.DEFAULT_FLY_SPEED;
            guid[4] = packet.ReadBit();
            guid[5] = packet.ReadBit();
            packet.ReadBit();
            var hasMovementFlags = !packet.ReadBit();
            var hasSplineElev = !packet.ReadBit("Has Spline Elevation");
            guid[3] = packet.ReadBit();
            var hasMovementFlags2 = !packet.ReadBit();
            guid[7] = packet.ReadBit();
            var hasTrans = packet.ReadBit("Has transport");
            guid[0] = packet.ReadBit();
            var hasFallData = packet.ReadBit("Has fall data");
            var hasTime = !packet.ReadBit("Has timestamp");
            var hasPitch = !packet.ReadBit("Has pitch");
            packet.ReadBit("Has Spline");
            var hasO = !packet.ReadBit();
            guid[6] = packet.ReadBit();
            guid[2] = packet.ReadBit();
            guid[1] = packet.ReadBit();

            if (hasTrans)
            {
                transportGuid[6] = packet.ReadBit();
                transportGuid[1] = packet.ReadBit();
                transportGuid[7] = packet.ReadBit();
                hasTransTime2 = packet.ReadBit();
                transportGuid[0] = packet.ReadBit();
                hasTransTime3 = packet.ReadBit();
                transportGuid[3] = packet.ReadBit();
                transportGuid[2] = packet.ReadBit();
                transportGuid[5] = packet.ReadBit();
                transportGuid[4] = packet.ReadBit();
            }

            if (hasFallData)
                hasFallDirection = packet.ReadBit();

            if (hasMovementFlags2)
                info.FlagsExtra = (uint)packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 12);

            if (hasMovementFlags)
                info.Flags = (uint)packet.ReadBitsE<MovementFlag>("Movement Flags", 30);

            packet.ParseBitStream(guid, 5, 6, 1, 7, 3, 0, 2, 4);

            if (hasTrans)
            {
                var tpos = new Vector4();

                packet.ReadXORByte(transportGuid, 6);
                packet.ReadXORByte(transportGuid, 0);
                info.TransportSeat = packet.ReadSByte("Transport Seat");

                if (hasTransTime2)
                    packet.ReadUInt32("Transport Time 2");

                tpos.O = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 1);
                packet.ReadXORByte(transportGuid, 5);

                if (hasTransTime3)
                    packet.ReadUInt32("Transport Time 3");

                packet.ReadXORByte(transportGuid, 2);
                tpos.Z = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 7);
                packet.ReadXORByte(transportGuid, 3);
                packet.ReadXORByte(transportGuid, 4);
                tpos.X = packet.ReadSingle();
                info.TransportTime = packet.ReadUInt32("Transport Time");
                tpos.Y = packet.ReadSingle();

                info.TransportGuid = packet.WriteGuid("Transport Guid", transportGuid);
                info.TransportOffset = packet.AddValue("Transport Position", tpos);
            }

            if (hasTime)
                info.MoveTime = packet.ReadUInt32("Timestamp");
            if (hasSplineElev)
                info.SplineElevation = packet.ReadSingle("Spline elevation");

            if (hasFallData)
            {
                if (hasFallDirection)
                {
                    info.JumpCosAngle = packet.ReadSingle("Fall Cos");
                    info.JumpSinAngle = packet.ReadSingle("Fall Sin");
                    info.JumpHorizontalSpeed = packet.ReadSingle("Horizontal Speed");
                }

                info.FallTime = packet.ReadUInt32("Fall time");
                info.JumpVerticalSpeed = packet.ReadSingle("Vertical Speed");
            }

            if (hasPitch)
                info.SwimPitch = packet.ReadSingle("Pitch");
            if (hasO)
                info.Orientation = pos.O = packet.ReadSingle();

            WowGuid moverGuid = packet.WriteGuid("Guid", guid);
            packet.AddValue("Position", pos);
            speedUpdate.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(packet.Time);
            Storage.StoreUnitSpeedUpdate(moverGuid, speedUpdate);
            Storage.StorePlayerMovement(moverGuid, info, packet);
        }

        [Parser(Opcode.CMSG_MOVE_SET_CAN_FLY_ACK)]
        public static void HandleMoveSetCanFlyAck434(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();
            MovementInfo info = new MovementInfo();

            info.Position.Y = pos.Y = packet.ReadSingle();
            packet.ReadInt32("Movement Counter");
            info.Position.X = pos.X = packet.ReadSingle();
            info.Position.Z = pos.Z = packet.ReadSingle();
            guid[3] = packet.ReadBit();
            var hasTime = !packet.ReadBit("Has timestamp");
            guid[4] = packet.ReadBit();
            guid[0] = packet.ReadBit();
            var hasO = !packet.ReadBit();
            var hasFallData = packet.ReadBit("Has fall data");
            guid[2] = packet.ReadBit();
            guid[5] = packet.ReadBit();
            var hasSplineElev = !packet.ReadBit("Has Spline Elevation");
            var hasMovementFlags2 = !packet.ReadBit();
            packet.ReadBit();
            guid[7] = packet.ReadBit();
            packet.ReadBit("Has Spline");
            guid[6] = packet.ReadBit();
            guid[1] = packet.ReadBit();
            var hasMovementFlags = !packet.ReadBit();
            var hasTrans = packet.ReadBit("Has transport");
            var hasPitch = !packet.ReadBit("Has pitch");

            if (hasTrans)
            {
                transportGuid[3] = packet.ReadBit();
                transportGuid[1] = packet.ReadBit();
                transportGuid[2] = packet.ReadBit();
                hasTransTime3 = packet.ReadBit();
                hasTransTime2 = packet.ReadBit();
                transportGuid[0] = packet.ReadBit();
                transportGuid[5] = packet.ReadBit();
                transportGuid[7] = packet.ReadBit();
                transportGuid[4] = packet.ReadBit();
                transportGuid[6] = packet.ReadBit();
            }

            if (hasMovementFlags2)
                info.FlagsExtra = (uint)packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 12);

            if (hasMovementFlags)
                info.Flags = (uint)packet.ReadBitsE<MovementFlag>("Movement Flags", 30);

            if (hasFallData)
                hasFallDirection = packet.ReadBit();

            packet.ParseBitStream(guid, 1, 0, 2, 3, 7, 6, 4, 5);

            if (hasTrans)
            {
                var tpos = new Vector4();

                if (hasTransTime2)
                    packet.ReadUInt32("Transport Time 2");

                packet.ReadXORByte(transportGuid, 6);
                info.TransportTime = packet.ReadUInt32("Transport Time");

                if (hasTransTime3)
                    packet.ReadUInt32("Transport Time 3");

                packet.ReadXORByte(transportGuid, 7);
                tpos.Z = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 3);
                tpos.Y = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 5);
                tpos.X = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 2);
                tpos.O = packet.ReadSingle();
                info.TransportSeat = packet.ReadSByte("Transport Seat");
                packet.ReadXORByte(transportGuid, 1);
                packet.ReadXORByte(transportGuid, 0);
                packet.ReadXORByte(transportGuid, 4);

                info.TransportGuid = packet.WriteGuid("Transport Guid", transportGuid);
                info.TransportOffset = packet.AddValue("Transport Position", tpos);
            }

            if (hasFallData)
            {
                info.FallTime = packet.ReadUInt32("Fall time");

                if (hasFallDirection)
                {
                    info.JumpCosAngle = packet.ReadSingle("Fall Cos");
                    info.JumpHorizontalSpeed = packet.ReadSingle("Horizontal Speed");
                    info.JumpSinAngle = packet.ReadSingle("Fall Sin");
                }

                info.JumpVerticalSpeed = packet.ReadSingle("Vertical Speed");
            }

            if (hasPitch)
                info.SwimPitch = packet.ReadSingle("Pitch");
            if (hasO)
                info.Orientation = pos.O = packet.ReadSingle();
            if (hasTime)
                info.MoveTime = packet.ReadUInt32("Timestamp");
            if (hasSplineElev)
                info.SplineElevation = packet.ReadSingle("Spline elevation");

            WowGuid moverGuid = packet.WriteGuid("Guid", guid);
            packet.AddValue("Position", pos);
            Storage.StorePlayerMovement(moverGuid, info, packet);
        }

        [Parser(Opcode.CMSG_MOVE_FORCE_SWIM_SPEED_CHANGE_ACK)]
        public static void HandleMoveForceSwimSpeedChangeAck434(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();
            MovementInfo info = new MovementInfo();

            info.Position.X = pos.X = packet.ReadSingle();
            packet.ReadInt32("Movement Counter");
            CreatureSpeedUpdate speedUpdate = new CreatureSpeedUpdate();
            speedUpdate.SpeedType = SpeedType.Swim;
            speedUpdate.SpeedRate = packet.ReadSingle("Speed") / MovementInfo.DEFAULT_SWIM_SPEED;
            info.Position.Y = pos.Y = packet.ReadSingle();
            info.Position.Z = pos.Z = packet.ReadSingle();
            guid[4] = packet.ReadBit();
            var hasO = !packet.ReadBit();
            var hasPitch = !packet.ReadBit("Has pitch");
            var hasTrans = packet.ReadBit("Has transport");
            var hasMovementFlags = !packet.ReadBit();
            var hasMovementFlags2 = !packet.ReadBit();
            guid[1] = packet.ReadBit();
            guid[3] = packet.ReadBit();
            var hasFallData = packet.ReadBit("Has fall data");
            guid[2] = packet.ReadBit();
            guid[7] = packet.ReadBit();
            var hasTime = !packet.ReadBit("Has timestamp");
            packet.ReadBit();
            guid[5] = packet.ReadBit();
            guid[6] = packet.ReadBit();
            var hasSplineElev = !packet.ReadBit("Has Spline Elevation");
            packet.ReadBit("Has Spline");
            guid[0] = packet.ReadBit();

            if (hasTrans)
            {
                transportGuid[1] = packet.ReadBit();
                transportGuid[4] = packet.ReadBit();
                transportGuid[0] = packet.ReadBit();
                transportGuid[5] = packet.ReadBit();
                transportGuid[7] = packet.ReadBit();
                transportGuid[3] = packet.ReadBit();
                hasTransTime2 = packet.ReadBit();
                hasTransTime3 = packet.ReadBit();
                transportGuid[6] = packet.ReadBit();
                transportGuid[2] = packet.ReadBit();
            }

            if (hasMovementFlags2)
                info.FlagsExtra = (uint)packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 12);

            if (hasFallData)
                hasFallDirection = packet.ReadBit();

            if (hasMovementFlags)
                info.Flags = (uint)packet.ReadBitsE<MovementFlag>("Movement Flags", 30);

            packet.ParseBitStream(guid, 2, 0, 6, 5, 1, 3, 4, 7);

            if (hasTrans)
            {
                var tpos = new Vector4();

                packet.ReadXORByte(transportGuid, 0);

                if (hasTransTime3)
                    packet.ReadUInt32("Transport Time 3");

                if (hasTransTime2)
                    packet.ReadUInt32("Transport Time 2");

                tpos.X = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 7);
                info.TransportSeat = packet.ReadSByte("Transport Seat");
                tpos.Y = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 5);
                tpos.Z = packet.ReadSingle();
                tpos.O = packet.ReadSingle();
                info.TransportTime = packet.ReadUInt32("Transport Time");
                packet.ReadXORByte(transportGuid, 2);
                packet.ReadXORByte(transportGuid, 6);
                packet.ReadXORByte(transportGuid, 3);
                packet.ReadXORByte(transportGuid, 1);
                packet.ReadXORByte(transportGuid, 4);

                info.TransportGuid = packet.WriteGuid("Transport Guid", transportGuid);
                info.TransportOffset = packet.AddValue("Transport Position", tpos);
            }

            if (hasFallData)
            {
                info.FallTime = packet.ReadUInt32("Fall time");

                if (hasFallDirection)
                {
                    info.JumpCosAngle = packet.ReadSingle("Fall Cos");
                    info.JumpSinAngle = packet.ReadSingle("Fall Sin");
                    info.JumpHorizontalSpeed = packet.ReadSingle("Horizontal Speed");
                }

                info.JumpVerticalSpeed = packet.ReadSingle("Vertical Speed");
            }

            if (hasO)
                info.Orientation = pos.O = packet.ReadSingle();
            if (hasPitch)
                info.SwimPitch = packet.ReadSingle("Pitch");
            if (hasSplineElev)
                info.SplineElevation = packet.ReadSingle("Spline elevation");
            if (hasTime)
                info.MoveTime = packet.ReadUInt32("Timestamp");

            WowGuid moverGuid = packet.WriteGuid("Guid", guid);
            packet.AddValue("Position", pos);
            speedUpdate.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(packet.Time);
            Storage.StoreUnitSpeedUpdate(moverGuid, speedUpdate);
            Storage.StorePlayerMovement(moverGuid, info, packet);
        }

        [Parser(Opcode.CMSG_MOVE_FORCE_WALK_SPEED_CHANGE_ACK)]
        public static void HandleMoveForceWalkSpeedChangeAck434(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();
            MovementInfo info = new MovementInfo();

            info.Position.Z = pos.Z = packet.ReadSingle();
            info.Position.Y = pos.Y = packet.ReadSingle();
            CreatureSpeedUpdate speedUpdate = new CreatureSpeedUpdate();
            speedUpdate.SpeedType = SpeedType.Walk;
            speedUpdate.SpeedRate = packet.ReadSingle("Speed") / MovementInfo.DEFAULT_WALK_SPEED;
            info.Position.X = pos.X = packet.ReadSingle();
            packet.ReadInt32("Movement Counter");
            guid[0] = packet.ReadBit();
            var hasMovementFlags2 = !packet.ReadBit();
            var hasTime = !packet.ReadBit("Has timestamp");
            guid[4] = packet.ReadBit();
            guid[5] = packet.ReadBit();
            guid[2] = packet.ReadBit();
            var hasMovementFlags = !packet.ReadBit();
            var hasFallData = packet.ReadBit("Has fall data");
            var hasO = !packet.ReadBit();
            packet.ReadBit("Has Spline");
            packet.ReadBit();
            guid[3] = packet.ReadBit();
            guid[1] = packet.ReadBit();
            var hasSplineElev = !packet.ReadBit("Has Spline Elevation");
            var hasTrans = packet.ReadBit("Has transport");
            guid[7] = packet.ReadBit();
            guid[6] = packet.ReadBit();
            var hasPitch = !packet.ReadBit("Has pitch");

            if (hasTrans)
            {
                hasTransTime2 = packet.ReadBit();
                transportGuid[5] = packet.ReadBit();
                transportGuid[1] = packet.ReadBit();
                transportGuid[6] = packet.ReadBit();
                hasTransTime3 = packet.ReadBit();
                transportGuid[2] = packet.ReadBit();
                transportGuid[0] = packet.ReadBit();
                transportGuid[4] = packet.ReadBit();
                transportGuid[7] = packet.ReadBit();
                transportGuid[3] = packet.ReadBit();
            }

            if (hasMovementFlags)
                info.Flags = (uint)packet.ReadBitsE<MovementFlag>("Movement Flags", 30);

            if (hasMovementFlags2)
                info.FlagsExtra = (uint)packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 12);

            if (hasFallData)
                hasFallDirection = packet.ReadBit();

            packet.ParseBitStream(guid, 5, 6, 7, 2, 1, 3, 4, 0);

            if (hasFallData)
            {
                info.JumpVerticalSpeed = packet.ReadSingle("Vertical Speed");

                if (hasFallDirection)
                {
                    info.JumpSinAngle = packet.ReadSingle("Fall Sin");
                    info.JumpHorizontalSpeed = packet.ReadSingle("Horizontal Speed");
                    info.JumpCosAngle = packet.ReadSingle("Fall Cos");
                }

                info.FallTime = packet.ReadUInt32("Fall time");
            }

            if (hasTrans)
            {
                var tpos = new Vector4();

                tpos.Z = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 7);
                packet.ReadXORByte(transportGuid, 5);
                tpos.X = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 0);
                info.TransportTime = packet.ReadUInt32("Transport Time");

                if (hasTransTime3)
                    packet.ReadUInt32("Transport Time 3");

                tpos.Y = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 2);
                packet.ReadXORByte(transportGuid, 4);
                packet.ReadXORByte(transportGuid, 1);
                packet.ReadXORByte(transportGuid, 3);
                tpos.O = packet.ReadSingle();
                info.TransportSeat = packet.ReadSByte("Transport Seat");
                packet.ReadXORByte(transportGuid, 6);

                if (hasTransTime2)
                    packet.ReadUInt32("Transport Time 2");

                info.TransportGuid = packet.WriteGuid("Transport Guid", transportGuid);
                info.TransportOffset = packet.AddValue("Transport Position", tpos);
            }

            if (hasSplineElev)
                info.SplineElevation = packet.ReadSingle("Spline elevation");
            if (hasTime)
                info.MoveTime = packet.ReadUInt32("Timestamp");
            if (hasO)
                info.Orientation = pos.O = packet.ReadSingle();
            if (hasPitch)
                info.SwimPitch = packet.ReadSingle("Pitch");

            WowGuid moverGuid = packet.WriteGuid("Guid", guid);
            packet.AddValue("Position", pos);
            speedUpdate.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(packet.Time);
            Storage.StoreUnitSpeedUpdate(moverGuid, speedUpdate);
            Storage.StorePlayerMovement(moverGuid, info, packet);
        }

        [Parser(Opcode.CMSG_MOVE_FORCE_RUN_BACK_SPEED_CHANGE_ACK)]
        public static void HandleMoveForceRunBackSpeedChangeAck434(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();
            MovementInfo info = new MovementInfo();

            CreatureSpeedUpdate speedUpdate = new CreatureSpeedUpdate();
            speedUpdate.SpeedType = SpeedType.RunBack;
            speedUpdate.SpeedRate = packet.ReadSingle("Speed") / MovementInfo.DEFAULT_RUN_BACK_SPEED;
            packet.ReadInt32("Movement Counter");
            info.Position.X = pos.X = packet.ReadSingle();
            info.Position.Z = pos.Z = packet.ReadSingle();
            info.Position.Y = pos.Y = packet.ReadSingle();
            guid[2] = packet.ReadBit();
            guid[4] = packet.ReadBit();
            var hasTime = !packet.ReadBit("Has timestamp");
            var hasTrans = packet.ReadBit("Has transport");
            guid[1] = packet.ReadBit();
            guid[0] = packet.ReadBit();
            guid[5] = packet.ReadBit();
            guid[3] = packet.ReadBit();
            var hasO = !packet.ReadBit();
            var hasFallData = packet.ReadBit("Has fall data");
            var hasMovementFlags2 = !packet.ReadBit();
            var hasMovementFlags = !packet.ReadBit();
            guid[7] = packet.ReadBit();
            packet.ReadBit("Has Spline");
            var hasPitch = !packet.ReadBit("Has pitch");
            guid[6] = packet.ReadBit();
            var hasSplineElev = !packet.ReadBit("Has Spline Elevation");
            packet.ReadBit();

            if (hasTrans)
            {
                transportGuid[3] = packet.ReadBit();
                hasTransTime2 = packet.ReadBit();
                transportGuid[7] = packet.ReadBit();
                transportGuid[0] = packet.ReadBit();
                transportGuid[2] = packet.ReadBit();
                transportGuid[1] = packet.ReadBit();
                hasTransTime3 = packet.ReadBit();
                transportGuid[4] = packet.ReadBit();
                transportGuid[6] = packet.ReadBit();
                transportGuid[5] = packet.ReadBit();
            }

            if (hasMovementFlags)
                info.Flags = (uint)packet.ReadBitsE<MovementFlag>("Movement Flags", 30);

            if (hasMovementFlags2)
                info.FlagsExtra = (uint)packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 12);

            if (hasFallData)
                hasFallDirection = packet.ReadBit();

            packet.ParseBitStream(guid, 0, 7, 2, 4, 3, 6, 5, 1);

            if (hasFallData)
            {
                info.FallTime = packet.ReadUInt32("Fall time");

                if (hasFallDirection)
                {
                    info.JumpHorizontalSpeed = packet.ReadSingle("Horizontal Speed");
                    info.JumpCosAngle = packet.ReadSingle("Fall Cos");
                    info.JumpSinAngle = packet.ReadSingle("Fall Sin");
                }

                info.JumpVerticalSpeed = packet.ReadSingle("Vertical Speed");
            }

            if (hasTrans)
            {
                var tpos = new Vector4();

                packet.ReadXORByte(transportGuid, 5);
                info.TransportSeat = packet.ReadSByte("Transport Seat");
                packet.ReadXORByte(transportGuid, 6);
                packet.ReadXORByte(transportGuid, 1);
                packet.ReadXORByte(transportGuid, 0);
                packet.ReadXORByte(transportGuid, 2);
                tpos.Y = packet.ReadSingle();

                if (hasTransTime3)
                    packet.ReadUInt32("Transport Time 3");

                tpos.X = packet.ReadSingle();
                tpos.O = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 3);
                info.TransportTime = packet.ReadUInt32("Transport Time");

                if (hasTransTime2)
                    packet.ReadUInt32("Transport Time 2");

                packet.ReadXORByte(transportGuid, 4);
                tpos.Z = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 7);

                info.TransportGuid = packet.WriteGuid("Transport Guid", transportGuid);
                info.TransportOffset = packet.AddValue("Transport Position", tpos);
            }

            if (hasTime)
                info.MoveTime = packet.ReadUInt32("Timestamp");
            if (hasSplineElev)
                info.SplineElevation = packet.ReadSingle("Spline elevation");
            if (hasO)
                info.Orientation = pos.O = packet.ReadSingle();
            if (hasPitch)
                info.SwimPitch = packet.ReadSingle("Pitch");

            WowGuid moverGuid = packet.WriteGuid("Guid", guid);
            packet.AddValue("Position", pos);
            speedUpdate.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(packet.Time);
            Storage.StoreUnitSpeedUpdate(moverGuid, speedUpdate);
            Storage.StorePlayerMovement(moverGuid, info, packet);
        }

        [Parser(Opcode.SMSG_MOVE_UPDATE_RUN_BACK_SPEED)]
        public static void HandleMoveUpdateRunBackSpeed434(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasFallDirection = false;
            var pos = new Vector4();
            MovementInfo info = new MovementInfo();

            guid[1] = packet.ReadBit();
            guid[2] = packet.ReadBit();
            var hasSplineElevation = !packet.ReadBit();
            guid[4] = packet.ReadBit();
            guid[3] = packet.ReadBit();
            var hasFallData = packet.ReadBit();
            guid[6] = packet.ReadBit();
            var hasTime = !packet.ReadBit();
            guid[0] = packet.ReadBit();
            packet.ReadBit();
            var hasMovementFlags = !packet.ReadBit();
            var hasPitch = !packet.ReadBit();
            packet.ReadBit("Has spline data");
            guid[5] = packet.ReadBit();
            var hasMovementFlagsExtra = !packet.ReadBit();

            if (hasMovementFlagsExtra)
                info.FlagsExtra = (uint)packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 12);

            var hasO = !packet.ReadBit();

            if (hasMovementFlags)
                info.Flags = (uint)packet.ReadBitsE<MovementFlag>("Movement Flags", 30);

            if (hasFallData)
                hasFallDirection = packet.ReadBit();

            var hasTransport = packet.ReadBit();

            var hasTransportTime2 = false;
            var hasTransportTime3 = false;
            if (hasTransport)
            {
                transportGuid[5] = packet.ReadBit();
                hasTransportTime2 = packet.ReadBit();
                transportGuid[3] = packet.ReadBit();
                transportGuid[1] = packet.ReadBit();
                transportGuid[6] = packet.ReadBit();
                transportGuid[7] = packet.ReadBit();
                transportGuid[2] = packet.ReadBit();
                transportGuid[4] = packet.ReadBit();
                transportGuid[0] = packet.ReadBit();
                hasTransportTime3 = packet.ReadBit();
            }

            guid[7] = packet.ReadBit();

            if (hasTransport)
            {
                var tpos = new Vector4();

                tpos.X = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 2);
                packet.ReadXORByte(transportGuid, 5);
                packet.ReadXORByte(transportGuid, 4);
                packet.ReadXORByte(transportGuid, 6);

                if (hasTransportTime2)
                    packet.ReadUInt32("Transport Time 2");

                packet.ReadXORByte(transportGuid, 0);
                packet.ReadXORByte(transportGuid, 3);
                tpos.Y = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 7);

                if (hasTransportTime3)
                    packet.ReadUInt32("Transport Time 3");

                tpos.Z = packet.ReadSingle();
                info.TransportTime = packet.ReadUInt32("Transport Time");
                info.TransportSeat = packet.ReadSByte("Transport Seat");
                packet.ReadXORByte(transportGuid, 1);
                tpos.O = packet.ReadSingle();

                info.TransportGuid = packet.WriteGuid("Transport Guid", transportGuid);
                info.TransportOffset = packet.AddValue("Transport Position", tpos);
            }

            packet.ReadXORByte(guid, 4);

            if (hasFallData)
            {
                info.FallTime = packet.ReadUInt32("Fall time");

                if (hasFallDirection)
                {
                    info.JumpHorizontalSpeed = packet.ReadSingle("Horizontal Speed");
                    info.JumpCosAngle = packet.ReadSingle("Fall Cos");
                    info.JumpSinAngle = packet.ReadSingle("Fall Sin");
                }

                info.JumpVerticalSpeed = packet.ReadSingle("Vertical Speed");
            }

            if (hasTime)
                info.MoveTime = packet.ReadUInt32("Timestamp");

            if (hasSplineElevation)
                info.SplineElevation = packet.ReadSingle("Spline elevation");

            packet.ReadXORByte(guid, 1);

            if (hasO)
                info.Orientation = pos.O = packet.ReadSingle();

            packet.ReadXORByte(guid, 0);
            packet.ReadXORByte(guid, 5);
            packet.ReadXORByte(guid, 3);
            info.Position.X = pos.X = packet.ReadSingle();
            info.Position.Y = pos.Y = packet.ReadSingle();

            if (hasPitch)
                info.SwimPitch = packet.ReadSingle("Pitch");

            packet.ReadXORByte(guid, 7);
            CreatureSpeedUpdate speedUpdate = new CreatureSpeedUpdate();
            speedUpdate.SpeedType = SpeedType.RunBack;
            speedUpdate.SpeedRate = packet.ReadSingle("Speed") / MovementInfo.DEFAULT_RUN_BACK_SPEED;
            packet.ReadXORByte(guid, 2);
            packet.ReadXORByte(guid, 6);
            info.Position.Z = pos.Z = packet.ReadSingle();

            WowGuid moverGuid = packet.WriteGuid("Guid", guid);
            packet.AddValue("Position", pos);
            speedUpdate.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(packet.Time);
            Storage.StoreUnitSpeedUpdate(moverGuid, speedUpdate);
            Storage.StorePlayerMovement(moverGuid, info, packet);
        }

        [Parser(Opcode.SMSG_MOVE_UPDATE_WALK_SPEED)]
        public static void HandleMoveUpdateWalkSpeed434(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasFallDirection = false;
            var pos = new Vector4();
            MovementInfo info = new MovementInfo();

            var hasPitch = !packet.ReadBit();
            var hasO = !packet.ReadBit();
            packet.ReadBit();
            guid[3] = packet.ReadBit();
            var hasSplineElevation = !packet.ReadBit();
            guid[2] = packet.ReadBit();
            var hasTransport = packet.ReadBit();

            var hasTransportTime2 = false;
            var hasTransportTime3 = false;
            if (hasTransport)
            {
                transportGuid[6] = packet.ReadBit();
                transportGuid[3] = packet.ReadBit();
                transportGuid[2] = packet.ReadBit();
                transportGuid[0] = packet.ReadBit();
                transportGuid[4] = packet.ReadBit();
                hasTransportTime2 = packet.ReadBit();
                transportGuid[7] = packet.ReadBit();
                transportGuid[1] = packet.ReadBit();
                transportGuid[5] = packet.ReadBit();
                hasTransportTime3 = packet.ReadBit();
            }

            guid[7] = packet.ReadBit();
            guid[5] = packet.ReadBit();
            guid[1] = packet.ReadBit();
            var hasFallData = packet.ReadBit();
            guid[0] = packet.ReadBit();
            var hasMovementFlagsExtra = !packet.ReadBit();
            var hasTime = !packet.ReadBit();
            var hasMovementFlags = !packet.ReadBit();
            guid[6] = packet.ReadBit();

            if (hasFallData)
                hasFallDirection = packet.ReadBit();

            if (hasMovementFlagsExtra)
                info.FlagsExtra = (uint)packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 12);

            if (hasMovementFlags)
                info.Flags = (uint)packet.ReadBitsE<MovementFlag>("Movement Flags", 30);

            packet.ReadBit("Has spline data");
            guid[4] = packet.ReadBit();

            if (hasPitch)
                info.SwimPitch = packet.ReadSingle("Pitch");

            if (hasTransport)
            {
                var tpos = new Vector4();

                packet.ReadXORByte(transportGuid, 6);
                packet.ReadXORByte(transportGuid, 0);
                packet.ReadXORByte(transportGuid, 4);
                packet.ReadXORByte(transportGuid, 2);
                tpos.X = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 7);
                info.TransportTime = packet.ReadUInt32("Transport Time");

                if (hasTransportTime2)
                    packet.ReadUInt32("Transport Time 2");

                tpos.Z = packet.ReadSingle();
                info.TransportSeat = packet.ReadSByte("Transport Seat");
                packet.ReadXORByte(transportGuid, 5);

                if (hasTransportTime3)
                    packet.ReadUInt32("Transport Time 3");

                tpos.O = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 1);
                tpos.Y = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 3);

                info.TransportGuid = packet.WriteGuid("Transport Guid", transportGuid);
                info.TransportOffset = packet.AddValue("Transport Position", tpos);
            }

            if (hasSplineElevation)
                info.SplineElevation = packet.ReadSingle("Spline elevation");

            if (hasFallData)
            {
                info.JumpVerticalSpeed = packet.ReadSingle("Vertical Speed");

                if (hasFallDirection)
                {
                    info.JumpHorizontalSpeed = packet.ReadSingle("Horizontal Speed");
                    info.JumpCosAngle = packet.ReadSingle("Fall Cos");
                    info.JumpSinAngle = packet.ReadSingle("Fall Sin");
                }

                info.FallTime = packet.ReadUInt32("Fall time");
            }

            packet.ReadXORByte(guid, 1);
            packet.ReadXORByte(guid, 4);
            packet.ReadXORByte(guid, 2);
            packet.ReadXORByte(guid, 6);
            packet.ReadXORByte(guid, 7);

            if (hasTime)
                info.MoveTime = packet.ReadUInt32("Timestamp");

            if (hasO)
                info.Orientation = pos.O = packet.ReadSingle();

            info.Position.Y = pos.Y = packet.ReadSingle();
            packet.ReadXORByte(guid, 0);
            info.Position.Z = pos.Z = packet.ReadSingle();
            info.Position.X = pos.X = packet.ReadSingle();
            packet.ReadXORByte(guid, 3);
            packet.ReadXORByte(guid, 5);
            CreatureSpeedUpdate speedUpdate = new CreatureSpeedUpdate();
            speedUpdate.SpeedType = SpeedType.Walk;
            speedUpdate.SpeedRate = packet.ReadSingle("Speed") / MovementInfo.DEFAULT_WALK_SPEED;

            WowGuid moverGuid = packet.WriteGuid("Guid", guid);
            packet.AddValue("Position", pos);
            speedUpdate.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(packet.Time);
            Storage.StoreUnitSpeedUpdate(moverGuid, speedUpdate);
            Storage.StorePlayerMovement(moverGuid, info, packet);
        }

        [Parser(Opcode.CMSG_FORCE_MOVE_ROOT_ACK)]
        public static void HandleForceMoveRootAck434(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();
            MovementInfo info = new MovementInfo();

            info.Position.Y = pos.Y = packet.ReadSingle();
            info.Position.Z = pos.Z = packet.ReadSingle();
            packet.ReadInt32("Movement Counter");
            info.Position.X = pos.X = packet.ReadSingle();
            guid[3] = packet.ReadBit();
            var hasTime = !packet.ReadBit("Has timestamp");
            var hasTrans = packet.ReadBit("Has transport");
            var hasPitch = !packet.ReadBit("Has pitch");
            guid[2] = packet.ReadBit();
            var hasO = !packet.ReadBit();
            var hasSplineElev = !packet.ReadBit("Has Spline Elevation");
            var hasFallData = packet.ReadBit("Has fall data");
            packet.ReadBit("Has Spline");
            guid[4] = packet.ReadBit();
            guid[0] = packet.ReadBit();
            guid[6] = packet.ReadBit();
            guid[1] = packet.ReadBit();
            guid[7] = packet.ReadBit();
            var hasMovementFlags2 = !packet.ReadBit();
            var hasMovementFlags = !packet.ReadBit();
            packet.ReadBit();
            guid[5] = packet.ReadBit();

            if (hasTrans)
            {
                transportGuid[6] = packet.ReadBit();
                transportGuid[0] = packet.ReadBit();
                transportGuid[7] = packet.ReadBit();
                transportGuid[3] = packet.ReadBit();
                transportGuid[1] = packet.ReadBit();
                transportGuid[2] = packet.ReadBit();
                hasTransTime2 = packet.ReadBit();
                transportGuid[4] = packet.ReadBit();
                hasTransTime3 = packet.ReadBit();
                transportGuid[5] = packet.ReadBit();
            }

            if (hasMovementFlags2)
                info.FlagsExtra = (uint)packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 12);

            if (hasMovementFlags)
                info.Flags = (uint)packet.ReadBitsE<MovementFlag>("Movement Flags", 30);

            if (hasFallData)
                hasFallDirection = packet.ReadBit();

            packet.ParseBitStream(guid, 5, 3, 1, 7, 4, 0, 6, 2);

            if (hasTrans)
            {
                var tpos = new Vector4();

                packet.ReadXORByte(transportGuid, 5);
                packet.ReadXORByte(transportGuid, 4);
                tpos.O = packet.ReadSingle();
                tpos.Z = packet.ReadSingle();

                if (hasTransTime3)
                    packet.ReadUInt32("Transport Time 3");

                if (hasTransTime2)
                    packet.ReadUInt32("Transport Time 2");

                packet.ReadXORByte(transportGuid, 7);
                packet.ReadXORByte(transportGuid, 2);
                packet.ReadXORByte(transportGuid, 0);
                info.TransportTime = packet.ReadUInt32("Transport Time");
                packet.ReadXORByte(transportGuid, 6);
                packet.ReadXORByte(transportGuid, 3);
                tpos.X = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 1);
                tpos.Y = packet.ReadSingle();
                info.TransportSeat = packet.ReadSByte("Transport Seat");

                info.TransportGuid = packet.WriteGuid("Transport Guid", transportGuid);
                info.TransportOffset = packet.AddValue("Transport Position", tpos);
            }

            if (hasTime)
                info.MoveTime = packet.ReadUInt32("Timestamp");

            if (hasFallData)
            {
                info.JumpVerticalSpeed = packet.ReadSingle("Vertical Speed");

                if (hasFallDirection)
                {
                    info.JumpHorizontalSpeed = packet.ReadSingle("Horizontal Speed");
                    info.JumpSinAngle = packet.ReadSingle("Fall Sin");
                    info.JumpCosAngle = packet.ReadSingle("Fall Cos");
                }

                info.FallTime = packet.ReadUInt32("Fall time");
            }

            if (hasO)
                info.Orientation = pos.O = packet.ReadSingle();
            if (hasSplineElev)
                info.SplineElevation = packet.ReadSingle("Spline elevation");
            if (hasPitch)
                info.SwimPitch = packet.ReadSingle("Pitch");

            WowGuid moverGuid = packet.WriteGuid("Guid", guid);
            packet.AddValue("Position", pos);
            Storage.StorePlayerMovement(moverGuid, info, packet);
        }

        [Parser(Opcode.CMSG_FORCE_MOVE_UNROOT_ACK)]
        public static void HandleForceMoveUnrootAck434(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();
            MovementInfo info = new MovementInfo();

            packet.ReadInt32("Movement Counter");
            info.Position.Z = pos.Z = packet.ReadSingle();
            info.Position.Y = pos.Y = packet.ReadSingle();
            info.Position.X = pos.X = packet.ReadSingle();
            guid[7] = packet.ReadBit();
            var hasSplineElev = !packet.ReadBit("Has Spline Elevation");
            packet.ReadBit("Has Spline");
            guid[5] = packet.ReadBit();
            guid[4] = packet.ReadBit();
            packet.ReadBit();
            var hasTime = !packet.ReadBit("Has timestamp");
            var hasMovementFlags = !packet.ReadBit();
            var hasO = !packet.ReadBit();
            var hasPitch = !packet.ReadBit("Has pitch");
            guid[3] = packet.ReadBit();
            guid[1] = packet.ReadBit();
            var hasTrans = packet.ReadBit("Has transport");
            var hasMovementFlags2 = !packet.ReadBit();
            guid[2] = packet.ReadBit();
            guid[0] = packet.ReadBit();
            var hasFallData = packet.ReadBit("Has fall data");
            guid[6] = packet.ReadBit();

            if (hasFallData)
                hasFallDirection = packet.ReadBit();

            if (hasTrans)
            {
                transportGuid[0] = packet.ReadBit();
                hasTransTime3 = packet.ReadBit();
                transportGuid[6] = packet.ReadBit();
                hasTransTime2 = packet.ReadBit();
                transportGuid[3] = packet.ReadBit();
                transportGuid[5] = packet.ReadBit();
                transportGuid[7] = packet.ReadBit();
                transportGuid[2] = packet.ReadBit();
                transportGuid[4] = packet.ReadBit();
                transportGuid[1] = packet.ReadBit();
            }

            if (hasMovementFlags2)
                info.FlagsExtra = (uint)packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 12);

            if (hasMovementFlags)
                info.Flags = (uint)packet.ReadBitsE<MovementFlag>("Movement Flags", 30);

            packet.ParseBitStream(guid, 7, 1, 0, 6, 2, 4, 5, 3);

            if (hasTrans)
            {
                var tpos = new Vector4();

                packet.ReadXORByte(transportGuid, 6);
                tpos.Z = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 2);
                packet.ReadXORByte(transportGuid, 3);
                tpos.Y = packet.ReadSingle();

                if (hasTransTime3)
                    packet.ReadUInt32("Transport Time 3");

                packet.ReadXORByte(transportGuid, 5);
                info.TransportSeat = packet.ReadSByte("Transport Seat");
                packet.ReadXORByte(transportGuid, 0);
                info.TransportTime = packet.ReadUInt32("Transport Time");

                if (hasTransTime2)
                    packet.ReadUInt32("Transport Time 2");

                packet.ReadXORByte(transportGuid, 4);
                tpos.O = packet.ReadSingle();
                tpos.X = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 1);
                packet.ReadXORByte(transportGuid, 7);

                info.TransportGuid = packet.WriteGuid("Transport Guid", transportGuid);
                info.TransportOffset = packet.AddValue("Transport Position", tpos);
            }

            if (hasFallData)
            {
                if (hasFallDirection)
                {
                    info.JumpCosAngle = packet.ReadSingle("Fall Cos");
                    info.JumpHorizontalSpeed = packet.ReadSingle("Horizontal Speed");
                    info.JumpSinAngle = packet.ReadSingle("Fall Sin");
                }

                info.JumpVerticalSpeed = packet.ReadSingle("Vertical Speed");
                info.FallTime = packet.ReadUInt32("Fall time");
            }

            if (hasTime)
                info.MoveTime = packet.ReadUInt32("Timestamp");
            if (hasSplineElev)
                info.SplineElevation = packet.ReadSingle("Spline elevation");
            if (hasO)
                info.Orientation = pos.O = packet.ReadSingle();
            if (hasPitch)
                info.SwimPitch = packet.ReadSingle("Pitch");

            WowGuid moverGuid = packet.WriteGuid("Guid", guid);
            packet.AddValue("Position", pos);
            Storage.StorePlayerMovement(moverGuid, info, packet);
        }

        [Parser(Opcode.CMSG_MOVE_FALL_RESET)]
        public static void HandleMoveFallReset434(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();
            MovementInfo info = new MovementInfo();

            info.Position.Z = pos.Z = packet.ReadSingle();
            info.Position.X = pos.X = packet.ReadSingle();
            info.Position.Y = pos.Y = packet.ReadSingle();
            guid[1] = packet.ReadBit();
            var hasTime = !packet.ReadBit("Has timestamp");
            var hasMovementFlags = !packet.ReadBit();
            packet.ReadBit();
            guid[6] = packet.ReadBit();
            var hasSplineElev = !packet.ReadBit("Has Spline Elevation");
            var hasPitch = !packet.ReadBit("Has pitch");
            guid[3] = packet.ReadBit();
            var hasTrans = packet.ReadBit("Has transport");
            guid[2] = packet.ReadBit();
            var hasMovementFlags2 = !packet.ReadBit();
            var hasO = !packet.ReadBit();
            guid[4] = packet.ReadBit();
            guid[5] = packet.ReadBit();
            packet.ReadBit("Has Spline");
            guid[7] = packet.ReadBit();
            guid[0] = packet.ReadBit();
            var hasFallData = packet.ReadBit("Has fall data");

            if (hasTrans)
            {
                transportGuid[0] = packet.ReadBit();
                transportGuid[1] = packet.ReadBit();
                transportGuid[5] = packet.ReadBit();
                transportGuid[4] = packet.ReadBit();
                hasTransTime2 = packet.ReadBit();
                transportGuid[2] = packet.ReadBit();
                hasTransTime3 = packet.ReadBit();
                transportGuid[6] = packet.ReadBit();
                transportGuid[7] = packet.ReadBit();
                transportGuid[3] = packet.ReadBit();
            }

            if (hasFallData)
                hasFallDirection = packet.ReadBit();

            if (hasMovementFlags2)
                info.FlagsExtra = (uint)packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 12);

            if (hasMovementFlags)
                info.Flags = (uint)packet.ReadBitsE<MovementFlag>("Movement Flags", 30);

            packet.ParseBitStream(guid, 4, 0, 1, 7, 5, 2, 3, 6);

            if (hasTrans)
            {
                var tpos = new Vector4();

                tpos.X = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 7);
                packet.ReadXORByte(transportGuid, 2);
                packet.ReadXORByte(transportGuid, 3);
                packet.ReadXORByte(transportGuid, 5);
                tpos.Y = packet.ReadSingle();
                tpos.Z = packet.ReadSingle();
                info.TransportSeat = packet.ReadSByte("Transport Seat");

                if (hasTransTime2)
                    packet.ReadUInt32("Transport Time 2");

                packet.ReadXORByte(transportGuid, 6);

                if (hasTransTime3)
                    packet.ReadUInt32("Transport Time 3");

                info.TransportTime = packet.ReadUInt32("Transport Time");
                packet.ReadXORByte(transportGuid, 0);
                tpos.O = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 4);
                packet.ReadXORByte(transportGuid, 1);

                info.TransportGuid = packet.WriteGuid("Transport Guid", transportGuid);
                info.TransportOffset = packet.AddValue("Transport Position", tpos);
            }

            if (hasFallData)
            {
                info.JumpVerticalSpeed = packet.ReadSingle("Vertical Speed");

                if (hasFallDirection)
                {
                    info.JumpCosAngle = packet.ReadSingle("Fall Cos");
                    info.JumpSinAngle = packet.ReadSingle("Fall Sin");
                    info.JumpHorizontalSpeed = packet.ReadSingle("Horizontal Speed");
                }

                info.FallTime = packet.ReadUInt32("Fall time");
            }

            if (hasO)
                info.Orientation = pos.O = packet.ReadSingle();
            if (hasTime)
                info.MoveTime = packet.ReadUInt32("Timestamp");
            if (hasSplineElev)
                info.SplineElevation = packet.ReadSingle("Spline elevation");
            if (hasPitch)
                info.SwimPitch = packet.ReadSingle("Pitch");

            WowGuid moverGuid = packet.WriteGuid("Guid", guid);
            packet.AddValue("Position", pos);
            Storage.StorePlayerMovement(moverGuid, info, packet);
        }

        [Parser(Opcode.CMSG_MOVE_FEATHER_FALL_ACK)]
        public static void HandleMoveFeatherFallAck434(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();
            MovementInfo info = new MovementInfo();

            info.Position.Z = pos.Z = packet.ReadSingle();
            packet.ReadInt32("Movement Counter");
            info.Position.Y = pos.Y = packet.ReadSingle();
            info.Position.X = pos.X = packet.ReadSingle();
            packet.ReadBit();
            guid[3] = packet.ReadBit();
            var hasSplineElev = !packet.ReadBit("Has Spline Elevation");
            guid[1] = packet.ReadBit();
            var hasPitch = !packet.ReadBit("Has pitch");
            guid[2] = packet.ReadBit();
            packet.ReadBit("Has Spline");
            var hasTime = !packet.ReadBit("Has timestamp");
            guid[5] = packet.ReadBit();
            var hasFallData = packet.ReadBit("Has fall data");
            var hasO = !packet.ReadBit();
            var hasTrans = packet.ReadBit("Has transport");
            guid[7] = packet.ReadBit();
            var hasMovementFlags2 = !packet.ReadBit();
            guid[6] = packet.ReadBit();
            var hasMovementFlags = !packet.ReadBit();
            guid[0] = packet.ReadBit();
            guid[4] = packet.ReadBit();

            if (hasTrans)
            {
                transportGuid[6] = packet.ReadBit();
                hasTransTime3 = packet.ReadBit();
                transportGuid[2] = packet.ReadBit();
                transportGuid[4] = packet.ReadBit();
                transportGuid[7] = packet.ReadBit();
                transportGuid[3] = packet.ReadBit();
                transportGuid[5] = packet.ReadBit();
                hasTransTime2 = packet.ReadBit();
                transportGuid[1] = packet.ReadBit();
                transportGuid[0] = packet.ReadBit();
            }

            if (hasFallData)
                hasFallDirection = packet.ReadBit();

            if (hasMovementFlags2)
                info.FlagsExtra = (uint)packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 12);

            if (hasMovementFlags)
                info.Flags = (uint)packet.ReadBitsE<MovementFlag>("Movement Flags", 30);

            packet.ParseBitStream(guid, 6, 1, 7, 0, 5, 4, 3, 2);

            if (hasTrans)
            {
                var tpos = new Vector4();

                if (hasTransTime3)
                    packet.ReadUInt32("Transport Time 3");

                tpos.O = packet.ReadSingle();
                tpos.Z = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 7);
                packet.ReadXORByte(transportGuid, 5);
                packet.ReadXORByte(transportGuid, 2);
                tpos.X = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 1);
                packet.ReadXORByte(transportGuid, 6);

                if (hasTransTime2)
                    packet.ReadUInt32("Transport Time 2");

                info.TransportTime = packet.ReadUInt32("Transport Time");
                tpos.Y = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 4);
                packet.ReadXORByte(transportGuid, 0);
                info.TransportSeat = packet.ReadSByte("Transport Seat");
                packet.ReadXORByte(transportGuid, 3);

                info.TransportGuid = packet.WriteGuid("Transport Guid", transportGuid);
                info.TransportOffset = packet.AddValue("Transport Position", tpos);
            }

            if (hasTime)
                info.MoveTime = packet.ReadUInt32("Timestamp");
            if (hasSplineElev)
                info.SplineElevation = packet.ReadSingle("Spline elevation");
            if (hasO)
                info.Orientation = pos.O = packet.ReadSingle();

            if (hasFallData)
            {
                info.JumpVerticalSpeed = packet.ReadSingle("Vertical Speed");

                if (hasFallDirection)
                {
                    info.JumpCosAngle = packet.ReadSingle("Fall Cos");
                    info.JumpHorizontalSpeed = packet.ReadSingle("Horizontal Speed");
                    info.JumpSinAngle = packet.ReadSingle("Fall Sin");
                }

                info.FallTime = packet.ReadUInt32("Fall time");
            }

            if (hasPitch)
                info.SwimPitch = packet.ReadSingle("Pitch");

            WowGuid moverGuid = packet.WriteGuid("Guid", guid);
            packet.AddValue("Position", pos);
            Storage.StorePlayerMovement(moverGuid, info, packet);
        }

        [Parser(Opcode.CMSG_MOVE_GRAVITY_DISABLE_ACK)]
        public static void HandleMoveGravityDisableAck434(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();
            MovementInfo info = new MovementInfo();

            info.Position.Z = pos.Z = packet.ReadSingle();
            info.Position.Y = pos.Y = packet.ReadSingle();
            packet.ReadInt32("Movement Counter");
            info.Position.X = pos.X = packet.ReadSingle();
            guid[3] = packet.ReadBit();
            var hasTrans = packet.ReadBit("Has transport");
            guid[4] = packet.ReadBit();
            guid[5] = packet.ReadBit();
            var hasMovementFlags = !packet.ReadBit();
            var hasO = !packet.ReadBit();
            packet.ReadBit("Has Spline");
            guid[6] = packet.ReadBit();
            guid[0] = packet.ReadBit();
            guid[7] = packet.ReadBit();
            var hasSplineElev = !packet.ReadBit("Has Spline Elevation");
            var hasTime = !packet.ReadBit("Has timestamp");
            var hasPitch = !packet.ReadBit("Has pitch");
            var hasMovementFlags2 = !packet.ReadBit();
            packet.ReadBit();
            guid[2] = packet.ReadBit();
            var hasFallData = packet.ReadBit("Has fall data");
            guid[1] = packet.ReadBit();

            if (hasTrans)
            {
                transportGuid[2] = packet.ReadBit();
                hasTransTime3 = packet.ReadBit();
                transportGuid[0] = packet.ReadBit();
                transportGuid[1] = packet.ReadBit();
                transportGuid[5] = packet.ReadBit();
                transportGuid[3] = packet.ReadBit();
                transportGuid[7] = packet.ReadBit();
                transportGuid[6] = packet.ReadBit();
                transportGuid[4] = packet.ReadBit();
                hasTransTime2 = packet.ReadBit();
            }

            if (hasMovementFlags)
                info.Flags = (uint)packet.ReadBitsE<MovementFlag>("Movement Flags", 30);

            if (hasMovementFlags2)
                info.FlagsExtra = (uint)packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 12);

            if (hasFallData)
                hasFallDirection = packet.ReadBit();

            packet.ParseBitStream(guid, 0, 2, 1, 3, 5, 7, 4, 6);

            if (hasTrans)
            {
                var tpos = new Vector4();

                tpos.Z = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 2);
                packet.ReadXORByte(transportGuid, 5);
                packet.ReadXORByte(transportGuid, 3);
                packet.ReadXORByte(transportGuid, 4);
                tpos.Y = packet.ReadSingle();
                info.TransportTime = packet.ReadUInt32("Transport Time");
                info.TransportSeat = packet.ReadSByte("Transport Seat");
                packet.ReadXORByte(transportGuid, 6);
                packet.ReadXORByte(transportGuid, 1);
                packet.ReadXORByte(transportGuid, 0);
                tpos.O = packet.ReadSingle();

                if (hasTransTime2)
                    packet.ReadUInt32("Transport Time 2");

                tpos.X = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 7);

                if (hasTransTime3)
                    packet.ReadUInt32("Transport Time 3");

                info.TransportGuid = packet.WriteGuid("Transport Guid", transportGuid);
                info.TransportOffset = packet.AddValue("Transport Position", tpos);
            }

            if (hasFallData)
            {
                if (hasFallDirection)
                {
                    info.JumpCosAngle = packet.ReadSingle("Fall Cos");
                    info.JumpSinAngle = packet.ReadSingle("Fall Sin");
                    info.JumpHorizontalSpeed = packet.ReadSingle("Horizontal Speed");
                }

                info.FallTime = packet.ReadUInt32("Fall time");
                info.JumpVerticalSpeed = packet.ReadSingle("Vertical Speed");
            }

            if (hasO)
                info.Orientation = pos.O = packet.ReadSingle();
            if (hasSplineElev)
                info.SplineElevation = packet.ReadSingle("Spline elevation");
            if (hasTime)
                info.MoveTime = packet.ReadUInt32("Timestamp");
            if (hasPitch)
                info.SwimPitch = packet.ReadSingle("Pitch");

            WowGuid moverGuid = packet.WriteGuid("Guid", guid);
            packet.AddValue("Position", pos);
            Storage.StorePlayerMovement(moverGuid, info, packet);
        }

        [Parser(Opcode.CMSG_MOVE_GRAVITY_ENABLE_ACK)]
        public static void HandleMoveGravityEnableAck434(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();
            MovementInfo info = new MovementInfo();

            info.Position.Z = pos.Z = packet.ReadSingle();
            info.Position.X = pos.X = packet.ReadSingle();
            packet.ReadInt32("Movement Counter");
            info.Position.Y = pos.Y = packet.ReadSingle();
            var hasSplineElev = !packet.ReadBit("Has Spline Elevation");
            var hasMovementFlags2 = !packet.ReadBit();
            guid[6] = packet.ReadBit();
            var hasO = !packet.ReadBit();
            packet.ReadBit();
            guid[5] = packet.ReadBit();
            packet.ReadBit("Has Spline");
            guid[3] = packet.ReadBit();
            var hasTrans = packet.ReadBit("Has transport");
            var hasPitch = !packet.ReadBit("Has pitch");
            guid[1] = packet.ReadBit();
            var hasTime = !packet.ReadBit("Has timestamp");
            guid[2] = packet.ReadBit();
            guid[4] = packet.ReadBit();
            guid[7] = packet.ReadBit();
            var hasMovementFlags = !packet.ReadBit();
            var hasFallData = packet.ReadBit("Has fall data");
            guid[0] = packet.ReadBit();

            if (hasTrans)
            {
                hasTransTime3 = packet.ReadBit();
                transportGuid[0] = packet.ReadBit();
                transportGuid[1] = packet.ReadBit();
                transportGuid[3] = packet.ReadBit();
                transportGuid[7] = packet.ReadBit();
                transportGuid[2] = packet.ReadBit();
                transportGuid[5] = packet.ReadBit();
                hasTransTime2 = packet.ReadBit();
                transportGuid[4] = packet.ReadBit();
                transportGuid[6] = packet.ReadBit();
            }

            if (hasFallData)
                hasFallDirection = packet.ReadBit();

            if (hasMovementFlags)
                info.Flags = (uint)packet.ReadBitsE<MovementFlag>("Movement Flags", 30);

            if (hasMovementFlags2)
                info.FlagsExtra = (uint)packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 12);

            packet.ParseBitStream(guid, 5, 4, 1, 7, 0, 2, 3, 6);

            if (hasFallData)
            {
                info.FallTime = packet.ReadUInt32("Fall time");

                if (hasFallDirection)
                {
                    info.JumpHorizontalSpeed = packet.ReadSingle("Horizontal Speed");
                    info.JumpSinAngle = packet.ReadSingle("Fall Sin");
                    info.JumpCosAngle = packet.ReadSingle("Fall Cos");
                }

                info.JumpVerticalSpeed = packet.ReadSingle("Vertical Speed");
            }

            if (hasTrans)
            {
                var tpos = new Vector4();

                packet.ReadXORByte(transportGuid, 1);
                tpos.X = packet.ReadSingle();

                if (hasTransTime3)
                    packet.ReadUInt32("Transport Time 3");

                tpos.O = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 2);
                packet.ReadXORByte(transportGuid, 0);
                packet.ReadXORByte(transportGuid, 7);
                packet.ReadXORByte(transportGuid, 6);
                info.TransportSeat = packet.ReadSByte("Transport Seat");
                packet.ReadXORByte(transportGuid, 3);
                packet.ReadXORByte(transportGuid, 4);
                tpos.Z = packet.ReadSingle();

                if (hasTransTime2)
                    packet.ReadUInt32("Transport Time 2");

                packet.ReadXORByte(transportGuid, 5);
                info.TransportTime = packet.ReadUInt32("Transport Time");
                tpos.Y = packet.ReadSingle();

                info.TransportGuid = packet.WriteGuid("Transport Guid", transportGuid);
                info.TransportOffset = packet.AddValue("Transport Position", tpos);
            }

            if (hasSplineElev)
                info.SplineElevation = packet.ReadSingle("Spline elevation");
            if (hasO)
                info.Orientation = pos.O = packet.ReadSingle();
            if (hasPitch)
                info.SwimPitch = packet.ReadSingle("Pitch");
            if (hasTime)
                info.MoveTime = packet.ReadUInt32("Timestamp");

            WowGuid moverGuid = packet.WriteGuid("Guid", guid);
            packet.AddValue("Position", pos);
            Storage.StorePlayerMovement(moverGuid, info, packet);
        }

        [Parser(Opcode.CMSG_MOVE_HOVER_ACK)]
        public static void HandleMoveHoverAck434(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();
            MovementInfo info = new MovementInfo();

            packet.ReadInt32("Movement Counter");
            info.Position.Z = pos.Z = packet.ReadSingle();
            info.Position.Y = pos.Y = packet.ReadSingle();
            info.Position.X = pos.X = packet.ReadSingle();
            guid[4] = packet.ReadBit();
            var hasTrans = packet.ReadBit("Has transport");
            guid[2] = packet.ReadBit();
            var hasTime = !packet.ReadBit("Has timestamp");
            packet.ReadBit("Has Spline");
            var hasMovementFlags = !packet.ReadBit();
            guid[1] = packet.ReadBit();
            var hasPitch = !packet.ReadBit("Has pitch");
            guid[0] = packet.ReadBit();
            guid[6] = packet.ReadBit();
            guid[7] = packet.ReadBit();
            guid[5] = packet.ReadBit();
            packet.ReadBit();
            var hasFallData = packet.ReadBit("Has fall data");
            var hasMovementFlags2 = !packet.ReadBit();
            var hasSplineElev = !packet.ReadBit("Has Spline Elevation");
            var hasO = !packet.ReadBit();
            guid[3] = packet.ReadBit();

            if (hasTrans)
            {
                transportGuid[4] = packet.ReadBit();
                transportGuid[6] = packet.ReadBit();
                hasTransTime3 = packet.ReadBit();
                transportGuid[1] = packet.ReadBit();
                transportGuid[0] = packet.ReadBit();
                transportGuid[3] = packet.ReadBit();
                transportGuid[2] = packet.ReadBit();
                transportGuid[5] = packet.ReadBit();
                transportGuid[7] = packet.ReadBit();
                hasTransTime2 = packet.ReadBit();
            }

            if (hasMovementFlags)
                info.Flags = (uint)packet.ReadBitsE<MovementFlag>("Movement Flags", 30);

            if (hasMovementFlags2)
                info.FlagsExtra = (uint)packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 12);

            if (hasFallData)
                hasFallDirection = packet.ReadBit();

            packet.ParseBitStream(guid, 1, 4, 7, 2, 5, 6, 3, 0);

            if (hasTime)
                info.MoveTime = packet.ReadUInt32("Timestamp");

            if (hasFallData)
            {
                if (hasFallDirection)
                {
                    info.JumpCosAngle = packet.ReadSingle("Fall Cos");
                    info.JumpSinAngle = packet.ReadSingle("Fall Sin");
                    info.JumpHorizontalSpeed = packet.ReadSingle("Horizontal Speed");
                }

                info.FallTime = packet.ReadUInt32("Fall time");
                info.JumpVerticalSpeed = packet.ReadSingle("Vertical Speed");
            }

            if (hasO)
                info.Orientation = pos.O = packet.ReadSingle();

            if (hasTrans)
            {
                var tpos = new Vector4();

                packet.ReadXORByte(transportGuid, 1);

                if (hasTransTime3)
                    packet.ReadUInt32("Transport Time 3");

                packet.ReadXORByte(transportGuid, 2);
                packet.ReadXORByte(transportGuid, 7);
                packet.ReadXORByte(transportGuid, 3);
                packet.ReadXORByte(transportGuid, 4);
                info.TransportSeat = packet.ReadSByte("Transport Seat");
                tpos.X = packet.ReadSingle();
                tpos.Z = packet.ReadSingle();
                tpos.O = packet.ReadSingle();
                info.TransportTime = packet.ReadUInt32("Transport Time");
                packet.ReadXORByte(transportGuid, 0);
                tpos.Y = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 5);

                if (hasTransTime2)
                    packet.ReadUInt32("Transport Time 2");

                packet.ReadXORByte(transportGuid, 6);

                info.TransportGuid = packet.WriteGuid("Transport Guid", transportGuid);
                info.TransportOffset = packet.AddValue("Transport Position", tpos);
            }

            if (hasSplineElev)
                info.SplineElevation = packet.ReadSingle("Spline elevation");
            if (hasPitch)
                info.SwimPitch = packet.ReadSingle("Pitch");

            WowGuid moverGuid = packet.WriteGuid("Guid", guid);
            packet.AddValue("Position", pos);
            Storage.StorePlayerMovement(moverGuid, info, packet);
        }

        [Parser(Opcode.CMSG_MOVE_KNOCK_BACK_ACK)]
        public static void HandleMoveKnockBackAck434(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();
            MovementInfo info = new MovementInfo();

            info.Position.Y = pos.Y = packet.ReadSingle();
            info.Position.Z = pos.Z = packet.ReadSingle();
            packet.ReadInt32("Movement Counter");
            info.Position.X = pos.X = packet.ReadSingle();
            guid[6] = packet.ReadBit();
            var hasO = !packet.ReadBit();
            var hasPitch = !packet.ReadBit("Has pitch");
            var hasSplineElev = !packet.ReadBit("Has Spline Elevation");
            guid[3] = packet.ReadBit();
            guid[4] = packet.ReadBit();
            guid[1] = packet.ReadBit();
            guid[2] = packet.ReadBit();
            packet.ReadBit("Has Spline");
            guid[7] = packet.ReadBit();
            packet.ReadBit();
            var hasMovementFlags2 = !packet.ReadBit();
            var hasTime = !packet.ReadBit("Has timestamp");
            guid[0] = packet.ReadBit();
            var hasMovementFlags = !packet.ReadBit();
            var hasTrans = packet.ReadBit("Has transport");
            guid[5] = packet.ReadBit();
            var hasFallData = packet.ReadBit("Has fall data");

            if (hasMovementFlags)
                info.Flags = (uint)packet.ReadBitsE<MovementFlag>("Movement Flags", 30);

            if (hasTrans)
            {
                transportGuid[4] = packet.ReadBit();
                transportGuid[1] = packet.ReadBit();
                hasTransTime2 = packet.ReadBit();
                transportGuid[0] = packet.ReadBit();
                hasTransTime3 = packet.ReadBit();
                transportGuid[6] = packet.ReadBit();
                transportGuid[2] = packet.ReadBit();
                transportGuid[3] = packet.ReadBit();
                transportGuid[5] = packet.ReadBit();
                transportGuid[7] = packet.ReadBit();
            }

            if (hasMovementFlags2)
                info.FlagsExtra = (uint)packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 12);

            if (hasFallData)
                hasFallDirection = packet.ReadBit();

            packet.ParseBitStream(guid, 4, 5, 1, 6, 0, 3, 2, 7);

            if (hasFallData)
            {
                if (hasFallDirection)
                {
                    info.JumpCosAngle = packet.ReadSingle("Fall Cos");
                    info.JumpHorizontalSpeed = packet.ReadSingle("Horizontal Speed");
                    info.JumpSinAngle = packet.ReadSingle("Fall Sin");
                }

                info.FallTime = packet.ReadUInt32("Fall time");
                info.JumpVerticalSpeed = packet.ReadSingle("Vertical Speed");
            }

            if (hasTime)
                info.MoveTime = packet.ReadUInt32("Timestamp");

            if (hasTrans)
            {
                var tpos = new Vector4();

                tpos.Y = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 4);

                if (hasTransTime2)
                    packet.ReadUInt32("Transport Time 2");

                packet.ReadXORByte(transportGuid, 7);
                tpos.O = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 6);
                info.TransportTime = packet.ReadUInt32("Transport Time");
                packet.ReadXORByte(transportGuid, 3);
                packet.ReadXORByte(transportGuid, 1);

                if (hasTransTime3)
                    packet.ReadUInt32("Transport Time 3");

                packet.ReadXORByte(transportGuid, 2);
                tpos.Z = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 0);
                packet.ReadXORByte(transportGuid, 5);
                tpos.X = packet.ReadSingle();
                info.TransportSeat = packet.ReadSByte("Transport Seat");

                info.TransportGuid = packet.WriteGuid("Transport Guid", transportGuid);
                info.TransportOffset = packet.AddValue("Transport Position", tpos);
            }

            if (hasPitch)
                info.SwimPitch = packet.ReadSingle("Pitch");
            if (hasSplineElev)
                info.SplineElevation = packet.ReadSingle("Spline elevation");
            if (hasO)
                info.Orientation = pos.O = packet.ReadSingle();

            WowGuid moverGuid = packet.WriteGuid("Guid", guid);
            packet.AddValue("Position", pos);
            Storage.StorePlayerMovement(moverGuid, info, packet);
        }

        [Parser(Opcode.CMSG_MOVE_NOT_ACTIVE_MOVER)]
        public static void HandleMoveNotActiveMover434(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();
            MovementInfo info = new MovementInfo();

            info.Position.Z = pos.Z = packet.ReadSingle();
            info.Position.X = pos.X = packet.ReadSingle();
            info.Position.Y = pos.Y = packet.ReadSingle();
            var hasMovementFlags2 = !packet.ReadBit();
            var hasTrans = packet.ReadBit("Has transport");
            guid[6] = packet.ReadBit();
            var hasSplineElev = !packet.ReadBit("Has Spline Elevation");
            guid[3] = packet.ReadBit();
            packet.ReadBit();
            var hasTime = !packet.ReadBit("Has timestamp");
            guid[0] = packet.ReadBit();
            var hasO = !packet.ReadBit();
            guid[5] = packet.ReadBit();
            var hasPitch = !packet.ReadBit("Has pitch");
            guid[1] = packet.ReadBit();
            guid[4] = packet.ReadBit();
            guid[7] = packet.ReadBit();
            packet.ReadBit("Has Spline");
            guid[2] = packet.ReadBit();
            var hasFallData = packet.ReadBit("Has fall data");
            var hasMovementFlags = !packet.ReadBit();

            if (hasTrans)
            {
                transportGuid[4] = packet.ReadBit();
                transportGuid[0] = packet.ReadBit();
                transportGuid[1] = packet.ReadBit();
                transportGuid[6] = packet.ReadBit();
                transportGuid[2] = packet.ReadBit();
                hasTransTime3 = packet.ReadBit();
                transportGuid[5] = packet.ReadBit();
                transportGuid[7] = packet.ReadBit();
                hasTransTime2 = packet.ReadBit();
                transportGuid[3] = packet.ReadBit();
            }

            if (hasFallData)
                hasFallDirection = packet.ReadBit();

            if (hasMovementFlags)
                info.Flags = (uint)packet.ReadBitsE<MovementFlag>("Movement Flags", 30);

            if (hasMovementFlags2)
                info.FlagsExtra = (uint)packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 12);

            packet.ParseBitStream(guid, 1, 0, 4, 2, 7, 5, 6, 3);

            if (hasFallData)
            {
                info.JumpVerticalSpeed = packet.ReadSingle("Vertical Speed");

                if (hasFallDirection)
                {
                    info.JumpCosAngle = packet.ReadSingle("Fall Cos");
                    info.JumpSinAngle = packet.ReadSingle("Fall Sin");
                    info.JumpHorizontalSpeed = packet.ReadSingle("Horizontal Speed");
                }

                info.FallTime = packet.ReadUInt32("Fall time");
            }

            if (hasTrans)
            {
                var tpos = new Vector4();

                if (hasTransTime3)
                    packet.ReadUInt32("Transport Time 3");

                packet.ReadXORByte(transportGuid, 1);

                if (hasTransTime2)
                    packet.ReadUInt32("Transport Time 2");

                tpos.O = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 0);
                info.TransportSeat = packet.ReadSByte("Transport Seat");
                packet.ReadXORByte(transportGuid, 4);
                packet.ReadXORByte(transportGuid, 6);
                packet.ReadXORByte(transportGuid, 3);
                packet.ReadXORByte(transportGuid, 5);
                tpos.Y = packet.ReadSingle();
                tpos.X = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 2);
                tpos.Z = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 7);
                info.TransportTime = packet.ReadUInt32("Transport Time");

                info.TransportGuid = packet.WriteGuid("Transport Guid", transportGuid);
                info.TransportOffset = packet.AddValue("Transport Position", tpos);
            }

            if (hasTime)
                info.MoveTime = packet.ReadUInt32("Timestamp");
            if (hasSplineElev)
                info.SplineElevation = packet.ReadSingle("Spline elevation");
            if (hasPitch)
                info.SwimPitch = packet.ReadSingle("Pitch");
            if (hasO)
                info.Orientation = pos.O = packet.ReadSingle();

            WowGuid moverGuid = packet.WriteGuid("Guid", guid);
            packet.AddValue("Position", pos);
            Storage.StorePlayerMovement(moverGuid, info, packet);
        }

        [Parser(Opcode.CMSG_MOVE_WATER_WALK_ACK)]
        public static void HandleMoveWaterWalkAck434(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasTransTime2 = false;
            var hasTransTime3 = false;
            var hasFallDirection = false;
            var pos = new Vector4();
            MovementInfo info = new MovementInfo();

            info.Position.Y = pos.Y = packet.ReadSingle();
            info.Position.Z = pos.Z = packet.ReadSingle();
            packet.ReadInt32("Movement Counter");
            info.Position.X = pos.X = packet.ReadSingle();
            var hasTime = !packet.ReadBit("Has timestamp");
            var hasPitch = !packet.ReadBit("Has pitch");
            guid[5] = packet.ReadBit();
            guid[0] = packet.ReadBit();
            guid[7] = packet.ReadBit();
            var hasO = !packet.ReadBit();
            guid[1] = packet.ReadBit();
            var hasMovementFlags2 = !packet.ReadBit();
            guid[2] = packet.ReadBit();
            var hasMovementFlags = !packet.ReadBit();
            guid[3] = packet.ReadBit();
            var hasTrans = packet.ReadBit("Has transport");
            guid[6] = packet.ReadBit();
            var hasFallData = packet.ReadBit("Has fall data");
            guid[4] = packet.ReadBit();
            packet.ReadBit();
            var hasSplineElev = !packet.ReadBit("Has Spline Elevation");
            packet.ReadBit("Has Spline");

            if (hasTrans)
            {
                transportGuid[0] = packet.ReadBit();
                transportGuid[1] = packet.ReadBit();
                transportGuid[6] = packet.ReadBit();
                transportGuid[2] = packet.ReadBit();
                transportGuid[7] = packet.ReadBit();
                transportGuid[5] = packet.ReadBit();
                hasTransTime2 = packet.ReadBit();
                transportGuid[3] = packet.ReadBit();
                hasTransTime3 = packet.ReadBit();
                transportGuid[4] = packet.ReadBit();
            }

            if (hasMovementFlags)
                info.Flags = (uint)packet.ReadBitsE<MovementFlag>("Movement Flags", 30);

            if (hasFallData)
                hasFallDirection = packet.ReadBit();

            if (hasMovementFlags2)
                info.FlagsExtra = (uint)packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 12);

            packet.ParseBitStream(guid, 2, 7, 3, 5, 6, 0, 4, 1);

            if (hasTrans)
            {
                var tpos = new Vector4();

                tpos.X = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 1);

                if (hasTransTime3)
                    packet.ReadUInt32("Transport Time 3");

                packet.ReadXORByte(transportGuid, 0);
                tpos.O = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 7);
                tpos.Y = packet.ReadSingle();

                if (hasTransTime2)
                    packet.ReadUInt32("Transport Time 2");

                info.TransportTime = packet.ReadUInt32("Transport Time");
                packet.ReadXORByte(transportGuid, 5);
                info.TransportSeat = packet.ReadSByte("Transport Seat");
                tpos.Z = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 3);
                packet.ReadXORByte(transportGuid, 2);
                packet.ReadXORByte(transportGuid, 6);
                packet.ReadXORByte(transportGuid, 4);

                info.TransportGuid = packet.WriteGuid("Transport Guid", transportGuid);
                info.TransportOffset = packet.AddValue("Transport Position", tpos);
            }

            if (hasSplineElev)
                info.SplineElevation = packet.ReadSingle("Spline elevation");

            if (hasFallData)
            {
                if (hasFallDirection)
                {
                    info.JumpCosAngle = packet.ReadSingle("Fall Cos");
                    info.JumpHorizontalSpeed = packet.ReadSingle("Horizontal Speed");
                    info.JumpSinAngle = packet.ReadSingle("Fall Sin");
                }

                info.JumpVerticalSpeed = packet.ReadSingle("Vertical Speed");
                info.FallTime = packet.ReadUInt32("Fall time");
            }

            if (hasO)
                info.Orientation = pos.O = packet.ReadSingle();
            if (hasTime)
                info.MoveTime = packet.ReadUInt32("Timestamp");
            if (hasPitch)
                info.SwimPitch = packet.ReadSingle("Pitch");

            WowGuid moverGuid = packet.WriteGuid("Guid", guid);
            packet.AddValue("Position", pos);
            Storage.StorePlayerMovement(moverGuid, info, packet);
        }

        [Parser(Opcode.SMSG_MOVE_DISABLE_GRAVITY)]
        public static void HandleMoveGravityDisable434(Packet packet)
        {
            var guid = packet.StartBitStream(0, 1, 5, 7, 6, 4, 3, 2);
            packet.ReadXORByte(guid, 7);
            packet.ReadXORByte(guid, 2);
            packet.ReadXORByte(guid, 0);
            packet.ReadInt32("Movement Coutner");
            packet.ReadXORByte(guid, 5);
            packet.ReadXORByte(guid, 1);
            packet.ReadXORByte(guid, 3);
            packet.ReadXORByte(guid, 4);
            packet.ReadXORByte(guid, 6);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_MOVE_ENABLE_GRAVITY)]
        public static void HandleMoveGravityEnable434(Packet packet)
        {
            var guid = packet.StartBitStream(1, 4, 7, 5, 2, 0, 3, 6);
            packet.ReadXORByte(guid, 3);
            packet.ReadInt32("Movement Counter");
            packet.ReadXORByte(guid, 7);
            packet.ReadXORByte(guid, 6);
            packet.ReadXORByte(guid, 4);
            packet.ReadXORByte(guid, 0);
            packet.ReadXORByte(guid, 1);
            packet.ReadXORByte(guid, 5);
            packet.ReadXORByte(guid, 2);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_MOVE_SET_NORMAL_FALL)]
        public static void HandleMoveSetNormalFall(Packet packet)
        {
            packet.ReadInt32("Movement Counter");
            var guid = packet.StartBitStream(3, 0, 1, 5, 7, 4, 6, 2);
            packet.ParseBitStream(guid, 2, 7, 1, 4, 5, 0, 3, 6);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_MOVE_SET_ACTIVE_MOVER)]
        public static void HandleMoveSetActiveMover434(Packet packet)
        {
            var guid = packet.StartBitStream(5, 7, 3, 6, 0, 4, 1, 2);
            packet.ParseBitStream(guid, 6, 2, 3, 0, 5, 7, 1, 4);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_MOVE_SET_COMPOUND_STATE)]
        public static void HandleMoveSetCompoundState434(Packet packet)
        {
            var guid = packet.StartBitStream(5, 0, 4, 1, 7, 6, 2, 3);

            var count = packet.ReadBits("StateChangeCount", 23);
            var HasVehicleRecID = new byte[count];
            var HasSpeed = new byte[count];
            var HasKnockBackInfo = new byte[count];
            var HasCollisionHeightInfo = new byte[count];

            for (int i = 0; i < count; ++i)
            {
                HasVehicleRecID[i] = packet.ReadBit("HasVehicleRecID", i);
                HasSpeed[i] = packet.ReadBit("HasSpeed", i);
                HasKnockBackInfo[i] = packet.ReadBit("HasKnockBackInfo", i);
                HasCollisionHeightInfo[i] = packet.ReadBit("HasCollisionHeightInfo", i);
                if (HasCollisionHeightInfo[i] != 0)
                    packet.ReadBits("CollisionHeightInfoReason", 2, i);
            }

            for (int i = 0; i < count; ++i)
            {
                if (HasCollisionHeightInfo[i] != 0)
                    packet.ReadSingle("CollisionHeightInfoHeight", i);

                if (HasKnockBackInfo[i] != 0)
                {
                    packet.ReadSingle("HorizontalSpeed", i);
                    packet.ReadVector2("Direction", i);
                    packet.ReadSingle("InitVerticalSpeed", i);
                }

                if (HasVehicleRecID[i] != 0)
                    packet.ReadInt32("VehicleRecID", i);

                packet.ReadInt32("SequenceIndex", i);

                if (HasSpeed[i] != 0)
                    packet.ReadSingle("Speed", i);

                var opcode = packet.ReadInt16();
                var opcodeName = Opcodes.GetOpcodeName(opcode, packet.Direction);
                packet.AddValue("Opcode", $"{ opcodeName } (0x{ opcode.ToString("X4") })", i);

            }

            packet.ParseBitStream(guid, 2, 1, 4, 5, 6, 7, 0, 3);
            packet.WriteGuid("MoverGUID", guid);
        }

        [Parser(Opcode.SMSG_MOVE_SET_FLIGHT_SPEED)]
        public static void HandleMoveSetFlightSpeed434(Packet packet)
        {
            var guid = packet.StartBitStream(0, 5, 1, 6, 3, 2, 7, 4);
            packet.ReadXORByte(guid, 0);
            packet.ReadXORByte(guid, 1);
            packet.ReadXORByte(guid, 7);
            packet.ReadXORByte(guid, 5);
            CreatureSpeedUpdate speedUpdate = new CreatureSpeedUpdate();
            speedUpdate.SpeedType = SpeedType.Fly;
            speedUpdate.SpeedRate = packet.ReadSingle("Speed") / MovementInfo.DEFAULT_FLY_SPEED;
            packet.ReadInt32("Movement Counter");
            packet.ReadXORByte(guid, 2);
            packet.ReadXORByte(guid, 6);
            packet.ReadXORByte(guid, 3);
            packet.ReadXORByte(guid, 4);
            WowGuid moverGuid = packet.WriteGuid("Guid", guid);
            speedUpdate.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(packet.Time);
            Storage.StoreUnitSpeedUpdate(moverGuid, speedUpdate);
        }

        [Parser(Opcode.SMSG_MOVE_SET_FLIGHT_BACK_SPEED)]
        public static void HandleMoveSetFlightBackSpeed434(Packet packet)
        {
            var guid = packet.StartBitStream(1, 2, 6, 4, 7, 3, 0, 5);
            packet.ReadXORByte(guid, 3);
            packet.ReadInt32("Movement Counter");
            packet.ReadXORByte(guid, 6);
            CreatureSpeedUpdate speedUpdate = new CreatureSpeedUpdate();
            speedUpdate.SpeedType = SpeedType.FlyBack;
            speedUpdate.SpeedRate = packet.ReadSingle("Speed") / MovementInfo.DEFAULT_FLY_BACK_SPEED;
            packet.ReadXORByte(guid, 1);
            packet.ReadXORByte(guid, 2);
            packet.ReadXORByte(guid, 4);
            packet.ReadXORByte(guid, 0);
            packet.ReadXORByte(guid, 5);
            packet.ReadXORByte(guid, 7);
            WowGuid moverGuid = packet.WriteGuid("Guid", guid);
            speedUpdate.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(packet.Time);
            Storage.StoreUnitSpeedUpdate(moverGuid, speedUpdate);
        }

        [Parser(Opcode.SMSG_MOVE_SET_RUN_BACK_SPEED)]
        public static void HandleMoveSetRunBackSpeed434(Packet packet)
        {
            var guid = packet.StartBitStream(0, 6, 2, 1, 3, 5, 4, 7);
            packet.ReadXORByte(guid, 5);
            packet.ReadInt32("Movement Counter");
            CreatureSpeedUpdate speedUpdate = new CreatureSpeedUpdate();
            speedUpdate.SpeedType = SpeedType.RunBack;
            speedUpdate.SpeedRate = packet.ReadSingle("Speed") / MovementInfo.DEFAULT_RUN_BACK_SPEED;
            packet.ReadXORByte(guid, 0);
            packet.ReadXORByte(guid, 4);
            packet.ReadXORByte(guid, 7);
            packet.ReadXORByte(guid, 3);
            packet.ReadXORByte(guid, 1);
            packet.ReadXORByte(guid, 2);
            packet.ReadXORByte(guid, 6);
            WowGuid moverGuid = packet.WriteGuid("Guid", guid);
            speedUpdate.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(packet.Time);
            Storage.StoreUnitSpeedUpdate(moverGuid, speedUpdate);
        }

        [Parser(Opcode.SMSG_MOVE_SET_SWIM_SPEED)]
        public static void HandleMoveSetSwimSpeed434(Packet packet)
        {
            var guid = packet.StartBitStream(5, 4, 7, 3, 2, 0, 1, 6);
            packet.ReadXORByte(guid, 0);
            packet.ReadInt32("Movement Counter");
            packet.ReadXORByte(guid, 6);
            packet.ReadXORByte(guid, 3);
            packet.ReadXORByte(guid, 5);
            packet.ReadXORByte(guid, 2);
            CreatureSpeedUpdate speedUpdate = new CreatureSpeedUpdate();
            speedUpdate.SpeedType = SpeedType.Swim;
            speedUpdate.SpeedRate = packet.ReadSingle("Speed") / MovementInfo.DEFAULT_SWIM_SPEED;
            packet.ReadXORByte(guid, 1);
            packet.ReadXORByte(guid, 7);
            packet.ReadXORByte(guid, 4);
            WowGuid moverGuid = packet.WriteGuid("Guid", guid);
            speedUpdate.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(packet.Time);
            Storage.StoreUnitSpeedUpdate(moverGuid, speedUpdate);
        }

        [Parser(Opcode.SMSG_MOVE_SET_SWIM_BACK_SPEED)]
        public static void HandleMoveSetSwimBackSpeed434(Packet packet)
        {
            var guid = packet.StartBitStream(4, 2, 3, 6, 5, 1, 0, 7);
            packet.ReadInt32("Movement Counter");
            packet.ReadXORByte(guid, 0);
            packet.ReadXORByte(guid, 3);
            packet.ReadXORByte(guid, 4);
            packet.ReadXORByte(guid, 6);
            packet.ReadXORByte(guid, 5);
            packet.ReadXORByte(guid, 1);
            CreatureSpeedUpdate speedUpdate = new CreatureSpeedUpdate();
            speedUpdate.SpeedType = SpeedType.SwimBack;
            speedUpdate.SpeedRate = packet.ReadSingle("Speed") / MovementInfo.DEFAULT_SWIM_BACK_SPEED;
            packet.ReadXORByte(guid, 7);
            packet.ReadXORByte(guid, 2);
            WowGuid moverGuid = packet.WriteGuid("Guid", guid);
            speedUpdate.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(packet.Time);
            Storage.StoreUnitSpeedUpdate(moverGuid, speedUpdate);
        }

        [Parser(Opcode.SMSG_MOVE_SET_WALK_SPEED)]
        public static void HandleMoveSetWalkSpeed434(Packet packet)
        {
            var guid = packet.StartBitStream(0, 4, 5, 2, 3, 1, 6, 7);
            packet.ReadXORByte(guid, 6);
            packet.ReadXORByte(guid, 1);
            packet.ReadXORByte(guid, 5);
            CreatureSpeedUpdate speedUpdate = new CreatureSpeedUpdate();
            speedUpdate.SpeedType = SpeedType.Walk;
            speedUpdate.SpeedRate = packet.ReadSingle("Speed") / MovementInfo.DEFAULT_WALK_SPEED;
            packet.ReadXORByte(guid, 2);
            packet.ReadInt32("Movement Counter");
            packet.ReadXORByte(guid, 4);
            packet.ReadXORByte(guid, 0);
            packet.ReadXORByte(guid, 7);
            packet.ReadXORByte(guid, 3);
            WowGuid moverGuid = packet.WriteGuid("Guid", guid);
            speedUpdate.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(packet.Time);
            Storage.StoreUnitSpeedUpdate(moverGuid, speedUpdate);
        }

        [Parser(Opcode.SMSG_MOVE_SET_TURN_RATE)]
        public static void HandleMoveSetTurnRate434(Packet packet)
        {
            var guid = packet.StartBitStream(7, 2, 1, 0, 4, 5, 6, 3);
            packet.ReadXORByte(guid, 5);
            packet.ReadXORByte(guid, 7);
            packet.ReadXORByte(guid, 2);
            CreatureSpeedUpdate speedUpdate = new CreatureSpeedUpdate();
            speedUpdate.SpeedType = SpeedType.Turn;
            speedUpdate.SpeedRate = packet.ReadSingle("Rate") / MovementInfo.DEFAULT_TURN_RATE;
            packet.ReadXORByte(guid, 3);
            packet.ReadXORByte(guid, 1);
            packet.ReadXORByte(guid, 0);
            packet.ReadInt32("Movement Counter");
            packet.ReadXORByte(guid, 6);
            packet.ReadXORByte(guid, 4);
            WowGuid moverGuid = packet.WriteGuid("Guid", guid);
            speedUpdate.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(packet.Time);
            Storage.StoreUnitSpeedUpdate(moverGuid, speedUpdate);
        }

        [Parser(Opcode.SMSG_MOVE_SET_PITCH_RATE)]
        public static void HandleMoveSetPitchRate434(Packet packet)
        {
            var guid = packet.StartBitStream(1, 2, 6, 7, 0, 3, 5, 4);
            CreatureSpeedUpdate speedUpdate = new CreatureSpeedUpdate();
            speedUpdate.SpeedType = SpeedType.Pitch;
            speedUpdate.SpeedRate = packet.ReadSingle("Rate") / MovementInfo.DEFAULT_PITCH_RATE;
            packet.ReadXORByte(guid, 6);
            packet.ReadXORByte(guid, 4);
            packet.ReadXORByte(guid, 0);
            packet.ReadInt32("Movement Counter");
            packet.ReadXORByte(guid, 1);
            packet.ReadXORByte(guid, 2);
            packet.ReadXORByte(guid, 7);
            packet.ReadXORByte(guid, 3);
            packet.ReadXORByte(guid, 5);
            WowGuid moverGuid = packet.WriteGuid("Guid", guid);
            speedUpdate.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(packet.Time);
            Storage.StoreUnitSpeedUpdate(moverGuid, speedUpdate);
        }

        [Parser(Opcode.SMSG_MOVE_SPLINE_SET_FEATHER_FALL)]
        public static void HandleSplineMoveSetFeatherFall434(Packet packet)
        {
            var guid = packet.StartBitStream(3, 2, 7, 5, 4, 6, 1, 0);
            packet.ParseBitStream(guid, 1, 4, 7, 6, 2, 0, 5, 3);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_MOVE_SPLINE_SET_LAND_WALK)]
        public static void HandleSplineMoveSetLandWalk434(Packet packet)
        {
            var guid = packet.StartBitStream(5, 0, 4, 6, 7, 2, 3, 1);
            packet.ParseBitStream(guid, 5, 7, 3, 4, 1, 2, 0, 6);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_MOVE_SPLINE_SET_NORMAL_FALL)]
        public static void HandleSplineMoveSetNormalFall434(Packet packet)
        {
            var guid = packet.StartBitStream(3, 5, 1, 0, 7, 6, 2, 4);
            packet.ParseBitStream(guid, 7, 6, 2, 0, 5, 4, 3, 1);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_MOVE_SPLINE_SET_ANIM)]
        public static void HandleSplineMoveSetAnim(Packet packet)
        {
            packet.ReadPackedGuid("Guid");
            packet.ReadUInt32E<MovementAnimationState>("Animation");
        }

        [Parser(Opcode.SMSG_MOVE_UPDATE_KNOCK_BACK)]
        public static void HandleMoveUpdateKnockBack434(Packet packet)
        {
            var guid = new byte[8];
            var transportGuid = new byte[8];
            var hasFallDirection = false;
            var pos = new Vector4();

            packet.ReadBit();
            guid[4] = packet.ReadBit();
            var hasMovementFlags = !packet.ReadBit();
            var hasPitch = !packet.ReadBit();
            var hasTime = !packet.ReadBit();
            guid[1] = packet.ReadBit();
            guid[0] = packet.ReadBit();
            guid[3] = packet.ReadBit();
            guid[2] = packet.ReadBit();
            guid[7] = packet.ReadBit();
            packet.ReadBit("Has spline data");
            var hasTransport = packet.ReadBit();

            var hasTransportTime2 = false;
            var hasTransportTime3 = false;
            if (hasTransport)
            {
                transportGuid[7] = packet.ReadBit();
                transportGuid[5] = packet.ReadBit();
                transportGuid[1] = packet.ReadBit();
                transportGuid[6] = packet.ReadBit();
                hasTransportTime2 = packet.ReadBit();
                transportGuid[2] = packet.ReadBit();
                transportGuid[4] = packet.ReadBit();
                transportGuid[0] = packet.ReadBit();
                hasTransportTime3 = packet.ReadBit();
                transportGuid[3] = packet.ReadBit();
            }

            guid[5] = packet.ReadBit();
            var hasSplineElevation = !packet.ReadBit();
            var hasMovementFlagsExtra = !packet.ReadBit();
            guid[6] = packet.ReadBit();

            if (hasMovementFlags)
                packet.ReadBitsE<MovementFlag>("Movement flags", 30);

            var hasFallData = packet.ReadBit();

            if (hasFallData)
                hasFallDirection = packet.ReadBit();

            var hasO = !packet.ReadBit();

            if (hasMovementFlagsExtra)
                packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", 12);

            if (hasO)
                pos.O = packet.ReadSingle();

            if (hasFallData)
            {
                if (hasFallDirection)
                {
                    packet.ReadSingle("Fall Cos");
                    packet.ReadSingle("Horizontal speed");
                    packet.ReadSingle("Fall Sin");
                }

                packet.ReadUInt32("Fall time");
                packet.ReadSingle("Vertical speed");
            }

            if (hasSplineElevation)
                packet.ReadSingle("Spline elevation");

            packet.ReadXORByte(guid, 3);

            if (hasTransport)
            {
                var tpos = new Vector4();

                packet.ReadXORByte(transportGuid, 5);

                if (hasTransportTime3)
                    packet.ReadUInt32("Transport Time 3");

                packet.ReadXORByte(transportGuid, 7);
                packet.ReadSByte("Transport Seat");
                packet.ReadXORByte(transportGuid, 3);
                packet.ReadXORByte(transportGuid, 6);
                tpos.Z = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 1);
                tpos.Y = packet.ReadSingle();
                tpos.X = packet.ReadSingle();
                packet.ReadXORByte(transportGuid, 2);
                packet.ReadXORByte(transportGuid, 0);
                tpos.O = packet.ReadSingle();
                packet.ReadUInt32("Transport Time");
                packet.ReadXORByte(transportGuid, 4);

                if (hasTransportTime2)
                    packet.ReadUInt32("Transport Time 2");

                packet.WriteGuid("Transport Guid", transportGuid);
                packet.AddValue("Transport Position", tpos);
            }

            if (hasPitch)
                packet.ReadSingle("Pitch");

            pos.Z = packet.ReadSingle();

            if (hasTime)
                packet.ReadUInt32("Timestamp");

            pos.X = packet.ReadSingle();
            packet.ReadXORByte(guid, 4);
            packet.ReadXORByte(guid, 6);
            packet.ReadXORByte(guid, 7);
            packet.ReadXORByte(guid, 2);
            packet.ReadXORByte(guid, 1);

            pos.Y = packet.ReadSingle();
            packet.ReadXORByte(guid, 0);
            packet.ReadXORByte(guid, 5);

            packet.WriteGuid("Guid", guid);
            packet.AddValue("Position", pos);
        }

        [Parser(Opcode.SMSG_MOVE_SET_WATER_WALK)]
        public static void HandleMoveSetWaterWalk(Packet packet)
        {
            var guid = new byte[8];

            guid[4] = packet.ReadBit();
            guid[7] = packet.ReadBit();
            guid[6] = packet.ReadBit();
            guid[0] = packet.ReadBit();
            guid[1] = packet.ReadBit();
            guid[3] = packet.ReadBit();
            guid[5] = packet.ReadBit();
            guid[2] = packet.ReadBit();

            packet.ReadXORByte(guid, 0);
            packet.ReadXORByte(guid, 5);
            packet.ReadXORByte(guid, 2);

            uint moveCounter = packet.ReadUInt32("Movement Counter: ");

            packet.ReadXORByte(guid, 7);
            packet.ReadXORByte(guid, 3);
            packet.ReadXORByte(guid, 4);
            packet.ReadXORByte(guid, 1);
            packet.ReadXORByte(guid, 6);

            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_MOVE_SET_LAND_WALK)]
        public static void HandleMoveSetLandWalk(Packet packet)
        {
            var guid = packet.StartBitStream(5, 0, 4, 6, 7, 2, 3, 1);
            packet.ParseBitStream(guid, 5, 7, 3, 4, 1, 2, 0, 6);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_MOVE_SET_HOVERING)]
        public static void HandleMoveSetHover(Packet packet)
        {
            var guid = packet.StartBitStream(3, 7, 0, 1, 4, 6, 2, 5);
            packet.ParseBitStream(guid, 2, 4, 3, 1, 7, 0, 5, 6);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_MOVE_UNSET_HOVERING)]
        public static void HandleMoveUnsetHover(Packet packet)
        {
            var guid = packet.StartBitStream(6, 7, 4, 0, 3, 1, 5, 2);
            packet.ParseBitStream(guid, 4, 5, 3, 0, 2, 7, 6, 1);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_MOVE_SET_FEATHER_FALL)]
        public static void HandleMoveSetFeatherFall(Packet packet)
        {
            var guid = packet.StartBitStream(3, 1, 7, 0, 4, 2, 5, 6);
            packet.ReadXORByte(guid, 5);
            packet.ReadXORByte(guid, 7);
            packet.ReadXORByte(guid, 2);
            packet.ReadInt32("Movement Counter");
            packet.ReadXORByte(guid, 0);
            packet.ReadXORByte(guid, 3);
            packet.ReadXORByte(guid, 4);
            packet.ReadXORByte(guid, 1);
            packet.ReadXORByte(guid, 6);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_MOVE_SET_CAN_FLY)]
        public static void HandleMoveSetCanFly(Packet packet)
        {
            var guid = packet.StartBitStream(1, 6, 5, 0, 7, 4, 2, 3);
            packet.ReadXORByte(guid, 6);
            packet.ReadXORByte(guid, 3);
            packet.ReadInt32("Movement Counter");
            packet.ReadXORByte(guid, 2);
            packet.ReadXORByte(guid, 1);
            packet.ReadXORByte(guid, 4);
            packet.ReadXORByte(guid, 7);
            packet.ReadXORByte(guid, 0);
            packet.ReadXORByte(guid, 5);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_MOVE_UNSET_CAN_FLY)]
        public static void HandleMoveUnsetCanFly(Packet packet)
        {
            var guid = packet.StartBitStream(1, 4, 2, 5, 0, 3, 6, 7);
            packet.ReadXORByte(guid, 4);
            packet.ReadXORByte(guid, 6);
            packet.ReadInt32("Movement Counter");
            packet.ReadXORByte(guid, 1);
            packet.ReadXORByte(guid, 0);
            packet.ReadXORByte(guid, 2);
            packet.ReadXORByte(guid, 3);
            packet.ReadXORByte(guid, 5);
            packet.ReadXORByte(guid, 7);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_MOVE_KNOCK_BACK)]
        public static void HandleMoveKnockBack(Packet packet)
        {
            var guid = packet.StartBitStream(0, 3, 6, 7, 2, 5, 1, 4);
            packet.ReadXORByte(guid, 1);
            packet.ReadSingle("DirectionY");
            packet.ReadInt32("SequenceIndex");
            packet.ReadXORByte(guid, 6);
            packet.ReadXORByte(guid, 7);
            packet.ReadSingle("HorzSpeed");
            packet.ReadXORByte(guid, 4);
            packet.ReadXORByte(guid, 5);
            packet.ReadXORByte(guid, 3);
            packet.ReadSingle("VertSpeed");
            packet.ReadSingle("DirectionX");
            packet.ReadXORByte(guid, 2);
            packet.ReadXORByte(guid, 0);
            packet.WriteGuid("MoverGUID", guid);
        }
    }
}
