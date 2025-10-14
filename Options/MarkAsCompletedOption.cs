namespace Options;

using Services;
public class MarkAsCompletedOption : IMenuOption
{
    private readonly TaskService _taskService;
    private readonly TaskConsoleView _taskConsoleView;
    public string Name => "Отметить задачу как выполненную";
    public MarkAsCompletedOption(TaskService taskService, TaskConsoleView taskConsoleView)
    {
        _taskService = taskService;
        _taskConsoleView = taskConsoleView;
    }
    public void Execute()
    {
        Console.Clear();
        int? idToMarkAsCompleted = _taskConsoleView.SelectTaskId("Введите номер задачи для отметки", allTasks: false);

        if (!idToMarkAsCompleted.HasValue)
        {
            Console.WriteLine(ConsoleStyler.Red($"\n❌ Не удалось отметить задачу (возможно, она уже была отмечена)."));
            return;
        }

        _taskService.MarkAsCompleted(idToMarkAsCompleted.Value);
        Console.WriteLine(ConsoleStyler.Green("\n✅ Задача успешно отмечена!"));
    }

}