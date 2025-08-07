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
using ToDoApp.Views;

namespace ToDoApp.ViewModels;

public class MainViewModel : ViewModelBase
{
    private ITaskRepository _taskRepository;
    private readonly ILogger<MainViewModel> _logger;

    ObservableCollection<TaskViewModel> _tasks;

    public MainViewModel(ITaskRepository taskService, ILoggerFactory loggerFactory)
    {
        _taskRepository = taskService;
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
        var items = await _taskRepository.GetAll(t => t.IsCompleted==false);
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
            await _taskRepository.Create(result.GetTaskItem());
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
                await _taskRepository.Update(item.GetTaskItem());
                _logger.LogInformation($"Task id:{item.Id} has been edited!");
            }
        }
    }

    private async Task ExecuteRemoveTaskCommand(TaskViewModel? item)
    {
        if (item != null)
        {
            _tasks.Remove(item);
            await _taskRepository.Delete(item.GetTaskItem());
            _logger.LogInformation($"Task id:{item.Id} has been removed!");
        }
    }

    private async Task ExecuteCompleteTaskCommand(TaskViewModel? item)
    {
        if (item != null)
        {
            var model = item.GetTaskItem();
            model.IsCompleted = true;
            model.CompletedDateTime = DateTime.Now;

            _tasks.Remove(item);
            await _taskRepository.Update(model);
            await LoadTaskCommand.Execute();
            _logger.LogInformation($"Task id:{item.Id} has been completed!");
        }
    }

    private async Task ExecuteUncompleteTaskCommand(TaskViewModel? item)
    {
        if (item != null)
        {
            var model = item.GetTaskItem();
            model.IsCompleted = false;

            await _taskRepository.Update(model);
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
