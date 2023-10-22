// See https://aka.ms/new-console-template for more information
using johnnybbabe.taskplanner.Domain.Logic;
using johnnybbabe.taskplanner.Domain.Models;

internal static class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Task Planner Application");

        Console.Write("Enter the number of WorkItems: ");
        if (int.TryParse(Console.ReadLine(), out int itemCount) && itemCount > 0)
        {
            WorkItem[] workItems = new WorkItem[itemCount];

            for (int i = 0; i < itemCount; i++)
            {
                Console.WriteLine($"WorkItem {i + 1}:");
                workItems[i] = CreateWorkItem();
            }

            SimpleTaskPlanner taskPlanner = new SimpleTaskPlanner();
            WorkItem[] sortedWorkItems = taskPlanner.CreatePlan(workItems);

            Console.WriteLine("\nSorted WorkItems:");
            foreach (WorkItem item in sortedWorkItems)
            {
                Console.WriteLine(item.ToString());
            }
        }
        else
        {
            Console.WriteLine("Invalid input. Please enter a valid number of WorkItems.");
        }
    }

    private static WorkItem CreateWorkItem()
    {
        Console.Write("Title: ");
        string title = Console.ReadLine();

        Console.Write("Due Date (dd.MM.yyyy): ");
        if (DateTime.TryParseExact(Console.ReadLine(), "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime dueDate))
        {
            Console.Write("Priority (None, Low, Medium, High, Urgent): ");
            if (Enum.TryParse<Priority>(Console.ReadLine(), out Priority priority))
            {
                Console.Write("Complexity (None, Minutes, Hours, Days, Weeks): ");
                if (Enum.TryParse<Complexity>(Console.ReadLine(), out Complexity complexity))
                {
                    Console.Write("Description: ");
                    string description = Console.ReadLine();

                    Console.Write("Is Completed (true or false): ");
                    if (bool.TryParse(Console.ReadLine(), out bool isCompleted))
                    {
                        return new WorkItem
                        {
                            Title = title,
                            DueDate = dueDate,
                            Priority = priority,
                            Complexity = complexity,
                            Description = description,
                            IsCompleted = isCompleted
                        };
                    }
                }
            }
        }

        Console.WriteLine("Invalid input. Please enter valid WorkItem details.");
        return CreateWorkItem();
    }
}