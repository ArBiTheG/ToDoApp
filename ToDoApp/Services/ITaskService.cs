using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoApp.Models;

namespace ToDoApp.Services
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskItem>> GetUncompleteList();
        Task<TaskItem?> Get(int id);
        Task Create(TaskItem taskItem);
        Task Edit(TaskItem taskItem, TaskItem taskItemNew);
        Task Remove(TaskItem taskItem);
        Task Complete(TaskItem taskItem);
        Task Uncomplete(TaskItem taskItem);
    }
}
