using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ToDoApp.DbContexts;
using ToDoApp.HostBuilders;
using ToDoApp.Models;
using ToDoApp.Repositories;
using ToDoApp.ViewModels;
using ToDoApp.Views;

namespace ToDoApp;

public partial class App : Application
{
    IHost? _host;

    public App()
    {
        // This line is needed to make the previewer happy (the previewer plugin cannot handle the following line).
        if (Design.IsDesignMode) return;

        _host = Host.CreateDefaultBuilder()
                .AddViewModels()
                .AddDbContext()
                .AddRepository().Build();
    }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        _host?.Start();

        IDbContextFactory<TasksDbContext>? contextFactory = _host?.Services.GetRequiredService<IDbContextFactory<TasksDbContext>>();
        if (contextFactory != null)
        {
            using (TasksDbContext context = contextFactory.CreateDbContext())
            {
                context.Database.Migrate();
            }
        }

        MainViewModel mainViewModel = _host?.Services.GetRequiredService<MainViewModel>() ?? new MainViewModel();

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
