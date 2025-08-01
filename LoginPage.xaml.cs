using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;

namespace TaskFlow
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }
        // Login inputs and validation
        private async void OnLoginClicked(object sender, EventArgs e)
        {
            var email = EmailEntry.Text?.Trim().ToLowerInvariant() ?? "";
            var password = PasswordEntry.Text ?? "";

            if (!Preferences.ContainsKey($"user_{email}"))
            {
                await ShowLoginError();
                return;
            }

            var userData = Preferences.Get($"user_{email}", "");
            var parts = userData.Split('|');
            if (parts.Length != 3 || parts[2] != password)
            {
                await ShowLoginError();
                return;
            }

            Preferences.Set("IsSignedIn", true);
            Preferences.Set("CurrentUserEmail", email);

            var currentWindow = Application.Current?.Windows.FirstOrDefault();
            if (currentWindow != null)
            {
                currentWindow.Page = new AppShell();
            }
        }
        // Show error message if login fails
        private async Task ShowLoginError()
        {
            var result = await DisplayAlert("Error", "Email or password may be wrong", "Try again", "Sign up");
            if (!result)
            {
                await Navigation.PushAsync(new SignUpPage());
            }
        }
        // Navigate to sign up page
        private async void OnGoToSignUpClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SignUpPage());
        }
    }
}