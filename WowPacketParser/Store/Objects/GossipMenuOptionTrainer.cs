using WowPacketParser.SQL;

namespace WowPacketParser.Store.Objects
{
    [DBTableName("gossip_menu_option_trainer")]
    public class GossipMenuOptionTrainer : IDataModel
    {
        [DBFieldName("menu_id", true)]
        public uint? MenuId;

        [DBFieldName("id", true)]
        public uint? OptionIndex;

        [DBFieldName("trainer_id")]
        public uint? TrainerId;
    }
}
