using System;
using System.Collections.Generic;
using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;
using WowPacketParser.Store;
using WowPacketParser.Store.Objects;

namespace WowPacketParserModule.V7_0_3_22248.Parsers
{
    public static class CombatHandler
    {
        [Parser(Opcode.SMSG_HIGHEST_THREAT_UPDATE, ClientVersionBuild.V7_2_5_24330)]
        public static void HandleHighestThreatlistUpdate(Packet packet)
        {
            CreatureThreatUpdate update = new CreatureThreatUpdate();
            WowGuid guid = packet.ReadPackedGuid128("UnitGUID");
            packet.ReadPackedGuid128("HighestThreatGUID");

            var count = packet.ReadUInt32("ThreatListCount");
            update.TargetsCount = (uint)count;

            if (count > 0)
            {
                update.TargetsList = new List<Tuple<WowGuid, long>>();
                for (int i = 0; i < count; i++)
                {
                    WowGuid target = packet.ReadPackedGuid128("UnitGUID", i);
                    long threat = (uint)packet.ReadInt64("Threat", i);
                    update.TargetsList.Add(new Tuple<WowGuid, long>(target, threat));
                }
            }

            update.Time = packet.Time;
            Storage.StoreCreatureThreatUpdate(guid, update);
        }

        [Parser(Opcode.SMSG_THREAT_UPDATE, ClientVersionBuild.V7_2_5_24330)]
        public static void HandleThreatlistUpdate(Packet packet)
        {
            CreatureThreatUpdate update = new CreatureThreatUpdate();
            WowGuid guid = packet.ReadPackedGuid128("UnitGUID");
            var count = packet.ReadInt32("ThreatListCount");
            update.TargetsCount = (uint)count;

            if (count > 0)
            {
                update.TargetsList = new List<Tuple<WowGuid, long>>();
                for (int i = 0; i < count; i++)
                {
                    WowGuid target = packet.ReadPackedGuid128("TargetGUID", i);
                    long threat = packet.ReadInt64("Threat", i);
                    update.TargetsList.Add(new Tuple<WowGuid, long>(target, threat));
                }
            }

            update.Time = packet.Time;
            Storage.StoreCreatureThreatUpdate(guid, update);
        }

        [Parser(Opcode.SMSG_PVP_CREDIT)]
        public static void HandlePvPCredit(Packet packet)
        {
            packet.ReadInt32("OriginalHonor");
            packet.ReadInt32("Honor");
            packet.ReadPackedGuid128("Target");
            packet.ReadInt32("Rank");
        }
    }
}
