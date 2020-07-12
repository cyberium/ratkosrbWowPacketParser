using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Store.Objects.UpdateFields;
using WowPacketParser.Store.Objects.UpdateFields.LegacyImplementation;

namespace WowPacketParser.Store.Objects
{
    public sealed class Unit : WoWObject
    {
        public List<Aura> Auras;

        public BlockingCollection<List<Aura>> AddedAuras = new BlockingCollection<List<Aura>>();

        public uint globalSplinesCounter = 0;
        public List<CreatureMovement> Waypoints;
        public List<CreatureMovementSpline> MovementSplines;

        public ushort? AIAnimKit;
        public ushort? MovementAnimKit;
        public ushort? MeleeAnimKit;

        // Fields from UPDATE_FIELDS
        public uint Bytes1;
        public UnitDynamicFlags? DynamicFlags;
        public UnitDynamicFlagsWOD? DynamicFlagsWod;
        public uint Bytes2;

        public IUnitData UnitData;

        public Unit() : base()
        {
            UnitData = new UnitData(this);
            Waypoints = new List<CreatureMovement>();
            MovementSplines = new List<CreatureMovementSpline>();
        }

        public override bool IsTemporarySpawn()
        {
            if (ForceTemporarySpawn)
                return true;

            // If our unit got any of the following update fields set,
            // it's probably a temporary spawn
            return !UnitData.SummonedBy.IsEmpty() || !UnitData.CreatedBy.IsEmpty() || UnitData.CreatedBySpell != 0;
        }

        public override void LoadValuesFromUpdateFields()
        {
            Bytes1 = BitConverter.ToUInt32(new byte[] { UnitData.StandState, UnitData.PetTalentPoints, UnitData.VisFlags, UnitData.AnimTier }, 0);
            Bytes2 = BitConverter.ToUInt32(new byte[] { UnitData.SheatheState, UnitData.PvpFlags, UnitData.PetFlags, UnitData.ShapeshiftForm }, 0);
            if (ClientVersion.AddedInVersion(ClientType.WarlordsOfDraenor))
                DynamicFlagsWod = (UnitDynamicFlagsWOD)ObjectData.DynamicFlags;
            else
                DynamicFlags  = UpdateFields.GetEnum<UnitField, UnitDynamicFlags?>(UnitField.UNIT_DYNAMIC_FLAGS);
        }

        public void AddWaypoint(CreatureMovement movementData, Vector3 startPosition, DateTime packetTime)
        {
            movementData.Point = (uint)Waypoints.Count + 1;
            movementData.StartPositionX = startPosition.X;
            movementData.StartPositionY = startPosition.Y;
            movementData.StartPositionZ = startPosition.Z;
            movementData.UnixTime = (uint)Utilities.GetUnixTimeFromDateTime(packetTime);

            if (movementData.SplineCount > 0 &&
                movementData.SplinePoints != null)
            {
                int index = (int)movementData.SplineCount - 1;
                movementData.EndPositionX = movementData.SplinePoints[index].X;
                movementData.EndPositionY = movementData.SplinePoints[index].Y;
                movementData.EndPositionZ = movementData.SplinePoints[index].Z;

                if (movementData.SplineCount > 1)
                {
                    uint counter = 0;
                    foreach (Vector3 vector in movementData.SplinePoints)
                    {
                        counter++;
                        CreatureMovementSpline spline = new CreatureMovementSpline();
                        spline.ParentPoint = movementData.Point;
                        spline.SplinePoint = counter;
                        spline.GlobalPoint = globalSplinesCounter++;
                        spline.PositionX = vector.X;
                        spline.PositionY = vector.Y;
                        spline.PositionZ = vector.Z;
                        MovementSplines.Add(spline);
                    }
                }
                movementData.SplinePoints = null; // free memory
            }
            Waypoints.Add(movementData);
        }
    }
}
