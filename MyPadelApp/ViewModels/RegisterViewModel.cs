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
    public partial class RegisterViewModel : BaseViewModel
    {
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
                (HasNameError, NameError) = FieldValidations.IsFieldNotEmpty(UserData.Name,AppResources.NameRequired);
                (HasSurnameError, SurnameError) = FieldValidations.IsFieldNotEmpty(UserData.Surname, AppResources.SurnameRequired);
                (HasEmailError, EmailError) = FieldValidations.IsEmailValid(UserData.Email);
                (HasPasswordError, PasswordError) = FieldValidations.IsPasswordValid(UserData.Password);
                (HasCheckboxError, CheckboxError) = FieldValidations.ValidateCheckBoxes(IsTermsAccepted, IsPrivacyAccepted, IsMarketingAccepted);

                if (!HasNameError && !HasSurnameError && !HasEmailError && !HasPasswordError && !HasCheckboxError)
                    await Shell.Current.GoToAsync("ResendEmailPage");
            }
            catch { }
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
    }
}
