using MyPadelApp.ViewModels;
using System.Collections.ObjectModel;

namespace MyPadelApp.Views;

public partial class PaymentScreenPage : ContentPage
{
    public PaymentScreenPage(PaymentScreenViewModel paymentScreenViewModel)
	{
		InitializeComponent();
        BindingContext = paymentScreenViewModel;
    }
    //private async void OnPayClicked(object sender, EventArgs e)
    //{
    //    if (IsHomeScreen)
    //        await Shell.Current.GoToAsync("PaymentBookingSummary");
    //    else
    //        await Navigation.PushAsync(new BookedFieldPage());
    //}
}