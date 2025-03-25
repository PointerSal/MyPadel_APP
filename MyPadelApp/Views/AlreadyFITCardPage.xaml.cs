using MyPadelApp.ViewModels;

namespace MyPadelApp.Views;

public partial class AlreadyFITCardPage : ContentPage
{
	public AlreadyFITCardPage(AlreadyFITCardViewModel alreadyFITCardViewModel)
	{
		InitializeComponent();
        BindingContext = alreadyFITCardViewModel;
    }

    private void ExpiryDatePicker_DateSelected(object sender, DateChangedEventArgs e)
    {
        try
        {
            ExpiryDatePlaceholderLabel.IsVisible = e.NewDate == null || e.NewDate == default;
            ExpiryDatePicker.Opacity = e.NewDate == null || e.NewDate == default ? 0 : 1;
        }
        catch { }
    }

    private void MedicalCertificateDatePicker_DateSelected(object sender, DateChangedEventArgs e)
    {
        try
        {
            MedicalCertificateDatePlaceholderLabel.IsVisible = e.NewDate == null || e.NewDate == default;
            MedicalCertificateDatePicker.Opacity = e.NewDate == null || e.NewDate == default ? 0 : 1;
        }
        catch { }
    }
}