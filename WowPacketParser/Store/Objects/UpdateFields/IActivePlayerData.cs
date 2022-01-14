namespace WowPacketParser.Store.Objects.UpdateFields
{
    public interface IActivePlayerData
    {
        ulong Coinage { get; }
        int XP { get; }
        ISkillInfo Skill { get; }
        float DodgePercentage { get; }
        float CritPercentage { get; }
    }
}
