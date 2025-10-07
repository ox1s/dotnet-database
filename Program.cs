using System.Text.Json;
using DataAccess.Factories;
using DataAccess.Repositories;
using Services;

var settingsText = File.ReadAllText("settings.json");
using var jsonDoc = JsonDocument.Parse(settingsText);
var connectionString = jsonDoc
    .RootElement
    .GetProperty("ConnectionStrings")
    .GetProperty("DefaultConnection")
    .GetString();

var connectionFactory = new MsSqlConnectionFactory(connectionString!);
var taskRepository = new TaskRepository(connectionFactory);
var taskService = new TaskService(taskRepository);
var taskConsoleView = new TaskConsoleView(taskService); 
var menu = new Menu(taskService, taskConsoleView); 
menu.DisplayMenu();
