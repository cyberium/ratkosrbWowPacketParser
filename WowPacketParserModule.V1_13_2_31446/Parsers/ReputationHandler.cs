using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;
using WowPacketParser.Store;
using WowPacketParser.Store.Objects;

namespace WowPacketParserModule.V1_13_2_31446.Parsers
{
    public static class ReputationHandler
    {
        public static ushort FactionCount = 300;

        [Parser(Opcode.SMSG_INITIALIZE_FACTIONS, ClientVersionBuild.V7_2_0_23826)]
        public static void HandleInitializeFactions(Packet packet)
        {
            Storage.ClearTemporaryReputationList();

            for (var i = 0; i < FactionCount; i++)
            {
                CharacterReputationData repData = new CharacterReputationData();
                repData.Faction = (uint)i;
                repData.Flags = (uint)packet.ReadByteE<FactionFlag>("FactionFlags", i);
                repData.Standing = (int)packet.ReadUInt32E<ReputationRank>("FactionStandings", i);
                Storage.StoreCharacterReputation(WowGuid64.Empty, repData);
            }

            for (var i = 0; i < FactionCount; i++)
                packet.ReadBit("FactionHasBonus", i);
        }
    }
}
