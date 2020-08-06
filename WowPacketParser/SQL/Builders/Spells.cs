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

            string query = "";
            query += "REPLACE INTO `spell_cast_start` (`UnixTime`, `CasterId`, `CasterType`, `SpellId`, `CastFlags`, `CastFlagsEx`, `TargetId`, `TargetType`, `VerifiedBuild`) VALUES";
            uint count = 0;
            foreach (var cast_pair in Storage.SpellCastStart)
            {
                if (count > 0)
                    query += ",";

                // make it clear that its a creature
                if (cast_pair.Item1.CasterType == "Unit")
                    cast_pair.Item1.CasterType = "Creature";
                if (cast_pair.Item1.MainTargetType == "Unit")
                    cast_pair.Item1.MainTargetType = "Creature";
                for (uint i = 0; i < SpellCastData.MAX_SPELL_HIT_TARGETS_DB; i++)
                {
                    if (cast_pair.Item1.HitTargetType[i] == "Unit")
                        cast_pair.Item1.HitTargetType[i] = "Creature";
                }
                if (cast_pair.Item1.MainTargetID == 0 &&
                    cast_pair.Item1.MainTargetType == "Object")
                    cast_pair.Item1.MainTargetType = "";

                query += "\n(" + cast_pair.Item1.UnixTime + ", " + cast_pair.Item1.CasterID + ", '" + cast_pair.Item1.CasterType + "', " + cast_pair.Item1.SpellID + ", " + cast_pair.Item1.CastFlags + ", " + cast_pair.Item1.CastFlagsEx + ", " + cast_pair.Item1.MainTargetID + ", '" + cast_pair.Item1.MainTargetType + "', " + cast_pair.Item1.VerifiedBuild + ")";
                count++;
            }
            query += ";\n";
            return query;
        }

        [BuilderMethod]
        public static string SpellCastGo()
        {
            if (Storage.SpellCastGo.IsEmpty())
                return string.Empty;

            if (!Settings.SqlTables.spell_cast_go)
                return string.Empty;

            string query = "";
            query += "REPLACE INTO `spell_cast_go` (`UnixTime`, `CasterId`, `CasterType`, `SpellId`, `CastFlags`, `CastFlagsEx`, `MainTargetId`, `MainTargetType`, `HitTargetsCount`, `HitTargetId1`, `HitTargetType1`, `HitTargetId2`, `HitTargetType2`, `HitTargetId3`, `HitTargetType3`, `HitTargetId4`, `HitTargetType4`, `HitTargetId5`, `HitTargetType5`, `HitTargetId6`, `HitTargetType6`, `HitTargetId7`, `HitTargetType7`, `HitTargetId8`, `HitTargetType8`, `VerifiedBuild`) VALUES";
            uint count = 0;
            foreach (var cast_pair in Storage.SpellCastGo)
            {
                if (count > 0)
                    query += ",";

                // make it clear that its a creature
                if (cast_pair.Item1.CasterType == "Unit")
                    cast_pair.Item1.CasterType = "Creature";
                if (cast_pair.Item1.MainTargetType == "Unit")
                    cast_pair.Item1.MainTargetType = "Creature";
                for (uint i = 0; i < SpellCastData.MAX_SPELL_HIT_TARGETS_DB; i++)
                {
                    if (cast_pair.Item1.HitTargetType[i] == "Unit")
                        cast_pair.Item1.HitTargetType[i] = "Creature";
                }
                if (cast_pair.Item1.MainTargetID == 0 &&
                    cast_pair.Item1.MainTargetType == "Object")
                    cast_pair.Item1.MainTargetType = "";

                query += "\n(" + cast_pair.Item1.UnixTime + ", " + cast_pair.Item1.CasterID + ", '" + cast_pair.Item1.CasterType + "', " + cast_pair.Item1.SpellID + ", " + cast_pair.Item1.CastFlags + ", " + cast_pair.Item1.CastFlagsEx + ", " + cast_pair.Item1.MainTargetID + ", '" + cast_pair.Item1.MainTargetType + "', " + cast_pair.Item1.HitTargetsCount + ", ";
                for (uint i = 0; i < SpellCastData.MAX_SPELL_HIT_TARGETS_DB; i++)
                {
                    query += cast_pair.Item1.HitTargetID[i] + ", '" + cast_pair.Item1.HitTargetType[i] + "', ";
                }
                query += cast_pair.Item1.VerifiedBuild + ")";
                count++;
            }
            query += ";\n";
            return query;
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
    }
}
