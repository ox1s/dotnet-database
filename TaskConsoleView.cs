using DataAccess.Entities;
using Services;
public class TaskConsoleView
{
    private readonly TaskService _taskService;

    private const string green = "\u001b[32m";
    private const string red = "\u001b[31m";
    private const string endColor = "\u001b[0m";

    public TaskConsoleView(TaskService taskService)
    {
        _taskService = taskService;
    }

    public void DisplayAllTasks()
    {
        Console.Clear();
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
            Console.Write($"{message} или введите {red}q{endColor} для отмены: ");
            string? input = Console.ReadLine();

            if (input == "q")
                return null;

            if (int.TryParse(input, out int id))
            {
                if (_taskService.TaskExists(id))
                    return id;
                Console.WriteLine($"{red}❌ Задачи с таким номером не существует.{endColor}");
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
            ? $"{green}✅ Выполненные задачи:"
            : $"{red}⌛ Не выполненные задачи:";

        Console.WriteLine(text + endColor);

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