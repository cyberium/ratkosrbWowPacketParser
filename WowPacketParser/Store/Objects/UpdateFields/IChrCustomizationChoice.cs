namespace WowPacketParser.Store.Objects.UpdateFields
{
    public interface IChrCustomizationChoice
    {
    }

    public class ChrCustomizationChoice : IChrCustomizationChoice
    {
        public uint ChrCustomizationOptionID { get; set; }
        public uint ChrCustomizationChoiceID { get; set; }
    }
}
