using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Models
{
    [Table("userTask")]
    public class UserTask
    {
        [PrimaryKey, AutoIncrement]
        [Column("id")]
        public int Id { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("description")]
        public string Description { get; set; }
        [Column("due_datetime")]
        public DateTime DueDateTime { get; set; }
        [Column("is_completed")]
        public bool IsCompleted { get; set; }

        public UserTask()
        {
            Name = "New Task";
            Description = "default description";
            DueDateTime = DateTime.Now;
            IsCompleted = false;
        }

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
