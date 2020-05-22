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

            if (!Settings.SQLOutputFlag.HasAnyFlagBit(SQLOutput.spell_target_position))
                return string.Empty;

            return SQLUtil.Compare(Storage.SpellTargetPositions, SQLDatabase.Get(Storage.SpellTargetPositions), t => t.EffectHelper);
        }

        [BuilderMethod]
        public static string SpellCastStart()
        {
            if (Storage.SpellCastStart.IsEmpty())
                return string.Empty;

            if (!Settings.SQLOutputFlag.HasAnyFlagBit(SQLOutput.spell_cast_start))
                return string.Empty;

            string query = "";
            query += "REPLACE INTO `spell_cast_start` (`casterId`, `casterType`, `spellId`, `castFlags`, `castFlagsEx`, `targetId`, `targetType`, `verifiedBuild`) VALUES";
            uint count = 0;
            foreach (var cast_pair in Storage.SpellCastStart)
            {
                if (count > 0)
                    query += ",";

                // do not export player guid to db
                uint targetId = cast_pair.Item1.TargetID;
                if (cast_pair.Item1.TargetType.Contains("Player"))
                    targetId = 0;

                // make it clear that its a creature
                if (cast_pair.Item1.CasterType == "Unit")
                    cast_pair.Item1.CasterType = "Creature";
                if (cast_pair.Item1.TargetType == "Unit")
                    cast_pair.Item1.TargetType = "Creature";

                query += "\n(" + cast_pair.Item1.CasterID + ", '" + MySql.Data.MySqlClient.MySqlHelper.EscapeString(cast_pair.Item1.CasterType.Replace("/0", "")) + "', " + cast_pair.Item1.SpellID + ", " + cast_pair.Item1.CastFlags + ", " + cast_pair.Item1.CastFlagsEx + ", " + targetId + ", '" + MySql.Data.MySqlClient.MySqlHelper.EscapeString(cast_pair.Item1.TargetType.Replace("/0", "")) + "', " + cast_pair.Item1.VerifiedBuild + ")";
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

            if (!Settings.SQLOutputFlag.HasAnyFlagBit(SQLOutput.spell_cast_go))
                return string.Empty;

            string query = "";
            query += "REPLACE INTO `spell_cast_go` (`casterId`, `casterType`, `spellId`, `castFlags`, `castFlagsEx`, `targetId`, `targetType`, `verifiedBuild`) VALUES";
            uint count = 0;
            foreach (var cast_pair in Storage.SpellCastGo)
            {
                if (count > 0)
                    query += ",";

                // do not export player guid to db
                uint targetId = cast_pair.Item1.TargetID;
                if (cast_pair.Item1.TargetType.Contains("Player"))
                    targetId = 0;

                // make it clear that its a creature
                if (cast_pair.Item1.CasterType == "Unit")
                    cast_pair.Item1.CasterType = "Creature";
                if (cast_pair.Item1.TargetType == "Unit")
                    cast_pair.Item1.TargetType = "Creature";

                query += "\n(" + cast_pair.Item1.CasterID + ", '" + MySql.Data.MySqlClient.MySqlHelper.EscapeString(cast_pair.Item1.CasterType.Replace("/0", "")) + "', " + cast_pair.Item1.SpellID + ", " + cast_pair.Item1.CastFlags + ", " + cast_pair.Item1.CastFlagsEx + ", " + targetId + ", '" + MySql.Data.MySqlClient.MySqlHelper.EscapeString(cast_pair.Item1.TargetType.Replace("/0", "")) + "', " + cast_pair.Item1.VerifiedBuild + ")";
                count++;
            }
            query += ";\n";
            return query;
        }
    }
}
