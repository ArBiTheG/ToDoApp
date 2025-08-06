using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoApp.DbContexts;
using ToDoApp.Models;

namespace ToDoApp.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        IDbContextFactory<TasksDbContext> _dbContextFactory;

        public TaskRepository(IDbContextFactory<TasksDbContext> factory)
        {
            _dbContextFactory = factory;
        }

        public async Task Create(TaskItem entity)
        {
            using TasksDbContext context = _dbContextFactory.CreateDbContext();
            context.Tasks.Add(entity);
            await context.SaveChangesAsync();
        }

        public async Task Delete(TaskItem entity)
        {
            using TasksDbContext context = _dbContextFactory.CreateDbContext();
            context.Tasks.Remove(entity);
            await context.SaveChangesAsync();
        }

        public async Task Update(TaskItem entity)
        {
            using TasksDbContext context = _dbContextFactory.CreateDbContext();
            context.Tasks.Update(entity);
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<TaskItem>> GetAll()
        {
            using TasksDbContext context = _dbContextFactory.CreateDbContext();
            return await context.Tasks.AsNoTracking().ToListAsync();
        }

        public async Task<TaskItem?> GetByID(int id)
        {
            using TasksDbContext context = _dbContextFactory.CreateDbContext();
            return await context.Tasks.FindAsync(id);
        }

        public async Task<IEnumerable<TaskItem>> GetAll(Func<TaskItem, bool> predicate)
        {
            using TasksDbContext context = _dbContextFactory.CreateDbContext();
            return await Task.Run(() => context.Tasks.AsNoTracking().Where(predicate).ToList());
        }
    }
}
