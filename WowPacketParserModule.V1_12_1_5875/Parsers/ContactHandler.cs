using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using WowPacketParser.Enums;
using WowPacketParser.Enums.Version;
using WowPacketParser.Misc;
using WowPacketParser.Store;
using WowPacketParser.Store.Objects;


namespace WowPacketParser.Parsing.Parsers
{
    public static class ContactHandler
    {
        [Parser(Opcode.SMSG_CONTACT_LIST)]
        public static void HandleContactList(Packet packet)
        {
            var count = packet.ReadByte("Count");

            for (var i = 0; i < count; i++)
            {
                packet.ReadGuid("GUID");
                var status = packet.ReadByte("Status");
                if (status != 0)
                {
                    packet.ReadUInt32("Area");
                    packet.ReadUInt32("Level");
                    packet.ReadUInt32("Class");
                }
            }

            if (packet.CanRead())
                WardenHandler.ReadCheatCheckDecryptionBlock(packet);
        }
    }
}
