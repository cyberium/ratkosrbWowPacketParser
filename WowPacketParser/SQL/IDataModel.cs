using System.Collections.Generic;
using WowPacketParser.Enums;

namespace WowPacketParser.SQL
{
    /// <summary>
    /// Interface which has to be implemented by every data model.
    /// </summary>
    public interface IDataModel
    {
    }

    public abstract class ITableWithSniffIdList : IDataModel
    {
        public int SniffId = 0;
        public SortedSet<int> SniffIdList = null;

        [DBFieldName("sniff_id_list", DbType = (TargetedDbType.WPP))]
        public readonly string SniffIdListPH = ""; // Placeholder to include an empty string in insert
    }
}
