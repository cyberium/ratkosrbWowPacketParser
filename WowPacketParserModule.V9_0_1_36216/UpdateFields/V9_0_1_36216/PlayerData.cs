using WowPacketParser.Misc;
using WowPacketParser.Store.Objects.UpdateFields;

namespace WowPacketParserModule.V9_0_1_36216.UpdateFields.V9_0_1_36216
{
    public class PlayerData : IPlayerData
    {
        public WowGuid DuelArbiter { get; set; }
        public WowGuid WowAccount { get; set; }
        public WowGuid LootTargetGUID { get; set; }
        public uint PlayerBytes1 { get; set; }
        public uint PlayerBytes2 { get; set; }
        public uint PlayerFlags { get; set; }
        public uint PlayerFlagsEx { get; set; }
        public uint PvPRank { get; set; }
        public uint Money { get; set; }
        public uint Experience { get; set; }
        public uint GuildRankID { get; set; }
        public uint GuildDeleteDate { get; set; }
        public int GuildLevel { get; set; }
        public byte PartyType { get; set; }
        public byte NativeSex { get; set; }
        public byte Inebriation { get; set; }
        public byte PvpTitle { get; set; }
        public byte ArenaFaction { get; set; }
        public uint DuelTeam { get; set; }
        public int GuildTimeStamp { get; set; }
        public IQuestLog[] QuestLog { get; } = new IQuestLog[125];
        public IVisibleItem[] VisibleItems { get; set; } = new IVisibleItem[19];
        public int PlayerTitle { get; set; }
        public int FakeInebriation { get; set; }
        public uint VirtualPlayerRealm { get; set; }
        public uint CurrentSpecID { get; set; }
        public int TaxiMountAnimKitID { get; set; }
        public float[] AvgItemLevel { get; } = new float[4];
        public byte CurrentBattlePetBreedQuality { get; set; }
        public int HonorLevel { get; set; }
        public int Field_B0 { get; set; }
        public int Field_B4 { get; set; }
        public ICTROptions CtrOptions { get; set; }
        public int CovenantID { get; set; }
        public int SoulbindID { get; set; }
        public DynamicUpdateField<IChrCustomizationChoice> Customizations { get; } = new DynamicUpdateField<IChrCustomizationChoice>();
        public DynamicUpdateField<IQuestLog> QuestSessionQuestLog { get; } = new DynamicUpdateField<IQuestLog>();
        public DynamicUpdateField<IArenaCooldown> ArenaCooldowns { get; } = new DynamicUpdateField<IArenaCooldown>();
        public bool HasQuestSession { get; set; }
        public bool HasLevelLink { get; set; }

        public ChrCustomizationChoice[] GetCustomizations()
        {
            ChrCustomizationChoice[] data = new ChrCustomizationChoice[Customizations.Count];
            for (var i = 0; i < Customizations.Count; ++i)
            {
                if (Customizations.UpdateMask[i])
                {
                    ChrCustomizationChoice custom = Customizations[i] as ChrCustomizationChoice;
                    if (custom != null)
                    {
                        data[i] = custom;
                    }
                    else
                        data[i] = new ChrCustomizationChoice();
                }
                else
                    data[i] = new ChrCustomizationChoice();
            }
            return data;
        }

        public IPlayerData Clone()
        {
            PlayerData copy = (PlayerData)MemberwiseClone();
            copy.VisibleItems = new IVisibleItem[19];
            for (int i = 0; i < 19; i++)
                copy.VisibleItems[i] = VisibleItems[i].Clone();
            return copy;
        }
    }
}

