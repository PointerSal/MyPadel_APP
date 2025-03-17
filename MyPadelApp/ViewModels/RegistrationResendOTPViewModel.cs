using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyPadelApp.Helpers;
using MyPadelApp.Models;
using MyPadelApp.Models.Responses;
using MyPadelApp.Resources.Languages;
using MyPadelApp.Services.AuthServices;
using MyPadelApp.ViewModels.ViewBaseModel;

namespace MyPadelApp.ViewModels
{
    public partial class RegistrationResendOTPViewModel : BaseViewModel
    {
        #region Services

        private readonly IAuthServices _authServices;

        #endregion

        #region Properties

        [ObservableProperty]
        public bool _hasPhoneError;

        [ObservableProperty]
        public string _phoneError;

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
                (HasPhoneError, PhoneError) = FieldValidations.IsItalianPhoneNumberValid("+39" + UserData.cell);

                if (!HasPhoneError)
                {
                    var response = new GeneralResponse();
                    var Data = new User { cell = "+39" + UserData.cell, email = Utils.GetUser.email };
                    response = await _authServices.AddPhoneNumber(Data);

                    if (response != null && response.code != null && response.code.Equals("0000"))
                    {
                        IsOTPSend = true;
                        await Shell.Current.DisplayAlert(AppResources.Success, AppResources.PhoneResent, AppResources.OK);
                        StartTimer();
                    }
                    else if (response != null && response.code != null)
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
                (HasPhoneError, PhoneError) = FieldValidations.IsItalianPhoneNumberValid("+39" + UserData.cell);
                (HasOTPError, OTPError) = FieldValidations.IsFieldNotEmpty(UserData.otp, AppResources.OTPRequired);

                if (!HasPhoneError && !HasOTPError)
                {
                    IsBusy = true;
                    var Data = new User { otp = UserData.otp, cell = "+39" + UserData.cell };
                    var response = await _authServices.VerifyPhone(Data);
                    if (response != null && response.code != null && response.code.Equals("0000"))
                    {
                        Utils.GetUser.isPhoneVerified = true;
                        await Shell.Current.GoToAsync("../FinalStepPage");
                    }
                    else if (response != null && response.code != null)
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
        public RegistrationResendOTPViewModel(IAuthServices authServices)
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
            {
                await Shell.Current.GoToAsync("..");
                //if (Type.Equals("login"))
                //    await Shell.Current.GoToAsync("..");
                //else
                //    await Shell.Current.GoToAsync("../..");
            }
        }

        #endregion
    }
}