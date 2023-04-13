namespace WorkTime.View;

public partial class AddItemsPage : ContentPage
{
	public AddItemsPage(AddItemsPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}