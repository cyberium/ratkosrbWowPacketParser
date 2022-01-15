using System.Collections.Generic;

namespace WowPacketParser.SQL
{
    /// <summary>
    /// Interface which has to be implemented by every data model.
    /// </summary>
    public interface IDataModel
    {
    }

    public interface ITableWithSniffIdList : IDataModel
    {
        HashSet<int> GetSniffIdList { get; }
    }
}
