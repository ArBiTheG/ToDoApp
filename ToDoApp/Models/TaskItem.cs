using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoApp.Models
{
    public class TaskItem: IEntity, ICloneable, IEquatable<TaskItem?>
    {
        public TaskItem()
        {
            Name = string.Empty;
            Description = string.Empty;
        }

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateTime { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CompletedDateTime { get; set; }

        public object Clone()
        {
            return new TaskItem()
            {
                Id = Id,
                Name = Name,
                Description = Description,
                DateTime = DateTime,
                IsCompleted = IsCompleted,
                CompletedDateTime = CompletedDateTime,
            };
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as TaskItem);
        }

        public bool Equals(TaskItem? other)
        {
            return other is not null &&
                   Id == other.Id &&
                   Name == other.Name &&
                   Description == other.Description &&
                   DateTime == other.DateTime &&
                   IsCompleted == other.IsCompleted &&
                   CompletedDateTime == other.CompletedDateTime;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name, Description, DateTime, IsCompleted, CompletedDateTime);
        }

        public static bool operator ==(TaskItem? left, TaskItem? right)
        {
            return EqualityComparer<TaskItem>.Default.Equals(left, right);
        }

        public static bool operator !=(TaskItem? left, TaskItem? right)
        {
            return !(left == right);
        }
    }
}
