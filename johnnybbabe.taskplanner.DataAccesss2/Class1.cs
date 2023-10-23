using johnnybbabe.taskplanner.DataAccess.Abstractions2;
using johnnybbabe.taskplanner.Domain.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
namespace johnnybbabe.taskplanner.DataAccesss2

{
    public class FileWorkItemsRepository : IWorkItemsRepository
    {
        private const string DataFile = "work-items.json";
        private readonly Dictionary<Guid, WorkItem> _data;

        public FileWorkItemsRepository()
        {
            _data = LoadDataFromFile();
        }

        public Guid Add(WorkItem workItem)
        {
            // Додавання нового робочого елементу до словника
            var newId = Guid.NewGuid();
            workItem.Id = newId;
            _data.Add(newId, workItem);
            return newId;
        }

        public WorkItem Get(Guid id)
        {
            if (_data.TryGetValue(id, out var workItem))
            {
                return workItem;
            }
            return null;
        }

        public WorkItem[] GetAll()
        {
            return _data.Values.ToArray();
        }

        public bool Update(WorkItem workItem)
        {
            if (_data.ContainsKey(workItem.Id))
            {
                _data[workItem.Id] = workItem;
                return true;
            }
            return false;
        }

        public bool Remove(Guid id)
        {
            return _data.Remove(id);
        }

        public void SaveChanges()
        {
            // Збереження всіх робочих елементів у файл
            var jsonData = JsonConvert.SerializeObject(_data.Values.ToArray(), Formatting.Indented);
            File.WriteAllText(DataFile, jsonData);
        }

        private Dictionary<Guid, WorkItem> LoadDataFromFile()
        {
            if (File.Exists(DataFile))
            {
               // var jsonData = File.ReadAllText(DataFile);
                //var items = JsonConvert.DeserializeObject<WorkItem[]>(jsonData);//DeserializeObject
               //if (items != null)
              //{
               //    return items.ToDictionary(item => item.Id);
               // }
            }
            return new Dictionary<Guid, WorkItem>();
        }
    }
}