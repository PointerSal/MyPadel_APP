using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyPadelApp.Helpers;
using MyPadelApp.Models;
using MyPadelApp.Resources.Languages;
using MyPadelApp.ViewModels.ViewBaseModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MyPadelApp.ViewModels
{
    public partial class LoginViewModel : BaseViewModel
    {
        #region Properties

        [ObservableProperty]
        public bool _hasEmailError;

        [ObservableProperty]
        public string _emailError;

        [ObservableProperty]
        public bool _hasPasswordError;

        [ObservableProperty]
        public string _passwordError;

        #endregion

        #region Commands

        [RelayCommand]
        public async Task Login()
        {
            try
            {
                (HasEmailError, EmailError) = FieldValidations.IsEmailValid(UserData.Email);
                (HasPasswordError, PasswordError) = FieldValidations.IsFieldNotEmpty(UserData.Password, AppResources.PasswordRequired);

                if (!HasEmailError && !HasPasswordError)
                    await Shell.Current.GoToAsync("//Home");
            }
            catch { }
        }

        [RelayCommand]
        public async Task RecoverPassword()
        {
            try
            {
                await Shell.Current.GoToAsync("ResetPasswordOTPPage");
            }
            catch { }
        }

        #endregion

        #region Constructor
        public LoginViewModel()
        {
        }

        #endregion
    }
}