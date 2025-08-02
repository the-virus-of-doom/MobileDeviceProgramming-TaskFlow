using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using System.Collections.ObjectModel;
using System.Linq;

namespace TaskFlow
{
    public partial class NotesPage : ContentPage
    {
        public ObservableCollection<NoteModel> Notes { get; set; } = new();

        public NotesPage()
        {
            InitializeComponent();
            BindingContext = this;
        }
        // Display notes logic
        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadNotes();
        }

        private void LoadNotes()
        {
            Notes.Clear();
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
                        noteModels.Add(new NoteModel
                        {
                            Title = parts[0],
                            Content = parts[1],
                            Timestamp = parts[2],
                            IsStarred = parts.Length > 3 && bool.TryParse(parts[3], out var starred) ? starred : false
                        });
                    }
                }
                // Sort notes by starred status and timestamp
                var sorted = noteModels
                    .OrderByDescending(n => n.IsStarred)
                    .ThenBy(n =>
                    {
                        DateTime.TryParse(n.Timestamp, out var dt);
                        return dt;
                    });

                foreach (var note in sorted)
                    Notes.Add(note);
            }
        }
        //Add note button logic
        private async void OnAddNoteClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CreateNotePage());
        }
        // Note selection logic
        private async void OnNoteSelected(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is NoteModel selectedNote)
            {
                var editNote = new EditNotePage.Note
                {
                    Title = selectedNote.Title,
                    Content = selectedNote.Content,
                    Timestamp = selectedNote.Timestamp,
                    IsStarred = selectedNote.IsStarred
                };
                await Navigation.PushAsync(new EditNotePage(editNote));
            }
            if (sender is CollectionView cv)
                cv.SelectedItem = null;
        }
        // Note tap logic
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

        private async void OnDeleteSwipeInvoked(object sender, EventArgs e)
        {
            if (sender is SwipeItem swipeItem && swipeItem.CommandParameter is NoteModel note)
            {
                bool confirm = await DisplayAlert("Delete Note", $"Are you sure you want to delete \"{note.Title}\"?", "Yes", "No");
                if (confirm)
                {
                    var email = Preferences.Get("CurrentUserEmail", "");
                    var noteKey = $"notes_{email}";
                    var notes = Preferences.Get(noteKey, "");
                    var noteList = string.IsNullOrEmpty(notes) ? new List<string>() : new List<string>(notes.Split('~'));

                    noteList.RemoveAll(n =>
                    {
                        var parts = n.Split('|');
                        return parts.Length == 3 && parts[0] == note.Title && parts[1] == note.Content && parts[2] == note.Timestamp;
                    });

                    Preferences.Set(noteKey, string.Join("~", noteList));
                    Notes.Remove(note);
                }
            }
        }

        private void OnStarSwipeInvoked(object sender, EventArgs e)
        {
            if (sender is SwipeItem swipeItem && swipeItem.CommandParameter is NoteModel note)
            {
                // Toggle star
                note.IsStarred = !note.IsStarred;

                // Update storage
                var email = Preferences.Get("CurrentUserEmail", "");
                var noteKey = $"notes_{email}";
                var notes = Preferences.Get(noteKey, "");
                var noteList = string.IsNullOrEmpty(notes) ? new List<string>() : new List<string>(notes.Split('~'));

                for (int i = 0; i < noteList.Count; i++)
                {
                    var parts = noteList[i].Split('|');
                    if (parts.Length >= 3 && parts[0] == note.Title && parts[1] == note.Content && parts[2] == note.Timestamp)
                    {
                        noteList[i] = $"{note.Title}|{note.Content}|{note.Timestamp}|{note.IsStarred}";
                        break;
                    }
                }
                Preferences.Set(noteKey, string.Join("~", noteList));

                // Refresh the list to update UI
                LoadNotes();
            }
        }
    }

    public class NoteModel
    {
        public required string Title { get; set; }
        public required string Content { get; set; }
        public required string Timestamp { get; set; }
        public bool IsStarred { get; set; }
    }

    public class Note
    {
        public required string Title { get; set; }
        public required string Content { get; set; }
        public required DateTime Timestamp { get; set; }
    }
}