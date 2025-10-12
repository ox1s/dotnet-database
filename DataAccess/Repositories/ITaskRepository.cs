namespace DataAccess.Repositories;

using Entities;

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