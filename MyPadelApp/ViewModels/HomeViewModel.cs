using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LocalizationResourceManager.Maui;
using MyPadelApp.Helpers;
using MyPadelApp.Models;
using MyPadelApp.Resources.Languages;
using MyPadelApp.Services.AuthServices;
using MyPadelApp.Services.BookingServices;
using MyPadelApp.ViewModels.ViewBaseModel;
using MyPadelApp.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MyPadelApp.ViewModels
{
    public partial class HomeViewModel : BaseViewModel
    {
        #region Services

        private readonly ILocalizationResourceManager _localizationResourceManager;
        private readonly IAuthServices _authServices;
        private readonly IBookingService _bookingService;

        #endregion

        #region Properties

        private bool IsClicked { get; set; } = false;
        private List<string> CurrentTimeList { get; set; } = new List<string>();
        public DateTime SelectedDate = DateTime.Now;
        public int CurrentField = 1;
        public int SelectedTimeSlot = 1;

        [ObservableProperty]
        public CalendarItem _selectedCalender;

        private ObservableCollection<CalendarItem> _CalendarItems;
        public ObservableCollection<CalendarItem> CalendarItems
        {
            get => _CalendarItems;
            set
            {
                _CalendarItems = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<TimeSlot> _TimeSlots;
        public ObservableCollection<TimeSlot> TimeSlots
        {
            get => _TimeSlots;
            set
            {
                _TimeSlots = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<TimeSlot> _TimeSlots2;
        public ObservableCollection<TimeSlot> TimeSlots2
        {
            get => _TimeSlots2;
            set
            {
                _TimeSlots2 = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Commands

        [RelayCommand]
        private async Task Padel1(TimeSlot timeSlot)
        {
            try
            {
                if (timeSlot == null || timeSlot.Status.Equals("Prenotato"))
                    return;

                SelectedTimeSlot = 1;
                var SportType = CurrentField switch
                {
                    1 => AppResources.PadelText,
                    2 => AppResources.TennisText,
                    3 => AppResources.SoccerText,
                    4 => AppResources.Cricket,
                    _ => ""
                };
                await Shell.Current.GoToAsync("BookingSummaryPage", true, new Dictionary<string, object>
                {
                    {"CurrentBooking",new Booking{ sportType = SportType, timeSlot = timeSlot.Time.ToString("HH:mm"), fieldId = SelectedTimeSlot,email = Utils.GetUser.email, date = SelectedDate } }
                });
            }
            catch { }
        }

        [RelayCommand]
        private async Task Padel2(TimeSlot timeSlot)
        {
            try
            {
                if (timeSlot == null || timeSlot.Status.Equals("Prenotato"))
                    return;

                SelectedTimeSlot = 2;
                var SportType = CurrentField switch
                {
                    1 => AppResources.PadelText,
                    2 => AppResources.TennisText,
                    3 => AppResources.SoccerText,
                    4 => AppResources.Cricket,
                    _ => ""
                };
                await Shell.Current.GoToAsync("BookingSummaryPage", true, new Dictionary<string, object>
                {
                    {"CurrentBooking",new Booking{ sportType = SportType, timeSlot = timeSlot.Time.ToString("HH:mm"), fieldId = SelectedTimeSlot,email = Utils.GetUser.email, date = SelectedDate } }
                });
            }
            catch { }
        }

        #endregion

        #region Constructor
        public HomeViewModel(IAuthServices authServices, IBookingService bookingService, ILocalizationResourceManager localizationResourceManager)
        {
            _authServices = authServices;
            _bookingService = bookingService;
            _localizationResourceManager = localizationResourceManager;
            CalendarItems = new ObservableCollection<CalendarItem>();
            GenerateCalendarForYears();
        }

        #endregion

        #region Methods
        public async void InitializeData()
        {
            try
            {
                if (!Utils.IsUserLogin)
                {
                    var data = new User { email = Preferences.Default.Get("username", string.Empty), password = Preferences.Default.Get("Password", string.Empty) };
                    var response = await _authServices.Login(data);
                    if (response != null && response.code != null && response.code.Equals("0000"))
                    {
                        Utils.GetUser = null;
                        Utils.GetUser = JsonSerializer.Deserialize<User>(response.data.ToString());
                        Utils.GetUser.password = UserData.password;
                        Preferences.Default.Set("Token", Utils.GetUser.token);
                    }
                    else
                    {
                        if (response != null && response.code != null)
                            await Shell.Current.DisplayAlert(AppResources.Error, response.message, AppResources.OK);
                        else
                            await Shell.Current.DisplayAlert(AppResources.Error, AppResources.SomethingWrong, AppResources.OK);

                        Preferences.Default.Set("username", string.Empty);
                        Preferences.Default.Set("Password", string.Empty);
                        Preferences.Default.Set("Token", string.Empty);
                        await Shell.Current.Navigation.PushAsync(new SelectionPage(_localizationResourceManager));
                    }
                }

                if (!Utils.IsHomeUpdated)
                    GenerateTimeSlots();
            }
            catch { }
        }
        private async Task GetAvailableSlots()
        {
            try
            {
                var response = await _bookingService.AvailableSlots(SelectedDate,CurrentField);
                if (response != null && response.code != null && response.code.Equals("0000"))
                    CurrentTimeList = JsonSerializer.Deserialize<List<string>>(response.data.ToString());
                else if (response != null && response.code != null)
                    await Shell.Current.DisplayAlert(AppResources.Error, response.message, AppResources.OK);
                else
                    await Shell.Current.DisplayAlert(AppResources.Error, AppResources.SomethingWrong, AppResources.OK);
            }
            catch { }
        }

        public async void GenerateTimeSlots()
        {
            IsBusy = true;
            var TempSLots = new ObservableCollection<TimeSlot>();
            DateTime startTime = new DateTime(1, 1, 1, 1, 0, 0);
            DateTime endTime = new DateTime(1, 1, 1, 23, 0, 0);

            await GetAvailableSlots();
            while (startTime <= endTime)
            {
                bool isBooked = CurrentTimeList?.Contains(startTime.ToString("HH:mm")) ?? false;
                TempSLots.Add(new TimeSlot
                {
                    Time = startTime,
                    Status = isBooked ? "Prenotato" : "",
                });
                startTime = startTime.AddMinutes(30);
            }

            TimeSlots2 = TimeSlots = new ObservableCollection<TimeSlot>();
            TimeSlots2 = TimeSlots = TempSLots;
            IsBusy = false;
        }
    
        private void GenerateCalendarForYears()
        {
            try
            {
                IsBusy = true;
                CalendarItems.Clear();
                var today = DateTime.Today;
                var startDate = DateTime.Today.AddYears(-1);
                var endDate = DateTime.Today.AddYears(3);

                for (var date = startDate; date <= endDate; date = date.AddDays(1))
                {
                    CalendarItems.Add(new CalendarItem
                    {
                        Date = date,
                        Day = date.ToString("ddd"),
                        MonthName = date.ToString("MMM")
                    });
                }
                SelectedCalender = CalendarItems.FirstOrDefault(c => c.Date == today) ?? CalendarItems.FirstOrDefault();
            }
            catch
            {
                IsBusy = false;
            }
        }

        #endregion
    }
}
