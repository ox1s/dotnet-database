namespace Options;

using Services;
public class DeleteTaskOption : IMenuOption
{
    private readonly TaskService _taskService;
    private readonly TaskConsoleView _taskConsoleView;

    public string Name => "Удалить задачу";

    public DeleteTaskOption(TaskService taskService, TaskConsoleView taskConsoleView)
    {
        _taskService = taskService;
        _taskConsoleView = taskConsoleView;
    }

    public void Execute()
    {
        Console.Clear();
        int? idToDelete = _taskConsoleView.SelectTaskId("Введите номер задачи для удаления");

        if (!idToDelete.HasValue)
        {
            Console.WriteLine(ConsoleStyler.Red($"\n❌ Не удалось удалить задачу (возможно, она уже была удалена)."));
            return;
        }
        
        _taskService.DeleteTask(idToDelete.Value);
        Console.WriteLine(ConsoleStyler.Green("\n✅ Задача успешно удалена!"));
    }
}