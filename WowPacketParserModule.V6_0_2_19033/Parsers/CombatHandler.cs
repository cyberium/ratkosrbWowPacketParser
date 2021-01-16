using System;
using System.Collections.Generic;
using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;
using WowPacketParser.Store;
using WowPacketParser.Store.Objects;
using SpellParsers = WowPacketParserModule.V6_0_2_19033.Parsers.SpellHandler;

namespace WowPacketParserModule.V6_0_2_19033.Parsers
{
    public static class CombatHandler
    {
        public static UnitMeleeAttackLog ReadAttackRoundInfo(Packet packet, params object[] indexes)
        {
            UnitMeleeAttackLog attackData = new UnitMeleeAttackLog();
            var hitInfo = packet.ReadInt32E<SpellHitInfo>("HitInfo");
            attackData.HitInfo = (uint)hitInfo;

            attackData.Attacker = packet.ReadPackedGuid128("Attacker Guid");
            attackData.Victim = packet.ReadPackedGuid128("Target Guid");

            attackData.Damage = (uint)packet.ReadInt32("Damage");
            attackData.OverkillDamage = packet.ReadInt32("OverDamage");

            var subDmgCount = packet.ReadBool("HasSubDmg");
            if (subDmgCount)
            {
                packet.ReadInt32("SchoolMask");
                packet.ReadSingle("Float Damage");
                packet.ReadInt32("Int Damage");

                if (hitInfo.HasAnyFlag(SpellHitInfo.HITINFO_PARTIAL_ABSORB | SpellHitInfo.HITINFO_FULL_ABSORB))
                    packet.ReadInt32("Damage Absorbed");

                if (hitInfo.HasAnyFlag(SpellHitInfo.HITINFO_PARTIAL_RESIST | SpellHitInfo.HITINFO_FULL_RESIST))
                    packet.ReadInt32("Damage Resisted");
            }

            attackData.VictimState = (uint)packet.ReadByteE<VictimStates>("VictimState");
            attackData.AttackerState = packet.ReadInt32("AttackerState");

            attackData.SpellId = (uint)packet.ReadInt32<SpellId>("Melee Spell ID");

            if (hitInfo.HasAnyFlag(SpellHitInfo.HITINFO_BLOCK))
                attackData.BlockedDamage = packet.ReadInt32("Block Amount");

            if (hitInfo.HasAnyFlag(SpellHitInfo.HITINFO_RAGE_GAIN))
                packet.ReadInt32("Rage Gained");

            if (hitInfo.HasAnyFlag(SpellHitInfo.HITINFO_UNK0))
            {
                packet.ReadInt32("Unk Attacker State 3 1");
                packet.ReadSingle("Unk Attacker State 3 2");
                packet.ReadSingle("Unk Attacker State 3 3");
                packet.ReadSingle("Unk Attacker State 3 4");
                packet.ReadSingle("Unk Attacker State 3 5");
                packet.ReadSingle("Unk Attacker State 3 6");
                packet.ReadSingle("Unk Attacker State 3 7");
                packet.ReadSingle("Unk Attacker State 3 8");
                packet.ReadSingle("Unk Attacker State 3 9");
                packet.ReadSingle("Unk Attacker State 3 10");
                packet.ReadSingle("Unk Attacker State 3 11");
                packet.ReadInt32("Unk Attacker State 3 12");
            }

            if (hitInfo.HasAnyFlag(SpellHitInfo.HITINFO_BLOCK | SpellHitInfo.HITINFO_UNK12))
                packet.ReadSingle("Unk Float");
            return attackData;
        }

        [Parser(Opcode.SMSG_ATTACKER_STATE_UPDATE)]
        public static void HandleAttackerStateUpdate(Packet packet)
        {
            var bit52 = packet.ReadBit("HasLogData");

            if (bit52)
                SpellParsers.ReadSpellCastLogData(packet);

            packet.ReadInt32("Size");

            UnitMeleeAttackLog attackData = ReadAttackRoundInfo(packet);
            attackData.Time = packet.Time;
            Storage.StoreUnitAttackLog(attackData);
        }

        [Parser(Opcode.SMSG_ATTACK_START)]
        public static void HandleAttackStartStart(Packet packet)
        {
            WowGuid attackerGuid = packet.ReadPackedGuid128("Attacker Guid");
            WowGuid victimGuid = packet.ReadPackedGuid128("Victim Guid");
            Storage.StoreUnitAttackToggle(attackerGuid, victimGuid, packet.Time, true);
        }

        [Parser(Opcode.SMSG_ATTACK_STOP)]
        public static void HandleAttackStartStop(Packet packet)
        {
            WowGuid attackerGuid = packet.ReadPackedGuid128("Attacker Guid");
            WowGuid victimGuid = packet.ReadPackedGuid128("Victim Guid");
            packet.ReadBit("NowDead");
            Storage.StoreUnitAttackToggle(attackerGuid, victimGuid, packet.Time, false);
        }

        [Parser(Opcode.SMSG_HIGHEST_THREAT_UPDATE)]
        public static void HandleHighestThreatlistUpdate(Packet packet)
        {
            CreatureThreatUpdate update = new CreatureThreatUpdate();
            WowGuid guid = packet.ReadPackedGuid128("UnitGUID");
            packet.ReadPackedGuid128("HighestThreatGUID");

            var count = packet.ReadUInt32("ThreatListCount");
            update.TargetsCount = (uint)count;

            if (count > 0)
            {
                update.TargetsList = new List<Tuple<WowGuid, uint>>();
                for (int i = 0; i < count; i++)
                {
                    WowGuid target = packet.ReadPackedGuid128("TargetGUID", i);
                    uint threat = packet.ReadUInt32("Threat", i);
                    update.TargetsList.Add(new Tuple<WowGuid, uint>(target, threat));
                }
            }

            update.Time = packet.Time;
            Storage.StoreCreatureThreatUpdate(guid, update);
        }

        [Parser(Opcode.SMSG_THREAT_UPDATE)]
        public static void HandleThreatlistUpdate(Packet packet)
        {
            CreatureThreatUpdate update = new CreatureThreatUpdate();
            WowGuid guid = packet.ReadPackedGuid128("UnitGUID");
            var count = packet.ReadInt32("Targets");
            update.TargetsCount = (uint)count;

            if (count > 0)
            {
                update.TargetsList = new List<Tuple<WowGuid, uint>>();
                for (int i = 0; i < count; i++)
                {
                    WowGuid target = packet.ReadPackedGuid128("TargetGUID", i);
                    uint threat = (uint)packet.ReadInt32("Threat", i);
                    update.TargetsList.Add(new Tuple<WowGuid, uint>(target, threat));
                }
            }

            update.Time = packet.Time;
            Storage.StoreCreatureThreatUpdate(guid, update);
        }

        [Parser(Opcode.SMSG_THREAT_REMOVE)]
        public static void HandleRemoveThreatlist(Packet packet)
        {
            packet.ReadPackedGuid128("UnitGUID");
            packet.ReadPackedGuid128("AboutGUID");
        }

        [Parser(Opcode.SMSG_THREAT_CLEAR)]
        public static void HandleClearThreatlist(Packet packet)
        {
            packet.ReadPackedGuid128("GUID");
        }

        [Parser(Opcode.SMSG_AI_REACTION)]
        public static void HandleAIReaction(Packet packet)
        {
            packet.ReadPackedGuid128("UnitGUID");
            packet.ReadInt32E<AIReaction>("Reaction");
        }

        [Parser(Opcode.CMSG_ATTACK_SWING)]
        public static void HandleAttackSwing(Packet packet)
        {
            packet.ReadPackedGuid128("Victim");
        }

        [Parser(Opcode.SMSG_CANCEL_AUTO_REPEAT)]
        public static void HandleCancelAutoRepeat(Packet packet)
        {
            packet.ReadPackedGuid128("Guid");
        }

        [Parser(Opcode.CMSG_SET_SHEATHED)]
        public static void HandleSetSheathed(Packet packet)
        {
            packet.ReadInt32E<SheathState>("CurrentSheathState");
            packet.ReadBit("Animate");
        }

        [Parser(Opcode.SMSG_PARTY_KILL_LOG)]
        public static void HandlePartyKillLog(Packet packet)
        {
            packet.ReadPackedGuid128("PlayerGUID");
            packet.ReadPackedGuid128("VictimGUID");
        }

        [Parser(Opcode.SMSG_ATTACK_SWING_ERROR)]
        public static void HandleAttackSwingError(Packet packet)
        {
            packet.ReadBitsE<AttackSwingErr>("Reason", 2);
        }

        [Parser(Opcode.SMSG_COMBAT_EVENT_FAILED)]
        public static void HandleAttackEventFailed(Packet packet)
        {
            packet.ReadPackedGuid128("Attacker");
            packet.ReadPackedGuid128("Victim");
        }

        [Parser(Opcode.SMSG_DUEL_WINNER)]
        public static void HandleDuelWinner(Packet packet)
        {
            var bits80 = packet.ReadBits(6);
            var bits24 = packet.ReadBits(6);

            packet.ReadBit("Fled");

            // Order guessed
            packet.ReadUInt32("BeatenVirtualRealmAddress");
            packet.ReadUInt32("WinnerVirtualRealmAddress");

            // Order guessed
            packet.ReadWoWString("BeatenName", bits80);
            packet.ReadWoWString("WinnerName", bits24);
        }

        [Parser(Opcode.SMSG_CAN_DUEL_RESULT)]
        public static void HandleCanDuelResult(Packet packet)
        {
            packet.ReadPackedGuid128("TargetGUID");
            packet.ReadBit("Result");
        }

        [Parser(Opcode.SMSG_DUEL_REQUESTED)]
        public static void HandleDuelRequested(Packet packet)
        {
            packet.ReadPackedGuid128("ArbiterGUID");
            packet.ReadPackedGuid128("RequestedByGUID");
            packet.ReadPackedGuid128("RequestedByWowAccount");
        }

        [Parser(Opcode.CMSG_DUEL_RESPONSE)]
        public static void HandleDuelResponse(Packet packet)
        {
            packet.ReadPackedGuid128("ArbiterGUID");
            packet.ReadBit("Accepted");
        }

        [Parser(Opcode.CMSG_CAN_DUEL)]
        public static void HandleCanDuel(Packet packet)
        {
            packet.ReadPackedGuid128("TargetGUID");
        }

        [Parser(Opcode.SMSG_PVP_CREDIT)]
        public static void HandlePvPCredit(Packet packet)
        {
            packet.ReadInt32("Honor");
            packet.ReadPackedGuid128("Target");
            packet.ReadInt32("Rank");
        }
    }
}
