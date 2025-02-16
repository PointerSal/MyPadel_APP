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
    public partial class RegisterViewModel : BaseViewModel
    {
        #region Services

        private readonly IAuthServices _authServices;

        #endregion

        #region Properties

        [ObservableProperty]
        public bool _hasNameError;

        [ObservableProperty]
        public string _nameError;

        [ObservableProperty]
        public bool _hasSurnameError;

        [ObservableProperty]
        public string _surnameError;

        [ObservableProperty]
        public bool _hasEmailError;

        [ObservableProperty]
        public string _emailError;

        [ObservableProperty]
        public bool _hasPasswordError;

        [ObservableProperty]
        public string _passwordError;

        [ObservableProperty]
        public bool isTermsAccepted;

        [ObservableProperty]
        public bool isPrivacyAccepted;

        [ObservableProperty]
        public bool isMarketingAccepted;

        [ObservableProperty]
        public bool hasCheckboxError;

        [ObservableProperty]
        public string checkboxError;

        #endregion

        #region Commands

        [RelayCommand]
        public async Task Register()
        {
            try
            {
                (HasNameError, NameError) = FieldValidations.IsFieldNotEmpty(UserData.name,AppResources.NameRequired);
                (HasSurnameError, SurnameError) = FieldValidations.IsFieldNotEmpty(UserData.surname, AppResources.SurnameRequired);
                (HasEmailError, EmailError) = FieldValidations.IsEmailValid(UserData.email);
                (HasPasswordError, PasswordError) = FieldValidations.IsPasswordValid(UserData.password);
                (HasCheckboxError, CheckboxError) = FieldValidations.ValidateCheckBoxes(IsTermsAccepted, IsPrivacyAccepted, IsMarketingAccepted);

                if (!HasNameError && !HasSurnameError && !HasEmailError && !HasPasswordError && !HasCheckboxError)
                {
                    IsBusy = true;
                    var response = await _authServices.RegisterUser(UserData);
                    if (response != null && response.code !=null && response.code.Equals("0000"))
                    {
                        Utils.GetUser = null;
                        Utils.GetUser = JsonSerializer.Deserialize<User>(response.data.ToString());
                        Preferences.Default.Set("Token", Utils.GetUser.token);
                        Utils.GetUser.password = UserData.password;
                        await Shell.Current.GoToAsync("ResendEmailPage");
                    }
                    else if (response != null && response.code !=null)
                        await Shell.Current.DisplayAlert(AppResources.Error, response.message, AppResources.OK);
                    else
                        await Shell.Current.DisplayAlert(AppResources.Error, AppResources.SomethingWrong, AppResources.OK);
                }
            }
            catch(Exception ex) { }
            IsBusy = false;
        }

        [RelayCommand]
        private void AcceptTerms()
        {
            IsTermsAccepted = !IsTermsAccepted;
        }

        [RelayCommand]
        private void PrivacyTerms()
        {
            IsPrivacyAccepted = !IsPrivacyAccepted;
        }

        [RelayCommand]
        private void MarketingTerms()
        {
            IsMarketingAccepted = !IsMarketingAccepted;
        }

        [RelayCommand]
        public async Task Back()
        {
            try
            {
                await Shell.Current.GoToAsync("..");
            }
            catch { }
        }

        #endregion

        #region Constructor
        public RegisterViewModel(IAuthServices authServices)
        {
            _authServices = authServices;
        }

        #endregion
    }
}
