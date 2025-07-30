using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using ToDoApp.Models;
using ToDoApp.Repositories;

namespace ToDoApp.ViewModels;

public class MainViewModel : ViewModelBase
{
    ITaskRepository _taskRepository;

    ObservableCollection<TaskItem> _tasks;

    public MainViewModel(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
        _tasks = new ObservableCollection<TaskItem>();
        AddTaskCommand = ReactiveCommand.CreateFromTask(ExecuteAddTaskCommand);
        ShowDialog = new Interaction<TaskEditorViewModel, TaskItem?>();
    }
    private async Task ExecuteAddTaskCommand()
    {
        var vm = new TaskEditorViewModel(new TaskItem());

        var result = await ShowDialog.Handle(vm);
        if (result != null)
        {
            Tasks?.Add(result);
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
    public ReactiveCommand<Unit, Unit> AddTaskCommand { get; }
    public Interaction<TaskEditorViewModel, TaskItem?> ShowDialog { get; }

}
