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
                    if (response != null && response.code != null && response.code.Equals("0000"))
                    {
                        Utils.GetUser = null;
                        Utils.GetUser = JsonSerializer.Deserialize<User>(response.data.ToString());
                        Utils.GetUser.password = UserData.password;
                        Preferences.Default.Set("Token", Utils.GetUser.token);
                        response = new Models.Responses.GeneralResponse();
                        if (Utils.GetUser.isEmailVerified == true && Utils.GetUser.isPhoneVerified == true && Utils.GetUser.isFitMember == true)
                        {
                            Preferences.Default.Set("username", Utils.GetUser.email);
                            Preferences.Default.Set("Password", Utils.GetUser.password);
                            await Shell.Current.Navigation.PopToRootAsync(true);
                        }
                        else if (Utils.GetUser.isEmailVerified == false)
                        {
                            var Data = new User { email = UserData.email };
                            response = await _authServices.ResendOTP(Data);
                            if (response != null && response.code != null && response.code.Equals("0000"))
                                await Shell.Current.GoToAsync("ResendEmailPage");
                            else if (response != null && response.code != null)
                                await Shell.Current.DisplayAlert(AppResources.Error, response.message, AppResources.OK);
                            else
                                await Shell.Current.DisplayAlert(AppResources.Error, AppResources.SomethingWrong, AppResources.OK);
                        }
                        else if (Utils.GetUser.isPhoneVerified == false)
                            await Shell.Current.GoToAsync("RegistrationResendOTPPage");
                        else if (Utils.GetUser.isFitMember == false)
                            await Shell.Current.GoToAsync("FinalStepPage");
                    }
                    else if (response != null && response.code != null)
                        await Shell.Current.DisplayAlert(AppResources.Error, response.message, AppResources.OK);
                    else
                        await Shell.Current.DisplayAlert(AppResources.Error, AppResources.SomethingWrong, AppResources.OK);
                }
            }
            catch(Exception ex) { }
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