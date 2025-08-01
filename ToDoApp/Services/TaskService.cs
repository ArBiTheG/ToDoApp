using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoApp.Models;
using ToDoApp.Repositories;

namespace ToDoApp.Services
{
    public class TaskService : ITaskService
    {
        ITaskRepository _taskRepository;


        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }
        public async Task<IEnumerable<TaskItem>> GetUncompleteList()
        {
            return await _taskRepository.GetAll(u => u.IsCompleted == false);
        }

        public async Task<TaskItem?> Get(int id)
        {
            return await _taskRepository.GetByID(id);
        }

        public async Task Create(TaskItem taskItem)
        {
            await _taskRepository.Create(taskItem);
        }

        public async Task Edit(TaskItem taskItem, TaskItem taskItemNew)
        {
            taskItem.Name = taskItemNew.Name;
            taskItem.Description = taskItemNew.Description;
            taskItem.DateTime = taskItemNew.DateTime;
            taskItem.IsCompleted = taskItemNew.IsCompleted;
            taskItem.CompletedDateTime = taskItemNew.CompletedDateTime;

            await _taskRepository.Update(taskItem);
        }

        public async Task Remove(TaskItem taskItem)
        {
            await _taskRepository.Delete(taskItem);
        }

        public async Task Complete(TaskItem taskItem)
        {
            taskItem.IsCompleted = true;
            taskItem.CompletedDateTime = DateTime.Now;
            await _taskRepository.Update(taskItem);
        }

        public async Task Uncomplete(TaskItem taskItem)
        {
            taskItem.IsCompleted = false;
            await _taskRepository.Update(taskItem);
        }
    }
}
