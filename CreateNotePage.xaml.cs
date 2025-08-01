using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using System;

namespace TaskFlow
{
    public partial class CreateNotePage : ContentPage
    {
        public CreateNotePage()
        {
            InitializeComponent();
        }
        // Save Button logic for creating a new note
        private async void OnSaveClicked(object sender, EventArgs e)
        {
            var title = TitleEntry.Text?.Trim() ?? "";
            var content = ContentEditor?.Text?.Trim() ?? "";
            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(content))
            {
                await DisplayAlert("Error", "Please enter both a title and content.", "OK");
                return;
            }

            var email = Preferences.Get("CurrentUserEmail", "");
            var noteKey = $"notes_{email}";
            var notes = Preferences.Get(noteKey, "");
            var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            var newNote = $"{title}|{content}|{timestamp}";
            notes = string.IsNullOrEmpty(notes) ? newNote : $"{notes}~{newNote}";
            Preferences.Set(noteKey, notes);

            await Navigation.PopAsync();
        }
    }
}