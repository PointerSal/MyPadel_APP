using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyPadelApp.Helpers;
using MyPadelApp.Models;
using MyPadelApp.Resources.Languages;
using MyPadelApp.Services.AuthServices;
using MyPadelApp.ViewModels.ViewBaseModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MyPadelApp.ViewModels
{
    public partial class LoginViewModel : BaseViewModel
    {
        #region Services

        private readonly IAuthServices _authServices;

        #endregion

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
                (HasEmailError, EmailError) = FieldValidations.IsEmailValid(UserData.email);
                (HasPasswordError, PasswordError) = FieldValidations.IsFieldNotEmpty(UserData.password, AppResources.PasswordRequired);

                if (!HasEmailError && !HasPasswordError)
                {
                    IsBusy = true;
                    var response = await _authServices.Login(UserData);
                    if (response != null && response.code.Equals("0000"))
                    {
                        Utils.GetUser = null;
                        Utils.GetUser = JsonSerializer.Deserialize<User>(response.data.ToString());
                        Utils.GetUser.password = UserData.password;
                        if (Utils.GetUser.isEmailVerified == true && Utils.GetUser.isPhoneVerified == true)
                        {
                            await SecureStorage.Default.SetAsync("username", Utils.GetUser.email);
                            await SecureStorage.Default.SetAsync("Password", Utils.GetUser.password);
                            //await Shell.Current.GoToAsync("//Home");
                            await Shell.Current.DisplayAlert("Success", "Account login successfully", "OK");
                        }
                        else if(Utils.GetUser.isEmailVerified == false)
                        {
                            var Data = new User { email = UserData.email };
                            response = await _authServices.ResendOTP(Data);
                            if (response != null && response.code.Equals("0000"))
                                await Shell.Current.GoToAsync("ResendEmailPage");
                            else if (response != null)
                                await Shell.Current.DisplayAlert(AppResources.Error, response.message, AppResources.OK);
                            else
                                await Shell.Current.DisplayAlert(AppResources.Error, AppResources.SomethingWrong, AppResources.OK);
                        }
                        else if (Utils.GetUser.isEmailVerified == true && Utils.GetUser.isPhoneVerified == false)
                            await Shell.Current.GoToAsync("RegistrationResendOTPPage", true, new Dictionary<string, object>
                            {
                                {"type", "login" }
                            });
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
        public LoginViewModel(IAuthServices authServices)
        {
            _authServices = authServices;
        }

        #endregion
    }
}