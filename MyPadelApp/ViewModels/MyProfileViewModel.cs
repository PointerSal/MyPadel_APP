using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LocalizationResourceManager.Maui;
using MyPadelApp.Helpers;
using MyPadelApp.Models;
using MyPadelApp.ViewModels.ViewBaseModel;
using MyPadelApp.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPadelApp.ViewModels
{
    public partial class MyProfileViewModel : BaseViewModel
    {
        #region Services

        private readonly ILocalizationResourceManager _localizationResourceManager;

        #endregion

        #region Commands

        [RelayCommand]
        public async Task Logout()
        {
            try
            {
                Utils.IsUserLogin = false;
                Utils.IsBookingPageUpdated = false;
                Preferences.Default.Set("username", string.Empty);
                Preferences.Default.Set("Password", string.Empty);
                Preferences.Default.Set("Token", string.Empty);
                await Shell.Current.Navigation.PushAsync(new SelectionPage(_localizationResourceManager));
            }
            catch { }
        }

        [RelayCommand]
        public async Task NavigateToPage(string PageName)
        {
            try
            {
                await Shell.Current.GoToAsync(PageName);
            }
            catch { }
        }

        #endregion

        #region Constructor
        public MyProfileViewModel(ILocalizationResourceManager localizationResourceManager)
        {
            _localizationResourceManager = localizationResourceManager;
        }

        #endregion
    }
}
