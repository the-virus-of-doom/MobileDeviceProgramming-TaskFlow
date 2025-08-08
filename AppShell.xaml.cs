using TaskFlow.Services;
using TaskFlow.Views;

namespace TaskFlow
{
    public partial class AppShell : Shell
    {
        private readonly LocalDBService _dbService;
        public AppShell(LocalDBService dbService)
        {
            InitializeComponent();
            _dbService = dbService;
            Routing.RegisterRoute(nameof(NotesPage), typeof(NotesPage));
            Routing.RegisterRoute(nameof(HomePage), typeof(HomePage));
            Routing.RegisterRoute(nameof(ViewNotePage), typeof(ViewNotePage));
            Routing.RegisterRoute(nameof(EditNotePage), typeof(EditNotePage));
            Routing.RegisterRoute(nameof(CreateNotePage), typeof(CreateNotePage));
            Routing.RegisterRoute(nameof(TodoPage), typeof(TodoPage));
            Routing.RegisterRoute(nameof(SettingsPage), typeof(SettingsPage));
            Routing.RegisterRoute(nameof(UserTaskDetailPage), typeof(UserTaskDetailPage));
            // TODO: set up routing for CalendarPage
            // Routing.RegisterRoute(nameof(CalendarPage), typeof(CalendarPage));
        }
    }
}
