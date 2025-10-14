namespace Options;

using Services;
using DataAccess.Entities;

public class AddTaskOption : IMenuOption
{
    private readonly TaskService _taskService;
    private readonly TaskConsoleView _taskConsoleView;

    public string Name => "Добавить задачу";

    public AddTaskOption(TaskService taskService, TaskConsoleView taskConsoleView)
    {
        _taskService = taskService;
        _taskConsoleView = taskConsoleView;
    }

    public void Execute()
    {
        Console.Clear();
        Console.Write("Введите название задачи: ");
        var title = Console.ReadLine() ?? "";
        Console.Write("Введите описание: ");
        string description = Console.ReadLine() ?? "";

        if (string.IsNullOrWhiteSpace(description))
        {
            description = "...";
        }

        var taskToCreate = new AppTask
        {
            Title = title,
            Description = description,
            IsCompleted = false,
            CreatedAt = DateTime.Now
        };

        AppTask createdTask = _taskService.CreateTask(taskToCreate);

        Console.WriteLine(ConsoleStyler.Green("\n✅ Задача успешно добавлена:"));
        _taskConsoleView.PrintInfo(createdTask);
    }
}
