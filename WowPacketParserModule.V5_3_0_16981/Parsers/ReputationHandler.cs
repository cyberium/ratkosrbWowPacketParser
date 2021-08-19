using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;
using WowPacketParser.Store;
using WowPacketParser.Store.Objects;

namespace WowPacketParserModule.V5_3_0_16981.Parsers
{
    public static class ReputationHandler
    {
        [Parser(Opcode.SMSG_INITIALIZE_FACTIONS)]
        public static void HandleInitializeFactions(Packet packet)
        {
            Storage.ClearTemporaryReputationList();

            for (var i = 0; i < 256; i++)
            {
                CharacterReputationData repData = new CharacterReputationData();
                repData.Faction = (uint)i;
                repData.Standing = (int)packet.ReadUInt32E<ReputationRank>("Faction Standing", i);
                repData.Flags = (uint)packet.ReadByteE<FactionFlag>("Faction Flags", i);
                Storage.StoreCharacterReputation(WowGuid64.Empty, repData);
            }

            for (var i = 0; i < 256; i++)
            {
                var bit1296 = packet.ReadBit("Count");
            }
        }
    }
}
