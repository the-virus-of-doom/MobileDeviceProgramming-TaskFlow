using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using TaskFlow.Models;

namespace TaskFlow.ViewModels
{
    public class NotesPageViewModel : BindableObject
    {
        public ObservableCollection<NoteModel> Notes { get; } = new();

        public ICommand AddNoteCommand { get; }
        public ICommand NoteTappedCommand { get; }
        public ICommand DeleteNoteCommand { get; }
        public ICommand StarNoteCommand { get; }

        public NotesPageViewModel()
        {
            AddNoteCommand = new Command(OnAddNote);
            NoteTappedCommand = new Command<NoteModel>(OnNoteTapped);
            DeleteNoteCommand = new Command<NoteModel>(OnDeleteNote);
            StarNoteCommand = new Command<NoteModel>(OnStarNote);

            LoadNotes();
        }

        public void LoadNotes()
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

        private async void OnAddNote()
        {
            await Shell.Current.GoToAsync(nameof(CreateNotePage));
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

        private async void OnDeleteNote(NoteModel note)
        {
            if (note != null)
            {
                var currentWindow = Application.Current?.Windows.FirstOrDefault();
                if (currentWindow?.Page != null)
                {
                    bool confirm = await currentWindow.Page.DisplayAlert("Delete Note", $"Are you sure you want to delete \"{note.Title}\"?", "Yes", "No");
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
        }

        private void OnStarNote(NoteModel note)
        {
            if (note != null)
            {
                note.IsStarred = !note.IsStarred;

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
                LoadNotes();
            }
        }
    }
}