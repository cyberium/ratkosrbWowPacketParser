using System.Collections.Generic;
using System.Globalization;
using WowPacketParser.Enums.Version.V1_12_1_5875;
using WowPacketParser.Enums.Version.V3_3_5a_12340;
using WowPacketParser.Enums.Version.V4_0_3_13329;
using WowPacketParser.Enums.Version.V4_0_6_13596;
using WowPacketParser.Enums.Version.V4_1_0_13914;
using WowPacketParser.Enums.Version.V4_2_0_14480;
using WowPacketParser.Enums.Version.V4_2_2_14545;
using WowPacketParser.Enums.Version.V4_3_0_15005;
using WowPacketParser.Enums.Version.V4_3_2_15211;
using WowPacketParser.Enums.Version.V4_3_3_15354;
using WowPacketParser.Enums.Version.V4_3_4_15595;
using WowPacketParser.Enums.Version.V5_0_4_16016;
using WowPacketParser.Enums.Version.V5_0_5_16048;
using WowPacketParser.Enums.Version.V5_1_0_16309;
using WowPacketParser.Enums.Version.V5_2_0_16650;
using WowPacketParser.Enums.Version.V5_3_0_16981;
using WowPacketParser.Enums.Version.V5_4_0_17359;
using WowPacketParser.Enums.Version.V5_4_1_17538;
using WowPacketParser.Enums.Version.V5_4_2_17658;
using WowPacketParser.Enums.Version.V5_4_7_17898;
using WowPacketParser.Enums.Version.V5_4_8_18291;
using WowPacketParser.Enums.Version.V6_0_2_19033;
using WowPacketParser.Enums.Version.V6_0_3_19103;
using WowPacketParser.Enums.Version.V6_1_0_19678;
using WowPacketParser.Enums.Version.V6_1_2_19802;
using WowPacketParser.Enums.Version.V6_2_0_20173;
using WowPacketParser.Enums.Version.V6_2_2_20444;
using WowPacketParser.Enums.Version.V6_2_3_20726;
using WowPacketParser.Enums.Version.V6_2_4_21315;
using WowPacketParser.Enums.Version.V7_0_3_22248;
using WowPacketParser.Enums.Version.V7_1_0_22900;
using WowPacketParser.Enums.Version.V7_1_5_23360;
using WowPacketParser.Enums.Version.V7_2_0_23826;
using WowPacketParser.Enums.Version.V7_2_5_24330;
using WowPacketParser.Enums.Version.V7_3_0_24920;
using WowPacketParser.Enums.Version.V7_3_2_25383;
using WowPacketParser.Enums.Version.V7_3_5_25848;
using WowPacketParser.Enums.Version.V8_0_1_27101;
using WowPacketParser.Enums.Version.V8_1_0_28724;
using WowPacketParser.Enums.Version.V8_1_5_29620;
using WowPacketParser.Enums.Version.V8_2_0_30898;
using WowPacketParser.Enums.Version.V8_2_5_31921;
using WowPacketParser.Enums.Version.V8_3_0_33062;
using WowPacketParser.Enums.Version.V8_3_7_35249;
using WowPacketParser.Enums.Version.V9_0_1_36216;
using WowPacketParser.Enums.Version.V9_0_2_36639;
using WowPacketParser.Enums.Version.V1_13_2_31446;
using WowPacketParser.Enums.Version.V1_13_4_33598;
using WowPacketParser.Enums.Version.v1_13_5_34713;
using WowPacketParser.Enums.Version.V1_13_6_36231;
using WowPacketParser.Misc;

namespace WowPacketParser.Enums.Version
{
    public static class Opcodes
    {
        private static BiDictionary<Opcode, int> _serverDict = GetOpcodeDictionary(OpcodeDefiningBuild, Direction.ServerToClient);
        private static BiDictionary<Opcode, int> _clientDict = GetOpcodeDictionary(OpcodeDefiningBuild, Direction.ClientToServer);
        private static BiDictionary<Opcode, int> _miscDict = GetOpcodeDictionary(OpcodeDefiningBuild, Direction.Bidirectional);

        private static readonly Dictionary<ClientVersionBuild, Dictionary<Opcode, string>> ServerNameDict = new Dictionary<ClientVersionBuild, Dictionary<Opcode, string>>();
        private static readonly Dictionary<ClientVersionBuild, Dictionary<Opcode, string>> ClientNameDict = new Dictionary<ClientVersionBuild, Dictionary<Opcode, string>>();
        private static readonly Dictionary<ClientVersionBuild, Dictionary<Opcode, string>> MiscNameDict = new Dictionary<ClientVersionBuild, Dictionary<Opcode, string>>();

        public static void InitializeOpcodeDictionary()
        {
            _serverDict = GetOpcodeDictionary(OpcodeDefiningBuild, Direction.ServerToClient);
            _clientDict = GetOpcodeDictionary(OpcodeDefiningBuild, Direction.ClientToServer);
            _miscDict = GetOpcodeDictionary(OpcodeDefiningBuild, Direction.Bidirectional);

            InitializeOpcodeNameDictionary();
        }

        private static void InitializeOpcodeNameDictionary()
        {
            var tempDict = new Dictionary<Opcode, string>();
            if (!ServerNameDict.ContainsKey(OpcodeDefiningBuild))
            {
                foreach (var o in _serverDict)
                    tempDict.Add(o.Key, o.Key.ToString());

                ServerNameDict[OpcodeDefiningBuild] = new Dictionary<Opcode, string>(tempDict);
                tempDict.Clear();
            }

            if (!ClientNameDict.ContainsKey(OpcodeDefiningBuild))
            {
                foreach (var o in _clientDict)
                    tempDict.Add(o.Key, o.Key.ToString());

                ClientNameDict[OpcodeDefiningBuild] = new Dictionary<Opcode, string>(tempDict);
                tempDict.Clear();
            }

            if (!MiscNameDict.ContainsKey(OpcodeDefiningBuild))
            {
                foreach (var o in _miscDict)
                    tempDict.Add(o.Key, o.Key.ToString());

                MiscNameDict[OpcodeDefiningBuild] = new Dictionary<Opcode, string>(tempDict);
            }
        }

        public static ClientVersionBuild GetOpcodeDefiningBuild(ClientVersionBuild build)
        {
            switch (build)
            {
                case ClientVersionBuild.V1_12_1_5875:
                    return ClientVersionBuild.V1_12_1_5875;
                case ClientVersionBuild.V2_0_1_6180:
                case ClientVersionBuild.V2_0_3_6299:
                case ClientVersionBuild.V2_0_6_6337:
                case ClientVersionBuild.V2_1_0_6692:
                case ClientVersionBuild.V2_1_1_6739:
                case ClientVersionBuild.V2_1_2_6803:
                case ClientVersionBuild.V2_1_3_6898:
                case ClientVersionBuild.V2_2_0_7272:
                case ClientVersionBuild.V2_2_2_7318:
                case ClientVersionBuild.V2_2_3_7359:
                case ClientVersionBuild.V2_3_0_7561:
                case ClientVersionBuild.V2_3_2_7741:
                case ClientVersionBuild.V2_3_3_7799:
                case ClientVersionBuild.V2_4_0_8089:
                case ClientVersionBuild.V2_4_1_8125:
                case ClientVersionBuild.V2_4_2_8209:
                case ClientVersionBuild.V2_4_3_8606:
                case ClientVersionBuild.V3_0_2_9056:
                case ClientVersionBuild.V3_0_3_9183:
                case ClientVersionBuild.V3_0_8_9464:
                case ClientVersionBuild.V3_0_8a_9506:
                case ClientVersionBuild.V3_0_9_9551:
                case ClientVersionBuild.V3_1_0_9767:
                case ClientVersionBuild.V3_1_1_9806:
                case ClientVersionBuild.V3_1_1a_9835:
                case ClientVersionBuild.V3_1_2_9901:
                case ClientVersionBuild.V3_1_3_9947:
                case ClientVersionBuild.V3_2_0_10192:
                case ClientVersionBuild.V3_2_0a_10314:
                case ClientVersionBuild.V3_2_2_10482:
                case ClientVersionBuild.V3_2_2a_10505:
                case ClientVersionBuild.V3_3_0_10958:
                case ClientVersionBuild.V3_3_0a_11159:
                case ClientVersionBuild.V3_3_3_11685:
                case ClientVersionBuild.V3_3_3a_11723:
                case ClientVersionBuild.V3_3_5_12213:
                case ClientVersionBuild.V3_3_5a_12340:
                    return ClientVersionBuild.V3_3_5a_12340;
                case ClientVersionBuild.V4_0_1_13164:
                case ClientVersionBuild.V4_0_1a_13205:
                case ClientVersionBuild.V4_0_3_13329:
                    return ClientVersionBuild.V4_0_3_13329;
                case ClientVersionBuild.V4_0_6_13596:
                case ClientVersionBuild.V4_0_6a_13623:
                    return ClientVersionBuild.V4_0_6_13596;
                case ClientVersionBuild.V4_1_0_13914:
                case ClientVersionBuild.V4_1_0a_14007:
                    return ClientVersionBuild.V4_1_0_13914;
                case ClientVersionBuild.V4_2_0_14333:
                case ClientVersionBuild.V4_2_0a_14480:
                    return ClientVersionBuild.V4_2_0_14333;
                case ClientVersionBuild.V4_2_2_14545:
                    return ClientVersionBuild.V4_2_2_14545;
                case ClientVersionBuild.V4_3_0_15005:
                case ClientVersionBuild.V4_3_0a_15050:
                    return ClientVersionBuild.V4_3_0_15005;
                case ClientVersionBuild.V4_3_2_15211:
                    return ClientVersionBuild.V4_3_2_15211;
                case ClientVersionBuild.V4_3_3_15354:
                    return ClientVersionBuild.V4_3_3_15354;
                case ClientVersionBuild.V4_3_4_15595:
                    return ClientVersionBuild.V4_3_4_15595;
                case ClientVersionBuild.V5_0_4_16016:
                    return ClientVersionBuild.V5_0_4_16016;
                case ClientVersionBuild.V5_0_5_16048:
                case ClientVersionBuild.V5_0_5a_16057:
                case ClientVersionBuild.V5_0_5b_16135:
                    return ClientVersionBuild.V5_0_5_16048;
                case ClientVersionBuild.V5_1_0_16309:
                case ClientVersionBuild.V5_1_0a_16357:
                    return ClientVersionBuild.V5_1_0_16309;
                case ClientVersionBuild.V5_2_0_16650:
                case ClientVersionBuild.V5_2_0_16669:
                case ClientVersionBuild.V5_2_0_16683:
                case ClientVersionBuild.V5_2_0_16685:
                case ClientVersionBuild.V5_2_0_16701:
                case ClientVersionBuild.V5_2_0_16709:
                case ClientVersionBuild.V5_2_0_16716:
                case ClientVersionBuild.V5_2_0_16733:
                case ClientVersionBuild.V5_2_0_16769:
                case ClientVersionBuild.V5_2_0_16826:
                    return ClientVersionBuild.V5_2_0_16650;
                case ClientVersionBuild.V5_3_0_16981:
                case ClientVersionBuild.V5_3_0_16983:
                case ClientVersionBuild.V5_3_0_16992:
                case ClientVersionBuild.V5_3_0_17055:
                case ClientVersionBuild.V5_3_0_17116:
                case ClientVersionBuild.V5_3_0_17128:
                    return ClientVersionBuild.V5_3_0_16981;
                case ClientVersionBuild.V5_4_0_17359:
                case ClientVersionBuild.V5_4_0_17371:
                case ClientVersionBuild.V5_4_0_17399:
                    return ClientVersionBuild.V5_4_0_17359;
                case ClientVersionBuild.V5_4_1_17538:
                    return ClientVersionBuild.V5_4_1_17538;
                case ClientVersionBuild.V5_4_2_17658:
                case ClientVersionBuild.V5_4_2_17688:
                    return ClientVersionBuild.V5_4_2_17658;
                case ClientVersionBuild.V5_4_7_17898:
                case ClientVersionBuild.V5_4_7_17930:
                case ClientVersionBuild.V5_4_7_17956:
                case ClientVersionBuild.V5_4_7_18019:
                    return ClientVersionBuild.V5_4_7_17898;
                case ClientVersionBuild.V5_4_8_18291:
                case ClientVersionBuild.V5_4_8_18414:
                    return ClientVersionBuild.V5_4_8_18291;
                case ClientVersionBuild.V6_0_2_19033:
                case ClientVersionBuild.V6_0_2_19034:
                    return ClientVersionBuild.V6_0_2_19033;
                case ClientVersionBuild.V6_0_3_19103:
                case ClientVersionBuild.V6_0_3_19116:
                case ClientVersionBuild.V6_0_3_19243:
                case ClientVersionBuild.V6_0_3_19342:
                    return ClientVersionBuild.V6_0_3_19103;
                case ClientVersionBuild.V6_1_0_19678:
                case ClientVersionBuild.V6_1_0_19702:
                    return ClientVersionBuild.V6_1_0_19678;
                case ClientVersionBuild.V6_1_2_19802:
                case ClientVersionBuild.V6_1_2_19831:
                case ClientVersionBuild.V6_1_2_19865:
                    return ClientVersionBuild.V6_1_2_19802;
                case ClientVersionBuild.V6_2_0_20173:
                case ClientVersionBuild.V6_2_0_20182:
                case ClientVersionBuild.V6_2_0_20201:
                case ClientVersionBuild.V6_2_0_20216:
                case ClientVersionBuild.V6_2_0_20253:
                case ClientVersionBuild.V6_2_0_20338:
                    return ClientVersionBuild.V6_2_0_20173;
                case ClientVersionBuild.V6_2_2_20444:
                case ClientVersionBuild.V6_2_2a_20490:
                case ClientVersionBuild.V6_2_2a_20574:
                    return ClientVersionBuild.V6_2_2_20444;
                case ClientVersionBuild.V6_2_3_20726:
                case ClientVersionBuild.V6_2_3_20779:
                case ClientVersionBuild.V6_2_3_20886:
                    return ClientVersionBuild.V6_2_3_20726;
                case ClientVersionBuild.V6_2_4_21315:
                case ClientVersionBuild.V6_2_4_21336:
                case ClientVersionBuild.V6_2_4_21343:
                case ClientVersionBuild.V6_2_4_21345:
                case ClientVersionBuild.V6_2_4_21348:
                case ClientVersionBuild.V6_2_4_21355:
                case ClientVersionBuild.V6_2_4_21463:
                case ClientVersionBuild.V6_2_4_21676:
                case ClientVersionBuild.V6_2_4_21742:
                    return ClientVersionBuild.V6_2_4_21315;
                case ClientVersionBuild.V7_0_3_22248:
                case ClientVersionBuild.V7_0_3_22267:
                case ClientVersionBuild.V7_0_3_22277:
                case ClientVersionBuild.V7_0_3_22280:
                case ClientVersionBuild.V7_0_3_22289:
                case ClientVersionBuild.V7_0_3_22293:
                case ClientVersionBuild.V7_0_3_22345:
                case ClientVersionBuild.V7_0_3_22396:
                case ClientVersionBuild.V7_0_3_22410:
                case ClientVersionBuild.V7_0_3_22423:
                case ClientVersionBuild.V7_0_3_22445:
                case ClientVersionBuild.V7_0_3_22498:
                case ClientVersionBuild.V7_0_3_22522:
                case ClientVersionBuild.V7_0_3_22566:
                case ClientVersionBuild.V7_0_3_22594:
                case ClientVersionBuild.V7_0_3_22624:
                case ClientVersionBuild.V7_0_3_22747:
                case ClientVersionBuild.V7_0_3_22810:
                    return ClientVersionBuild.V7_0_3_22248;
                case ClientVersionBuild.V7_1_0_22900:
                case ClientVersionBuild.V7_1_0_22908:
                case ClientVersionBuild.V7_1_0_22950:
                case ClientVersionBuild.V7_1_0_22989:
                case ClientVersionBuild.V7_1_0_22995:
                case ClientVersionBuild.V7_1_0_22996:
                case ClientVersionBuild.V7_1_0_23171:
                case ClientVersionBuild.V7_1_0_23222:
                    return ClientVersionBuild.V7_1_0_22900;
                case ClientVersionBuild.V7_1_5_23360:
                case ClientVersionBuild.V7_1_5_23420:
                    return ClientVersionBuild.V7_1_5_23360;
                case ClientVersionBuild.V7_2_0_23706:
                case ClientVersionBuild.V7_2_0_23826:
                case ClientVersionBuild.V7_2_0_23835:
                case ClientVersionBuild.V7_2_0_23836:
                case ClientVersionBuild.V7_2_0_23846:
                case ClientVersionBuild.V7_2_0_23852:
                case ClientVersionBuild.V7_2_0_23857:
                case ClientVersionBuild.V7_2_0_23877:
                case ClientVersionBuild.V7_2_0_23911:
                case ClientVersionBuild.V7_2_0_23937:
                case ClientVersionBuild.V7_2_0_24015:
                    return ClientVersionBuild.V7_2_0_23706;
                case ClientVersionBuild.V7_2_5_24330:
                case ClientVersionBuild.V7_2_5_24367:
                case ClientVersionBuild.V7_2_5_24414:
                case ClientVersionBuild.V7_2_5_24415:
                case ClientVersionBuild.V7_2_5_24430:
                case ClientVersionBuild.V7_2_5_24461:
                case ClientVersionBuild.V7_2_5_24742:
                    return ClientVersionBuild.V7_2_5_24330;
                case ClientVersionBuild.V7_3_0_24920:
                case ClientVersionBuild.V7_3_0_24931:
                case ClientVersionBuild.V7_3_0_24956:
                case ClientVersionBuild.V7_3_0_24970:
                case ClientVersionBuild.V7_3_0_24974:
                case ClientVersionBuild.V7_3_0_25021:
                case ClientVersionBuild.V7_3_0_25195:
                    return ClientVersionBuild.V7_3_0_24920;
                case ClientVersionBuild.V7_3_2_25383:
                case ClientVersionBuild.V7_3_2_25442:
                case ClientVersionBuild.V7_3_2_25455:
                case ClientVersionBuild.V7_3_2_25477:
                case ClientVersionBuild.V7_3_2_25480:
                case ClientVersionBuild.V7_3_2_25497:
                case ClientVersionBuild.V7_3_2_25549:
                    return ClientVersionBuild.V7_3_2_25383;
                case ClientVersionBuild.V7_3_5_25848:
                case ClientVersionBuild.V7_3_5_25860:
                case ClientVersionBuild.V7_3_5_25864:
                case ClientVersionBuild.V7_3_5_25875:
                case ClientVersionBuild.V7_3_5_25881:
                case ClientVersionBuild.V7_3_5_25901:
                case ClientVersionBuild.V7_3_5_25928:
                case ClientVersionBuild.V7_3_5_25937:
                case ClientVersionBuild.V7_3_5_25944:
                case ClientVersionBuild.V7_3_5_25946:
                case ClientVersionBuild.V7_3_5_25950:
                case ClientVersionBuild.V7_3_5_25961:
                case ClientVersionBuild.V7_3_5_25996:
                case ClientVersionBuild.V7_3_5_26124:
                case ClientVersionBuild.V7_3_5_26365:
                case ClientVersionBuild.V7_3_5_26654:
                case ClientVersionBuild.V7_3_5_26755:
                case ClientVersionBuild.V7_3_5_26822:
                case ClientVersionBuild.V7_3_5_26899:
                case ClientVersionBuild.V7_3_5_26972:
                    return ClientVersionBuild.V7_3_5_25848;
                case ClientVersionBuild.V8_0_1_27101:
                case ClientVersionBuild.V8_0_1_27144:
                case ClientVersionBuild.V8_0_1_27165:
                case ClientVersionBuild.V8_0_1_27178:
                case ClientVersionBuild.V8_0_1_27219:
                case ClientVersionBuild.V8_0_1_27291:
                case ClientVersionBuild.V8_0_1_27326:
                case ClientVersionBuild.V8_0_1_27355:
                case ClientVersionBuild.V8_0_1_27356:
                case ClientVersionBuild.V8_0_1_27366:
                case ClientVersionBuild.V8_0_1_27377:
                case ClientVersionBuild.V8_0_1_27404:
                case ClientVersionBuild.V8_0_1_27481:
                case ClientVersionBuild.V8_0_1_27547:
                case ClientVersionBuild.V8_0_1_27602:
                case ClientVersionBuild.V8_0_1_27791:
                case ClientVersionBuild.V8_0_1_27843:
                case ClientVersionBuild.V8_0_1_27980:
                case ClientVersionBuild.V8_0_1_28153:
                    return ClientVersionBuild.V8_0_1_27101;
                case ClientVersionBuild.V8_1_0_28724:
                case ClientVersionBuild.V8_1_0_28768:
                case ClientVersionBuild.V8_1_0_28807:
                case ClientVersionBuild.V8_1_0_28822:
                case ClientVersionBuild.V8_1_0_28833:
                case ClientVersionBuild.V8_1_0_29088:
                case ClientVersionBuild.V8_1_0_29139:
                case ClientVersionBuild.V8_1_0_29235:
                case ClientVersionBuild.V8_1_0_29285:
                case ClientVersionBuild.V8_1_0_29297:
                case ClientVersionBuild.V8_1_0_29482:
                case ClientVersionBuild.V8_1_0_29600:
                case ClientVersionBuild.V8_1_0_29621:
                    return ClientVersionBuild.V8_1_0_28724;
                case ClientVersionBuild.V8_1_5_29683:
                case ClientVersionBuild.V8_1_5_29701:
                case ClientVersionBuild.V8_1_5_29704:
                case ClientVersionBuild.V8_1_5_29705:
                case ClientVersionBuild.V8_1_5_29718:
                case ClientVersionBuild.V8_1_5_29732:
                case ClientVersionBuild.V8_1_5_29737:
                case ClientVersionBuild.V8_1_5_29814:
                case ClientVersionBuild.V8_1_5_29869:
                case ClientVersionBuild.V8_1_5_29896:
                case ClientVersionBuild.V8_1_5_29981:
                case ClientVersionBuild.V8_1_5_30477:
                case ClientVersionBuild.V8_1_5_30706:
                    return ClientVersionBuild.V8_1_5_29683;
                case ClientVersionBuild.V8_2_0_30898:
                case ClientVersionBuild.V8_2_0_30918:
                case ClientVersionBuild.V8_2_0_30920:
                case ClientVersionBuild.V8_2_0_30948:
                case ClientVersionBuild.V8_2_0_30993:
                case ClientVersionBuild.V8_2_0_31229:
                case ClientVersionBuild.V8_2_0_31429:
                case ClientVersionBuild.V8_2_0_31478:
                    return ClientVersionBuild.V8_2_0_30898;
                case ClientVersionBuild.V8_2_5_31921:
                case ClientVersionBuild.V8_2_5_31958:
                case ClientVersionBuild.V8_2_5_31960:
                case ClientVersionBuild.V8_2_5_31961:
                case ClientVersionBuild.V8_2_5_31984:
                case ClientVersionBuild.V8_2_5_32028:
                case ClientVersionBuild.V8_2_5_32144:
                case ClientVersionBuild.V8_2_5_32185:
                case ClientVersionBuild.V8_2_5_32265:
                case ClientVersionBuild.V8_2_5_32294:
                case ClientVersionBuild.V8_2_5_32305:
                case ClientVersionBuild.V8_2_5_32494:
                case ClientVersionBuild.V8_2_5_32580:
                case ClientVersionBuild.V8_2_5_32638:
                case ClientVersionBuild.V8_2_5_32722:
                case ClientVersionBuild.V8_2_5_32750:
                case ClientVersionBuild.V8_2_5_32978:
                    return ClientVersionBuild.V8_2_5_31921;
                case ClientVersionBuild.V8_3_0_33062:
                case ClientVersionBuild.V8_3_0_33073:
                case ClientVersionBuild.V8_3_0_33084:
                case ClientVersionBuild.V8_3_0_33095:
                case ClientVersionBuild.V8_3_0_33115:
                case ClientVersionBuild.V8_3_0_33169:
                case ClientVersionBuild.V8_3_0_33237:
                case ClientVersionBuild.V8_3_0_33369:
                case ClientVersionBuild.V8_3_0_33528:
                case ClientVersionBuild.V8_3_0_33724:
                case ClientVersionBuild.V8_3_0_33775:
                case ClientVersionBuild.V8_3_0_33941:
                case ClientVersionBuild.V8_3_0_34220:
                case ClientVersionBuild.V8_3_0_34601:
                case ClientVersionBuild.V8_3_0_34769:
                case ClientVersionBuild.V8_3_0_34963:
                    return ClientVersionBuild.V8_3_0_33062;
                case ClientVersionBuild.V8_3_7_35249:
                case ClientVersionBuild.V8_3_7_35284:
                case ClientVersionBuild.V8_3_7_35435:
                case ClientVersionBuild.V8_3_7_35662:
                    return ClientVersionBuild.V8_3_7_35249;
                case ClientVersionBuild.V9_0_1_36216:
                case ClientVersionBuild.V9_0_1_36228:
                case ClientVersionBuild.V9_0_1_36230:
                case ClientVersionBuild.V9_0_1_36247:
                case ClientVersionBuild.V9_0_1_36272:
                case ClientVersionBuild.V9_0_1_36322:
                case ClientVersionBuild.V9_0_1_36372:
                case ClientVersionBuild.V9_0_1_36492:
                case ClientVersionBuild.V9_0_1_36577:
                    return ClientVersionBuild.V9_0_1_36216;
                case ClientVersionBuild.V9_0_2_36639:
                case ClientVersionBuild.V9_0_2_36665:
                case ClientVersionBuild.V9_0_2_36671:
                case ClientVersionBuild.V9_0_2_36710:
                case ClientVersionBuild.V9_0_2_36734:
                case ClientVersionBuild.V9_0_2_36751:
                case ClientVersionBuild.V9_0_2_36753:
                case ClientVersionBuild.V9_0_2_36839:
                case ClientVersionBuild.V9_0_2_36949:
                    return ClientVersionBuild.V9_0_2_36639;
                case ClientVersionBuild.V1_13_2_31446:
                case ClientVersionBuild.V1_13_2_31650:
                case ClientVersionBuild.V1_13_2_31687:
                case ClientVersionBuild.V1_13_2_31727:
                case ClientVersionBuild.V1_13_2_31830:
                case ClientVersionBuild.V1_13_2_31882:
                case ClientVersionBuild.V1_13_2_32089:
                case ClientVersionBuild.V1_13_2_32421:
                case ClientVersionBuild.V1_13_2_32600:
                case ClientVersionBuild.V1_13_3_32790:
                case ClientVersionBuild.V1_13_3_32836:
                case ClientVersionBuild.V1_13_3_32887:
                case ClientVersionBuild.V1_13_3_33155:
                case ClientVersionBuild.V1_13_3_33302:
                case ClientVersionBuild.V1_13_3_33526:
                    return ClientVersionBuild.V1_13_2_31446;
                case ClientVersionBuild.V1_13_4_33598:
                case ClientVersionBuild.V1_13_4_33645:
                case ClientVersionBuild.V1_13_4_33728:
                case ClientVersionBuild.V1_13_4_33920:
                case ClientVersionBuild.v1_13_4_34219:
                case ClientVersionBuild.v1_13_4_34266:
                case ClientVersionBuild.v1_13_4_34600:
                case ClientVersionBuild.v1_13_4_34835:
                    return ClientVersionBuild.V1_13_4_33598;
                case ClientVersionBuild.v1_13_5_34713:
                case ClientVersionBuild.v1_13_5_34911:
                case ClientVersionBuild.v1_13_5_35000:
                case ClientVersionBuild.v1_13_5_35186:
                case ClientVersionBuild.v1_13_5_35395:
                case ClientVersionBuild.v1_13_5_35753:
                case ClientVersionBuild.v1_13_5_36035:
                case ClientVersionBuild.v1_13_5_36307:
                case ClientVersionBuild.v1_13_5_36325:
                    return ClientVersionBuild.v1_13_5_34713;
                case ClientVersionBuild.v1_13_6_36231:
                case ClientVersionBuild.V1_13_6_36324:
                case ClientVersionBuild.V1_13_6_36497:
                case ClientVersionBuild.V1_13_6_36524:
                case ClientVersionBuild.V1_13_6_36611:
                case ClientVersionBuild.V1_13_6_36714:
                case ClientVersionBuild.V1_13_6_36935:
                case ClientVersionBuild.V1_13_6_37497:
                case ClientVersionBuild.V1_13_7_38363:
                case ClientVersionBuild.V1_13_7_38386:
                case ClientVersionBuild.V1_13_7_38475:
                case ClientVersionBuild.V1_13_7_38631:
                case ClientVersionBuild.V1_13_7_38704:
                    return ClientVersionBuild.v1_13_6_36231;
                default:
                    return ClientVersionBuild.V3_3_5a_12340;
            }
        }
        public static ClientVersionBuild OpcodeDefiningBuild
        {
            get
            {
                return GetOpcodeDefiningBuild(ClientVersion.Build);
            }
        }

        public static BiDictionary<Opcode, int> GetOpcodeDictionary(ClientVersionBuild build, Direction direction)
        {
            switch (GetOpcodeDefiningBuild(build))
            {
                case ClientVersionBuild.V1_12_1_5875:
                    return Opcodes_1_12_1.Opcodes(direction);
                case ClientVersionBuild.V3_3_5a_12340:
                    return Opcodes_3_3_5.Opcodes(direction);
                case ClientVersionBuild.V4_0_3_13329:
                    return Opcodes_4_0_3.Opcodes(direction);
                case ClientVersionBuild.V4_0_6_13596:
                    return Opcodes_4_0_6.Opcodes(direction);
                case ClientVersionBuild.V4_1_0_13914:
                    return Opcodes_4_1_0.Opcodes(direction);
                case ClientVersionBuild.V4_2_0_14333:
                    return Opcodes_4_2_0.Opcodes(direction);
                case ClientVersionBuild.V4_2_2_14545:
                    return Opcodes_4_2_2.Opcodes(direction);
                case ClientVersionBuild.V4_3_0_15005:
                    return Opcodes_4_3_0.Opcodes(direction);
                case ClientVersionBuild.V4_3_2_15211:
                    return Opcodes_4_3_2.Opcodes(direction);
                case ClientVersionBuild.V4_3_3_15354:
                    return Opcodes_4_3_3.Opcodes(direction);
                case ClientVersionBuild.V4_3_4_15595:
                    return Opcodes_4_3_4.Opcodes(direction);
                case ClientVersionBuild.V5_0_4_16016:
                    return Opcodes_5_0_4.Opcodes(direction);
                case ClientVersionBuild.V5_0_5_16048:
                    return Opcodes_5_0_5.Opcodes(direction);
                case ClientVersionBuild.V5_1_0_16309:
                    return Opcodes_5_1_0.Opcodes(direction);
                case ClientVersionBuild.V5_2_0_16650:
                    return Opcodes_5_2_0.Opcodes(direction);
                case ClientVersionBuild.V5_3_0_16981:
                    return Opcodes_5_3_0.Opcodes(direction);
                case ClientVersionBuild.V5_4_0_17359:
                    return Opcodes_5_4_0.Opcodes(direction);
                case ClientVersionBuild.V5_4_1_17538:
                    return Opcodes_5_4_1.Opcodes(direction);
                case ClientVersionBuild.V5_4_2_17658:
                    return Opcodes_5_4_2.Opcodes(direction);
                case ClientVersionBuild.V5_4_7_17898:
                    return Opcodes_5_4_7.Opcodes(direction);
                case ClientVersionBuild.V5_4_8_18291:
                    return Opcodes_5_4_8.Opcodes(direction);
                case ClientVersionBuild.V6_0_2_19033:
                    return Opcodes_6_0_2.Opcodes(direction);
                case ClientVersionBuild.V6_0_3_19103:
                    return Opcodes_6_0_3.Opcodes(direction);
                case ClientVersionBuild.V6_1_0_19678:
                    return Opcodes_6_1_0.Opcodes(direction);
                case ClientVersionBuild.V6_1_2_19802:
                    return Opcodes_6_1_2.Opcodes(direction);
                case ClientVersionBuild.V6_2_0_20173:
                    return Opcodes_6_2_0.Opcodes(direction);
                case ClientVersionBuild.V6_2_2_20444:
                    return Opcodes_6_2_2.Opcodes(direction);
                case ClientVersionBuild.V6_2_3_20726:
                    return Opcodes_6_2_3.Opcodes(direction);
                case ClientVersionBuild.V6_2_4_21315:
                    return Opcodes_6_2_4.Opcodes(direction);
                case ClientVersionBuild.V7_0_3_22248:
                    return Opcodes_7_0_3.Opcodes(direction);
                case ClientVersionBuild.V7_1_0_22900:
                    return Opcodes_7_1_0.Opcodes(direction);
                case ClientVersionBuild.V7_1_5_23360:
                    return Opcodes_7_1_5.Opcodes(direction);
                case ClientVersionBuild.V7_2_0_23706:
                    return Opcodes_7_2_0.Opcodes(direction);
                case ClientVersionBuild.V7_2_5_24330:
                    return Opcodes_7_2_5.Opcodes(direction);
                case ClientVersionBuild.V7_3_0_24920:
                    return Opcodes_7_3_0.Opcodes(direction);
                case ClientVersionBuild.V7_3_2_25383:
                    return Opcodes_7_3_2.Opcodes(direction);
                case ClientVersionBuild.V7_3_5_25848:
                    return Opcodes_7_3_5.Opcodes(direction);
                case ClientVersionBuild.V8_0_1_27101:
                    return Opcodes_8_0_1.Opcodes(direction);
                case ClientVersionBuild.V8_1_0_28724:
                    return Opcodes_8_1_0.Opcodes(direction);
                case ClientVersionBuild.V8_1_5_29683:
                    return Opcodes_8_1_5.Opcodes(direction);
                case ClientVersionBuild.V8_2_0_30898:
                    return Opcodes_8_2_0.Opcodes(direction);
                case ClientVersionBuild.V8_2_5_31921:
                    return Opcodes_8_2_5.Opcodes(direction);
                case ClientVersionBuild.V8_3_0_33062:
                    return Opcodes_8_3_0.Opcodes(direction);
                case ClientVersionBuild.V8_3_7_35249:
                    return Opcodes_8_3_7.Opcodes(direction);
                case ClientVersionBuild.V9_0_1_36216:
                    return Opcodes_9_0_1.Opcodes(direction);
                case ClientVersionBuild.V9_0_2_36639:
                    return Opcodes_9_0_2.Opcodes(direction);
                case ClientVersionBuild.V1_13_2_31446:
                    return Opcodes_1_13_2.Opcodes(direction);
                case ClientVersionBuild.V1_13_4_33598:
                    return Opcodes_1_13_4.Opcodes(direction);
                case ClientVersionBuild.v1_13_5_34713:
                    return Opcodes_1_13_5.Opcodes(direction);
                case ClientVersionBuild.v1_13_6_36231:
                    return Opcodes_1_13_6.Opcodes(direction);
                default:
                    return Opcodes_3_3_5.Opcodes(direction);
            }
        }

        public static Opcode GetOpcode(int opcodeId, Direction direction)
        {
            var opcode = Opcode.NULL_OPCODE;
            switch (direction)
            {
                case Direction.ClientToServer:
                    if (_clientDict.TryGetBySecond(opcodeId, out opcode))
                        return opcode;
                    if (_miscDict.TryGetBySecond(opcodeId, out opcode))
                        return opcode;
                    break;
                case Direction.ServerToClient:
                case Direction.Bidirectional:
                    if (_serverDict.TryGetBySecond(opcodeId, out opcode))
                        return opcode;
                    if (_miscDict.TryGetBySecond(opcodeId, out opcode))
                        return opcode;
                    break;
            }
            return opcode;
        }

        public static int GetOpcode(Opcode opcodeId, Direction direction)
        {
            var opcode = 0;
            switch (direction)
            {
                case Direction.ClientToServer:
                    if (_clientDict.TryGetByFirst(opcodeId, out opcode))
                        return opcode;
                    if (_miscDict.TryGetByFirst(opcodeId, out opcode))
                        return opcode;
                    break;
                case Direction.ServerToClient:
                case Direction.Bidirectional:
                    if (_serverDict.TryGetByFirst(opcodeId, out opcode))
                        return opcode;
                    if (_miscDict.TryGetByFirst(opcodeId, out opcode))
                        return opcode;
                    break;
            }

            return opcode;
        }

        public static string GetOpcodeName(int opcodeId, Direction direction, bool hex = true)
        {
            var opc = GetOpcode(opcodeId, direction);

            if (opc != 0)
            {
                string name;
                if (direction == Direction.ClientToServer || direction == Direction.BNClientToServer)
                {
                    if (ClientNameDict[OpcodeDefiningBuild].TryGetValue(opc, out name))
                        return name;
                    if (MiscNameDict[OpcodeDefiningBuild].TryGetValue(opc, out name))
                        return name;

                }
                else if (direction == Direction.ServerToClient || direction == Direction.BNServerToClient)
                {
                    if (ServerNameDict[OpcodeDefiningBuild].TryGetValue(opc, out name))
                        return name;
                    if (MiscNameDict[OpcodeDefiningBuild].TryGetValue(opc, out name))
                        return name;
                }
            }

            if (hex)
                return "0x" + opcodeId.ToString("X4", CultureInfo.InvariantCulture);

            return opcodeId.ToString();
        }
    }
}
