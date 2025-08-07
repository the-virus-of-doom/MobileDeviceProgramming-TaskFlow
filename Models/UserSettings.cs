using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Models
{
    internal class UserSettings
    {
        public bool InAppNotifications { get; set; }
        public bool Notifications { get; set; }
        public bool LocationServices { get; set; }
    }
}
