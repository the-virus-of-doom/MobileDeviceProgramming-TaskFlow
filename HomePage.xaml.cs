using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;

namespace TaskFlow
{
    public partial class HomePage : ContentPage
    {
        public HomePage()
        {
            InitializeComponent();

            // Get the current user's name and display it
            var email = Preferences.Get("CurrentUserEmail", "");
            var userData = Preferences.Get($"user_{email}", "");
            var parts = userData.Split('|');
            if (parts.Length >= 2)
            {
                UserNameLabel.Text = $"Welcome {parts[0]} {parts[1]}";
            }
            else
            {
                UserNameLabel.Text = "Welcome";
            }
        }
        // Sign out button logic
        private void OnSignOutClicked(object sender, EventArgs e)
        {
            Preferences.Set("IsSignedIn", false);
            Preferences.Remove("CurrentUserEmail");
            var currentWindow = Application.Current?.Windows.FirstOrDefault();
            if (currentWindow != null)
            {
                currentWindow.Page = new NavigationPage(new LoginPage());
            }
        }
    }
}