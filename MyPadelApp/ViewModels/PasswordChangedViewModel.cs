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
    public partial class PasswordChangedViewModel : BaseViewModel
    {
        #region Properties

        [ObservableProperty]
        public bool _hasPasswordError;

        [ObservableProperty]
        public string _passwordError;

        [ObservableProperty]
        public string _confirmPassword;

        [ObservableProperty]
        public bool _hasConfirmPasswordError;

        [ObservableProperty]
        public string _confirmPasswordError;

        #endregion

        #region Commands

        [RelayCommand]
        public async Task UpdatePassword()
        {
            try
            {
                (HasPasswordError, PasswordError) = FieldValidations.IsPasswordValid(UserData.Password);
                (HasConfirmPasswordError, ConfirmPasswordError) = FieldValidations.ArePasswordsMatching(UserData.Password, ConfirmPassword);

                if (!HasConfirmPasswordError && !HasPasswordError)
                {
                    await Shell.Current.DisplayAlert(AppResources.Success, AppResources.PasswordUpdated, AppResources.OK);
                    await Shell.Current.GoToAsync("//Home");
                }
            }
            catch { }
        }

        #endregion
    }
}
