namespace WorkTime.View;

public partial class MainPage : ContentPage
{
	public MainPage(WorkTimeMainViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}

