using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoApp.Models
{
    public class Task
    {
        public Task()
        {
            Name = string.Empty;
            Description = string.Empty;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateTime { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CompletedDateTime { get; set; }
    }
}
