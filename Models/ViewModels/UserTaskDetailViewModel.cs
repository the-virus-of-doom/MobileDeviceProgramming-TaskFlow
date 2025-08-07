using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Models.ViewModels
{
    partial class UserTaskDetailViewModel : ObservableObject
    {
        [ObservableProperty]
        private UserTask userTask;
    }
}
