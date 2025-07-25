using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToDoApp.Models;

namespace ToDoApp.DbContexts
{
    public class TasksDbContext: DbContext
    {
        public TasksDbContext(DbContextOptions options) : base(options) { }

        DbSet<TaskItem> Tasks { get; set; } = null!;
    }
}
