using Avalonia;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using ToDoApp.Models;

namespace ToDoApp.ViewModels
{
    public class TaskViewModel: ViewModelBase, ICloneable
    {
        TaskItem _taskItem;

        public TaskViewModel()
        {
            _taskItem = new TaskItem()
            {
                Name = "Новая задача"
            };
        }
        public TaskViewModel(TaskItem taskItem)
        {
            _taskItem = taskItem;
        }

        public int Id
        {
            get => _taskItem.Id;
            set
            {
                _taskItem.Id = value;
                this.RaisePropertyChanged();
            }
        }
        public string Name
        {
            get => _taskItem.Name;
            set
            {
                _taskItem.Name = value;
                this.RaisePropertyChanged();
            }
        }
        public string Description
        {
            get => _taskItem.Description;
            set
            {
                _taskItem.Description = value;
                this.RaisePropertyChanged();
            }
        }
        public bool IsDeadline
        {
            get => _taskItem.IsDeadline;
            set
            {
                _taskItem.IsDeadline = value;
                this.RaisePropertyChanged();
            }
        }
        public DateTimeOffset DeadlineDate
        {
            get => _taskItem.DeadlineDateTime;
            set
            {
                var time = DeadlineTime;
                var date = value;
                _taskItem.DeadlineDateTime = 
                    new DateTime(date.Year, date.Month, date.Day, time.Hours, time.Minutes, time.Seconds); ;
                this.RaisePropertyChanged();
            }
        }
        public TimeSpan DeadlineTime
        {
            get => _taskItem.DeadlineDateTime.TimeOfDay;
            set
            {
                var time = value;
                var date = DeadlineDate;
                _taskItem.DeadlineDateTime =
                    new DateTime(date.Year, date.Month, date.Day, time.Hours, time.Minutes, time.Seconds); ;
                this.RaisePropertyChanged();
            }
        }

        public object Clone()
        {
            return new TaskViewModel((TaskItem)_taskItem.Clone());
        }

        public void ApplyChanges(TaskViewModel taskViewModel)
        {
            Name = taskViewModel.Name;
            Description = taskViewModel.Description;
            IsDeadline = taskViewModel.IsDeadline;
            DeadlineDate = taskViewModel.DeadlineDate;
            DeadlineTime = taskViewModel.DeadlineTime;
        }

        public TaskItem GetTaskItem()
        {
            return _taskItem;
        }
    }
}
