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
}