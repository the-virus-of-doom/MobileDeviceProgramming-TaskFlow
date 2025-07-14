namespace TaskFlow
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // Force Light theme
            Current.UserAppTheme = AppTheme.Light;
            this.RequestedThemeChanged += (s, e) => { Application.Current.UserAppTheme = AppTheme.Light; };

            MainPage = new NavigationPage(new MainPage());
        }
    }
}
