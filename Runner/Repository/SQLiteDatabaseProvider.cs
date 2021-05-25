using System.Data;
using System.Data.SQLite;

namespace Runner
{
    /// <summary>
    /// Author: Mikkel Emil Nielsen-Man
    /// 
    /// This class implements the IDatabaseProvider interface and provides a database connection of type SQLiteConnection.
    /// </summary>
    public class SQLiteDatabaseProvider : IDatabaseProvider
    {
        #region Fields
        private readonly string connectionString;
        #endregion

        #region Constructors
        public SQLiteDatabaseProvider(string connectionString)
        {
            this.connectionString = connectionString;
        }
        #endregion

        #region Public Methods
        public IDbConnection CreateConnection()
        {
            return new SQLiteConnection(connectionString);
        }
        #endregion
    }
}
