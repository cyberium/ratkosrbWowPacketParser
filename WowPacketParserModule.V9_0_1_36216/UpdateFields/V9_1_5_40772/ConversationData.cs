using WowPacketParser.Misc;
using WowPacketParser.Store.Objects.UpdateFields;

// This file is automatically generated, DO NOT EDIT

namespace WowPacketParserModule.V9_0_1_36216.UpdateFields.V9_1_5_40772
{
    public class ConversationData : IConversationData
    {
        public int LastLineEndTime { get; set; }
        public uint Progress { get; set; }
        public IConversationLine[] Lines { get; set; }
        public uint Flags { get; set; }
        public bool DontPlayBroadcastTextSounds { get; set; }
        public DynamicUpdateField<IConversationActor> Actors { get; } = new DynamicUpdateField<IConversationActor>();
    }
}

