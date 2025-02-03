using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyPadelApp.Helpers;
using MyPadelApp.Resources.Languages;
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
                //Code for sending new OTP
            }
            catch { }
        }

        [RelayCommand]
        public async Task Confirm()
        {
            try
            {
                (HasPhoneError, PhoneError) = FieldValidations.IsItalianPhoneNumberValid(UserData.cell);
                (HasOTPError, OTPError) = FieldValidations.IsFieldNotEmpty(UserData.otp, AppResources.OTPRequired);

                if (!HasPhoneError && !HasOTPError)
                    await Shell.Current.GoToAsync("FinalStepPage");
            }
            catch { }
        }

        #endregion

        #region Constructor
        public RegistrationResendOTPViewModel()
        {
        }

        #endregion
    }
}