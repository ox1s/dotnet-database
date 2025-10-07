// using Services;
// using DataAccess.Entities;

// namespace Options
// {
//     public class AddTaskOption : IMenuOption
//     {
//         private readonly TaskService _taskService;

//         public string Name => "Добавить задачу";

//         public AddTaskOption(TaskService taskService)
//         {
//             _taskService = taskService;
//         }

//         public void Execute()
//         {
//             Console.Write("Введите название задачи: ");
//             var title = Console.ReadLine() ?? "";
//             Console.Write("Введите описание: ");
//             var desc = Console.ReadLine() ?? "...";

//             var task = new AppTask
//             {
//                 Title = title,
//                 Description = desc,
//                 IsCompleted = false,
//                 CreatedAt = DateTime.Now
//             };

//             _taskService.CreateTask(task);
//             Console.WriteLine("\n✅ Задача успешно добавлена!");
//         }
//     }
//     public class DeleteTaskOption : IMenuOption
//     {
//         private readonly TaskService _taskService;

//         public string Name => "Добавить задачу";

//         public AddTaskOption(TaskService taskService)
//         {
//             _taskService = taskService;
//         }

//         public void Execute()
//         {
//             Console.Write("Введите название задачи: ");
//             var title = Console.ReadLine() ?? "";
//             Console.Write("Введите описание: ");
//             var desc = Console.ReadLine() ?? "...";

//             var task = new AppTask
//             {
//                 Title = title,
//                 Description = desc,
//                 IsCompleted = false,
//                 CreatedAt = DateTime.Now
//             };

//             _taskService.CreateTask(task);
//             Console.WriteLine("\n✅ Задача успешно добавлена!");
//         }
//     }
// }