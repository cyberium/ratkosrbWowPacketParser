using System.Collections.Generic;
using WowPacketParser.Enums;
using WowPacketParser.Misc;

namespace WowPacketParser.Store.Objects.UpdateFields.LegacyImplementation
{
    class DynamicObjectData : IDynamicObjectData
    {
        private WoWObject Object { get; }
        private Dictionary<int, UpdateField> UpdateFields => Object.UpdateFields;

        public DynamicObjectData(WoWObject obj)
        {
            Object = obj;
        }

        private WowGuid GetGuidValue(DynamicObjectField field)
        {
            if (!ClientVersion.AddedInVersion(ClientType.WarlordsOfDraenor))
            {
                var parts = UpdateFields.GetArray<DynamicObjectField, uint>(field, 2);
                return new WowGuid64(Utilities.MAKE_PAIR64(parts[0], parts[1]));
            }
            else
            {
                var parts = UpdateFields.GetArray<DynamicObjectField, uint>(field, 4);
                return new WowGuid128(Utilities.MAKE_PAIR64(parts[0], parts[1]), Utilities.MAKE_PAIR64(parts[2], parts[3]));
            }
        }

        public WowGuid Caster => GetGuidValue(DynamicObjectField.DYNAMICOBJECT_CASTER);

        public int SpellXSpellVisualID => UpdateFields.GetValue<DynamicObjectField, int>(DynamicObjectField.DYNAMICOBJECT_SPELL_X_SPELL_VISUAL_ID);

        public int SpellID => UpdateFields.GetValue<DynamicObjectField, int>(DynamicObjectField.DYNAMICOBJECT_SPELLID);

        public float Radius => UpdateFields.GetValue<DynamicObjectField, float>(DynamicObjectField.DYNAMICOBJECT_RADIUS);

        public uint CastTime => UpdateFields.GetValue<DynamicObjectField, uint>(DynamicObjectField.DYNAMICOBJECT_CASTTIME);

        public byte Type => (byte)(ClientVersion.AddedInVersion(ClientVersionBuild.V8_0_1_27101)
            ? UpdateFields.GetValue<DynamicObjectField, uint>(DynamicObjectField.DYNAMICOBJECT_TYPE)
            : UpdateFields.GetValue<DynamicObjectField, uint>(DynamicObjectField.DYNAMICOBJECT_BYTES));

    }
    class OriginalDynamicObjectData : IDynamicObjectData
    {
        private WoWObject Object { get; }
        private Dictionary<int, UpdateField> UpdateFields => Object.OriginalUpdateFields;

        public OriginalDynamicObjectData(WoWObject obj)
        {
            Object = obj;
        }

        private WowGuid GetGuidValue(DynamicObjectField field)
        {
            if (!ClientVersion.AddedInVersion(ClientType.WarlordsOfDraenor))
            {
                var parts = UpdateFields.GetArray<DynamicObjectField, uint>(field, 2);
                return new WowGuid64(Utilities.MAKE_PAIR64(parts[0], parts[1]));
            }
            else
            {
                var parts = UpdateFields.GetArray<DynamicObjectField, uint>(field, 4);
                return new WowGuid128(Utilities.MAKE_PAIR64(parts[0], parts[1]), Utilities.MAKE_PAIR64(parts[2], parts[3]));
            }
        }

        public WowGuid Caster => GetGuidValue(DynamicObjectField.DYNAMICOBJECT_CASTER);

        public int SpellXSpellVisualID => UpdateFields.GetValue<DynamicObjectField, int>(DynamicObjectField.DYNAMICOBJECT_SPELL_X_SPELL_VISUAL_ID);

        public int SpellID => UpdateFields.GetValue<DynamicObjectField, int>(DynamicObjectField.DYNAMICOBJECT_SPELLID);

        public float Radius => UpdateFields.GetValue<DynamicObjectField, float>(DynamicObjectField.DYNAMICOBJECT_RADIUS);

        public uint CastTime => UpdateFields.GetValue<DynamicObjectField, uint>(DynamicObjectField.DYNAMICOBJECT_CASTTIME);

        public byte Type => (byte)(ClientVersion.AddedInVersion(ClientVersionBuild.V8_0_1_27101)
            ? UpdateFields.GetValue<DynamicObjectField, uint>(DynamicObjectField.DYNAMICOBJECT_TYPE)
            : UpdateFields.GetValue<DynamicObjectField, uint>(DynamicObjectField.DYNAMICOBJECT_BYTES));

    }
}
