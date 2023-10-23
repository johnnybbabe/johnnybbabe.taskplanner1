
using johnnybbabe.taskplanner.DataAccess.Abstractions2;
using johnnybbabe.taskplanner.Domain.Models;

namespace johnnybbabe.taskplanner.Domain.Logic
{

    public class SimpleTaskPlanner
    {
        private readonly IWorkItemsRepository _workItemsRepository;

        public SimpleTaskPlanner(IWorkItemsRepository workItemsRepository)
        {
            _workItemsRepository = workItemsRepository;
        }

        public List<WorkItem> CreatePlan(IEnumerable<WorkItem> tasks)
        {
            var pendingTasks = tasks.Where(task => !task.IsCompleted);

            var sortedTasks = pendingTasks
                .OrderByDescending(task => task.Priority)
                .ThenBy(task => task.DueDate)
                .ThenBy(task => task.Title)
                .ToList();

            // Повертаємо список відсортованих завдань
            return sortedTasks;
        }
    }
    /*
    public class SimpleTaskPlanner
    {
        private readonly IWorkItemsRepository _workItemsRepository;

        public SimpleTaskPlanner(IWorkItemsRepository workItemsRepository)
        {
            _workItemsRepository = workItemsRepository;
        }

        public List<WorkItem> CreatePlan(IEnumerable<WorkItem> tasks)

        {
            var pendingTasks = tasks.Where(task => !task.IsCompleted);//
            //var workItems = _workItemsRepository.GetAll();
            //return workItems
              //  .OrderByDescending(item => item.Priority)
              //  .ThenBy(item => item.DueDate)
               // .ThenBy(item => item.Title)
               // .ToList();
            var sortedTasks = pendingTasks
            .OrderByDescending(task => task.Priority)
            .ThenBy(task => task.DueDate)
            .ThenBy(task => task.Title);

        // Повертаємо список відсортованих завдань
        return sortedTasks.ToList();
        }
    }*/



    /*

    public class SimpleTaskPlanner
    {
        public WorkItem[] CreatePlan(WorkItem[] Items)
        {
            List<WorkItem> workItemList = Items.ToList();

            workItemList.Sort((a, b) =>
            {
                if (a.Priority != b.Priority)
                    return b.Priority.CompareTo(a.Priority);
                if (a.DueDate != b.DueDate)
                    return a.DueDate.CompareTo(b.DueDate);
                return string.Compare(a.Title, b.Title, StringComparison.Ordinal);
            });

            return workItemList.ToArray();
        }









        /*public WorkItem[] CreatePlan(WorkItem[] items)
        {
            return items
                .OrderByDescending(item => item.Priority)
                .ThenBy(item => item.DueDate)
                .ThenBy(item => item.Title)
                .ToArray();
        }*\\
    }
   */
}
