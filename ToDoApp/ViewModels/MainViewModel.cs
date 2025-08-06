using Microsoft.Extensions.Logging;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using ToDoApp.Models;
using ToDoApp.Repositories;
using ToDoApp.Services;
using ToDoApp.Views;

namespace ToDoApp.ViewModels;

public class MainViewModel : ViewModelBase
{
    private ITaskService _taskService;
    private readonly ILogger<MainViewModel> _logger;

    ObservableCollection<TaskViewModel> _tasks;

    public MainViewModel(ITaskService taskService, ILoggerFactory loggerFactory)
    {
        _taskService = taskService;
        _logger = loggerFactory.CreateLogger<MainViewModel>();

        _tasks = new ObservableCollection<TaskViewModel>();
        LoadTaskCommand = ReactiveCommand.CreateFromTask(ExecuteLoadTaskCommand);
        AddTaskCommand = ReactiveCommand.CreateFromTask(ExecuteAddTaskCommand);
        EditTaskCommand = ReactiveCommand.CreateFromTask<TaskViewModel?>(ExecuteEditTaskCommand);
        RemoveTaskCommand = ReactiveCommand.CreateFromTask<TaskViewModel?>(ExecuteRemoveTaskCommand);
        CompleteTaskCommand = ReactiveCommand.CreateFromTask<TaskViewModel?>(ExecuteCompleteTaskCommand);
        UncompleteTaskCommand = ReactiveCommand.CreateFromTask<TaskViewModel?>(ExecuteUncompleteTaskCommand);
        ShowDialog = new Interaction<TaskEditorViewModel, TaskViewModel?>();
    }

    private async Task ExecuteLoadTaskCommand()
    {
        var items = await _taskService.GetUncompleteList();
        foreach (var taskItem in items)
        {
            Tasks.Add(new TaskViewModel(taskItem));
        }
        _logger.LogInformation("Tasks has been loaded!");
    }

    private async Task ExecuteAddTaskCommand()
    {
        var result = await ShowDialog.Handle(new TaskEditorViewModel());
        if (result != null)
        {
            _tasks.Add(result);
            await _taskService.Create(result.GetTaskItem());
            _logger.LogInformation($"Task id:{result.Id} has been created!");
        }
    }

    private async Task ExecuteEditTaskCommand(TaskViewModel? item)
    {
        if (item != null)
        {
            var result = await ShowDialog.Handle(new TaskEditorViewModel((TaskViewModel)item.Clone()));
            if (result != null)
            {
                item.ApplyChanges(result);
                await _taskService.Edit(item.GetTaskItem());
                _logger.LogInformation($"Task id:{item.Id} has been edited!");
            }
        }
    }

    private async Task ExecuteRemoveTaskCommand(TaskViewModel? item)
    {
        if (item != null)
        {
            _tasks.Remove(item);
            await _taskService.Remove(item.GetTaskItem());
            _logger.LogInformation($"Task id:{item.Id} has been removed!");
        }
    }

    private async Task ExecuteCompleteTaskCommand(TaskViewModel? item)
    {
        if (item != null)
        {
            await _taskService.Complete(item.GetTaskItem());
            await LoadTaskCommand.Execute();
            _logger.LogInformation($"Task id:{item.Id} has been completed!");
        }
    }

    private async Task ExecuteUncompleteTaskCommand(TaskViewModel? item)
    {
        if (item != null)
        {
            await _taskService.Uncomplete(item.GetTaskItem());
            await LoadTaskCommand.Execute();
            _logger.LogInformation($"Task id:{item.Id} has been uncompleted!");
        }
    }

    public ObservableCollection<TaskViewModel> Tasks 
    { 
        get 
        { 
            return _tasks;
        } 
        set
        {
            this.RaiseAndSetIfChanged(ref _tasks, value);
        }
    }
    public ReactiveCommand<Unit, Unit> LoadTaskCommand { get; }
    public ReactiveCommand<Unit, Unit> AddTaskCommand { get; }
    public ReactiveCommand<TaskViewModel?, Unit> EditTaskCommand { get; }
    public ReactiveCommand<TaskViewModel?, Unit> RemoveTaskCommand { get; }
    public ReactiveCommand<TaskViewModel?, Unit> CompleteTaskCommand { get; }
    public ReactiveCommand<TaskViewModel?, Unit> UncompleteTaskCommand { get; }
    public Interaction<TaskEditorViewModel, TaskViewModel?> ShowDialog { get; }

}
