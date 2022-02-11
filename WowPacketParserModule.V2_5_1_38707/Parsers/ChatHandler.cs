using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;
using WowPacketParser.Store;

namespace WowPacketParserModule.V2_5_1_38835.Parsers
{
    public static class ChatHandler
    {
        [Parser(Opcode.SMSG_EMOTE)]
        public static void HandleEmote(Packet packet)
        {
            var guid = packet.ReadPackedGuid128("GUID");
            var emote = packet.ReadUInt32E<EmoteType>("Emote ID");
            var count = packet.ReadUInt32("SpellVisualKitCount");

            if (ClientVersion.AddedInVersion(ClientVersionBuild.V2_5_3_41531))
                packet.ReadInt32("Unk253");

            for (var i = 0; i < count; ++i)
                packet.ReadUInt32("SpellVisualKitID", i);

            Storage.StoreUnitEmote(guid, emote, packet);
        }
    }
}