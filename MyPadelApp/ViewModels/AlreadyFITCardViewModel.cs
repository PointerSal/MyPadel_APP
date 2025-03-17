﻿using CommunityToolkit.Mvvm.ComponentModel;
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
    public partial class AlreadyFITCardViewModel : BaseViewModel
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
                if (!ValidateInputs())
                    return;

                IsBusy = true;
                var response = await _membershipUserService.RegisterFitMemberShipUser(CardModel, ImageToBase64.BytesToBase64(CardModel.MedicalCertificate));
                if (response != null && response.code !=null && response.code.Equals("0000"))
                {
                    Utils.GetUser.isFitVerified = false;
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
                        // Compress and reduce image size
                        await stream.CopyToAsync(memoryStream);
                        var Bytes = memoryStream.ToArray();
                        byte[] compressedImage = CompressImage(Bytes, 800, 600, 70);
                        CardModel.MedicalCertificate = compressedImage;
                    }
                }

                UploadCertificateError = string.Empty;
                HasUploadCertificateError = false;
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert(AppResources.Error, $"{AppResources.FailCapturePhoto}\n{ex.Message}", AppResources.OK);
            }
        }

        /// <summary>
        /// Resizes and compresses an image using SkiaSharp.
        /// </summary>
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
        public AlreadyFITCardViewModel(IMembershipUserService membershipUserService)
        {
            _membershipUserService = membershipUserService;
        }

        #endregion

        #region Validation

        private bool ValidateInputs()
        {
            bool isValid = true;

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

            if (CardModel.ExpiryDate < DateTime.Now.AddMonths(1))
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

            if (CardModel.MedicalCertificateDate == null)
            {
                CertificateDateError = AppResources.MedicalCertificateDateRequired;
                HasCertificateDateError = true;
                isValid = false;
            }
            else if (CardModel.MedicalCertificateDate > DateTime.Now)
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

            if (string.IsNullOrWhiteSpace(CertificateFilePath) || !File.Exists(CertificateFilePath))
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
