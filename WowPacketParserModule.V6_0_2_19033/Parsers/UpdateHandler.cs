using System.Collections;
using System.Collections.Generic;
using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;
using WowPacketParser.Store;
using WowPacketParser.Store.Objects;
using CoreParsers = WowPacketParser.Parsing.Parsers;

namespace WowPacketParserModule.V6_0_2_19033.Parsers
{
    public static class UpdateHandler
    {
        [HasSniffData] // in ReadCreateObjectBlock
        [Parser(Opcode.SMSG_UPDATE_OBJECT)]
        public static void HandleUpdateObject(Packet packet)
        {
            var count = packet.ReadUInt32("NumObjUpdates");
            uint map = packet.ReadUInt16<MapId>("MapID");
            packet.ResetBitReader();
            var hasRemovedObjects = packet.ReadBit("HasRemovedObjects");
            if (hasRemovedObjects)
            {
                var destroyedObjCount = packet.ReadInt16("DestroyedObjCount");
                var removedObjCount = packet.ReadUInt32("RemovedObjCount"); // destroyed + out of range
                var outOfRangeObjCount = removedObjCount - destroyedObjCount;

                for (var i = 0; i < destroyedObjCount; i++)
                {
                    WowGuid guid = packet.ReadPackedGuid128("ObjectGUID", "Destroyed", i);
                    Storage.StoreObjectDestroyTime(guid, packet.Time);
                }
                for (var i = 0; i < outOfRangeObjCount; i++)
                {
                    WowGuid guid = packet.ReadPackedGuid128("ObjectGUID", "OutOfRange", i);
                    Storage.StoreObjectDestroyTime(guid, packet.Time);
                }
            }
            packet.ReadUInt32("Data size");

            for (var i = 0; i < count; i++)
            {
                var type = packet.ReadByte();
                var typeString = ((UpdateTypeCataclysm)type).ToString();

                packet.AddValue("UpdateType", typeString, i);
                switch (typeString)
                {
                    case "Values":
                    {
                        var guid = packet.ReadPackedGuid128("Object Guid", i);
                        CoreParsers.UpdateHandler.ReadValuesUpdateBlock(packet, guid, i);
                        break;
                    }
                    case "CreateObject1":
                    {
                        var guid = packet.ReadPackedGuid128("Object Guid", i);
                        ReadCreateObjectBlock(packet, guid, map, i, ObjectCreateType.Create1);
                        break;
                    }
                    case "CreateObject2": // Might != CreateObject1 on Cata
                    {
                        var guid = packet.ReadPackedGuid128("Object Guid", i);
                        ReadCreateObjectBlock(packet, guid, map, i, ObjectCreateType.Create2);
                        break;
                    }
                }
            }
        }

        private static void ReadCreateObjectBlock(Packet packet, WowGuid guid, uint map, object index, ObjectCreateType type)
        {
            ObjectType objType = ObjectTypeConverter.Convert(packet.ReadByteE<ObjectTypeLegacy>("Object Type", index));

            WoWObject obj;
            if (Storage.Objects.ContainsKey(guid))
                obj = Storage.Objects[guid].Item1;
            else
            {
                switch (objType)
                {
                    case ObjectType.Unit:
                        obj = new Unit();
                        break;
                    case ObjectType.GameObject:
                        obj = new GameObject();
                        break;
                    case ObjectType.DynamicObject:
                        obj = new DynamicObject();
                        break;
                    case ObjectType.Player:
                        obj = new Player();
                        break;
                    default:
                        obj = new WoWObject();
                        break;
                }
            }

            var moves = ReadMovementUpdateBlock(packet, guid, obj, index);
            Storage.StoreObjectCreateTime(guid, map, moves, packet.Time, type);

            BitArray updateMaskArray = null;
            var updates = CoreParsers.UpdateHandler.ReadValuesUpdateBlockOnCreate(packet, objType, index, out updateMaskArray);
            var dynamicUpdates = CoreParsers.UpdateHandler.ReadDynamicValuesUpdateBlockOnCreate(packet, objType, index);

            // If this is the second time we see the same object (same guid,
            // same position) update its phasemask
            if (Storage.Objects.ContainsKey(guid))
            {
                CoreParsers.UpdateHandler.ProcessExistingObject(ref obj, guid, packet, updateMaskArray, updates, dynamicUpdates, moves); // can't do "ref Storage.Objects[guid].Item1 directly
            }
            else
            {
                obj.Type = objType;
                obj.Movement = moves;
                obj.UpdateFields = updates;
                obj.DynamicUpdateFields = dynamicUpdates;
                obj.Map = map;
                obj.Area = CoreParsers.WorldStateHandler.CurrentAreaId;
                obj.Zone = CoreParsers.WorldStateHandler.CurrentZoneId;
                obj.PhaseMask = (uint)CoreParsers.MovementHandler.CurrentPhaseMask;
                obj.Phases = new HashSet<ushort>(CoreParsers.MovementHandler.ActivePhases.Keys);
                obj.DifficultyID = CoreParsers.MovementHandler.CurrentDifficultyID;
                Storage.StoreNewObject(guid, obj, type, packet);
            }   

            if (guid.HasEntry() && (objType == ObjectType.Unit || objType == ObjectType.GameObject))
                packet.AddSniffData(Utilities.ObjectTypeToStore(objType), (int)guid.GetEntry(), "SPAWN");
        }

        private static MovementInfo ReadMovementUpdateBlock(Packet packet, WowGuid guid, WoWObject obj, object index)
        {
            var moveInfo = new MovementInfo();

            packet.ResetBitReader();

            packet.ReadBit("NoBirthAnim", index);
            packet.ReadBit("EnablePortals", index);
            moveInfo.Hover = packet.ReadBit("PlayHoverAnim", index);
            packet.ReadBit("IsSuppressingGreetings", index);

            var hasMovementUpdate = packet.ReadBit("HasMovementUpdate", index);
            var hasMovementTransport = packet.ReadBit("HasMovementTransport", index);
            var hasStationaryPosition = packet.ReadBit("Stationary", index);
            var hasCombatVictim = packet.ReadBit("HasCombatVictim", index);
            var hasServerTime = packet.ReadBit("HasServerTime", index);
            var hasVehicleCreate = packet.ReadBit("HasVehicleCreate", index);
            var hasAnimKitCreate = packet.ReadBit("HasAnimKitCreate", index);
            var hasRotation = packet.ReadBit("HasRotation", index);
            var hasAreaTrigger = packet.ReadBit("HasAreaTrigger", index);
            var hasGameObject = packet.ReadBit("HasGameObject", index);

            var isSelf = packet.ReadBit("ThisIsYou", index);
            if (isSelf)
                Storage.SetCurrentActivePlayer(guid, packet.Time);

            packet.ReadBit("ReplaceActive", index);

            var sceneObjCreate = packet.ReadBit("SceneObjCreate", index);
            var scenePendingInstances = packet.ReadBit("ScenePendingInstances", index);

            var pauseTimesCount = packet.ReadUInt32("PauseTimesCount", index);

            if (hasMovementUpdate) // 392
            {
                ReadMovementStatusData(moveInfo, packet, index);

                moveInfo.WalkSpeed = packet.ReadSingle("WalkSpeed", index);
                moveInfo.RunSpeed = packet.ReadSingle("RunSpeed", index);
                moveInfo.RunBackSpeed = packet.ReadSingle("RunBackSpeed", index);
                moveInfo.SwimSpeed = packet.ReadSingle("SwimSpeed", index);
                moveInfo.SwimBackSpeed = packet.ReadSingle("SwimBackSpeed", index);
                moveInfo.FlightSpeed = packet.ReadSingle("FlightSpeed", index);
                moveInfo.FlightBackSpeed = packet.ReadSingle("FlightBackSpeed", index);
                moveInfo.TurnRate = packet.ReadSingle("TurnRate", index);
                moveInfo.PitchRate = packet.ReadSingle("PitchRate", index);

                var movementForceCount = packet.ReadInt32("MovementForceCount", index);

                for (var i = 0; i < movementForceCount; ++i)
                {
                    packet.ReadPackedGuid128("Id", index);
                    packet.ReadVector3("Direction", index);
                    if (ClientVersion.AddedInVersion(ClientVersionBuild.V6_1_2_19802)) // correct?
                        packet.ReadVector3("TransportPosition", index);
                    packet.ReadInt32("TransportID", index);
                    packet.ReadSingle("Magnitude", index);
                    packet.ReadByte("Type", index);
                }

                packet.ResetBitReader();

                moveInfo.HasSplineData = packet.ReadBit("HasMovementSpline", index);

                if (moveInfo.HasSplineData)
                {
                    packet.ReadInt32("ID", index);
                    packet.ReadVector3("Destination", index);

                    packet.ResetBitReader();

                    var hasMovementSplineMove = packet.ReadBit("MovementSplineMove", index);
                    if (hasMovementSplineMove)
                    {
                        packet.ResetBitReader();

                        ServerSideMovement movementData = new ServerSideMovement();
                        movementData.SplineFlags = (uint)packet.ReadBitsE<SplineFlag434>("SplineFlags", ClientVersion.AddedInVersion(ClientVersionBuild.V6_2_0_20173) ? 28 : 25, index);
                        var face = packet.ReadBits("Face", 2, index);

                        var hasJumpGravity = packet.ReadBit("HasJumpGravity", index);
                        var hasSpecialTime = packet.ReadBit("HasSpecialTime", index);

                        packet.ReadBitsE<SplineMode>("Mode", 2, index);

                        var hasSplineFilterKey = packet.ReadBit("HasSplineFilterKey", index);

                        packet.ReadUInt32("Elapsed", index);
                        movementData.MoveTime = packet.ReadUInt32("Duration", index);

                        packet.ReadSingle("DurationModifier", index);
                        packet.ReadSingle("NextDurationModifier", index);

                        var pointsCount = packet.ReadUInt32("PointsCount", index);
                        movementData.SplineCount = pointsCount;
                        if (pointsCount > 0)
                            movementData.SplinePoints = new List<Vector3>();

                        float orientation = 100;
                        switch (face)
                        {
                            case 1:
                                var faceSpot = packet.ReadVector3("FaceSpot", index);
                                orientation = Utilities.GetAngle(moveInfo.Position.X, moveInfo.Position.Y, faceSpot.X, faceSpot.Y);
                                break;
                            case 2:
                                packet.ReadPackedGuid128("FaceGUID", index);
                                break;
                            case 3:
                                orientation = packet.ReadSingle("FaceDirection", index);
                                break;
                            default:
                                break;
                        }
                        movementData.Orientation = orientation;

                        if (hasJumpGravity)
                            packet.ReadSingle("JumpGravity", index);

                        if (hasSpecialTime)
                            packet.ReadInt32("SpecialTime", index);

                        if (hasSplineFilterKey)
                        {
                            var filterKeysCount = packet.ReadUInt32("FilterKeysCount", index);
                            for (var i = 0; i < filterKeysCount; ++i)
                            {
                                packet.ReadSingle("In", index, i);
                                packet.ReadSingle("Out", index, i);
                            }

                            if (ClientVersion.AddedInVersion(ClientVersionBuild.V6_2_0_20173))
                                packet.ResetBitReader();

                            packet.ReadBits("FilterFlags", 2, index);
                        }

                        for (var i = 0; i < pointsCount; ++i)
                        {
                            var spot = packet.ReadVector3("Points", index, i);
                            movementData.SplinePoints.Add(spot);
                        }

                        if (pointsCount > 0 && (Settings.SaveTransports || (moveInfo.TransportGuid == null || moveInfo.TransportGuid.IsEmpty())))
                        {
                            if (moveInfo.TransportGuid != null)
                                movementData.TransportGuid = moveInfo.TransportGuid;

                            Unit unit = obj as Unit;
                            if (unit != null)
                                unit.AddWaypoint(movementData, moveInfo.Position, packet.Time);
                        }
                    }
                }
            }

            if (hasMovementTransport) // 456
            {
                moveInfo.TransportGuid = packet.ReadPackedGuid128("TransportGUID", index);
                moveInfo.TransportOffset = packet.ReadVector4("TransportPosition", index);
                var seat = packet.ReadByte("VehicleSeatIndex", index);
                packet.ReadUInt32("MoveTime", index);

                packet.ResetBitReader();

                var hasPrevMoveTime = packet.ReadBit("HasPrevMoveTime", index);
                var hasVehicleRecID = packet.ReadBit("HasVehicleRecID", index);

                if (hasPrevMoveTime)
                    packet.ReadUInt32("PrevMoveTime", index);

                if (hasVehicleRecID)
                    packet.ReadInt32("VehicleRecID", index);

                if (moveInfo.TransportGuid.HasEntry() && moveInfo.TransportGuid.GetHighType() == HighGuidType.Vehicle &&
                    guid.HasEntry() && guid.GetHighType() == HighGuidType.Creature)
                {
                    VehicleTemplateAccessory vehicleAccessory = new VehicleTemplateAccessory
                    {
                        Entry = moveInfo.TransportGuid.GetEntry(),
                        AccessoryEntry = guid.GetEntry(),
                        SeatId = seat
                    };
                    Storage.VehicleTemplateAccessories.Add(vehicleAccessory, packet.TimeSpan);
                }
            }

            if (hasStationaryPosition) // 480
            {
                moveInfo.Position = packet.ReadVector3();
                moveInfo.Orientation = packet.ReadSingle();

                packet.AddValue("Stationary Position", moveInfo.Position, index);
                packet.AddValue("Stationary Orientation", moveInfo.Orientation, index);
            }

            if (hasCombatVictim) // 504
                packet.ReadPackedGuid128("CombatVictim Guid", index);

            if (hasServerTime) // 516
                moveInfo.TransportPathTimer = packet.ReadUInt32("ServerTime", index);

            if (hasVehicleCreate) // 528
            {
                moveInfo.VehicleId = packet.ReadUInt32("RecID", index);
                packet.ReadSingle("InitialRawFacing", index);
            }

            if (hasAnimKitCreate) // 538
            {
                packet.ReadUInt16("AiID", index);
                packet.ReadUInt16("MovementID", index);
                packet.ReadUInt16("MeleeID", index);
            }

            if (hasRotation) // 552
                moveInfo.Rotation = packet.ReadPackedQuaternion("GameObject Rotation", index);

            if (hasAreaTrigger) // 772
            {
                // CliAreaTrigger
                packet.ReadInt32("ElapsedMs", index);

                packet.ReadVector3("RollPitchYaw1", index);

                packet.ResetBitReader();

                packet.ReadBit("HasAbsoluteOrientation", index);
                packet.ReadBit("HasDynamicShape", index);
                packet.ReadBit("HasAttached", index);
                packet.ReadBit("HasFaceMovementDir", index);
                packet.ReadBit("HasFollowsTerrain", index);

                if (ClientVersion.AddedInVersion(ClientVersionBuild.V6_2_0_20173))
                    packet.ReadBit("Unk bit WoD62x", index);

                var hasTargetRollPitchYaw = packet.ReadBit("HasTargetRollPitchYaw", index);
                var hasScaleCurveID = packet.ReadBit("HasScaleCurveID", index);
                var hasMorphCurveID = packet.ReadBit("HasMorphCurveID", index);
                var hasFacingCurveID = packet.ReadBit("HasFacingCurveID", index);
                var hasMoveCurveID = packet.ReadBit("HasMoveCurveID", index);
                var hasAreaTriggerSphere = packet.ReadBit("HasAreaTriggerSphere", index);
                var hasAreaTriggerBox = packet.ReadBit("HasAreaTriggerBox", index);
                var hasAreaTriggerPolygon = packet.ReadBit("HasAreaTriggerPolygon", index);
                var hasAreaTriggerCylinder = packet.ReadBit("HasAreaTriggerCylinder", index);
                var hasAreaTriggerSpline = packet.ReadBit("HasAreaTriggerSpline", index);

                if (hasTargetRollPitchYaw)
                    packet.ReadVector3("TargetRollPitchYaw", index);

                if (hasScaleCurveID)
                    packet.ReadInt32("ScaleCurveID, index");

                if (hasMorphCurveID)
                    packet.ReadInt32("MorphCurveID", index);

                if (hasFacingCurveID)
                    packet.ReadInt32("FacingCurveID", index);

                if (hasMoveCurveID)
                    packet.ReadInt32("MoveCurveID", index);

                if (hasAreaTriggerSphere)
                {
                    packet.ReadSingle("Radius", index);
                    packet.ReadSingle("RadiusTarget", index);
                }

                if (hasAreaTriggerBox)
                {
                    packet.ReadVector3("Extents", index);
                    packet.ReadVector3("ExtentsTarget", index);
                }

                if (hasAreaTriggerPolygon)
                {
                    var verticesCount = packet.ReadInt32("VerticesCount", index);
                    var verticesTargetCount = packet.ReadInt32("VerticesTargetCount", index);
                    packet.ReadSingle("Height", index);
                    packet.ReadSingle("HeightTarget", index);

                    for (var i = 0; i < verticesCount; ++i)
                        packet.ReadVector2("Vertices", index, i);

                    for (var i = 0; i < verticesTargetCount; ++i)
                        packet.ReadVector2("VerticesTarget", index, i);
                }

                if (hasAreaTriggerCylinder)
                {
                    packet.ReadSingle("Radius", index);
                    packet.ReadSingle("RadiusTarget", index);
                    packet.ReadSingle("Height", index);
                    packet.ReadSingle("HeightTarget", index);
                    packet.ReadSingle("Float4", index);
                    packet.ReadSingle("Float5", index);
                }

                if (hasAreaTriggerSpline)
                    AreaTriggerHandler.ReadAreaTriggerSpline(packet, index);
            }

            if (hasGameObject) // 788
            {
                packet.ReadInt32("WorldEffectID", index);

                packet.ResetBitReader();

                var bit8 = packet.ReadBit("bit8", index);
                if (bit8)
                    packet.ReadInt32("Int1", index);
            }

            if (sceneObjCreate) // 1184
            {
                packet.ResetBitReader();

                var hasSceneLocalScriptData = packet.ReadBit("HasSceneLocalScriptData", index);
                var petBattleFullUpdate = packet.ReadBit("HasPetBattleFullUpdate", index);

                if (hasSceneLocalScriptData)
                {
                    packet.ResetBitReader();
                    var dataLength = packet.ReadBits(7);
                    packet.ReadWoWString("Data", dataLength, index);
                }

                if (petBattleFullUpdate)
                    BattlePetHandler.ReadPetBattleFullUpdate(packet, index);
            }

            if (scenePendingInstances) // 1208
            {
                var sceneInstanceIDs = packet.ReadInt32("SceneInstanceIDsCount");

                for (var i = 0; i < sceneInstanceIDs; ++i)
                    packet.ReadInt32("SceneInstanceIDs", index, i);
            }

            for (var i = 0; i < pauseTimesCount; ++i)
                packet.ReadInt32("PauseTimes", index, i);

            return moveInfo;
        }

        private static void ReadMovementStatusData(MovementInfo moveInfo, Packet packet, object index)
        {
            packet.ReadPackedGuid128("MoverGUID", index);

            moveInfo.MoveTime = packet.ReadUInt32("MoveTime", index);
            moveInfo.Position = packet.ReadVector3("Position", index);
            moveInfo.Orientation = packet.ReadSingle("Orientation", index);

            moveInfo.SwimPitch = packet.ReadSingle("Pitch", index);
            moveInfo.SplineElevation = packet.ReadSingle("StepUpStartElevation", index);

            var removeForcesIDsCount = packet.ReadInt32();
            packet.ReadInt32("MoveIndex", index);

            for (var i = 0; i < removeForcesIDsCount; i++)
                packet.ReadPackedGuid128("RemoveForcesIDs", index, i);

            packet.ResetBitReader();

            moveInfo.Flags = (uint)(MovementFlag)packet.ReadBitsE<Enums.MovementFlag>("Movement Flags", 30, index);
            moveInfo.FlagsExtra = (uint)packet.ReadBitsE<MovementFlagExtra>("Extra Movement Flags", ClientVersion.AddedInVersion(ClientVersionBuild.V6_2_0_20173) ? 16 : 15, index);

            var hasTransport = packet.ReadBit("Has Transport Data", index);
            var hasFall = packet.ReadBit("Has Fall Data", index);
            packet.ReadBit("HasSpline", index);
            packet.ReadBit("HeightChangeFailed", index);
            packet.ReadBit("RemoteTimeValid", index);

            if (hasTransport)
            {
                moveInfo.TransportGuid = packet.ReadPackedGuid128("Transport Guid", index);
                moveInfo.TransportOffset = packet.ReadVector4("Transport Position", index);
                packet.ReadSByte("Transport Seat", index);
                packet.ReadInt32("Transport Time", index);

                packet.ResetBitReader();

                var hasPrevMoveTime = packet.ReadBit("HasPrevMoveTime", index);
                var hasVehicleRecID = packet.ReadBit("HasVehicleRecID", index);

                if (hasPrevMoveTime)
                    packet.ReadUInt32("PrevMoveTime", index);

                if (hasVehicleRecID)
                    packet.ReadUInt32("VehicleRecID", index);
            }

            if (hasFall)
            {
                moveInfo.FallTime = packet.ReadUInt32("Jump Fall Time", index);
                moveInfo.JumpVerticalSpeed = packet.ReadSingle("Jump Vertical Speed", index);

                packet.ResetBitReader();
                var bit20 = packet.ReadBit("Has Fall Direction", index);
                if (bit20)
                {
                    moveInfo.JumpSinAngle = packet.ReadSingle("Jump Sin Angle", index);
                    moveInfo.JumpCosAngle = packet.ReadSingle("Jump Cos Angle", index);
                    moveInfo.JumpHorizontalSpeed = packet.ReadSingle("Jump Horizontal Speed", index);
                }
            }
        }

        [Parser(Opcode.SMSG_DESTROY_ARENA_UNIT)]
        public static void HandleDestroyArenaUnit(Packet packet)
        {
            packet.ReadPackedGuid128("Guid");
        }

        [Parser(Opcode.SMSG_MAP_OBJ_EVENTS)]
        public static void HandleMapObjEvents(Packet packet)
        {
            packet.ReadInt32("UniqueID");
            packet.ReadInt32("DataSize");

            var count = packet.ReadByte("Unk1");
            for (var i = 0; i < count; i++)
            {
                var byte20 = packet.ReadByte("Unk2", i);
                packet.ReadInt32(byte20 == 1 ? "Unk3" : "Unk4", i);
            }
        }

        [Parser(Opcode.SMSG_SET_ANIM_TIER)]
        public static void HandleSetAnimTier(Packet packet)
        {
            packet.ReadPackedGuid128("Unit");
            packet.ReadBits("Tier", 3);
        }

        [Parser(Opcode.CMSG_OBJECT_UPDATE_FAILED)]
        [Parser(Opcode.CMSG_OBJECT_UPDATE_RESCUED)]
        public static void HandleObjectUpdateOrRescued(Packet packet)
        {
            if (!ClientVersion.AddedInVersion(ClientVersionBuild.V6_1_0_19702))
                packet.ReadPackedGuid128("ObjectGUID");
        }
    }
}
