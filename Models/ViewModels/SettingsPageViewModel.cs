using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Models.ViewModels
{
    partial class SettingsPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private UserSettings userSettings;
    }
}
