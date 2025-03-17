using MyPadelApp.ViewModels;

namespace MyPadelApp.Views;

public partial class FITCardPage : ContentPage
{
    FITCardViewModel _fITCardViewModel;
    public FITCardPage(FITCardViewModel fITCardViewModel)
	{
		InitializeComponent();
        BindingContext = _fITCardViewModel = fITCardViewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _fITCardViewModel.OnPageAppearing();
    }
}