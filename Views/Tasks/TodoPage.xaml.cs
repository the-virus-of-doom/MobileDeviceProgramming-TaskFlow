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

		const int demoListLength = 20;

        for (int i = 0; i < demoListLength; i++)
        {
            var temp = new UserTask();
            temp.Name = "demo list #" + i.ToString();
            //temp.Description = "default description";
            UserTasks.Add(temp);
        }

        // TODO: move UserTasks to global variable when implementing DB service
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