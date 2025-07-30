using Avalonia.Controls.Shapes;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoApp.Models;

namespace ToDoApp.ViewModels
{
    public class TaskEditorViewModel: ViewModelBase
    {
        public TaskEditorViewModel(TaskItem taskItem)
        {
            TaskItem = taskItem;
            ApplyCommand = ReactiveCommand.Create<bool,TaskItem?>(ExecuteApplyCommand);
        }
        private TaskItem?  ExecuteApplyCommand(bool arg)
        {
            if (arg)
                return TaskItem;
            return null;
        }
        TaskItem TaskItem { get; set; }
        public ReactiveCommand<bool, TaskItem?> ApplyCommand { get; }
    }
}
