using DataAccess.Repositories;
using DataAccess.Entities;

namespace Services
{
    public class TaskService
    {
        private readonly ITaskRepository _taskRepository;

        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public IEnumerable<AppTask> GetAllTasks() => _taskRepository.GetAllTasks();
        public IEnumerable<AppTask> GetAllCompletedTasks() => _taskRepository.GetAllCompletedTasks();
        public IEnumerable<AppTask> GetAllNotCompletedTasks() => _taskRepository.GetAllNotCompletedTasks();
        public bool TaskExists(int id) => _taskRepository.TaskExists(id);
        public AppTask? GetTaskInfo(int id) =>
            _taskRepository.TaskExists(id)
            ? _taskRepository.GetTaskInfo(id)
            : null;

        public void CreateTask(AppTask task)
        {
            if (string.IsNullOrWhiteSpace(task.Title))
            {
                const string baseTitle = "Без имени";
                var existingTitles = new HashSet<string>(_taskRepository.GetTitlesLike(baseTitle));
                task.Title = GenerateUniqueTitle(existingTitles, baseTitle);
            }
            _taskRepository.CreateTask(task);
        }
        private string GenerateUniqueTitle(HashSet<string> existingTitles, string baseTitle)
        {
            if (!existingTitles.Contains(baseTitle))
            {
                return baseTitle;
            }
            int counter = 1;
            while (true)
            {
                string numberedTitle = $"{baseTitle} ({counter})";
                if (!existingTitles.Contains(numberedTitle))
                {
                    return numberedTitle;
                }
                counter++;
            }
        }
        public bool MarkAsCompleted(int id)
        {
            if (!_taskRepository.TaskExists(id))
                return false;
            _taskRepository.MarkAsCompleted(id);
            return true;
        }
        public bool DeleteTask(int id)
        {
            if (!_taskRepository.TaskExists(id))
                return false;
            _taskRepository.DeleteTask(id);
            return true;
        }
        
    }
}