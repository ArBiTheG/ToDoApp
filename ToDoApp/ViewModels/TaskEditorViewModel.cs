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
        TaskItem _taskItem;
        public TaskEditorViewModel(TaskItem taskItem)
        {
            TaskItem = taskItem;
            SubmitCommand = ReactiveCommand.Create<bool, TaskItem?>(ExecuteSubmitCommand);

        }

        private TaskItem? ExecuteSubmitCommand(bool args)
        {
            if (args)
                return TaskItem;
            return null;
        }

        public TaskItem TaskItem 
        { 
            get => _taskItem; 
            set => this.RaiseAndSetIfChanged(ref _taskItem, value); 
        }
        public ReactiveCommand<bool, TaskItem?> SubmitCommand { get; }
    }
}
