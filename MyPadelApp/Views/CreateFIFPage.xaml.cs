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

    private void BrithDatePicker_DateSelected(object sender, DateChangedEventArgs e)
    {
        try
        {
            BrithDatePlaceholderLabel.IsVisible = e.NewDate == null || e.NewDate == default;
            BrithDatePicker.Opacity = e.NewDate == null || e.NewDate == default ? 0 : 1;
        }
        catch { }
    }
}