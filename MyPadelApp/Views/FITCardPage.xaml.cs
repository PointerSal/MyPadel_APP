using MyPadelApp.ViewModels;

namespace MyPadelApp.Views;

public partial class FITCardPage : ContentPage
{
	public FITCardPage(FITCardViewModel fITCardViewModel)
	{
		InitializeComponent();
        BindingContext = fITCardViewModel;
    }
}