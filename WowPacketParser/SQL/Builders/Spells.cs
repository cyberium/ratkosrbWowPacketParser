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
            query += "REPLACE INTO `spell_cast_start` (`UnixTime`, `CasterGuid`, `CasterId`, `CasterType`, `SpellId`, `CastFlags`, `CastFlagsEx`, `TargetGuid`, `TargetId`, `TargetType`, `VerifiedBuild`) VALUES";
            uint count = 0;
            foreach (var cast_pair in Storage.SpellCastStart)
            {
                if (count > 0)
                    query += ",";

                uint casterId = cast_pair.Item1.CasterGuid.GetEntry();
                string casterType = cast_pair.Item1.CasterGuid.GetObjectType().ToString();

                uint targetId = cast_pair.Item1.MainTargetGuid.GetEntry();
                string targetType = cast_pair.Item1.MainTargetGuid.GetObjectType().ToString();

                // make it clear that its a pet
                if (cast_pair.Item1.CasterGuid.GetObjectType() == ObjectType.Unit &&
                    cast_pair.Item1.CasterGuid.GetHighType() == HighGuidType.Pet)
                    casterType = "Pet";
                if (cast_pair.Item1.MainTargetGuid.GetObjectType() == ObjectType.Unit &&
                    cast_pair.Item1.MainTargetGuid.GetHighType() == HighGuidType.Pet)
                    casterType = "Pet";
                // make it clear that its a creature
                if (casterType == "Unit")
                    casterType = "Creature";
                if (targetType == "Unit")
                    targetType = "Creature";
                // hide real player guids
                if (targetType == "Player")
                    targetId = 0;

                uint casterGuid = 0;
                string casterGuidType = "";
                if (!cast_pair.Item1.CasterGuid.IsEmpty())
                {
                    if (Storage.Objects.ContainsKey(cast_pair.Item1.CasterGuid))
                    {
                        if (cast_pair.Item1.CasterGuid.GetObjectType() == ObjectType.Unit)
                        {
                            casterGuid = (Storage.Objects[cast_pair.Item1.CasterGuid].Item1 as Unit).DbGuid;
                            casterGuidType = "@CGUID+";
                        }
                        else if (cast_pair.Item1.CasterGuid.GetObjectType() == ObjectType.GameObject)
                        {
                            casterGuid = (Storage.Objects[cast_pair.Item1.CasterGuid].Item1 as GameObject).DbGuid;
                            casterGuidType = "@OGUID+";
                        }
                    }
                }


                uint targetGuid = 0;
                string targetGuidType = "";
                if (!cast_pair.Item1.MainTargetGuid.IsEmpty())
                {
                    if (Storage.Objects.ContainsKey(cast_pair.Item1.MainTargetGuid))
                    {
                        if (cast_pair.Item1.MainTargetGuid.GetObjectType() == ObjectType.Unit)
                        {
                            targetGuid = (Storage.Objects[cast_pair.Item1.MainTargetGuid].Item1 as Unit).DbGuid;
                            targetGuidType = "@CGUID+";
                        }
                        else if (cast_pair.Item1.MainTargetGuid.GetObjectType() == ObjectType.GameObject)
                        {
                            targetGuid = (Storage.Objects[cast_pair.Item1.MainTargetGuid].Item1 as GameObject).DbGuid;
                            targetGuidType = "@OGUID+";
                        }
                    }
                }

                for (uint i = 0; i < SpellCastData.MAX_SPELL_HIT_TARGETS_DB; i++)
                {
                    if (cast_pair.Item1.HitTargetType[i] == "Unit")
                        cast_pair.Item1.HitTargetType[i] = "Creature";
                }
                if (targetId == 0 &&
                    targetType == "Object")
                    targetType = "";

                query += "\n(" + cast_pair.Item1.UnixTime + ", " + casterGuidType + casterGuid + ", " + casterId  + ", '" + casterType + "', " + cast_pair.Item1.SpellID + ", " + cast_pair.Item1.CastFlags + ", " + cast_pair.Item1.CastFlagsEx + ", " + targetGuidType + targetGuid + ", " + targetId + ", '" + targetType + "', " + cast_pair.Item1.VerifiedBuild + ")";
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
            query += "REPLACE INTO `spell_cast_go` (`UnixTime`, `CasterGuid`, `CasterId`, `CasterType`, `SpellId`, `CastFlags`, `CastFlagsEx`, `MainTargetGuid`, `MainTargetId`, `MainTargetType`, `HitTargetsCount`, `HitTargetId1`, `HitTargetType1`, `HitTargetId2`, `HitTargetType2`, `HitTargetId3`, `HitTargetType3`, `HitTargetId4`, `HitTargetType4`, `HitTargetId5`, `HitTargetType5`, `HitTargetId6`, `HitTargetType6`, `HitTargetId7`, `HitTargetType7`, `HitTargetId8`, `HitTargetType8`, `VerifiedBuild`) VALUES";
            uint count = 0;
            foreach (var cast_pair in Storage.SpellCastGo)
            {
                if (count > 0)
                    query += ",";

                uint casterId = cast_pair.Item1.CasterGuid.GetEntry();
                string casterType = cast_pair.Item1.CasterGuid.GetObjectType().ToString();

                uint targetId = cast_pair.Item1.MainTargetGuid.GetEntry();
                string targetType = cast_pair.Item1.MainTargetGuid.GetObjectType().ToString();

                // make it clear that its a pet
                if (cast_pair.Item1.CasterGuid.GetObjectType() == ObjectType.Unit &&
                    cast_pair.Item1.CasterGuid.GetHighType() == HighGuidType.Pet)
                    casterType = "Pet";
                if (cast_pair.Item1.MainTargetGuid.GetObjectType() == ObjectType.Unit &&
                    cast_pair.Item1.MainTargetGuid.GetHighType() == HighGuidType.Pet)
                    casterType = "Pet";
                // make it clear that its a creature
                if (casterType == "Unit")
                    casterType = "Creature";
                if (targetType == "Unit")
                    targetType = "Creature";
                // hide real player guids
                if (targetType == "Player")
                    targetId = 0;

                uint casterGuid = 0;
                string casterGuidType = "";
                if (!cast_pair.Item1.CasterGuid.IsEmpty())
                {
                    if (Storage.Objects.ContainsKey(cast_pair.Item1.CasterGuid))
                    {
                        if (cast_pair.Item1.CasterGuid.GetObjectType() == ObjectType.Unit)
                        {
                            casterGuid = (Storage.Objects[cast_pair.Item1.CasterGuid].Item1 as Unit).DbGuid;
                            casterGuidType = "@CGUID+";
                        }
                        else if (cast_pair.Item1.CasterGuid.GetObjectType() == ObjectType.GameObject)
                        {
                            casterGuid = (Storage.Objects[cast_pair.Item1.CasterGuid].Item1 as GameObject).DbGuid;
                            casterGuidType = "@OGUID+";
                        }
                    }
                }


                uint targetGuid = 0;
                string targetGuidType = "";
                if (!cast_pair.Item1.MainTargetGuid.IsEmpty())
                {
                    if (Storage.Objects.ContainsKey(cast_pair.Item1.MainTargetGuid))
                    {
                        if (cast_pair.Item1.MainTargetGuid.GetObjectType() == ObjectType.Unit)
                        {
                            targetGuid = (Storage.Objects[cast_pair.Item1.MainTargetGuid].Item1 as Unit).DbGuid;
                            targetGuidType = "@CGUID+";
                        }
                        else if (cast_pair.Item1.MainTargetGuid.GetObjectType() == ObjectType.GameObject)
                        {
                            targetGuid = (Storage.Objects[cast_pair.Item1.MainTargetGuid].Item1 as GameObject).DbGuid;
                            targetGuidType = "@OGUID+";
                        }
                    }
                }

                for (uint i = 0; i < SpellCastData.MAX_SPELL_HIT_TARGETS_DB; i++)
                {
                    if (cast_pair.Item1.HitTargetType[i] == "Unit")
                        cast_pair.Item1.HitTargetType[i] = "Creature";
                }
                if (targetId == 0 &&
                    targetType == "Object")
                    targetType = "";

                query += "\n(" + cast_pair.Item1.UnixTime + ", " + casterGuidType + casterGuid + ", " + casterId + ", '" + casterType + "', " + cast_pair.Item1.SpellID + ", " + cast_pair.Item1.CastFlags + ", " + cast_pair.Item1.CastFlagsEx + ", " + targetGuidType + targetGuid + ", " + targetId + ", '" + targetType + "', " + cast_pair.Item1.HitTargetsCount + ", ";
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
