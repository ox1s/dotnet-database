namespace Services;

using Dapper;
using DataAccess.Factories;
using Microsoft.Data.SqlClient;
using System.Data;

public class DatabaseSetup
{
    private readonly IDbConnectionFactory _connectionFactory;

    public DatabaseSetup(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public void Setup()
    {
        using (var connection = _connectionFactory.CreateConnection())
        {
            var connectionStringBuilder = new SqlConnectionStringBuilder(connection.ConnectionString);
            string dbName = connectionStringBuilder.InitialCatalog;

            connectionStringBuilder.InitialCatalog = "master";
            using (var masterConnection = new SqlConnection(connectionStringBuilder.ConnectionString))
            {
                EnsureDatabaseCreated(masterConnection, dbName);
            }
        }

        using (var connection = _connectionFactory.CreateConnection())
        {
            EnsureTableCreated(connection);
        }
    }

    private void EnsureDatabaseCreated(IDbConnection masterConnection, string dbName)
    {
        var databases = masterConnection.Query<string>(
            """
                SELECT name
                FROM master.sys.databases
                WHERE name = @name;
            """
            , new { name = dbName });

        if (!databases.Any())
        {
            Console.WriteLine($"База данных '{dbName}' не найдена. Создаю...");
            masterConnection.Execute($"CREATE DATABASE [{dbName}]"); 
            Console.WriteLine(ConsoleStyler.Green($"База данных '{dbName}' успешно создана."));
        }
    }

    private void EnsureTableCreated(IDbConnection connection)
    {
        var query = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Tasks'";
        var tables = connection.Query<string>(query);

        if (!tables.Any())
        {
            Console.WriteLine("Таблица 'Tasks' не найдена. Создаю...");
            connection.Execute(
                """
                    CREATE TABLE Tasks (
                        Id INT PRIMARY KEY IDENTITY(1,1),
                        Title NVARCHAR(255) NOT NULL,
                        Description NVARCHAR(MAX),
                        IsCompleted BIT NOT NULL DEFAULT 0,
                        CreatedAt DATETIME NOT NULL
                    ); 
                """ 
            );
            Console.WriteLine(ConsoleStyler.Green("Таблица 'Tasks' успешно создана."));
        }
    }
}