using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ToDoApp.DbContexts;
using ToDoApp.Models;
using ToDoApp.Repositories;
using ToDoApp.Services;
using ToDoApp.ViewModels;
using ToDoApp.Views;

namespace ToDoApp;

public partial class App : Application
{
    private IHost _host;

    public App()
    {
        _host = CreateHostBuilder().Build();
    }


    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }
    private IHostBuilder CreateHostBuilder()
    {
        return Host.CreateDefaultBuilder()
            .ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<MainViewModel>();

                string? connectionString = hostContext.Configuration.GetConnectionString("Default");
                if (connectionString != null)
                    services.AddSingleton<ITasksDbContextFactory>(new TasksDbContextFactory(connectionString));

                services.AddSingleton<ITaskRepository, MockTaskRepository>();
                services.AddSingleton<ITaskService, TaskService>();
            });
    }

    public override void OnFrameworkInitializationCompleted()
    {
        _host.Start();

        ITasksDbContextFactory contextFactory = _host.Services.GetRequiredService<ITasksDbContextFactory>();
        using (TasksDbContext context = contextFactory.CreateDbContext())
        {
            context.Database.EnsureCreated();
        }

        var mainViewModel = _host.Services.GetRequiredService<MainViewModel>();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = mainViewModel
            };
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = new MainView
            {
                DataContext = mainViewModel
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}
