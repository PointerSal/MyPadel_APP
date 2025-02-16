using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LocalizationResourceManager.Maui;
using MyPadelApp.Helpers;
using MyPadelApp.Models;
using MyPadelApp.Resources.Languages;
using MyPadelApp.Services.AuthServices;
using MyPadelApp.ViewModels.ViewBaseModel;
using MyPadelApp.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPadelApp.ViewModels
{
    public partial class AccountDeletionViewModel : BaseViewModel
    {
        #region Services

        private readonly ILocalizationResourceManager _localizationResourceManager;
        private readonly IAuthServices _authServices;

        #endregion

        #region Properties

        [ObservableProperty]
        public string _password;

        [ObservableProperty]
        public bool _hasPasswordError;

        [ObservableProperty]
        public string _passwordError;

        #endregion

        #region Commands

        [RelayCommand]
        public async Task DeleteAccount()
        {
            try
            {
                (HasPasswordError, PasswordError) = FieldValidations.IsFieldNotEmpty(Password,AppResources.PasswordRequired);

                if (!HasPasswordError)
                {
                    IsBusy = true;
                    var Data = new User { email = Utils.GetUser.email, password = Password };
                    var response = await _authServices.DeleteAccount(Data);
                    if (response != null && response.code != null && response.code.Equals("0000"))
                    {
                        await Shell.Current.DisplayAlert(AppResources.Success, AppResources.AccountDeleted, AppResources.OK);
                        Utils.IsBookingPageUpdated = false;
                        Utils.IsUserLogin = false;
                        Preferences.Default.Set("username", string.Empty);
                        Preferences.Default.Set("Password", string.Empty);
                        Preferences.Default.Set("Token", string.Empty);
                        await Shell.Current.GoToAsync("..");
                        await Shell.Current.Navigation.PushAsync(new SelectionPage(_localizationResourceManager));
                    }
                    else if (response != null && response.code != null)
                        await Shell.Current.DisplayAlert(AppResources.Error, response.message, AppResources.OK);
                    else
                        await Shell.Current.DisplayAlert(AppResources.Error, AppResources.SomethingWrong, AppResources.OK);
                }
            }
            catch { }
            IsBusy = false;
        }

        [RelayCommand]
        public async Task Back()
        {
            try
            {
                await Shell.Current.GoToAsync("..");
            }
            catch { }
        }

        #endregion

        #region Constructor
        public AccountDeletionViewModel(IAuthServices authServices, ILocalizationResourceManager localizationResourceManager)
        {
            _authServices = authServices;
            _localizationResourceManager = localizationResourceManager; 
        }

        #endregion
    }
}