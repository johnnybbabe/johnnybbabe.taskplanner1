
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
            // завантаження з файлу на початку програми
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
            var tasks = _workItemsRepository.GetAll();
            // Серіалізація в JSON-рядок
            var jsonData = JsonConvert.SerializeObject(tasks, Formatting.Indented);
            // Записування JSON-рядка в файл, перезаписуючи існуючий вміст
            File.WriteAllText("work-items.json", jsonData);
            Console.WriteLine("Work Items saved to file.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving Work Items: {ex.Message}");
        }
    }  
}
