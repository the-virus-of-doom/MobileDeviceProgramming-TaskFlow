namespace TaskFlow.Views;

public partial class UserTaskDetailPage : ContentPage
{
    private readonly Random _random = new Random();
    public UserTaskDetailPage()
	{
		InitializeComponent();

        // Graphic design is my passion (grid debug)
        //RandomizeGridCellColors(DetailGrid);
    }

    private void RandomizeGridCellColors(Grid grid)
    {
        foreach (var child in grid.Children)
        {
            if (child is VisualElement element)
            {
                element.BackgroundColor = GetRandomColor();
            }
        }
    }

    private Color GetRandomColor()
    {
        return Color.FromRgb(
            _random.Next(128, 256),
            _random.Next(128, 256),
            _random.Next(128, 256)
        );
    }

}