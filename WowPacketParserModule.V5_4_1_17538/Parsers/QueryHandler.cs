using System.Diagnostics.CodeAnalysis;
using WowPacketParser.Enums;
using WowPacketParser.Hotfix;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;
using WowPacketParser.Store;
using WowPacketParser.Store.Objects;

namespace WowPacketParserModule.V5_4_1_17538.Parsers
{
    [SuppressMessage("ReSharper", "UseObjectOrCollectionInitializer")]
    public static class QueryHandler
    {
        [Parser(Opcode.CMSG_QUERY_CREATURE)]
        public static void HandleCreatureQuery(Packet packet)
        {
            packet.ReadInt32("Entry");
        }

        [HasSniffData]
        [Parser(Opcode.SMSG_QUERY_CREATURE_RESPONSE)]
        public static void HandleCreatureQueryResponse(Packet packet)
        {
            var entry = packet.ReadEntry("Entry");
            Bit hasData = packet.ReadBit();
            if (!hasData)
                return; // nothing to do

            CreatureTemplate creature = new CreatureTemplate
            {
                Entry = (uint)entry.Key
            };

            packet.ReadBits(11);
            uint qItemCount = packet.ReadBits(22);
            int lenS4 = (int)packet.ReadBits(6);
            creature.RacialLeader = packet.ReadBit("Racial Leader");

            var stringLens = new int[4][];
            for (int i = 0; i < 4; i++)
            {
                stringLens[i] = new int[2];
                stringLens[i][0] = (int)packet.ReadBits(11);
                stringLens[i][1] = (int)packet.ReadBits(11);
            }

            int lenS5 = (int)packet.ReadBits(11);

            packet.ResetBitReader();

            creature.Family = packet.ReadInt32E<CreatureFamily>("Family");
            creature.RequiredExpansion = packet.ReadUInt32E<ClientType>("Expansion");
            creature.Type = packet.ReadInt32E<CreatureType>("Type");

            if (lenS5 > 1)
                creature.SubName = packet.ReadCString("Sub Name");

            creature.DisplayIDs = new uint?[4];
            creature.DisplayIDs[0] = packet.ReadUInt32("Display ID 0");
            creature.DisplayIDs[3] = packet.ReadUInt32("Display ID 3");

            //TODO: move to creature_questitems
            //creature.QuestItems = new uint[qItemCount];
            for (int i = 0; i < qItemCount; ++i)
                /*creature.QuestItems[i] = (uint)*/packet.ReadInt32<ItemId>("Quest Item", i);

            var name = new string[4];
            var femaleName = new string[4];
            for (int i = 0; i < 4; ++i)
            {
                if (stringLens[i][0] > 1)
                    name[i] = packet.ReadCString("Name", i);
                if (stringLens[i][1] > 1)
                    femaleName[i] = packet.ReadCString("Female Name", i);
            }
            creature.Name = name[0];
            creature.FemaleName = femaleName[0];

            if (lenS4 > 1)
                creature.IconName = packet.ReadCString("Icon Name");

            creature.TypeFlags = packet.ReadUInt32E<CreatureTypeFlag>("Type Flags");
            creature.TypeFlags2 = packet.ReadUInt32("Creature Type Flags 2"); // Missing enum

            creature.HealthMultiplier = packet.ReadSingle("Modifier 1");

            creature.Rank = packet.ReadInt32E<CreatureRank>("Rank");

            creature.KillCredits = new uint?[2];
            for (int i = 0; i < 2; ++i)
                creature.KillCredits[i] = packet.ReadUInt32("Kill Credit", i);

            creature.ManaMultiplier = packet.ReadSingle("Modifier 2");

            creature.MovementID = packet.ReadUInt32("Movement ID");

            creature.DisplayIDs[1] = packet.ReadUInt32("Display ID 1");
            creature.DisplayIDs[2] = packet.ReadUInt32("Display ID 2");

            packet.AddSniffData(StoreNameType.Unit, entry.Key, "QUERY_RESPONSE");

            Storage.CreatureTemplates.Add(creature, packet.TimeSpan);

            if (ClientLocale.PacketLocale != LocaleConstant.enUS)
            {
                CreatureTemplateLocale localesCreature = new CreatureTemplateLocale
                {
                    ID = (uint)entry.Key,
                    Name = creature.Name,
                    NameAlt = creature.FemaleName,
                    Title = creature.SubName,
                    TitleAlt = creature.TitleAlt
                };

                Storage.LocalesCreatures.Add(localesCreature, packet.TimeSpan);
            }

            ObjectName objectName = new ObjectName
            {
                ObjectType = StoreNameType.Unit,
                ID = entry.Key,
                Name = creature.Name
            };
            Storage.ObjectNames.Add(objectName, packet.TimeSpan);
        }

        [Parser(Opcode.CMSG_QUERY_PLAYER_NAME)]
        public static void HandlePlayerQueryName(Packet packet)
        {
            var guid = new byte[8];

            guid[1] = packet.ReadBit();
            guid[3] = packet.ReadBit();
            guid[6] = packet.ReadBit();
            guid[7] = packet.ReadBit();
            guid[2] = packet.ReadBit();
            guid[5] = packet.ReadBit();
            var bit20 = packet.ReadBit("bit20");
            guid[0] = packet.ReadBit();
            var bit28 = packet.ReadBit("bit28");
            guid[4] = packet.ReadBit();

            packet.ParseBitStream(guid, 4, 6, 7, 1, 2, 5, 0, 3);

            if (bit20)
                packet.ReadUInt32("unk20");

            if (bit28)
                packet.ReadUInt32("unk28");
            packet.WriteGuid("Guid", guid);
        }

        [HasSniffData]
        [Parser(Opcode.SMSG_DB_REPLY)]
        public static void HandleDBReply(Packet packet)
        {
            packet.ReadTime("Hotfix date");
            var type = packet.ReadUInt32E<DB2Hash>("DB2 File");

            var size = packet.ReadInt32("Size");
            var data = packet.ReadBytes(size);
            var db2File = new Packet(data, packet.Opcode, packet.Time, packet.Direction, packet.Number, packet.Writer, packet.FileName);

            var entry = packet.ReadInt32("Entry");
            if (entry < 0)
            {
                packet.WriteLine("Row {0} has been removed.", -entry);
                HotfixStoreMgr.RemoveRecord(type, entry);
            }
            else
            {
                packet.AddSniffData(StoreNameType.None, entry, type.ToString());
                HotfixStoreMgr.AddRecord(type, entry, db2File);
                db2File.ClosePacket(false);
            }
        }

        [Parser(Opcode.CMSG_DB_QUERY_BULK)]
        public static void HandleDBQueryBulk(Packet packet)
        {
            packet.ReadInt32E<DB2Hash>("DB2 File");
            var count = packet.ReadBits(21);

            var guids = new byte[count][];
            for (var i = 0; i < count; ++i)
            {
                guids[i] = new byte[8];
                packet.StartBitStream(guids[i], 1, 7, 2, 5, 0, 6, 3, 4);
            }

            packet.ResetBitReader();
            for (var i = 0; i < count; ++i)
            {
                packet.ReadXORBytes(guids[i], 4, 7, 6, 0, 2, 3);
                packet.ReadInt32("Entry", i);
                packet.ReadXORBytes(guids[i], 5, 1);
                packet.WriteGuid("Guid", guids[i], i);
            }
        }

        [Parser(Opcode.SMSG_REALM_QUERY_RESPONSE)]
        public static void HandleRealmQueryResponse(Packet packet)
        {
            packet.ReadByte("Unk byte");
            packet.ReadInt32("Realm Id");

            var bits278 = packet.ReadBits(8);
            packet.ReadBit();
            var bits22 = packet.ReadBits(8);

            packet.ReadWoWString("Realmname", bits22);
            packet.ReadWoWString("Realmname (without white char)", bits278);
        }

        [HasSniffData]
        [Parser(Opcode.SMSG_QUERY_PAGE_TEXT_RESPONSE)]
        public static void HandlePageTextResponse(Packet packet)
        {
            uint entry = packet.ReadUInt32("Entry");

            Bit hasData = packet.ReadBit();
            if (!hasData)
                return; // nothing to do

            PageText pageText = new PageText();
            pageText.ID = entry;

            uint textLen = packet.ReadBits(12);

            packet.ResetBitReader();

            pageText.Text = packet.ReadWoWString("Page Text", textLen);

            pageText.NextPageID = packet.ReadUInt32("Next Page");
            packet.ReadUInt32("Entry");

            packet.AddSniffData(StoreNameType.PageText, (int)entry, "QUERY_RESPONSE");
            Storage.PageTexts.Add(pageText, packet.TimeSpan);
        }
    }
}
