using Microsoft.Maui.Controls;
using System.Collections.Generic;

namespace TaskFlow
{
    public partial class ViewNotePage : ContentPage, IQueryAttributable
    {
        private NoteModel _noteModel = new NoteModel
        {
            Title = string.Empty,
            Content = string.Empty,
            Timestamp = string.Empty
        };

        public ViewNotePage()
        {
            InitializeComponent();
        }
        // View Note logic
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue("Note", out var noteObj) && noteObj is NoteModel note)
            {
                _noteModel = note;
                TitleLabel.Text = note.Title;
                ContentLabel.Text = note.Content;
                TimestampLabel.Text = note.Timestamp;
                StarButton.Source = note.IsStarred ? "star_fill.png" : "star.png";
            }
        }
        // Edit button logic
        private async void OnEditClicked(object sender, EventArgs e)
        {
            var editNote = new EditNotePage.Note
            {
                Title = _noteModel.Title,
                Content = _noteModel.Content,
                Timestamp = _noteModel.Timestamp,
                IsStarred = _noteModel.IsStarred
            };
            await Navigation.PushAsync(new EditNotePage(editNote));
        }
        // Star button logic
        private void OnStarClicked(object sender, EventArgs e)
        {
            _noteModel.IsStarred = !_noteModel.IsStarred;
            StarButton.Source = _noteModel.IsStarred ? "star_fill.png" : "star.png";
            SaveStarState();
        }
        // Star state saving logic
        private void SaveStarState()
        {
            var email = Preferences.Get("CurrentUserEmail", "");
            var noteKey = $"notes_{email}";
            var notes = Preferences.Get(noteKey, "");
            var noteList = string.IsNullOrEmpty(notes) ? new List<string>() : new List<string>(notes.Split('~'));

            for (int i = 0; i < noteList.Count; i++)
            {
                var parts = noteList[i].Split('|');
                if (parts.Length >= 3 && parts[0] == _noteModel.Title && parts[1] == _noteModel.Content && parts[2] == _noteModel.Timestamp)
                {
                    noteList[i] = $"{_noteModel.Title}|{_noteModel.Content}|{_noteModel.Timestamp}|{_noteModel.IsStarred}";
                    break;
                }
            }
            Preferences.Set(noteKey, string.Join("~", noteList));
        }
        // Back button logic
        private async void OnBackClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("NotesPage", animate: false);
        }
    }
}