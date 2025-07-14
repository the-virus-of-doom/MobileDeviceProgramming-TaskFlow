namespace TaskFlow
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
        }

        private void Btn_OpenTodo_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new TodoPage());
        }

        private void Btn_OpenSettings_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SettingsPage());
        }
    }

}
