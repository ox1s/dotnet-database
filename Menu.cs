
using System;
using System.Net;
using DataAccess.Repositories;
using DataAccess.Entities;
using Services;
using System.Diagnostics;
using Options;

public class Menu
{
    private readonly List<IMenuOption> _options;
    public Menu(TaskService taskService, TaskConsoleView taskConsoleView)
    {
        _options = new List<IMenuOption>
        {
            new AddTaskOption(taskService, taskConsoleView),
            new ListTasksOption(taskConsoleView),
            new MarkAsCompletedOption(taskService, taskConsoleView),
            new DeleteTaskOption(taskService, taskConsoleView),
        };
    }

    public void DisplayMenu()
    {
        while (true)
        {
            Console.Clear();
            string logo =
"""

  _______        _                         
 |__   __|      | |                        
    | | __ _ ___| | __                     
    | |/ _` / __| |/ /                     
    | | (_| \__ \   <                      
  __|_|\__,_|___/_|\_\                     
 |  \/  |                                  
 | \  / | __ _ _ __   __ _  __ _  ___ _ __ 
 | |\/| |/ _` | '_ \ / _` |/ _` |/ _ \ '__|
 | |  | | (_| | | | | (_| | (_| |  __/ |   
 |_|  |_|\__,_|_| |_|\__,_|\__, |\___|_|   
                            __/ |          
                           |___/           

""";
            Console.WriteLine(ConsoleStyler.Green(logo));
            for (int i = 0; i < _options.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {_options[i].Name}");
            }
            Console.WriteLine($"{_options.Count + 1}. {ConsoleStyler.Red("Выход")}");
            
            Console.Write("\nВведите опцию: ");
            if (int.TryParse(Console.ReadLine(), out int choice))
            {
                if (choice > 0 && choice <= _options.Count)
                {
                    Console.Clear();
                    _options[choice - 1].Execute();
                    PauseToReturn();
                }
                else if (choice == _options.Count + 1)
                {
                    return;
                }
            }
        }
    }
    void PauseToReturn()
    {
        Console.WriteLine("\nНажмите любую клавишу, чтобы вернуться в меню...");
        Console.ReadKey();
    }

}
