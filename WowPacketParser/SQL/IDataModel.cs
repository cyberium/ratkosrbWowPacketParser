using System.Collections.Generic;

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
    }
}
