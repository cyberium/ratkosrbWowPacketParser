using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Store;
using WowPacketParser.Store.Objects;

namespace WowPacketParser.Parsing.Parsers
{
    public static class ReputationHandler
    {
        [Parser(Opcode.CMSG_RESET_FACTION_CHEAT)]
        public static void HandleResetFactionCheat(Packet packet)
        {
            packet.ReadUInt32("Faction Id");
        }

        [Parser(Opcode.SMSG_INITIALIZE_FACTIONS)]
        public static void HandleInitializeFactions(Packet packet)
        {
            Storage.ClearTemporaryReputationList();

            var count = packet.ReadInt32("Count");
            for (var i = 0; i < count; i++)
            {
                CharacterReputationData repData = new CharacterReputationData();
                repData.Faction = (uint)i;
                repData.Flags = (uint)packet.ReadByteE<FactionFlag>("Faction Flags", i);
                repData.Standing = (int)packet.ReadUInt32E<ReputationRank>("Faction Standing", i);
                Storage.StoreCharacterReputation(WowGuid64.Empty, repData);
            }
        }

        [Parser(Opcode.SMSG_SET_FACTION_VISIBLE)]
        [Parser(Opcode.CMSG_SET_WATCHED_FACTION)]
        [Parser(Opcode.SMSG_SET_FACTION_NOT_VISIBLE)]
        public static void HandleSetFactionMisc(Packet packet)
        {
            packet.ReadUInt32("FactionIndex");
        }

        [Parser(Opcode.SMSG_SET_FORCED_REACTIONS)]
        public static void HandleForcedReactions(Packet packet)
        {
            var counter = packet.ReadInt32("Factions");
            for (var i = 0; i < counter; i++)
            {
                packet.ReadUInt32("Faction Id");
                packet.ReadUInt32("Reputation Rank");
            }
        }

        [Parser(Opcode.SMSG_SET_FACTION_STANDING)]
        public static void HandleSetFactionStanding(Packet packet)
        {
            if (ClientVersion.AddedInVersion(ClientVersionBuild.V2_4_0_8089))
                packet.ReadSingle("Reputation loss");

            bool showVisual = false;
            if (ClientVersion.AddedInVersion(ClientType.WrathOfTheLichKing))
                showVisual = packet.ReadBool("Show Visual");

            var count = packet.ReadInt32("Count");
            for (var i = 0; i < count; i++)
            {
                FactionStandingUpdate update = new FactionStandingUpdate();
                update.ReputationListId = packet.ReadInt32("Reputation List Id");
                update.Standing = packet.ReadInt32("Standing");
                update.ShowVisual = showVisual;
                update.Time = packet.Time;
                Storage.FactionStandingUpdates.Add(update);
            }
        }

        [Parser(Opcode.CMSG_SET_FACTION_INACTIVE)]
        public static void HandleSetFactionInactive(Packet packet)
        {
            packet.ReadUInt32("Index");
            packet.ReadBool("State");
        }

        [Parser(Opcode.CMSG_SET_FACTION_AT_WAR)]
        public static void HandleSetFactionAtWar(Packet packet)
        {
            packet.ReadUInt32("Faction Id");
            packet.ReadBool("At War");
        }
    }
}
