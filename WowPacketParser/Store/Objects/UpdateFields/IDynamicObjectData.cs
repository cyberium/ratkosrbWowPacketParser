using WowPacketParser.Misc;

namespace WowPacketParser.Store.Objects.UpdateFields
{
    public interface IDynamicObjectData
    {
        WowGuid Caster { get; }
        int SpellXSpellVisualID { get; }
        int SpellID { get; }
        float Radius { get; }
        uint CastTime { get; }
        byte Type { get; }
    }
}
