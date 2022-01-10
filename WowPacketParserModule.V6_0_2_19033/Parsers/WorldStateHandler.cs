using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;
using WowPacketParser.Store;
using WowPacketParser.Store.Objects;
using CoreParsers = WowPacketParser.Parsing.Parsers;

namespace WowPacketParserModule.V6_0_2_19033.Parsers
{
    public static class WorldStateHandler
    {
        public static void ReadWorldStateBlock(out int variable, out int value, Packet packet, params object[] indexes)
        {
            variable = packet.ReadInt32();
            value = packet.ReadInt32();
            packet.AddValue("VariableID", variable + " - Value: " + value, indexes);
        }

        [Parser(Opcode.SMSG_INIT_WORLD_STATES)]
        public static void HandleInitWorldStates(Packet packet)
        {
            
            uint map = packet.ReadUInt32<MapId>("MapID");
            int zoneId = CoreParsers.WorldStateHandler.CurrentZoneId = packet.ReadInt32<ZoneId>("AreaId");
            int areaId = CoreParsers.WorldStateHandler.CurrentAreaId = packet.ReadInt32<AreaId>("SubareaID");

            var numFields = packet.ReadInt32("Field Count");
            for (var i = 0; i < numFields; i++)
            {
                WorldStateInit wsData = new WorldStateInit();
                wsData.Map = map;
                wsData.ZoneId = zoneId;
                wsData.AreaId = areaId;
                ReadWorldStateBlock(out wsData.Variable, out wsData.Value, packet);
                wsData.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(packet.Time);
                Storage.WorldStateInits.Add(wsData);
                packet.AddSniffData(StoreNameType.WorldState, wsData.Variable, wsData.Value.ToString());
            }  
        }

        [Parser(Opcode.SMSG_UPDATE_WORLD_STATE)]
        public static void HandleUpdateWorldState(Packet packet)
        {
            WorldStateUpdate wsData = new WorldStateUpdate();
            ReadWorldStateBlock(out wsData.Variable, out wsData.Value, packet);
            packet.ReadBit("Hidden");
            wsData.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(packet.Time);
            Storage.WorldStateUpdates.Add(wsData);
            packet.AddSniffData(StoreNameType.WorldState, wsData.Variable, wsData.Value.ToString());
        }
    }
}
