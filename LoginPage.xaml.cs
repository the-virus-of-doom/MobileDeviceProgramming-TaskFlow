using Microsoft.Maui.Controls;
using TaskFlow.ViewModels;

namespace TaskFlow
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            BindingContext = new LoginPageViewModel();
        }
    }
}