using System;
using System.Collections.Generic;
using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;
using WowPacketParser.Store;
using WowPacketParser.Store.Objects;

namespace WowPacketParserModule.V6_0_2_19033.Parsers
{
    public static class ReputationHandler
    {
        [Parser(Opcode.CMSG_REQUEST_FORCED_REACTIONS)]
        public static void HandleReputationZero(Packet packet)
        {
        }

        [Parser(Opcode.CMSG_RESET_FACTION_CHEAT)]
        public static void HandleResetFactionCheat(Packet packet)
        {
            packet.ReadUInt32("FactionId");
            packet.ReadInt32("Level");
        }

        [Parser(Opcode.SMSG_FACTION_BONUS_INFO)]
        public static void HandleFactionBonusInfo(Packet packet)
        {
            for (var i = 0; i < 0x100; i++)
                packet.ReadBit("FactionHasBonus", i);
        }

        [Parser(Opcode.SMSG_INITIALIZE_FACTIONS)]
        public static void HandleInitializeFactions(Packet packet)
        {
            for (var i = 0; i < 0x100; i++)
            {
                packet.ReadByteE<FactionFlag>("FactionFlags", i);
                packet.ReadUInt32E<ReputationRank>("FactionStandings", i);
            }

            for (var i = 0; i < 0x100; i++)
                packet.ReadBit("FactionHasBonus", i);
        }

        [Parser(Opcode.SMSG_SET_FORCED_REACTIONS)]
        public static void HandleForcedReactions(Packet packet)
        {
            var counter = packet.ReadBits("ForcedReactionCount", 6);

            for (var i = 0; i < counter; i++)
            {
                packet.ReadUInt32("Faction");
                packet.ReadUInt32("Reaction");
            }
        }

        [Parser(Opcode.SMSG_SET_FACTION_STANDING)]
        public static void HandleSetFactionStanding(Packet packet)
        {
            float rafBonus = packet.ReadSingle("ReferAFriendBonus");
            float achievementBonus = packet.ReadSingle("BonusFromAchievementSystem");

            var count = packet.ReadInt32();
            List<Tuple<int, int>> reputations = new List<Tuple<int, int>>();
            for (int i = 0; i < count; i++)
            {
                int reputationListId = packet.ReadInt32("Index");
                int standing = packet.ReadInt32("Standing");
                reputations.Add(new Tuple<int, int>(reputationListId, standing));
            }
            
            packet.ResetBitReader();
            bool showVisual = packet.ReadBit("ShowVisual");

            foreach (var faction in  reputations)
            {
                FactionStandingUpdate update = new FactionStandingUpdate();
                update.ShowVisual = showVisual;
                update.RAFBonus = rafBonus;
                update.AchievementBonus = achievementBonus;
                update.ReputationListId = faction.Item1;
                update.Standing = faction.Item2;
                update.Time = packet.Time;
                Storage.FactionStandingUpdates.Add(update);
            }
        }

        [Parser(Opcode.CMSG_SET_FACTION_AT_WAR)]
        public static void HandleSetFactionAtWar(Packet packet)
        {
            packet.ReadByte("FactionIndex");
        }

        [Parser(Opcode.CMSG_SET_FACTION_NOT_AT_WAR)]
        public static void HandleSetFactionNotAtWar(Packet packet)
        {
            packet.ReadByte("FactionIndex");
        }
    }
}
