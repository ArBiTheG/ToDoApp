using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ToDoApp.DbContexts;
using ToDoApp.Models;
using ToDoApp.Repositories;
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
                services.AddDbContextFactory<TasksDbContext>(options=> options.UseSqlite(connectionString));

                services.AddSingleton<ITaskRepository, TaskRepository>();
            });
    }

    public override void OnFrameworkInitializationCompleted()
    {
        _host.Start();

        IDbContextFactory<TasksDbContext> contextFactory = _host.Services.GetRequiredService<IDbContextFactory<TasksDbContext>>();
        using (TasksDbContext context = contextFactory.CreateDbContext())
        {
            context.Database.Migrate();
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
