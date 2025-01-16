using LocalizationResourceManager.Maui;
using MyPadelApp.Views;

namespace MyPadelApp
{
    public partial class AppShell : Shell
    {
        public AppShell(ILocalizationResourceManager localizationResourceManager)
        {
            InitializeComponent();
            Navigation.PushAsync(new SelectionPage(localizationResourceManager));
        }
    }
}
