using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyPadelApp.Helpers;
using MyPadelApp.Models;
using MyPadelApp.Resources.Languages;
using MyPadelApp.Services.MembershipUserServices;
using MyPadelApp.ViewModels.ViewBaseModel;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MyPadelApp.ViewModels
{
    public partial class MedicalCertificateViewModel : BaseViewModel
    {
        #region Services

        private readonly IMembershipUserService _membershipUserService;

        #endregion

        #region Properties

        [ObservableProperty]
        public Card _card;

        [ObservableProperty]
        private DateTime _expiryDate;
        
        [ObservableProperty]
        private string _expiryDateError;

        [ObservableProperty]
        private bool _hasExpiryDateError;

        [ObservableProperty]
        private string _uploadCertificateError;

        [ObservableProperty]
        private bool _hasUploadCertificateError;

        private string CertificateFilePath = "";

        private byte[] _certificate;

        #endregion

        #region Commands
        public async void OnPageAppearing()
        {
            try
            {
                IsBusy = true;
                var response = await _membershipUserService.ExpiryDate(Preferences.Default.Get("username", string.Empty));
                if (response != null && response.code != null && response.code.Equals("0000"))
                {
                    Card = JsonSerializer.Deserialize<Card>(response.data.ToString());
                    ExpiryDate = Card.expiryDate ?? new DateTime();
                }
                else if (response != null && response.code != null)
                    await Shell.Current.DisplayAlert(AppResources.Error, response.message, AppResources.OK);
                else
                    await Shell.Current.DisplayAlert(AppResources.Error, AppResources.SomethingWrong, AppResources.OK);
            }
            catch { }
            IsBusy = false;
        }

        [RelayCommand]
        public async Task UpdateCertificate()
        {
            try
            {
                if (!ValidateFields())
                    return;

                IsBusy = true;
                var CertificatePic = Convert.ToBase64String(_certificate);
                var response = await _membershipUserService.UpdateFitMemberShipUser(CertificatePic);
                if (response != null && response.code != null && response.code.Equals("0000"))
                {
                    CertificateFilePath = string.Empty;
                    await Shell.Current.DisplayAlert(AppResources.Success, AppResources.CertificateUpdate, AppResources.OK);
                }
                else if (response != null && response.code != null)
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
                        var Bytes = memoryStream.ToArray();
                        byte[] compressedImage = CompressImage(Bytes, 800, 600, 70);
                        _certificate = compressedImage;
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
        public byte[] CompressImage(byte[] imageData, int maxWidth, int maxHeight, int quality = 75)
        {
            try
            {
                using var inputStream = new MemoryStream(imageData);
                using var original = SKBitmap.Decode(inputStream);

                if (original == null)
                    return Array.Empty<byte>();

                int newWidth = maxWidth;
                int newHeight = maxHeight;

                // Maintain aspect ratio
                if (original.Width > maxWidth || original.Height > maxHeight)
                {
                    float aspectRatio = (float)original.Width / original.Height;
                    if (original.Width > original.Height)
                    {
                        newHeight = (int)(maxWidth / aspectRatio);
                    }
                    else
                    {
                        newWidth = (int)(maxHeight * aspectRatio);
                    }
                }

                using var resized = original.Resize(new SKImageInfo(newWidth, newHeight), SKFilterQuality.High);
                using var image = SKImage.FromBitmap(resized);
                using var outputStream = new MemoryStream();

                // Encode image with quality factor
                image.Encode(SKEncodedImageFormat.Jpeg, quality).SaveTo(outputStream);

                return outputStream.ToArray();
            }
            catch (Exception ex)
            {
                return Array.Empty<byte>(); // Handle error gracefully
            }
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
        public MedicalCertificateViewModel(IMembershipUserService membershipUserService)
        {
            _membershipUserService = membershipUserService;
            OnPageAppearing();
        }

        #endregion

        #region Validation
        public bool ValidateFields()
        {
            bool isValid = true;

            //if (ExpiryDate <= DateTime.Now)
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

            return isValid;
        }

        #endregion
    }
}