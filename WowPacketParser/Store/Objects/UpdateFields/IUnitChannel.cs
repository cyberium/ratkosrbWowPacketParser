namespace WowPacketParser.Store.Objects.UpdateFields
{
    public interface IUnitChannel
    {
        int SpellID { get; }
        ISpellCastVisual SpellVisual { get; }
    }
}
