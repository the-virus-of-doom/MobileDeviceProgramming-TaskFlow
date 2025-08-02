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
        }
    }
}
