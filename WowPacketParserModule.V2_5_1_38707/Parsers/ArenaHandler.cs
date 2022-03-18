using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;

namespace WowPacketParserModule.V2_5_1_38707.Parsers
{
    public static class ArenaHandler
    {
        [Parser(Opcode.CMSG_ARENA_TEAM_ROSTER)]
        [Parser(Opcode.CMSG_ARENA_TEAM_QUERY)]
        public static void HandleArenaTeamQuery(Packet packet)
        {
            packet.ReadUInt32("TeamId");
        }

        public static void ReadArenaTeamMemberInfo(Packet packet, params object[] idx)
        {
            packet.ReadPackedGuid128("MemberGUID", idx);
            packet.ReadByte("Online", idx); // ???????
            packet.ReadInt32("Captain", idx);
            packet.ReadByte("Level", idx);
            packet.ReadByteE<Class>("Class", idx);
            packet.ReadUInt32("WeekGamesPlayed", idx);
            packet.ReadUInt32("WeekGamesWon", idx);
            packet.ReadUInt32("SeasonGamesPlayed", idx);
            packet.ReadUInt32("SeasonGamesWon", idx);
            packet.ReadUInt32("PersonalRating", idx);
            packet.ResetBitReader();
            var strLen = packet.ReadBits(6);
            var byte64 = packet.ReadBit("byte64", idx);
            var byte6C = packet.ReadBit("byte6C", idx);
            packet.ReadWoWString("Name", strLen, idx);
            if (byte64)
                packet.ReadSingle("dword60", idx);
            if (byte6C)
                packet.ReadSingle("dword68", idx);
        }

        [Parser(Opcode.SMSG_ARENA_TEAM_ROSTER)]
        public static void HandleArenaNoIdeaYet(Packet packet)
        {
            packet.ReadInt32("dword20");
            packet.ReadUInt32("TeamSize");
            packet.ReadUInt32("TeamPlayed");
            packet.ReadUInt32("TeamWins");
            packet.ReadUInt32("SeasonPlayed");
            packet.ReadUInt32("SeasonWins");
            packet.ReadUInt32("TeamRating");
            packet.ReadUInt32("PlayerRating");
            var count = packet.ReadUInt32();
            for (var i = 0; i < count; i++)
                ReadArenaTeamMemberInfo(packet, i);
        }
    }
}
