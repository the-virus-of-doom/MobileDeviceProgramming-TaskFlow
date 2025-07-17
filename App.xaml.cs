using Microsoft.Maui.Storage;

namespace TaskFlow
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // Check if user is signed in
            if (Preferences.Get("IsSignedIn", true))
            {
                MainPage = new AppShell();
            }
            else
            {
                MainPage = new NavigationPage(new LoginPage());
            }
        }

        private async void OnSignOutClicked(object sender, EventArgs e)
        {
            Preferences.Set("IsSignedIn", false);
            Preferences.Remove("CurrentUserEmail");
            Application.Current.MainPage = new NavigationPage(new LoginPage());
        }
    }
}