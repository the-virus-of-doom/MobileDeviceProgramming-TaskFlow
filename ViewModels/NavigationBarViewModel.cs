using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace TaskFlow.ViewModels
{
    public class NavigationBarViewModel
    {
        public ICommand HomeCommand { get; }
        public ICommand NotesCommand { get; }

        public NavigationBarViewModel()
        {
            HomeCommand = new Command(OnHome);
            NotesCommand = new Command(OnNotes);
        }

        private async void OnHome()
        {
            await Shell.Current.GoToAsync("HomePage", animate: false);
        }

        private async void OnNotes()
        {
            await Shell.Current.GoToAsync("NotesPage", animate: false);
        }
    }
}