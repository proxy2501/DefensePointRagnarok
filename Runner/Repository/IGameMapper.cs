using System.Collections.Generic;
using System.Data.SQLite;

namespace Runner
{
    /// <summary>
    /// Author: Mikkel Emil Nielsen-Man
    /// 
    /// This interface defines functionality that must be implemented by data mappers for translating table data into game domain classes.
    /// </summary>
    public interface IGameMapper
    {
        List<GameObject> MapGameObjectsFromReader(SQLiteDataReader reader);
    }
}
