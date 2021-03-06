using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;
using WowPacketParser.Store;
using CoreParsers = WowPacketParser.Parsing.Parsers;

namespace WowPacketParserModule.V7_0_3_22248.Parsers
{
    public static class TaxiHandler
    {
        [Parser(Opcode.SMSG_SHOW_TAXI_NODES, ClientVersionBuild.V7_3_0_24920)]
        public static void HandleShowTaxiNodes(Packet packet)
        {
            var hasWindowInfo = packet.ReadBit("HasWindowInfo");
            var canLandNodesCount = packet.ReadUInt32();
            var canUseNodesCount = packet.ReadUInt32();

            if (hasWindowInfo)
            {
                packet.ReadPackedGuid128("UnitGUID");
                Storage.CurrentTaxiNode = packet.ReadUInt32("CurrentNode");
            }

            for (var i = 0u; i < canLandNodesCount; ++i)
                packet.ReadByte("CanLandNodes", i);

            for (var i = 0u; i < canUseNodesCount; ++i)
                packet.ReadByte("CanUseNodes", i);

            CoreParsers.NpcHandler.LastGossipOption.Reset();
            CoreParsers.NpcHandler.TempGossipOptionPOI.Reset();
        }

        [HasSniffData]
        [Parser(Opcode.CMSG_ACTIVATE_TAXI)]
        public static void HandleActivateTaxi(Packet packet)
        {
            WowGuid guid = packet.ReadPackedGuid128("GUID");
            uint startNode = Storage.CurrentTaxiNode;
            uint destNode = packet.ReadUInt32("DestNode");
            packet.ReadUInt32("GroundMountID");
            packet.ReadUInt32("FlyingMountID");
            packet.AddSniffData(StoreNameType.Taxi, (int)guid.GetEntry(), startNode.ToString() + "-" + destNode.ToString());
        }
    }
}
