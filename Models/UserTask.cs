using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Models
{
    public class UserTask
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DueDateTime { get; set; }
        public bool IsCompleted { get; set; }

        public UserTask(string name = "New Task", string description = "default description", DateTime dueDateTime = new DateTime(), bool isCompleted  = false) {
            Name = name;
            Description = description;
            IsCompleted = isCompleted;
            
            // set due date to Now if no dueDateTime is given
            if (dueDateTime == new DateTime())
            {
                DueDateTime = DateTime.Now;
            }
            else
            {
                DueDateTime = dueDateTime;
            }
        }
    }
}
