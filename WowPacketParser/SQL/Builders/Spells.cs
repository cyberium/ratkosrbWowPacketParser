using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Store;
using WowPacketParser.Store.Objects;

namespace WowPacketParser.SQL.Builders
{
    [BuilderClass]
    public static class Spells
    {
        [BuilderMethod]
        public static string SpellTargetPosition()
        {
            if (Storage.SpellTargetPositions.IsEmpty())
                return string.Empty;

            if (!Settings.SqlTables.spell_target_position)
                return string.Empty;

            return SQLUtil.Compare(Storage.SpellTargetPositions, SQLDatabase.Get(Storage.SpellTargetPositions), t => t.EffectHelper);
        }

        [BuilderMethod]
        public static string SpellCastStart()
        {
            if (Storage.SpellCastStart.IsEmpty())
                return string.Empty;

            if (!Settings.SqlTables.spell_cast_start)
                return string.Empty;

            var spellRows = new RowList<SpellCastStart>();
            foreach (var cast_pair in Storage.SpellCastStart)
            {
                Row<SpellCastStart> row = new Row<SpellCastStart>();

                row.Data.UnixTime = cast_pair.Item1.UnixTime;
                row.Data.SpellId = cast_pair.Item1.SpellID;
                row.Data.CastFlags = cast_pair.Item1.CastFlags;
                row.Data.CastFlagsEx = cast_pair.Item1.CastFlagsEx;

                Storage.GetObjectDbGuidEntryType(cast_pair.Item1.CasterGuid, out row.Data.CasterGuid, out row.Data.CasterId, out row.Data.CasterType);
                Storage.GetObjectDbGuidEntryType(cast_pair.Item1.MainTargetGuid, out row.Data.TargetGuid, out row.Data.TargetId, out row.Data.TargetType);

                spellRows.Add(row);
            }
            var spellsSql = new SQLInsert<SpellCastStart>(spellRows, false);
            return spellsSql.Build();
        }

        [BuilderMethod]
        public static string SpellCastGo()
        {
            if (Storage.SpellCastGo.IsEmpty())
                return string.Empty;

            if (!Settings.SqlTables.spell_cast_go)
                return string.Empty;

            var spellRows = new RowList<SpellCastGo>();
            foreach (var cast_pair in Storage.SpellCastGo)
            {
                Row<SpellCastGo> row = new Row<SpellCastGo>();

                row.Data.UnixTime = cast_pair.Item1.UnixTime;
                row.Data.SpellId = cast_pair.Item1.SpellID;
                row.Data.CastFlags = cast_pair.Item1.CastFlags;
                row.Data.CastFlagsEx = cast_pair.Item1.CastFlagsEx;
                row.Data.HitTargetsCount = cast_pair.Item1.HitTargetsCount;

                Storage.GetObjectDbGuidEntryType(cast_pair.Item1.CasterGuid, out row.Data.CasterGuid, out row.Data.CasterId, out row.Data.CasterType);
                Storage.GetObjectDbGuidEntryType(cast_pair.Item1.MainTargetGuid, out row.Data.MainTargetGuid, out row.Data.MainTargetId, out row.Data.MainTargetType);

                for (uint i = 0; i < SpellCastData.MAX_SPELL_HIT_TARGETS_DB; i++)
                {
                    if (cast_pair.Item1.HitTargetType[i] == "Unit")
                        cast_pair.Item1.HitTargetType[i] = "Creature";
                    row.Data.HitTargetId[i] = cast_pair.Item1.HitTargetID[i];
                    row.Data.HitTargetType[i] = cast_pair.Item1.HitTargetType[i];
                }
                
                spellRows.Add(row);
            }
            var spellsSql = new SQLInsert<SpellCastGo>(spellRows, false);
            return spellsSql.Build();
        }

        [BuilderMethod(true)]
        public static string SpellPetCooldown()
        {
            if (!Settings.SqlTables.spell_pet_cooldown)
                return string.Empty;

            if (Storage.SpellPetCooldown.IsEmpty())
                return string.Empty;

            var templatesDb = SQLDatabase.Get(Storage.SpellPetCooldown);

            return SQLUtil.Compare(Storage.SpellPetCooldown, templatesDb, StoreNameType.None);
        }

        [BuilderMethod(true)]
        public static string SpellPetActions()
        {
            if (!Settings.SqlTables.spell_pet_action)
                return string.Empty;

            if (Storage.SpellPetActions.IsEmpty())
                return string.Empty;

            var templatesDb = SQLDatabase.Get(Storage.SpellPetActions);

            return SQLUtil.Compare(Storage.SpellPetActions, templatesDb, StoreNameType.None);
        }

        [BuilderMethod]
        public static string PlaySpellVisualKit()
        {
            if (Storage.SpellPlayVisualKit.IsEmpty())
                return string.Empty;

            if (!Settings.SqlTables.play_spell_visual_kit)
                return string.Empty;

            var spellRows = new RowList<PlaySpellVisualKit>();
            foreach (var spellVisual in Storage.SpellPlayVisualKit)
            {
                if (spellVisual.Item1.Guid.GetObjectType() == ObjectType.Player && !Settings.SavePlayerCasts)
                    continue;

                Row<PlaySpellVisualKit> row = new Row<PlaySpellVisualKit>();
                row.Data = spellVisual.Item1;
                Storage.GetObjectDbGuidEntryType(spellVisual.Item1.Guid, out row.Data.CasterGuid, out row.Data.CasterId, out row.Data.CasterType);
                row.Data.UnixTime = (uint)Utilities.GetUnixTimeFromDateTime(spellVisual.Item1.Time);
                spellRows.Add(row);
            }
            var spellsSql = new SQLInsert<PlaySpellVisualKit>(spellRows, false);
            return spellsSql.Build();
        }

        [BuilderMethod]
        public static string SpellCastFailed()
        {
            if (Storage.SpellCastFailed.IsEmpty())
                return string.Empty;

            if (!Settings.SqlTables.spell_cast_failed)
                return string.Empty;

            var spellRows = new RowList<SpellCastFailed>();
            foreach (var failedCast in Storage.SpellCastFailed)
            {
                if (failedCast.Item1.Guid.GetObjectType() == ObjectType.Player && !Settings.SavePlayerCasts)
                    continue;

                Row<SpellCastFailed> row = new Row<SpellCastFailed>();
                row.Data = failedCast.Item1;
                Storage.GetObjectDbGuidEntryType(failedCast.Item1.Guid, out row.Data.CasterGuid, out row.Data.CasterId, out row.Data.CasterType);
                row.Data.UnixTime = (uint)Utilities.GetUnixTimeFromDateTime(failedCast.Item1.Time);
                spellRows.Add(row);
            }
            var spellsSql = new SQLInsert<SpellCastFailed>(spellRows, false);
            return spellsSql.Build();
        }
    }
}
