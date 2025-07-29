using TaskFlow.Models;

namespace TaskFlow;

public partial class TodoPage : ContentPage
{
	public List<UserTask> UserTasks = new List<UserTask>();
	public TodoPage()
	{
		InitializeComponent();

		const int demoListLength = 5;

        for (int i = 0; i < demoListLength; i++)
        {
            var temp = new UserTask();
            temp.Name = "demo list #" + i.ToString();
            //temp.Description = "default description";
            UserTasks.Add(temp);
        }

        Cv_TodoList.ItemsSource = UserTasks;

    }

    private void Cv_TodoList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var selectedUserTask = e.CurrentSelection as Models.UserTask;
        if (selectedUserTask == null) return;

        // TODO: open task detail page
        
        //var userTaskViewModel = new UserTaskDetailViewModel { UserTask = selectedUserTask };
        //var userTaskDetail = new UserTaskDetail();
        //userTaskDetail.BindingContext = userTaskViewModel;
        //Navigation.PushAsync(userTaskDetail);

        ((CollectionView)sender).SelectedItem = null;
    }
}