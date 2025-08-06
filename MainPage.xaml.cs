using TaskFlow.Models;
using TaskFlow.Services;

namespace TaskFlow
{
    public partial class MainPage : ContentPage
    {
        private readonly LocalDBService _dbService;
        private const int DEFAULT_USER_ID = 1;

        // public List<UserTask> _userTasks = new List<UserTask>();
        public MainPage(LocalDBService dbService)
        {
            InitializeComponent();
            _dbService = dbService;
            
            // example of loading data on start
            // Task.Run(async () => _userTasks = await _dbService.GetUserTasks(DEFAULT_USER_ID));
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
