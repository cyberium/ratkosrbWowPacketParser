using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;
using WowPacketParser.Store;
using WowPacketParser.Store.Objects;
using CoreParsers = WowPacketParser.Parsing.Parsers;

namespace WowPacketParserModule.V5_4_8_18291.Parsers
{
    public static class WorldStateHandler
    {
        [Parser(Opcode.SMSG_INIT_WORLD_STATES)]
        public static void HandleInitWorldStates(Packet packet)
        {
            uint map = packet.ReadUInt32<MapId>("Map ID");
            int areaId = CoreParsers.WorldStateHandler.CurrentAreaId = packet.ReadInt32<AreaId>("Area Id");
            int zoneId = CoreParsers.WorldStateHandler.CurrentZoneId = packet.ReadInt32<ZoneId>("Zone Id");

            var numFields = packet.ReadBits("Field Count", 21);

            for (var i = 0; i < numFields; i++)
            {
                WorldStateInit wsData = new WorldStateInit();
                wsData.AreaId = areaId;
                wsData.ZoneId = zoneId;
                wsData.Value = packet.ReadInt32();
                wsData.Variable = packet.ReadInt32();
                packet.AddValue("Field", wsData.Variable + " - Value: " + wsData.Value, i);
                wsData.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(packet.Time);
                Storage.WorldStateInits.Add(wsData);
                packet.AddSniffData(StoreNameType.WorldState, wsData.Variable, wsData.Value.ToString());
            }
        }

        [Parser(Opcode.SMSG_UPDATE_WORLD_STATE)]
        public static void HandleUpdateWorldState(Packet packet)
        {
            packet.ReadBit("bit18");
            WorldStateUpdate wsData = new WorldStateUpdate();
            wsData.Value = packet.ReadInt32();
            wsData.Variable = packet.ReadInt32();
            packet.AddValue("Field", wsData.Variable + " - Value: " + wsData.Value);
            wsData.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(packet.Time);
            Storage.WorldStateUpdates.Add(wsData);
            packet.AddSniffData(StoreNameType.WorldState, wsData.Variable, wsData.Value.ToString());
        }
    }
}
