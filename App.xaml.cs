using TaskFlow.Services;

using Microsoft.Maui.Storage;

namespace TaskFlow
{
    public partial class App : Application
    {
        public static AppShell? ShellInstance { get; private set; }
        public static LocalDBService? DbService { get; private set; }

        public App()
        {
            InitializeComponent();
            if (DbService == null)
                DbService = new LocalDBService();
            if (ShellInstance == null)
                ShellInstance = new AppShell(DbService);
        }

        //Determine the initial page based on sign-in status
        protected override Window CreateWindow(IActivationState? activationState)
        {
            Window window = new Window();
            if (Preferences.Get("IsSignedIn", false))
            {
                window.Page = ShellInstance;
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