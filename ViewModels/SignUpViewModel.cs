using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;

namespace TaskFlow.ViewModels
{
    public class SignUpViewModel : INotifyPropertyChanged
    {
        private string _firstName = string.Empty;
        private string _lastName = string.Empty;
        private string _email = string.Empty;
        private string _password = string.Empty;
        private string _confirmPassword = string.Empty;

        public string FirstName
        {
            get => _firstName;
            set { _firstName = value; OnPropertyChanged(); }
        }
        public string LastName
        {
            get => _lastName;
            set { _lastName = value; OnPropertyChanged(); }
        }
        public string Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged(); }
        }
        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(); }
        }
        public string ConfirmPassword
        {
            get => _confirmPassword;
            set { _confirmPassword = value; OnPropertyChanged(); }
        }

        public ICommand SignUpCommand { get; }
        public ICommand GoToLoginCommand { get; }

        public SignUpViewModel()
        {
            SignUpCommand = new Command(async () => await OnSignUp());
            GoToLoginCommand = new Command(async () => await OnGoToLogin());
        }

        private async Task OnSignUp()
        {
            var errors = new StringBuilder();
            var firstName = FirstName?.Trim() ?? "";
            var lastName = LastName?.Trim() ?? "";
            var email = Email?.Trim().ToLowerInvariant() ?? "";
            var password = Password ?? "";
            var confirmPassword = ConfirmPassword ?? "";

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
                await Application.Current.MainPage.DisplayAlert("Error", errors.ToString().Trim(), "OK");
                return;
            }

            Preferences.Set($"user_{email}", $"{firstName}|{lastName}|{password}");
            Preferences.Set("IsSignedIn", true);
            Preferences.Set("CurrentUserEmail", email);

            var currentWindow = Application.Current?.Windows.FirstOrDefault();
            if (currentWindow != null)
            {
                currentWindow.Page = new AppShell();
            }
        }

        private async Task OnGoToLogin()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new LoginPage());
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}