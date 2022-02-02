using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WowPacketParser.Enums;
using WowPacketParser.Enums.Version;
using WowPacketParser.Misc;
using WowPacketParser.Store;
using WowPacketParser.Store.Objects;
using WowPacketParser.Store.Objects.UpdateFields;

namespace WowPacketParser.SQL.Builders
{
    [BuilderClass]
    public static class Characters
    {
        private static Random random = new Random();
        public static string GetRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string randomString = new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
            return randomString.Substring(0, 1) + randomString.Substring(1).ToLower();
        }

        public enum CharCustomizationOption
        {
            None,
            Skin,
            Face,
            HairStyle,
            HairColor,
            FacialHair,
        }

        public static CharCustomizationOption GetCustomizationOption(uint option)
        {
            switch (option)
            {
                /*
                case 1: // UNK - Skin Color
                    return 0;
                case 2: // UNK - Face
                    return 1;
                case 3: // UNK - Hair Style
                    return 2;
                case 4: // UNK - Hair Color
                    return 3;
                case 5: // UNK - Facial Hair
                    return 4;
                case 6: // UNK - Tattoo Style
                    return 5;
                case 7: // UNK - Horn Style
                    return 6;
                case 8: // UNK - Blindfolds
                    return 7;
                */
                case 9: // Human Male - Skin Color
                    return CharCustomizationOption.Skin;
                case 10: // Human Male - Face
                    return CharCustomizationOption.Face;
                case 11: // Human Male - Hair Style
                    return CharCustomizationOption.HairStyle;
                case 12: // Human Male - Hair Color
                    return CharCustomizationOption.HairColor;
                case 13: // Human Male - Facial Hair
                    return CharCustomizationOption.FacialHair;
                case 14: // Human Female - Skin Color
                    return CharCustomizationOption.Skin;
                case 15: // Human Female - Face
                    return CharCustomizationOption.Face;
                case 16: // Human Female - Hair Style
                    return CharCustomizationOption.HairStyle;
                case 17: // Human Female - Hair Color
                    return CharCustomizationOption.HairColor;
                case 18: // Human Female - Piercings
                    return CharCustomizationOption.FacialHair;
                case 19: // Orc Male - Skin Color
                    return CharCustomizationOption.Skin;
                case 20: // Orc Male - Face
                    return CharCustomizationOption.Face;
                case 21: // Orc Male - Hair Style
                    return CharCustomizationOption.HairStyle;
                case 22: // Orc Male - Hair Color
                    return CharCustomizationOption.HairColor;
                case 23: // Orc Male - Facial Hair
                    return CharCustomizationOption.FacialHair;
                case 25: // Orc Female - Skin Color
                    return CharCustomizationOption.Skin;
                case 26: // Orc Female - Face
                    return CharCustomizationOption.Face;
                case 27: // Orc Female - Hair Style
                    return CharCustomizationOption.HairStyle;
                case 28: // Orc Female - Hair Color
                    return CharCustomizationOption.HairColor;
                case 29: // Orc Female - Piercings
                    return CharCustomizationOption.FacialHair;
                case 30: // Dwarf Male - Skin Color
                    return CharCustomizationOption.Skin;
                case 31: // Dwarf Male - Face
                    return CharCustomizationOption.Face;
                case 32: // Dwarf Male - Hair Style
                    return CharCustomizationOption.HairStyle;
                case 33: // Dwarf Male - Hair Color
                    return CharCustomizationOption.HairColor;
                case 34: // Dwarf Male - Facial Hair
                    return CharCustomizationOption.FacialHair;
                case 35: // Dwarf Female - Skin Color
                    return CharCustomizationOption.Skin;
                case 36: // Dwarf Female - Face
                    return CharCustomizationOption.Face;
                case 37: // Dwarf Female - Hair Style
                    return CharCustomizationOption.HairStyle;
                case 38: // Dwarf Female - Hair Color
                    return CharCustomizationOption.HairColor;
                case 39: // Dwarf Female - Piercings
                    return CharCustomizationOption.FacialHair;
                case 40: // Night Elf Male - Skin Color
                    return CharCustomizationOption.Skin;
                case 41: // Night Elf Male - Face
                    return CharCustomizationOption.Face;
                case 42: // Night Elf Male - Hair Style
                    return CharCustomizationOption.HairStyle;
                case 43: // Night Elf Male - Hair Color
                    return CharCustomizationOption.HairColor;
                case 44: // Night Elf Male - Facial Hair
                    return CharCustomizationOption.FacialHair;
                case 49: // Night Elf Female - Skin Color
                    return CharCustomizationOption.Skin;
                case 50: // Night Elf Female - Face
                    return CharCustomizationOption.Face;
                case 51: // Night Elf Female - Hair Style
                    return CharCustomizationOption.HairStyle;
                case 52: // Night Elf Female - Hair Color
                    return CharCustomizationOption.HairColor;
                case 53: // Night Elf Female - Markings
                    return CharCustomizationOption.FacialHair;
                case 58: // Undead Male - Skin Color
                    return CharCustomizationOption.Skin;
                case 59: // Undead Male - Face
                    return CharCustomizationOption.Face;
                case 60: // Undead Male - Hair Style
                    return CharCustomizationOption.HairStyle;
                case 61: // Undead Male - Hair Color
                    return CharCustomizationOption.HairColor;
                case 62: // Undead Male - Features
                    return CharCustomizationOption.FacialHair;
                case 63: // Undead Female - Skin Color
                    return CharCustomizationOption.Skin;
                case 64: // Undead Female - Face
                    return CharCustomizationOption.Face;
                case 65: // Undead Female - Hair Style
                    return CharCustomizationOption.HairStyle;
                case 66: // Undead Female - Hair Color
                    return CharCustomizationOption.HairColor;
                case 67: // Undead Female - Features
                    return CharCustomizationOption.FacialHair;
                case 68: // Tauren Male - Skin Color
                    return CharCustomizationOption.Skin;
                case 71: // Tauren Male - Horn Style
                    return CharCustomizationOption.HairStyle;
                case 72: // Tauren Male - Horn Color
                    return CharCustomizationOption.HairColor;
                case 73: // Tauren Male - Facial Hair
                    return CharCustomizationOption.FacialHair;
                case 74: // Tauren Female - Skin Color
                    return CharCustomizationOption.Skin;
                case 77: // Tauren Female - Horn Style
                    return CharCustomizationOption.HairStyle;
                case 78: // Tauren Female - Horn Color
                    return CharCustomizationOption.HairColor;
                case 79: // Tauren Female - Hair
                    return CharCustomizationOption.FacialHair;
                case 80: // Gnome Male - Skin Color
                    return CharCustomizationOption.Skin;
                case 81: // Gnome Male - Face
                    return CharCustomizationOption.Face;
                case 82: // Gnome Male - Hair Style
                    return CharCustomizationOption.HairStyle;
                case 83: // Gnome Male - Hair Color
                    return CharCustomizationOption.HairColor;
                case 84: // Gnome Male - Facial Hair
                    return CharCustomizationOption.FacialHair;
                case 85: // Gnome Female - Skin Color
                    return CharCustomizationOption.Skin;
                case 86: // Gnome Female - Face
                    return CharCustomizationOption.Face;
                case 87: // Gnome Female - Hair Style
                    return CharCustomizationOption.HairStyle;
                case 88: // Gnome Female - Hair Color
                    return CharCustomizationOption.HairColor;
                case 89: // Gnome Female - Earrings
                    return CharCustomizationOption.FacialHair;
                case 90: // Troll Male - Skin Color
                    return CharCustomizationOption.Skin;
                case 91: // Troll Male - Face
                    return CharCustomizationOption.Face;
                case 92: // Troll Male - Hair Style
                    return CharCustomizationOption.HairStyle;
                case 93: // Troll Male - Hair Color
                    return CharCustomizationOption.HairColor;
                case 94: // Troll Male - Tusks
                    return CharCustomizationOption.FacialHair;
                case 95: // Troll Female - Skin Color
                    return CharCustomizationOption.Skin;
                case 96: // Troll Female - Face
                    return CharCustomizationOption.Face;
                case 97: // Troll Female - Hair Style
                    return CharCustomizationOption.HairStyle;
                case 98: // Troll Female - Hair Color
                    return CharCustomizationOption.HairColor;
                case 99: // Troll Female - Tusks
                    return CharCustomizationOption.FacialHair;
                case 100: // Goblin Male - Skin Color
                    return CharCustomizationOption.Skin;
                case 102: // Goblin Male - Hair Style
                    return CharCustomizationOption.Face;
                case 105: // Goblin Female - Skin Color
                    return CharCustomizationOption.Skin;
                case 110: // Blood Elf Male - Skin Color
                    return CharCustomizationOption.Skin;
                case 111: // Blood Elf Male - Face
                    return CharCustomizationOption.Face;
                case 112: // Blood Elf Male - Hair Style
                    return CharCustomizationOption.HairStyle;
                case 113: // Blood Elf Male - Hair Color
                    return CharCustomizationOption.HairColor;
                case 114: // Blood Elf Male - Facial Hair
                    return CharCustomizationOption.FacialHair;
                case 119: // Blood Elf Female - Skin Color
                    return CharCustomizationOption.Skin;
                case 120: // Blood Elf Female - Face
                    return CharCustomizationOption.Face;
                case 121: // Blood Elf Female - Hair Style
                    return CharCustomizationOption.HairStyle;
                case 122: // Blood Elf Female - Hair Color
                    return CharCustomizationOption.HairColor;
                case 123: // Blood Elf Female - Earrings
                    return CharCustomizationOption.FacialHair;
                case 128: // Draenei Male - Skin Color
                    return CharCustomizationOption.Skin;
                case 129: // Draenei Male - Face
                    return CharCustomizationOption.Face;
                case 130: // Draenei Male - Hair Style
                    return CharCustomizationOption.HairStyle;
                case 131: // Draenei Male - Hair Color
                    return CharCustomizationOption.HairColor;
                case 132: // Draenei Male - Facial Hair
                    return CharCustomizationOption.FacialHair;
                case 133: // Draenei Female - Skin Color
                    return CharCustomizationOption.Skin;
                case 134: // Draenei Female - Face
                    return CharCustomizationOption.Face;
                case 135: // Draenei Female - Hair Style
                    return CharCustomizationOption.HairStyle;
                case 136: // Draenei Female - Hair Color
                    return CharCustomizationOption.HairColor;
                case 137: // Draenei Female - Horn Style
                    return CharCustomizationOption.FacialHair;
                case 138: // Fel Orc Male - Skin Color
                    return CharCustomizationOption.Skin;
                case 139: // Fel Orc Male - Face
                    return CharCustomizationOption.Face;
                case 140: // Fel Orc Male - Hair Style
                    return CharCustomizationOption.HairStyle;
                case 141: // Fel Orc Male - Hair Color
                    return CharCustomizationOption.HairColor;
                case 142: // Fel Orc Female - Hair Style
                    return CharCustomizationOption.Skin;
                case 143: // Fel Orc Female - Hair Color
                    return CharCustomizationOption.Face;
                case 144: // Naga Male - Skin Color
                    return CharCustomizationOption.Skin;
                case 145: // Naga Male - Hair Style
                    return CharCustomizationOption.HairStyle;
                case 146: // Naga Male - Hair Color
                    return CharCustomizationOption.HairColor;
                case 147: // Naga Female - Skin Color
                    return CharCustomizationOption.Skin;
                case 148: // Naga Female - Hair Style
                    return CharCustomizationOption.HairStyle;
                case 149: // Naga Female - Hair Color
                    return CharCustomizationOption.HairColor;
                case 150: // Broken Male - Skin Color
                    return CharCustomizationOption.Skin;
                case 151: // Broken Male - Face
                    return CharCustomizationOption.Face;
                case 152: // Broken Male - Hair Style
                    return CharCustomizationOption.HairStyle;
                case 153: // Broken Male - Hair Color
                    return CharCustomizationOption.HairColor;
                case 154: // Broken Female - Hair Style
                    return CharCustomizationOption.Skin;
                case 155: // Broken Female - Hair Color
                    return CharCustomizationOption.Face;
                case 156: // Skeleton Male - Skin Color
                    return CharCustomizationOption.Skin;
                case 157: // Skeleton Male - Face
                    return CharCustomizationOption.Face;
                case 158: // Skeleton Male - Hair Style
                    return CharCustomizationOption.HairStyle;
                case 159: // Skeleton Male - Hair Color
                    return CharCustomizationOption.HairColor;
                case 160: // Skeleton Female - Hair Style
                    return CharCustomizationOption.Skin;
                case 161: // Skeleton Female - Hair Color
                    return CharCustomizationOption.Face;
                case 176: // Forest Troll Male - Skin Color
                    return CharCustomizationOption.Skin;
                case 177: // Forest Troll Male - Face
                    return CharCustomizationOption.Face;
                case 178: // Forest Troll Male - Hair Style
                    return CharCustomizationOption.HairStyle;
                case 179: // Forest Troll Male - Hair Color
                    return CharCustomizationOption.HairColor;
                case 180: // Forest Troll Male - Facial Hair
                    return CharCustomizationOption.FacialHair;
                case 181: // Forest Troll Female - Hair Style
                    return CharCustomizationOption.Skin;
                case 182: // Forest Troll Female - Hair Color
                    return CharCustomizationOption.Face;
                case 378: // Tauren Male - Face
                    return CharCustomizationOption.Face;
                case 379: // Tauren Female - Face
                    return CharCustomizationOption.Face;
                case 1000: // Fel Orc Male - Facial Hair
                    return CharCustomizationOption.FacialHair;
                case 1001: // Fel Orc Female - Facial Hair
                    return CharCustomizationOption.HairStyle;
                case 1002: // Naga Male - Face
                    return CharCustomizationOption.Face;
                case 1003: // Naga Male - Facial Hair
                    return CharCustomizationOption.FacialHair;
                case 1004: // Naga Female - Face
                    return CharCustomizationOption.Face;
                case 1005: // Naga Female - Facial Hair
                    return CharCustomizationOption.FacialHair;
                case 1006: // Broken Female - Facial Hair
                    return CharCustomizationOption.HairStyle;
                case 1007: // Skeleton Male - Facial Hair
                    return CharCustomizationOption.FacialHair;
                case 1008: // Skeleton Female - Facial Hair
                    return CharCustomizationOption.HairStyle;
                case 1009: // Forest Troll Female - Facial Hair
                    return CharCustomizationOption.HairStyle;
            }
            return CharCustomizationOption.None;
        }

        public static byte GetCustomizationChoice(uint value)
        {
            switch (value)
            {
                case 17160: // Human Male - Skin Color
                    return 0;
                case 17161: // Human Male - Skin Color
                    return 1;
                case 17162: // Human Male - Skin Color
                    return 2;
                case 17163: // Human Male - Skin Color
                    return 3;
                case 17164: // Human Male - Skin Color
                    return 4;
                case 17165: // Human Male - Skin Color
                    return 5;
                case 17166: // Human Male - Skin Color
                    return 6;
                case 17167: // Human Male - Skin Color
                    return 7;
                case 17168: // Human Male - Skin Color
                    return 8;
                case 17169: // Human Male - Skin Color
                    return 9;
                case 17170: // Human Male - Skin Color
                    return 10;
                case 17171: // Human Male - Skin Color
                    return 11;
                case 17172: // Human Male - Face
                    return 0;
                case 17173: // Human Male - Face
                    return 1;
                case 17174: // Human Male - Face
                    return 2;
                case 17175: // Human Male - Face
                    return 3;
                case 17176: // Human Male - Face
                    return 4;
                case 17177: // Human Male - Face
                    return 5;
                case 17178: // Human Male - Face
                    return 6;
                case 17179: // Human Male - Face
                    return 7;
                case 17180: // Human Male - Face
                    return 8;
                case 17181: // Human Male - Face
                    return 9;
                case 17182: // Human Male - Face
                    return 10;
                case 17183: // Human Male - Face
                    return 11;
                case 17184: // Human Male - Hair Style
                    return 0;
                case 17185: // Human Male - Hair Style
                    return 1;
                case 17186: // Human Male - Hair Style
                    return 2;
                case 17187: // Human Male - Hair Style
                    return 3;
                case 17188: // Human Male - Hair Style
                    return 4;
                case 17189: // Human Male - Hair Style
                    return 5;
                case 17190: // Human Male - Hair Style
                    return 6;
                case 17191: // Human Male - Hair Style
                    return 7;
                case 17192: // Human Male - Hair Style
                    return 8;
                case 17193: // Human Male - Hair Style
                    return 9;
                case 17194: // Human Male - Hair Style
                    return 10;
                case 17195: // Human Male - Hair Style
                    return 11;
                case 17196: // Human Male - Hair Color
                    return 0;
                case 17197: // Human Male - Hair Color
                    return 1;
                case 17198: // Human Male - Hair Color
                    return 2;
                case 17199: // Human Male - Hair Color
                    return 3;
                case 17200: // Human Male - Hair Color
                    return 4;
                case 17201: // Human Male - Hair Color
                    return 5;
                case 17202: // Human Male - Hair Color
                    return 6;
                case 17203: // Human Male - Hair Color
                    return 7;
                case 17204: // Human Male - Hair Color
                    return 8;
                case 17205: // Human Male - Hair Color
                    return 9;
                case 17206: // Human Male - Facial Hair
                    return 0;
                case 17207: // Human Male - Facial Hair
                    return 1;
                case 17208: // Human Male - Facial Hair
                    return 2;
                case 17209: // Human Male - Facial Hair
                    return 3;
                case 17210: // Human Male - Facial Hair
                    return 4;
                case 17211: // Human Male - Facial Hair
                    return 5;
                case 17212: // Human Male - Facial Hair
                    return 6;
                case 17213: // Human Male - Facial Hair
                    return 7;
                case 17214: // Human Male - Facial Hair
                    return 8;
                case 17215: // Human Female - Skin Color
                    return 0;
                case 17216: // Human Female - Skin Color
                    return 1;
                case 17217: // Human Female - Skin Color
                    return 2;
                case 17218: // Human Female - Skin Color
                    return 3;
                case 17219: // Human Female - Skin Color
                    return 4;
                case 17220: // Human Female - Skin Color
                    return 5;
                case 17221: // Human Female - Skin Color
                    return 6;
                case 17222: // Human Female - Skin Color
                    return 7;
                case 17223: // Human Female - Skin Color
                    return 8;
                case 17224: // Human Female - Skin Color
                    return 9;
                case 17225: // Human Female - Skin Color
                    return 10;
                case 17226: // Human Female - Skin Color
                    return 11;
                case 17227: // Human Female - Face
                    return 0;
                case 17228: // Human Female - Face
                    return 1;
                case 17229: // Human Female - Face
                    return 2;
                case 17230: // Human Female - Face
                    return 3;
                case 17231: // Human Female - Face
                    return 4;
                case 17232: // Human Female - Face
                    return 5;
                case 17233: // Human Female - Face
                    return 6;
                case 17234: // Human Female - Face
                    return 7;
                case 17235: // Human Female - Face
                    return 8;
                case 17236: // Human Female - Face
                    return 9;
                case 17237: // Human Female - Face
                    return 10;
                case 17238: // Human Female - Face
                    return 11;
                case 17239: // Human Female - Face
                    return 12;
                case 17240: // Human Female - Face
                    return 13;
                case 17241: // Human Female - Face
                    return 14;
                case 17242: // Human Female - Hair Style
                    return 0;
                case 17243: // Human Female - Hair Style
                    return 1;
                case 17244: // Human Female - Hair Style
                    return 2;
                case 17245: // Human Female - Hair Style
                    return 3;
                case 17246: // Human Female - Hair Style
                    return 4;
                case 17247: // Human Female - Hair Style
                    return 5;
                case 17248: // Human Female - Hair Style
                    return 6;
                case 17249: // Human Female - Hair Style
                    return 7;
                case 17250: // Human Female - Hair Style
                    return 8;
                case 17251: // Human Female - Hair Style
                    return 9;
                case 17252: // Human Female - Hair Style
                    return 10;
                case 17253: // Human Female - Hair Style
                    return 11;
                case 17254: // Human Female - Hair Style
                    return 12;
                case 17255: // Human Female - Hair Style
                    return 13;
                case 17256: // Human Female - Hair Style
                    return 14;
                case 17257: // Human Female - Hair Style
                    return 15;
                case 17258: // Human Female - Hair Style
                    return 16;
                case 17259: // Human Female - Hair Style
                    return 17;
                case 17260: // Human Female - Hair Style
                    return 18;
                case 17261: // Human Female - Hair Color
                    return 0;
                case 17262: // Human Female - Hair Color
                    return 1;
                case 17263: // Human Female - Hair Color
                    return 2;
                case 17264: // Human Female - Hair Color
                    return 3;
                case 17265: // Human Female - Hair Color
                    return 4;
                case 17266: // Human Female - Hair Color
                    return 5;
                case 17267: // Human Female - Hair Color
                    return 6;
                case 17268: // Human Female - Hair Color
                    return 7;
                case 17269: // Human Female - Hair Color
                    return 8;
                case 17270: // Human Female - Hair Color
                    return 9;
                case 17271: // Human Female - Piercings
                    return 0;
                case 17272: // Human Female - Piercings
                    return 1;
                case 17273: // Human Female - Piercings
                    return 2;
                case 17274: // Human Female - Piercings
                    return 3;
                case 17275: // Human Female - Piercings
                    return 4;
                case 17276: // Human Female - Piercings
                    return 5;
                case 17277: // Human Female - Piercings
                    return 6;
                case 17278: // Orc Male - Skin Color
                    return 0;
                case 17279: // Orc Male - Skin Color
                    return 1;
                case 17280: // Orc Male - Skin Color
                    return 2;
                case 17281: // Orc Male - Skin Color
                    return 3;
                case 17282: // Orc Male - Skin Color
                    return 4;
                case 17283: // Orc Male - Skin Color
                    return 5;
                case 17284: // Orc Male - Skin Color
                    return 6;
                case 17285: // Orc Male - Skin Color
                    return 7;
                case 17286: // Orc Male - Skin Color
                    return 8;
                case 17287: // Orc Male - Skin Color
                    return 9;
                case 17288: // Orc Male - Skin Color
                    return 10;
                case 17289: // Orc Male - Skin Color
                    return 11;
                case 17290: // Orc Male - Skin Color
                    return 12;
                case 17291: // Orc Male - Skin Color
                    return 13;
                case 17292: // Orc Male - Skin Color
                    return 14;
                case 17293: // Orc Male - Face
                    return 0;
                case 17294: // Orc Male - Face
                    return 1;
                case 17295: // Orc Male - Face
                    return 2;
                case 17296: // Orc Male - Face
                    return 3;
                case 17297: // Orc Male - Face
                    return 4;
                case 17298: // Orc Male - Face
                    return 5;
                case 17299: // Orc Male - Face
                    return 6;
                case 17300: // Orc Male - Face
                    return 7;
                case 17301: // Orc Male - Face
                    return 8;
                case 17302: // Orc Male - Hair Style
                    return 0;
                case 17303: // Orc Male - Hair Style
                    return 1;
                case 17304: // Orc Male - Hair Style
                    return 2;
                case 17305: // Orc Male - Hair Style
                    return 3;
                case 17306: // Orc Male - Hair Style
                    return 4;
                case 17307: // Orc Male - Hair Style
                    return 5;
                case 17308: // Orc Male - Hair Style
                    return 6;
                case 17309: // Orc Male - Hair Color
                    return 0;
                case 17310: // Orc Male - Hair Color
                    return 1;
                case 17311: // Orc Male - Hair Color
                    return 2;
                case 17312: // Orc Male - Hair Color
                    return 3;
                case 17313: // Orc Male - Hair Color
                    return 4;
                case 17314: // Orc Male - Hair Color
                    return 5;
                case 17315: // Orc Male - Hair Color
                    return 6;
                case 17316: // Orc Male - Hair Color
                    return 7;
                case 17317: // Orc Male - Facial Hair
                    return 0;
                case 17318: // Orc Male - Facial Hair
                    return 1;
                case 17319: // Orc Male - Facial Hair
                    return 2;
                case 17320: // Orc Male - Facial Hair
                    return 3;
                case 17321: // Orc Male - Facial Hair
                    return 4;
                case 17322: // Orc Male - Facial Hair
                    return 5;
                case 17323: // Orc Male - Facial Hair
                    return 6;
                case 17324: // Orc Male - Facial Hair
                    return 7;
                case 17325: // Orc Male - Facial Hair
                    return 8;
                case 17326: // Orc Male - Facial Hair
                    return 9;
                case 17327: // Orc Male - Facial Hair
                    return 10;
                case 17328: // Orc Female - Skin Color
                    return 0;
                case 17329: // Orc Female - Skin Color
                    return 1;
                case 17330: // Orc Female - Skin Color
                    return 2;
                case 17331: // Orc Female - Skin Color
                    return 3;
                case 17332: // Orc Female - Skin Color
                    return 4;
                case 17333: // Orc Female - Skin Color
                    return 5;
                case 17334: // Orc Female - Skin Color
                    return 6;
                case 17335: // Orc Female - Skin Color
                    return 7;
                case 17336: // Orc Female - Skin Color
                    return 8;
                case 17337: // Orc Female - Skin Color
                    return 9;
                case 17338: // Orc Female - Skin Color
                    return 10;
                case 17339: // Orc Female - Face
                    return 0;
                case 17340: // Orc Female - Face
                    return 1;
                case 17341: // Orc Female - Face
                    return 2;
                case 17342: // Orc Female - Face
                    return 3;
                case 17343: // Orc Female - Face
                    return 4;
                case 17344: // Orc Female - Face
                    return 5;
                case 17345: // Orc Female - Face
                    return 6;
                case 17346: // Orc Female - Face
                    return 7;
                case 17347: // Orc Female - Face
                    return 8;
                case 17348: // Orc Female - Hair Style
                    return 0;
                case 17349: // Orc Female - Hair Style
                    return 1;
                case 17350: // Orc Female - Hair Style
                    return 2;
                case 17351: // Orc Female - Hair Style
                    return 3;
                case 17352: // Orc Female - Hair Style
                    return 4;
                case 17353: // Orc Female - Hair Style
                    return 5;
                case 17354: // Orc Female - Hair Style
                    return 6;
                case 17355: // Orc Female - Hair Style
                    return 7;
                case 17356: // Orc Female - Hair Color
                    return 0;
                case 17357: // Orc Female - Hair Color
                    return 1;
                case 17358: // Orc Female - Hair Color
                    return 2;
                case 17359: // Orc Female - Hair Color
                    return 3;
                case 17360: // Orc Female - Hair Color
                    return 4;
                case 17361: // Orc Female - Hair Color
                    return 5;
                case 17362: // Orc Female - Hair Color
                    return 6;
                case 17363: // Orc Female - Hair Color
                    return 7;
                case 17364: // Orc Female - Piercings
                    return 0;
                case 17365: // Orc Female - Piercings
                    return 1;
                case 17366: // Orc Female - Piercings
                    return 2;
                case 17367: // Orc Female - Piercings
                    return 3;
                case 17368: // Orc Female - Piercings
                    return 4;
                case 17369: // Orc Female - Piercings
                    return 5;
                case 17370: // Orc Female - Piercings
                    return 6;
                case 17371: // Dwarf Male - Skin Color
                    return 0;
                case 17372: // Dwarf Male - Skin Color
                    return 1;
                case 17373: // Dwarf Male - Skin Color
                    return 2;
                case 17374: // Dwarf Male - Skin Color
                    return 3;
                case 17375: // Dwarf Male - Skin Color
                    return 4;
                case 17376: // Dwarf Male - Skin Color
                    return 5;
                case 17377: // Dwarf Male - Skin Color
                    return 6;
                case 17378: // Dwarf Male - Skin Color
                    return 7;
                case 17379: // Dwarf Male - Skin Color
                    return 8;
                case 17380: // Dwarf Male - Skin Color
                    return 9;
                case 17381: // Dwarf Male - Skin Color
                    return 10;
                case 17382: // Dwarf Male - Skin Color
                    return 11;
                case 17383: // Dwarf Male - Skin Color
                    return 12;
                case 17384: // Dwarf Male - Skin Color
                    return 13;
                case 17385: // Dwarf Male - Skin Color
                    return 14;
                case 17386: // Dwarf Male - Skin Color
                    return 15;
                case 17387: // Dwarf Male - Skin Color
                    return 16;
                case 17388: // Dwarf Male - Skin Color
                    return 17;
                case 17389: // Dwarf Male - Skin Color
                    return 18;
                case 17390: // Dwarf Male - Face
                    return 0;
                case 17391: // Dwarf Male - Face
                    return 1;
                case 17392: // Dwarf Male - Face
                    return 2;
                case 17393: // Dwarf Male - Face
                    return 3;
                case 17394: // Dwarf Male - Face
                    return 4;
                case 17395: // Dwarf Male - Face
                    return 5;
                case 17396: // Dwarf Male - Face
                    return 6;
                case 17397: // Dwarf Male - Face
                    return 7;
                case 17398: // Dwarf Male - Face
                    return 8;
                case 17399: // Dwarf Male - Face
                    return 9;
                case 17400: // Dwarf Male - Hair Style
                    return 0;
                case 17401: // Dwarf Male - Hair Style
                    return 1;
                case 17402: // Dwarf Male - Hair Style
                    return 2;
                case 17403: // Dwarf Male - Hair Style
                    return 3;
                case 17404: // Dwarf Male - Hair Style
                    return 4;
                case 17405: // Dwarf Male - Hair Style
                    return 5;
                case 17406: // Dwarf Male - Hair Style
                    return 6;
                case 17407: // Dwarf Male - Hair Style
                    return 7;
                case 17408: // Dwarf Male - Hair Style
                    return 8;
                case 17409: // Dwarf Male - Hair Style
                    return 9;
                case 17410: // Dwarf Male - Hair Style
                    return 10;
                case 17411: // Dwarf Male - Hair Color
                    return 0;
                case 17412: // Dwarf Male - Hair Color
                    return 1;
                case 17413: // Dwarf Male - Hair Color
                    return 2;
                case 17414: // Dwarf Male - Hair Color
                    return 3;
                case 17415: // Dwarf Male - Hair Color
                    return 4;
                case 17416: // Dwarf Male - Hair Color
                    return 5;
                case 17417: // Dwarf Male - Hair Color
                    return 6;
                case 17418: // Dwarf Male - Hair Color
                    return 7;
                case 17419: // Dwarf Male - Hair Color
                    return 8;
                case 17420: // Dwarf Male - Hair Color
                    return 9;
                case 17421: // Dwarf Male - Facial Hair
                    return 0;
                case 17422: // Dwarf Male - Facial Hair
                    return 1;
                case 17423: // Dwarf Male - Facial Hair
                    return 2;
                case 17424: // Dwarf Male - Facial Hair
                    return 3;
                case 17425: // Dwarf Male - Facial Hair
                    return 4;
                case 17426: // Dwarf Male - Facial Hair
                    return 5;
                case 17427: // Dwarf Male - Facial Hair
                    return 6;
                case 17428: // Dwarf Male - Facial Hair
                    return 7;
                case 17429: // Dwarf Male - Facial Hair
                    return 8;
                case 17430: // Dwarf Male - Facial Hair
                    return 9;
                case 17431: // Dwarf Male - Facial Hair
                    return 10;
                case 17432: // Dwarf Female - Skin Color
                    return 0;
                case 17433: // Dwarf Female - Skin Color
                    return 1;
                case 17434: // Dwarf Female - Skin Color
                    return 2;
                case 17435: // Dwarf Female - Skin Color
                    return 3;
                case 17436: // Dwarf Female - Skin Color
                    return 4;
                case 17437: // Dwarf Female - Skin Color
                    return 5;
                case 17438: // Dwarf Female - Skin Color
                    return 6;
                case 17439: // Dwarf Female - Skin Color
                    return 7;
                case 17440: // Dwarf Female - Skin Color
                    return 8;
                case 17441: // Dwarf Female - Skin Color
                    return 9;
                case 17442: // Dwarf Female - Skin Color
                    return 10;
                case 17443: // Dwarf Female - Face
                    return 0;
                case 17444: // Dwarf Female - Face
                    return 1;
                case 17445: // Dwarf Female - Face
                    return 2;
                case 17446: // Dwarf Female - Face
                    return 3;
                case 17447: // Dwarf Female - Face
                    return 4;
                case 17448: // Dwarf Female - Face
                    return 5;
                case 17449: // Dwarf Female - Face
                    return 6;
                case 17450: // Dwarf Female - Face
                    return 7;
                case 17451: // Dwarf Female - Face
                    return 8;
                case 17452: // Dwarf Female - Face
                    return 9;
                case 17453: // Dwarf Female - Hair Style
                    return 0;
                case 17454: // Dwarf Female - Hair Style
                    return 1;
                case 17455: // Dwarf Female - Hair Style
                    return 2;
                case 17456: // Dwarf Female - Hair Style
                    return 3;
                case 17457: // Dwarf Female - Hair Style
                    return 4;
                case 17458: // Dwarf Female - Hair Style
                    return 5;
                case 17459: // Dwarf Female - Hair Style
                    return 6;
                case 17460: // Dwarf Female - Hair Style
                    return 7;
                case 17461: // Dwarf Female - Hair Style
                    return 8;
                case 17462: // Dwarf Female - Hair Style
                    return 9;
                case 17463: // Dwarf Female - Hair Style
                    return 10;
                case 17464: // Dwarf Female - Hair Style
                    return 11;
                case 17465: // Dwarf Female - Hair Style
                    return 12;
                case 17466: // Dwarf Female - Hair Style
                    return 13;
                case 17467: // Dwarf Female - Hair Color
                    return 0;
                case 17468: // Dwarf Female - Hair Color
                    return 1;
                case 17469: // Dwarf Female - Hair Color
                    return 2;
                case 17470: // Dwarf Female - Hair Color
                    return 3;
                case 17471: // Dwarf Female - Hair Color
                    return 4;
                case 17472: // Dwarf Female - Hair Color
                    return 5;
                case 17473: // Dwarf Female - Hair Color
                    return 6;
                case 17474: // Dwarf Female - Hair Color
                    return 7;
                case 17475: // Dwarf Female - Hair Color
                    return 8;
                case 17476: // Dwarf Female - Hair Color
                    return 9;
                case 17477: // Dwarf Female - Piercings
                    return 0;
                case 17478: // Dwarf Female - Piercings
                    return 1;
                case 17479: // Dwarf Female - Piercings
                    return 2;
                case 17480: // Dwarf Female - Piercings
                    return 3;
                case 17481: // Dwarf Female - Piercings
                    return 4;
                case 17482: // Dwarf Female - Piercings
                    return 5;
                case 17483: // Night Elf Male - Skin Color
                    return 0;
                case 17484: // Night Elf Male - Skin Color
                    return 1;
                case 17485: // Night Elf Male - Skin Color
                    return 2;
                case 17486: // Night Elf Male - Skin Color
                    return 3;
                case 17487: // Night Elf Male - Skin Color
                    return 4;
                case 17488: // Night Elf Male - Skin Color
                    return 5;
                case 17489: // Night Elf Male - Skin Color
                    return 6;
                case 17490: // Night Elf Male - Skin Color
                    return 7;
                case 17491: // Night Elf Male - Skin Color
                    return 8;
                case 17492: // Night Elf Male - Face
                    return 0;
                case 17493: // Night Elf Male - Face
                    return 1;
                case 17494: // Night Elf Male - Face
                    return 2;
                case 17495: // Night Elf Male - Face
                    return 3;
                case 17496: // Night Elf Male - Face
                    return 4;
                case 17497: // Night Elf Male - Face
                    return 5;
                case 17498: // Night Elf Male - Face
                    return 6;
                case 17499: // Night Elf Male - Face
                    return 7;
                case 17500: // Night Elf Male - Face
                    return 8;
                case 17501: // Night Elf Male - Hair Style
                    return 0;
                case 17502: // Night Elf Male - Hair Style
                    return 1;
                case 17503: // Night Elf Male - Hair Style
                    return 2;
                case 17504: // Night Elf Male - Hair Style
                    return 3;
                case 17505: // Night Elf Male - Hair Style
                    return 4;
                case 17506: // Night Elf Male - Hair Style
                    return 5;
                case 17507: // Night Elf Male - Hair Style
                    return 6;
                case 17508: // Night Elf Male - Hair Color
                    return 0;
                case 17509: // Night Elf Male - Hair Color
                    return 1;
                case 17510: // Night Elf Male - Hair Color
                    return 2;
                case 17511: // Night Elf Male - Hair Color
                    return 3;
                case 17512: // Night Elf Male - Hair Color
                    return 4;
                case 17513: // Night Elf Male - Hair Color
                    return 5;
                case 17514: // Night Elf Male - Hair Color
                    return 6;
                case 17515: // Night Elf Male - Hair Color
                    return 7;
                case 17516: // Night Elf Male - Facial Hair
                    return 0;
                case 17517: // Night Elf Male - Facial Hair
                    return 1;
                case 17518: // Night Elf Male - Facial Hair
                    return 2;
                case 17519: // Night Elf Male - Facial Hair
                    return 3;
                case 17520: // Night Elf Male - Facial Hair
                    return 4;
                case 17521: // Night Elf Male - Facial Hair
                    return 5;
                case 17522: // Night Elf Female - Skin Color
                    return 0;
                case 17523: // Night Elf Female - Skin Color
                    return 1;
                case 17524: // Night Elf Female - Skin Color
                    return 2;
                case 17525: // Night Elf Female - Skin Color
                    return 3;
                case 17526: // Night Elf Female - Skin Color
                    return 4;
                case 17527: // Night Elf Female - Skin Color
                    return 5;
                case 17528: // Night Elf Female - Skin Color
                    return 6;
                case 17529: // Night Elf Female - Skin Color
                    return 7;
                case 17530: // Night Elf Female - Skin Color
                    return 8;
                case 17531: // Night Elf Female - Face
                    return 0;
                case 17532: // Night Elf Female - Face
                    return 1;
                case 17533: // Night Elf Female - Face
                    return 2;
                case 17534: // Night Elf Female - Face
                    return 3;
                case 17535: // Night Elf Female - Face
                    return 4;
                case 17536: // Night Elf Female - Face
                    return 5;
                case 17537: // Night Elf Female - Face
                    return 6;
                case 17538: // Night Elf Female - Face
                    return 7;
                case 17539: // Night Elf Female - Face
                    return 8;
                case 17540: // Night Elf Female - Hair Style
                    return 0;
                case 17541: // Night Elf Female - Hair Style
                    return 1;
                case 17542: // Night Elf Female - Hair Style
                    return 2;
                case 17543: // Night Elf Female - Hair Style
                    return 3;
                case 17544: // Night Elf Female - Hair Style
                    return 4;
                case 17545: // Night Elf Female - Hair Style
                    return 5;
                case 17546: // Night Elf Female - Hair Style
                    return 6;
                case 17547: // Night Elf Female - Hair Color
                    return 0;
                case 17548: // Night Elf Female - Hair Color
                    return 1;
                case 17549: // Night Elf Female - Hair Color
                    return 2;
                case 17550: // Night Elf Female - Hair Color
                    return 3;
                case 17551: // Night Elf Female - Hair Color
                    return 4;
                case 17552: // Night Elf Female - Hair Color
                    return 5;
                case 17553: // Night Elf Female - Hair Color
                    return 6;
                case 17554: // Night Elf Female - Hair Color
                    return 7;
                case 17555: // Night Elf Female - Markings
                    return 0;
                case 17556: // Night Elf Female - Markings
                    return 1;
                case 17557: // Night Elf Female - Markings
                    return 2;
                case 17558: // Night Elf Female - Markings
                    return 3;
                case 17559: // Night Elf Female - Markings
                    return 4;
                case 17560: // Night Elf Female - Markings
                    return 5;
                case 17561: // Night Elf Female - Markings
                    return 6;
                case 17562: // Night Elf Female - Markings
                    return 7;
                case 17563: // Night Elf Female - Markings
                    return 8;
                case 17564: // Night Elf Female - Markings
                    return 9;
                case 17565: // Undead Male - Skin Color
                    return 0;
                case 17566: // Undead Male - Skin Color
                    return 1;
                case 17567: // Undead Male - Skin Color
                    return 2;
                case 17568: // Undead Male - Skin Color
                    return 3;
                case 17569: // Undead Male - Skin Color
                    return 4;
                case 17570: // Undead Male - Skin Color
                    return 5;
                case 17571: // Undead Male - Face
                    return 0;
                case 17572: // Undead Male - Face
                    return 1;
                case 17573: // Undead Male - Face
                    return 2;
                case 17574: // Undead Male - Face
                    return 3;
                case 17575: // Undead Male - Face
                    return 4;
                case 17576: // Undead Male - Face
                    return 5;
                case 17577: // Undead Male - Face
                    return 6;
                case 17578: // Undead Male - Face
                    return 7;
                case 17579: // Undead Male - Face
                    return 8;
                case 17580: // Undead Male - Face
                    return 9;
                case 17581: // Undead Male - Hair Style
                    return 0;
                case 17582: // Undead Male - Hair Style
                    return 1;
                case 17583: // Undead Male - Hair Style
                    return 2;
                case 17584: // Undead Male - Hair Style
                    return 3;
                case 17585: // Undead Male - Hair Style
                    return 4;
                case 17586: // Undead Male - Hair Style
                    return 5;
                case 17587: // Undead Male - Hair Style
                    return 6;
                case 17588: // Undead Male - Hair Style
                    return 7;
                case 17589: // Undead Male - Hair Style
                    return 8;
                case 17590: // Undead Male - Hair Style
                    return 9;
                case 17591: // Undead Male - Hair Color
                    return 0;
                case 17592: // Undead Male - Hair Color
                    return 1;
                case 17593: // Undead Male - Hair Color
                    return 2;
                case 17594: // Undead Male - Hair Color
                    return 3;
                case 17595: // Undead Male - Hair Color
                    return 4;
                case 17596: // Undead Male - Hair Color
                    return 5;
                case 17597: // Undead Male - Hair Color
                    return 6;
                case 17598: // Undead Male - Hair Color
                    return 7;
                case 17599: // Undead Male - Hair Color
                    return 8;
                case 17600: // Undead Male - Hair Color
                    return 9;
                case 17601: // Undead Male - Features
                    return 0;
                case 17602: // Undead Male - Features
                    return 1;
                case 17603: // Undead Male - Features
                    return 2;
                case 17604: // Undead Male - Features
                    return 3;
                case 17605: // Undead Male - Features
                    return 4;
                case 17606: // Undead Male - Features
                    return 5;
                case 17607: // Undead Male - Features
                    return 6;
                case 17608: // Undead Male - Features
                    return 7;
                case 17609: // Undead Male - Features
                    return 8;
                case 17610: // Undead Male - Features
                    return 9;
                case 17611: // Undead Male - Features
                    return 10;
                case 17612: // Undead Male - Features
                    return 11;
                case 17613: // Undead Male - Features
                    return 12;
                case 17614: // Undead Male - Features
                    return 13;
                case 17615: // Undead Male - Features
                    return 14;
                case 17616: // Undead Male - Features
                    return 15;
                case 17617: // Undead Male - Features
                    return 16;
                case 17618: // Undead Female - Skin Color
                    return 0;
                case 17619: // Undead Female - Skin Color
                    return 1;
                case 17620: // Undead Female - Skin Color
                    return 2;
                case 17621: // Undead Female - Skin Color
                    return 3;
                case 17622: // Undead Female - Skin Color
                    return 4;
                case 17623: // Undead Female - Skin Color
                    return 5;
                case 17624: // Undead Female - Face
                    return 0;
                case 17625: // Undead Female - Face
                    return 1;
                case 17626: // Undead Female - Face
                    return 2;
                case 17627: // Undead Female - Face
                    return 3;
                case 17628: // Undead Female - Face
                    return 4;
                case 17629: // Undead Female - Face
                    return 5;
                case 17630: // Undead Female - Face
                    return 6;
                case 17631: // Undead Female - Face
                    return 7;
                case 17632: // Undead Female - Face
                    return 8;
                case 17633: // Undead Female - Face
                    return 9;
                case 17634: // Undead Female - Hair Style
                    return 0;
                case 17635: // Undead Female - Hair Style
                    return 1;
                case 17636: // Undead Female - Hair Style
                    return 2;
                case 17637: // Undead Female - Hair Style
                    return 3;
                case 17638: // Undead Female - Hair Style
                    return 4;
                case 17639: // Undead Female - Hair Style
                    return 5;
                case 17640: // Undead Female - Hair Style
                    return 6;
                case 17641: // Undead Female - Hair Style
                    return 7;
                case 17642: // Undead Female - Hair Style
                    return 8;
                case 17643: // Undead Female - Hair Style
                    return 9;
                case 17644: // Undead Female - Hair Color
                    return 0;
                case 17645: // Undead Female - Hair Color
                    return 1;
                case 17646: // Undead Female - Hair Color
                    return 2;
                case 17647: // Undead Female - Hair Color
                    return 3;
                case 17648: // Undead Female - Hair Color
                    return 4;
                case 17649: // Undead Female - Hair Color
                    return 5;
                case 17650: // Undead Female - Hair Color
                    return 6;
                case 17651: // Undead Female - Hair Color
                    return 7;
                case 17652: // Undead Female - Hair Color
                    return 8;
                case 17653: // Undead Female - Hair Color
                    return 9;
                case 17654: // Undead Female - Features
                    return 0;
                case 17655: // Undead Female - Features
                    return 1;
                case 17656: // Undead Female - Features
                    return 2;
                case 17657: // Undead Female - Features
                    return 3;
                case 17658: // Undead Female - Features
                    return 4;
                case 17659: // Undead Female - Features
                    return 5;
                case 17660: // Undead Female - Features
                    return 6;
                case 17661: // Undead Female - Features
                    return 7;
                case 17662: // Tauren Male - Skin Color
                    return 0;
                case 17663: // Tauren Male - Skin Color
                    return 1;
                case 17664: // Tauren Male - Skin Color
                    return 2;
                case 17665: // Tauren Male - Skin Color
                    return 3;
                case 17666: // Tauren Male - Skin Color
                    return 4;
                case 17667: // Tauren Male - Skin Color
                    return 5;
                case 17668: // Tauren Male - Skin Color
                    return 6;
                case 17669: // Tauren Male - Skin Color
                    return 7;
                case 17670: // Tauren Male - Skin Color
                    return 8;
                case 17671: // Tauren Male - Skin Color
                    return 9;
                case 17672: // Tauren Male - Skin Color
                    return 10;
                case 17673: // Tauren Male - Skin Color
                    return 11;
                case 17674: // Tauren Male - Skin Color
                    return 12;
                case 17675: // Tauren Male - Skin Color
                    return 13;
                case 17676: // Tauren Male - Skin Color
                    return 14;
                case 17677: // Tauren Male - Skin Color
                    return 15;
                case 17678: // Tauren Male - Skin Color
                    return 16;
                case 17679: // Tauren Male - Skin Color
                    return 17;
                case 17680: // Tauren Male - Skin Color
                    return 18;
                case 17681: // 378 - Face
                    return 0;
                case 17682: // 378 - Face
                    return 1;
                case 17683: // 378 - Face
                    return 2;
                case 17684: // 378 - Face
                    return 3;
                case 17685: // 378 - Face
                    return 4;
                case 17686: // Tauren Male - Horn Style
                    return 0;
                case 17687: // Tauren Male - Horn Style
                    return 1;
                case 17688: // Tauren Male - Horn Style
                    return 2;
                case 17689: // Tauren Male - Horn Style
                    return 3;
                case 17690: // Tauren Male - Horn Style
                    return 4;
                case 17691: // Tauren Male - Horn Style
                    return 5;
                case 17692: // Tauren Male - Horn Style
                    return 6;
                case 17693: // Tauren Male - Horn Style
                    return 7;
                case 17694: // Tauren Male - Horn Color
                    return 0;
                case 17695: // Tauren Male - Horn Color
                    return 1;
                case 17696: // Tauren Male - Horn Color
                    return 2;
                case 17697: // Tauren Male - Facial Hair
                    return 0;
                case 17698: // Tauren Male - Facial Hair
                    return 1;
                case 17699: // Tauren Male - Facial Hair
                    return 2;
                case 17700: // Tauren Male - Facial Hair
                    return 3;
                case 17701: // Tauren Male - Facial Hair
                    return 4;
                case 17702: // Tauren Male - Facial Hair
                    return 5;
                case 17703: // Tauren Male - Facial Hair
                    return 6;
                case 17704: // Tauren Female - Skin Color
                    return 0;
                case 17705: // Tauren Female - Skin Color
                    return 1;
                case 17706: // Tauren Female - Skin Color
                    return 2;
                case 17707: // Tauren Female - Skin Color
                    return 3;
                case 17708: // Tauren Female - Skin Color
                    return 4;
                case 17709: // Tauren Female - Skin Color
                    return 5;
                case 17710: // Tauren Female - Skin Color
                    return 6;
                case 17711: // Tauren Female - Skin Color
                    return 7;
                case 17712: // Tauren Female - Skin Color
                    return 8;
                case 17713: // Tauren Female - Skin Color
                    return 9;
                case 17714: // Tauren Female - Skin Color
                    return 10;
                case 17715: // 379 - Face
                    return 0;
                case 17716: // 379 - Face
                    return 1;
                case 17717: // 379 - Face
                    return 2;
                case 17718: // 379 - Face
                    return 3;
                case 17719: // Tauren Female - Horn Style
                    return 0;
                case 17720: // Tauren Female - Horn Style
                    return 1;
                case 17721: // Tauren Female - Horn Style
                    return 2;
                case 17722: // Tauren Female - Horn Style
                    return 3;
                case 17723: // Tauren Female - Horn Style
                    return 4;
                case 17724: // Tauren Female - Horn Style
                    return 5;
                case 17725: // Tauren Female - Horn Style
                    return 6;
                case 17726: // Tauren Female - Horn Color
                    return 0;
                case 17727: // Tauren Female - Horn Color
                    return 1;
                case 17728: // Tauren Female - Horn Color
                    return 2;
                case 17729: // Tauren Female - Hair
                    return 0;
                case 17730: // Tauren Female - Hair
                    return 1;
                case 17731: // Tauren Female - Hair
                    return 2;
                case 17732: // Tauren Female - Hair
                    return 3;
                case 17733: // Tauren Female - Hair
                    return 4;
                case 17734: // Gnome Male - Skin Color
                    return 0;
                case 17735: // Gnome Male - Skin Color
                    return 1;
                case 17736: // Gnome Male - Skin Color
                    return 2;
                case 17737: // Gnome Male - Skin Color
                    return 3;
                case 17738: // Gnome Male - Skin Color
                    return 4;
                case 17739: // Gnome Male - Skin Color
                    return 5;
                case 17740: // Gnome Male - Skin Color
                    return 6;
                case 17741: // Gnome Male - Face
                    return 0;
                case 17742: // Gnome Male - Face
                    return 1;
                case 17743: // Gnome Male - Face
                    return 2;
                case 17744: // Gnome Male - Face
                    return 3;
                case 17745: // Gnome Male - Face
                    return 4;
                case 17746: // Gnome Male - Face
                    return 5;
                case 17747: // Gnome Male - Face
                    return 6;
                case 17748: // Gnome Male - Hair Style
                    return 0;
                case 17749: // Gnome Male - Hair Style
                    return 1;
                case 17750: // Gnome Male - Hair Style
                    return 2;
                case 17751: // Gnome Male - Hair Style
                    return 3;
                case 17752: // Gnome Male - Hair Style
                    return 4;
                case 17753: // Gnome Male - Hair Style
                    return 5;
                case 17754: // Gnome Male - Hair Style
                    return 6;
                case 17755: // Gnome Male - Hair Color
                    return 0;
                case 17756: // Gnome Male - Hair Color
                    return 1;
                case 17757: // Gnome Male - Hair Color
                    return 2;
                case 17758: // Gnome Male - Hair Color
                    return 3;
                case 17759: // Gnome Male - Hair Color
                    return 4;
                case 17760: // Gnome Male - Hair Color
                    return 5;
                case 17761: // Gnome Male - Hair Color
                    return 6;
                case 17762: // Gnome Male - Hair Color
                    return 7;
                case 17763: // Gnome Male - Hair Color
                    return 8;
                case 17764: // Gnome Male - Facial Hair
                    return 0;
                case 17765: // Gnome Male - Facial Hair
                    return 1;
                case 17766: // Gnome Male - Facial Hair
                    return 2;
                case 17767: // Gnome Male - Facial Hair
                    return 3;
                case 17768: // Gnome Male - Facial Hair
                    return 4;
                case 17769: // Gnome Male - Facial Hair
                    return 5;
                case 17770: // Gnome Male - Facial Hair
                    return 6;
                case 17771: // Gnome Male - Facial Hair
                    return 7;
                case 17772: // Gnome Female - Skin Color
                    return 0;
                case 17773: // Gnome Female - Skin Color
                    return 1;
                case 17774: // Gnome Female - Skin Color
                    return 2;
                case 17775: // Gnome Female - Skin Color
                    return 3;
                case 17776: // Gnome Female - Skin Color
                    return 4;
                case 17777: // Gnome Female - Skin Color
                    return 5;
                case 17778: // Gnome Female - Skin Color
                    return 6;
                case 17779: // Gnome Female - Face
                    return 0;
                case 17780: // Gnome Female - Face
                    return 1;
                case 17781: // Gnome Female - Face
                    return 2;
                case 17782: // Gnome Female - Face
                    return 3;
                case 17783: // Gnome Female - Face
                    return 4;
                case 17784: // Gnome Female - Face
                    return 5;
                case 17785: // Gnome Female - Face
                    return 6;
                case 17786: // Gnome Female - Hair Style
                    return 0;
                case 17787: // Gnome Female - Hair Style
                    return 1;
                case 17788: // Gnome Female - Hair Style
                    return 2;
                case 17789: // Gnome Female - Hair Style
                    return 3;
                case 17790: // Gnome Female - Hair Style
                    return 4;
                case 17791: // Gnome Female - Hair Style
                    return 5;
                case 17792: // Gnome Female - Hair Style
                    return 6;
                case 17793: // Gnome Female - Hair Color
                    return 0;
                case 17794: // Gnome Female - Hair Color
                    return 1;
                case 17795: // Gnome Female - Hair Color
                    return 2;
                case 17796: // Gnome Female - Hair Color
                    return 3;
                case 17797: // Gnome Female - Hair Color
                    return 4;
                case 17798: // Gnome Female - Hair Color
                    return 5;
                case 17799: // Gnome Female - Hair Color
                    return 6;
                case 17800: // Gnome Female - Hair Color
                    return 7;
                case 17801: // Gnome Female - Hair Color
                    return 8;
                case 17802: // Gnome Female - Earrings
                    return 0;
                case 17803: // Gnome Female - Earrings
                    return 1;
                case 17804: // Gnome Female - Earrings
                    return 2;
                case 17805: // Gnome Female - Earrings
                    return 3;
                case 17806: // Gnome Female - Earrings
                    return 4;
                case 17807: // Gnome Female - Earrings
                    return 5;
                case 17808: // Gnome Female - Earrings
                    return 6;
                case 17809: // Troll Male - Skin Color
                    return 0;
                case 17810: // Troll Male - Skin Color
                    return 1;
                case 17811: // Troll Male - Skin Color
                    return 2;
                case 17812: // Troll Male - Skin Color
                    return 3;
                case 17813: // Troll Male - Skin Color
                    return 4;
                case 17814: // Troll Male - Skin Color
                    return 5;
                case 17815: // Troll Male - Skin Color
                    return 6;
                case 17816: // Troll Male - Skin Color
                    return 7;
                case 17817: // Troll Male - Skin Color
                    return 8;
                case 17818: // Troll Male - Skin Color
                    return 9;
                case 17819: // Troll Male - Skin Color
                    return 10;
                case 17820: // Troll Male - Skin Color
                    return 11;
                case 17821: // Troll Male - Skin Color
                    return 12;
                case 17822: // Troll Male - Skin Color
                    return 13;
                case 17823: // Troll Male - Skin Color
                    return 14;
                case 17824: // Troll Male - Face
                    return 0;
                case 17825: // Troll Male - Face
                    return 1;
                case 17826: // Troll Male - Face
                    return 2;
                case 17827: // Troll Male - Face
                    return 3;
                case 17828: // Troll Male - Face
                    return 4;
                case 17829: // Troll Male - Hair Style
                    return 0;
                case 17830: // Troll Male - Hair Style
                    return 1;
                case 17831: // Troll Male - Hair Style
                    return 2;
                case 17832: // Troll Male - Hair Style
                    return 3;
                case 17833: // Troll Male - Hair Style
                    return 4;
                case 17834: // Troll Male - Hair Style
                    return 5;
                case 17835: // Troll Male - Hair Color
                    return 0;
                case 17836: // Troll Male - Hair Color
                    return 1;
                case 17837: // Troll Male - Hair Color
                    return 2;
                case 17838: // Troll Male - Hair Color
                    return 3;
                case 17839: // Troll Male - Hair Color
                    return 4;
                case 17840: // Troll Male - Hair Color
                    return 5;
                case 17841: // Troll Male - Hair Color
                    return 6;
                case 17842: // Troll Male - Hair Color
                    return 7;
                case 17843: // Troll Male - Hair Color
                    return 8;
                case 17844: // Troll Male - Hair Color
                    return 9;
                case 17845: // Troll Male - Tusks
                    return 0;
                case 17846: // Troll Male - Tusks
                    return 1;
                case 17847: // Troll Male - Tusks
                    return 2;
                case 17848: // Troll Male - Tusks
                    return 3;
                case 17849: // Troll Male - Tusks
                    return 4;
                case 17850: // Troll Male - Tusks
                    return 5;
                case 17851: // Troll Male - Tusks
                    return 6;
                case 17852: // Troll Male - Tusks
                    return 7;
                case 17853: // Troll Male - Tusks
                    return 8;
                case 17854: // Troll Male - Tusks
                    return 9;
                case 17855: // Troll Male - Tusks
                    return 10;
                case 17856: // Troll Female - Skin Color
                    return 0;
                case 17857: // Troll Female - Skin Color
                    return 1;
                case 17858: // Troll Female - Skin Color
                    return 2;
                case 17859: // Troll Female - Skin Color
                    return 3;
                case 17860: // Troll Female - Skin Color
                    return 4;
                case 17861: // Troll Female - Skin Color
                    return 5;
                case 17862: // Troll Female - Skin Color
                    return 6;
                case 17863: // Troll Female - Skin Color
                    return 7;
                case 17864: // Troll Female - Skin Color
                    return 8;
                case 17865: // Troll Female - Skin Color
                    return 9;
                case 17866: // Troll Female - Skin Color
                    return 10;
                case 17867: // Troll Female - Skin Color
                    return 11;
                case 17868: // Troll Female - Skin Color
                    return 12;
                case 17869: // Troll Female - Skin Color
                    return 13;
                case 17870: // Troll Female - Skin Color
                    return 14;
                case 17871: // Troll Female - Face
                    return 0;
                case 17872: // Troll Female - Face
                    return 1;
                case 17873: // Troll Female - Face
                    return 2;
                case 17874: // Troll Female - Face
                    return 3;
                case 17875: // Troll Female - Face
                    return 4;
                case 17876: // Troll Female - Face
                    return 5;
                case 17877: // Troll Female - Hair Style
                    return 0;
                case 17878: // Troll Female - Hair Style
                    return 1;
                case 17879: // Troll Female - Hair Style
                    return 2;
                case 17880: // Troll Female - Hair Style
                    return 3;
                case 17881: // Troll Female - Hair Style
                    return 4;
                case 17882: // Troll Female - Hair Color
                    return 0;
                case 17883: // Troll Female - Hair Color
                    return 1;
                case 17884: // Troll Female - Hair Color
                    return 2;
                case 17885: // Troll Female - Hair Color
                    return 3;
                case 17886: // Troll Female - Hair Color
                    return 4;
                case 17887: // Troll Female - Hair Color
                    return 5;
                case 17888: // Troll Female - Hair Color
                    return 6;
                case 17889: // Troll Female - Hair Color
                    return 7;
                case 17890: // Troll Female - Hair Color
                    return 8;
                case 17891: // Troll Female - Hair Color
                    return 9;
                case 17892: // Troll Female - Tusks
                    return 0;
                case 17893: // Troll Female - Tusks
                    return 1;
                case 17894: // Troll Female - Tusks
                    return 2;
                case 17895: // Troll Female - Tusks
                    return 3;
                case 17896: // Troll Female - Tusks
                    return 4;
                case 17897: // Troll Female - Tusks
                    return 5;
                case 17898: // Goblin Male - Skin Color
                    return 0;
                case 17899: // Goblin Male - Skin Color
                    return 1;
                case 17900: // Goblin Male - Skin Color
                    return 2;
                case 17901: // Goblin Male - Hair Style
                    return 0;
                case 17902: // Goblin Male - Hair Style
                    return 1;
                case 17903: // Goblin Female - Skin Color
                    return 0;
                case 17904: // Goblin Female - Skin Color
                    return 1;
                case 17905: // Goblin Female - Skin Color
                    return 2;
                case 17906: // Blood Elf Male - Skin Color
                    return 0;
                case 17907: // Blood Elf Male - Skin Color
                    return 1;
                case 17908: // Blood Elf Male - Skin Color
                    return 2;
                case 17909: // Blood Elf Male - Skin Color
                    return 3;
                case 17910: // Blood Elf Male - Skin Color
                    return 4;
                case 17911: // Blood Elf Male - Skin Color
                    return 5;
                case 17912: // Blood Elf Male - Skin Color
                    return 6;
                case 17913: // Blood Elf Male - Skin Color
                    return 7;
                case 17914: // Blood Elf Male - Skin Color
                    return 8;
                case 17915: // Blood Elf Male - Skin Color
                    return 9;
                case 17916: // Blood Elf Male - Skin Color
                    return 10;
                case 17917: // Blood Elf Male - Skin Color
                    return 11;
                case 17918: // Blood Elf Male - Skin Color
                    return 12;
                case 17919: // Blood Elf Male - Skin Color
                    return 13;
                case 17920: // Blood Elf Male - Skin Color
                    return 14;
                case 17921: // Blood Elf Male - Skin Color
                    return 15;
                case 17922: // Blood Elf Male - Face
                    return 0;
                case 17923: // Blood Elf Male - Face
                    return 1;
                case 17924: // Blood Elf Male - Face
                    return 2;
                case 17925: // Blood Elf Male - Face
                    return 3;
                case 17926: // Blood Elf Male - Face
                    return 4;
                case 17927: // Blood Elf Male - Face
                    return 5;
                case 17928: // Blood Elf Male - Face
                    return 6;
                case 17929: // Blood Elf Male - Face
                    return 7;
                case 17930: // Blood Elf Male - Face
                    return 8;
                case 17931: // Blood Elf Male - Face
                    return 9;
                case 17932: // Blood Elf Male - Hair Style
                    return 0;
                case 17933: // Blood Elf Male - Hair Style
                    return 1;
                case 17934: // Blood Elf Male - Hair Style
                    return 2;
                case 17935: // Blood Elf Male - Hair Style
                    return 3;
                case 17936: // Blood Elf Male - Hair Style
                    return 4;
                case 17937: // Blood Elf Male - Hair Style
                    return 5;
                case 17938: // Blood Elf Male - Hair Style
                    return 6;
                case 17939: // Blood Elf Male - Hair Style
                    return 7;
                case 17940: // Blood Elf Male - Hair Style
                    return 8;
                case 17941: // Blood Elf Male - Hair Style
                    return 9;
                case 17942: // Blood Elf Male - Hair Style
                    return 10;
                case 17943: // Blood Elf Male - Hair Color
                    return 0;
                case 17944: // Blood Elf Male - Hair Color
                    return 1;
                case 17945: // Blood Elf Male - Hair Color
                    return 2;
                case 17946: // Blood Elf Male - Hair Color
                    return 3;
                case 17947: // Blood Elf Male - Hair Color
                    return 4;
                case 17948: // Blood Elf Male - Hair Color
                    return 5;
                case 17949: // Blood Elf Male - Hair Color
                    return 6;
                case 17950: // Blood Elf Male - Hair Color
                    return 7;
                case 17951: // Blood Elf Male - Hair Color
                    return 8;
                case 17952: // Blood Elf Male - Hair Color
                    return 9;
                case 17953: // Blood Elf Male - Facial Hair
                    return 0;
                case 17954: // Blood Elf Male - Facial Hair
                    return 1;
                case 17955: // Blood Elf Male - Facial Hair
                    return 2;
                case 17956: // Blood Elf Male - Facial Hair
                    return 3;
                case 17957: // Blood Elf Male - Facial Hair
                    return 4;
                case 17958: // Blood Elf Male - Facial Hair
                    return 5;
                case 17959: // Blood Elf Male - Facial Hair
                    return 6;
                case 17960: // Blood Elf Male - Facial Hair
                    return 7;
                case 17961: // Blood Elf Male - Facial Hair
                    return 8;
                case 17962: // Blood Elf Male - Facial Hair
                    return 9;
                case 17963: // Blood Elf Female - Skin Color
                    return 0;
                case 17964: // Blood Elf Female - Skin Color
                    return 1;
                case 17965: // Blood Elf Female - Skin Color
                    return 2;
                case 17966: // Blood Elf Female - Skin Color
                    return 3;
                case 17967: // Blood Elf Female - Skin Color
                    return 4;
                case 17968: // Blood Elf Female - Skin Color
                    return 5;
                case 17969: // Blood Elf Female - Skin Color
                    return 6;
                case 17970: // Blood Elf Female - Skin Color
                    return 7;
                case 17971: // Blood Elf Female - Skin Color
                    return 8;
                case 17972: // Blood Elf Female - Skin Color
                    return 9;
                case 17973: // Blood Elf Female - Skin Color
                    return 10;
                case 17974: // Blood Elf Female - Skin Color
                    return 11;
                case 17975: // Blood Elf Female - Skin Color
                    return 12;
                case 17976: // Blood Elf Female - Skin Color
                    return 13;
                case 17977: // Blood Elf Female - Skin Color
                    return 14;
                case 17978: // Blood Elf Female - Skin Color
                    return 15;
                case 17979: // Blood Elf Female - Face
                    return 0;
                case 17980: // Blood Elf Female - Face
                    return 1;
                case 17981: // Blood Elf Female - Face
                    return 2;
                case 17982: // Blood Elf Female - Face
                    return 3;
                case 17983: // Blood Elf Female - Face
                    return 4;
                case 17984: // Blood Elf Female - Face
                    return 5;
                case 17985: // Blood Elf Female - Face
                    return 6;
                case 17986: // Blood Elf Female - Face
                    return 7;
                case 17987: // Blood Elf Female - Face
                    return 8;
                case 17988: // Blood Elf Female - Face
                    return 9;
                case 17989: // Blood Elf Female - Hair Style
                    return 0;
                case 17990: // Blood Elf Female - Hair Style
                    return 1;
                case 17991: // Blood Elf Female - Hair Style
                    return 2;
                case 17992: // Blood Elf Female - Hair Style
                    return 3;
                case 17993: // Blood Elf Female - Hair Style
                    return 4;
                case 17994: // Blood Elf Female - Hair Style
                    return 5;
                case 17995: // Blood Elf Female - Hair Style
                    return 6;
                case 17996: // Blood Elf Female - Hair Style
                    return 7;
                case 17997: // Blood Elf Female - Hair Style
                    return 8;
                case 17998: // Blood Elf Female - Hair Style
                    return 9;
                case 17999: // Blood Elf Female - Hair Style
                    return 10;
                case 18000: // Blood Elf Female - Hair Style
                    return 11;
                case 18001: // Blood Elf Female - Hair Style
                    return 12;
                case 18002: // Blood Elf Female - Hair Style
                    return 13;
                case 18004: // Blood Elf Female - Hair Color
                    return 0;
                case 18005: // Blood Elf Female - Hair Color
                    return 1;
                case 18006: // Blood Elf Female - Hair Color
                    return 2;
                case 18007: // Blood Elf Female - Hair Color
                    return 3;
                case 18008: // Blood Elf Female - Hair Color
                    return 4;
                case 18009: // Blood Elf Female - Hair Color
                    return 5;
                case 18010: // Blood Elf Female - Hair Color
                    return 6;
                case 18011: // Blood Elf Female - Hair Color
                    return 7;
                case 18012: // Blood Elf Female - Hair Color
                    return 8;
                case 18013: // Blood Elf Female - Hair Color
                    return 9;
                case 18014: // Blood Elf Female - Earrings
                    return 0;
                case 18015: // Blood Elf Female - Earrings
                    return 1;
                case 18016: // Blood Elf Female - Earrings
                    return 2;
                case 18017: // Blood Elf Female - Earrings
                    return 3;
                case 18018: // Blood Elf Female - Earrings
                    return 4;
                case 18019: // Blood Elf Female - Earrings
                    return 5;
                case 18020: // Blood Elf Female - Earrings
                    return 6;
                case 18021: // Blood Elf Female - Earrings
                    return 7;
                case 18022: // Blood Elf Female - Earrings
                    return 8;
                case 18023: // Blood Elf Female - Earrings
                    return 9;
                case 18024: // Blood Elf Female - Earrings
                    return 10;
                case 18025: // Draenei Male - Skin Color
                    return 0;
                case 18026: // Draenei Male - Skin Color
                    return 1;
                case 18027: // Draenei Male - Skin Color
                    return 2;
                case 18028: // Draenei Male - Skin Color
                    return 3;
                case 18029: // Draenei Male - Skin Color
                    return 4;
                case 18030: // Draenei Male - Skin Color
                    return 5;
                case 18031: // Draenei Male - Skin Color
                    return 6;
                case 18032: // Draenei Male - Skin Color
                    return 7;
                case 18033: // Draenei Male - Skin Color
                    return 8;
                case 18034: // Draenei Male - Skin Color
                    return 9;
                case 18035: // Draenei Male - Skin Color
                    return 10;
                case 18036: // Draenei Male - Skin Color
                    return 11;
                case 18037: // Draenei Male - Skin Color
                    return 12;
                case 18038: // Draenei Male - Skin Color
                    return 13;
                case 18039: // Draenei Male - Face
                    return 0;
                case 18040: // Draenei Male - Face
                    return 1;
                case 18041: // Draenei Male - Face
                    return 2;
                case 18042: // Draenei Male - Face
                    return 3;
                case 18043: // Draenei Male - Face
                    return 4;
                case 18044: // Draenei Male - Face
                    return 5;
                case 18045: // Draenei Male - Face
                    return 6;
                case 18046: // Draenei Male - Face
                    return 7;
                case 18047: // Draenei Male - Face
                    return 8;
                case 18048: // Draenei Male - Face
                    return 9;
                case 18049: // Draenei Male - Hair Style
                    return 0;
                case 18050: // Draenei Male - Hair Style
                    return 1;
                case 18051: // Draenei Male - Hair Style
                    return 2;
                case 18052: // Draenei Male - Hair Style
                    return 3;
                case 18053: // Draenei Male - Hair Style
                    return 4;
                case 18054: // Draenei Male - Hair Style
                    return 5;
                case 18055: // Draenei Male - Hair Style
                    return 6;
                case 18056: // Draenei Male - Hair Style
                    return 7;
                case 18057: // Draenei Male - Hair Style
                    return 8;
                case 18058: // Draenei Male - Hair Color
                    return 0;
                case 18059: // Draenei Male - Hair Color
                    return 1;
                case 18060: // Draenei Male - Hair Color
                    return 2;
                case 18061: // Draenei Male - Hair Color
                    return 3;
                case 18062: // Draenei Male - Hair Color
                    return 4;
                case 18063: // Draenei Male - Hair Color
                    return 5;
                case 18064: // Draenei Male - Hair Color
                    return 6;
                case 18065: // Draenei Male - Facial Hair
                    return 0;
                case 18066: // Draenei Male - Facial Hair
                    return 1;
                case 18067: // Draenei Male - Facial Hair
                    return 2;
                case 18068: // Draenei Male - Facial Hair
                    return 3;
                case 18069: // Draenei Male - Facial Hair
                    return 4;
                case 18070: // Draenei Male - Facial Hair
                    return 5;
                case 18071: // Draenei Male - Facial Hair
                    return 6;
                case 18072: // Draenei Male - Facial Hair
                    return 7;
                case 18073: // Draenei Female - Skin Color
                    return 0;
                case 18074: // Draenei Female - Skin Color
                    return 1;
                case 18075: // Draenei Female - Skin Color
                    return 2;
                case 18076: // Draenei Female - Skin Color
                    return 3;
                case 18077: // Draenei Female - Skin Color
                    return 4;
                case 18078: // Draenei Female - Skin Color
                    return 5;
                case 18079: // Draenei Female - Skin Color
                    return 6;
                case 18080: // Draenei Female - Skin Color
                    return 7;
                case 18081: // Draenei Female - Skin Color
                    return 8;
                case 18082: // Draenei Female - Skin Color
                    return 9;
                case 18083: // Draenei Female - Skin Color
                    return 10;
                case 18084: // Draenei Female - Skin Color
                    return 11;
                case 18085: // Draenei Female - Face
                    return 0;
                case 18086: // Draenei Female - Face
                    return 1;
                case 18087: // Draenei Female - Face
                    return 2;
                case 18088: // Draenei Female - Face
                    return 3;
                case 18089: // Draenei Female - Face
                    return 4;
                case 18090: // Draenei Female - Face
                    return 5;
                case 18091: // Draenei Female - Face
                    return 6;
                case 18092: // Draenei Female - Face
                    return 7;
                case 18093: // Draenei Female - Face
                    return 8;
                case 18094: // Draenei Female - Face
                    return 9;
                case 18095: // Draenei Female - Hair Style
                    return 0;
                case 18096: // Draenei Female - Hair Style
                    return 1;
                case 18097: // Draenei Female - Hair Style
                    return 2;
                case 18098: // Draenei Female - Hair Style
                    return 3;
                case 18099: // Draenei Female - Hair Style
                    return 4;
                case 18100: // Draenei Female - Hair Style
                    return 5;
                case 18101: // Draenei Female - Hair Style
                    return 6;
                case 18102: // Draenei Female - Hair Style
                    return 7;
                case 18103: // Draenei Female - Hair Style
                    return 8;
                case 18104: // Draenei Female - Hair Style
                    return 9;
                case 18105: // Draenei Female - Hair Style
                    return 10;
                case 18106: // Draenei Female - Hair Color
                    return 0;
                case 18107: // Draenei Female - Hair Color
                    return 1;
                case 18108: // Draenei Female - Hair Color
                    return 2;
                case 18109: // Draenei Female - Hair Color
                    return 3;
                case 18110: // Draenei Female - Hair Color
                    return 4;
                case 18111: // Draenei Female - Hair Color
                    return 5;
                case 18112: // Draenei Female - Hair Color
                    return 6;
                case 18113: // Draenei Female - Horn Style
                    return 0;
                case 18114: // Draenei Female - Horn Style
                    return 1;
                case 18115: // Draenei Female - Horn Style
                    return 2;
                case 18116: // Draenei Female - Horn Style
                    return 3;
                case 18117: // Draenei Female - Horn Style
                    return 4;
                case 18118: // Draenei Female - Horn Style
                    return 5;
                case 18119: // Draenei Female - Horn Style
                    return 6;
                case 18120: // Fel Orc Male - Skin Color
                    return 0;
                case 18121: // Fel Orc Male - Skin Color
                    return 1;
                case 18122: // Fel Orc Male - Skin Color
                    return 2;
                case 18123: // Fel Orc Male - Face
                    return 0;
                case 18124: // Fel Orc Male - Hair Style
                    return 0;
                case 18125: // Fel Orc Male - Hair Color
                    return 0;
                case 18126: // Fel Orc Male - Facial Hair
                    return 0;
                case 18127: // Fel Orc Female - Hair Style
                    return 0;
                case 18128: // Fel Orc Female - Hair Color
                    return 0;
                case 18129: // Fel Orc Female - Facial Hair
                    return 0;
                case 18130: // Naga Male - Skin Color
                    return 0;
                case 18131: // Naga Male - Skin Color
                    return 1;
                case 18132: // Naga Male - Skin Color
                    return 2;
                case 18133: // Naga Male - Skin Color
                    return 3;
                case 18134: // Naga Male - Skin Color
                    return 4;
                case 18135: // Naga Male - Face
                    return 0;
                case 18136: // Naga Male - Hair Style
                    return 0;
                case 18137: // Naga Male - Hair Color
                    return 0;
                case 18138: // Naga Male - Facial Hair
                    return 0;
                case 18139: // Naga Female - Skin Color
                    return 0;
                case 18140: // Naga Female - Skin Color
                    return 1;
                case 18141: // Naga Female - Skin Color
                    return 2;
                case 18142: // Naga Female - Skin Color
                    return 3;
                case 18143: // Naga Female - Skin Color
                    return 4;
                case 18144: // Naga Female - Face
                    return 0;
                case 18145: // Naga Female - Hair Style
                    return 0;
                case 18146: // Naga Female - Hair Color
                    return 0;
                case 18147: // Naga Female - Facial Hair
                    return 0;
                case 18148: // Broken Male - Skin Color
                    return 0;
                case 18149: // Broken Male - Skin Color
                    return 1;
                case 18150: // Broken Male - Skin Color
                    return 2;
                case 18151: // Broken Male - Skin Color
                    return 3;
                case 18152: // Broken Male - Skin Color
                    return 4;
                case 18153: // Broken Male - Skin Color
                    return 5;
                case 18154: // Broken Male - Face
                    return 0;
                case 18155: // Broken Male - Hair Style
                    return 0;
                case 18156: // Broken Male - Hair Style
                    return 1;
                case 18157: // Broken Male - Hair Style
                    return 2;
                case 18158: // Broken Male - Hair Color
                    return 0;
                case 18159: // Broken Male - Hair Color
                    return 1;
                case 18160: // Broken Male - Hair Color
                    return 2;
                case 18161: // Broken Male - Hair Color
                    return 3;
                case 18162: // Broken Male - Hair Color
                    return 4;
                case 18163: // Broken Male - Hair Color
                    return 5;
                case 18164: // Broken Male - Hair Color
                    return 6;
                case 18165: // Broken Male - Hair Color
                    return 7;
                case 18166: // Broken Male - Hair Color
                    return 8;
                case 18167: // Broken Male - Hair Color
                    return 9;
                case 18168: // Broken Female - Hair Style
                    return 0;
                case 18169: // Broken Female - Hair Color
                    return 0;
                case 18170: // Broken Female - Facial Hair
                    return 0;
                case 18171: // Skeleton Male - Skin Color
                    return 0;
                case 18172: // Skeleton Male - Skin Color
                    return 1;
                case 18173: // Skeleton Male - Skin Color
                    return 2;
                case 18174: // Skeleton Male - Skin Color
                    return 3;
                case 18175: // Skeleton Male - Skin Color
                    return 4;
                case 18176: // Skeleton Male - Skin Color
                    return 5;
                case 18177: // Skeleton Male - Face
                    return 0;
                case 18178: // Skeleton Male - Hair Style
                    return 0;
                case 18179: // Skeleton Male - Hair Color
                    return 0;
                case 18180: // Skeleton Male - Facial Hair
                    return 0;
                case 18181: // Skeleton Female - Hair Style
                    return 0;
                case 18182: // Skeleton Female - Hair Color
                    return 0;
                case 18183: // Skeleton Female - Facial Hair
                    return 0;
                case 18184: // Forest Troll Male - Skin Color
                    return 0;
                case 18185: // Forest Troll Male - Skin Color
                    return 1;
                case 18186: // Forest Troll Male - Skin Color
                    return 2;
                case 18187: // Forest Troll Male - Skin Color
                    return 3;
                case 18188: // Forest Troll Male - Skin Color
                    return 4;
                case 18189: // Forest Troll Male - Skin Color
                    return 5;
                case 18190: // Forest Troll Male - Face
                    return 0;
                case 18191: // Forest Troll Male - Face
                    return 1;
                case 18192: // Forest Troll Male - Face
                    return 2;
                case 18193: // Forest Troll Male - Face
                    return 3;
                case 18194: // Forest Troll Male - Face
                    return 4;
                case 18195: // Forest Troll Male - Hair Style
                    return 0;
                case 18196: // Forest Troll Male - Hair Style
                    return 1;
                case 18197: // Forest Troll Male - Hair Style
                    return 2;
                case 18198: // Forest Troll Male - Hair Style
                    return 3;
                case 18199: // Forest Troll Male - Hair Style
                    return 4;
                case 18200: // Forest Troll Male - Hair Style
                    return 5;
                case 18201: // Forest Troll Male - Hair Color
                    return 0;
                case 18202: // Forest Troll Male - Hair Color
                    return 1;
                case 18203: // Forest Troll Male - Hair Color
                    return 2;
                case 18204: // Forest Troll Male - Hair Color
                    return 3;
                case 18205: // Forest Troll Male - Hair Color
                    return 4;
                case 18206: // Forest Troll Male - Hair Color
                    return 5;
                case 18207: // Forest Troll Male - Hair Color
                    return 6;
                case 18208: // Forest Troll Male - Hair Color
                    return 7;
                case 18209: // Forest Troll Male - Hair Color
                    return 8;
                case 18210: // Forest Troll Male - Hair Color
                    return 9;
                case 18211: // Forest Troll Male - Facial Hair
                    return 0;
                case 18212: // Forest Troll Male - Facial Hair
                    return 1;
                case 18213: // Forest Troll Male - Facial Hair
                    return 2;
                case 18214: // Forest Troll Male - Facial Hair
                    return 3;
                case 18215: // Forest Troll Male - Facial Hair
                    return 4;
                case 18216: // Forest Troll Male - Facial Hair
                    return 5;
                case 18217: // Forest Troll Male - Facial Hair
                    return 6;
                case 18218: // Forest Troll Male - Facial Hair
                    return 7;
                case 18219: // Forest Troll Male - Facial Hair
                    return 8;
                case 18220: // Forest Troll Male - Facial Hair
                    return 9;
                case 18221: // Forest Troll Male - Facial Hair
                    return 10;
                case 18222: // Forest Troll Female - Hair Style
                    return 0;
                case 18223: // Forest Troll Female - Hair Color
                    return 0;
                case 18224: // Forest Troll Female - Facial Hair
                    return 0;
            }
            return 0;
        }

        public static void AssignPlayerAppearanceFields(Store.Objects.UpdateFields.IPlayerData playerData, out byte skin, out byte face, out byte hairStyle, out byte hairColor, out byte facialHair)
        {
            var customizations = playerData.GetCustomizations();
            if (customizations == null || customizations.Length < 1)
            {
                skin = (byte)(playerData.PlayerBytes1 & 0xFF);
                face = (byte)((playerData.PlayerBytes1 >> 8) & 0xFF);
                hairStyle = (byte)((playerData.PlayerBytes1 >> 16) & 0xFF);
                hairColor = (byte)((playerData.PlayerBytes1 >> 24) & 0xFF);
                facialHair = (byte)(playerData.PlayerBytes2 & 0xFF);
                return;
            }

            skin = 0;
            face = 0;
            hairStyle = 0;
            hairColor = 0;
            facialHair = 0;
            foreach (var custom in customizations)
            {
                CharCustomizationOption option = GetCustomizationOption(custom.ChrCustomizationOptionID);
                byte choice = GetCustomizationChoice(custom.ChrCustomizationChoiceID);

                switch (option)
                {
                    case CharCustomizationOption.Skin:
                    {
                        skin = choice;
                        break;
                    }
                    case CharCustomizationOption.Face:
                    {
                        face = choice;
                        break;
                    }
                    case CharCustomizationOption.HairStyle:
                    {
                        hairStyle = choice;
                        break;
                    }
                    case CharCustomizationOption.HairColor:
                    {
                        hairColor = choice;
                        break;
                    }
                    case CharCustomizationOption.FacialHair:
                    {
                        facialHair = choice;
                        break;
                    }
                }
            }
        }

        [BuilderMethod]
        public static string CharactersBuilder()
        {
            if (!Settings.SqlTables.characters && !Settings.SqlTables.player)
                return string.Empty;

            StringBuilder result = new StringBuilder();
            uint maxDbGuid = 0;
            uint itemGuidCounter = 0;
            var characterRows = new RowList<CharacterTemplate>();
            var characterInventoryRows = new RowList<CharacterInventory>();
            var characterItemInstaceRows = new RowList<CharacterItemInstance>();
            var characterReputationRows = new RowList<CharacterReputation>();
            var characterSkillRows = new RowList<CharacterSkill>();
            var characterSpellRows = new RowList<CharacterSpell>();
            var guildMemberRows = new RowList<GuildMember>();
            var playerRows = new RowList<PlayerTemplate>();
            var playerGuidValuesRows = new RowList<CreatureGuidValues>();
            var playerPowerValuesRows = new RowList<CreaturePowerValues>();
            var playerAttackLogRows = new RowList<UnitMeleeAttackLog>();
            var playerAttackStartRows = new RowList<CreatureAttackToggle>();
            var playerAttackStopRows = new RowList<CreatureAttackToggle>();
            var playerAurasUpdateRows = new RowList<CreatureAurasUpdate>();
            var playerCreate1Rows = new RowList<PlayerCreate1>();
            var playerCreate2Rows = new RowList<PlayerCreate2>();
            var playerDestroyRows = new RowList<PlayerDestroy>();
            var playerEmoteRows = new RowList<CreatureEmote>();
            var playerEquipmentValuesUpdateRows = new RowList<CreatureEquipmentValuesUpdate>();
            var playerGuidValuesUpdateRows = new RowList<CreatureGuidValuesUpdate>();
            var playerPowerValuesUpdateRows = new RowList<CreaturePowerValuesUpdate>();
            var playerValuesUpdateRows = new RowList<CreatureValuesUpdate>();
            var playerSpeedUpdateRows = new RowList<CreatureSpeedUpdate>();
            var playerServerMovementRows = new RowList<ServerSideMovement>();
            var playerServerMovementSplineRows = new RowList<ServerSideMovementSpline>();
            var playerMinimapPingRows = new RowList<PlayerMinimapPing>();
            Dictionary<WowGuid, uint> accountIdDictionary = new Dictionary<WowGuid, uint>();
            foreach (var objPair in Storage.Objects)
            {
                if (objPair.Key.GetObjectType() != ObjectType.Player)
                    continue;

                Player player = objPair.Value.Item1 as Player;
                if (player == null)
                    continue;

                if (!player.IsActivePlayer && Settings.SkipOtherPlayers)
                    continue;

                Row<CharacterTemplate> row = new Row<CharacterTemplate>();

                row.Data.Guid = "@PGUID+" + player.DbGuid;
                if (accountIdDictionary.ContainsKey(player.PlayerDataOriginal.WowAccount))
                    row.Data.Account = "@ACCID+" + accountIdDictionary[player.PlayerDataOriginal.WowAccount];
                else
                {
                    uint id = (uint)accountIdDictionary.Count;
                    accountIdDictionary.Add(player.PlayerDataOriginal.WowAccount, id);
                    row.Data.Account = "@ACCID+" + id;
                }

                row.Data.Name = Settings.RandomizePlayerNames ? GetRandomString(8) : StoreGetters.GetName(objPair.Key);
                row.Data.Race = player.UnitDataOriginal.RaceId;
                row.Data.Class = player.UnitDataOriginal.ClassId;
                row.Data.Gender = player.UnitDataOriginal.Sex;
                row.Data.Level = (uint)player.UnitDataOriginal.Level;
                row.Data.XP = (uint)player.ActivePlayerData.XP;
                row.Data.Money = (uint)player.ActivePlayerData.Coinage;
                AssignPlayerAppearanceFields(player.PlayerDataOriginal, out row.Data.Skin, out row.Data.Face, out row.Data.HairStyle, out row.Data.HairColor, out row.Data.FacialHair);
                row.Data.PlayerFlags = player.PlayerDataOriginal.PlayerFlags;

                MovementInfo moveData = player.OriginalMovement == null ? player.Movement : player.OriginalMovement;
                if (moveData != null)
                {
                    row.Data.PositionX = moveData.Position.X;
                    row.Data.PositionY = moveData.Position.Y;
                    row.Data.PositionZ = moveData.Position.Z;
                    row.Data.Orientation = moveData.Orientation;
                }
                row.Data.Map = player.Map;
                row.Data.Health = (uint)player.UnitDataOriginal.MaxHealth;
                row.Data.Power1 = (uint)player.UnitDataOriginal.MaxMana;

                Store.Objects.UpdateFields.IVisibleItem[] visibleItems = player.PlayerDataOriginal.VisibleItems;

                for (int i = 0; i < 19; i++)
                {
                    int itemId = visibleItems[i].ItemID;
                    ushort enchantId = visibleItems[i].ItemVisual;

                    Row<CharacterInventory> inventoryRow = new Row<CharacterInventory>();
                    inventoryRow.Data.Guid = row.Data.Guid;
                    inventoryRow.Data.Bag = 0;
                    inventoryRow.Data.Slot = (uint)i;
                    inventoryRow.Data.ItemGuid = "@IGUID+" + itemGuidCounter;
                    inventoryRow.Data.ItemTemplate = (uint)itemId;
                    characterInventoryRows.Add(inventoryRow);

                    Row<CharacterItemInstance> itemInstanceRow = new Row<CharacterItemInstance>();
                    itemInstanceRow.Data.Guid = "@IGUID+" + itemGuidCounter;
                    itemInstanceRow.Data.ItemEntry = (uint)itemId;
                    itemInstanceRow.Data.OwnerGuid = row.Data.Guid;
                    characterItemInstaceRows.Add(itemInstanceRow);

                    itemGuidCounter++;

                    if (row.Data.EquipmentCache.Length > 0)
                        row.Data.EquipmentCache += " ";

                    row.Data.EquipmentCache += itemId + " " + enchantId;
                }

                characterRows.Add(row);

                if (maxDbGuid < player.DbGuid)
                    maxDbGuid = player.DbGuid;

                // Character wasn't actually seen in game, so there is no replay data.
                // Object was constructed from characters enum packet (before enter world).
                if (moveData == null)
                    continue;

                if (Settings.SqlTables.player)
                {
                    Row<PlayerTemplate> playerRow = new Row<PlayerTemplate>();

                    playerRow.Data.AreaID = 0;
                    if (player.Area != -1)
                        playerRow.Data.AreaID = (uint)player.Area;

                    playerRow.Data.ZoneID = 0;
                    if (player.Zone != -1)
                        playerRow.Data.ZoneID = (uint)player.Zone;

                    playerRow.Data.Guid = row.Data.Guid;
                    playerRow.Data.Name = row.Data.Name;
                    playerRow.Data.Race = row.Data.Race;
                    playerRow.Data.Class = row.Data.Class;
                    playerRow.Data.Gender = row.Data.Gender;
                    playerRow.Data.Level = row.Data.Level;
                    playerRow.Data.Skin = row.Data.Skin;
                    playerRow.Data.Face = row.Data.Face;
                    playerRow.Data.HairStyle = row.Data.HairStyle;
                    playerRow.Data.HairColor = row.Data.HairColor;
                    playerRow.Data.FacialHair = row.Data.FacialHair;
                    playerRow.Data.PlayerFlags = row.Data.PlayerFlags;
                    playerRow.Data.PvPRank = player.PlayerDataOriginal.PvPRank;
                    playerRow.Data.PositionX = row.Data.PositionX;
                    playerRow.Data.PositionY = row.Data.PositionY;
                    playerRow.Data.PositionZ = row.Data.PositionZ;
                    playerRow.Data.Orientation = row.Data.Orientation;
                    playerRow.Data.Map = row.Data.Map;
                    playerRow.Data.DisplayID = (uint)player.UnitDataOriginal.DisplayID;
                    playerRow.Data.NativeDisplayID = (uint)player.UnitDataOriginal.NativeDisplayID;
                    playerRow.Data.MountDisplayID = (uint)player.UnitDataOriginal.MountDisplayID;
                    playerRow.Data.FactionTemplate = (uint)player.UnitDataOriginal.FactionTemplate;
                    playerRow.Data.UnitFlags = player.UnitDataOriginal.Flags;
                    playerRow.Data.UnitFlags2 = player.UnitDataOriginal.Flags2;
                    playerRow.Data.CurHealth = (uint)player.UnitDataOriginal.Health;
                    playerRow.Data.MaxHealth = (uint)player.UnitDataOriginal.MaxHealth;
                    playerRow.Data.PowerType = player.UnitDataOriginal.DisplayPower;
                    if (ClientVersion.AddedInVersion(ClientVersionBuild.V4_1_0a_14007))
                    {
                        // power indexes are class specific
                        playerRow.Data.CurrentPower = (uint)player.UnitDataOriginal.Mana;
                        playerRow.Data.MaxPower = (uint)player.UnitDataOriginal.MaxMana;
                    }
                    else
                    {
                        playerRow.Data.CurrentPower = (uint)player.UnitDataOriginal.Power[(int)playerRow.Data.PowerType];
                        playerRow.Data.MaxPower = (uint)player.UnitDataOriginal.MaxPower[(int)playerRow.Data.PowerType];
                    }
                    playerRow.Data.AuraState = player.UnitDataOriginal.AuraState;
                    playerRow.Data.EmoteState = (uint)player.UnitDataOriginal.EmoteState;
                    playerRow.Data.StandState = player.UnitDataOriginal.StandState;
                    //playerRow.Data.PetTalentPoints = player.UnitDataOriginal.PetTalentPoints;
                    playerRow.Data.VisFlags = player.UnitDataOriginal.VisFlags;
                    playerRow.Data.AnimTier = player.UnitDataOriginal.AnimTier;
                    playerRow.Data.SheatheState = player.UnitDataOriginal.SheatheState;
                    playerRow.Data.PvpFlags = player.UnitDataOriginal.PvpFlags;
                    //playerRow.Data.PetFlags = player.UnitDataOriginal.PetFlags;
                    playerRow.Data.ShapeshiftForm = player.UnitDataOriginal.ShapeshiftForm;
                    playerRow.Data.SpeedWalk = moveData.WalkSpeed / MovementInfo.DEFAULT_WALK_SPEED;
                    playerRow.Data.SpeedRun = moveData.RunSpeed / MovementInfo.DEFAULT_RUN_SPEED;
                    playerRow.Data.SpeedRunBack = moveData.RunBackSpeed / MovementInfo.DEFAULT_RUN_BACK_SPEED;
                    playerRow.Data.SpeedSwim = moveData.SwimSpeed / MovementInfo.DEFAULT_SWIM_SPEED;
                    playerRow.Data.SpeedSwimBack = moveData.SwimBackSpeed / MovementInfo.DEFAULT_SWIM_BACK_SPEED;
                    playerRow.Data.SpeedFly = moveData.FlightSpeed / MovementInfo.DEFAULT_FLY_SPEED;
                    playerRow.Data.SpeedFlyBack = moveData.FlightBackSpeed / MovementInfo.DEFAULT_FLY_BACK_SPEED;
                    playerRow.Data.Scale = player.ObjectDataOriginal.Scale;
                    playerRow.Data.BoundingRadius = player.UnitDataOriginal.BoundingRadius;
                    playerRow.Data.CombatReach = player.UnitDataOriginal.CombatReach;
                    playerRow.Data.ModMeleeHaste = player.UnitDataOriginal.ModHaste;
                    playerRow.Data.MainHandAttackTime = player.UnitDataOriginal.AttackRoundBaseTime[0];
                    playerRow.Data.OffHandAttackTime = player.UnitDataOriginal.AttackRoundBaseTime[1];
                    playerRow.Data.RangedAttackTime = player.UnitDataOriginal.RangedAttackRoundBaseTime;
                    playerRow.Data.ChannelSpellId = (uint)player.UnitDataOriginal.ChannelData.SpellID;
                    playerRow.Data.ChannelVisualId = (uint)player.UnitDataOriginal.ChannelData.SpellVisual.SpellXSpellVisualID;
                    playerRow.Data.Auras = player.GetOriginalAurasString(false);
                    playerRow.Data.EquipmentCache = row.Data.EquipmentCache;
                    playerRows.Add(playerRow);
                }

                if (Settings.SqlTables.player_guid_values)
                {
                    if (!player.UnitDataOriginal.Charm.IsEmpty() ||
                        !player.UnitDataOriginal.Summon.IsEmpty() ||
                        !player.UnitDataOriginal.CharmedBy.IsEmpty() ||
                        !player.UnitDataOriginal.SummonedBy.IsEmpty() ||
                        !player.UnitDataOriginal.CreatedBy.IsEmpty() ||
                        !player.UnitDataOriginal.DemonCreator.IsEmpty() ||
                        !player.UnitDataOriginal.Target.IsEmpty())
                    {
                        Row<CreatureGuidValues> guidsRow = new Row<CreatureGuidValues>();
                        guidsRow.Data.GUID = row.Data.Guid;
                        Storage.GetObjectDbGuidEntryType(player.UnitDataOriginal.Charm, out guidsRow.Data.CharmGuid, out guidsRow.Data.CharmId, out guidsRow.Data.CharmType);
                        Storage.GetObjectDbGuidEntryType(player.UnitDataOriginal.Summon, out guidsRow.Data.SummonGuid, out guidsRow.Data.SummonId, out guidsRow.Data.SummonType);
                        Storage.GetObjectDbGuidEntryType(player.UnitDataOriginal.CharmedBy, out guidsRow.Data.CharmedByGuid, out guidsRow.Data.CharmedById, out guidsRow.Data.CharmedByType);
                        Storage.GetObjectDbGuidEntryType(player.UnitDataOriginal.SummonedBy, out guidsRow.Data.SummonedByGuid, out guidsRow.Data.SummonedById, out guidsRow.Data.SummonedByType);
                        Storage.GetObjectDbGuidEntryType(player.UnitDataOriginal.CreatedBy, out guidsRow.Data.CreatedByGuid, out guidsRow.Data.CreatedById, out guidsRow.Data.CreatedByType);
                        Storage.GetObjectDbGuidEntryType(player.UnitDataOriginal.DemonCreator, out guidsRow.Data.DemonCreatorGuid, out guidsRow.Data.DemonCreatorId, out guidsRow.Data.DemonCreatorType);
                        Storage.GetObjectDbGuidEntryType(player.UnitDataOriginal.Target, out guidsRow.Data.TargetGuid, out guidsRow.Data.TargetId, out guidsRow.Data.TargetType);
                        playerGuidValuesRows.Add(guidsRow);
                    }
                }

                if (Settings.SqlTables.player_power_values)
                {
                    var powers = player.UnitDataOriginal.Power;
                    var maxPowers = player.UnitDataOriginal.MaxPower;
                    for (int i = 0; i < ClientVersion.GetPowerCountForClientVersion(); i++)
                    {
                        if (powers[i] != 0 || maxPowers[i] != 0)
                        {
                            Row<CreaturePowerValues> powerRow = new Row<CreaturePowerValues>();
                            powerRow.Data.GUID = "@PGUID+" + row.Data.Guid;
                            powerRow.Data.PowerType = (uint)i;
                            powerRow.Data.CurrentPower = (uint)powers[i];
                            powerRow.Data.MaxPower = (uint)maxPowers[i];
                            playerPowerValuesRows.Add(powerRow);
                        }
                    }
                }

                if (Settings.SqlTables.player_attack_log)
                {
                    if (Storage.UnitAttackLogs.ContainsKey(objPair.Key))
                    {
                        foreach (var attack in Storage.UnitAttackLogs[objPair.Key])
                        {
                            Row<UnitMeleeAttackLog> attackRow = new Row<UnitMeleeAttackLog>();
                            attackRow.Data = attack;
                            attackRow.Data.GUID = row.Data.Guid;
                            Storage.GetObjectDbGuidEntryType(attack.Victim, out attackRow.Data.VictimGuid, out attackRow.Data.VictimId, out attackRow.Data.VictimType);
                            attackRow.Data.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(attack.Time);
                            playerAttackLogRows.Add(attackRow);
                        }
                    }
                }

                if (Settings.SqlTables.player_attack_start)
                {
                    if (Storage.UnitAttackStartTimes.ContainsKey(objPair.Key))
                    {
                        foreach (var attack in Storage.UnitAttackStartTimes[objPair.Key])
                        {
                            Row<CreatureAttackToggle> attackRow = new Row<CreatureAttackToggle>();
                            attackRow.Data.GUID = row.Data.Guid;
                            Storage.GetObjectDbGuidEntryType(attack.victim, out attackRow.Data.VictimGuid, out attackRow.Data.VictimId, out attackRow.Data.VictimType);
                            attackRow.Data.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(attack.time);
                            playerAttackStartRows.Add(attackRow);
                        }
                    }
                }

                if (Settings.SqlTables.player_attack_stop)
                {
                    if (Storage.UnitAttackStopTimes.ContainsKey(objPair.Key))
                    {
                        foreach (var attack in Storage.UnitAttackStopTimes[objPair.Key])
                        {
                            Row<CreatureAttackToggle> attackRow = new Row<CreatureAttackToggle>();
                            attackRow.Data.GUID = row.Data.Guid;
                            Storage.GetObjectDbGuidEntryType(attack.victim, out attackRow.Data.VictimGuid, out attackRow.Data.VictimId, out attackRow.Data.VictimType);
                            attackRow.Data.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(attack.time);
                            playerAttackStopRows.Add(attackRow);
                        }
                    }
                }

                if (Settings.SqlTables.player_create1_time)
                {
                    if (Storage.ObjectCreate1Times.ContainsKey(objPair.Key))
                    {
                        foreach (var createTime in Storage.ObjectCreate1Times[objPair.Key])
                        {
                            var create1Row = new Row<PlayerCreate1>();
                            create1Row.Data.GUID = row.Data.Guid;
                            create1Row.Data.Map = createTime.Map;
                            create1Row.Data.PositionX = createTime.MoveInfo.Position.X;
                            create1Row.Data.PositionY = createTime.MoveInfo.Position.Y;
                            create1Row.Data.PositionZ = createTime.MoveInfo.Position.Z;
                            create1Row.Data.Orientation = createTime.MoveInfo.Orientation;
                            create1Row.Data.VehicleId = createTime.MoveInfo.VehicleId;
                            create1Row.Data.VehicleOrientation = createTime.MoveInfo.VehicleOrientation;
                            if (createTime.MoveInfo.TransportGuid != null && !createTime.MoveInfo.TransportGuid.IsEmpty())
                            {
                                Storage.GetObjectDbGuidEntryType(createTime.MoveInfo.TransportGuid, out create1Row.Data.TransportGuid, out create1Row.Data.TransportId, out create1Row.Data.TransportType);
                                create1Row.Data.TransportPositionX = createTime.MoveInfo.TransportOffset.X;
                                create1Row.Data.TransportPositionY = createTime.MoveInfo.TransportOffset.Y;
                                create1Row.Data.TransportPositionZ = createTime.MoveInfo.TransportOffset.Z;
                                create1Row.Data.TransportOrientation = createTime.MoveInfo.TransportOffset.O;
                                create1Row.Data.TransportTime = createTime.MoveInfo.TransportTime;
                                create1Row.Data.TransportSeat = createTime.MoveInfo.TransportSeat;
                            }
                            create1Row.Data.MoveTime = createTime.MoveInfo.MoveTime;
                            create1Row.Data.MoveFlags = createTime.MoveInfo.Flags;
                            create1Row.Data.MoveFlags2 = createTime.MoveInfo.FlagsExtra;
                            create1Row.Data.SwimPitch = createTime.MoveInfo.SwimPitch;
                            create1Row.Data.FallTime = createTime.MoveInfo.FallTime;
                            create1Row.Data.JumpHorizontalSpeed = createTime.MoveInfo.JumpHorizontalSpeed;
                            create1Row.Data.JumpVerticalSpeed = createTime.MoveInfo.JumpVerticalSpeed;
                            create1Row.Data.JumpCosAngle = createTime.MoveInfo.JumpCosAngle;
                            create1Row.Data.JumpSinAngle = createTime.MoveInfo.JumpSinAngle;
                            create1Row.Data.SplineElevation = createTime.MoveInfo.SplineElevation;
                            create1Row.Data.UnixTimeMs = createTime.UnixTimeMs;
                            playerCreate1Rows.Add(create1Row);
                        }
                    }
                }

                if (Settings.SqlTables.player_create2_time)
                {
                    if (Storage.ObjectCreate2Times.ContainsKey(objPair.Key))
                    {
                        foreach (var createTime in Storage.ObjectCreate2Times[objPair.Key])
                        {
                            var create2Row = new Row<PlayerCreate2>();
                            create2Row.Data.GUID = row.Data.Guid;
                            create2Row.Data.Map = createTime.Map;
                            create2Row.Data.PositionX = createTime.MoveInfo.Position.X;
                            create2Row.Data.PositionY = createTime.MoveInfo.Position.Y;
                            create2Row.Data.PositionZ = createTime.MoveInfo.Position.Z;
                            create2Row.Data.Orientation = createTime.MoveInfo.Orientation;
                            create2Row.Data.VehicleId = createTime.MoveInfo.VehicleId;
                            create2Row.Data.VehicleOrientation = createTime.MoveInfo.VehicleOrientation;
                            if (createTime.MoveInfo.TransportGuid != null && !createTime.MoveInfo.TransportGuid.IsEmpty())
                            {
                                Storage.GetObjectDbGuidEntryType(createTime.MoveInfo.TransportGuid, out create2Row.Data.TransportGuid, out create2Row.Data.TransportId, out create2Row.Data.TransportType);
                                create2Row.Data.TransportPositionX = createTime.MoveInfo.TransportOffset.X;
                                create2Row.Data.TransportPositionY = createTime.MoveInfo.TransportOffset.Y;
                                create2Row.Data.TransportPositionZ = createTime.MoveInfo.TransportOffset.Z;
                                create2Row.Data.TransportOrientation = createTime.MoveInfo.TransportOffset.O;
                                create2Row.Data.TransportTime = createTime.MoveInfo.TransportTime;
                                create2Row.Data.TransportSeat = createTime.MoveInfo.TransportSeat;
                            }
                            create2Row.Data.MoveTime = createTime.MoveInfo.MoveTime;
                            create2Row.Data.MoveFlags = createTime.MoveInfo.Flags;
                            create2Row.Data.MoveFlags2 = createTime.MoveInfo.FlagsExtra;
                            create2Row.Data.SwimPitch = createTime.MoveInfo.SwimPitch;
                            create2Row.Data.FallTime = createTime.MoveInfo.FallTime;
                            create2Row.Data.JumpHorizontalSpeed = createTime.MoveInfo.JumpHorizontalSpeed;
                            create2Row.Data.JumpVerticalSpeed = createTime.MoveInfo.JumpVerticalSpeed;
                            create2Row.Data.JumpCosAngle = createTime.MoveInfo.JumpCosAngle;
                            create2Row.Data.JumpSinAngle = createTime.MoveInfo.JumpSinAngle;
                            create2Row.Data.SplineElevation = createTime.MoveInfo.SplineElevation;
                            create2Row.Data.UnixTimeMs = createTime.UnixTimeMs;
                            playerCreate2Rows.Add(create2Row);
                        }
                    }
                }

                if (Settings.SqlTables.player_destroy_time)
                {
                    if (Storage.ObjectDestroyTimes.ContainsKey(objPair.Key))
                    {
                        foreach (var createTime in Storage.ObjectDestroyTimes[objPair.Key])
                        {
                            var destroyRow = new Row<PlayerDestroy>();
                            destroyRow.Data.GUID = row.Data.Guid;
                            destroyRow.Data.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(createTime);
                            playerDestroyRows.Add(destroyRow);
                        }
                    }
                }

                if (Settings.SqlTables.player_emote)
                {
                    if (Storage.Emotes.ContainsKey(objPair.Key))
                    {
                        foreach (var emote in Storage.Emotes[objPair.Key])
                        {
                            var emoteRow = new Row<CreatureEmote>();
                            emoteRow.Data = emote;
                            emoteRow.Data.GUID = row.Data.Guid;
                            playerEmoteRows.Add(emoteRow);
                        }
                    }
                }

                if (Settings.SqlTables.player_equipment_values_update)
                {
                    if (Storage.UnitEquipmentValuesUpdates.ContainsKey(objPair.Key))
                    {
                        foreach (var update in Storage.UnitEquipmentValuesUpdates[objPair.Key])
                        {
                            Row<CreatureEquipmentValuesUpdate> updateRow = new Row<CreatureEquipmentValuesUpdate>();
                            updateRow.Data = update;
                            updateRow.Data.GUID = row.Data.Guid;
                            updateRow.Data.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(update.time);
                            playerEquipmentValuesUpdateRows.Add(updateRow);
                        }
                    }
                }

                if (Settings.SqlTables.player_guid_values_update)
                {
                    if (Storage.UnitGuidValuesUpdates.ContainsKey(objPair.Key))
                    {
                        foreach (var update in Storage.UnitGuidValuesUpdates[objPair.Key])
                        {
                            Row<CreatureGuidValuesUpdate> updateRow = new Row<CreatureGuidValuesUpdate>();
                            updateRow.Data.GUID = row.Data.Guid;
                            updateRow.Data.FieldName = update.FieldName;
                            Storage.GetObjectDbGuidEntryType(update.guid, out updateRow.Data.ObjectGuid, out updateRow.Data.ObjectId, out updateRow.Data.ObjectType);
                            updateRow.Data.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(update.time);
                            playerGuidValuesUpdateRows.Add(updateRow);
                        }
                    }
                }

                if (Settings.SqlTables.player_power_values_update)
                {
                    if (Storage.UnitPowerValuesUpdates.ContainsKey(objPair.Key))
                    {
                        foreach (var update in Storage.UnitPowerValuesUpdates[objPair.Key])
                        {
                            var updateRow = new Row<CreaturePowerValuesUpdate>();
                            updateRow.Data = update;
                            updateRow.Data.GUID = row.Data.Guid;
                            playerPowerValuesUpdateRows.Add(updateRow);
                        }
                    }
                }

                if (Settings.SqlTables.player_auras_update)
                {
                    if (Storage.UnitAurasUpdates.ContainsKey(objPair.Key))
                    {
                        uint updateId = 0;
                        foreach (var update in Storage.UnitAurasUpdates[objPair.Key])
                        {
                            updateId++;

                            if (update.Auras == null)
                            {
                                var updateRow = new Row<CreatureAurasUpdate>();
                                updateRow.Data.GUID = row.Data.Guid;
                                updateRow.Data.UpdateId = updateId;
                                updateRow.Data.IsFullUpdate = update.IsFullUpdate;
                                updateRow.Data.Slot = -1;
                                updateRow.Data.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(update.Time);
                                playerAurasUpdateRows.Add(updateRow);
                                continue;
                            }

                            foreach (var aura in update.Auras)
                            {
                                var updateRow = new Row<CreatureAurasUpdate>();
                                updateRow.Data.GUID = row.Data.Guid;
                                updateRow.Data.UpdateId = updateId;
                                updateRow.Data.IsFullUpdate = update.IsFullUpdate;
                                updateRow.Data.Slot = (int)aura.Slot;
                                updateRow.Data.SpellId = aura.SpellId;
                                updateRow.Data.VisualId = aura.VisualId;
                                updateRow.Data.AuraFlags = aura.AuraFlags;
                                updateRow.Data.ActiveFlags = aura.ActiveFlags;
                                updateRow.Data.Level = aura.Level;
                                updateRow.Data.Charges = aura.Charges;
                                updateRow.Data.ContentTuningId = aura.ContentTuningId;
                                updateRow.Data.Duration = aura.Duration;
                                updateRow.Data.MaxDuration = aura.MaxDuration;
                                Storage.GetObjectDbGuidEntryType(aura.CasterGuid, out updateRow.Data.CasterGuid, out updateRow.Data.CasterId, out updateRow.Data.CasterType);
                                updateRow.Data.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(update.Time);
                                playerAurasUpdateRows.Add(updateRow);
                            }
                        }
                    }
                }

                if (Settings.SqlTables.player_values_update)
                {
                    if (Storage.UnitValuesUpdates.ContainsKey(objPair.Key))
                    {
                        foreach (var update in Storage.UnitValuesUpdates[objPair.Key])
                        {
                            var updateRow = new Row<CreatureValuesUpdate>();
                            updateRow.Data = update;
                            updateRow.Data.GUID = row.Data.Guid;
                            playerValuesUpdateRows.Add(updateRow);
                        }
                    }
                }

                if (Settings.SqlTables.player_speed_update)
                {
                    if (Storage.UnitSpeedUpdates.ContainsKey(objPair.Key))
                    {
                        foreach (var update in Storage.UnitSpeedUpdates[objPair.Key])
                        {
                            var updateRow = new Row<CreatureSpeedUpdate>();
                            updateRow.Data = update;
                            updateRow.Data.GUID = row.Data.Guid;
                            playerSpeedUpdateRows.Add(updateRow);
                        }
                    }
                }

                if (Settings.SqlTables.player_chat)
                {
                    foreach (var text in Storage.CharacterTexts)
                    {
                        if (text.Item1.SenderGUID == objPair.Key)
                        {
                            text.Item1.Guid = "@PGUID+" + player.DbGuid;
                            text.Item1.SenderName = row.Data.Name;
                        }
                    }
                }

                if (Settings.SqlTables.player_movement_server)
                {
                    foreach (ServerSideMovementSpline waypoint in player.CombatMovementSplines)
                    {
                        var movementSplineRow = new Row<ServerSideMovementSpline>();
                        movementSplineRow.Data = waypoint;
                        movementSplineRow.Data.GUID = "@PGUID+" + player.DbGuid;
                        playerServerMovementSplineRows.Add(movementSplineRow);
                    }

                    foreach (ServerSideMovement waypoint in player.CombatMovements)
                    {
                        if (waypoint == null)
                            break;

                        var movementRow = new Row<ServerSideMovement>();
                        movementRow.Data = waypoint;
                        movementRow.Data.GUID = "@PGUID+" + player.DbGuid;
                        if (waypoint.TransportGuid != null && !waypoint.TransportGuid.IsEmpty())
                            Storage.GetObjectDbGuidEntryType(waypoint.TransportGuid, out movementRow.Data.TransportGUID, out movementRow.Data.TransportId, out movementRow.Data.TransportType);
                        playerServerMovementRows.Add(movementRow);
                    }
                }

                if (Settings.SqlTables.player_minimap_ping)
                {
                    if (Storage.PlayerMinimapPings.ContainsKey(objPair.Key))
                    {
                        foreach (var ping in Storage.PlayerMinimapPings[objPair.Key])
                        {
                            Row<PlayerMinimapPing> pingRow = new Row<PlayerMinimapPing>();
                            pingRow.Data = ping;
                            pingRow.Data.GUID = row.Data.Guid;
                            playerMinimapPingRows.Add(pingRow);
                        }
                    }
                }

                if (Settings.SqlTables.character_reputation)
                {
                    if (Storage.CharacterReputations.ContainsKey(objPair.Key))
                    {
                        foreach (var repData in Storage.CharacterReputations[objPair.Key])
                        {
                            if (repData.Value.Standing != 0 || repData.Value.Flags != 0)
                            {
                                var repRow = new Row<CharacterReputation>();
                                repRow.Data.Guid = "@PGUID+" + player.DbGuid;
                                repRow.Data.Faction = repData.Value.Faction;
                                repRow.Data.Standing = repData.Value.Standing;
                                repRow.Data.Flags = repData.Value.Flags != null ? (uint)repData.Value.Flags : 0;
                                characterReputationRows.Add(repRow);
                            }
                        }
                    }
                }

                if (Settings.SqlTables.character_skills)
                {
                    const uint PLAYER_MAX_SKILLS = 256;
                    ISkillInfo skillData = player.ActivePlayerData.Skill;

                    for (uint i = 0; i < PLAYER_MAX_SKILLS; ++i)
                    {
                        uint skillId = skillData.SkillLineID[i];
                        uint skillRank = skillData.SkillRank[i];
                        uint skillMaxRank = skillData.SkillMaxRank[i];

                        if (skillId != 0 && skillMaxRank != 0)
                        {
                            var skillRow = new Row<CharacterSkill>();
                            skillRow.Data.Guid = "@PGUID+" + player.DbGuid;
                            skillRow.Data.Skill = skillId;
                            skillRow.Data.Value = skillRank;
                            skillRow.Data.Max = skillMaxRank;
                            characterSkillRows.Add(skillRow);
                        }
                    }
                }

                if (Settings.SqlTables.character_spell)
                {
                    if (Storage.CharacterSpells.ContainsKey(objPair.Key))
                    {
                        foreach (var spellId in Storage.CharacterSpells[objPair.Key])
                        {
                            var spellRow = new Row<CharacterSpell>();
                            spellRow.Data.Guid = "@PGUID+" + player.DbGuid;
                            spellRow.Data.Spell = spellId;
                            spellRow.Data.Active = 1;
                            spellRow.Data.Disabled = 0;
                            characterSpellRows.Add(spellRow);
                        }
                    }
                }

                if (Settings.SqlTables.guild)
                {
                    if (player.UnitData.GuildGUID.Low != 0)
                    {
                        var guildRow = new Row<GuildMember>();
                        guildRow.Data.GuildGUID = player.UnitDataOriginal.GuildGUID.Low;
                        guildRow.Data.Guid = "@PGUID+" + player.DbGuid;
                        guildRow.Data.GuildRank = player.PlayerDataOriginal.GuildRankID;
                        guildMemberRows.Add(guildRow);
                    }
                }
            }

            if (Settings.SqlTables.characters && characterRows.Count != 0)
            {
                var characterDelete = new SQLDelete<CharacterTemplate>(Tuple.Create("@PGUID+0", "@PGUID+" + maxDbGuid));
                result.Append(characterDelete.Build());
                var characterSql = new SQLInsert<CharacterTemplate>(characterRows, false);
                result.Append(characterSql.Build());
                result.AppendLine();
            }

            if (Settings.SqlTables.character_inventory && characterInventoryRows.Count != 0)
            {
                var inventoryDelete = new SQLDelete<CharacterInventory>(Tuple.Create("@IGUID+0", "@IGUID+" + itemGuidCounter));
                result.Append(inventoryDelete.Build());
                var inventorySql = new SQLInsert<CharacterInventory>(characterInventoryRows, false);
                result.Append(inventorySql.Build());
                result.AppendLine();

                var itemInstanceDelete = new SQLDelete<CharacterItemInstance>(Tuple.Create("@IGUID+0", "@IGUID+" + itemGuidCounter));
                result.Append(itemInstanceDelete.Build());
                var itemInstanceSql = new SQLInsert<CharacterItemInstance>(characterItemInstaceRows, false);
                result.Append(itemInstanceSql.Build());
                result.AppendLine();
            }

            if (Settings.SqlTables.character_reputation && characterReputationRows.Count != 0)
            {
                var repSql = new SQLInsert<CharacterReputation>(characterReputationRows, false);
                result.Append(repSql.Build());
                result.AppendLine();
            }

            if (Settings.SqlTables.character_skills && characterSkillRows.Count != 0)
            {
                var skillsSql = new SQLInsert<CharacterSkill>(characterSkillRows, false);
                result.Append(skillsSql.Build());
                result.AppendLine();
            }

            if (Settings.SqlTables.character_spell && characterSpellRows.Count != 0)
            {
                var spellsSql = new SQLInsert<CharacterSpell>(characterSpellRows, false);
                result.Append(spellsSql.Build());
                result.AppendLine();
            }

            if (Settings.SqlTables.guild && guildMemberRows.Count != 0)
            {
                var guildSql = new SQLInsert<GuildMember>(guildMemberRows, false);
                result.Append(guildSql.Build());
                result.AppendLine();
            }

            if (Settings.SqlTables.player && playerRows.Count != 0)
            {
                var playerDelete = new SQLDelete<PlayerTemplate>(Tuple.Create("@PGUID+0", "@PGUID+" + maxDbGuid));
                result.Append(playerDelete.Build());
                var playerSql = new SQLInsert<PlayerTemplate>(playerRows, false);
                result.Append(playerSql.Build());
                result.AppendLine();
            }

            if (Settings.SqlTables.player_guid_values && playerGuidValuesRows.Count != 0)
            {
                var guidValuesDelete = new SQLDelete<CreatureGuidValues>(Tuple.Create("@PGUID+0", "@PGUID+" + maxDbGuid));
                guidValuesDelete.tableNameOverride = "player_guid_values";
                result.Append(guidValuesDelete.Build());
                var guidValuesSql = new SQLInsert<CreatureGuidValues>(playerGuidValuesRows, false, false, "player_guid_values");
                result.Append(guidValuesSql.Build());
                result.AppendLine();
            }

            if (Settings.SqlTables.player_power_values && playerPowerValuesRows.Count != 0)
            {
                var powerValuesDelete = new SQLDelete<CreaturePowerValues>(Tuple.Create("@PGUID+0", "@PGUID+" + maxDbGuid));
                powerValuesDelete.tableNameOverride = "player_power_values";
                result.Append(powerValuesDelete.Build());
                var powerValuesSql = new SQLInsert<CreaturePowerValues>(playerPowerValuesRows, false, false, "player_power_values");
                result.Append(powerValuesSql.Build());
                result.AppendLine();
            }

            if (Settings.SqlTables.player_active_player && Storage.PlayerActiveCreateTime.Count != 0)
            {
                var activePlayersRows = new RowList<CharacterActivePlayer>();
                foreach (var itr in Storage.PlayerActiveCreateTime)
                {
                    Row<CharacterActivePlayer> row = new Row<CharacterActivePlayer>();
                    row.Data.Guid = Storage.GetObjectDbGuid(itr.Guid);
                    row.Data.UnixTime = (uint)Utilities.GetUnixTimeFromDateTime(itr.Time);
                    activePlayersRows.Add(row);
                }
                var activePlayersSql = new SQLInsert<CharacterActivePlayer>(activePlayersRows, false);
                result.Append(activePlayersSql.Build());
                result.AppendLine();
            }

            if (Settings.SqlTables.player_create1_time && playerCreate1Rows.Count != 0)
            {
                var createSql = new SQLInsert<PlayerCreate1>(playerCreate1Rows, false);
                result.Append(createSql.Build());
                result.AppendLine();
            }

            if (Settings.SqlTables.player_create2_time && playerCreate2Rows.Count != 0)
            {
                var createSql = new SQLInsert<PlayerCreate2>(playerCreate2Rows, false);
                result.Append(createSql.Build());
                result.AppendLine();
            }

            if (Settings.SqlTables.player_destroy_time && playerDestroyRows.Count != 0)
            {
                var destroySql = new SQLInsert<PlayerDestroy>(playerDestroyRows, false);
                result.Append(destroySql.Build());
                result.AppendLine();
            }

            if (Settings.SqlTables.player_movement_client)
            {
                uint moveCounter = 0;
                var movementRows = new RowList<ClientSideMovement>();
                foreach (var movement in Storage.PlayerMovements)
                {
                    if (movement.Guid.GetObjectType() != ObjectType.Player &&
                        movement.Guid.GetObjectType() != ObjectType.ActivePlayer)
                        continue;

                    WoWObject obj;
                    if (!Storage.Objects.TryGetValue(movement.Guid, out obj))
                        continue;

                    Player player = obj as Player;
                    if (player == null)
                        continue;

                    if (Settings.SkipOtherPlayers && !player.IsActivePlayer &&
                           (movement.OpcodeDirection != Direction.ClientToServer))
                        continue;

                    Row<ClientSideMovement> row = new Row<ClientSideMovement>();
                    row.Data.Guid = "@PGUID+" + player.DbGuid;
                    row.Data.MoveFlags = movement.MoveInfo.Flags;
                    row.Data.MoveFlags2 = movement.MoveInfo.FlagsExtra;
                    row.Data.MoveTime = movement.MoveInfo.MoveTime;
                    row.Data.Map = movement.Map;
                    row.Data.PositionX = movement.MoveInfo.Position.X;
                    row.Data.PositionY = movement.MoveInfo.Position.Y;
                    row.Data.PositionZ = movement.MoveInfo.Position.Z;
                    row.Data.Orientation = movement.MoveInfo.Orientation;
                    if (movement.MoveInfo.TransportGuid != null && !movement.MoveInfo.TransportGuid.IsEmpty())
                    {
                        Storage.GetObjectDbGuidEntryType(movement.MoveInfo.TransportGuid, out row.Data.TransportGuid, out row.Data.TransportId, out row.Data.TransportType);
                        row.Data.TransportPositionX = movement.MoveInfo.TransportOffset.X;
                        row.Data.TransportPositionY = movement.MoveInfo.TransportOffset.Y;
                        row.Data.TransportPositionZ = movement.MoveInfo.TransportOffset.Z;
                        row.Data.TransportOrientation = movement.MoveInfo.TransportOffset.O;
                        row.Data.TransportTime = movement.MoveInfo.TransportTime;
                        row.Data.TransportSeat = movement.MoveInfo.TransportSeat;
                    }
                    row.Data.SwimPitch = movement.MoveInfo.SwimPitch;
                    row.Data.FallTime = movement.MoveInfo.FallTime;
                    row.Data.JumpHorizontalSpeed = movement.MoveInfo.JumpHorizontalSpeed;
                    row.Data.JumpVerticalSpeed = movement.MoveInfo.JumpVerticalSpeed;
                    row.Data.JumpCosAngle = movement.MoveInfo.JumpCosAngle;
                    row.Data.JumpSinAngle = movement.MoveInfo.JumpSinAngle;
                    row.Data.SplineElevation = movement.MoveInfo.SplineElevation;
                    row.Data.PacketId = moveCounter++;
                    row.Data.Opcode = Opcodes.GetOpcodeName(movement.Opcode, movement.OpcodeDirection);
                    row.Data.UnixTimeMs = (ulong)Utilities.GetUnixTimeMsFromDateTime(movement.Time);
                    movementRows.Add(row);
                }

                if (movementRows.Count != 0)
                {
                    var movementSql = new SQLInsert<ClientSideMovement>(movementRows, false);
                    result.Append(movementSql.Build());
                    result.AppendLine();
                }
            }

            if (Settings.SqlTables.player_movement_server && playerServerMovementRows.Count != 0)
            {
                var movementSql = new SQLInsert<ServerSideMovement>(playerServerMovementRows, false, false, "player_movement_server");
                result.Append(movementSql.Build());
                result.AppendLine();

                var movementSplineSql = new SQLInsert<ServerSideMovementSpline>(playerServerMovementSplineRows, false, false, "player_movement_server_spline");
                result.Append(movementSplineSql.Build());
                result.AppendLine();
            }

            if (Settings.SqlTables.player_attack_log && playerAttackLogRows.Count != 0)
            {
                var attackLogSql = new SQLInsert<UnitMeleeAttackLog>(playerAttackLogRows, false, false, "player_attack_log");
                result.Append(attackLogSql.Build());
                result.AppendLine();
            }

            if (Settings.SqlTables.player_attack_start && playerAttackStartRows.Count != 0)
            {
                var attackStartSql = new SQLInsert<CreatureAttackToggle>(playerAttackStartRows, false, false, "player_attack_start");
                result.Append(attackStartSql.Build());
                result.AppendLine();
            }

            if (Settings.SqlTables.player_attack_stop && playerAttackStopRows.Count != 0)
            {
                var attackStopSql = new SQLInsert<CreatureAttackToggle>(playerAttackStopRows, false, false, "player_attack_stop");
                result.Append(attackStopSql.Build());
                result.AppendLine();
            }

            if (Settings.SqlTables.player_emote && playerEmoteRows.Count != 0)
            {
                var emoteSql = new SQLInsert<CreatureEmote>(playerEmoteRows, false, false, "player_emote");
                result.Append(emoteSql.Build());
                result.AppendLine();
            }

            if (Settings.SqlTables.player_minimap_ping && playerMinimapPingRows.Count != 0)
            {
                var pingSql = new SQLInsert<PlayerMinimapPing>(playerMinimapPingRows, false, false);
                result.Append(pingSql.Build());
                result.AppendLine();
            }

            if (Settings.SqlTables.player_equipment_values_update && playerEquipmentValuesUpdateRows.Count != 0)
            {
                var equipmentUpdateSql = new SQLInsert<CreatureEquipmentValuesUpdate>(playerEquipmentValuesUpdateRows, false, false, "player_equipment_values_update");
                result.Append(equipmentUpdateSql.Build());
                result.AppendLine();
            }

            if (Settings.SqlTables.player_auras_update && playerAurasUpdateRows.Count != 0)
            {
                var aurasUpdateSql = new SQLInsert<CreatureAurasUpdate>(playerAurasUpdateRows, false, false, "player_auras_update");
                result.Append(aurasUpdateSql.Build());
                result.AppendLine();
            }

            if (Settings.SqlTables.player_values_update && playerValuesUpdateRows.Count != 0)
            {
                var valuesUpdateSql = new SQLInsert<CreatureValuesUpdate>(playerValuesUpdateRows, false, false, "player_values_update");
                result.Append(valuesUpdateSql.Build());
                result.AppendLine();
            }

            if (Settings.SqlTables.player_guid_values_update && playerGuidValuesUpdateRows.Count != 0)
            {
                var guidsUpdateSql = new SQLInsert<CreatureGuidValuesUpdate>(playerGuidValuesUpdateRows, false, false, "player_guid_values_update");
                result.Append(guidsUpdateSql.Build());
                result.AppendLine();
            }

            if (Settings.SqlTables.player_power_values_update && playerPowerValuesUpdateRows.Count != 0)
            {
                var updateSql = new SQLInsert<CreaturePowerValuesUpdate>(playerPowerValuesUpdateRows, false, false, "player_power_values_update");
                result.Append(updateSql.Build());
                result.AppendLine();
            }

            if (Settings.SqlTables.player_speed_update && playerSpeedUpdateRows.Count != 0)
            {
                var speedUpdateSql = new SQLInsert<CreatureSpeedUpdate>(playerSpeedUpdateRows, false, false, "player_speed_update");
                result.Append(speedUpdateSql.Build());
                result.AppendLine();
            }

            if (Settings.SqlTables.player_chat && !Storage.CharacterTexts.IsEmpty())
            {
                foreach (var text in Storage.CharacterTexts)
                {
                    if (text.Item1.Guid == null)
                    {
                        text.Item1.Guid = "0";
                        if (String.IsNullOrEmpty(text.Item1.SenderName) && !text.Item1.SenderGUID.IsEmpty())
                            text.Item1.SenderName = StoreGetters.GetName(text.Item1.SenderGUID);
                    }
                    if (text.Item1.ChannelName == null)
                        text.Item1.ChannelName = "";
                }
                result.Append(SQLUtil.Insert(Storage.CharacterTexts, false, true));
            }

            return result.ToString();
        }

        [BuilderMethod]
        public static string RaidTargetIconUpdates()
        {
            if (Storage.RaidTargetIconUpdates.IsEmpty())
                return string.Empty;

            if (!Settings.SqlTables.raid_target_icon_update)
                return string.Empty;

            RowList<RaidTargetIconUpdate> rows = new RowList<RaidTargetIconUpdate>();
            foreach (var iconUpdate in Storage.RaidTargetIconUpdates)
            {
                Row<RaidTargetIconUpdate> row = new Row<RaidTargetIconUpdate>();
                row.Data = iconUpdate.Item1;

                if (row.Data.TargetGUID != null)
                {
                    Storage.GetObjectDbGuidEntryType(row.Data.TargetGUID, out row.Data.TargetGuid, out row.Data.TargetId, out row.Data.TargetType);
                    if (String.IsNullOrEmpty(row.Data.TargetType))
                        continue;
                }
                else
                {
                    if (!row.Data.IsFullUpdate)
                        continue;
                }
                rows.Add(row);
            }

            var sql = new SQLInsert<RaidTargetIconUpdate>(rows, false);
            return sql.Build();
        }

        [BuilderMethod]
        public static string PlayerClassLevelStats()
        {
            if (!Settings.SqlTables.player_classlevelstats && !Settings.SqlTables.player_levelstats)
                return string.Empty;

            foreach (var objPair in Storage.Objects)
            {
                if (objPair.Key.GetObjectType() != ObjectType.Player)
                    continue;

                Player player = objPair.Value.Item1 as Player;
                if (player == null)
                    continue;

                Storage.SavePlayerStats(player, true, null);
            }

            string result = "";

            if (Settings.SqlTables.player_classlevelstats &&
                !Storage.PlayerClassLevelStats.IsEmpty())
            {
                if (Settings.TargetedDbType == TargetedDbType.WPP)
                {
                    result += SQLUtil.MakeInsertWithSniffIdList(Storage.PlayerClassLevelStats, false, true);
                }
                else
                {
                    var templateDb = SQLDatabase.Get(Storage.PlayerClassLevelStats, Settings.TDBDatabase);

                    result += SQLUtil.Compare(Storage.PlayerClassLevelStats, templateDb, StoreNameType.None);
                }
                
            }

            if (Settings.SqlTables.player_levelstats &&
                !Storage.PlayerLevelStats.IsEmpty())
            {
                if (Settings.TargetedDbType == TargetedDbType.WPP)
                {
                    result += SQLUtil.MakeInsertWithSniffIdList(Storage.PlayerLevelStats, false, true);
                }
                else
                {
                    var templateDb = SQLDatabase.Get(Storage.PlayerLevelStats, Settings.TDBDatabase);

                    result += SQLUtil.Compare(Storage.PlayerLevelStats, templateDb, StoreNameType.None);
                }
            }

            return result;
        }

        [BuilderMethod]
        public static string PlayerLevelupInfos()
        {
            if (Storage.PlayerLevelupInfos.IsEmpty())
                return string.Empty;

            if (!Settings.SqlTables.player_levelup_info)
                return string.Empty;

            var rows = new RowList<PlayerLevelupInfo>();
            foreach (var info in Storage.PlayerLevelupInfos)
            {
                if (info.Item1.GUID == null)
                    continue;
                if (!Storage.Objects.ContainsKey(info.Item1.GUID))
                    continue;

                Player player = Storage.Objects[info.Item1.GUID].Item1 as Player;
                if (player == null)
                    continue;

                Row<PlayerLevelupInfo> row = new Row<PlayerLevelupInfo>();
                row.Data = info.Item1;
                row.Data.RaceId = player.UnitData.RaceId;
                row.Data.ClassId = player.UnitData.ClassId;
                rows.Add(row);
            }

            var sql = new SQLInsert<PlayerLevelupInfo>(rows);
            return sql.Build();
        }

        [BuilderMethod]
        public static string PlayerMeleeCritChances()
        {
            if (Storage.PlayerMeleeCritChances.IsEmpty())
                return string.Empty;

            if (!Settings.SqlTables.player_melee_crit_chance)
                return string.Empty;

            foreach (var objPair in Storage.Objects)
            {
                if (objPair.Key.GetObjectType() != ObjectType.Player)
                    continue;

                Player player = objPair.Value.Item1 as Player;
                if (player == null)
                    continue;

                Storage.SavePlayerMeleeCrit(player, null);
            }

            return SQLUtil.Insert(Storage.PlayerMeleeCritChances, false, true, "player_melee_crit_chance");
        }

        [BuilderMethod]
        public static string PlayerRangedCritChances()
        {
            if (Storage.PlayerRangedCritChances.IsEmpty())
                return string.Empty;

            if (!Settings.SqlTables.player_ranged_crit_chance)
                return string.Empty;

            foreach (var objPair in Storage.Objects)
            {
                if (objPair.Key.GetObjectType() != ObjectType.Player)
                    continue;

                Player player = objPair.Value.Item1 as Player;
                if (player == null)
                    continue;

                Storage.SavePlayerRangedCrit(player, null);
            }

            return SQLUtil.Insert(Storage.PlayerRangedCritChances, false, true, "player_ranged_crit_chance");
        }

        [BuilderMethod]
        public static string PlayerSpellCritChances()
        {
            if (Storage.PlayerSpellCritChances.IsEmpty())
                return string.Empty;

            if (!Settings.SqlTables.player_spell_crit_chance)
                return string.Empty;

            foreach (var objPair in Storage.Objects)
            {
                if (objPair.Key.GetObjectType() != ObjectType.Player)
                    continue;

                Player player = objPair.Value.Item1 as Player;
                if (player == null)
                    continue;

                Storage.SavePlayerSpellCrit(player, null);
            }

            return SQLUtil.Insert(Storage.PlayerSpellCritChances, false, true);
        }

        [BuilderMethod]
        public static string PlayerDodgeChances()
        {
            if (Storage.PlayerDodgeChances.IsEmpty())
                return string.Empty;

            if (!Settings.SqlTables.player_dodge_chance)
                return string.Empty;

            foreach (var objPair in Storage.Objects)
            {
                if (objPair.Key.GetObjectType() != ObjectType.Player)
                    continue;

                Player player = objPair.Value.Item1 as Player;
                if (player == null)
                    continue;

                Storage.SavePlayerDodge(player, null);
            }

            return SQLUtil.Insert(Storage.PlayerDodgeChances, false, true);
        }
    }
}
