using System;
using System.Collections.Generic;
using WowPacketParser.DBC;
using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;
using WowPacketParser.Store;
using WowPacketParser.Store.Objects;

namespace WowPacketParserModule.V7_0_3_22248.Parsers
{
    public static class SpellHandler
    {
        public static void ReadSpellCastRequest(Packet packet, params object[] idx)
        {
            packet.ReadPackedGuid128("CastID", idx);

            for (var i = 0; i < 2; i++)
                packet.ReadInt32("Misc", idx, i);

            var spellId = packet.ReadUInt32<SpellId>("SpellID", idx);
            packet.ReadInt32("SpellXSpellVisualID", idx);

            V6_0_2_19033.Parsers.SpellHandler.ReadMissileTrajectoryRequest(packet, idx, "MissileTrajectory");

            packet.ReadPackedGuid128("Guid", idx);

            packet.ResetBitReader();

            packet.ReadBits("SendCastFlags", 5, idx);
            var hasMoveUpdate = packet.ReadBit("HasMoveUpdate", idx);

            var weightCount = packet.ReadBits("WeightCount", 2, idx);

            SpellCastData temp = new SpellCastData();
            ReadSpellTargetData(temp, packet, spellId, idx, "Target");

            if (hasMoveUpdate)
                MovementHandler.ReadMovementStats(packet, idx, "MoveUpdate");

            for (var i = 0; i < weightCount; ++i)
                V6_0_2_19033.Parsers.SpellHandler.ReadSpellWeight(packet, idx, "Weight", i);
        }

        public static void ReadSpellCastData(SpellCastData dbdata, Packet packet, params object[] idx)
        {
            dbdata.CasterGuid = packet.ReadPackedGuid128("CasterGUID", idx);
            dbdata.CasterUnitGuid = packet.ReadPackedGuid128("CasterUnit", idx);

            packet.ReadPackedGuid128("CastID", idx);
            packet.ReadPackedGuid128("OriginalCastID", idx);

            dbdata.SpellID = packet.ReadUInt32<SpellId>("SpellID", idx);
            dbdata.VisualID = packet.ReadUInt32("SpellXSpellVisualID", idx);

            dbdata.CastFlags = packet.ReadUInt32("CastFlags", idx);
            dbdata.CastTime = packet.ReadUInt32("CastTime", idx);

            V6_0_2_19033.Parsers.SpellHandler.ReadMissileTrajectoryResult(packet, idx, "MissileTrajectory");

            dbdata.AmmoDisplayId = packet.ReadInt32("Ammo.DisplayID", idx);

            packet.ReadByte("DestLocSpellCastIndex", idx);

            V6_0_2_19033.Parsers.SpellHandler.ReadCreatureImmunities(packet, idx, "Immunities");

            V6_0_2_19033.Parsers.SpellHandler.ReadSpellHealPrediction(packet, idx, "Predict");

            packet.ResetBitReader();

            dbdata.CastFlagsEx = packet.ReadBits("CastFlagsEx", ClientVersion.AddedInVersion(ClientVersionBuild.V7_3_2_25383) ? 23 : 22, idx);
            var hitTargetsCount = packet.ReadBits("HitTargetsCount", 16, idx);
            dbdata.HitTargetsCount = hitTargetsCount;
            var missTargetsCount = packet.ReadBits("MissTargetsCount", 16, idx);
            dbdata.MissTargetsCount = missTargetsCount;
            var missStatusCount = packet.ReadBits("MissStatusCount", 16, idx);
            dbdata.MissReasonsCount = missStatusCount;
            var remainingPowerCount = packet.ReadBits("RemainingPowerCount", 9, idx);

            var hasRuneData = packet.ReadBit("HasRuneData", idx);

            var targetPointsCount = packet.ReadBits("TargetPointsCount", 16, idx);

            for (var i = 0; i < missStatusCount; ++i)
            {
                uint reason = V6_0_2_19033.Parsers.SpellHandler.ReadSpellMissStatus(packet, idx, "MissStatus", i);
                dbdata.AddMissReason(reason);
            }

            ReadSpellTargetData(dbdata, packet, dbdata.SpellID, idx, "Target");

            for (var i = 0; i < hitTargetsCount; ++i)
            {
                WowGuid hitTarget = packet.ReadPackedGuid128("HitTarget", idx, i);
                dbdata.AddHitTarget(hitTarget);
                Storage.StoreSpellScriptTarget(dbdata.SpellID, hitTarget);
            }

            for (var i = 0; i < missTargetsCount; ++i)
            {
                WowGuid missTarget = packet.ReadPackedGuid128("MissTarget", idx, i);
                dbdata.AddMissTarget(missTarget);
            }

            for (var i = 0; i < remainingPowerCount; ++i)
                V6_0_2_19033.Parsers.SpellHandler.ReadSpellPowerData(packet, idx, "RemainingPower", i);

            if (hasRuneData)
                ReadRuneData(packet, idx, "RemainingRunes");

            for (var i = 0; i < targetPointsCount; ++i)
                V6_0_2_19033.Parsers.SpellHandler.ReadLocation(packet, idx, "TargetPoints", i);

            packet.AddSniffData(StoreNameType.Spell, (int)dbdata.SpellID, "CAST");
        }

        public static void ReadSpellTargetData(SpellCastData dbdata, Packet packet, uint spellID, params object[] idx)
        {
            packet.ResetBitReader();

            packet.ReadBitsE<TargetFlag>("Flags", ClientVersion.AddedInVersion(ClientVersionBuild.V8_1_5_29683) ? 26 : 25, idx);
            var hasSrcLoc = packet.ReadBit("HasSrcLocation", idx);
            var hasDstLoc = packet.ReadBit("HasDstLocation", idx);
            var hasOrient = packet.ReadBit("HasOrientation", idx);
            var hasMapID = packet.ReadBit("hasMapID ", idx);
            var nameLength = packet.ReadBits(7);

            dbdata.MainTargetGuid = packet.ReadPackedGuid128("Unit", idx);
            packet.ReadPackedGuid128("Item", idx);

            if (hasSrcLoc)
                dbdata.SrcPosition = V6_0_2_19033.Parsers.SpellHandler.ReadLocation(packet, "SrcLocation");

            if (hasDstLoc)
                dbdata.DstPosition = V6_0_2_19033.Parsers.SpellHandler.ReadLocation(packet, "DstLocation");

            float orientation = 0;
            if (hasOrient)
                orientation = packet.ReadSingle("Orientation", idx);
            dbdata.Orientation = orientation;

            int mapID = -1;
            if (hasMapID)
                mapID = packet.ReadInt32("MapID", idx);

            if (dbdata.DstPosition != null && mapID != -1 && packet.Direction == Direction.ServerToClient)
                Storage.StoreSpellTargetPosition(spellID, mapID, dbdata.DstPosition.Value, orientation);

            packet.ReadWoWString("Name", nameLength, idx);
        }

        public static void ReadRuneData(Packet packet, params object[] idx)
        {
            packet.ReadByte("Start", idx);
            packet.ReadByte("Count", idx);

            packet.ResetBitReader();

            var cooldownCount = packet.ReadInt32("CooldownCount", idx);

            for (var i = 0; i < cooldownCount; ++i)
                packet.ReadByte("Cooldowns", idx, i);
        }

        public static void ReadSandboxScalingData(Packet packet, params object[] idx)
        {
            packet.ResetBitReader();

            packet.ReadBits("Type", 3, idx);
            packet.ReadInt16("PlayerLevelDelta", idx);
            if (ClientVersion.AddedInVersion(ClientVersionBuild.V7_2_0_23826))
                packet.ReadUInt16("PlayerItemLevel", idx);
            packet.ReadByte("TargetLevel", idx);
            packet.ReadByte("Expansion", idx);
            packet.ReadByte("Class", idx);
            packet.ReadByte("TargetMinScalingLevel", idx);
            packet.ReadByte("TargetMaxScalingLevel", idx);
            packet.ReadSByte("TargetScalingLevelDelta", idx);
        }

        public static void ReadSpellCastLogData(Packet packet, params object[] idx)
        {
            packet.ReadInt64("Health", idx);
            packet.ReadInt32("AttackPower", idx);
            packet.ReadInt32("SpellPower", idx);

            packet.ResetBitReader();

            var spellLogPowerDataCount = packet.ReadBits("SpellLogPowerData", 9, idx);

            // SpellLogPowerData
            for (var i = 0; i < spellLogPowerDataCount; ++i)
            {
                packet.ReadInt32("PowerType", idx, i);
                packet.ReadInt32("Amount", idx, i);
                packet.ReadInt32("Cost", idx, i);
            }
        }

        public static void ReadTalentInfoUpdate(Packet packet, params object[] idx)
        {
            packet.ReadByte("ActiveGroup", idx);
            packet.ReadUInt32("PrimarySpecialization", idx);

            var talentGroupsCount = packet.ReadUInt32("TalentGroupsCount", idx);
            for (var i = 0; i < talentGroupsCount; ++i)
                ReadTalentGroupInfo(packet, idx, "TalentGroupsCount", i);
        }

        public static void ReadTalentGroupInfo(Packet packet, params object[] idx)
        {
            packet.ReadUInt32("SpecId", idx);
            var talentIDsCount = packet.ReadInt32("TalentIDsCount", idx);
            var pvpTalentIDsCount = packet.ReadInt32("PvPTalentIDsCount", idx);

            for (var i = 0; i < talentIDsCount; ++i)
                packet.ReadUInt16("TalentID", idx, i);

            for (var i = 0; i < pvpTalentIDsCount; ++i)
                packet.ReadUInt16("PvPTalentID", idx, i);
        }

        [Parser(Opcode.SMSG_SPELL_PREPARE)]
        public static void SpellPrepare(Packet packet)
        {
            packet.ReadPackedGuid128("ClientCastID");
            packet.ReadPackedGuid128("ServerCastID");
        }

        [Parser(Opcode.CMSG_CAST_SPELL)]
        public static void HandleCastSpell(Packet packet)
        {
            ReadSpellCastRequest(packet, "Cast");
        }

        [Parser(Opcode.CMSG_PET_CAST_SPELL)]
        public static void HandlePetCastSpell(Packet packet)
        {
            packet.ReadPackedGuid128("PetGUID");
            ReadSpellCastRequest(packet, "Cast");
        }
        
        [Parser(Opcode.SMSG_SPELL_START)]
        public static void HandleSpellStart(Packet packet)
        {
            SpellCastData castData = new SpellCastData();
            ReadSpellCastData(castData, packet, "Cast");
            Storage.StoreSpellCastData(castData, CastDataType.Start, packet);
        }

        [Parser(Opcode.SMSG_SPELL_GO)]
        public static void HandleSpellGo(Packet packet)
        {
            SpellCastData castData = new SpellCastData();

            ReadSpellCastData(castData, packet, "Cast");

            packet.ResetBitReader();

            var hasLogData = packet.ReadBit();
            if (hasLogData)
                ReadSpellCastLogData(packet, "LogData");

            Storage.StoreSpellCastData(castData, CastDataType.Go, packet);
        }

        [HasSniffData]
        [Parser(Opcode.SMSG_AURA_UPDATE)]
        public static void HandleAuraUpdate(Packet packet)
        {
            bool isFullUpdate = packet.ReadBit("UpdateAll");
            var count = packet.ReadBits("AurasCount", 9);

            var auras = new List<Aura>();
            for (var i = 0; i < count; ++i)
            {
                var aura = new Aura();

                aura.Slot = packet.ReadByte("Slot", i);

                packet.ResetBitReader();
                var hasAura = packet.ReadBit("HasAura", i);
                if (hasAura)
                {
                    packet.ReadPackedGuid128("CastID", i);
                    aura.SpellId = (uint)packet.ReadInt32<SpellId>("SpellID", i);
                    aura.VisualId = (uint)packet.ReadInt32("SpellXSpellVisualID", i);
                    aura.AuraFlags = (uint)packet.ReadByteE<AuraFlagMoP>("Flags", i);
                    aura.ActiveFlags = packet.ReadUInt32("ActiveFlags", i);
                    aura.Level = packet.ReadUInt16("CastLevel", i);
                    aura.Charges = packet.ReadByte("Applications", i);

                    packet.ResetBitReader();

                    var hasCastUnit = packet.ReadBit("HasCastUnit", i);
                    var hasDuration = packet.ReadBit("HasDuration", i);
                    var hasRemaining = packet.ReadBit("HasRemaining", i);

                    var hasTimeMod = packet.ReadBit("HasTimeMod", i);

                    var pointsCount = packet.ReadBits("PointsCount", 6, i);
                    var effectCount = packet.ReadBits("EstimatedPoints", 6, i);

                    var hasSandboxScaling = packet.ReadBit("HasSandboxScaling", i);

                    if (ClientVersion.AddedInVersion(ClientVersionBuild.V7_2_0_23826))
                    {
                        if (hasSandboxScaling)
                            ReadSandboxScalingData(packet, "SandboxScalingData", i);
                    }

                    if (hasCastUnit)
                        aura.CasterGuid = packet.ReadPackedGuid128("CastUnit", i);

                    aura.Duration = hasDuration ? (int)packet.ReadUInt32("Duration", i) : 0;
                    aura.MaxDuration = hasRemaining ? (int)packet.ReadUInt32("Remaining", i) : 0;

                    if (hasTimeMod)
                        packet.ReadSingle("TimeMod");

                    for (var j = 0; j < pointsCount; ++j)
                        packet.ReadSingle("Points", i, j);

                    for (var j = 0; j < effectCount; ++j)
                        packet.ReadSingle("EstimatedPoints", i, j);

                    if (ClientVersion.RemovedInVersion(ClientVersionBuild.V7_2_0_23826))
                    {
                        if (hasSandboxScaling)
                            ReadSandboxScalingData(packet, "SandboxScalingData", i);
                    }

                    packet.AddSniffData(StoreNameType.Spell, (int)aura.SpellId, "AURA_UPDATE");
                }
                auras.Add(aura);
            }

            var guid = packet.ReadPackedGuid128("UnitGUID");
            Storage.StoreUnitAurasUpdate(guid, auras, packet.Time, isFullUpdate);
        }

        [Parser(Opcode.SMSG_CAST_FAILED)]
        public static void HandleCastFailed(Packet packet)
        {
            packet.ReadPackedGuid128("CastID");
            packet.ReadInt32<SpellId>("SpellID");
            packet.ReadInt32("SpellXSpellVisualID");
            packet.ReadInt32("Reason");
            packet.ReadInt32("FailedArg1");
            packet.ReadInt32("FailedArg2");
        }

        [Parser(Opcode.SMSG_PET_CAST_FAILED)]
        public static void HandlePetCastFailed(Packet packet)
        {
            packet.ReadPackedGuid128("CastID");
            packet.ReadInt32<SpellId>("SpellID");
            packet.ReadInt32("Reason");
            packet.ReadInt32("FailedArg1");
            packet.ReadInt32("FailedArg2");
        }

        [Parser(Opcode.SMSG_SPELL_FAILURE)]
        public static void HandleSpellFailure(Packet packet)
        {
            SpellCastFailed failData = new SpellCastFailed();
            failData.Guid = packet.ReadPackedGuid128("CasterUnit");
            packet.ReadPackedGuid128("CastID");
            failData.SpellId = (uint)packet.ReadInt32<SpellId>("SpellID");
            failData.VisualId = packet.ReadUInt32("SpellXSpellVisualID");
            failData.Reason = (uint)packet.ReadInt16E<SpellCastFailureReason>("Reason");
            failData.Time = packet.Time;
            Storage.SpellCastFailed.Add(failData);
        }

        [Parser(Opcode.SMSG_SPELL_FAILED_OTHER)]
        public static void HandleSpellFailedOther(Packet packet)
        {
            SpellCastFailed failData = new SpellCastFailed();
            failData.Guid = packet.ReadPackedGuid128("CasterUnit");
            packet.ReadPackedGuid128("CastID");
            failData.SpellId = packet.ReadUInt32<SpellId>("SpellID");
            failData.VisualId = packet.ReadUInt32("SpellXSpellVisualID");
            failData.Reason = (uint)packet.ReadByteE<SpellCastFailureReason>("Reason");
            failData.Time = packet.Time;
            Storage.SpellCastFailed.Add(failData);
        }

        [Parser(Opcode.SMSG_LEARNED_SPELLS)]
        public static void HandleLearnedSpells(Packet packet)
        {
            var spellCount = packet.ReadUInt32("SpellCount");
            var favoriteSpellCount = packet.ReadUInt32("FavoriteSpellCount");

            for (var i = 0; i < spellCount; ++i)
                packet.ReadInt32<SpellId>("SpellID", i);

            for (var i = 0; i < favoriteSpellCount; ++i)
                packet.ReadInt32<SpellId>("FavoriteSpellID", i);

            packet.ReadBit("SuppressMessaging");
        }

        [Parser(Opcode.SMSG_UPDATE_TALENT_DATA)]
        public static void ReadUpdateTalentData(Packet packet)
        {
            ReadTalentInfoUpdate(packet, "Info");
        }

        [Parser(Opcode.SMSG_SEND_KNOWN_SPELLS)]
        public static void HandleSendKnownSpells(Packet packet)
        {
            Storage.ClearTemporarySpellList();

            packet.ReadBit("InitialLogin");
            var knownSpells = packet.ReadUInt32("KnownSpellsCount");
            var favoriteSpells = packet.ReadUInt32("FavoriteSpellsCount");

            for (var i = 0; i < knownSpells; i++)
            {
                uint spellId = packet.ReadUInt32<SpellId>("KnownSpellId", i);
                Storage.StoreCharacterSpell(WowGuid64.Empty, spellId);
            }

            for (var i = 0; i < favoriteSpells; i++)
                packet.ReadUInt32<SpellId>("FavoriteSpellId", i);
        }

        [Parser(Opcode.CMSG_CANCEL_CAST)]
        public static void HandleCancelCast(Packet packet)
        {
            packet.ReadPackedGuid128("CastID");
            packet.ReadUInt32<SpellId>("SpellID");
        }

        [Parser(Opcode.CMSG_LEARN_TALENTS)]
        public static void HandleLearnTalents(Packet packet)
        {
            var talentCount = packet.ReadBits("TalentCount", 6);
            for (int i = 0; i < talentCount; i++)
                packet.ReadUInt16("Talents");
        }

        [Parser(Opcode.SMSG_RESYNC_RUNES)]
        public static void HandleResyncRunes(Packet packet)
        {
            packet.ReadByte("Start");
            packet.ReadByte("Count");

            var cooldownCount = packet.ReadUInt32("CooldownCount");
            for (var i = 0; i < cooldownCount; ++i)
                packet.ReadByte("Cooldown");
        }

        [Parser(Opcode.SMSG_SPELL_COOLDOWN, ClientVersionBuild.V7_1_0_22900)]
        public static void HandleSpellCooldown(Packet packet)
        {
            WowGuid casterGuid = packet.ReadPackedGuid128("Caster");
            byte flags = packet.ReadByte("Flags");

            var count = packet.ReadInt32("SpellCooldownsCount");
            for (int i = 0; i < count; i++)
            {
                CreaturePetCooldown petCooldown = new CreaturePetCooldown();
                petCooldown.SpellID = (uint)packet.ReadInt32("SrecID", i);
                petCooldown.Cooldown = (uint)packet.ReadInt32("ForcedCooldown", i);
                petCooldown.ModRate = packet.ReadSingle("ModRate", i);
                if (casterGuid.GetObjectType() == ObjectType.Unit)
                {
                    petCooldown.CasterID = casterGuid.GetEntry();
                    petCooldown.Flags = flags;
                    petCooldown.Index = (byte)i;
                    petCooldown.SniffId = packet.SniffIdString;
                    Storage.CreaturePetCooldown.Add(petCooldown);
                }
            }
        }


        [Parser(Opcode.SMSG_REFRESH_SPELL_HISTORY, ClientVersionBuild.V7_1_0_22900)]
        [Parser(Opcode.SMSG_SEND_SPELL_HISTORY, ClientVersionBuild.V7_1_0_22900)]
        public static void HandleSendSpellHistory(Packet packet)
        {
            var int4 = packet.ReadInt32("SpellHistoryEntryCount");
            for (int i = 0; i < int4; i++)
            {
                packet.ReadUInt32<SpellId>("SpellID", i);
                packet.ReadUInt32("ItemID", i);
                packet.ReadUInt32("Category", i);
                packet.ReadInt32("RecoveryTime", i);
                packet.ReadInt32("CategoryRecoveryTime", i);
                packet.ReadSingle("ModRate", i);

                packet.ResetBitReader();
                var unused622_1 = packet.ReadBit();
                var unused622_2 = packet.ReadBit();

                packet.ReadBit("OnHold", i);

                if (unused622_1)
                    packet.ReadUInt32("Unk_622_1", i);

                if (unused622_2)
                    packet.ReadUInt32("Unk_622_2", i);
            }
        }


        [Parser(Opcode.SMSG_SEND_SPELL_CHARGES, ClientVersionBuild.V7_1_0_22900)]
        public static void HandleSendSpellCharges(Packet packet)
        {
            var int4 = packet.ReadInt32("SpellChargeEntryCount");
            for (int i = 0; i < int4; i++)
            {
                packet.ReadUInt32("Category", i);
                packet.ReadUInt32("NextRecoveryTime", i);
                packet.ReadByte("ConsumedCharges", i);
                packet.ReadSingle("ChargeModRate", i);

                packet.ReadBit("IsPet");
            }
        }

        [Parser(Opcode.SMSG_RESURRECT_REQUEST, ClientVersionBuild.V7_1_0_22900)]
        public static void HandleResurrectRequest(Packet packet)
        {
            packet.ReadPackedGuid128("ResurrectOffererGUID");

            packet.ReadUInt32("ResurrectOffererVirtualRealmAddress");
            packet.ReadUInt32("PetNumber");
            packet.ReadInt32<SpellId>("SpellID");

            var len = packet.ReadBits(11);

            packet.ReadBit("UseTimer");
            packet.ReadBit("Sickness");

            packet.ReadWoWString("Name", len);
        }

        [Parser(Opcode.SMSG_TOTEM_CREATED, ClientVersionBuild.V7_1_0_22900)]
        public static void HandleTotemCreated(Packet packet)
        {
            packet.ReadByte("Slot");
            packet.ReadPackedGuid128("Totem");
            packet.ReadUInt32("Duration");
            packet.ReadUInt32<SpellId>("SpellID");
            packet.ReadSingle("TimeMod");

            packet.ResetBitReader();
            packet.ReadBit("CannotDismiss");
        }

        [Parser(Opcode.SMSG_SPELL_CHANNEL_START, ClientVersionBuild.V7_2_0_23826)]
        public static void HandleSpellChannelStart(Packet packet)
        {
            SpellChannelStart channel = new SpellChannelStart();
            channel.Guid = packet.ReadPackedGuid128("CasterGUID");
            channel.SpellId = (uint)packet.ReadInt32<SpellId>("SpellID");
            channel.VisualId = (uint)packet.ReadInt32("SpellXSpellVisualID");
            channel.Duration = packet.ReadInt32("ChannelDuration");

            var hasInterruptImmunities = packet.ReadBit("HasInterruptImmunities");
            var hasHealPrediction = packet.ReadBit("HasHealPrediction");

            if (hasInterruptImmunities)
                V6_0_2_19033.Parsers.SpellHandler.ReadSpellChannelStartInterruptImmunities(packet, "InterruptImmunities");

            if (hasHealPrediction)
                V6_0_2_19033.Parsers.SpellHandler.ReadSpellTargetedHealPrediction(packet, "HealPrediction");

            channel.Time = packet.Time;
            Storage.SpellChannelStart.Add(channel);
        }

        [Parser(Opcode.SMSG_RESUME_CAST_BAR, ClientVersionBuild.V7_2_0_23826)]
        public static void HandleResumeCastBar(Packet packet)
        {
            packet.ReadPackedGuid128("Guid");
            packet.ReadPackedGuid128("Target");

            packet.ReadUInt32<SpellId>("SpellID");
            packet.ReadInt32("SpellXSpellVisualID");
            packet.ReadUInt32("TimeRemaining");
            packet.ReadUInt32("TotalTime");

            var result = packet.ReadBit("HasInterruptImmunities");
            if (result)
            {
                packet.ReadUInt32("SchoolImmunities");
                packet.ReadUInt32("Immunities");
            }
        }

        [Parser(Opcode.SMSG_MIRROR_IMAGE_COMPONENTED_DATA)]
        public static void HandleMirrorImageData(Packet packet)
        {
            packet.ReadPackedGuid128("UnitGUID");
            packet.ReadInt32("DisplayID");

            packet.ReadByte("RaceID");
            packet.ReadByte("Gender");
            packet.ReadByte("ClassID");
            packet.ReadByte("SkinColor");
            packet.ReadByte("FaceVariation");
            packet.ReadByte("HairVariation");
            packet.ReadByte("HairColor");
            packet.ReadByte("BeardVariation");

            for (var i = 0; i < 3; i++)
                packet.ReadByte("CustomDisplayOption", i);

            packet.ReadPackedGuid128("GuildGUID");

            var count = packet.ReadInt32("ItemDisplayCount");
            for (var i = 0; i < count; i++)
                packet.ReadInt32("ItemDisplayID", i);
        }

        [Parser(Opcode.CMSG_UPDATE_SPELL_VISUAL)]
        public static void HandleUpdateSpellVisual(Packet packet)
        {
            packet.ReadUInt32("SpellID");
            packet.ReadUInt32("SpellXSpellVisualID");
            packet.ReadPackedGuid128("GUID");
        }

        [Parser(Opcode.SMSG_LOSS_OF_CONTROL_AURA_UPDATE)]
        public static void HandleLossOfControlAuraUpdate(Packet packet)
        {
            if (ClientVersion.AddedInVersion(ClientVersionBuild.V7_2_5_24330))
                packet.ReadPackedGuid128("AffectedGUID");
            var count = packet.ReadInt32("LossOfControlInfoCount");
            for (int i = 0; i < count; i++)
            {
                packet.ReadByte("AuraSlot", i);
                packet.ReadByte("EffectIndex", i);
                packet.ReadByte("Type", i);
                packet.ReadByte("Mechanic", i);
            }
        }

        [Parser(Opcode.SMSG_LEARN_TALENT_FAILED)]
        public static void HandleLearnTalentFailed(Packet packet)
        {
            packet.ReadBits("Reason", 4);
            packet.ReadInt32("SpellID");
            var count = packet.ReadUInt32("TalentCount");
            for (int i = 0; i < count; i++)
                packet.ReadUInt16("Talent");
        }

        public static void ReadGlyphBinding(Packet packet, params object[] index)
        {
            packet.ReadUInt32("SpellID", index);
            packet.ReadUInt16("GlyphID", index);
        }

        [Parser(Opcode.SMSG_ACTIVE_GLYPHS)]
        public static void HandleActiveGlyphs(Packet packet)
        {
            var count = packet.ReadUInt32("GlyphsCount");
            for (int i = 0; i < count; i++)
                ReadGlyphBinding(packet, i);
            packet.ResetBitReader();
            packet.ReadBit("IsFullUpdate");
        }

        [Parser(Opcode.SMSG_NOTIFY_DEST_LOC_SPELL_CAST)]
        public static void HandleNotifyDestLocSpellCast(Packet packet)
        {
            packet.ReadPackedGuid128("Caster");
            packet.ReadPackedGuid128("DestTransport");
            packet.ReadUInt32<SpellId>("SpellID");
            packet.ReadUInt32("SpellXSpellVisualID");
            packet.ReadVector3("SourceLoc");
            packet.ReadVector3("DestLoc");
            packet.ReadSingle("MissileTrajectoryPitch");
            packet.ReadSingle("MissileTrajectorySpeed");
            packet.ReadInt32("TravelTime");
            packet.ReadByte("DestLocSpellCastIndex");
            packet.ReadPackedGuid128("CastID");
        }

        [Parser(Opcode.SMSG_NOTIFY_MISSILE_TRAJECTORY_COLLISION)]
        public static void HandleNotifyMissileTrajectoryCollision(Packet packet)
        {
            packet.ReadPackedGuid128("Caster");
            packet.ReadPackedGuid128("CastID");
            packet.ReadVector3("CollisionPos");
        }

        [Parser(Opcode.CMSG_MISSILE_TRAJECTORY_COLLISION)]
        public static void HandleMissileTrajectoryCollision(Packet packet)
        {
            packet.ReadPackedGuid128("CasterGUID");
            packet.ReadInt32<SpellId>("SpellID");
            packet.ReadPackedGuid128("CastID");
            packet.ReadVector3("CollisionPos");
        }

        [Parser(Opcode.SMSG_SUPERCEDED_SPELLS)]
        public static void HandleSupercededSpells(Packet packet)
        {
            var spellCount = packet.ReadInt32("SpellCount");
            var supercededCount = packet.ReadInt32("SupercededCount");
            var favoriteSpellCount = packet.ReadInt32("FavoriteSpellCount");

            for (int i = 0; i < spellCount; i++)
                packet.ReadUInt32("SpellID", i);

            for (int i = 0; i < supercededCount; i++)
                packet.ReadUInt32("Superceded", i);

            for (int i = 0; i < favoriteSpellCount; i++)
                packet.ReadUInt32("FavoriteSpellID", i);
        }

        [Parser(Opcode.SMSG_SET_SPELL_CHARGES)]
        public static void HandleSetSpellCharges(Packet packet)
        {
            packet.ReadInt32("Category");
            packet.ReadInt32("RecoveryTime");
            packet.ReadByte("ConsumedCharges");
            packet.ReadSingle("ChargeModRate");
            packet.ReadBit("IsPet");
        }

        [Parser(Opcode.SMSG_UPDATE_COOLDOWN)] 
        public static void HandleUpdateCooldown(Packet packet)
        {
            packet.ReadInt32("SpellId");
            packet.ReadSingle("SpeedRate");
            packet.ReadSingle("SpeedRate2");
        }

        [Parser(Opcode.SMSG_UPDATE_CHARGE_CATEGORY_COOLDOWN)]
        public static void HandleUpdateChargeCategoryCooldown(Packet packet)
        {
            packet.ReadInt32("ChargeCategoryId");
            packet.ReadSingle("SpeedRate");
            packet.ReadSingle("UnkFloat");
            packet.ReadBit("UnkBool");
        }
    }
}
