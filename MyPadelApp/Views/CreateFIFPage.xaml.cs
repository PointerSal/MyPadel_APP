using MyPadelApp.ViewModels;

namespace MyPadelApp.Views;

public partial class CreateFIFPage : ContentPage
{
    CreateFIFViewModel _createFIFViewModel;
    public CreateFIFPage(CreateFIFViewModel createFIFViewModel)
	{
		InitializeComponent();
		BindingContext = _createFIFViewModel = createFIFViewModel;
	}
    private void OnBrithProvinceSelected(object sender, EventArgs e)
    {
        _createFIFViewModel.LoadCitiesForProvince(true);
    }
    private void OnResidenceProvinceSelected(object sender, EventArgs e)
    {
        _createFIFViewModel.LoadCitiesForProvince(false);
    }
}