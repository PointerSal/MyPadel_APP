using LocalizationResourceManager.Maui;

namespace MyPadelApp
{
    public partial class App : Application
    {
        public App(ILocalizationResourceManager localizationResourceManager)
        {
            InitializeComponent();

            MainPage = new AppShell(localizationResourceManager);
        }
    }
}
