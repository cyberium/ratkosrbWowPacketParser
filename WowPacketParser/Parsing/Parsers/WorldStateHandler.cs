using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Store;
using WowPacketParser.Store.Objects;

namespace WowPacketParser.Parsing.Parsers
{
    public static class WorldStateHandler
    {
        public static int CurrentAreaId = -1;
        public static int CurrentZoneId = -1;

        [Parser(Opcode.SMSG_INIT_WORLD_STATES)]
        public static void HandleInitWorldStates(Packet packet)
        {
            uint map = packet.ReadUInt32<MapId>("Map ID");
            int zoneId = CurrentZoneId = packet.ReadInt32<ZoneId>("Zone Id");
            int areaId = ClientVersion.AddedInVersion(ClientVersionBuild.V2_1_0_6692) ? CurrentAreaId = packet.ReadInt32<AreaId>("Area Id") : 0;

            var numFields = packet.ReadInt16("Field Count");
            for (var i = 0; i < numFields; i++)
            {
                WorldStateInit wsData = new WorldStateInit();
                wsData.Map = map;
                wsData.ZoneId = zoneId;
                wsData.AreaId = areaId;
                ReadWorldStateBlock(out wsData.Variable, out wsData.Value, packet, i);
                wsData.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(packet.Time);
                Storage.WorldStateInits.Add(wsData);
            }
        }

        public static void ReadWorldStateBlock(out int variable, out int value, Packet packet, params object[] indexes)
        {
            variable = packet.ReadInt32();
            value = packet.ReadInt32();
            packet.AddValue("Field", variable + " - Value: " + value, indexes);
        }

        [Parser(Opcode.SMSG_UPDATE_WORLD_STATE)]
        public static void HandleUpdateWorldState(Packet packet)
        {
            WorldStateUpdate wsData = new WorldStateUpdate();
            ReadWorldStateBlock(out wsData.Variable, out wsData.Value, packet);
            wsData.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(packet.Time);
            Storage.WorldStateUpdates.Add(wsData);

            if (ClientVersion.AddedInVersion(ClientVersionBuild.V4_2_2_14545))
                packet.ReadByte("Unk byte");
        }

        [Parser(Opcode.SMSG_UI_TIME)]
        [Parser(Opcode.SMSG_SERVER_TIME_OFFSET)]
        public static void HandleUITimer(Packet packet)
        {
            if (ClientVersion.IsVersionWith64BitTime())
                packet.ReadTime64("Time");
            else
                packet.ReadTime("Time");
        }
    }
}
