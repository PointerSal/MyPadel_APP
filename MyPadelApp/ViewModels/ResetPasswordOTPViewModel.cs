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
    public partial class ResetPasswordOTPViewModel : BaseViewModel
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
        public bool _hasOTPError;

        [ObservableProperty]
        public string _oTPError;

        [ObservableProperty]
        public bool _isOTPSend;

        [ObservableProperty]
        private bool _isResendVisible = false;

        [ObservableProperty]
        private string _timerText = "00:00";

        private IDispatcherTimer _timer;
        private int _timerCount = 60;

        #endregion

        #region Commands

        [RelayCommand]
        public async Task SendCode()
        {
            try
            {
                IsBusy = true;
                (HasEmailError, EmailError) = FieldValidations.IsEmailValid(UserData.email);

                if (!HasEmailError)
                {
                    var Data = new User { email = UserData.email };
                    var response = await _authServices.ResendOTP(Data);
                    if (response != null && response.code !=null && response.code.Equals("0000"))
                    {
                        IsOTPSend = true;
                        await Shell.Current.DisplayAlert(AppResources.Success, AppResources.EmailResent, AppResources.OK);
                        StartTimer();
                    }
                    else if (response != null && response.code !=null)
                        await Shell.Current.DisplayAlert(AppResources.Error, response.message, AppResources.OK);
                    else
                        await Shell.Current.DisplayAlert(AppResources.Error, AppResources.SomethingWrong, AppResources.OK);
                }
            }
            catch { }
            IsBusy = false;
        }

        [RelayCommand]
        public async Task Confirm()
        {
            try
            {
                (HasEmailError, EmailError) = FieldValidations.IsEmailValid(UserData.email);
                (HasOTPError, OTPError) = FieldValidations.IsFieldNotEmpty(UserData.otp, AppResources.OTPRequired);

                if (!HasEmailError && !HasOTPError)
                {
                    IsBusy = true;
                    var response = await _authServices.VerifyEmail(UserData);
                    if (response != null && response.code !=null && response.code.Equals("0000"))
                    {
                        Utils.GetUser = null;
                        Utils.GetUser = new User();
                        Utils.GetUser.email = UserData.email;
                        await Shell.Current.GoToAsync("PasswordChangedPage");
                    }
                    else if (response != null && response.code !=null)
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
        public ResetPasswordOTPViewModel(IAuthServices authServices)
        {
            _authServices = authServices;
        }

        #endregion

        #region Methods
        private void StartTimer()
        {
            try
            {
                _timerCount = 60;
                IsResendVisible = false;
                TimerText = FormatTime(_timerCount);

                _timer?.Stop();
                _timer = Dispatcher.GetForCurrentThread().CreateTimer();
                _timer.Interval = TimeSpan.FromSeconds(1);
                _timer.Tick += (s, e) =>
                {
                    _timerCount--;
                    TimerText = FormatTime(_timerCount);

                    if (_timerCount <= 0)
                    {
                        _timer.Stop();
                        IsResendVisible = true;
                        TimerText = "00:00";
                    }
                };
                _timer.Start();
            }
            catch (Exception ex) { }
        }
        private string FormatTime(int seconds)
        {
            try
            {
                TimeSpan time = TimeSpan.FromSeconds(seconds);
                return time.ToString(@"mm\:ss");
            }
            catch { }
            return seconds.ToString();
        }
        public void StopTimer()
        {
            if (_timer != null && _timer.IsRunning)
            {
                TimerText = "00:00";
                _timer.Stop();
                _timer = null;
                Console.WriteLine("Timer Stopped");
            }
        }
        public async void OnBack()
        {
            if (IsOTPSend)
            {
                StopTimer();
                IsOTPSend = false;
            }
            else
                await Shell.Current.GoToAsync("..");
        }

        #endregion
    }
}