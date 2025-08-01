using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using System.Text;

namespace TaskFlow
{
    public partial class SignUpPage : ContentPage
    {
        public SignUpPage()
        {
            InitializeComponent();
        }
        // Sign up inputs and validation
        private async void OnSignUpClicked(object sender, EventArgs e)
        {
            var firstName = FirstNameEntry.Text?.Trim() ?? "";
            var lastName = LastNameEntry.Text?.Trim() ?? "";
            var email = EmailEntry.Text?.Trim().ToLowerInvariant() ?? "";
            var password = PasswordEntry.Text ?? "";
            var confirmPassword = ConfirmPasswordEntry.Text ?? "";

            var errors = new StringBuilder();

            if (firstName.Length < 2)
                errors.AppendLine("First name must be at least 2 characters.");
            if (lastName.Length < 2)
                errors.AppendLine("Last name must be at least 2 characters.");
            if (!email.Contains("@") || !email.Contains("."))
                errors.AppendLine("Please enter a valid email address.");
            if (password.Length < 8)
                errors.AppendLine("Password must be at least 8 characters.");
            if (password != confirmPassword)
                errors.AppendLine("Passwords do not match.");
            if (Preferences.ContainsKey($"user_{email}"))
                errors.AppendLine("An account already exists with that email address.");

            if (errors.Length > 0)
            {
                await DisplayAlert("Error", errors.ToString().Trim(), "OK");
                return;
            }

            // Save user data and sign in
            Preferences.Set($"user_{email}", $"{firstName}|{lastName}|{password}");
            Preferences.Set("IsSignedIn", true);
            Preferences.Set("CurrentUserEmail", email);

            var currentWindow = Application.Current?.Windows.FirstOrDefault();
            if (currentWindow != null)
            {
                currentWindow.Page = new AppShell();
            }
        }
        // Navigate to login page
        private async void OnGoToLoginClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new LoginPage());
        }
    }
}