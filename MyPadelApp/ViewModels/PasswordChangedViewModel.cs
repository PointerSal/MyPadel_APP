using Android.Service.Autofill;
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
    public partial class PasswordChangedViewModel : BaseViewModel
    {
        #region Services

        private readonly IAuthServices _authServices;

        #endregion

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
                (HasPasswordError, PasswordError) = FieldValidations.IsPasswordValid(UserData.password);
                (HasConfirmPasswordError, ConfirmPasswordError) = FieldValidations.ArePasswordsMatching(UserData.password, ConfirmPassword);

                if (!HasConfirmPasswordError && !HasPasswordError)
                {
                    IsBusy = true;
                    UserData.email = Utils.GetUser.email;
                    var Data = new { newPassword = UserData.password, email = UserData.email };
                    var response = await _authServices.ResetPassword(Data);
                    if (response != null && response.code.Equals("0000"))
                    {
                        var APIResponse = await _authServices.Login(UserData);
                        if (APIResponse != null && APIResponse.code.Equals("0000"))
                            Utils.GetUser = JsonSerializer.Deserialize<User>(APIResponse.data.ToString());
                        Utils.GetUser.password = UserData.password;
                        await SecureStorage.Default.SetAsync("username", Utils.GetUser.email);
                        await SecureStorage.Default.SetAsync("Password", Utils.GetUser.password);
                        await Shell.Current.DisplayAlert(AppResources.Success, AppResources.PasswordUpdated, AppResources.OK);
                        await Shell.Current.GoToAsync("../..");
                        //await Shell.Current.GoToAsync("//Home");
                    }
                    else if (response != null)
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
        public PasswordChangedViewModel(IAuthServices authServices)
        {
            _authServices = authServices;
        }

        #endregion
    }
}