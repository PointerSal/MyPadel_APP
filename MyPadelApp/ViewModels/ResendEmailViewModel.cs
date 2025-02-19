using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyPadelApp.Helpers;
using MyPadelApp.Models;
using MyPadelApp.Resources.Languages;
using MyPadelApp.Services.AuthServices;
using MyPadelApp.ViewModels.ViewBaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MyPadelApp.ViewModels
{
    public partial class ResendEmailViewModel : BaseViewModel
    {
        #region Services

        private readonly IAuthServices _authServices;

        #endregion

        #region Properties

        [ObservableProperty]
        public bool _hasOTPError;

        [ObservableProperty]
        public string _oTPError;

        #endregion

        #region Commands

        [RelayCommand]
        private async Task Back()
        {
            try
            {
                await Shell.Current.GoToAsync("..");
            }
            catch { }
        }

        [RelayCommand]
        private async void Appearing()
        {
            try
            {
            }
            catch { }
        }

        [RelayCommand]
        private async Task VerifyEmail()
        {
            try
            {
                IsBusy = true;
                (HasOTPError, OTPError) = FieldValidations.IsFieldNotEmpty(UserData.otp, AppResources.OTPRequired);
                if (!HasOTPError)
                {
                    UserData.email = Utils.GetUser.email;
                    var response = await _authServices.VerifyEmail(UserData);
                    if (response != null && response.code != null && response.code.Equals("0000"))
                    {
                        Utils.GetUser.isEmailVerified = true;
                        await Shell.Current.GoToAsync("../RegistrationResendOTPPage");
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
        private async Task ResendEmail()
        {
            try
            {
                IsBusy = true;
                var Data = new User { email = Utils.GetUser.email };
                var response = await _authServices.ResendOTP(Data);
                if (response != null && response.code != null && response.code.Equals("0000"))
                    await Shell.Current.DisplayAlert(AppResources.Success, AppResources.EmailResent, AppResources.OK);
                else if (response != null && response.code != null)
                    await Shell.Current.DisplayAlert(AppResources.Error, response.message, AppResources.OK);
                else
                    await Shell.Current.DisplayAlert(AppResources.Error, AppResources.SomethingWrong, AppResources.OK);
            }
            catch { }
            IsBusy = false;
        }

        public async void OnBack()
        {
            await Shell.Current.GoToAsync("..");
        }

        #endregion

        #region Constructor
        public ResendEmailViewModel(IAuthServices authServices)
        {
            _authServices = authServices;
        }

        #endregion
    }
}
