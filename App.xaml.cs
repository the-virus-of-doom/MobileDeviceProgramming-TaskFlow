using Microsoft.Maui.Storage;

namespace TaskFlow
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        //Determine the initial page based on sign-in status
        protected override Window CreateWindow(IActivationState? activationState)
        {
            Window window = new Window();

            if (Preferences.Get("IsSignedIn", true))
            {
                window.Page = new AppShell();
            }
            else
            {
                window.Page = new NavigationPage(new LoginPage());
            }

            return window;
        }
        // Sign out logic
        private async void OnSignOutClicked(object sender, EventArgs e)
        {
            Preferences.Set("IsSignedIn", false);
            Preferences.Remove("CurrentUserEmail");

            await Task.Run(() =>
            {
                if (Application.Current?.Windows?.Count > 0)
                {
                    Application.Current.Windows[0].Page = new NavigationPage(new LoginPage());
                }
            });
        }
    }
}