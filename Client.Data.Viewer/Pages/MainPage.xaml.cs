using Client.Data.Viewer.ViewModels;

namespace Client.Data.Viewer.Pages;

public partial class MainPage : ContentPage
{
	public MainPage(MainViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
