using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyPadelApp.Helpers;
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
    public partial class ResetPasswordViewModel : BaseViewModel
    {
        #region Services

        private readonly IAuthServices _authServices;

        #endregion

        #region Properties

        [ObservableProperty]
        public bool _hasPasswordError;

        [ObservableProperty]
        public string _oldPasswordError;

        [ObservableProperty]
        public bool _hasOldPasswordError;

        [ObservableProperty]
        public string _passwordError;

        [ObservableProperty]
        public string _oldPassword;

        [ObservableProperty]
        public string _newPassword;

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
                (HasOldPasswordError, OldPasswordError) = FieldValidations.IsFieldNotEmpty(OldPassword, AppResources.PasswordRequired);
                (HasPasswordError, PasswordError) = FieldValidations.IsPasswordValid(NewPassword);
                (HasConfirmPasswordError, ConfirmPasswordError) = FieldValidations.ArePasswordsMatching(NewPassword, ConfirmPassword);

                if (!HasConfirmPasswordError && !HasPasswordError && !HasOldPasswordError)
                {
                    IsBusy = true;
                    var Data = new { email = Utils.GetUser.email, currentPassword = OldPassword, newPassword = NewPassword };
                    var response = await _authServices.UpdatePassword(Data);
                    if (response != null && response.code != null && response.code.Equals("0000"))
                    {
                        Preferences.Default.Set("Password", NewPassword);
                        await Shell.Current.DisplayAlert(AppResources.Success, AppResources.PasswordUpdated, AppResources.OK);
                        await Shell.Current.GoToAsync("..");
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

        #endregion

        #region Constructor
        public ResetPasswordViewModel(IAuthServices authServices)
        {
            _authServices = authServices;
        }

        #endregion
    }
}