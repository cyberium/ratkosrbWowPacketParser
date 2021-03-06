using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;
using WowPacketParser.Store;
using CoreParsers = WowPacketParser.Parsing.Parsers;

namespace WowPacketParserModule.V6_0_2_19033.Parsers
{
    public static class TaxiHandler
    {
        [HasSniffData]
        [Parser(Opcode.CMSG_ACTIVATE_TAXI, ClientVersionBuild.V6_0_2_19033, ClientVersionBuild.V6_1_0_19678)]
        public static void HandleActivateTaxi60x(Packet packet)
        {
            WowGuid guid = packet.ReadPackedGuid128("GUID");
            uint startNode = packet.ReadUInt32("StartNode");
            uint destNode = packet.ReadUInt32("DestNode");
            packet.AddSniffData(StoreNameType.Taxi, (int)guid.GetEntry(), startNode.ToString() + "-" + destNode.ToString());
        }

        [HasSniffData]
        [Parser(Opcode.CMSG_ACTIVATE_TAXI, ClientVersionBuild.V6_1_0_19678)]
        public static void HandleActivateTaxi61x(Packet packet)
        {
            WowGuid guid = packet.ReadPackedGuid128("GUID");
            uint startNode = Storage.CurrentTaxiNode;
            uint destNode = packet.ReadUInt32("DestNode");
            packet.AddSniffData(StoreNameType.Taxi, (int)guid.GetEntry(), startNode.ToString() + "-" + destNode.ToString());
        }

        [Parser(Opcode.CMSG_ACTIVATE_TAXI_EXPRESS)]
        public static void HandleActivateTaxiExpress(Packet packet)
        {
            packet.ReadPackedGuid128("Unit");

            var count = packet.ReadUInt32("PathNodesCount");
            for (var i = 0; i < count; ++i)
                packet.ReadUInt32("PathNodes", i);
        }

        [Parser(Opcode.CMSG_ENABLE_TAXI_NODE)]
        [Parser(Opcode.CMSG_TAXI_QUERY_AVAILABLE_NODES)]
        public static void HandleTaxiStatusQuery(Packet packet)
        {
            packet.ReadPackedGuid128("Unit");
        }

        [Parser(Opcode.SMSG_SHOW_TAXI_NODES)]
        public static void HandleShowTaxiNodes(Packet packet)
        {
            var bit56 = packet.ReadBit("HasWindowInfo");
            var int16 = packet.ReadInt32("NodesCount");
            if (bit56)
            {
                packet.ReadPackedGuid128("UnitGUID");
                Storage.CurrentTaxiNode = packet.ReadUInt32("CurrentNode");
            }

            for (int i = 0; i < int16; ++i)
                packet.ReadByte("Nodes", i);

            CoreParsers.NpcHandler.LastGossipOption.Reset();
            CoreParsers.NpcHandler.TempGossipOptionPOI.Reset();
        }

        [Parser(Opcode.SMSG_TAXI_NODE_STATUS)]
        public static void HandleTaxiStatus(Packet packet)
        {
            packet.ReadPackedGuid128("Unit");
            packet.ReadBits("Status", 2);
        }

        [Parser(Opcode.CMSG_TAXI_NODE_STATUS_QUERY)]
        public static void HandleTaxinodeStatusQuery(Packet packet)
        {
            packet.ReadPackedGuid128("UnitGUID");
        }

        [Parser(Opcode.SMSG_ACTIVATE_TAXI_REPLY)]
        public static void HandleActivateTaxiReply(Packet packet)
        {
            packet.ReadBitsE<TaxiError>("Result", 4);
        }
    }
}
