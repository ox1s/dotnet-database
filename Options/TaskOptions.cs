using Services;
using DataAccess.Entities;

namespace Options
{

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
            if (idToDelete.HasValue)
            {
                if (_taskService.DeleteTask(idToDelete.Value))
                {
                    Console.WriteLine(ConsoleStyler.Green("\n✅ Задача успешно удалена!"));
                }
                else
                {
                    Console.WriteLine(ConsoleStyler.Red($"\n❌ Не удалось удалить задачу (возможно, она уже была удалена)."));
                }
            }
        }
    }
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
            if (idToMarkAsCompleted.HasValue)
            {

                if (_taskService.MarkAsCompleted(idToMarkAsCompleted.Value))
                {
                    Console.WriteLine(ConsoleStyler.Green("\n✅ Задача успешно отмечена!"));
                }
                else
                {
                    Console.WriteLine(ConsoleStyler.Red($"\n❌ Не удалось отметить задачу (возможно, она уже была отмечена)."));
                }
            }
        }

    }
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
}