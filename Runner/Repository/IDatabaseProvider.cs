using System.Data;

namespace Runner
{
    /// <summary>
    /// Author: Mikkel Emil Nielsen-Man
    /// 
    /// This interface provides a general template for classes that provide database connections.
    /// </summary>
    public interface IDatabaseProvider
    {
        IDbConnection CreateConnection();
    }
}
