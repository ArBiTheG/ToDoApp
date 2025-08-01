using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoApp.DbContexts;
using ToDoApp.Models;

namespace ToDoApp.Repositories
{
    public class MockTaskRepository: ITaskRepository
    {
        List<TaskItem> _tasks;
        public MockTaskRepository()
        {
            _tasks = new List<TaskItem>();
            _tasks.Add(new TaskItem() { Name="Test", Description="DescriptionTest"});
        }

        public async Task Create(TaskItem entity)
        {
            await Task.Run(() => _tasks.Add(entity));
            ;
        }

        public async Task Delete(TaskItem entity)
        {
            await Task.Run(() => _tasks.Remove(entity));
        }

        public async Task Update(TaskItem entity)
        {
            await Task.Run(() => _tasks);
        }

        public async Task<IEnumerable<TaskItem>> GetAll()
        {
            return await Task.Run(() => _tasks);
        }

        public async Task<TaskItem?> GetByID(int id)
        {
            return await Task.Run(() => _tasks.Find(u => u.Id == id));
        }

        public async Task<IEnumerable<TaskItem>> GetAll(Func<TaskItem, bool> predicate)
        {
            return await Task.Run(() => _tasks.Where(predicate).ToList());
        }
    }
}
