using Dapper;
using System.Data;
using DataAccess.Factories;
using DataAccess.Entities;

namespace DataAccess.Repositories
{
    public interface ITaskRepository
    {
        IEnumerable<AppTask> GetAllTasks();
        IEnumerable<AppTask> GetAllCompletedTasks();
        IEnumerable<AppTask> GetAllNotCompletedTasks();
        IEnumerable<string> GetTitlesLike(string pattern);
        AppTask? GetTaskInfo(int id);
        bool TaskExists(int id);
        AppTask CreateTask(AppTask task);
        void MarkAsCompleted(int id);
        void DeleteTask(int id);

    }
    public class TaskRepository : ITaskRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public TaskRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public IEnumerable<AppTask> GetAllTasks()
        {
            using (IDbConnection dbConnection = _connectionFactory.CreateConnection())
            {
                return dbConnection.Query<AppTask>(
                    """
                    SELECT * FROM Tasks
                    """
                    );
            }

        }
        public IEnumerable<AppTask> GetAllCompletedTasks()
        {
            using (IDbConnection dbConnection = _connectionFactory.CreateConnection())
            {
                return dbConnection.Query<AppTask>(
                    """
                    SELECT * FROM Tasks
                    WHERE IsCompleted = 1;
                    """
                    );
            }

        }
        public IEnumerable<AppTask> GetAllNotCompletedTasks()
        {
            using (IDbConnection dbConnection = _connectionFactory.CreateConnection())
            {
                return dbConnection.Query<AppTask>(
                    """
                    SELECT * FROM Tasks
                    WHERE IsCompleted = 0;
                    """
                    );
            }

        }
        public AppTask? GetTaskInfo(int id)
        {
            using (IDbConnection dbConnection = _connectionFactory.CreateConnection())
            {
                return dbConnection.QueryFirstOrDefault<AppTask>(
                    """
                    SELECT * FROM Tasks
                    WHERE id = @Id;
                    """
                    , new { Id = id });
            }

        }


        public IEnumerable<string> GetTitlesLike(string pattern)
        {
            using (IDbConnection dbConnection = _connectionFactory.CreateConnection())
            {
                return dbConnection.Query<string>(
                    """
                    SELECT Title FROM Tasks WHERE Title LIKE @SearchPattern
                    """
                    , new { SearchPattern = pattern + "%" });
            }
        }
        public bool TaskExists(int id)
        {
            using (IDbConnection dbConnection = _connectionFactory.CreateConnection())
            {
                return dbConnection.ExecuteScalar<bool>(
                    """
                    SELECT Count(1) FROM Tasks WHERE id = @Id
                    """
                    , new { Id = id });
            }
        }

        public AppTask CreateTask(AppTask task)
        {
            using (IDbConnection dbConnection = _connectionFactory.CreateConnection())
            {
                return dbConnection.QuerySingle<AppTask>(
                    """
                    INSERT INTO Tasks (Title, Description, IsCompleted, CreatedAt) 
                    OUTPUT INSERTED.*
                    VALUES(@Title, @Description, @IsCompleted, @CreatedAt)
                    """
                    , task);
            }
        }
        public void MarkAsCompleted(int id)
        {
            using (IDbConnection dbConnection = _connectionFactory.CreateConnection())
            {
                dbConnection.Execute(
                    """
                    UPDATE Tasks SET IsCompleted = 1 
                    WHERE Id = @Id
                    """,
                    new { Id = id });
            }
        }

        public void DeleteTask(int id)
        {
            using (IDbConnection dbConnection = _connectionFactory.CreateConnection())
            {
                dbConnection.Execute(
                    """
                    DELETE FROM Tasks WHERE Id = @id
                    """
                    , new { Id = id });
            }
        }
    }
}
