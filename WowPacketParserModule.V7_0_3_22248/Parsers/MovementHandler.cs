using System.Collections.Generic;
using System.Linq;
using WowPacketParser.DBC;
using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;
using WowPacketParser.Store;
using WowPacketParser.Store.Objects;
using WowPacketParserModule.V7_0_3_22248.Enums;
using CoreParsers = WowPacketParser.Parsing.Parsers;
using MovementFlag = WowPacketParser.Enums.v4.MovementFlag;
using MovementFlag2 = WowPacketParser.Enums.v7.MovementFlag2;
using SplineFacingType = WowPacketParserModule.V6_0_2_19033.Enums.SplineFacingType;
using SplineFlag = WowPacketParserModule.V7_0_3_22248.Enums.SplineFlag;

namespace WowPacketParserModule.V7_0_3_22248.Parsers
{
    public static class MovementHandler
    {
        public static WowGuid ReadMovementStats(Packet packet, params object[] idx)
        {
            MovementInfo moveInfo = new MovementInfo();
            WowGuid moverGuid = packet.ReadPackedGuid128("MoverGUID", idx);

            if (ClientVersion.IsVersionWithUpdatedMovementInfo())
            {
                moveInfo.Flags = (uint)packet.ReadUInt32E<MovementFlag>("MovementFlags", idx);
                moveInfo.Flags2 = (uint)packet.ReadUInt32E<MovementFlag2>("MovementFlags2", idx);
                moveInfo.Flags3 = (uint)packet.ReadUInt32E<MovementFlag3>("MovementFlags3", idx);
            }

            moveInfo.MoveTime = packet.ReadUInt32("MoveTime", idx);
            moveInfo.Position = packet.ReadVector3("Position", idx);
            moveInfo.Orientation = packet.ReadSingle("Orientation", idx);

            moveInfo.SwimPitch = packet.ReadSingle("Pitch", idx);
            moveInfo.SplineElevation = packet.ReadSingle("SplineElevation", idx);

            var removeForcesIDsCount = packet.ReadInt32("RemoveForcesCount", idx);
            packet.ReadInt32("MoveIndex", idx);

            for (var i = 0; i < removeForcesIDsCount; i++)
                packet.ReadPackedGuid128("RemoveForcesIDs", idx, i);

            packet.ResetBitReader();

            if (!ClientVersion.IsVersionWithUpdatedMovementInfo())
            {
                moveInfo.Flags = (uint)packet.ReadBitsE<MovementFlag>("MovementFlags", 30, idx);
                moveInfo.Flags2 = (uint)packet.ReadBitsE<MovementFlag2>("MovementFlags2", 18, idx);
            }

            var hasTransport = packet.ReadBit("HasTransportData", idx);
            var hasFall = packet.ReadBit("HasFallData", idx);
            packet.ReadBit("HasSpline", idx);
            packet.ReadBit("HeightChangeFailed", idx);
            packet.ReadBit("RemoteTimeValid", idx);

            if (hasTransport)
                V6_0_2_19033.Parsers.MovementHandler.ReadTransportData(moveInfo, packet, idx, "TransportData");

            if (hasFall)
                V6_0_2_19033.Parsers.MovementHandler.ReadFallData(moveInfo, packet, idx, "FallData");

            Storage.StorePlayerMovement(moverGuid, moveInfo, packet);
            return moverGuid;
        }

        public static void ReadMovementAck(Packet packet, params object[] idx)
        {
            ReadMovementStats(packet, idx);
            packet.ReadInt32("AckIndex", idx);
        }

        public static void ReadMovementForce(Packet packet, params object[] idx)
        {
            packet.ReadPackedGuid128("ID", idx);
            packet.ReadVector3("Origin", idx);
            packet.ReadVector3("Direction", idx);
            packet.ReadVector3("TransportPosition", idx);
            packet.ReadInt32("TransportID", idx);
            packet.ReadSingle("Magnitude", idx);

            packet.ResetBitReader();

            packet.ReadBits("Type", 2, idx);
        }

        public static void ReadMovementMonsterSpline(ServerSideMovement monsterMove, Packet packet, Vector3 pos, params object[] indexes)
        {
            packet.ReadUInt32("Id", indexes);
            packet.ReadVector3("Destination", indexes);

            packet.ResetBitReader();

            packet.ReadBit("CrzTeleport", indexes);
            packet.ReadBits("StopDistanceTolerance", 3, indexes);

            ReadMovementSpline(monsterMove, packet, pos, indexes, "MovementSpline");
        }

        public static void ReadMonsterSplineFilter(Packet packet, params object[] indexes)
        {
            var count = packet.ReadUInt32("MonsterSplineFilterKey", indexes);
            packet.ReadSingle("BaseSpeed", indexes);
            packet.ReadUInt16("StartOffset", indexes);
            packet.ReadSingle("DistToPrevFilterKey", indexes);
            packet.ReadUInt16("AddedToStart", indexes);

            for (int i = 0; i < count; i++)
            {
                packet.ReadInt16("IDx", indexes, i);
                packet.ReadUInt16("Speed", indexes, i);
            }

            packet.ResetBitReader();
            packet.ReadBits("FilterFlags", 2, indexes);
        }

        public static void ReadMonsterSplineSpellEffectExtraData(Packet packet, params object[] indexes)
        {
            packet.ReadPackedGuid128("TargetGUID", indexes);
            packet.ReadUInt32("SpellVisualID", indexes);
            packet.ReadUInt32("ProgressCurveID", indexes);
            packet.ReadUInt32("ParabolicCurveID", indexes);
        }

        public static float ReadMovementSpline(ServerSideMovement monsterMove, Packet packet, Vector3 pos, params object[] indexes)
        {
            uint splineFlags = (uint)packet.ReadInt32E<SplineFlag>("Flags", indexes);
            if (monsterMove != null)
                monsterMove.SplineFlags = splineFlags;
            packet.ReadByte("AnimTier", indexes);
            packet.ReadUInt32("TierTransStartTime", indexes);
            packet.ReadInt32("Elapsed", indexes);
            uint moveTime = packet.ReadUInt32("MoveTime", indexes);
            if (monsterMove != null)
                monsterMove.MoveTime = moveTime;
            packet.ReadSingle("JumpGravity", indexes);
            packet.ReadUInt32("SpecialTime", indexes);

            packet.ReadByte("Mode", indexes);
            packet.ReadByte("VehicleExitVoluntary", indexes);

            WowGuid transportGuid = packet.ReadPackedGuid128("TransportGUID", indexes);
            if (monsterMove != null)
                monsterMove.TransportGuid = transportGuid;
            sbyte seat = packet.ReadSByte("VehicleSeat", indexes);
            if (monsterMove != null)
                monsterMove.TransportSeat = seat;

            packet.ResetBitReader();

            var type = packet.ReadBitsE<SplineFacingType>("Face", 2, indexes);
            var pointsCount = packet.ReadBits("PointsCount", 16, indexes);
            var packedDeltasCount = packet.ReadBits("PackedDeltasCount", 16, indexes);
            var totalPointsCount = pointsCount + packedDeltasCount;
            var hasSplineFilter = packet.ReadBit("HasSplineFilter", indexes);
            var hasSpellEffectExtraData = packet.ReadBit("HasSpellEffectExtraData", indexes);

            if (hasSplineFilter)
                ReadMonsterSplineFilter(packet, indexes, "MonsterSplineFilter");

            float orientation = 100;
            switch (type)
            {
                case SplineFacingType.Spot:
                    var faceSpot = packet.ReadVector3("FaceSpot", indexes);
                    orientation = Utilities.GetAngle(pos.X, pos.Y, faceSpot.X, faceSpot.Y);
                    break;
                case SplineFacingType.Target:
                    orientation = packet.ReadSingle("FaceDirection", indexes);
                    packet.ReadPackedGuid128("FacingGUID", indexes);
                    break;
                case SplineFacingType.Angle:
                    orientation = packet.ReadSingle("FaceDirection", indexes);
                    break;
            }

            if (monsterMove != null)
            {
                monsterMove.Orientation = orientation;
                monsterMove.SplineCount = totalPointsCount;
                if (totalPointsCount > 0)
                    monsterMove.SplinePoints = new List<Vector3>();
            }

            Vector3 endpos = new Vector3();
            List<Vector3> pointsList = (monsterMove != null) ? new List<Vector3>() : null;
            for (int i = 0; i < pointsCount; i++)
            {
                var spot = packet.ReadVector3();

                // client always taking first point
                if (i == 0)
                    endpos = spot;

                if (monsterMove != null)
                    pointsList.Add(spot);

                packet.AddValue("Points", spot, indexes, i);
            }

            var waypoints = new Vector3[packedDeltasCount];
            for (int i = 0; i < packedDeltasCount; i++)
            {
                var packedDeltas = packet.ReadPackedVector3();
                waypoints[i].X = packedDeltas.X;
                waypoints[i].Y = packedDeltas.Y;
                waypoints[i].Z = packedDeltas.Z;
            }

            if (hasSpellEffectExtraData)
                ReadMonsterSplineSpellEffectExtraData(packet, "MonsterSplineSpellEffectExtra");

            // Calculate mid pos
            var mid = new Vector3
            {
                X = (pos.X + endpos.X) * 0.5f,
                Y = (pos.Y + endpos.Y) * 0.5f,
                Z = (pos.Z + endpos.Z) * 0.5f
            };

            for (var i = 0; i < packedDeltasCount; ++i)
            {
                var vec = new Vector3
                {
                    X = mid.X - waypoints[i].X,
                    Y = mid.Y - waypoints[i].Y,
                    Z = mid.Z - waypoints[i].Z
                };

                if (monsterMove != null)
                    monsterMove.SplinePoints.Add(vec);

                packet.AddValue("WayPoints", vec, indexes, i);
            }

            if (monsterMove != null)
            {
                foreach (var point in pointsList)
                {
                    monsterMove.SplinePoints.Add(point);
                }
            }

            return orientation;
        }

        [Parser(Opcode.SMSG_ON_MONSTER_MOVE)]
        public static void HandleOnMonsterMove(Packet packet)
        {
            WowGuid guid = packet.ReadPackedGuid128("MoverGUID");
            var pos = packet.ReadVector3("Position");

            Unit obj = null;
            ServerSideMovement monsterMove = null;
            if (Storage.Objects != null && Storage.Objects.ContainsKey(guid))
            {
                obj = Storage.Objects[guid].Item1 as Unit;
                if (obj.UpdateFields != null)
                {
                    obj.Movement.HasWpsOrRandMov = true;
                    monsterMove = new ServerSideMovement();
                }
            }

            ReadMovementMonsterSpline(monsterMove, packet, pos, "MovementMonsterSpline");

            if (monsterMove != null && (Settings.SaveTransports || (monsterMove.TransportGuid == null || monsterMove.TransportGuid.IsEmpty())))
                obj.AddWaypoint(monsterMove, pos, packet.Time);
        }

        [Parser(Opcode.CMSG_MOVE_CHANGE_TRANSPORT)]
        [Parser(Opcode.CMSG_MOVE_DISMISS_VEHICLE)]
        [Parser(Opcode.CMSG_MOVE_FALL_LAND)]
        [Parser(Opcode.CMSG_MOVE_FALL_RESET)]
        [Parser(Opcode.CMSG_MOVE_HEARTBEAT)]
        [Parser(Opcode.CMSG_MOVE_JUMP)]
        [Parser(Opcode.CMSG_MOVE_REMOVE_MOVEMENT_FORCES)]
        [Parser(Opcode.CMSG_MOVE_SET_FACING)]
        [Parser(Opcode.CMSG_MOVE_SET_FLY)]
        [Parser(Opcode.CMSG_MOVE_SET_PITCH)]
        [Parser(Opcode.CMSG_MOVE_SET_RUN_MODE)]
        [Parser(Opcode.CMSG_MOVE_SET_WALK_MODE)]
        [Parser(Opcode.CMSG_MOVE_START_ASCEND)]
        [Parser(Opcode.CMSG_MOVE_START_BACKWARD)]
        [Parser(Opcode.CMSG_MOVE_START_DESCEND)]
        [Parser(Opcode.CMSG_MOVE_START_FORWARD)]
        [Parser(Opcode.CMSG_MOVE_START_PITCH_DOWN)]
        [Parser(Opcode.CMSG_MOVE_START_PITCH_UP)]
        [Parser(Opcode.CMSG_MOVE_START_SWIM)]
        [Parser(Opcode.CMSG_MOVE_START_TURN_LEFT)]
        [Parser(Opcode.CMSG_MOVE_START_TURN_RIGHT)]
        [Parser(Opcode.CMSG_MOVE_START_STRAFE_LEFT)]
        [Parser(Opcode.CMSG_MOVE_START_STRAFE_RIGHT)]
        [Parser(Opcode.CMSG_MOVE_STOP)]
        [Parser(Opcode.CMSG_MOVE_STOP_ASCEND)]
        [Parser(Opcode.CMSG_MOVE_STOP_PITCH)]
        [Parser(Opcode.CMSG_MOVE_STOP_STRAFE)]
        [Parser(Opcode.CMSG_MOVE_STOP_SWIM)]
        [Parser(Opcode.CMSG_MOVE_STOP_TURN)]
        [Parser(Opcode.SMSG_MOVE_UPDATE_KNOCK_BACK)]
        [Parser(Opcode.SMSG_MOVE_UPDATE)]
        [Parser(Opcode.CMSG_MOVE_DOUBLE_JUMP)]
        public static void HandlePlayerMove(Packet packet)
        {
            ReadMovementStats(packet, "MovementStats");
        }

        [Parser(Opcode.CMSG_MOVE_GRAVITY_DISABLE_ACK)]
        [Parser(Opcode.CMSG_MOVE_GRAVITY_ENABLE_ACK)]
        [Parser(Opcode.CMSG_MOVE_HOVER_ACK)]
        [Parser(Opcode.CMSG_MOVE_KNOCK_BACK_ACK, ClientVersionBuild.V7_0_3_22248, ClientVersionBuild.V7_2_0_23826)]
        [Parser(Opcode.CMSG_MOVE_WATER_WALK_ACK)]
        [Parser(Opcode.CMSG_MOVE_FORCE_ROOT_ACK)]
        [Parser(Opcode.CMSG_MOVE_FORCE_UNROOT_ACK)]
        [Parser(Opcode.CMSG_MOVE_SET_CAN_FLY_ACK)]
        [Parser(Opcode.CMSG_MOVE_ENABLE_SWIM_TO_FLY_TRANS_ACK)]
        [Parser(Opcode.CMSG_MOVE_FEATHER_FALL_ACK)]
        [Parser(Opcode.CMSG_MOVE_SET_CAN_TURN_WHILE_FALLING_ACK)]
        [Parser(Opcode.CMSG_MOVE_ENABLE_DOUBLE_JUMP_ACK)]
        public static void HandleMovementAck(Packet packet)
        {
            ReadMovementAck(packet, "MovementAck");
        }

        [Parser(Opcode.CMSG_MOVE_KNOCK_BACK_ACK, ClientVersionBuild.V7_2_0_23826)]
        public static void HandleMoveKnockBackAck(Packet packet)
        {
            ReadMovementAck(packet, "MovementAck");

            packet.ResetBitReader();

            var hasSpeeds = packet.ReadBit("HasSpeeds");
            if (hasSpeeds)
            {
                packet.ReadSingle("HorzSpeed");
                packet.ReadSingle("VertSpeed");
            }
        }

        [Parser(Opcode.CMSG_MOVE_FORCE_FLIGHT_SPEED_CHANGE_ACK)]
        [Parser(Opcode.CMSG_MOVE_FORCE_RUN_SPEED_CHANGE_ACK)]
        [Parser(Opcode.CMSG_MOVE_FORCE_RUN_BACK_SPEED_CHANGE_ACK)]
        [Parser(Opcode.CMSG_MOVE_FORCE_SWIM_SPEED_CHANGE_ACK)]
        [Parser(Opcode.CMSG_MOVE_FORCE_WALK_SPEED_CHANGE_ACK)]
        public static void HandleMovementSpeedAck(Packet packet)
        {
            ReadMovementAck(packet, "MovementAck");
            packet.ReadSingle("Speed");
        }

        [Parser(Opcode.SMSG_MOVE_UPDATE_WALK_SPEED)]
        public static void HandleMovementUpdateWalkSpeed(Packet packet)
        {
            WowGuid guid = ReadMovementStats(packet, "MovementStats");
            CreatureSpeedUpdate speedUpdate = new CreatureSpeedUpdate();
            speedUpdate.SpeedType = SpeedType.Walk;
            speedUpdate.SpeedRate = packet.ReadSingle("Speed") / MovementInfo.DEFAULT_WALK_SPEED;
            speedUpdate.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(packet.Time);
            Storage.StoreUnitSpeedUpdate(guid, speedUpdate);
        }

        [Parser(Opcode.SMSG_MOVE_UPDATE_RUN_SPEED)]
        public static void HandleMovementUpdateRunSpeed(Packet packet)
        {
            WowGuid guid = ReadMovementStats(packet, "MovementStats");
            CreatureSpeedUpdate speedUpdate = new CreatureSpeedUpdate();
            speedUpdate.SpeedType = SpeedType.Run;
            speedUpdate.SpeedRate = packet.ReadSingle("Speed") / MovementInfo.DEFAULT_RUN_SPEED;
            speedUpdate.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(packet.Time);
            Storage.StoreUnitSpeedUpdate(guid, speedUpdate);
        }

        [Parser(Opcode.SMSG_MOVE_UPDATE_RUN_BACK_SPEED)]
        public static void HandleMovementUpdateRunBackSpeed(Packet packet)
        {
            WowGuid guid = ReadMovementStats(packet, "MovementStats");
            CreatureSpeedUpdate speedUpdate = new CreatureSpeedUpdate();
            speedUpdate.SpeedType = SpeedType.RunBack;
            speedUpdate.SpeedRate = packet.ReadSingle("Speed") / MovementInfo.DEFAULT_RUN_BACK_SPEED;
            speedUpdate.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(packet.Time);
            Storage.StoreUnitSpeedUpdate(guid, speedUpdate);
        }

        [Parser(Opcode.SMSG_MOVE_UPDATE_SWIM_SPEED)]
        public static void HandleMovementUpdateSwimSpeed(Packet packet)
        {
            WowGuid guid = ReadMovementStats(packet, "MovementStats");
            CreatureSpeedUpdate speedUpdate = new CreatureSpeedUpdate();
            speedUpdate.SpeedType = SpeedType.Swim;
            speedUpdate.SpeedRate = packet.ReadSingle("Speed") / MovementInfo.DEFAULT_SWIM_SPEED;
            speedUpdate.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(packet.Time);
            Storage.StoreUnitSpeedUpdate(guid, speedUpdate);
        }

        [Parser(Opcode.SMSG_MOVE_UPDATE_SWIM_BACK_SPEED)]
        public static void HandleMovementUpdateSwimBackSpeed(Packet packet)
        {
            WowGuid guid = ReadMovementStats(packet, "MovementStats");
            CreatureSpeedUpdate speedUpdate = new CreatureSpeedUpdate();
            speedUpdate.SpeedType = SpeedType.SwimBack;
            speedUpdate.SpeedRate = packet.ReadSingle("Speed") / MovementInfo.DEFAULT_SWIM_BACK_SPEED;
            speedUpdate.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(packet.Time);
            Storage.StoreUnitSpeedUpdate(guid, speedUpdate);
        }

        [Parser(Opcode.SMSG_MOVE_UPDATE_TURN_RATE)]
        public static void HandleMovementUpdateTurnRate(Packet packet)
        {
            WowGuid guid = ReadMovementStats(packet, "MovementStats");
            CreatureSpeedUpdate speedUpdate = new CreatureSpeedUpdate();
            speedUpdate.SpeedType = SpeedType.Turn;
            speedUpdate.SpeedRate = packet.ReadSingle("Speed") / MovementInfo.DEFAULT_TURN_RATE;
            speedUpdate.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(packet.Time);
            Storage.StoreUnitSpeedUpdate(guid, speedUpdate);
        }

        [Parser(Opcode.SMSG_MOVE_UPDATE_FLIGHT_SPEED)]
        public static void HandleMovementUpdateFlightSpeed(Packet packet)
        {
            WowGuid guid = ReadMovementStats(packet, "MovementStats");
            CreatureSpeedUpdate speedUpdate = new CreatureSpeedUpdate();
            speedUpdate.SpeedType = SpeedType.Fly;
            speedUpdate.SpeedRate = packet.ReadSingle("Speed") / MovementInfo.DEFAULT_FLY_SPEED;
            speedUpdate.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(packet.Time);
            Storage.StoreUnitSpeedUpdate(guid, speedUpdate);
        }

        [Parser(Opcode.SMSG_MOVE_UPDATE_FLIGHT_BACK_SPEED)]
        public static void HandleMovementUpdateFlightBackSpeed(Packet packet)
        {
            WowGuid guid = ReadMovementStats(packet, "MovementStats");
            CreatureSpeedUpdate speedUpdate = new CreatureSpeedUpdate();
            speedUpdate.SpeedType = SpeedType.FlyBack;
            speedUpdate.SpeedRate = packet.ReadSingle("Speed") / MovementInfo.DEFAULT_FLY_BACK_SPEED;
            speedUpdate.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(packet.Time);
            Storage.StoreUnitSpeedUpdate(guid, speedUpdate);
        }

        [Parser(Opcode.SMSG_MOVE_UPDATE_PITCH_RATE)]
        public static void HandleMovementUpdatePitchRate(Packet packet)
        {
            WowGuid guid = ReadMovementStats(packet, "MovementStats");
            CreatureSpeedUpdate speedUpdate = new CreatureSpeedUpdate();
            speedUpdate.SpeedType = SpeedType.Pitch;
            speedUpdate.SpeedRate = packet.ReadSingle("Speed") / MovementInfo.DEFAULT_PITCH_RATE;
            speedUpdate.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(packet.Time);
            Storage.StoreUnitSpeedUpdate(guid, speedUpdate);
        }

        [Parser(Opcode.CMSG_MOVE_SPLINE_DONE)]
        public static void HandleMoveSplineDone(Packet packet)
        {
            ReadMovementStats(packet, "MovementStats");
            packet.ReadInt32("SplineID");
        }

        [Parser(Opcode.SMSG_MOVE_UPDATE_COLLISION_HEIGHT)]
        public static void HandleMoveUpdateCollisionHeight434(Packet packet)
        {
            ReadMovementStats(packet, "MovementStats");
            packet.ReadSingle("Height");
            packet.ReadSingle("Scale");
        }

        [Parser(Opcode.CMSG_MOVE_SET_COLLISION_HEIGHT_ACK)]
        public static void HandleMoveSetCollisionHeightAck(Packet packet)
        {
            ReadMovementAck(packet, "MovementAck");
            packet.ReadSingle("Height");
            packet.ReadInt32("MountDisplayID");

            packet.ResetBitReader();

            packet.ReadBits("Reason", 2);
        }

        [Parser(Opcode.SMSG_MOVE_UPDATE_TELEPORT, ClientVersionBuild.V7_0_3_22248, ClientVersionBuild.V7_2_0_23826)]
        public static void HandleMoveUpdateTeleport(Packet packet)
        {
            WowGuid guid = ReadMovementStats(packet, "MovementStats");

            var int32 = packet.ReadInt32("MovementForcesCount");
            for (int i = 0; i < int32; i++)
                ReadMovementForce(packet, i, "MovementForce");

            packet.ResetBitReader();

            var hasWalkSpeed = packet.ReadBit("HasWalkSpeed");
            var hasRunSpeed = packet.ReadBit("HasRunSpeed");
            var hasRunBackSpeed = packet.ReadBit("HasRunBackSpeed");
            var hasSwimSpeed = packet.ReadBit("HasSwimSpeed");
            var hasSwimBackSpeed = packet.ReadBit("HasSwimBackSpeed");
            var hasFlightSpeed = packet.ReadBit("HasFlightSpeed");
            var hasFlightBackSpeed = packet.ReadBit("HasFlightBackSpeed");
            var hasTurnRate = packet.ReadBit("HasTurnRate");
            var hasPitchRate = packet.ReadBit("HasPitchRate");

            if (hasWalkSpeed)
            {
                CreatureSpeedUpdate speedUpdate = new CreatureSpeedUpdate();
                speedUpdate.SpeedType = SpeedType.Walk;
                speedUpdate.SpeedRate = packet.ReadSingle("WalkSpeed") / MovementInfo.DEFAULT_WALK_SPEED;
                speedUpdate.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(packet.Time);
                Storage.StoreUnitSpeedUpdate(guid, speedUpdate);
            }

            if (hasRunSpeed)
            {
                CreatureSpeedUpdate speedUpdate = new CreatureSpeedUpdate();
                speedUpdate.SpeedType = SpeedType.Run;
                speedUpdate.SpeedRate = packet.ReadSingle("RunSpeed") / MovementInfo.DEFAULT_RUN_SPEED;
                speedUpdate.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(packet.Time);
                Storage.StoreUnitSpeedUpdate(guid, speedUpdate);
            }

            if (hasRunBackSpeed)
            {
                CreatureSpeedUpdate speedUpdate = new CreatureSpeedUpdate();
                speedUpdate.SpeedType = SpeedType.RunBack;
                speedUpdate.SpeedRate = packet.ReadSingle("RunBackSpeed") / MovementInfo.DEFAULT_RUN_BACK_SPEED;
                speedUpdate.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(packet.Time);
                Storage.StoreUnitSpeedUpdate(guid, speedUpdate);
            }

            if (hasSwimSpeed)
            {
                CreatureSpeedUpdate speedUpdate = new CreatureSpeedUpdate();
                speedUpdate.SpeedType = SpeedType.Swim;
                speedUpdate.SpeedRate = packet.ReadSingle("SwimSpeed") / MovementInfo.DEFAULT_SWIM_SPEED;
                speedUpdate.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(packet.Time);
                Storage.StoreUnitSpeedUpdate(guid, speedUpdate);
            }

            if (hasSwimBackSpeed)
            {
                CreatureSpeedUpdate speedUpdate = new CreatureSpeedUpdate();
                speedUpdate.SpeedType = SpeedType.SwimBack;
                speedUpdate.SpeedRate = packet.ReadSingle("SwimBackSpeed") / MovementInfo.DEFAULT_SWIM_BACK_SPEED;
                speedUpdate.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(packet.Time);
                Storage.StoreUnitSpeedUpdate(guid, speedUpdate);
            }

            if (hasFlightSpeed)
            {
                CreatureSpeedUpdate speedUpdate = new CreatureSpeedUpdate();
                speedUpdate.SpeedType = SpeedType.Fly;
                speedUpdate.SpeedRate = packet.ReadSingle("FlightSpeed") / MovementInfo.DEFAULT_FLY_SPEED;
                speedUpdate.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(packet.Time);
                Storage.StoreUnitSpeedUpdate(guid, speedUpdate);
            }

            if (hasFlightBackSpeed)
            {
                CreatureSpeedUpdate speedUpdate = new CreatureSpeedUpdate();
                speedUpdate.SpeedType = SpeedType.FlyBack;
                speedUpdate.SpeedRate = packet.ReadSingle("FlightBackSpeed") / MovementInfo.DEFAULT_FLY_BACK_SPEED;
                speedUpdate.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(packet.Time);
                Storage.StoreUnitSpeedUpdate(guid, speedUpdate);
            }

            if (hasTurnRate)
            {
                CreatureSpeedUpdate speedUpdate = new CreatureSpeedUpdate();
                speedUpdate.SpeedType = SpeedType.Turn;
                speedUpdate.SpeedRate = packet.ReadSingle("TurnRate") / MovementInfo.DEFAULT_TURN_RATE;
                speedUpdate.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(packet.Time);
                Storage.StoreUnitSpeedUpdate(guid, speedUpdate);
            }

            if (hasPitchRate)
            {
                CreatureSpeedUpdate speedUpdate = new CreatureSpeedUpdate();
                speedUpdate.SpeedType = SpeedType.Pitch;
                speedUpdate.SpeedRate = packet.ReadSingle("PitchRate") / MovementInfo.DEFAULT_PITCH_RATE;
                speedUpdate.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(packet.Time);
                Storage.StoreUnitSpeedUpdate(guid, speedUpdate);
            }
        }

        [Parser(Opcode.SMSG_MOVE_UPDATE_TELEPORT, ClientVersionBuild.V7_2_0_23826)]
        public static void HandleMoveUpdateTeleport720(Packet packet)
        {
            WowGuid guid = ReadMovementStats(packet, "MovementStats");

            var movementForcesCount = packet.ReadUInt32("MovementForcesCount");

            packet.ResetBitReader();

            var hasWalkSpeed = packet.ReadBit("HasWalkSpeed");
            var hasRunSpeed = packet.ReadBit("HasRunSpeed");
            var hasRunBackSpeed = packet.ReadBit("HasRunBackSpeed");
            var hasSwimSpeed = packet.ReadBit("HasSwimSpeed");
            var hasSwimBackSpeed = packet.ReadBit("HasSwimBackSpeed");
            var hasFlightSpeed = packet.ReadBit("HasFlightSpeed");
            var hasFlightBackSpeed = packet.ReadBit("HasFlightBackSpeed");
            var hasTurnRate = packet.ReadBit("HasTurnRate");
            var hasPitchRate = packet.ReadBit("HasPitchRate");

            for (int i = 0; i < movementForcesCount; i++)
                ReadMovementForce(packet, i, "MovementForce");

            if (hasWalkSpeed)
            {
                CreatureSpeedUpdate speedUpdate = new CreatureSpeedUpdate();
                speedUpdate.SpeedType = SpeedType.Walk;
                speedUpdate.SpeedRate = packet.ReadSingle("WalkSpeed") / MovementInfo.DEFAULT_WALK_SPEED;
                speedUpdate.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(packet.Time);
                Storage.StoreUnitSpeedUpdate(guid, speedUpdate);
            }

            if (hasRunSpeed)
            {
                CreatureSpeedUpdate speedUpdate = new CreatureSpeedUpdate();
                speedUpdate.SpeedType = SpeedType.Run;
                speedUpdate.SpeedRate = packet.ReadSingle("RunSpeed") / MovementInfo.DEFAULT_RUN_SPEED;
                speedUpdate.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(packet.Time);
                Storage.StoreUnitSpeedUpdate(guid, speedUpdate);
            }

            if (hasRunBackSpeed)
            {
                CreatureSpeedUpdate speedUpdate = new CreatureSpeedUpdate();
                speedUpdate.SpeedType = SpeedType.RunBack;
                speedUpdate.SpeedRate = packet.ReadSingle("RunBackSpeed") / MovementInfo.DEFAULT_RUN_BACK_SPEED;
                speedUpdate.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(packet.Time);
                Storage.StoreUnitSpeedUpdate(guid, speedUpdate);
            }

            if (hasSwimSpeed)
            {
                CreatureSpeedUpdate speedUpdate = new CreatureSpeedUpdate();
                speedUpdate.SpeedType = SpeedType.Swim;
                speedUpdate.SpeedRate = packet.ReadSingle("SwimSpeed") / MovementInfo.DEFAULT_SWIM_SPEED;
                speedUpdate.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(packet.Time);
                Storage.StoreUnitSpeedUpdate(guid, speedUpdate);
            }

            if (hasSwimBackSpeed)
            {
                CreatureSpeedUpdate speedUpdate = new CreatureSpeedUpdate();
                speedUpdate.SpeedType = SpeedType.SwimBack;
                speedUpdate.SpeedRate = packet.ReadSingle("SwimBackSpeed") / MovementInfo.DEFAULT_SWIM_BACK_SPEED;
                speedUpdate.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(packet.Time);
                Storage.StoreUnitSpeedUpdate(guid, speedUpdate);
            }

            if (hasFlightSpeed)
            {
                CreatureSpeedUpdate speedUpdate = new CreatureSpeedUpdate();
                speedUpdate.SpeedType = SpeedType.Fly;
                speedUpdate.SpeedRate = packet.ReadSingle("FlightSpeed") / MovementInfo.DEFAULT_FLY_SPEED;
                speedUpdate.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(packet.Time);
                Storage.StoreUnitSpeedUpdate(guid, speedUpdate);
            }

            if (hasFlightBackSpeed)
            {
                CreatureSpeedUpdate speedUpdate = new CreatureSpeedUpdate();
                speedUpdate.SpeedType = SpeedType.FlyBack;
                speedUpdate.SpeedRate = packet.ReadSingle("FlightBackSpeed") / MovementInfo.DEFAULT_FLY_BACK_SPEED;
                speedUpdate.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(packet.Time);
                Storage.StoreUnitSpeedUpdate(guid, speedUpdate);
            }

            if (hasTurnRate)
            {
                CreatureSpeedUpdate speedUpdate = new CreatureSpeedUpdate();
                speedUpdate.SpeedType = SpeedType.Turn;
                speedUpdate.SpeedRate = packet.ReadSingle("TurnRate") / MovementInfo.DEFAULT_TURN_RATE;
                speedUpdate.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(packet.Time);
                Storage.StoreUnitSpeedUpdate(guid, speedUpdate);
            }

            if (hasPitchRate)
            {
                CreatureSpeedUpdate speedUpdate = new CreatureSpeedUpdate();
                speedUpdate.SpeedType = SpeedType.Pitch;
                speedUpdate.SpeedRate = packet.ReadSingle("PitchRate") / MovementInfo.DEFAULT_PITCH_RATE;
                speedUpdate.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(packet.Time);
                Storage.StoreUnitSpeedUpdate(guid, speedUpdate);
            }
        }

        [Parser(Opcode.SMSG_MOVE_TELEPORT)]
        public static void HandleMoveTeleport(Packet packet)
        {
            packet.ReadPackedGuid128("MoverGUID");
            packet.ReadInt32("SequenceIndex");
            packet.ReadVector3("Position");
            packet.ReadSingle("Facing");
            packet.ReadByte("PreloadWorld");

            var hasVehicleTeleport = packet.ReadBit("HasVehicleTeleport");
            var hasTransport = packet.ReadBit("HasTransport");

            // VehicleTeleport
            if (hasVehicleTeleport)
            {
                packet.ReadByte("VehicleSeatIndex");
                packet.ReadBit("VehicleExitVoluntary");
                packet.ReadBit("VehicleExitTeleport");
            }

            if (hasTransport)
                packet.ReadPackedGuid128("TransportGUID");
        }

        [HasSniffData]
        [Parser(Opcode.SMSG_NEW_WORLD)]
        public static void HandleNewWorld(Packet packet)
        {
            WowPacketParser.Parsing.Parsers.MovementHandler.CurrentMapId = (uint)packet.ReadInt32<MapId>("Map");
            packet.ReadVector4("Position");
            packet.ReadUInt32("Reason");
            packet.ReadVector3("MovementOffset");

            Storage.ClearDataOnMapChange();
            packet.AddSniffData(StoreNameType.Map, (int)WowPacketParser.Parsing.Parsers.MovementHandler.CurrentMapId, "NEW_WORLD");
        }

        [Parser(Opcode.SMSG_MOVE_ENABLE_DOUBLE_JUMP)]
        public static void HandleMoveEnableDoubleJump(Packet packet)
        {
            packet.ReadPackedGuid128("MoverGUID");
            packet.ReadUInt32("SequenceId");
        }


        [Parser(Opcode.SMSG_MOVE_SET_COLLISION_HEIGHT, ClientVersionBuild.V7_1_0_22900)]
        public static void HandleSetCollisionHeight(Packet packet)
        {
            packet.ReadPackedGuid128("MoverGUID");
            packet.ReadInt32("SequenceIndex");
            packet.ReadSingle("Height");
            packet.ReadSingle("Scale");
            packet.ReadUInt32("MountDisplayID");
            packet.ReadInt32("ScaleDuration");

            packet.ResetBitReader();

            packet.ReadBits("Reason", 2);
        }

        [Parser(Opcode.SMSG_TRANSFER_PENDING)]
        public static void HandleTransferPending(Packet packet)
        {
            packet.ReadInt32<MapId>("MapID");
            if (ClientVersion.AddedInVersion(ClientVersionBuild.V7_3_0_24920))
                packet.ReadVector3("OldMapPosition");

            packet.ResetBitReader();

            var hasShipTransferPending = packet.ReadBit();
            var hasTransferSpell = packet.ReadBit();

            if (hasShipTransferPending)
            {
                packet.ReadUInt32<GOId>("ID");
                packet.ReadInt32<MapId>("OriginMapID");
            }

            if (hasTransferSpell)
                packet.ReadUInt32<SpellId>("TransferSpellID");
        }

        [Parser(Opcode.CMSG_MOVE_SET_VEHICLE_REC_ID_ACK)]
        public static void HandleMoveSetVehicleRecIdAck(Packet packet)
        {
            ReadMovementAck(packet);
            packet.ReadInt32("VehicleRecID");
        }

        [Parser(Opcode.SMSG_PHASE_SHIFT_CHANGE)]
        public static void HandlePhaseShift(Packet packet)
        {
            CoreParsers.MovementHandler.ActivePhases.Clear();

            packet.ReadPackedGuid128("Client");

            // PhaseShiftData
            packet.ReadInt32("PhaseShiftFlags");
            var count = packet.ReadInt32("PhaseShiftCount");
            packet.ReadPackedGuid128("PersonalGUID");
            for (var i = 0; i < count; ++i)
            {
                var flags = packet.ReadUInt16E<PhaseFlags>("PhaseFlags", i);
                var id = packet.ReadUInt16();

                if (Settings.UseDBC && DBC.Phase.ContainsKey(id))
                {
                    packet.WriteLine($"[{i}] ID: {id} ({(DBCPhaseFlags)DBC.Phase[id].Flags})");
                }
                else
                    packet.AddValue("ID", id, i);

                CoreParsers.MovementHandler.ActivePhases.Add(id, true);
            }

            if (DBC.Phases.Any())
            {
                foreach (var phaseGroup in DBC.GetPhaseGroups(CoreParsers.MovementHandler.ActivePhases.Keys))
                    packet.WriteLine($"PhaseGroup: { phaseGroup } Phases: { string.Join(" - ", DBC.Phases[phaseGroup]) }");
            }

            var visibleMapIDsCount = packet.ReadInt32("VisibleMapIDsCount") / 2;
            for (var i = 0; i < visibleMapIDsCount; ++i)
                packet.ReadInt16<MapId>("VisibleMapID", i);

            var preloadMapIDCount = packet.ReadInt32("PreloadMapIDsCount") / 2;
            for (var i = 0; i < preloadMapIDCount; ++i)
                packet.ReadInt16<MapId>("PreloadMapID", i);

            var uiWorldMapAreaIDSwapsCount = packet.ReadInt32("UiWorldMapAreaIDSwap") / 2;
            for (var i = 0; i < uiWorldMapAreaIDSwapsCount; ++i)
                packet.ReadInt16("UiWorldMapAreaIDSwaps", i);
        }

        [Parser(Opcode.SMSG_TRANSFER_ABORTED)]
        public static void HandleTransferAborted(Packet packet)
        {
            packet.ReadInt32<MapId>("MapID");
            packet.ReadByte("Arg");
            packet.ReadInt32("MapDifficultyXConditionID");
            if (ClientVersion.AddedInVersion(9, 1, 0, 1, 14, 0, 2, 5, 1))
                packet.ReadBitsE<TransferAbortReason>("TransfertAbort", 6);
            else
                packet.ReadBitsE<TransferAbortReason>("TransfertAbort", 5);
        }
    }
}
