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

    ObservableCollection<TaskItem> _tasks;

    public MainViewModel(ITaskService taskService)
    {
        _taskService = taskService;
        _tasks = new ObservableCollection<TaskItem>();
        LoadTaskCommand = ReactiveCommand.CreateFromTask(ExecuteLoadTaskCommand);
        AddTaskCommand = ReactiveCommand.CreateFromTask(ExecuteAddTaskCommand);
        EditTaskCommand = ReactiveCommand.CreateFromTask<TaskItem?>(ExecuteEditTaskCommand);
        RemoveTaskCommand = ReactiveCommand.CreateFromTask<TaskItem?>(ExecuteRemoveTaskCommand);
        CompleteTaskCommand = ReactiveCommand.CreateFromTask<TaskItem?>(ExecuteCompleteTaskCommand);
        UncompleteTaskCommand = ReactiveCommand.CreateFromTask<TaskItem?>(ExecuteUncompleteTaskCommand);
        ShowDialog = new Interaction<TaskEditorViewModel, TaskItem?>();
    }

    private async Task ExecuteLoadTaskCommand()
    {
        Tasks = new ObservableCollection<TaskItem>(await _taskService.GetUncompleteList());
    }

    private async Task ExecuteAddTaskCommand()
    {
        var result = await ShowDialog.Handle(new TaskEditorViewModel(new TaskItem()));
        if (result != null)
        {
            await _taskService.Create(result);
            await LoadTaskCommand.Execute();
        }
    }

    private async Task ExecuteEditTaskCommand(TaskItem? item)
    {
        if (item != null)
        {
            var result = await ShowDialog.Handle(new TaskEditorViewModel((TaskItem)item.Clone()));
            if (result != null)
            {
                await _taskService.Edit(item, result);
                await LoadTaskCommand.Execute();
            }
        }
    }

    private async Task ExecuteRemoveTaskCommand(TaskItem? item)
    {
        if (item != null)
        {
            await _taskService.Remove(item);
            await LoadTaskCommand.Execute();
        }
    }

    private async Task ExecuteCompleteTaskCommand(TaskItem? item)
    {
        if (item != null)
        {
            await _taskService.Complete(item);
            await LoadTaskCommand.Execute();
        }
    }

    private async Task ExecuteUncompleteTaskCommand(TaskItem? item)
    {
        if (item != null)
        {
            await _taskService.Uncomplete(item);
            await LoadTaskCommand.Execute();
        }
    }

    public ObservableCollection<TaskItem> Tasks 
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
    public ReactiveCommand<TaskItem?, Unit> EditTaskCommand { get; }
    public ReactiveCommand<TaskItem?, Unit> RemoveTaskCommand { get; }
    public ReactiveCommand<TaskItem?, Unit> CompleteTaskCommand { get; }
    public ReactiveCommand<TaskItem?, Unit> UncompleteTaskCommand { get; }
    public Interaction<TaskEditorViewModel, TaskItem?> ShowDialog { get; }

}
