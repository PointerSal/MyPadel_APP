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
        private List<Datum> CurrentTimeList { get; set; } = new List<Datum>();
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
                SelectedDate = new DateTime(SelectedDate.Year,SelectedDate.Month,SelectedDate.Day,timeSlot.Time.Hour,timeSlot.Time.Minute,0);
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
                SelectedDate = new DateTime(SelectedDate.Year, SelectedDate.Month, SelectedDate.Day, timeSlot.Time.Hour, timeSlot.Time.Minute, timeSlot.Time.Second, 0);
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
                {
                    await GenerateTimeSlots(1);
                    await GenerateTimeSlots(2);
                    Utils.IsHomeUpdated = true;
                }
            }
            catch { }
        }
        private async Task GetAvailableSlots(int SelectedField)
        {
            try
            {
                var response = await _bookingService.AvailableSlots(SelectedDate, SelectedField);
                if (response != null && response.code != null && response.code.Equals("0000"))
                    CurrentTimeList = JsonSerializer.Deserialize<List<Datum>>(response.data.ToString());
                else if (response != null && response.code != null)
                    await Shell.Current.DisplayAlert(AppResources.Error, response.message, AppResources.OK);
                else
                    await Shell.Current.DisplayAlert(AppResources.Error, AppResources.SomethingWrong, AppResources.OK);
            }
            catch { }
        }

        public async Task GenerateTimeSlots(int SelectedField)
        {
            IsBusy = true;
            var tempSlots = new ObservableCollection<TimeSlot>();

            DateTime startTime = SelectedDate.Date.AddHours(1);
            DateTime endTime = SelectedDate.Date.AddHours(23);

            while (startTime <= endTime)
            {
                tempSlots.Add(new TimeSlot
                {
                    Time = startTime,
                    Status = ""
                });

                startTime = startTime.AddMinutes(30);
            }

            await GetAvailableSlots(SelectedField);
            if (CurrentTimeList == null || !CurrentTimeList.Any())
            {
                AssignSlots(SelectedField, tempSlots);
                return;
            }

            var selectedDay = CurrentTimeList.FirstOrDefault(d => d.date == SelectedDate.Date.ToString("yyyy-MM-dd"));

            if (selectedDay != null)
            {
                var bookedSlots = selectedDay.slots
                    .Select(s => new
                    {
                        Start = DateTime.Parse(s.startTime),
                        End = DateTime.Parse(s.endTime)
                    })
                    .ToList();

                var slotsToRemove = new List<TimeSlot>();

                foreach (var booking in bookedSlots)
                {
                    foreach (var slot in tempSlots)
                    {
                        if (slot.Time >= booking.Start && slot.Time < booking.End)
                        {
                            slot.Status = "Prenotato";
                            if (booking.End - booking.Start > TimeSpan.FromMinutes(30) && slot.Time > booking.Start)
                            {
                                slotsToRemove.Add(slot);
                            }
                        }
                    }
                }

                foreach (var slot in slotsToRemove)
                {
                    tempSlots.Remove(slot);
                }
            }

            AssignSlots(SelectedField, tempSlots);
        }

        private void AssignSlots(int SelectedField, ObservableCollection<TimeSlot> tempSlots)
        {
            if (SelectedField == 1)
            {
                TimeSlots = tempSlots;
            }
            else
            {
                TimeSlots2 = tempSlots;
            }

            IsBusy = false;
        }


        private async void GenerateCalendarForYears()
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

                await GenerateTimeSlots(1);
                await GenerateTimeSlots(2);
            }
            catch
            {
                IsBusy = false;
            }
        }

        #endregion
    }
}
