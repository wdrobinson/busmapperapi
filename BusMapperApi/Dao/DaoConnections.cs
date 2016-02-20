using System.Configuration;
using System.Data.SqlClient;

namespace BusMapperApi.Dao
{
    public class DaoConnections
    {
        public static SqlConnection GetDatabaseConnection()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["BusDatabase"].ConnectionString;
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            return conn;
        }
    }
}