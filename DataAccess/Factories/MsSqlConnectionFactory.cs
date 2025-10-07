using System.Data;
using Microsoft.Data.SqlClient;

namespace DataAccess.Factories
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();

    }
    public class MsSqlConnectionFactory : IDbConnectionFactory
    {
        private readonly string _connectionString;

        public MsSqlConnectionFactory(string connectionString)
        {
            _connectionString = connectionString
                ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public IDbConnection CreateConnection() =>
            new SqlConnection(_connectionString);
    }
}