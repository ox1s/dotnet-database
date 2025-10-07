using Services;
using DataAccess.Entities;

namespace Options
{

    public class AddTaskOption : IMenuOption
    {
        private readonly TaskService _taskService;

        public string Name => "Добавить задачу";

        public AddTaskOption(TaskService taskService)
        {
            _taskService = taskService;
        }

        public void Execute()
        {
            Console.Clear();
            Console.Write("Введите название задачи: ");
            var title = Console.ReadLine() ?? "";
            Console.Write("Введите описание: ");
            var desciption = Console.ReadLine() ?? "...";

            var task = new AppTask
            {
                Title = title,
                Description = desciption,
                IsCompleted = false,
                CreatedAt = DateTime.Now
            };

            _taskService.CreateTask(task);

            Console.WriteLine(ConsoleStyler.Green("✅ Задача\n"));
            Console.WriteLine($"\t{title}\n\t{desciption}");
            Console.WriteLine(ConsoleStyler.Green("успешно добавлена!"));
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
                _taskService.MarkAsCompleted(idToMarkAsCompleted.Value);
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
            Console.ForegroundColor = ConsoleColor.Green;
            _taskConsoleView.DisplayAllTasks();
        }

    }
}