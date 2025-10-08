using DataAccess.Entities;
using Services;
public class TaskConsoleView
{
    private readonly TaskService _taskService;

    public TaskConsoleView(TaskService taskService)
    {
        _taskService = taskService;
    }

    public void DisplayAllTasks()
    {
        PrintTasksByStatus(true);
        PrintTasksByStatus(false);
    }

    public int? SelectTaskId(string message, bool allTasks = true)
    {
        if (allTasks)
            DisplayAllTasks();
        else
            PrintTasksByStatus(false);
        Console.WriteLine("===================================");

        while (true)
        {
            Console.Write($"{message} или введите {ConsoleStyler.Red("q")} для отмены: ");
            string? input = Console.ReadLine();

            if (input == "q")
                return null;

            if (int.TryParse(input, out int id))
            {
                if (_taskService.TaskExists(id))
                    return id;
                Console.WriteLine(ConsoleStyler.Red($"❌ Задачи с таким номером не существует."));
            }
            else
            {
                Console.WriteLine("⚠️ Введите корректное число.");
            }
        }
    }

    private void PrintTasksByStatus(bool isCompleted)
    {
        var tasksList = isCompleted
            ? _taskService.GetAllCompletedTasks()
            : _taskService.GetAllNotCompletedTasks();

        string text = isCompleted
            ? ConsoleStyler.Green($"✅ Выполненные задачи:")
            : ConsoleStyler.Red($"⌛ Не выполненные задачи:");

        Console.WriteLine(text);

        if (tasksList.Any())
        {
            foreach (var task in tasksList)
                PrintInfo(task);
        }
        else
        {
            Console.WriteLine("(нет задач в этой категории)");
        }
    }
    public void PrintInfo(AppTask task)
    {
        Console.WriteLine($"[{task.Id}] {task.Title} - {task.Description}");
    }
}