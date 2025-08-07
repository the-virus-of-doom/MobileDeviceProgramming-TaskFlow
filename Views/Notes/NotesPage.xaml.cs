using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using System.Collections.ObjectModel;
using System.Linq;
using TaskFlow.Models.ViewModels.Notes;

namespace TaskFlow
{
    public partial class NotesPage : ContentPage
    {
        public NotesPage()
        {
            InitializeComponent();
            BindingContext = new NotesPageViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            (BindingContext as NotesPageViewModel)?.LoadNotes();
        }
    }
}