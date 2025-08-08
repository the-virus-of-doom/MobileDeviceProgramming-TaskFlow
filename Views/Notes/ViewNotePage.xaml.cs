using Microsoft.Maui.Controls;
using System.Collections.Generic;
using TaskFlow.Models;
using TaskFlow.ViewModels;

namespace TaskFlow
{
    public partial class ViewNotePage : ContentPage, IQueryAttributable
    {
        public ViewNoteViewModel ViewModel { get; }

        public ViewNotePage()
        {
            InitializeComponent();
            ViewModel = new ViewNoteViewModel();
            BindingContext = ViewModel;
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue("Note", out var noteObj) && noteObj is NoteModel note)
            {
                ViewModel.LoadNote(note);
            }
        }
    }
}