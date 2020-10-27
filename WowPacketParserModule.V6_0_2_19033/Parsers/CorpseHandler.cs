using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;
using WowPacketParser.Store;

namespace WowPacketParserModule.V6_0_2_19033.Parsers
{
    public static class CorpseHandler
    {
        [Parser(Opcode.CMSG_QUERY_CORPSE_LOCATION_FROM_CLIENT)]
        public static void HandleCorpseZero(Packet packet)
        {
        }

        [Parser(Opcode.CMSG_RECLAIM_CORPSE)]
        public static void HandleReclaimCorpse(Packet packet)
        {
            packet.ReadPackedGuid128("CorpseGUID");
            Storage.ClientReclaimCorpseTimes.Add(new WowPacketParser.Store.Objects.ClientReclaimCorpse
            {
                UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(packet.Time)
            }, packet.TimeSpan);
        }

        [Parser(Opcode.SMSG_CORPSE_LOCATION)]
        public static void HandleCorpseLocation(Packet packet)
        {
            packet.ReadBit("Valid");
            packet.ReadInt32("ActualMapID");
            packet.ReadVector3("Position");
            packet.ReadInt32("MapID");
            packet.ReadPackedGuid128("Transport");
        }
    }
}
