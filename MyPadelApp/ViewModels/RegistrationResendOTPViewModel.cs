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
using System.Threading.Tasks;

namespace MyPadelApp.ViewModels
{
    public partial class RegistrationResendOTPViewModel : BaseViewModel
    {
        #region Services

        private readonly IAuthServices _authServices;

        #endregion

        #region Properties

        [ObservableProperty]
        public bool _hasPhoneError;

        [ObservableProperty]
        public string _phoneError;

        [ObservableProperty]
        public bool _hasOTPError;

        [ObservableProperty]
        public string _oTPError;

        #endregion

        #region Commands

        [RelayCommand]
        public async Task SendCode()
        {
            try
            {
                IsBusy = true;
                (HasPhoneError, PhoneError) = FieldValidations.IsItalianPhoneNumberValid(UserData.cell);

                if (!HasPhoneError)
                {
                    var Data = new User { cell = UserData.cell };
                    var response = await _authServices.ResendPhoneOTP(Data);
                    if (response != null && response.code.Equals("0000"))
                        await Shell.Current.DisplayAlert(AppResources.Success, AppResources.PhoneResent, AppResources.OK);
                    else if (response != null)
                        await Shell.Current.DisplayAlert(AppResources.Error, response.message, AppResources.OK);
                    else
                        await Shell.Current.DisplayAlert(AppResources.Error, AppResources.SomethingWrong, AppResources.OK);
                }
            }
            catch { }
            IsBusy = false;
        }

        [RelayCommand]
        public async Task Confirm()
        {
            try
            {
                (HasPhoneError, PhoneError) = FieldValidations.IsItalianPhoneNumberValid(UserData.cell);
                (HasOTPError, OTPError) = FieldValidations.IsFieldNotEmpty(UserData.otp, AppResources.OTPRequired);

                if (!HasPhoneError && !HasOTPError)
                {
                    var response = await _authServices.VerifyPhone(UserData);
                    if (response != null && response.code.Equals("0000"))
                    {
                        Utils.GetUser.isPhoneVerified = true;
                        await Shell.Current.GoToAsync("FinalStepPage");
                    }
                    else if (response != null)
                        await Shell.Current.DisplayAlert(AppResources.Error, response.message, AppResources.OK);
                    else
                        await Shell.Current.DisplayAlert(AppResources.Error, AppResources.SomethingWrong, AppResources.OK);
                }
            }
            catch { }
        }

        #endregion

        #region Constructor
        public RegistrationResendOTPViewModel(IAuthServices authServices)
        {
            _authServices = authServices;
        }

        #endregion
    }
}