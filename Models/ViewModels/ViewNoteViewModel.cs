using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using System.Globalization;
using Microsoft.Maui.Storage;
using TaskFlow.Models;

namespace TaskFlow.ViewModels
{
    public class ViewNoteViewModel : INotifyPropertyChanged
    {
        private string _title;
        private string _content;
        private string _timestamp;
        private bool _isStarred;

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
        public string Timestamp
        {
            get => _timestamp;
            set { _timestamp = value; OnPropertyChanged(); }
        }
        public bool IsStarred
        {
            get => _isStarred;
            set { _isStarred = value; OnPropertyChanged(); }
        }

        public ICommand BackCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand StarCommand { get; }

        public ViewNoteViewModel()
        {
            _title = string.Empty;
            _content = string.Empty;
            _timestamp = string.Empty;
            _isStarred = false;

            BackCommand = new Command(OnBack);
            EditCommand = new Command(OnEdit);
            StarCommand = new Command(OnStar);
        }

        public void LoadNote(NoteModel note)
        {
            Title = note.Title;
            Content = note.Content;
            Timestamp = note.Timestamp;
            IsStarred = note.IsStarred;
        }

        private async void OnBack()
        {
            await Shell.Current.GoToAsync("NotesPage", false);
        }

        private async void OnEdit()
        {
            var note = new NoteModel
            {
                Title = Title,
                Content = Content,
                Timestamp = Timestamp,
                IsStarred = IsStarred
            };
            await Shell.Current.GoToAsync(nameof(EditNotePage), new Dictionary<string, object>
            {
                ["Note"] = note
            });
        }

        private void OnStar()
        {
            IsStarred = !IsStarred;
            SaveStarState();
        }

        private void SaveStarState()
        {
            var email = Preferences.Get("CurrentUserEmail", "");
            var noteKey = $"notes_{email}";
            var notes = Preferences.Get(noteKey, "");
            var noteList = string.IsNullOrEmpty(notes) ? new List<string>() : new List<string>(notes.Split('~'));

            for (int i = 0; i < noteList.Count; i++)
            {
                var parts = noteList[i].Split('|');
                if (parts.Length >= 3 && parts[0] == Title && parts[1] == Content && parts[2] == Timestamp)
                {
                    noteList[i] = $"{Title}|{Content}|{Timestamp}|{IsStarred}";
                    break;
                }
            }
            Preferences.Set(noteKey, string.Join("~", noteList));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    public class StarImageConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return (bool)value! ? "star_fill.png" : "star.png";
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}