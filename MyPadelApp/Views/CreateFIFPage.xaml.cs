using MyPadelApp.ViewModels;

namespace MyPadelApp.Views;

public partial class CreateFIFPage : ContentPage
{
	public CreateFIFPage(CreateFIFViewModel createFIFViewModel)
	{
		InitializeComponent();
		BindingContext = createFIFViewModel;
	}
}