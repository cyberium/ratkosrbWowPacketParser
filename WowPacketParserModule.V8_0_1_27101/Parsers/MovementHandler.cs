﻿using System.Linq;
using WowPacketParser.DBC;
using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Store;
using WowPacketParser.Store.Objects;
using WowPacketParser.Parsing;
using CoreParsers = WowPacketParser.Parsing.Parsers;
using SplineFlag = WowPacketParserModule.V7_0_3_22248.Enums.SplineFlag;
using System.Collections.Generic;

namespace WowPacketParserModule.V8_0_1_27101.Parsers
{
    public static class MovementHandler
    {
        public static void ReadMonsterSplineFilter(Packet packet, params object[] indexes)
        {
            var count = packet.ReadUInt32("MonsterSplineFilterKey", indexes);
            packet.ReadSingle("BaseSpeed", indexes);
            packet.ReadInt16("StartOffset", indexes);
            packet.ReadSingle("DistToPrevFilterKey", indexes);
            packet.ReadInt16("AddedToStart", indexes);

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
            packet.ReadSingle("JumpGravity", indexes);
        }

        public static void ReadMonsterSplineJumpExtraData(Packet packet, params object[] indexes)
        {
            packet.ReadSingle("JumpGravity", indexes);
            packet.ReadUInt32("StartTime", indexes);
            packet.ReadUInt32("Duration", indexes);
        }

        public static void ReadMovementSpline(CreatureMovement savedata, Packet packet, Vector3 pos, params object[] indexes)
        {
            uint splineflags = (uint)packet.ReadUInt32E<SplineFlag>("Flags", indexes);
            if (savedata != null)
                savedata.SplineFlags = splineflags;
            packet.ReadByte("AnimTier", indexes);
            packet.ReadUInt32("TierTransStartTime", indexes);
            packet.ReadInt32("Elapsed", indexes);
            uint movetime = packet.ReadUInt32("MoveTime", indexes);
            if (savedata != null)
                savedata.MoveTime = movetime;
            packet.ReadUInt32("FadeObjectTime", indexes);

            packet.ReadByte("Mode", indexes);
            packet.ReadByte("VehicleExitVoluntary", indexes);

            packet.ReadPackedGuid128("TransportGUID", indexes);
            packet.ReadSByte("VehicleSeat", indexes);

            packet.ResetBitReader();

            var type = packet.ReadBits("Face", 2, indexes);
            var pointsCount = packet.ReadBits("PointsCount", 16, indexes);
            var packedDeltasCount = packet.ReadBits("PackedDeltasCount", 16, indexes);
            var hasSplineFilter = packet.ReadBit("HasSplineFilter", indexes);
            var hasSpellEffectExtraData = packet.ReadBit("HasSpellEffectExtraData", indexes);
            var hasJumpExtraData = packet.ReadBit("HasJumpExtraData", indexes);

            if (hasSplineFilter)
                ReadMonsterSplineFilter(packet, indexes, "MonsterSplineFilter");

            float orientation = 100;
            switch (type)
            {
                case 1:
                    var faceSpot = packet.ReadVector3("FaceSpot", indexes);
                    orientation = CreatureMovement.GetAngle(pos.X, pos.Y, faceSpot.X, faceSpot.Y);
                    break;
                case 2:
                    orientation = packet.ReadSingle("FaceDirection", indexes);
                    packet.ReadPackedGuid128("FacingGUID", indexes);
                    break;
                case 3:
                    orientation = packet.ReadSingle("FaceDirection", indexes);
                    break;
                default:
                    break;
            }

            if (savedata != null)
            {
                savedata.Orientation = orientation;
                savedata.SplineCount = pointsCount;
                if (pointsCount > 0)
                    savedata.SplinePoints = new List<Vector3>();
            }

            Vector3 endpos = new Vector3();
            for (int i = 0; i < pointsCount; i++)
            {
                var spot = packet.ReadVector3();

                // client always taking first point
                if (i == 0)
                    endpos = spot;

                if (savedata != null)
                    savedata.SplinePoints.Add(spot);

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

            if (hasJumpExtraData)
                ReadMonsterSplineJumpExtraData(packet, "MonsterSplineJumpExtraData");

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
                packet.AddValue("WayPoints", vec, indexes, i);
            }
        }

        public static void ReadMovementMonsterSpline(CreatureMovement savedata, Packet packet, Vector3 pos, params object[] indexes)
        {
            packet.ReadUInt32("Id", indexes);
            packet.ReadVector3("Destination", indexes);

            packet.ResetBitReader();

            packet.ReadBit("CrzTeleport", indexes);
            packet.ReadBits("StopDistanceTolerance", 3, indexes);

            ReadMovementSpline(savedata, packet, pos, indexes, "MovementSpline");
        }

        [Parser(Opcode.SMSG_ON_MONSTER_MOVE)]
        public static void HandleOnMonsterMove(Packet packet)
        {
            WowGuid guid = packet.ReadPackedGuid128("MoverGUID");
            var pos = packet.ReadVector3("Position");

            Unit obj = null;
            CreatureMovement movementData = null;
            if (guid.GetHighType() == HighGuidType.Creature && Storage.Objects != null && Storage.Objects.ContainsKey(guid))
            {
                obj = Storage.Objects[guid].Item1 as Unit;
                if ((obj.UnitData.Flags & (uint)UnitFlags.IsInCombat) == 0) // movement could be because of aggro so ignore that
                {
                    obj.Movement.HasWpsOrRandMov = true;
                    movementData = new CreatureMovement();
                }
            }

            ReadMovementMonsterSpline(movementData, packet, pos, "MovementMonsterSpline");

            if (movementData != null)
                obj.AddWaypoint(movementData, pos, packet.Time);
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
                var flags = packet.ReadUInt16("PhaseFlags", i);
                var id = packet.ReadUInt16("Id", i);
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
            var uiMapPhaseIdCount = packet.ReadInt32("UiMapPhaseIDsCount") / 2;
            for (var i = 0; i < uiMapPhaseIdCount; ++i)
                packet.ReadInt16("UiMapPhaseId", i);
        }
    }
}
