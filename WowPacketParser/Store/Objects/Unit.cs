using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Store.Objects.UpdateFields;
using WowPacketParser.Store.Objects.UpdateFields.LegacyImplementation;

namespace WowPacketParser.Store.Objects
{
    public class Unit : WoWObject
    {
        public static uint UnitGuidCounter = 0;
        public uint DbGuid;

        public List<Aura> Auras;
        public List<Aura> AurasOriginal;
        public List<ServerSideMovement> Waypoints;
        public List<ServerSideMovement> CombatMovements;
        public List<ServerSideMovementSpline> WaypointSplines;
        public List<ServerSideMovementSpline> CombatMovementSplines;

        public ushort? AIAnimKit;
        public ushort? MovementAnimKit;
        public ushort? MeleeAnimKit;

        // Fields from UPDATE_FIELDS
        public uint Bytes1;
        public UnitDynamicFlags? DynamicFlags;
        public UnitDynamicFlagsWOD? DynamicFlagsWod;
        public uint Bytes2;

        public IUnitData UnitData;
        public IUnitData UnitDataOriginal;

        public Unit(bool isCreature = true) : base()
        {
            UnitData = new UnitData(this);
            UnitDataOriginal = new OriginalUnitData(this);
            CombatMovements = new List<ServerSideMovement>();
            CombatMovementSplines = new List<ServerSideMovementSpline>();

            if (isCreature)
            {
                DbGuid = ++UnitGuidCounter;
                Waypoints = new List<ServerSideMovement>();
                WaypointSplines = new List<ServerSideMovementSpline>();
            }   
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

        public uint GetDynamicFlags()
        {
            if (ClientVersion.AddedInVersion(ClientType.WarlordsOfDraenor))
                return ObjectData.DynamicFlags;

            return UnitData.DynamicFlags;
        }

        public uint GetDynamicFlagsOriginal()
        {
            if (ClientVersion.AddedInVersion(ClientType.WarlordsOfDraenor))
                return ObjectDataOriginal.DynamicFlags;

            return UnitDataOriginal.DynamicFlags;
        }

        public Aura GetAuraInSlot(uint slot)
        {
            if (Auras == null)
                return null;

            foreach (Aura aura in Auras)
            {
                if (aura.Slot == slot)
                    return aura;
            }

            return null;
        }

        public string GetOriginalAurasString(bool noCaster)
        {
            string auras = string.Empty;
            if (AurasOriginal != null && AurasOriginal.Count != 0)
            {
                foreach (Aura aura in AurasOriginal)
                {
                    if (aura == null)
                        continue;

                    if (noCaster)
                    {
                        // usually "template auras" do not have caster
                        if (ClientVersion.AddedInVersion(ClientType.MistsOfPandaria) ? !aura.AuraFlags.HasAnyFlag(AuraFlagMoP.NoCaster) : !aura.AuraFlags.HasAnyFlag(AuraFlag.NotCaster))
                            continue;
                    }

                    auras += aura.SpellId + " ";
                }
                auras = auras.TrimEnd(' ');
            }

            return auras;
        }

        public void ApplyAuraUpdates(List<Aura> updates)
        {
            if (Auras == null)
            {
                Auras = updates.Select(aura => aura.Clone()).ToList(); ;
                return;
            }

            foreach (Aura update in updates)
            {
                Aura aura = GetAuraInSlot((uint)update.Slot);
                if (aura == null)
                {
                    Auras.Add(update.Clone());
                    continue;
                }

                if (aura.SpellId != update.SpellId)
                    aura.SpellId = update.SpellId;
                if (aura.VisualId != update.VisualId)
                    aura.VisualId = update.VisualId;
                if (aura.AuraFlags != update.AuraFlags)
                    aura.AuraFlags = update.AuraFlags;
                if (aura.ActiveFlags != update.ActiveFlags)
                    aura.ActiveFlags = update.ActiveFlags;
                if (aura.Level != update.Level)
                    aura.Level = update.Level;
                if (aura.Charges != update.Charges)
                    aura.Charges = update.Charges;
                if (aura.ContentTuningId != update.ContentTuningId)
                    aura.ContentTuningId = update.ContentTuningId;
                if (aura.CasterGuid != update.CasterGuid)
                    aura.CasterGuid = update.CasterGuid;
                if (aura.MaxDuration != update.MaxDuration)
                    aura.MaxDuration = update.MaxDuration;
                if (aura.Duration != update.Duration)
                    aura.Duration = update.Duration;
            }
        }

        public bool HasAuraMatchingCriteria(Func<uint,bool> auraCheckFunc)
        {
            if (Auras == null)
                return false;

            foreach (Aura aura in Auras)
            {
                if (aura == null || aura.SpellId == 0)
                    continue;

                if (auraCheckFunc(aura.SpellId))
                    return true;
            }

            return false;
        }

        public void AddWaypoint(ServerSideMovement movementData, Vector3 startPosition, DateTime packetTime)
        {
            List<ServerSideMovement> list = null;
            if ((Type == ObjectType.Unit) && ((UnitData.Flags & (uint)UnitFlags.IsInCombat) == 0))
                list = Waypoints;
            else
                list = CombatMovements;

            movementData.Point = (uint)list.Count + 1;
            movementData.StartPositionX = startPosition.X;
            movementData.StartPositionY = startPosition.Y;
            movementData.StartPositionZ = startPosition.Z;
            movementData.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(packetTime);

            if (movementData.SplineCount > 0 &&
                movementData.SplinePoints != null)
            {
                int index = (int)movementData.SplineCount - 1;
                movementData.EndPositionX = movementData.SplinePoints[index].X;
                movementData.EndPositionY = movementData.SplinePoints[index].Y;
                movementData.EndPositionZ = movementData.SplinePoints[index].Z;

                if (movementData.SplineCount > 1)
                {
                    List<ServerSideMovementSpline> splinesList = null;
                    if ((Type == ObjectType.Unit) && ((UnitData.Flags & (uint)UnitFlags.IsInCombat) == 0))
                        splinesList = WaypointSplines;
                    else
                        splinesList = CombatMovementSplines;

                    uint counter = 0;
                    foreach (Vector3 vector in movementData.SplinePoints)
                    {
                        counter++;
                        ServerSideMovementSpline spline = new ServerSideMovementSpline();
                        spline.ParentPoint = movementData.Point;
                        spline.SplinePoint = counter;
                        spline.PositionX = vector.X;
                        spline.PositionY = vector.Y;
                        spline.PositionZ = vector.Z;
                        splinesList.Add(spline);
                    }
                }
                movementData.SplinePoints = null; // free memory
            }
            list.Add(movementData);
        }
    }
}
