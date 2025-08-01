using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using System.Collections.ObjectModel;

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
                foreach (var note in noteList)
                {
                    var parts = note.Split('|');
                    if (parts.Length == 3)
                    {
                        Notes.Add(new NoteModel
                        {
                            Title = parts[0],
                            Content = parts[1],
                            Timestamp = parts[2]
                        });
                    }
                }
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
                    Content = selectedNote.Content
                };
                await Navigation.PushAsync(new EditNotePage(editNote));
            }
            if (sender is CollectionView cv)
                cv.SelectedItem = null;
        }

        private async void OnNoteTapped(object sender, EventArgs e)
        {
            var border = sender as Border;
            if (border != null)
            {
                var noteModel = border.BindingContext as NoteModel;
                if (noteModel != null)
                {
                    var editNote = new EditNotePage.Note
                    {
                        Title = noteModel.Title,
                        Content = noteModel.Content,
                        Timestamp = noteModel.Timestamp
                    };
                    await Navigation.PushAsync(new EditNotePage(editNote));
                }
            }
        }
    }

    public class NoteModel
    {
        public required string Title { get; set; }
        public required string Content { get; set; }
        public required string Timestamp { get; set; }
    }

    public class Note
    {
        public required string Title { get; set; }
        public required string Content { get; set; }
        public required DateTime Timestamp { get; set; }
    }
}