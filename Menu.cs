
using System;
using System.Net;
using DataAccess.Repositories;
using DataAccess.Entities;
using Services;
using System.Diagnostics;

public class Menu
{

    private readonly TaskService _taskService;
    public Menu(TaskService taskService)
    {
        _taskService = taskService;
    }
    string? readResult;
    string greenWithArrow = " \u001b[32m >  ";
    string rednWithArrow = "\u001b[31m x  ";
    string green = "\u001b[32m";
    string red = "\u001b[31m";
    string endColor = "\u001b[0m";

    bool isSelected = false;
    int option = 1;

    ConsoleKeyInfo key;
    bool validEntry = false;

    public void DisplayMenu()
    {
        do
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            string logo = """
 _________  ________  ________  ___  __                                         
|\___   ___\\   __  \|\   ____\|\  \|\  \                                       
\|___ \  \_\ \  \|\  \ \  \___|\ \  \/  /|_                                     
     \ \  \ \ \   __  \ \_____  \ \   ___  \                                    
      \ \  \ \ \  \ \  \|____|\  \ \  \\ \  \                                   
       \ \__\ \ \__\ \__\____\_\  \ \__\\ \__\                                  
        \|__|  \|__|\|__|\_________\|__| \|__|                                  
                        \|_________|                                            
                                                                                
                                                                                
 _____ ______   ________  ________   ________  ________  _______   ________     
|\   _ \  _   \|\   __  \|\   ___  \|\   __  \|\   ____\|\  ___ \ |\   __  \    
\ \  \\\__\ \  \ \  \|\  \ \  \\ \  \ \  \|\  \ \  \___|\ \   __/|\ \  \|\  \   
 \ \  \\|__| \  \ \   __  \ \  \\ \  \ \   __  \ \  \  __\ \  \_|/_\ \   _  _\  
  \ \  \    \ \  \ \  \ \  \ \  \\ \  \ \  \ \  \ \  \|\  \ \  \_|\ \ \  \\  \| 
   \ \__\    \ \__\ \__\ \__\ \__\\ \__\ \__\ \__\ \_______\ \_______\ \__\\ _\ 
    \|__|     \|__|\|__|\|__|\|__| \|__|\|__|\|__|\|_______|\|_______|\|__|\|__|
                                                                                
                                                                                
                                                                                                                                              
""";
            Console.WriteLine(logo);
            Console.ResetColor();
            Console.WriteLine($"\nИспользуйте стрелки вверх и вниз для выбора опции и нажмите \u001b[32mEnter{endColor} для выбора.");
            (int left, int top) = Console.GetCursorPosition();

            while (!isSelected)
            {
                Console.SetCursorPosition(left, top);
                Console.CursorVisible = false;

                Console.WriteLine("\nВыберите опцию:");
                Console.WriteLine($"{(option == 1 ? greenWithArrow : "     ")}1. Добавить задачу" + endColor);
                Console.WriteLine($"{(option == 2 ? greenWithArrow : "     ")}2. Просмотреть список задач" + endColor);
                Console.WriteLine($"{(option == 3 ? greenWithArrow : "     ")}4. Отметить задачу как выполненную" + endColor);
                Console.WriteLine($"{(option == 4 ? greenWithArrow : "     ")}5. Удалить задачу" + endColor);
                Console.WriteLine($"{(option == 5 ? rednWithArrow : "    ")}6. Выйти" + endColor);

                key = Console.ReadKey(true);

                switch (key.Key)
                {
                    case ConsoleKey.DownArrow:
                        option = (option == 5 ? 1 : option + 1);
                        break;
                    case ConsoleKey.UpArrow:
                        option = (option == 1 ? 5 : option - 1);
                        break;
                    case ConsoleKey.Enter:
                        isSelected = true;
                        break;
                }

            }
            Console.Clear();

            switch (option)
            {
                case 1:
                    Console.CursorVisible = true;
                    AddTask();
                    break;
                case 2:
                    Console.CursorVisible = true;
                    ListTask();
                    break;
                case 3:
                    MarkAsCompleted();
                    break;
                case 4:
                    DeleteTask();
                    Console.CursorVisible = true;
                    break;
            }
        } while (option != 5);


    }
    public void AddTask()
    {
        string? userInput = "y";
        do
        {
            Console.Write("Введите название задачи: ");
            var title = Console.ReadLine() ?? "";
            Console.Write("Введите описание: ");
            var desc = Console.ReadLine() ?? "...";

            var task = new AppTask
            {
                Title = title,
                Description = desc,
                IsCompleted = false,
                CreatedAt = DateTime.Now
            };

            _taskService.CreateTask(task);
            Console.WriteLine("Задача добавлена!");
            Debug.Assert(task != null, $"{red}Task должен был создаться, но произошла фатальная ошибка");
            PrintInfo(_taskService.GetTaskInfo(task.Id));
            Console.ReadKey();
            Console.Clear();
            Console.Write("Желаете добавить еще задачу? (y/n): ");
            do
            {
                userInput = Console.ReadLine();

                if (userInput == "y")
                {
                    Console.Clear();
                    isSelected = false;
                    validEntry = true;
                }
                else if (userInput == "n")
                {
                    PauseToReturn();
                    validEntry = true;
                }
                else
                {
                    Console.Write("\nВы ввели символ отличный от y/n, попробуйте еще раз: ");
                    validEntry = false;
                }
            } while (!validEntry);

        } while (userInput == "y");
    }

    public void MarkAsCompleted()
    {
        PrintTasks(false);
        Console.WriteLine("================================");
        int? idToMarkAsCompleted = SelectTaskId("Какую задачу отметить выполненной?: ");
        if (idToMarkAsCompleted.HasValue)
        {
            _taskService.MarkAsCompleted(idToMarkAsCompleted.Value);
        }
        PauseToReturn();
    }
    public void PrintTasks(bool isCompletedTasks)
    {
        var completedTasks = _taskService.GetAllCompletedTasks().ToList();
        var notCompletedTasks = _taskService.GetAllNotCompletedTasks().ToList();

        var checkedList = isCompletedTasks ? completedTasks.Any() : notCompletedTasks.Any();
        var tasksList = isCompletedTasks ? completedTasks : notCompletedTasks;

        string text = isCompletedTasks ? $"{green}✅ Выполненные задачи:" : $"{red}⌛ Не выполненные задачи:";
        Console.WriteLine(text + endColor);
        if (checkedList)
        {
            foreach (var task in tasksList)
                PrintInfo(task);
        }
        else
        {
            Console.WriteLine("(нет выполненных задач)");
        }
    }
    public void ListTask(bool isInAnotherOption = false)
    {
        Console.Clear();
        Console.WriteLine("📋 Список задач:\n");

        Console.ForegroundColor = ConsoleColor.Green;

        PrintTasks(true);
        PrintTasks(false);

        if (!isInAnotherOption)
        {
            PauseToReturn();
        }
    }
    public void PrintInfo(AppTask task)
    {
        Console.WriteLine($"[{task.Id}] {task.Title} - {task.Description}");
    }


    public void DeleteTask()
    {
        Console.Clear();
        int? idToDelete = SelectTaskId("Введите номер задачи для удаления");
        if (idToDelete.HasValue)
        {
            if (_taskService.DeleteTask(idToDelete.Value))
            {
                Console.WriteLine("\n✅ Задача успешно удалена!");
            }
            else
            {
                Console.WriteLine("\n❌ Не удалось удалить задачу (возможно, она уже была удалена).");
            }
        }
        PauseToReturn();
    }

    int? SelectTaskId(string message)
    {
        Console.Clear();
        ListTask(isInAnotherOption: true);
        Console.WriteLine("===================================");

        while (true)
        {
            Console.Write($"{message} или введите {red}q{endColor} для отмены: ");
            string? input = Console.ReadLine();

            if (input == "q")
                return null;

            if (int.TryParse(input, out int id))
            {
                if (_taskService.TaskExists(id))
                    return id;
                Console.WriteLine("❌ Задачи с таким номером не существует.");
            }
            else
            {
                Console.WriteLine("⚠️ Введите корректное число.");
            }
        }

    }

    void PauseToReturn()
    {
        Console.WriteLine("\nНажмите любую клавишу, чтобы вернуться в меню...");
        Console.ReadKey();
        isSelected = false;
    }

}
