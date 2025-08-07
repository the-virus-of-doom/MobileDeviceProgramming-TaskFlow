using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;
using TaskFlow.ViewModels;

namespace TaskFlow
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        public HomePage()
        {
            InitializeComponent();
            BindingContext = new HomePageViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            (BindingContext as HomePageViewModel)?.LoadUserName();
            (BindingContext as HomePageViewModel)?.LoadStarredNotes();
        }
    }
}