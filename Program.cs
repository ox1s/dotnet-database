using System.Text.Json;
using DataAccess.Factories;
using DataAccess.Repositories;

var settingsText = File.ReadAllText("settings.json");
using var jsonDoc = JsonDocument.Parse(settingsText);
var connectionString = jsonDoc
    .RootElement
    .GetProperty("ConnectionStrings")
    .GetProperty("DefaultConnection")
    .GetString();

var connectionFactory = new MsSqlConnectionFactory(connectionString!);

var taskRepository = new TaskRepository(connectionFactory);


var menu = new Menu(taskRepository);
menu.DisplayMenu();
