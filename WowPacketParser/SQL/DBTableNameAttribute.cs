using System;
using WowPacketParser.Enums;
using WowPacketParser.Misc;

namespace WowPacketParser.SQL
{
    /// <summary>
    /// Table name in database
    /// Only usuable with structs or classes
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true)]
    public sealed class DBTableNameAttribute : Attribute
    {
        /// <summary>
        /// Table name
        /// </summary>
        public readonly string Name;

        /// <summary>
        /// Database type
        /// </summary>
        public TargetedDbType DbType = TargetedDbType.WPP | TargetedDbType.TRINITY | TargetedDbType.VMANGOS | TargetedDbType.CMANGOS;

        /// <summary>
        /// </summary>
        /// <param name="name">table name</param>
        public DBTableNameAttribute(string name, TargetedDbType dbType = (TargetedDbType.WPP | TargetedDbType.TRINITY | TargetedDbType.VMANGOS | TargetedDbType.CMANGOS))
        {
            Name = name;
            DbType = dbType;
        }

        public bool IsNameAppropriateForDatabaseType()
        {
            return ((Settings.TargetedDbType & DbType) != 0);
        }
    }
}