using MyPadelApp.Models;
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
        //min60Border.BackgroundColor = Colors.Transparent;
        //min90Border.BackgroundColor = Colors.Transparent;
        //min120Border.BackgroundColor = Colors.Transparent;

        //if (sender == min60Border)
        //{
        //    min60Border.BackgroundColor = Color.FromArgb("#a4b0e7");
        //    _bookingSummaryViewModel.SelectedTime = 2;
        //}
        //else if (sender == min90Border)
        //{
        //    min90Border.BackgroundColor = Color.FromArgb("#a4b0e7");
        //    _bookingSummaryViewModel.SelectedTime = 3;
        //}
        //else if (sender == min120Border)
        //{
        //    min120Border.BackgroundColor = Color.FromArgb("#a4b0e7");
        //    _bookingSummaryViewModel.SelectedTime = 4;
        //}


        try
        {
            if (sender is Border border && border.BindingContext is CourtsDuration selectedItem)
            {
                foreach (var item in _bookingSummaryViewModel.DurationSlots)
                {
                    item.IsSelected = false; 
                }
                selectedItem.IsSelected = true; 
            }
            if (_bookingSummaryViewModel.bookingPricesList != null)
            {
                var CurrentSlot = _bookingSummaryViewModel.DurationSlots.FirstOrDefault(e => e.IsSelected == true).Duration;
                _bookingSummaryViewModel.Duration = CurrentSlot ?? 0;
                _bookingSummaryViewModel.Amount = (int)(_bookingSummaryViewModel.bookingPricesList.FirstOrDefault(e => e.duration == CurrentSlot).price);
            }
        }
        catch { }
    }
}