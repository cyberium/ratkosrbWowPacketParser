using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;

namespace WowPacketParserModule.V9_0_1_36216.Parsers
{
    public static class QueryHandler
    {
        public static void ReadNameCacheLookupResult(Packet packet, params object[] idx)
        {
            packet.ResetBitReader();
            packet.ReadByte("Result", idx);
            packet.ReadPackedGuid128("Player", idx);
            var hasPlayerGuidLookupData = packet.ReadBit("HasPlayerGuidLookupData", idx);
            var hasThingy = packet.ReadBit("HasNameCacheUnused920", idx);

            if (hasPlayerGuidLookupData)
                V8_0_1_27101.Parsers.CharacterHandler.ReadPlayerGuidLookupData(packet, idx, "PlayerGuidLookupData");

            if (hasThingy)
            {
                packet.ResetBitReader();
                packet.ReadUInt32("Unused1", idx, "NameCacheUnused920");
                packet.ReadPackedGuid128("Unused2", idx, "NameCacheUnused920");
                var length = packet.ReadBits(7);
                packet.ReadWoWString("Unused3", length, idx, "NameCacheUnused920");
            }
        }

        [Parser(Opcode.CMSG_QUERY_PLAYER_NAME, ClientVersionBuild.V9_2_0_42423)]
        public static void HandleNameQuery(Packet packet)
        {
            var count = packet.ReadUInt32();
            for (var i = 0; i < count; ++i)
                packet.ReadPackedGuid128("Players", i);
        }

        [Parser(Opcode.SMSG_QUERY_PLAYER_NAME_RESPONSE, ClientVersionBuild.V9_2_0_42423)]
        public static void HandleNameQueryResponse(Packet packet)
        {
            var count = packet.ReadUInt32("Count");
            for (var i = 0; i < count; ++i)
                ReadNameCacheLookupResult(packet, i);
        }
    }
}
