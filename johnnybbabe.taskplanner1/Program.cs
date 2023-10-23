// See https://aka.ms/new-console-template for more information
using johnnybbabe.taskplanner.DataAccess.Abstractions2;
using johnnybbabe.taskplanner.DataAccesss2;
using johnnybbabe.taskplanner.Domain.Logic;
using johnnybbabe.taskplanner.Domain.Models;
using Newtonsoft.Json;

public static class Program
{
    private static IWorkItemsRepository _workItemsRepository;
    private static SimpleTaskPlanner _taskPlanner;

    public static void Main(string[] args)
    {
        ConfigureDependencies();

        Console.WriteLine("Task Planner");

        LoadDataFromFile();

        while (true)
        {
            Console.WriteLine("Choose an option:");
            Console.WriteLine("[A] Add Work Item");
            Console.WriteLine("[B] Build a Plan");
            Console.WriteLine("[M] Mark Work Item as Completed");
            Console.WriteLine("[R] Remove a Work Item");
            Console.WriteLine("[Q] Quit the App");

            string choice = Console.ReadLine().Trim().ToUpper();

            switch (choice)
            {
                case "A":
                    AddTask();
                    break;
                case "B":
                    BuildPlan();
                    break;
                case "M":
                    MarkCompleted();
                    break;
                case "R":
                    RemoveTask();
                    break;
                case "Q":
                    SaveDataToFile();
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    private static void ConfigureDependencies()
    {
        // Set up your dependencies here
        _workItemsRepository = new FileWorkItemsRepository();
        _taskPlanner = new SimpleTaskPlanner(_workItemsRepository);
    }

    private static void AddTask()
    {
        Console.WriteLine("Add a New Work Item:");

        WorkItem newTask = new WorkItem();

        Console.Write("Title: ");
        newTask.Title = Console.ReadLine();

        Console.Write("Due Date (dd.MM.yyyy): ");
        if (DateTime.TryParseExact(Console.ReadLine(), "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime dueDate))
        {
            newTask.DueDate = dueDate;
        }
        else
        {
            Console.WriteLine("Invalid date format. Work Item not added.");
            return;
        }

        Console.Write("Priority (None, Low, Medium, High, Urgent): ");
        if (Enum.TryParse<Priority>(Console.ReadLine(), ignoreCase: true, out Priority priority))
        {
            newTask.Priority = priority;
        }
        else
        {
            Console.WriteLine("Invalid priority. Work Item not added.");
            return;
        }

        Console.Write("Complexity (None, Minutes, Hours, Days, Weeks): ");
        if (Enum.TryParse<Complexity>(Console.ReadLine(), ignoreCase: true, out Complexity complexity))
        {
            newTask.Complexity = complexity;
        }
        else
        {
            Console.WriteLine("Invalid complexity. Work Item not added.");
            return;
        }

        Console.Write("Description: ");
        newTask.Description = Console.ReadLine();

        newTask.CreationDate = DateTime.Now;
        newTask.IsCompleted = false;

        Guid taskId = _workItemsRepository.Add(newTask);
        Console.WriteLine($"Work Item added with ID: {taskId}");
    }

    private static void BuildPlan()
    {
        Console.WriteLine("Building a Plan...");

        var tasks = _workItemsRepository.GetAll();
        var plannedTasks = _taskPlanner.CreatePlan(tasks);//tasks

        Console.WriteLine("Planned Work Items:");
        foreach (var task in plannedTasks)
        {
            Console.WriteLine($"Work Item ID: {task.Id}");
            Console.WriteLine(task);
            Console.WriteLine();
        }
    }

    private static void MarkCompleted()
    {
        Console.WriteLine("Mark Work Item as Completed:");

        Console.Write("Enter the ID of the Work Item to mark as completed: ");
        if (Guid.TryParse(Console.ReadLine(), out Guid taskId))
        {
            var task = _workItemsRepository.Get(taskId);
            if (task != null)
            {
                task.IsCompleted = true;
                _workItemsRepository.Update(task);
                Console.WriteLine("Work Item marked as completed.");
            }
            else
            {
                Console.WriteLine("Work Item with that ID not found.");
            }
        }
        else
        {
            Console.WriteLine("Invalid ID format.");
        }
    }

    private static void RemoveTask()
    {
        Console.WriteLine("Remove a Work Item:");

        Console.Write("Enter the ID of the Work Item to remove: ");
        if (Guid.TryParse(Console.ReadLine(), out Guid taskId))
        {
            var task = _workItemsRepository.Get(taskId);
            if (task != null)
            {
                _workItemsRepository.Remove(taskId);
                Console.WriteLine("Work Item removed.");
            }
            else
            {
                Console.WriteLine("Work Item with that ID not found.");
            }
        }
        else
        {
            Console.WriteLine("Invalid ID format.");
        }
    }

    private static void LoadDataFromFile()
    {
        try
        {
            Console.WriteLine("Loading Work Items from file...");
            // Load data from the file when the program starts
            var jsonData = File.ReadAllText("work-items.json");
            var tasks = JsonConvert.DeserializeObject<WorkItem[]>(jsonData);
            foreach (var task in tasks)
            {
                _workItemsRepository.Add(task);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading Work Items: {ex.Message}");
        }
    }
    private static void SaveDataToFile()
    {
       
        try
        {
            // Отримуємо всі робочі елементи
            var tasks = _workItemsRepository.GetAll();

            // Серіалізуємо їх в JSON-рядок
            var jsonData = JsonConvert.SerializeObject(tasks, Formatting.Indented);

            // Записуємо весь JSON-рядок в файл, перезаписуючи існуючий вміст
            File.WriteAllText("work-items.json", jsonData);

            Console.WriteLine("Work Items saved to file.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving Work Items: {ex.Message}");
        }
    }
    /*
    private static void SaveDataToFile()
    {
        try
        {
            // Save data to the file before quitting the program
            var tasks = _workItemsRepository.GetAll();
            var jsonData = JsonConvert.SerializeObject(tasks, Formatting.Indented);
            File.WriteAllText("work-items.json", jsonData);

            Console.WriteLine("Work Items saved to file.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving Work Items: {ex.Message}");
        }
    }
    */
}
/*
internal static class Program
{
    //private static readonly IWorkItemsRepository _workItemsRepository = new FileWorkItemsRepository();
    //private static readonly SimpleTaskPlanner _taskPlanner = new SimpleTaskPlanner();
    

    public static void Main(string[] args)
    {
        IWorkItemsRepository workItemsRepository = new FileWorkItemsRepository();
        SimpleTaskPlanner taskPlanner = new SimpleTaskPlanner(workItemsRepository);

        // Створення екземпляра Program і передача репозиторія та планувальника через Dependency Injection
        
        Console.WriteLine("Task Planner");

        LoadDataFromFile();

        while (true)
        {
            Console.WriteLine("Choose an option:");
            Console.WriteLine("[A] Add Work Item");
            Console.WriteLine("[B] Build a Plan");
            Console.WriteLine("[M] Mark Work Item as Completed");
            Console.WriteLine("[R] Remove a Work Item");
            Console.WriteLine("[Q] Quit the App");

            string choice = Console.ReadLine().Trim().ToUpper();

            switch (choice)
            {
                case "A":
                    AddTask();
                    break;
                case "B":
                    BuildPlan();
                    break;
                case "M":
                    MarkCompleted();
                    break;
                case "R":
                    RemoveTask();
                    break;
                case "Q":
                    SaveDataToFile();
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }
    

    private static void AddTask()
    {
        Console.WriteLine("Add a New Work Item:");

        WorkItem newTask = new WorkItem();

        Console.Write("Title: ");
        newTask.Title = Console.ReadLine();

        Console.Write("Due Date (dd.MM.yyyy): ");
        if (DateTime.TryParseExact(Console.ReadLine(), "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime dueDate))
        {
            newTask.DueDate = dueDate;
        }
        else
        {
            Console.WriteLine("Invalid date format. Work Item not added.");
            return;
        }

        Console.Write("Priority (None, Low, Medium, High, Urgent): ");
        if (Enum.TryParse<Priority>(Console.ReadLine(), ignoreCase: true, out Priority priority))
        {
            newTask.Priority = priority;
        }
        else
        {
            Console.WriteLine("Invalid priority. Work Item not added.");
            return;
        }

        Console.Write("Complexity (None, Minutes, Hours, Days, Weeks): ");
        if (Enum.TryParse<Complexity>(Console.ReadLine(), ignoreCase: true, out Complexity complexity))
        {
            newTask.Complexity = complexity;
        }
        else
        {
            Console.WriteLine("Invalid complexity. Work Item not added.");
            return;
        }

        Console.Write("Description: ");
        newTask.Description = Console.ReadLine();

        newTask.CreationDate = DateTime.Now;
        newTask.IsCompleted = false;

        Guid taskId = _workItemsRepository.Add(newTask);
        Console.WriteLine($"Work Item added with ID: {taskId}");
    }

    private static void BuildPlan()
    {
        Console.WriteLine("Building a Plan...");

        var tasks = _workItemsRepository.GetAll();
        var plannedTasks = _taskPlanner.CreatePlan();  //tasks

        Console.WriteLine("Planned Work Items:");
        foreach (var task in plannedTasks)
        {
            Console.WriteLine($"Work Item ID: {task.Id}");
            Console.WriteLine(task);
            Console.WriteLine();
        }
    }

    private static void MarkCompleted()
    {
        Console.WriteLine("Mark Work Item as Completed:");

        Console.Write("Enter the ID of the Work Item to mark as completed: ");
        if (Guid.TryParse(Console.ReadLine(), out Guid taskId))
        {
            var task = _workItemsRepository.Get(taskId);
            if (task != null)
            {
                task.IsCompleted = true;
                _workItemsRepository.Update(task);
                Console.WriteLine("Work Item marked as completed.");
            }
            else
            {
                Console.WriteLine("Work Item with that ID not found.");
            }
        }
        else
        {
            Console.WriteLine("Invalid ID format.");
        }
    }

    private static void RemoveTask()
    {
        Console.WriteLine("Remove a Work Item:");

        Console.Write("Enter the ID of the Work Item to remove: ");
        if (Guid.TryParse(Console.ReadLine(), out Guid taskId))
        {
            var task = _workItemsRepository.Get(taskId);
            if (task != null)
            {
                _workItemsRepository.Remove(taskId);
                Console.WriteLine("Work Item removed.");
            }
            else
            {
                Console.WriteLine("Work Item with that ID not found.");
            }
        }
        else
        {
            Console.WriteLine("Invalid ID format.");
        }
    }

    private static void LoadDataFromFile()
    {
        try
        {
            Console.WriteLine("Loading Work Items from file...");
            _workItemsRepository.SaveChanges();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading Work Items: {ex.Message}");
        }
    }

    private static void SaveDataToFile()
    {
        try
        {
            _workItemsRepository.SaveChanges();

            var tasks = _workItemsRepository.GetAll();
            var jsonData = JsonConvert.SerializeObject(tasks, Formatting.Indented);
            File.WriteAllText("work-items.json", jsonData);

            Console.WriteLine("Work Items saved to file.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving Work Items: {ex.Message}");
        }
    }
}*/

/*
internal static class Program
{
    private static readonly IWorkItemsRepository _workItemsRepository = new FileWorkItemsRepository();

    public static void Main(string[] args)
    {
        Console.WriteLine("Task Planner");

        // Зчитуємо завдання з файлу під час запуску програми
        LoadDataFromFile();

        while (true)
        {
            Console.WriteLine("Choose an option:");
            Console.WriteLine("1. Add Task");
            Console.WriteLine("2. List Tasks");
            Console.WriteLine("3. Exit");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    AddTask();
                    break;
                case "2":
                    ListTasks();
                    break;
                case "3":
                    SaveDataToFile(); // Зберігаємо дані перед виходом
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    private static void AddTask()
    {
        Console.WriteLine("Add a New Task:");

        WorkItem newTask = new WorkItem();

        Console.Write("Title: ");
        newTask.Title = Console.ReadLine();

        Console.Write("Due Date (dd.MM.yyyy): ");
        if (DateTime.TryParseExact(Console.ReadLine(), "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime dueDate))
        {
            newTask.DueDate = dueDate;
        }
        else
        {
            Console.WriteLine("Invalid date format. Task not added.");
            return;
        }

        Console.Write("Priority (None, Low, Medium, High, Urgent): ");
        if (Enum.TryParse<Priority>(Console.ReadLine(), ignoreCase: true, out Priority priority))
        {
            newTask.Priority = priority;
        }
        else
        {
            Console.WriteLine("Invalid priority. Task not added.");
            return;
        }

        Console.Write("Complexity (None, Minutes, Hours, Days, Weeks): ");
        if (Enum.TryParse<Complexity>(Console.ReadLine(), ignoreCase: true, out Complexity complexity))
        {
            newTask.Complexity = complexity;
        }
        else
        {
            Console.WriteLine("Invalid complexity. Task not added.");
            return;
        }

        Console.Write("Description: ");
        newTask.Description = Console.ReadLine();

        newTask.CreationDate = DateTime.Now;
        newTask.IsCompleted = false;

        // Додаємо завдання та виводимо його інформацію
        Guid taskId = _workItemsRepository.Add(newTask);
        Console.WriteLine($"Task added with ID: {taskId}");
    }

    private static void ListTasks()
    {
        Console.WriteLine("List of Tasks:");

        var tasks = _workItemsRepository.GetAll();

        if (tasks.Length == 0)
        {
            Console.WriteLine("No tasks found.");
        }
        else
        {
            foreach (var task in tasks)
            {
                Console.WriteLine($"Task ID: {task.Id}");
                Console.WriteLine(task);
                Console.WriteLine();
            }
        }
    }

    private static void LoadDataFromFile()
    {
        // Зчитуємо дані з файлу при запуску
        try
        {
            Console.WriteLine("Loading tasks from file...");
            _workItemsRepository.SaveChanges(); // Зберігаємо будь-які зміни, щоб переконатися, що дані актуальні
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading tasks: {ex.Message}");
        }
    }

   /* private static void SaveDataToFile()
    {
        // Зберігаємо всі дані у файл перед виходом
        try
        {
            _workItemsRepository.SaveChanges();
            Console.WriteLine("Tasks saved to file.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving tasks: {ex.Message}");
        }
    }*//*
    private static void SaveDataToFile()
    {
        try
        {
            _workItemsRepository.SaveChanges();

            // Додайте наступний код для збереження даних в файл
            var tasks = _workItemsRepository.GetAll();
            var jsonData = JsonConvert.SerializeObject(tasks, Formatting.Indented);
            File.WriteAllText("work-items.json", jsonData);

            Console.WriteLine("Tasks saved to file.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving tasks: {ex.Message}");
        }
    }
}
*/