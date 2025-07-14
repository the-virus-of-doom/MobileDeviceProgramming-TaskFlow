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
            UserTasks.Add(new UserTask());
        }

        Cv_TodoList.ItemsSource = UserTasks;

    }

    private void Cv_TodoList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        // TODO: mark task as complete
    }
}