using System.Collections.ObjectModel;

namespace MyPadelApp.Views;

public partial class PaymentScreenPage : ContentPage
{
    private bool _IsHomeScreen;
    public bool IsHomeScreen
    {
        get => _IsHomeScreen;
        set
        {
            _IsHomeScreen = value;
            OnPropertyChanged();
        }
    }
    public PaymentScreenPage(bool IsHomeScreen = false)
	{
		InitializeComponent();
        BindingContext = this;
        this.IsHomeScreen = IsHomeScreen;
    }
    private async void OnPayClicked(object sender, EventArgs e)
    {
        if(IsHomeScreen)
            await Navigation.PushAsync(new PaymentBookingSummary());
        else
            await Navigation.PushAsync(new BookedFieldPage());
    }
}