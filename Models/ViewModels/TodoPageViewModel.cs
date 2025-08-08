using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Views;

namespace TaskFlow.Models.ViewModels
{
    partial class TodoPageViewModel:ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<UserTask> userTasks = new();

        [ObservableProperty]
        private UserTask userTask = new();

        [RelayCommand]
        private async Task Add()
        {
            UserTasks.Add(UserTask);
            var newTask = UserTask;
            UserTask = new();

            Console.WriteLine("========= UserTask ========");
            Console.WriteLine(UserTask);

            // Navigate to the detail page using Shell navigation
            await Shell.Current.GoToAsync($"userTaskDetailPage", true, new Dictionary<string, object>
            {
                { "UserTask", newTask }
            });
        }
    }
}
