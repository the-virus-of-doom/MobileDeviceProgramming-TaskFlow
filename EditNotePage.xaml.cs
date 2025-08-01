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
        }

        private Note _note;
        private string _originalTitle;
        private string _originalContent;

        public EditNotePage(Note note)
        {
            InitializeComponent();
            _note = note;
            _originalTitle = note.Title;
            _originalContent = note.Content;
            TitleEntry.Text = note.Title;
            ContentEditor.Text = note.Content;
        }
        //Save button logic
        private async void OnSaveClicked(object sender, EventArgs e)
        {
            var email = Preferences.Get("CurrentUserEmail", "");
            var noteKey = $"notes_{email}";
            var notes = Preferences.Get(noteKey, "");
            var noteList = string.IsNullOrEmpty(notes) ? new List<string>() : new List<string>(notes.Split('~'));

            // Find and update the note
            for (int i = 0; i < noteList.Count; i++)
            {
                var parts = noteList[i].Split('|');
                if (parts.Length == 3 && parts[0] == _originalTitle && parts[1] == _originalContent)
                {
                    var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    noteList[i] = $"{TitleEntry.Text}|{ContentEditor.Text}|{timestamp}";
                    break;
                }
            }

            Preferences.Set(noteKey, string.Join("~", noteList));
            await Navigation.PopAsync();
        }
        // Delete button logic
        private async void OnDeleteClicked(object sender, EventArgs e)
        {
            var email = Preferences.Get("CurrentUserEmail", "");
            var noteKey = $"notes_{email}";
            var notes = Preferences.Get(noteKey, "");
            var noteList = string.IsNullOrEmpty(notes) ? new List<string>() : new List<string>(notes.Split('~'));

            // Find and remove the note
            noteList.RemoveAll(n =>
            {
                var parts = n.Split('|');
                return parts.Length == 3 && parts[0] == _originalTitle && parts[1] == _originalContent;
            });

            Preferences.Set(noteKey, string.Join("~", noteList));
            await Navigation.PopAsync();
        }
    }
}