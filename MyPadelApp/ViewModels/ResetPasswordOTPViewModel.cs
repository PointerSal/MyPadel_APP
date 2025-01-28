using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyPadelApp.Helpers;
using MyPadelApp.Models;
using MyPadelApp.Resources.Languages;
using MyPadelApp.ViewModels.ViewBaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPadelApp.ViewModels
{
    public partial class ResetPasswordOTPViewModel : BaseViewModel
    {
        #region Properties

        [ObservableProperty]
        public bool _hasEmailError;

        [ObservableProperty]
        public string _emailError;

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
                (HasEmailError, EmailError) = FieldValidations.IsEmailValid(UserData.Email);
                (HasOTPError, OTPError) = FieldValidations.IsFieldNotEmpty(UserData.OTP, AppResources.OTPRequired);

                if (!HasEmailError && !HasOTPError)
                    await Shell.Current.GoToAsync("PasswordChangedPage");
            }
            catch { }
        }

        #endregion

        #region Constructor
        public ResetPasswordOTPViewModel()
        {
        }

        #endregion
    }
}