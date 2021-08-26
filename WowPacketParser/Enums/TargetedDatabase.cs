
using System;

namespace WowPacketParser.Enums
{
    public enum TargetedDbExpansion
    {
        Zero                = 0,
        TheBurningCrusade   = 1,
        WrathOfTheLichKing  = 2,
        Cataclysm           = 3,
        WarlordsOfDraenor   = 4,
        Legion              = 5,
        BattleForAzeroth    = 6,
        Shadowlands         = 7,

        Classic             = -1,
        BurningCrusadeClassic = -2,
    }
    [Flags]
    public enum TargetedDbType
    {
        WPP     = 0x00000001,
        TRINITY = 0x00000002,
        VMANGOS = 0x00000004,
        CMANGOS = 0x00000008,
    }
}
