using System.Windows.Input;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;

namespace TaskFlow.ViewModels
{
    public class EditNoteViewModel : BindableObject
    {
        private string _title;
        private string _content;
        private bool _isStarred;
        private string _timestamp;
        private readonly string _originalTitle;
        private readonly string _originalContent;
        private readonly INavigation _navigation;

        public EditNoteViewModel(NoteModel note, INavigation navigation)
        {
            _title = note.Title;
            _content = note.Content;
            _isStarred = note.IsStarred;
            _timestamp = note.Timestamp;
            _originalTitle = note.Title;
            _originalContent = note.Content;
            _navigation = navigation;

            SaveCommand = new Command(async () => await SaveAsync());
            DeleteCommand = new Command(async () => await DeleteAsync());
            StarCommand = new Command(ToggleStar);
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

        public bool IsStarred
        {
            get => _isStarred;
            set { _isStarred = value; OnPropertyChanged(); }
        }

        public string Timestamp
        {
            get => _timestamp;
            set { _timestamp = value; OnPropertyChanged(); }
        }

        public ICommand SaveCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand StarCommand { get; }
        public ICommand BackCommand { get; }

        private async Task SaveAsync()
        {
            var email = Preferences.Get("CurrentUserEmail", "");
            var noteKey = $"notes_{email}";
            var notes = Preferences.Get(noteKey, "");
            var noteList = string.IsNullOrEmpty(notes) ? new List<string>() : new List<string>(notes.Split('~'));

            string updatedTimestamp = DateTime.Now.ToString("MM-dd-yyyy h:mm tt");
            for (int i = 0; i < noteList.Count; i++)
            {
                var parts = noteList[i].Split('|');
                if (parts.Length >= 3 && parts[0] == _originalTitle && parts[1] == _originalContent)
                {
                    noteList[i] = $"{Title}|{Content}|{updatedTimestamp}|{IsStarred}";
                    break;
                }
            }

            Preferences.Set(noteKey, string.Join("~", noteList));

            var updatedNote = new NoteModel
            {
                Title = this.Title,
                Content = this.Content,
                Timestamp = updatedTimestamp,
                IsStarred = this.IsStarred
            };

            // Navigate to ViewNotePage with the updated note
            await Shell.Current.GoToAsync(nameof(ViewNotePage), new Dictionary<string, object>
            {
                ["Note"] = updatedNote
            });
        }

        private async Task DeleteAsync()
        {
            var mainPage = Application.Current?.Windows.FirstOrDefault()?.Page;
            if (mainPage == null)
                return;

            bool confirm = await mainPage.DisplayAlert("Delete Note", "Are you sure you want to delete this note?", "Yes", "No");
            if (!confirm)
                return;

            var email = Preferences.Get("CurrentUserEmail", "");
            var noteKey = $"notes_{email}";
            var notes = Preferences.Get(noteKey, "");
            var noteList = string.IsNullOrEmpty(notes) ? new List<string>() : new List<string>(notes.Split('~'));

            noteList.RemoveAll(n =>
            {
                var parts = n.Split('|');
                return parts.Length >= 3 && parts[0] == _originalTitle && parts[1] == _originalContent;
            });

            Preferences.Set(noteKey, string.Join("~", noteList));

            // Navigate to NotesPage
            await Shell.Current.GoToAsync("NotesPage", false);
        }

        private void ToggleStar()
        {
            IsStarred = !IsStarred;
        }

        private async Task BackAsync()
        {
            var mainPage = Application.Current?.Windows.FirstOrDefault()?.Page;
            if (mainPage == null)
                return;

            bool confirm = await mainPage.DisplayAlert(
                "Discard Changes?",
                "Your edits will not be saved. Do you want to go back?",
                "Yes, Go Back",
                "Cancel"
            );
            if (confirm)
            {
                await _navigation.PopAsync(animated: false);
            }
        }
    }
}