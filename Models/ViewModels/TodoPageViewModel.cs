using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Models.ViewModels
{
    partial class TodoPageViewModel:ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<UserTask> userTasks = new();

        [ObservableProperty]
        private UserTask userTask = new();

        [RelayCommand]
        private void Add()
        {
            UserTasks.Add(UserTask);
            UserTask = new();
        }
    }
}
