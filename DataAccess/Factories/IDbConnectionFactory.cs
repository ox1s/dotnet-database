namespace DataAccess.Factories;

using System.Data;

public interface IDbConnectionFactory
{
    IDbConnection CreateConnection();

}
