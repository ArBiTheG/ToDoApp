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
        EditTaskCommand = ReactiveCommand.CreateFromTask<int>(ExecuteEditTaskCommand);
        RemoveTaskCommand = ReactiveCommand.CreateFromTask<int>(ExecuteRemoveTaskCommand);
        CompleteTaskCommand = ReactiveCommand.CreateFromTask<int>(ExecuteCompleteTaskCommand);
        UncompleteTaskCommand = ReactiveCommand.CreateFromTask<int>(ExecuteUncompleteTaskCommand);
        ShowDialog = new Interaction<TaskEditorViewModel, TaskItem?>();
    }

    private async Task ExecuteLoadTaskCommand()
    {
        Tasks = new ObservableCollection<TaskItem>(await _taskService.GetUncompleteList());
    }

    private async Task ExecuteAddTaskCommand()
    {
        var vm = new TaskEditorViewModel(new TaskItem());

        var result = await ShowDialog.Handle(vm);
        if (result != null)
        {
            await _taskService.Create(result);
            await LoadTaskCommand.Execute();
        }
    }

    private async Task ExecuteEditTaskCommand(int id)
    {
        TaskItem? item = await _taskService.Get(id);
        if (item != null)
        {
            var vm = new TaskEditorViewModel((TaskItem)item.Clone());

            var result = await ShowDialog.Handle(vm);
            if (result != null)
            {
                await _taskService.Edit(item, result);
                await LoadTaskCommand.Execute();
            }
        }
    }

    private async Task ExecuteRemoveTaskCommand(int id)
    {
        TaskItem? item = await _taskService.Get(id);
        if (item != null)
        {
            await _taskService.Remove(item);
            await LoadTaskCommand.Execute();
        }
    }

    private async Task ExecuteCompleteTaskCommand(int id)
    {
        TaskItem? item = await _taskService.Get(id);
        if (item != null)
        {
            await _taskService.Complete(item);
            await LoadTaskCommand.Execute();
        }
    }

    private async Task ExecuteUncompleteTaskCommand(int id)
    {
        TaskItem? item = await _taskService.Get(id);
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
    public ReactiveCommand<int, Unit> EditTaskCommand { get; }
    public ReactiveCommand<int, Unit> RemoveTaskCommand { get; }
    public ReactiveCommand<int, Unit> CompleteTaskCommand { get; }
    public ReactiveCommand<int, Unit> UncompleteTaskCommand { get; }
    public Interaction<TaskEditorViewModel, TaskItem?> ShowDialog { get; }

}
