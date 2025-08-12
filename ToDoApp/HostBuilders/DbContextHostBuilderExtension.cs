using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoApp.DbContexts;

namespace ToDoApp.HostBuilders
{
    public static class DbContextHostBuilderExtension
    {
        public static IHostBuilder AddDbContext(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((context, services) =>
            {
                string? connectionString = context.Configuration.GetConnectionString("Default");

                Action<DbContextOptionsBuilder> configureDbContext = o => o.UseSqlite(connectionString);

                //services.AddDbContext<TasksDbContext>(configureDbContext);
                services.AddDbContextFactory<TasksDbContext>(configureDbContext);
            });
            return hostBuilder;
        }
    }
}
