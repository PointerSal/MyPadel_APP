using MyPadelApp.ViewModels;

namespace MyPadelApp.Views;

public partial class CancelBookingPage : ContentPage
{
	public CancelBookingPage(CancelBookingViewModel cancelBookingViewModel)
	{
		InitializeComponent();
        BindingContext = cancelBookingViewModel;
    }
}