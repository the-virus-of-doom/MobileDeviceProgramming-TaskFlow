using System.Windows.Input;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;

namespace TaskFlow.ViewModels
{
    public class CreateNoteViewModel : BindableObject
    {
        private string _title = string.Empty;
        private string _content = string.Empty;
        private readonly INavigation _navigation;

        public CreateNoteViewModel(INavigation navigation)
        {
            _navigation = navigation;
            SaveCommand = new Command(async () => await SaveAsync());
            BackCommand = new Command(async () => await BackAsync());
        }

        public string Title
        {
            get => _title;
            set { _title = value; OnPropertyChanged(); }
        }

        public string Content
        {
            get => _content;
            set { _content = value; OnPropertyChanged(); }
        }

        public ICommand SaveCommand { get; }
        public ICommand BackCommand { get; }

        private async Task SaveAsync()
        {
            if (string.IsNullOrWhiteSpace(Title) || string.IsNullOrWhiteSpace(Content))
            {
                var currentPage = Application.Current?.Windows[0]?.Page;
                if (currentPage != null)
                {
                    await currentPage.DisplayAlert("Error", "Please enter both a title and content.", "OK");
                }
                return;
            }

            var email = Preferences.Get("CurrentUserEmail", "");
            var noteKey = $"notes_{email}";
            var notes = Preferences.Get(noteKey, "");
            var timestamp = DateTime.Now.ToString("MM-dd-yyyy h:mm tt");
            var newNote = $"{Title}|{Content}|{timestamp}";
            notes = string.IsNullOrEmpty(notes) ? newNote : $"{notes}~{newNote}";
            Preferences.Set(noteKey, notes);

            await _navigation.PopAsync();
        }

        private async Task BackAsync()
        {
            var currentPage = Application.Current?.Windows[0]?.Page;
            if (currentPage != null)
            {
                bool confirm = await currentPage.DisplayAlert(
                    "Discard Changes?",
                    "Your edits will not be saved. Do you want to go back?",
                    "Yes, Go Back",
                    "Cancel"
                );
                if (confirm)
                {
                    await _navigation.PopAsync(animated: true);
                }
            }
        }
    }
}