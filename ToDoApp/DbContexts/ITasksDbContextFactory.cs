using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoApp.DbContexts
{
    public interface ITasksDbContextFactory
    {
        public TasksDbContext CreateDbContext();
    }
}
