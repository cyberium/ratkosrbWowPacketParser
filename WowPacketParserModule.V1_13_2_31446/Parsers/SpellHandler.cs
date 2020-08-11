using System;
using System.Collections.Generic;
using WowPacketParser.DBC;
using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;
using WowPacketParser.Store;
using WowPacketParser.Store.Objects;

namespace WowPacketParserModule.V1_13_2_31446.Parsers
{
    public static class SpellHandler
    {
        // Hardcoding all spells that need coordinates in classic
        public static readonly HashSet<uint> dbCoordinateSpells = new HashSet<uint> { 27, 31, 33, 34, 35, 427, 428, 442, 443, 444, 445, 446, 447, 518, 720, 731, 1121, 1573, 1936, 3561, 3562, 3563, 3565, 3566, 3567, 3721, 4170, 4801, 4996, 4997, 4998, 4999, 5000, 6348, 6349, 6483, 6484, 6714, 6719, 6766, 7136, 7586, 7587, 8136, 8195, 8606, 8735, 8912, 8995, 8996, 8997, 9002, 9003, 9004, 9055, 9268, 11012, 11362, 11409, 11511, 11795, 12158, 12159, 12241, 12510, 12520, 12885, 13044, 13142, 13461, 13912, 13954, 14208, 15737, 15739, 15740, 15741, 16572, 16767, 16768, 16772, 16775, 16776, 16777, 16778, 16779, 16780, 16786, 16787, 17086, 17087, 17088, 17089, 17090, 17091, 17092, 17093, 17094, 17095, 17097, 17159, 17160, 17167, 17237, 17239, 17240, 17278, 17334, 17475, 17476, 17477, 17478, 17479, 17480, 17607, 17608, 17609, 17610, 17611, 17863, 17939, 17943, 17944, 17946, 17948, 18351, 18352, 18353, 18354, 18355, 18356, 18357, 18358, 18359, 18360, 18361, 18564, 18565, 18566, 18567, 18568, 18569, 18570, 18571, 18572, 18573, 18574, 18575, 18576, 18578, 18579, 18580, 18581, 18582, 18583, 18584, 18585, 18586, 18587, 18588, 18589, 18590, 18591, 18592, 18593, 18594, 18595, 18596, 18597, 18598, 18599, 18600, 18601, 18602, 18603, 18604, 18605, 18606, 18607, 18609, 18611, 18612, 18613, 18614, 18615, 18616, 18617, 18618, 18619, 18620, 18621, 18622, 18623, 18624, 18625, 18626, 18627, 18628, 18634, 18907, 18960, 19527, 19723, 20449, 20534, 20618, 20682, 21110, 21111, 21112, 21113, 21114, 21115, 21116, 21117, 21128, 21131, 21132, 21133, 21135, 21136, 21137, 21138, 21139, 21171, 21425, 21886, 21900, 21901, 21902, 21903, 21904, 21905, 21906, 21907, 22191, 22192, 22193, 22194, 22195, 22196, 22197, 22198, 22199, 22200, 22201, 22202, 22267, 22268, 22563, 22564, 22651, 22668, 22669, 22670, 22671, 22672, 22673, 22674, 22675, 22676, 22950, 22951, 22972, 22975, 22976, 22977, 22978, 22979, 22980, 22981, 22982, 22983, 22984, 22985, 23405, 23406, 23441, 23442, 23446, 23460, 24214, 24228, 24229, 24230, 24231, 24232, 24233, 24325, 24466, 24593, 24730, 24831, 25004, 25139, 25182, 25708, 25709, 25725, 25825, 25826, 25827, 25828, 25831, 25832, 25865, 25866, 25867, 25868, 25869, 25870, 25871, 25872, 25873, 25874, 25875, 25876, 25877, 25878, 25879, 25880, 25881, 25882, 25883, 25884, 25904, 26220, 26285, 26448, 26450, 26451, 26452, 26453, 26454, 26455, 26456, 26538, 26539, 26630, 26631, 26632, 27597, 27598, 27694, 28025, 28026, 28147, 28280, 28444, 29181, 29190, 29215, 29216, 29217, 29223, 29224, 29225, 29226, 29227, 29231, 29237, 29238, 29239, 29240, 29247, 29248, 29249, 29255, 29256, 29257, 29258, 29262, 29267, 29268, 29269, 29273, 29295, 29318, 29508, 30211, 31605 };

        public static void ReadSpellCastLogData(Packet packet, params object[] idx)
        {
            packet.ReadSByte("Unk1_13_2", idx);
        }

        public static void ReadSpellTargetData(SpellCastData dbdata, Packet packet, uint spellID, params object[] idx)
        {
            packet.ResetBitReader();

            packet.ReadBitsE<TargetFlag>("Flags", 25, idx);

            var hasSrcLoc = packet.ReadBit("HasSrcLocation", idx);
            var hasDstLoc = packet.ReadBit("HasDstLocation", idx);
            var hasOrient = packet.ReadBit("HasOrientation", idx);
            var hasMapID = packet.ReadBit("hasMapID ", idx);
            var nameLength = packet.ReadBits(7);

            dbdata.MainTargetGuid = packet.ReadPackedGuid128("Unit", idx);
            packet.ReadPackedGuid128("Item", idx);

            if (hasSrcLoc)
                V6_0_2_19033.Parsers.SpellHandler.ReadLocation(packet, "SrcLocation");

            var dstLocation = new Vector3();
            if (hasDstLoc)
                dstLocation = V6_0_2_19033.Parsers.SpellHandler.ReadLocation(packet, "DstLocation");

            if (hasOrient)
                packet.ReadSingle("Orientation", idx);

            int mapID = -1;
            if (hasMapID)
                mapID = (ushort)packet.ReadInt32("MapID", idx);

            if (dstLocation != null && mapID != -1 && dbCoordinateSpells.Contains(spellID))
            {
                string effectHelper = $"Spell: { StoreGetters.GetName(StoreNameType.Spell, (int)spellID) } Efffect: { 0 } ({ (SpellEffects)0 })";

                var spellTargetPosition = new SpellTargetPosition
                {
                    ID = spellID,
                    EffectIndex = (byte)0,
                    PositionX = dstLocation.X,
                    PositionY = dstLocation.Y,
                    PositionZ = dstLocation.Z,
                    MapID = (ushort)mapID,
                    EffectHelper = effectHelper
                };

                if (!Storage.SpellTargetPositions.ContainsKey(spellTargetPosition))
                    Storage.SpellTargetPositions.Add(spellTargetPosition);
            }
            /*
            if (Settings.UseDBC && dstLocation != null && mapID != -1)
            {
                for (uint i = 0; i < 32; i++)
                {
                    var tuple = Tuple.Create(spellID, i);
                    if (DBC.SpellEffectStores.ContainsKey(tuple))
                    {
                        var effect = DBC.SpellEffectStores[tuple];
                        if ((Targets)effect.ImplicitTarget[0] == Targets.TARGET_DEST_DB || (Targets)effect.ImplicitTarget[1] == Targets.TARGET_DEST_DB)
                        {
                            string effectHelper = $"Spell: { StoreGetters.GetName(StoreNameType.Spell, (int)spellID) } Efffect: { effect.Effect } ({ (SpellEffects)effect.Effect })";

                            var spellTargetPosition = new SpellTargetPosition
                            {
                                ID = spellID,
                                EffectIndex = (byte)i,
                                PositionX = dstLocation.X,
                                PositionY = dstLocation.Y,
                                PositionZ = dstLocation.Z,
                                MapID = (ushort)mapID,
                                EffectHelper = effectHelper
                            };

                            if (!Storage.SpellTargetPositions.ContainsKey(spellTargetPosition))
                                Storage.SpellTargetPositions.Add(spellTargetPosition);
                        }
                    }
                }
            }
            */

            packet.ReadWoWString("Name", nameLength, idx);
        }

        public static void ReadSpellCastData(SpellCastData dbdata, Packet packet, params object[] idx)
        {
            dbdata.CasterGuid = packet.ReadPackedGuid128("CasterGUID", idx);

            packet.ReadPackedGuid128("CasterUnit", idx);

            packet.ReadPackedGuid128("CastID", idx);
            packet.ReadPackedGuid128("OriginalCastID", idx);

            var spellID = packet.ReadUInt32<SpellId>("SpellID", idx);
            dbdata.SpellID = spellID;
            packet.ReadUInt32("SpellXSpellVisualID", idx);

            uint castFlags = packet.ReadUInt32("CastFlags", idx);
            dbdata.CastFlags = castFlags;
            uint castFlagsEx = packet.ReadUInt32("CastFlagsEx", idx);
            dbdata.CastFlagsEx = castFlagsEx;
            packet.ReadUInt32("CastTime", idx);

            V6_0_2_19033.Parsers.SpellHandler.ReadMissileTrajectoryResult(packet, idx, "MissileTrajectory");

            packet.ReadByte("DestLocSpellCastIndex", idx);

            V6_0_2_19033.Parsers.SpellHandler.ReadCreatureImmunities(packet, idx, "Immunities");

            V6_0_2_19033.Parsers.SpellHandler.ReadSpellHealPrediction(packet, idx, "Predict");

            packet.ResetBitReader();

            var hitTargetsCount = packet.ReadBits("HitTargetsCount", 16, idx);
            dbdata.HitTargetsCount = hitTargetsCount;
            var missTargetsCount = packet.ReadBits("MissTargetsCount", 16, idx);
            var missStatusCount = packet.ReadBits("MissStatusCount", 16, idx);
            var remainingPowerCount = packet.ReadBits("RemainingPowerCount", 9, idx);

            var hasRuneData = packet.ReadBit("HasRuneData", idx);
            var targetPointsCount = packet.ReadBits("TargetPointsCount", 16, idx);
            var hasAmmoDisplayId = packet.ReadBit("HasAmmoDisplayId", idx);
            var hasAmmoInventoryType = packet.ReadBit("HasAmmoInventoryType", idx);

            for (var i = 0; i < missStatusCount; ++i)
                V6_0_2_19033.Parsers.SpellHandler.ReadSpellMissStatus(packet, idx, "MissStatus", i);

            ReadSpellTargetData(dbdata, packet, spellID, idx, "Target");

            for (var i = 0; i < hitTargetsCount; ++i)
            {
                WowGuid hitTarget = packet.ReadPackedGuid128("HitTarget", idx, i);

                for (uint j = 0; j < SpellCastData.MAX_SPELL_HIT_TARGETS_DB; j++)
                {
                    if (hitTarget.GetObjectType() == ObjectType.Player &&
                        dbdata.HitTargetType[j].Contains("Player"))
                        break;

                    if (dbdata.HitTargetID[j] == hitTarget.GetEntry() &&
                        dbdata.HitTargetType[j] == hitTarget.GetObjectType().ToString())
                        break;

                    if (dbdata.HitTargetID[j] == 0 &&
                        dbdata.HitTargetType[j] == "")
                    {
                        dbdata.HitTargetID[j] = hitTarget.GetEntry();
                        dbdata.HitTargetType[j] = hitTarget.GetObjectType().ToString();
                        break;
                    }
                }
            }

            for (var i = 0; i < missTargetsCount; ++i)
                packet.ReadPackedGuid128("MissTarget", idx, i);

            for (var i = 0; i < remainingPowerCount; ++i)
                V6_0_2_19033.Parsers.SpellHandler.ReadSpellPowerData(packet, idx, "RemainingPower", i);

            if (hasRuneData)
                V7_0_3_22248.Parsers.SpellHandler.ReadRuneData(packet, idx, "RemainingRunes");

            for (var i = 0; i < targetPointsCount; ++i)
                V6_0_2_19033.Parsers.SpellHandler.ReadLocation(packet, idx, "TargetPoints", i);

            if (hasAmmoDisplayId)
                packet.ReadInt32("AmmoDisplayId", idx);

            if (hasAmmoInventoryType)
                packet.ReadInt32E<InventoryType>("AmmoInventoryType", idx);
        }

        [Parser(Opcode.SMSG_SPELL_START)]
        public static void HandleSpellStart(Packet packet)
        {
            SpellCastData castData = new SpellCastData();
            ReadSpellCastData(castData, packet, "Cast");
            Storage.StoreSpellCastData(castData, Storage.SpellCastStart, packet);
        }

        [Parser(Opcode.SMSG_SPELL_GO)]
        public static void HandleSpellGo(Packet packet)
        {
            SpellCastData castData = new SpellCastData();
            ReadSpellCastData(castData, packet, "Cast");

            packet.ResetBitReader();

            var unkBit = packet.ReadBit();
            if (unkBit)
                packet.ReadSByte("UnkSByte");

            Storage.StoreSpellCastData(castData, Storage.SpellCastGo, packet);
        }

        [HasSniffData]
        [Parser(Opcode.SMSG_AURA_UPDATE)]
        public static void HandleAuraUpdate(Packet packet)
        {
            packet.ReadBit("UpdateAll");
            int countBits = ClientVersion.AddedInVersion(ClientVersionBuild.V1_13_3_32790) ? 8 : 7;
            var count = packet.ReadBits("AurasCount", countBits);

            var auras = new List<Aura>();
            for (var i = 0; i < count; ++i)
            {
                var aura = new Aura();

                packet.ReadByte("Slot", i);

                packet.ResetBitReader();
                var hasAura = packet.ReadBit("HasAura", i);
                if (hasAura)
                {
                    packet.ReadPackedGuid128("CastID", i);
                    aura.SpellId = (uint)packet.ReadInt32<SpellId>("SpellID", i);
                    packet.ReadInt32("SpellXSpellVisualID", i);
                    aura.AuraFlags = packet.ReadByteE<AuraFlagMoP>("Flags", i);
                    packet.ReadUInt32("ActiveFlags", i);
                    aura.Level = packet.ReadUInt16("CastLevel", i);
                    aura.Charges = packet.ReadByte("Applications", i);
                    packet.ReadInt32("ContentTuningID", i);

                    packet.ResetBitReader();

                    var hasCastUnit = packet.ReadBit("HasCastUnit", i);
                    var hasDuration = packet.ReadBit("HasDuration", i);
                    var hasRemaining = packet.ReadBit("HasRemaining", i);

                    var hasTimeMod = packet.ReadBit("HasTimeMod", i);

                    var pointsCount = packet.ReadBits("PointsCount", 6, i);
                    var effectCount = packet.ReadBits("EstimatedPoints", 6, i);

                    var hasContentTuning = packet.ReadBit("HasContentTuning", i);

                    if (hasContentTuning)
                        V8_0_1_27101.Parsers.SpellHandler.ReadContentTuningParams(packet, i, "ContentTuning");

                    if (hasCastUnit)
                        packet.ReadPackedGuid128("CastUnit", i);

                    aura.Duration = hasDuration ? packet.ReadInt32("Duration", i) : 0;
                    aura.MaxDuration = hasRemaining ? packet.ReadInt32("Remaining", i) : 0;

                    if (hasTimeMod)
                        packet.ReadSingle("TimeMod");

                    for (var j = 0; j < pointsCount; ++j)
                        packet.ReadSingle("Points", i, j);

                    for (var j = 0; j < effectCount; ++j)
                        packet.ReadSingle("EstimatedPoints", i, j);

                    auras.Add(aura);
                    packet.AddSniffData(StoreNameType.Spell, (int)aura.SpellId, "AURA_UPDATE");
                }
            }

            var guid = packet.ReadPackedGuid128("UnitGUID");

            if (Storage.Objects.ContainsKey(guid))
            {
                var unit = Storage.Objects[guid].Item1 as Unit;
                if (unit != null)
                {
                    // If this is the first packet that sends auras
                    // (hopefully at spawn time) add it to the "Auras" field,
                    // if not create another row of auras in AddedAuras
                    // (similar to ChangedUpdateFields)

                    if (unit.Auras == null)
                        unit.Auras = auras;
                    else
                        unit.AddedAuras.Add(auras);
                }
            }
        }
    }
}
