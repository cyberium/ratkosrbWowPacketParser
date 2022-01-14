namespace WowPacketParser.Store.Objects.UpdateFields
{
    public interface ISkillInfo
    {
        ushort[] SkillLineID { get; }
        ushort[] SkillRank { get; }
        ushort[] SkillMaxRank { get; }
    }
}
