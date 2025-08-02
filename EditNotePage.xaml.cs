using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using System;

namespace TaskFlow
{
    public partial class EditNotePage : ContentPage
    {
        public class Note
        {
            public string Title { get; set; } = string.Empty;
            public string Content { get; set; } = string.Empty;
            public string Timestamp { get; set; } = string.Empty;
            public bool IsStarred { get; set; } = false;
        }

        private Note _note;
        private string _originalTitle;
        private string _originalContent;
        // Edit Note Page constructor
        public EditNotePage(Note note)
        {
            InitializeComponent();
            _note = note;
            _originalTitle = note.Title;
            _originalContent = note.Content;
            TitleEntry.Text = note.Title;
            ContentEditor.Text = note.Content;
            StarButton.Source = note.IsStarred ? "star_fill.png" : "star.png";
        }

        //Save button logic
        private async void OnSaveClicked(object sender, EventArgs e)
        {
            var email = Preferences.Get("CurrentUserEmail", "");
            var noteKey = $"notes_{email}";
            var notes = Preferences.Get(noteKey, "");
            var noteList = string.IsNullOrEmpty(notes) ? new List<string>() : new List<string>(notes.Split('~'));

            // Find and update the note
            string updatedTitle = TitleEntry.Text;
            string updatedContent = ContentEditor.Text;
            string updatedTimestamp = DateTime.Now.ToString("MM-dd-yyyy h:mm tt");
            for (int i = 0; i < noteList.Count; i++)
            {
                var parts = noteList[i].Split('|');
                if (parts.Length >= 3 && parts[0] == _originalTitle && parts[1] == _originalContent)
                {
                    noteList[i] = $"{updatedTitle}|{updatedContent}|{updatedTimestamp}|{_note.IsStarred}";
                    break;
                }
            }

            Preferences.Set(noteKey, string.Join("~", noteList));

            // Prepare the updated note for the view page
            var updatedNote = new NoteModel
            {
                Title = updatedTitle,
                Content = updatedContent,
                Timestamp = updatedTimestamp,
                IsStarred = _note.IsStarred
            };

            // Navigate to ViewNotePage with the updated note
            await Shell.Current.GoToAsync(
                $"NotesPage/{nameof(ViewNotePage)}",
                false, 
                new Dictionary<string, object>
                {
                    ["Note"] = updatedNote
                }
            );
        }
        // Delete button logic
        private async void OnDeleteClicked(object sender, EventArgs e)
        {
            bool confirm = await DisplayAlert("Delete Note", "Are you sure you want to delete this note?", "Yes", "No");
            if (!confirm)
                return;

            var email = Preferences.Get("CurrentUserEmail", "");
            var noteKey = $"notes_{email}";
            var notes = Preferences.Get(noteKey, "");
            var noteList = string.IsNullOrEmpty(notes) ? new List<string>() : new List<string>(notes.Split('~'));

            // Find and remove the note
            noteList.RemoveAll(n =>
            {
                var parts = n.Split('|');
                return parts.Length >= 3 && parts[0] == _originalTitle && parts[1] == _originalContent;
            });

            Preferences.Set(noteKey, string.Join("~", noteList));

            // Navigate to NotesPage
            await Shell.Current.GoToAsync("NotesPage", false);
        }
        // Star button logic
        private void OnStarClicked(object sender, EventArgs e)
        {
            _note.IsStarred = !_note.IsStarred;
            StarButton.Source = _note.IsStarred ? "star_fill.png" : "star.png";
        }
        // Back button logic
        private async void OnBackClicked(object sender, EventArgs e)
        {
            bool confirm = await DisplayAlert(
                "Discard Changes?",
                "Your edits will not be saved. Do you want to go back?",
                "Yes, Go Back",
                "Cancel"
            );
            if (confirm)
            {
                await Navigation.PopAsync(animated: false);
            }
        }
    }
}