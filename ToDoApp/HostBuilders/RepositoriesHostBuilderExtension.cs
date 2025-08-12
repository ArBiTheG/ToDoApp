using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoApp.Repositories;

namespace ToDoApp.HostBuilders
{
    public static class RepositoriesHostBuilderExtension
    {
        public static IHostBuilder AddRepository(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((context, services) =>
            {
                services.AddSingleton<ITaskRepository, TaskRepository>();
            });
            return hostBuilder;
        }
    }
}
