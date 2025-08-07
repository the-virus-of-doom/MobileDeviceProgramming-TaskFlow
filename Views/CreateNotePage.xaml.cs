using Microsoft.Maui.Controls;
using TaskFlow.ViewModels;

namespace TaskFlow
{
    public partial class CreateNotePage : ContentPage
    {
        public CreateNotePage()
        {
            InitializeComponent();
            BindingContext = new CreateNoteViewModel(Navigation);
        }
    }
}