using System.Diagnostics;
using TaskFlow.Models;
using TaskFlow.Models.ViewModels;
using TaskFlow.Views;

namespace TaskFlow;

public partial class TodoPage : ContentPage
{
	public List<UserTask> UserTasks = new List<UserTask>();
    public TodoPage()
    {
        InitializeComponent();

        // Get the current user's Id from Preferences (default to 1 if not found)
        var currentUserId = Preferences.Get("CurrentUserId", 1);

        // Get dbService from App
        //var dbService = App.DbService;
        //if (dbService != null)
        //{
        //    // Populate UserTasks asynchronously
        //    Task.Run(async () =>
        //    {
        //        var tasks = await dbService.GetUserTasks(currentUserId);
        //        MainThread.BeginInvokeOnMainThread(() =>
        //        {
        //            UserTasks = tasks;
        //            // Cv_TodoList.ItemsSource = UserTasks;
        //        });
        //    });
        //}

        const int demoListLength = 20;


        for (int i = 0; i < demoListLength; i++)
        {
            var temp = new UserTask();
            temp.Name = "demo list #" + i.ToString();
            temp.Description = "default description #" + i.ToString();
            UserTasks.Add(temp);
        }

        Cv_TodoList.ItemsSource = UserTasks;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        // refresh bindings on load
        // should fix navigating back from details showing stale data?
        //var currentContext = BindingContext;
        //BindingContext = null;
        //BindingContext = currentContext;

        Debug.WriteLine("UserTasks =======================================");
        foreach (var task in UserTasks)
        {
            Debug.WriteLine(task.Name);
        }
    }

    private void Cv_TodoList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var selectedUserTask = e.CurrentSelection.FirstOrDefault() as UserTask;
        if (selectedUserTask == null) return;

        var userTaskViewModel = new UserTaskDetailViewModel { UserTask = selectedUserTask };
        var userTaskDetailPage = new UserTaskDetailPage
        {
            BindingContext = userTaskViewModel
        };
        Navigation.PushAsync(userTaskDetailPage);

        ((CollectionView)sender).SelectedItem = null;
    }
}