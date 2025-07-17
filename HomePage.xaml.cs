using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;

namespace TaskFlow
{
    public partial class HomePage : ContentPage
    {
        public HomePage()
        {
            InitializeComponent();

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

        private async void OnSignOutClicked(object sender, EventArgs e)
        {
            Preferences.Set("IsSignedIn", false);
            Preferences.Remove("CurrentUserEmail");
            Application.Current.MainPage = new NavigationPage(new LoginPage());
        }

        private async void OnDeleteAccountClicked(object sender, EventArgs e)
        {
            bool confirm = await DisplayAlert("Delete Account", "Are you sure you want to delete your account? This action cannot be undone.", "Delete", "Cancel");
            if (confirm)
            {
                var email = Preferences.Get("CurrentUserEmail", "");
                Preferences.Remove($"user_{email}");
                Preferences.Set("IsSignedIn", false);
                Preferences.Remove("CurrentUserEmail");
                Application.Current.MainPage = new NavigationPage(new LoginPage());
            }
        }
    }
}