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
        void CreateTask(AppTask task);
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


        public void CreateTask(AppTask task)
        {
            using (IDbConnection dbConnection = _connectionFactory.CreateConnection())
            {
                dbConnection.Execute(
                    """
                    INSERT INTO Tasks (Title, Description,IsCompleted, CreatedAt) 
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
