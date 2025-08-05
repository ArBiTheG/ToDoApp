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
            _taskItem = taskItem;
            SubmitCommand = ReactiveCommand.Create<bool, TaskItem?>(ExecuteSubmitCommand);

        }

        public string Name 
        { 
            get => _taskItem.Name;
            set {
                _taskItem.Name = value;
                this.RaisePropertyChanged(nameof(Name));
            }
        }
        public string Description 
        { 
            get => _taskItem.Description;
            set
            {
                _taskItem.Description = value;
                this.RaisePropertyChanged(nameof(Description));
            }
        }
        public bool IsDeadline 
        { 
            get => _taskItem.IsDeadline; 
            set
            {
                _taskItem.IsDeadline = value;
                this.RaisePropertyChanged(nameof(IsDeadline));
            } 
        }
        public DateTimeOffset DeadlineDate
        {
            get => new DateTimeOffset(_taskItem.DeadlineDateTime);
            set
            {
                _taskItem.DeadlineDateTime = value.DateTime;
                this.RaisePropertyChanged(nameof(DeadlineDate));
            }
        }
        public TimeSpan DeadlineTime
        {
            get => _taskItem.DeadlineDateTime.TimeOfDay;
            set
            {
                var dateTime = _taskItem.DeadlineDateTime.Date;
                _taskItem.DeadlineDateTime = dateTime.Add(value);
                this.RaisePropertyChanged(nameof(DeadlineTime));
            }
        }

        private TaskItem? ExecuteSubmitCommand(bool args)
        {
            if (args)
                return _taskItem;
            return null;
        }

        public ReactiveCommand<bool, TaskItem?> SubmitCommand { get; }
    }
}
