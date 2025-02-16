using MyPadelApp.ViewModels;

namespace MyPadelApp.Views;

public partial class HistoryBookingSummaryPage : ContentPage
{
	public HistoryBookingSummaryPage(HistoryBookingSummaryViewModel historyBookingSummaryViewModel)
	{
		InitializeComponent();
        BindingContext = historyBookingSummaryViewModel;
    }
}