using System;
using System.Diagnostics.CodeAnalysis;

namespace WowPacketParser.Enums
{
    [Flags]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    enum SpellHitType
    {
        CritDebug        = 0x01,
        Crit             = 0x02,
        HitDebug         = 0x04,
        Split            = 0x08,
        VictimIsAttacker = 0x10,
        AttackTableDebug = 0x20,
        Unk              = 0x40,
        NoAttacker       = 0x80, // does the same as SPELL_ATTR4_COMBAT_LOG_NO_CASTER
    };
}
