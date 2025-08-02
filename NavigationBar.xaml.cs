using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;

namespace TaskFlow
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NavigationBar : ContentView
    {
        public NavigationBar()
        {
            InitializeComponent();
        }
        // Event handlers for navigation buttons
        private async void OnHomeClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("HomePage", animate: false);
        }

        private async void OnNotesClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("NotesPage", animate: false);
        }
    }
}