namespace TaskFlow
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(NotesPage), typeof(NotesPage));
            Routing.RegisterRoute(nameof(HomePage), typeof(HomePage));
            Routing.RegisterRoute(nameof(ViewNotePage), typeof(ViewNotePage));
            Routing.RegisterRoute(nameof(EditNotePage), typeof(EditNotePage));
            Routing.RegisterRoute(nameof(CreateNotePage), typeof(CreateNotePage));
            Routing.RegisterRoute(nameof(TodoPage), typeof(TodoPage));
            Routing.RegisterRoute(nameof(SettingsPage), typeof(SettingsPage));
            // TODO: set up routing for CalendarPage
            // Routing.RegisterRoute(nameof(CalendarPage), typeof(CalendarPage));
        }
    }
}
