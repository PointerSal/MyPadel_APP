using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyPadelApp.Helpers;
using MyPadelApp.Models;
using MyPadelApp.Resources.Languages;
using MyPadelApp.Services.MembershipUserServices;
using MyPadelApp.ViewModels.ViewBaseModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPadelApp.ViewModels
{
    public partial class CreateFIFViewModel : BaseViewModel
    {
        #region Services

        private readonly IMembershipUserService _membershipUserService;

        #endregion

        #region Properties

        [ObservableProperty]
        private MembershipRequestModel _cardModel = new();

        [ObservableProperty]
        private string _cardNumberError;

        [ObservableProperty]
        private bool _hasCardNumberError;

        [ObservableProperty]
        private string _expiryDateError;

        [ObservableProperty]
        private bool _hasExpiryDateError;

        [ObservableProperty]
        private string _certificateDateError;

        [ObservableProperty]
        private bool _hasCertificateDateError;

        [ObservableProperty]
        private string _firstNameError;

        [ObservableProperty]
        public bool _hasFirstNameError;

        [ObservableProperty]
        private string _lastNameError;

        [ObservableProperty]
        public bool _hasLastNameError;

        [ObservableProperty]
        private string _birthDateError;

        [ObservableProperty]
        public bool _hasBirthDateError;

        [ObservableProperty]
        private string _genderError;

        [ObservableProperty]
        public bool _hasGenderError;

        [ObservableProperty]
        private string _addressError;

        [ObservableProperty]
        public bool _hasAddressError;

        [ObservableProperty]
        private string _zipCodeError;

        [ObservableProperty] 
        public bool _hasZipCodeError;

        [ObservableProperty]
        private string _selectedPaymentMethod;
        
        [ObservableProperty]
        private string _municipalityError;

        [ObservableProperty]
        public bool _hasMunicipalityError;
        public ObservableCollection<string> PaymentMethods { get; } = new()
        {
            "PayPal",
            "CreditCard",
            "Satispay"
        };

        [ObservableProperty]
        public ObservableCollection<string> _gender = new()
        {
            AppResources.Male,
            AppResources.Female
        };

        [ObservableProperty]
        private string _paymentmethodError;

        [ObservableProperty]
        private bool _hasPaymentmethodError;

        [ObservableProperty]
        private string _uploadCertificateError;

        [ObservableProperty]
        private bool _hasUploadCertificateError;

        private string CertificateFilePath = "";

        #endregion

        #region Commands

        [RelayCommand]
        public async Task AddCard()
        {
            try
            {
                if (!ValidateFields())
                    return;

                IsBusy = true;
                CardModel.PaymentMethod = SelectedPaymentMethod;
                var response = await _membershipUserService.RegisterMemberShipUser(CardModel, ImageToBase64.BytesToBase64(CardModel.MedicalCertificate));
                if (response != null && response.code !=null && response.code.Equals("0000"))
                {
                    Preferences.Default.Set("username", Utils.GetUser.email);
                    Preferences.Default.Set("Password", Utils.GetUser.password);
                    await Shell.Current.GoToAsync("../../BookedFieldPage");
                }
                else if (response != null && response.code !=null)
                    await Shell.Current.DisplayAlert(AppResources.Error, response.message, AppResources.OK);
                else
                    await Shell.Current.DisplayAlert(AppResources.Error, AppResources.SomethingWrong, AppResources.OK);
            }
            catch (Exception ex) { }
            IsBusy = false;
        }

        [RelayCommand]
        public async Task OpenCamera()
        {
            try
            {
                if (!MediaPicker.Default.IsCaptureSupported)
                {
                    await Shell.Current.DisplayAlert(AppResources.NotSupported, AppResources.NotSupportedCamera, AppResources.OK);
                    return;
                }

                var cameraStatus = await Permissions.RequestAsync<Permissions.Camera>();
                var storageStatus = await Permissions.RequestAsync<Permissions.StorageWrite>();

                if (cameraStatus != PermissionStatus.Granted || storageStatus != PermissionStatus.Granted)
                {
                    await Shell.Current.DisplayAlert(AppResources.PermissionDenied, AppResources.PermissionNotGranted, AppResources.OK);
                    return;
                }

                var photo = await MediaPicker.Default.CapturePhotoAsync();
                if (photo == null)
                {
                    UploadCertificateError = AppResources.NoFileChosen;
                    HasUploadCertificateError = true;
                    return;
                }

                CertificateFilePath = photo.FullPath;

                using (var stream = await photo.OpenReadAsync())
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await stream.CopyToAsync(memoryStream);
                        CardModel.MedicalCertificate = memoryStream.ToArray();
                    }
                }

                UploadCertificateError = string.Empty;
                HasUploadCertificateError = false;
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert(AppResources.Error, AppResources.FailCapturePhoto, AppResources.OK);
            }
        }

        [RelayCommand]
        public async Task NewCard()
        {
            try
            {
                await Shell.Current.GoToAsync("../CreateFIFPage");
            }
            catch { }
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
        public CreateFIFViewModel(IMembershipUserService membershipUserService)
        {
            _membershipUserService = membershipUserService;
        }

        #endregion

        #region Validation
        public bool ValidateFields()
        {
            bool isValid = true;

            if (string.IsNullOrWhiteSpace(CardModel.FirstName))
            {
                FirstNameError = AppResources.NameRequired;
                HasFirstNameError = true;
                isValid = false;
            }
            else
            {
                FirstNameError = string.Empty;
                HasFirstNameError = false;
            }

            if (string.IsNullOrWhiteSpace(CardModel.LastName))
            {
                LastNameError = AppResources.SurnameRequired;
                HasLastNameError = true;
                isValid = false;
            }
            else
            {
                LastNameError = string.Empty;
                HasLastNameError = false;
            }

            if (CardModel.BirthDate > DateTime.Now.AddYears(-7))
            {
                BirthDateError = AppResources.InvalidBrithDate;
                HasBirthDateError = true;
                isValid = false;
            }
            else
            {
                BirthDateError = string.Empty;
                HasBirthDateError = false;
            }

            if (string.IsNullOrWhiteSpace(CardModel.Gender))
            {
                GenderError = AppResources.GenderSelected;
                HasGenderError = true;
                isValid = false;
            }
            else
            {
                GenderError = string.Empty;
                HasGenderError = false;
            }

            if (string.IsNullOrWhiteSpace(CardModel.Address))
            {
                AddressError = AppResources.AddressRequired;
                HasAddressError = true;
                isValid = false;
            }
            else
            {
                AddressError = string.Empty;
                HasAddressError = false;
            }

            if (string.IsNullOrWhiteSpace(CardModel.PostalCode))
            {
                ZipCodeError = AppResources.PostalCodeRequired;
                HasZipCodeError = true;
                isValid = false;
            }
            else if (!CardModel.PostalCode.All(char.IsDigit) || CardModel.PostalCode.Length < 5 || CardModel.PostalCode.Length > 10)
            {
                ZipCodeError = AppResources.Invalidpostalcode;
                HasZipCodeError = true;
                isValid = false;
            }
            else
            {
                ZipCodeError = string.Empty;
                HasZipCodeError = false;
            }

            if (string.IsNullOrWhiteSpace(CardModel.Municipality))
            {
                MunicipalityError = AppResources.MunicipalityRequried;
                HasMunicipalityError = true;
                isValid = false;
            }
            else
            {
                MunicipalityError = string.Empty;
                HasMunicipalityError = false;
            }

            if (string.IsNullOrWhiteSpace(CardModel.CardNumber))
            {
                CardNumberError = AppResources.CardNumberRequired;
                HasCardNumberError = true;
                isValid = false;
            }
            else if (!CardModel.CardNumber.All(char.IsDigit) || CardModel.CardNumber.Length != 16)
            {
                CardNumberError = AppResources.Invalidcardnumber;
                HasCardNumberError = true;
                isValid = false;
            }
            else
            {
                CardNumberError = string.Empty;
                HasCardNumberError = false;
            }

            if (CardModel.ExpiryDate <= DateTime.Now)
            {
                ExpiryDateError = AppResources.ExpiryDateRequired;
                HasExpiryDateError = true;
                isValid = false;
            }
            else
            {
                ExpiryDateError = string.Empty;
                HasExpiryDateError = false;
            }

            if (CardModel.MedicalCertificateDate > DateTime.Now)
            {
                CertificateDateError = AppResources.InvalidCertificateDate;
                HasCertificateDateError = true;
                isValid = false;
            }
            else
            {
                CertificateDateError = string.Empty;
                HasCertificateDateError = false;
            }

            if (string.IsNullOrWhiteSpace(CertificateFilePath))
            {
                UploadCertificateError = AppResources.MedicalCertificateRequired;
                HasUploadCertificateError = true;
                isValid = false;
            }
            else
            {
                UploadCertificateError = string.Empty;
                HasUploadCertificateError = false;
            }

            if (string.IsNullOrWhiteSpace(SelectedPaymentMethod))
            {
                PaymentmethodError = AppResources.PaymentMethodSelected;
                HasPaymentmethodError = true;
                isValid = false;
            }
            else
            {
                PaymentmethodError = string.Empty;
                HasPaymentmethodError = false;
            }

            return isValid;
        }


        #endregion
    }
}