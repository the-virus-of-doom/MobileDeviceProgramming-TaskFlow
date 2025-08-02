using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Maui.Controls.Xaml;

namespace TaskFlow
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        public ObservableCollection<NoteModel> StarredNotes { get; set; } = new();
        // Home Page logic
        public HomePage()
        {
            InitializeComponent();
            BindingContext = this;

            var email = Preferences.Get("CurrentUserEmail", "");
            var userData = Preferences.Get($"user_{email}", "");
            var parts = userData.Split('|');
            if (parts.Length >= 2)
            {
                if (FindByName("UserNameLabel") is Label userNameLabel)
                {
                    userNameLabel.Text = $"Welcome, {parts[0]} {parts[1]}!";
                }
            }
            else
            {
                if (FindByName("UserNameLabel") is Label userNameLabel)
                {
                    userNameLabel.Text = "Welcome";
                }
            }

            LoadStarredNotes();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadStarredNotes();
        }
        // Load starred notes logic
        private void LoadStarredNotes()
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

                // Sort starred notes by timestamp
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
        // Starred note logic
        private async void StarredNotesCollection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is NoteModel selectedNote)
            {
                await Shell.Current.GoToAsync(nameof(ViewNotePage), new Dictionary<string, object>
                {
                    ["Note"] = selectedNote
                });
                ((CollectionView)sender).SelectedItem = null;
            }
        }
        // Tapped note logic
        private async void OnNoteTapped(object sender, EventArgs e)
        {
            var border = sender as Border;
            if (border != null)
            {
                var noteModel = border.BindingContext as NoteModel;
                if (noteModel != null)
                {
                    await Shell.Current.GoToAsync(nameof(ViewNotePage), new Dictionary<string, object>
                    {
                        ["Note"] = noteModel
                    });
                }
            }
        }

        // Sign out button logic
        private async void OnSignOutClicked(object sender, EventArgs e)
        {
            await Task.Run(() =>
            {
                Preferences.Set("IsSignedIn", false);
                Preferences.Remove("CurrentUserEmail");
            });

            var currentWindow = Application.Current?.Windows.FirstOrDefault();
            if (currentWindow != null)
            {
                currentWindow.Page = new NavigationPage(new LoginPage());
            }
        }

        private async void OnUserButtonClicked(object sender, EventArgs e)
        {
            // Example: Navigate to a user profile page or show a dialog
            await DisplayAlert("User", "User button clicked!", "OK");
        }
    }
}