using Microsoft.Maui.Controls.Shapes;
using MyPadelApp.Resources.Languages;
using MyPadelApp.ViewModels;

namespace MyPadelApp.Views;

public partial class FITPaymentPage : ContentPage
{
    FITPaymentViewModel _fITPaymentViewModel;
    public FITPaymentPage(FITPaymentViewModel fITPaymentViewModel)
	{
		InitializeComponent();
		BindingContext = _fITPaymentViewModel = fITPaymentViewModel;
	}
    private async void OnWebNavigated(object sender, WebNavigatedEventArgs e)
    {
        try
        {
            if (e.Url != null && !string.IsNullOrEmpty(e.Url) && e.Url.Contains("http://ufficio.pointer.re.it:7070/api/stripe/success?session_id="))
            {
                if (e.Url.Contains("success"))
                    await _fITPaymentViewModel.AddCard();
                else
                {
                    await Shell.Current.DisplayAlert(AppResources.Error, AppResources.SomethingWrong, AppResources.OK);
                    Loader.IsVisible = false;
                    await Shell.Current.GoToAsync("..");
                }
            }
            else
                Loader.IsVisible = false;
        }
        catch (Exception ex)
        {
            Loader.IsVisible = false;
        }
    }

    private async void OnWebNavigating(object sender, WebNavigatingEventArgs e)
    {
        try
        {
            Loader.IsVisible = true;
            if(e.Url.Equals("http://ufficio.pointer.re.it:7070/api/stripe/success"))
            {
                e.Cancel = true;
                await Shell.Current.GoToAsync("..");
            }
        }
        catch (Exception ex) { }
    }
}