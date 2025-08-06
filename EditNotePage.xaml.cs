using Microsoft.Maui.Controls;
using System.Collections.Generic;
using TaskFlow.ViewModels;

namespace TaskFlow
{
    public partial class EditNotePage : ContentPage, IQueryAttributable
    {
        public EditNotePage()
        {
            InitializeComponent();
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue("Note", out var noteObj) && noteObj is NoteModel note)
            {
                BindingContext = new EditNoteViewModel(note, Navigation);
            }
        }
    }
}