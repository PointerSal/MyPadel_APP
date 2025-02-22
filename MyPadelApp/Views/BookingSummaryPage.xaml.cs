using MyPadelApp.ViewModels;

namespace MyPadelApp.Views;

public partial class BookingSummaryPage : ContentPage
{
    BookingSummaryViewModel _bookingSummaryViewModel;
    public BookingSummaryPage(BookingSummaryViewModel bookingSummaryViewModel)
	{
		InitializeComponent();
        BindingContext = _bookingSummaryViewModel = bookingSummaryViewModel;
    }
    private void OnBorderTapped(object sender, TappedEventArgs e)
    {
        min60Border.BackgroundColor = Colors.Transparent;
        min90Border.BackgroundColor = Colors.Transparent;
        min120Border.BackgroundColor = Colors.Transparent;

        if (sender == min60Border)
        {
            min60Border.BackgroundColor = Color.FromArgb("#a4b0e7");
            _bookingSummaryViewModel.SelectedTime = 2;
        }
        else if (sender == min90Border)
        {
            min90Border.BackgroundColor = Color.FromArgb("#a4b0e7");
            _bookingSummaryViewModel.SelectedTime = 3;
        }
        else if (sender == min120Border)
        {
            min120Border.BackgroundColor = Color.FromArgb("#a4b0e7");
            _bookingSummaryViewModel.SelectedTime = 4;
        }

        _bookingSummaryViewModel.Duration = (30 * _bookingSummaryViewModel.SelectedTime);

        try
        {
            if (_bookingSummaryViewModel.bookingPricesList != null)
                _bookingSummaryViewModel.Amount = (int)(_bookingSummaryViewModel.bookingPricesList.FirstOrDefault(e => e.duration == _bookingSummaryViewModel.Duration).price);
        }
        catch { }
    }
}