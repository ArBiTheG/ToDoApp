using Avalonia.Controls.Shapes;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using ToDoApp.Models;

namespace ToDoApp.ViewModels
{
    public class TaskEditorViewModel: ViewModelBase
    {
        TaskViewModel _taskViewModel;

        public TaskEditorViewModel(): this(new TaskViewModel()) { }
        public TaskEditorViewModel(TaskViewModel taskViewModel)
        {
            TaskViewModel = taskViewModel;
            SubmitCommand = ReactiveCommand.Create<bool, TaskViewModel?>(ExecuteSubmitCommand);
        }

        public TaskViewModel TaskViewModel 
        { 
            get => _taskViewModel; 
            set => this.RaiseAndSetIfChanged(ref _taskViewModel, value); 
        }

        private TaskViewModel? ExecuteSubmitCommand(bool args)
        {
            if (args)
                return TaskViewModel;
            return null;
        }

        public ReactiveCommand<bool, TaskViewModel?> SubmitCommand { get; }
    }
}
