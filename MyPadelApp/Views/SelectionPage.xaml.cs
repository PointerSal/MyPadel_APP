using LocalizationResourceManager.Maui;
using System.Globalization;

namespace MyPadelApp.Views;

public partial class SelectionPage : ContentPage
{
    ILocalizationResourceManager localizationResourceManager;
    public SelectionPage(ILocalizationResourceManager localizationResourceManager)
	{
        InitializeComponent();
        this.localizationResourceManager = localizationResourceManager;
        var Language = Preferences.Default.Get("SetLanguage", "it");
        this.localizationResourceManager.CurrentCulture = new CultureInfo(Language);
        styleSwitch.IsToggled = Preferences.Default.Get("SetLanguage", "it").Equals("it");
    }

    protected override bool OnBackButtonPressed()
    {
        Environment.Exit(0);
        return true;
    }

    private async void OnRegisteredClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new RegisterPage());
    }
    private async void OnLoginClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new LoginPage());
    }
    private void styleSwitch_Toggled(object sender, ToggledEventArgs e)
    {
        if (e.Value)
        {
            Preferences.Default.Set("SetLanguage", "it");
            localizationResourceManager.CurrentCulture = new CultureInfo("it");
        }
        else
        {
            Preferences.Default.Set("SetLanguage", "en");
            localizationResourceManager.CurrentCulture = new CultureInfo("en");
        }
    }
}