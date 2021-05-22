using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;
using WowPacketParser.Store;
using WowPacketParser.Store.Objects;

namespace WowPacketParserModule.V5_4_7_17898.Parsers
{
    public static class ReputationHandler
    {
        [Parser(Opcode.CMSG_RESET_FACTION_CHEAT)]
        public static void HandleResetFactionCheat(Packet packet)
        {
            packet.ReadUInt32("Unk Int");
            packet.ReadUInt32("Faction Id");
        }

        [Parser(Opcode.SMSG_INITIALIZE_FACTIONS)]
        public static void HandleInitializeFactions(Packet packet)
        {
            Storage.ClearTemporaryReputationList();

            for (var i = 0; i < 256; i++)
            {
                CharacterReputationData repData = new CharacterReputationData();
                repData.Faction = (uint)i;
                repData.Flags = (uint)packet.ReadByteE<FactionFlag>("Faction Flags", i);
                repData.Standing = (int)packet.ReadUInt32E<ReputationRank>("Faction Standing", i);
                Storage.StoreCharacterReputation(WowGuid.Empty, repData);
            }

            for (var i = 0; i < 256; i++)
                packet.ReadBit("Count", i);
        }
    }
}
