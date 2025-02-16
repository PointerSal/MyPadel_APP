using MyPadelApp.ViewModels;

namespace MyPadelApp.Views;

public partial class AlreadyFITCardPage : ContentPage
{
	public AlreadyFITCardPage(AlreadyFITCardViewModel alreadyFITCardViewModel)
	{
		InitializeComponent();
        BindingContext = alreadyFITCardViewModel;
    }
}