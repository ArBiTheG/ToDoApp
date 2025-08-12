using DynamicData.Binding;
using Microsoft.Extensions.Logging;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using ToDoApp.Models;
using ToDoApp.Repositories;
using ToDoApp.Views;

namespace ToDoApp.ViewModels;

public class MainViewModel : ViewModelBase
{
    private ITaskRepository? _taskRepository;
    private readonly ILogger<MainViewModel>? _logger;

    private ObservableCollection<TaskViewModel> _tasks;
    private bool _isBusy;
    private string _searchText;

    public MainViewModel()
    {
        _tasks = new ObservableCollection<TaskViewModel>();
        _isBusy = false;
        _searchText = "";

        this.WhenAnyValue(vm => vm.SearchText)
            .Throttle(TimeSpan.FromMilliseconds(400))
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe(DoSearch!);

        var canExecute = this.WhenAnyValue(vm => vm.IsBusy, isBusy => isBusy != true);
        AddTaskCommand = ReactiveCommand.CreateFromTask(ExecuteAddTaskCommand, canExecute);
        EditTaskCommand = ReactiveCommand.CreateFromTask<TaskViewModel?>(ExecuteEditTaskCommand, canExecute);
        RemoveTaskCommand = ReactiveCommand.CreateFromTask<TaskViewModel?>(ExecuteRemoveTaskCommand, canExecute);
        CompleteTaskCommand = ReactiveCommand.CreateFromTask<TaskViewModel?>(ExecuteCompleteTaskCommand, canExecute);
        UncompleteTaskCommand = ReactiveCommand.CreateFromTask<TaskViewModel?>(ExecuteUncompleteTaskCommand, canExecute);
        ShowDialog = new Interaction<TaskEditorViewModel, TaskViewModel?>();
    }

    public MainViewModel(ITaskRepository taskService, ILoggerFactory loggerFactory) : this()
    {
        _taskRepository = taskService;
        _logger = loggerFactory.CreateLogger<MainViewModel>();
    }

    private async void DoSearch(string str)
    {
        if (!string.IsNullOrWhiteSpace(str) && str.Length>3)
            await LoadTasksList(str, false);
        else
            await LoadTasksList();
    }

    private async Task ExecuteAddTaskCommand()
    {
        if (_taskRepository == null) return;
        IsBusy = true;
        var result = await ShowDialog.Handle(new TaskEditorViewModel());
        if (result != null)
        {
            _tasks.Add(result);
            await _taskRepository.Create(result.GetTaskItem());
            _logger?.LogInformation($"Task id:{result.Id} has been created!");
        }
        IsBusy = false;
    }

    private async Task ExecuteEditTaskCommand(TaskViewModel? itemVM)
    {
        if (_taskRepository == null) return;
        IsBusy = true;
        if (itemVM != null)
        {
            var result = await ShowDialog.Handle(new TaskEditorViewModel((TaskViewModel)itemVM.Clone()));
            if (result != null)
            {
                itemVM.ApplyChanges(result);
                await _taskRepository.Update(itemVM.GetTaskItem());
                _logger?.LogInformation($"Task id:{itemVM.Id} has been edited!");
            }
        }
        IsBusy = false;
    }

    private async Task ExecuteRemoveTaskCommand(TaskViewModel? itemVM)
    {
        if (_taskRepository == null) return;
        IsBusy = true;
        if (itemVM != null)
        {
            _tasks.Remove(itemVM);
            await _taskRepository.Delete(itemVM.GetTaskItem());
            _logger?.LogInformation($"Task id:{itemVM.Id} has been removed!");
        }
        IsBusy = false;
    }

    private async Task ExecuteCompleteTaskCommand(TaskViewModel? itemVM)
    {
        if (_taskRepository == null) return;
        IsBusy = true;
        if (itemVM != null)
        {
            var item = itemVM.GetTaskItem();
            item.IsCompleted = true;
            item.CompletedDateTime = DateTime.Now;

            _tasks.Remove(itemVM);
            await _taskRepository.Update(item);
            _logger?.LogInformation($"Task id:{itemVM.Id} has been completed!");
        }
        IsBusy = false;
    }

    private async Task ExecuteUncompleteTaskCommand(TaskViewModel? itemVM)
    {
        if (_taskRepository == null) return;
        IsBusy = true;
        if (itemVM != null)
        {
            var model = itemVM.GetTaskItem();
            model.IsCompleted = false;

            await _taskRepository.Update(model);
            _logger?.LogInformation($"Task id:{itemVM.Id} has been uncompleted!");
        }
        IsBusy = false;
    }

    private async Task LoadTasksList(string searchText = "", bool isCompleted = false)
    {
        if (_taskRepository == null) return;
        IsBusy = true;
        Tasks.Clear();

        var items = await _taskRepository.GetAll(t => (t.Name.Contains(searchText) || t.Description.Contains(searchText)) &&
                                                      t.IsCompleted == isCompleted);
        foreach (var taskItem in items)
        {
            Tasks.Add(new TaskViewModel(taskItem));
        }
        _logger?.LogInformation("Tasks has been loaded!");
        IsBusy = false;
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
    public bool IsBusy
    {
        get
        {
            return _isBusy;
        }
        set
        {
            this.RaiseAndSetIfChanged(ref _isBusy, value);
        }
    }

    public string SearchText 
    { 
        get => _searchText;
        set => this.RaiseAndSetIfChanged(ref _searchText, value);
    }
    public ReactiveCommand<Unit, Unit> AddTaskCommand { get; }
    public ReactiveCommand<TaskViewModel?, Unit> EditTaskCommand { get; }
    public ReactiveCommand<TaskViewModel?, Unit> RemoveTaskCommand { get; }
    public ReactiveCommand<TaskViewModel?, Unit> CompleteTaskCommand { get; }
    public ReactiveCommand<TaskViewModel?, Unit> UncompleteTaskCommand { get; }
    public Interaction<TaskEditorViewModel, TaskViewModel?> ShowDialog { get; }

}
