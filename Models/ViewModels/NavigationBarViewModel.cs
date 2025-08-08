using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace TaskFlow.ViewModels
{
    public class NavigationBarViewModel
    {
        public ICommand HomeCommand { get; }
        public ICommand NotesCommand { get; }
        public ICommand TodoCommand { get; }
        public ICommand CalendarCommand { get; }

        public NavigationBarViewModel()
        {
            HomeCommand = new Command(OnHome);
            NotesCommand = new Command(OnNotes);
            TodoCommand = new Command(OnTodo);
            CalendarCommand = new Command(OnCalendar);
        }

        private async void OnHome()
        {
            await Shell.Current.GoToAsync(nameof(HomePage), animate: true);
        }

        private async void OnNotes()
        {
            await Shell.Current.GoToAsync(nameof(NotesPage), animate: false);
        }
        private async void OnTodo()
        {
            await Shell.Current.GoToAsync(nameof(TodoPage), animate: false);
        }
        private async void OnCalendar()
        {
            // TODO: wire up calendar to bottom nav bar
            //await Shell.Current.GoToAsync(nameof(CalendarPage), animate: false);
        }
    }
}