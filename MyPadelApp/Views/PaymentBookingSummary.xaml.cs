using MyPadelApp.ViewModels;

namespace MyPadelApp.Views;

public partial class PaymentBookingSummary : ContentPage
{
	public PaymentBookingSummary(PaymentBookingSummaryViewModel paymentBookingSummaryViewModel)
	{
		InitializeComponent();
		BindingContext = paymentBookingSummaryViewModel;
    }
}