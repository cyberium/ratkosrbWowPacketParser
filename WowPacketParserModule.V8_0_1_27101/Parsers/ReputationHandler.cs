using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;
using WowPacketParser.Store;
using WowPacketParser.Store.Objects;

namespace WowPacketParserModule.V8_0_1_27101.Parsers
{
    public static class ReputationHandler
    {
        public static readonly ushort FactionCount = 350; // 8.1.0

        [Parser(Opcode.SMSG_FACTION_BONUS_INFO, ClientVersionBuild.V8_1_0_28724)]
        public static void HandleFactionBonusInfo(Packet packet)
        {
            for (var i = 0; i < FactionCount; i++)
                packet.ReadBit("FactionHasBonus", i);
        }

        [Parser(Opcode.SMSG_INITIALIZE_FACTIONS, ClientVersionBuild.V8_1_0_28724)]
        public static void HandleInitializeFactions(Packet packet)
        {
            Storage.ClearTemporaryReputationList();

            for (var i = 0; i < FactionCount; i++)
            {
                CharacterReputationData repData = new CharacterReputationData();
                repData.Faction = (uint)i;
                repData.Flags = (uint)packet.ReadByteE<FactionFlag>("FactionFlags", i);
                repData.Standing = (int)packet.ReadUInt32E<ReputationRank>("FactionStandings", i);
                Storage.StoreCharacterReputation(repData);
            }

            for (var i = 0; i < FactionCount; i++)
                packet.ReadBit("FactionHasBonus", i);
        }
    }
}
