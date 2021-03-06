using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;
using WowPacketParser.Store;
using WowPacketParser.Store.Objects;

namespace WowPacketParserModule.V7_0_3_22248.Parsers
{
    public static class CombatLogHandler
    {
        public static UnitMeleeAttackLog ReadAttackRoundInfo(Packet packet, params object[] indexes)
        {
            UnitMeleeAttackLog attackData = new UnitMeleeAttackLog();
            var hitInfo = packet.ReadInt32E<SpellHitInfo>("HitInfo", indexes);
            attackData.HitInfo = (uint)hitInfo;

            attackData.Attacker = packet.ReadPackedGuid128("AttackerGUID", indexes);
            attackData.Victim = packet.ReadPackedGuid128("TargetGUID", indexes);

            attackData.Damage = packet.ReadInt32("Damage", indexes);
            attackData.OverkillDamage = packet.ReadInt32("OverDamage", indexes);

            attackData.SubDamageCount = packet.ReadByte("Sub Damage Count", indexes);
            for (int i = 0; i < attackData.SubDamageCount; i++)
            {
                attackData.TotalSchoolMask |= (uint)packet.ReadInt32("SchoolMask", indexes);
                packet.ReadSingle("FloatDamage", indexes);
                packet.ReadInt32("IntDamage", indexes);

                if (hitInfo.HasAnyFlag(SpellHitInfo.HITINFO_PARTIAL_ABSORB | SpellHitInfo.HITINFO_FULL_ABSORB))
                    attackData.TotalAbsorbedDamage += packet.ReadInt32("DamageAbsorbed", indexes);

                if (hitInfo.HasAnyFlag(SpellHitInfo.HITINFO_PARTIAL_RESIST | SpellHitInfo.HITINFO_FULL_RESIST))
                    attackData.TotalResistedDamage += packet.ReadInt32("DamageResisted", indexes);
            }

            attackData.VictimState = (uint)packet.ReadByteE<VictimStates>("VictimState", indexes);
            attackData.AttackerState = packet.ReadInt32("AttackerState", indexes);

            attackData.SpellId = (uint)packet.ReadInt32<SpellId>("MeleeSpellID", indexes);

            if (hitInfo.HasAnyFlag(SpellHitInfo.HITINFO_BLOCK))
                attackData.BlockedDamage = packet.ReadInt32("BlockAmount", indexes);

            if (hitInfo.HasAnyFlag(SpellHitInfo.HITINFO_RAGE_GAIN))
                packet.ReadInt32("RageGained", indexes);

            if (hitInfo.HasAnyFlag(SpellHitInfo.HITINFO_UNK0))
            {
                packet.ReadInt32("Unk Attacker State 3 1", indexes);
                packet.ReadSingle("Unk Attacker State 3 2", indexes);
                packet.ReadSingle("Unk Attacker State 3 3", indexes);
                packet.ReadSingle("Unk Attacker State 3 4", indexes);
                packet.ReadSingle("Unk Attacker State 3 5", indexes);
                packet.ReadSingle("Unk Attacker State 3 6", indexes);
                packet.ReadSingle("Unk Attacker State 3 7", indexes);
                packet.ReadSingle("Unk Attacker State 3 8", indexes);
                packet.ReadSingle("Unk Attacker State 3 9", indexes);
                packet.ReadSingle("Unk Attacker State 3 10", indexes);
                packet.ReadSingle("Unk Attacker State 3 11", indexes);
                packet.ReadInt32("Unk Attacker State 3 12", indexes);
            }

            if (hitInfo.HasAnyFlag(SpellHitInfo.HITINFO_BLOCK | SpellHitInfo.HITINFO_UNK12))
                packet.ReadSingle("Unk Float", indexes);

            ReadSandboxScalingData(packet, indexes, "SandboxScalingData");
            return attackData;
        }

        public static void ReadSandboxScalingData(Packet packet, params object[] idx)
        {
            packet.ReadByte("Type", idx);
            packet.ReadByte("TargetLevel", idx);
            packet.ReadByte("Expansion", idx);
            packet.ReadByte("Class", idx);
            packet.ReadByte("TargetMinScalingLevel", idx);
            packet.ReadByte("TargetMaxScalingLevel", idx);
            packet.ReadInt16("PlayerLevelDelta", idx);
            packet.ReadSByte("TargetScalingLevelDelta", idx);
            if (ClientVersion.AddedInVersion(ClientVersionBuild.V7_2_0_23826))
                packet.ReadUInt16("PlayerItemLevel", idx);
        }

        public static void ReadPeriodicAuraLogEffectData(Packet packet, params object[] idx)
        {
            packet.ReadInt32("Effect", idx);
            packet.ReadInt32("Amount", idx);
            packet.ReadInt32("OverHealOrKill", idx);
            packet.ReadInt32("SchoolMaskOrPower", idx);
            packet.ReadInt32("AbsorbedOrAmplitude", idx);
            packet.ReadInt32("Resisted", idx);

            packet.ResetBitReader();

            packet.ReadBit("Crit", idx);
            var hasDebugData = packet.ReadBit("HasPeriodicAuraLogEffectDebugInfo", idx);
            var hasSandboxScaling = packet.ReadBit("HasSandboxScaling", idx);

            if (hasSandboxScaling)
                SpellHandler.ReadSandboxScalingData(packet, idx, "SandboxScalingData");

            if (hasDebugData)
            {
                packet.ReadSingle("CritRollMade", idx);
                packet.ReadSingle("CritRollNeeded", idx);
            }
        }

        public static void ReadSpellNonMeleeDebugData(Packet packet, params object[] idx)
        {
            packet.ReadSingle("CritRoll", idx);
            packet.ReadSingle("CritNeeded", idx);
            packet.ReadSingle("HitRoll", idx);
            packet.ReadSingle("HitNeeded", idx);
            packet.ReadSingle("MissChance", idx);
            packet.ReadSingle("DodgeChance", idx);
            packet.ReadSingle("ParryChance", idx);
            packet.ReadSingle("BlockChance", idx);
            packet.ReadSingle("GlanceChance", idx);
            packet.ReadSingle("CrushChance", idx);
        }

        [Parser(Opcode.SMSG_SPELL_NON_MELEE_DAMAGE_LOG)]
        public static void HandleSpellNonMeleeDmgLog(Packet packet)
        {
            packet.ReadPackedGuid128("Me");
            packet.ReadPackedGuid128("CasterGUID");
            packet.ReadPackedGuid128("CastID");

            packet.ReadInt32<SpellId>("SpellID");
            if (ClientVersion.AddedInVersion(ClientVersionBuild.V7_2_0_23826))
                packet.ReadInt32("SpellXSpellVisualID");
            packet.ReadInt32("Damage");
            packet.ReadInt32("OverKill");

            packet.ReadByte("SchoolMask");

            packet.ReadInt32("Absorbed");
            packet.ReadInt32("Resisted");
            packet.ReadInt32("ShieldBlock");

            packet.ResetBitReader();

            packet.ReadBit("Periodic");

            packet.ReadBitsE<AttackerStateFlags>("Flags", 7);

            var hasDebugData = packet.ReadBit("HasDebugData");
            var hasLogData = packet.ReadBit("HasLogData");
            var hasSandboxScaling = packet.ReadBit("HasSandboxScaling");

            if (ClientVersion.AddedInVersion(ClientVersionBuild.V7_2_0_23826))
            {
                if (hasLogData)
                    SpellHandler.ReadSpellCastLogData(packet, "SpellCastLogData");

                if (hasDebugData)
                    ReadSpellNonMeleeDebugData(packet, "DebugData");
            }
            else
            {
                if (hasDebugData)
                    ReadSpellNonMeleeDebugData(packet, "DebugData");

                if (hasLogData)
                    SpellHandler.ReadSpellCastLogData(packet, "SpellCastLogData");
            }

            if (hasSandboxScaling)
                SpellHandler.ReadSandboxScalingData(packet, "SandboxScalingData");
        }

        [Parser(Opcode.SMSG_ATTACKER_STATE_UPDATE)]
        public static void HandleAttackerStateUpdate(Packet packet)
        {
            var hasLogData = packet.ReadBit("HasLogData");

            if (hasLogData)
                SpellHandler.ReadSpellCastLogData(packet);

            packet.ReadInt32("Size");

            UnitMeleeAttackLog attackData = ReadAttackRoundInfo(packet, "AttackRoundInfo");
            attackData.Time = packet.Time;
            Storage.StoreUnitAttackLog(attackData);
        }

        [Parser(Opcode.SMSG_SPELL_PERIODIC_AURA_LOG, ClientVersionBuild.V7_0_3_22248, ClientVersionBuild.V7_2_0_23826)]
        public static void HandleSpellPeriodicAuraLog(Packet packet)
        {
            packet.ReadPackedGuid128("TargetGUID");
            packet.ReadPackedGuid128("CasterGUID");

            packet.ReadInt32<SpellId>("SpellID");

            var periodicAuraLogEffectCount = packet.ReadInt32("PeriodicAuraLogEffectCount");
            for (var i = 0; i < periodicAuraLogEffectCount; i++)
                ReadPeriodicAuraLogEffectData(packet, "PeriodicAuraLogEffectData");

            packet.ResetBitReader();

            var hasLogData = packet.ReadBit("HasLogData");
            if (hasLogData)
                SpellHandler.ReadSpellCastLogData(packet, "SpellCastLogData");
        }

        [Parser(Opcode.SMSG_SPELL_PERIODIC_AURA_LOG, ClientVersionBuild.V7_2_0_23826)]
        public static void HandleSpellPeriodicAuraLog720(Packet packet)
        {
            packet.ReadPackedGuid128("TargetGUID");
            packet.ReadPackedGuid128("CasterGUID");

            packet.ReadInt32<SpellId>("SpellID");

            var periodicAuraLogEffectCount = packet.ReadInt32("PeriodicAuraLogEffectCount");

            packet.ResetBitReader();
            var hasLogData = packet.ReadBit("HasLogData");

            for (var i = 0; i < periodicAuraLogEffectCount; i++)
                ReadPeriodicAuraLogEffectData(packet, "PeriodicAuraLogEffectData");

            if (hasLogData)
                SpellHandler.ReadSpellCastLogData(packet, "SpellCastLogData");
        }

        [Parser(Opcode.SMSG_SPELL_HEAL_LOG)]
        public static void HandleSpellHealLog(Packet packet)
        {
            packet.ReadPackedGuid128("TargetGUID");
            packet.ReadPackedGuid128("CasterGUID");

            packet.ReadInt32<SpellId>("SpellID");
            packet.ReadInt32("Health");
            packet.ReadInt32("OverHeal");
            packet.ReadInt32("Absorbed");

            packet.ResetBitReader();

            packet.ReadBit("Crit");
            var hasCritRollMade = packet.ReadBit("HasCritRollMade");
            var hasCritRollNeeded = packet.ReadBit("HasCritRollNeeded");
            var hasLogData = packet.ReadBit("HasLogData");
            var hasSandboxScaling = packet.ReadBit("HasLogData");

            if (ClientVersion.AddedInVersion(ClientVersionBuild.V7_2_0_23826))
            {
                if (hasLogData)
                    SpellHandler.ReadSpellCastLogData(packet);
            }

            if (hasCritRollMade)
                packet.ReadSingle("CritRollMade");

            if (hasCritRollNeeded)
                packet.ReadSingle("CritRollNeeded");

            if (ClientVersion.RemovedInVersion(ClientVersionBuild.V7_2_0_23826))
            {
                if (hasLogData)
                    SpellHandler.ReadSpellCastLogData(packet);
            }

            if (hasSandboxScaling)
                SpellHandler.ReadSandboxScalingData(packet, "SandboxScalingData");
        }

        [Parser(Opcode.SMSG_SPELL_ENERGIZE_LOG, ClientVersionBuild.V7_2_0_23826)]
        public static void HandleSpellEnergizeLog(Packet packet)
        {
            packet.ReadPackedGuid128("CasterGUID");
            packet.ReadPackedGuid128("TargetGUID");

            packet.ReadInt32<SpellId>("SpellID");
            packet.ReadUInt32E<PowerType>("Type");

            packet.ReadInt32("Amount");
            packet.ReadInt32("OverEnergize");

            packet.ResetBitReader();

            var bit100 = packet.ReadBit("HasLogData");
            if (bit100)
                V6_0_2_19033.Parsers.SpellHandler.ReadSpellCastLogData(packet);
        }
    }
}
