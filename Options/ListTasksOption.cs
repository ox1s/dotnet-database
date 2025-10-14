namespace Options;

public class ListTasksOption : IMenuOption
{
    private readonly TaskConsoleView _taskConsoleView;

    public string Name => "Просмотреть список задач";
    public ListTasksOption(TaskConsoleView taskConsoleView)
    {
        _taskConsoleView = taskConsoleView;
    }
    public void Execute()
    {
        Console.Clear();
        Console.WriteLine("📋 Список задач:\n");
        _taskConsoleView.DisplayAllTasks();
    }

}
