using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyPadelApp.Helpers;
using MyPadelApp.Models;
using MyPadelApp.Resources.Languages;
using MyPadelApp.Services.MembershipUserServices;
using MyPadelApp.Services.PriceServices;
using MyPadelApp.Services.StripeServices;
using MyPadelApp.ViewModels.ViewBaseModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MyPadelApp.ViewModels
{
    public partial class CreateFIFViewModel : BaseViewModel
    {
        #region Services

        private readonly IStripeService _stripeService;
        private readonly IPriceService _priceService;

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
        private string _taxCodeError;

        [ObservableProperty]
        public bool _hasTaxCodeError;

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
        private string _phoneError;

        [ObservableProperty] 
        public bool _hasPhoneError;

        [ObservableProperty]
        private string _selectedPaymentMethod;
        
        [ObservableProperty]
        private string _municipalityError;

        [ObservableProperty]
        public bool _hasMunicipalityError;
        
        [ObservableProperty]
        private string _phoneErrorError;

        [ObservableProperty]
        public bool _hasPhoneErrorError;

        [ObservableProperty]
        private string _brithProvinceError;

        [ObservableProperty]
        public bool _hasBrithProvinceError;

        [ObservableProperty]
        private string _brithMunicipalityError;

        [ObservableProperty]
        public bool _hasBrithMunicipalityError;

        [ObservableProperty]
        private string _citizenShipsError;

        [ObservableProperty]
        public bool _hasCitizenShipsError;
        
        [ObservableProperty]
        private string _residenceProvinceError;

        [ObservableProperty]
        public bool _hasResidenceProvinceError;

        [ObservableProperty]
        private string _residenceMunicipalityError;

        [ObservableProperty]
        public bool _hasResidenceMunicipalityError;
        public ObservableCollection<string> PaymentMethods { get; } = new()
        {
            //"PayPal",
            "CreditCard"
            //"Satispay"
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
        private BookingPrices bookingPrices = null;

        [ObservableProperty]
        public ObservableCollection<string> _brithProvince = new();
        
        [ObservableProperty]
        public ObservableCollection<string> _brithMunicipality = new();

        [ObservableProperty]
        public ObservableCollection<string> _residenceProvince = new();

        [ObservableProperty]
        public ObservableCollection<string> _residenceMunicipality = new();

        [ObservableProperty]
        public ObservableCollection<string> _citizenShips = new();

        private Dictionary<string, List<string>> _provinceCities = new();

        [ObservableProperty]
        private string _selectedBrithProvince;

        [ObservableProperty]
        private string _selectedResidenceProvince;

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
                CardModel.provinceOfBirth = SelectedBrithProvince;
                CardModel.provinceOfResidence = SelectedResidenceProvince;

                bool IsSuccess = false;
                if (bookingPrices == null)
                    IsSuccess = await GetPrices();
                else
                    IsSuccess = true;

                if (IsSuccess)
                {
                    var Data = new StripePayment { email = Utils.GetUser.email, amount = bookingPrices.fitMembershipFee };
                    var response = await _stripeService.CheckoutSession(Data);
                    if (response != null && response.code != null && response.code.Equals("0000"))
                    {
                        var result = JsonSerializer.Deserialize<PaymentResponse>(response.data.ToString());
                        await Shell.Current.GoToAsync("FITPaymentPage", true, new Dictionary<string, object>
                        {
                            { "PaymentURL",result.sessionUrl },
                            { "CurrentFIT",CardModel },
                        });
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
                var mediaStatus = await Permissions.RequestAsync<Permissions.Media>();

                if (cameraStatus != PermissionStatus.Granted || mediaStatus != PermissionStatus.Granted)
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

        public CreateFIFViewModel(IStripeService stripeService, IPriceService priceService)
        {
            _stripeService = stripeService;
            _priceService = priceService;
            SelectedPaymentMethod = "CreditCard";

            MainThread.BeginInvokeOnMainThread(async () =>
            {
                IsBusy = true;
                await GetPrices();
                await LoadDataAsync();
                IsBusy = false;
            });
        }

        #endregion

        #region Methods
        public async Task<bool> GetPrices()
        {
            try
            {
                var response = await _priceService.FITMemberShipPrices();
                if (response != null && response.code != null && response.code.Equals("0000"))
                {
                    bookingPrices = JsonSerializer.Deserialize<BookingPrices>(response.data.ToString());
                    return true;
                }
                else if (response != null && response.code != null)
                    await Shell.Current.DisplayAlert(AppResources.Error, response.message, AppResources.OK);
                else
                    await Shell.Current.DisplayAlert(AppResources.Error, AppResources.SomethingWrong, AppResources.OK);
            }
            catch (Exception ex) { }
            return false;
        }

        private async Task LoadDataAsync()
        {
            try
            {
                await Task.Run(async () =>
                {
                    Stream stream3 = await FileSystem.Current.OpenAppPackageFileAsync("citizenships.txt");
                    using (StreamReader reader = new StreamReader(stream3))
                    {
                        while (!reader.EndOfStream)
                        {
                            var line = reader.ReadLine();
                            var parts = line.Split('\t');
                            if (parts.Length == 2)
                                CitizenShips.Add(parts[1][..1].ToUpperInvariant() + parts[1][1..].ToLowerInvariant());
                        }
                    }

                    Stream stream = await FileSystem.OpenAppPackageFileAsync("provinces.txt");
                    var TempProvinces = new ObservableCollection<string>();
                    var _provinceDict = new Dictionary<string, string>();
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        while (!reader.EndOfStream)
                        {
                            var line = await reader.ReadLineAsync();
                            var parts = line.Split('\t');
                            if (parts.Length > 1) 
                            {
                                var provinceCode = parts[0].Trim();
                                var provinceName = parts[1].Trim();
                                if (!_provinceDict.ContainsKey(provinceCode))
                                    _provinceDict[provinceCode] = provinceName;
                                TempProvinces.Add(provinceName[..1].ToUpperInvariant() + provinceName[1..].ToLowerInvariant());
                            }
                        }
                    }

                    BrithProvince = new ObservableCollection<string>(TempProvinces.Select(s => s).ToList());
                    ResidenceProvince = new ObservableCollection<string>(TempProvinces.Select(s => s).ToList());

                    Stream stream2 = await FileSystem.OpenAppPackageFileAsync("cities.txt");
                    using (StreamReader reader = new StreamReader(stream2))
                    {
                        while (!reader.EndOfStream)
                        {
                            var line = await reader.ReadLineAsync();
                            var parts = line.Split('\t');
                            if (parts.Length > 1)
                            {
                                var provinceName = _provinceDict.ContainsKey(parts[0].Trim()) ? _provinceDict[parts[0].Trim()] : "";
                                var cityName = parts[1].Trim();
                                if (!_provinceCities.ContainsKey(provinceName))
                                    _provinceCities[provinceName] = new List<string>();

                                _provinceCities[provinceName].Add(cityName[..1].ToUpperInvariant() + cityName[1..].ToLowerInvariant());
                            }
                        }
                    }
                });
            }
            catch (Exception ex) { }
        }

        public void LoadCitiesForProvince(bool IsBrithProvince)
        {
            if (IsBrithProvince)
            {
                CardModel.BrithMunicipality = "";
                BrithMunicipality.Clear();
                if (!string.IsNullOrEmpty(SelectedBrithProvince) && _provinceCities.ContainsKey(SelectedBrithProvince))
                {
                    foreach (var city in _provinceCities[SelectedBrithProvince])
                    {
                        BrithMunicipality.Add(city);
                    }
                }
            }
            else
            {
                CardModel.ResidenceMunicipality = "";
                ResidenceMunicipality.Clear();
                if (!string.IsNullOrEmpty(SelectedResidenceProvince) && _provinceCities.ContainsKey(SelectedResidenceProvince))
                {
                    foreach (var city in _provinceCities[SelectedResidenceProvince])
                    {
                        ResidenceMunicipality.Add(city);
                    }
                }
            }
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

            if (string.IsNullOrWhiteSpace(CardModel.TaxCode))
            {
                TaxCodeError = AppResources.TaxCodeRequired;
                HasTaxCodeError = true;
                isValid = false;
            }
            else
            {
                TaxCodeError = string.Empty;
                HasTaxCodeError = false;
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

            if (string.IsNullOrWhiteSpace(CardModel.ResidenceMunicipality))
            {
                ResidenceMunicipalityError = AppResources.ResidenceMunicipalityRequired;
                HasResidenceMunicipalityError = true;
                isValid = false;
            }
            else
            {
                ResidenceMunicipalityError = string.Empty;
                HasResidenceMunicipalityError = false;
            }

            if (string.IsNullOrWhiteSpace(CardModel.BrithMunicipality))
            {
                BrithMunicipalityError = AppResources.BrithMunicipality;
                HasBrithMunicipalityError = true;
                isValid = false;
            }
            else
            {
                BrithMunicipalityError = string.Empty;
                HasBrithMunicipalityError = false;
            }

            if (string.IsNullOrWhiteSpace(SelectedBrithProvince))
            {
                BrithProvinceError = AppResources.BrithProvinceRequired;
                HasBrithProvinceError = true;
                isValid = false;
            }
            else
            {
                BrithProvinceError = string.Empty;
                HasBrithProvinceError = false;
            }

            if (string.IsNullOrWhiteSpace(SelectedResidenceProvince))
            {
                ResidenceProvinceError = AppResources.SelectedResidenceProvinceRequired;
                HasResidenceProvinceError = true;
                isValid = false;
            }
            else
            {
                ResidenceProvinceError = string.Empty;
                HasResidenceProvinceError = false;
            }

            if (string.IsNullOrWhiteSpace(CardModel.CitizenShips))
            {
                CitizenShipsError = AppResources.CitizenShipsRequired;
                HasCitizenShipsError = true;
                isValid = false;
            }
            else
            {
                CitizenShipsError = string.Empty;
                HasCitizenShipsError = false;
            }

            (HasPhoneError, PhoneError) = FieldValidations.IsItalianPhoneNumberValid("+39" + CardModel.Cell);
            if(HasPhoneError)
                isValid = false;

            //if (string.IsNullOrWhiteSpace(CardModel.Municipality))
            //{
            //    MunicipalityError = AppResources.MunicipalityRequried;
            //    HasMunicipalityError = true;
            //    isValid = false;
            //}
            //else
            //{
            //    MunicipalityError = string.Empty;
            //    HasMunicipalityError = false;
            //}

            //if (string.IsNullOrWhiteSpace(CardModel.CardNumber))
            //{
            //    CardNumberError = AppResources.CardNumberRequired;
            //    HasCardNumberError = true;
            //    isValid = false;
            //}
            //else if (!CardModel.CardNumber.All(char.IsDigit) || CardModel.CardNumber.Length != 16)
            //{
            //    CardNumberError = AppResources.Invalidcardnumber;
            //    HasCardNumberError = true;
            //    isValid = false;
            //}
            //else
            //{
            //    CardNumberError = string.Empty;
            //    HasCardNumberError = false;
            //}

            //if (CardModel.ExpiryDate <= DateTime.Now)
            //{
            //    ExpiryDateError = AppResources.ExpiryDateRequired;
            //    HasExpiryDateError = true;
            //    isValid = false;
            //}
            //else
            //{
            //    ExpiryDateError = string.Empty;
            //    HasExpiryDateError = false;
            //}

            //if (CardModel.MedicalCertificateDate > DateTime.Now)
            //{
            //    CertificateDateError = AppResources.InvalidCertificateDate;
            //    HasCertificateDateError = true;
            //    isValid = false;
            //}
            //else
            //{
            //    CertificateDateError = string.Empty;
            //    HasCertificateDateError = false;
            //}

            //if (string.IsNullOrWhiteSpace(CertificateFilePath))
            //{
            //    UploadCertificateError = AppResources.MedicalCertificateRequired;
            //    HasUploadCertificateError = true;
            //    isValid = false;
            //}
            //else
            //{
            //    UploadCertificateError = string.Empty;
            //    HasUploadCertificateError = false;
            //}

            //if (string.IsNullOrWhiteSpace(SelectedPaymentMethod))
            //{
            //    PaymentmethodError = AppResources.PaymentMethodSelected;
            //    HasPaymentmethodError = true;
            //    isValid = false;
            //}
            //else
            //{
            //    PaymentmethodError = string.Empty;
            //    HasPaymentmethodError = false;
            //}

            return isValid;
        }


        #endregion
    }
}