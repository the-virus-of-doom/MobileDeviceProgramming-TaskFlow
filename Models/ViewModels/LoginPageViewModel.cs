using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;

namespace TaskFlow.ViewModels
{
    public class LoginPageViewModel : INotifyPropertyChanged
    {
        private string _email = string.Empty;
        private string _password = string.Empty;

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

        public ICommand LoginCommand { get; }
        public ICommand GoToSignUpCommand { get; }

        public LoginPageViewModel()
        {
            LoginCommand = new Command(async () => await OnLogin());
            GoToSignUpCommand = new Command(async () => await OnGoToSignUp());
        }

        private async Task OnLogin()
        {
            var email = Email?.Trim().ToLowerInvariant() ?? "";
            var password = Password ?? "";

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
            if (currentWindow != null && App.ShellInstance != null)
            {
                currentWindow.Page = App.ShellInstance;
            }
        }

        private async Task ShowLoginError()
        {
            var currentWindow = Application.Current?.Windows.FirstOrDefault();
            var mainPage = currentWindow?.Page;
            if (mainPage != null)
            {
                var result = await mainPage.DisplayAlert("Error", "Email or password may be wrong", "Try again", "Sign up");
                if (!result)
                {
                    await mainPage.Navigation.PushAsync(new SignUpPage());
                }
            }
        }

        private async Task OnGoToSignUp()
        {
            var currentWindow = Application.Current?.Windows.FirstOrDefault();
            var mainPage = currentWindow?.Page;
            if (mainPage != null)
            {
                await mainPage.Navigation.PushAsync(new SignUpPage());
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}