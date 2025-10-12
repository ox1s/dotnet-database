using System.Text.Json;
using DataAccess.Factories;
using DataAccess.Repositories;
using Services;

try
{
    var settingsText = File.ReadAllText("settings.json");
    using var jsonDoc = JsonDocument.Parse(settingsText);
    var connectionString = jsonDoc
        .RootElement
        .GetProperty("ConnectionStrings")
        .GetProperty("DefaultConnection")
        .GetString();

    var dbConnectionFactory = new MsSqlConnectionFactory(connectionString!);
    var dbSetup = new DatabaseSetup(dbConnectionFactory);
    dbSetup.Setup();

    var connectionFactory = new MsSqlConnectionFactory(connectionString!);
    var taskRepository = new TaskRepository(connectionFactory);
    var taskService = new TaskService(taskRepository);
    var taskConsoleView = new TaskConsoleView(taskService);
    var menu = new Menu(taskService, taskConsoleView);
    menu.DisplayMenu();
}
catch (Exception ex)
{
    Console.WriteLine(ConsoleStyler.Red("Произошла критическая ошибка. Приложение будет закрыто."));
    Console.WriteLine(ex.Message);
}