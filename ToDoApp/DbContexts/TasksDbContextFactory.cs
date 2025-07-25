using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoApp.DbContexts
{
    public class TasksDbContextFactory : ITasksDbContextFactory
    {
        private readonly string _connectionString;

        public TasksDbContextFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public TasksDbContext CreateDbContext()
        {
            DbContextOptions options = new DbContextOptionsBuilder().UseSqlite(_connectionString).Options;
            return new TasksDbContext(options);
        }
    }
}
