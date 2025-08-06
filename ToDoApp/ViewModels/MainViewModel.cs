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

namespace ToDoApp.ViewModels;

public class MainViewModel : ViewModelBase
{
    ITaskService _taskService;

    ObservableCollection<TaskViewModel> _tasks;

    public MainViewModel(ITaskService taskService)
    {
        _taskService = taskService;
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
    }

    private async Task ExecuteAddTaskCommand()
    {
        var result = await ShowDialog.Handle(new TaskEditorViewModel());
        if (result != null)
        {
            _tasks.Add(result);
            await _taskService.Create(result.GetTaskItem());
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
            }
        }
    }

    private async Task ExecuteRemoveTaskCommand(TaskViewModel? item)
    {
        if (item != null)
        {
            _tasks.Remove(item);
            await _taskService.Remove(item.GetTaskItem());
        }
    }

    private async Task ExecuteCompleteTaskCommand(TaskViewModel? item)
    {
        if (item != null)
        {
            await _taskService.Complete(item.GetTaskItem());
            await LoadTaskCommand.Execute();
        }
    }

    private async Task ExecuteUncompleteTaskCommand(TaskViewModel? item)
    {
        if (item != null)
        {
            await _taskService.Uncomplete(item.GetTaskItem());
            await LoadTaskCommand.Execute();
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
