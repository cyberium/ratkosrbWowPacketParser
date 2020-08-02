using WowPacketParser.SQL;

namespace WowPacketParser.Store.Objects
{
    [DBTableName("creature_trainer")]
    public class CreatureTrainer : IDataModel
    {
        [DBFieldName("creature_id", true)]
        public uint? CreatureId;

        [DBFieldName("trainer_id")]
        public uint? TrainerId;

        [DBFieldName("menu_id", true)]
        public uint? MenuID;

        [DBFieldName("id", true)]
        public uint? OptionIndex;
    }
}
