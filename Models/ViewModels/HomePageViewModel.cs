using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using TaskFlow.Models;

namespace TaskFlow.ViewModels
{
    public class HomePageViewModel : BindableObject
    {
        private string _userName = string.Empty;
        public ObservableCollection<NoteModel> StarredNotes { get; } = new();

        public string UserName
        {
            get => _userName;
            set { _userName = value; OnPropertyChanged(); }
        }

        public ICommand NoteTappedCommand { get; }
        public ICommand SignOutCommand { get; }
        public ICommand UserButtonCommand { get; }

        public HomePageViewModel()
        {
            NoteTappedCommand = new Command<NoteModel>(OnNoteTapped);
            SignOutCommand = new Command(async () => await OnSignOut());
            UserButtonCommand = new Command(async () => await OnUserButton());

            LoadUserName();
            LoadStarredNotes();
        }

        public void LoadUserName()
        {
            var email = Preferences.Get("CurrentUserEmail", "");
            var userData = Preferences.Get($"user_{email}", "");
            var parts = userData.Split('|');
            UserName = parts.Length >= 2 ? $"Welcome, {parts[0]} {parts[1]}!" : "Welcome";
        }

        public void LoadStarredNotes()
        {
            StarredNotes.Clear();
            var email = Preferences.Get("CurrentUserEmail", "");
            var notes = Preferences.Get($"notes_{email}", "");
            if (!string.IsNullOrEmpty(notes))
            {
                var noteList = notes.Split('~');
                var noteModels = new List<NoteModel>();

                foreach (var note in noteList)
                {
                    var parts = note.Split('|');
                    if (parts.Length >= 3)
                    {
                        var isStarred = parts.Length > 3 && bool.TryParse(parts[3], out var starred) ? starred : false;
                        if (isStarred)
                        {
                            noteModels.Add(new NoteModel
                            {
                                Title = parts[0],
                                Content = parts[1],
                                Timestamp = parts[2],
                                IsStarred = isStarred
                            });
                        }
                    }
                }

                var sorted = noteModels
                    .OrderBy(n =>
                    {
                        DateTime.TryParse(n.Timestamp, out var dt);
                        return dt;
                    });

                foreach (var note in sorted)
                    StarredNotes.Add(note);
            }
        }

        private async void OnNoteTapped(NoteModel note)
        {
            if (note != null)
            {
                await Shell.Current.GoToAsync(nameof(ViewNotePage), new Dictionary<string, object>
                {
                    ["Note"] = note
                });
            }
        }

        private Task OnSignOut()
        {
            Preferences.Set("IsSignedIn", false);
            Preferences.Remove("CurrentUserEmail");

            // Switch to LoginPage on sign out
            var currentWindow = Application.Current?.Windows.FirstOrDefault();
            if (currentWindow != null)
            {
                Dispatcher.Dispatch(() =>
                {
                    currentWindow.Page = new NavigationPage(new LoginPage());
                });
            }
            return Task.CompletedTask;
        }

        private async Task OnUserButton()
        {
            var currentWindow = Application.Current?.Windows.FirstOrDefault();
            var currentPage = currentWindow?.Page;
            if (currentPage != null)
            {
                
                await currentPage.DisplayAlert("User", "User button clicked!", "OK");
            }
        }
    }
}