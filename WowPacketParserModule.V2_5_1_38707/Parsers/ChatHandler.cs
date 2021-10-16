using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;
using WowPacketParser.Store;
using WowPacketParser.Store.Objects;

namespace WowPacketParserModule.V2_5_1_38707.Parsers
{
    public static class ChatHandler
    {
        [Parser(Opcode.SMSG_CHAT)]
        public static void HandleServerChatMessage(Packet packet)
        {
            if (ClientVersion.AddedInVersion(ClientVersionBuild.V2_5_2_39570))
            {
                WowPacketParserModule.V9_0_1_36216.Parsers.ChatHandler.HandleServerChatMessage(packet);
            }
            else
            {
                WowPacketParserModule.V1_13_2_31446.Parsers.ChatHandler.HandleServerChatMessage(packet);
            }
        }
    }
}
