namespace WorkTime.View;

public partial class MainPage : ContentPage
{
	public MainPage(WorkTimeMainViewModel viewModel)
	{
        InitializeComponent();
		BindingContext = viewModel;
	}

    private void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        Delete.IsVisible = true;
        Modify.IsVisible = true;
    }
}

