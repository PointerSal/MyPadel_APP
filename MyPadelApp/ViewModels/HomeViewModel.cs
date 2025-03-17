using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LocalizationResourceManager.Maui;
using MyPadelApp.Helpers;
using MyPadelApp.Models;
using MyPadelApp.Resources.Languages;
using MyPadelApp.Services.AuthServices;
using MyPadelApp.Services.BookingServices;
using MyPadelApp.Services.DesktopCourtSportsServices;
using MyPadelApp.ViewModels.ViewBaseModel;
using MyPadelApp.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
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
        private readonly IDesktopCourtSportsService _desktopCourtSportsService;
        private readonly IAuthServices _authServices;
        private readonly IBookingService _bookingService;

        #endregion

        #region Properties

        private bool IsClicked { get; set; } = false;
        private List<Datum> CurrentTimeList { get; set; } = new List<Datum>();
        public DateTime SelectedDate = DateTime.Now;
        //public int CurrentField = 1;
        //public int SelectedTimeSlot = 1;
        private List<BookingTypes> BookingTypesList { get; set; } = new List<BookingTypes>();

        [ObservableProperty]
        private ObservableCollection<Courts> _courtLists;

        [ObservableProperty]
        private Courts _selectedCourt;

        [ObservableProperty]
        public CalendarItem _selectedCalender;

        [ObservableProperty]
        public ObservableCollection<TimeSlot> _timeSlots;

        [ObservableProperty]
        public ObservableCollection<CalendarItem> _calendarItems;

        #endregion

        #region Commands

        [RelayCommand]
        private async Task Padel1(TimeSlotDetails timeSlot)
        {
            try
            {
                if (timeSlot == null || timeSlot.Status.Equals("Prenotato"))
                    return;

                if (Utils.GetUser.isFitVerified == false || Utils.GetUser.isFitVerified == null)
                {
                    await Shell.Current.DisplayAlert(AppResources.Error, AppResources.FITMemberShipNotUpdate, AppResources.OK);
                    return;
                }
                
                if ((DateTime.Now > timeSlot.Time))
                {
                    await Shell.Current.DisplayAlert(AppResources.Error, "Cannot booked slot", AppResources.OK);
                    return;
                }

                SelectedDate = new DateTime(SelectedDate.Year, SelectedDate.Month, SelectedDate.Day, timeSlot.Time.Hour, timeSlot.Time.Minute, 0);
                await Shell.Current.GoToAsync("BookingSummaryPage", true, new Dictionary<string, object>
                {
                    {"CurrentBooking",new Booking{ sportType = SelectedCourt.SportName, timeSlot = timeSlot.Time.ToString("HH:mm"), fieldId = int.Parse(timeSlot.FieldName),email = Utils.GetUser.email, date = SelectedDate }}, 
                    { "BookingType", BookingTypesList.FirstOrDefault(e=>e.fieldType.Equals(timeSlot.FieldName) && e.sportsName.Equals(SelectedCourt.SportName)) }
                });
            }
            catch { }
        }

        [RelayCommand]
        private async Task SelectedCategory(Courts courts)
        {
            try
            {
                IsBusy = true;
                SelectedCourt = courts;
                foreach (var item in CourtLists)
                {
                    item.IsSelected = item.SportName.Equals(SelectedCourt.SportName);
                }
                await GenerateTimeSlots(courts.SportName);
            }
            catch(Exception ex) { }
            IsBusy = false;
        }

        #endregion

        #region Constructor
        public HomeViewModel(IAuthServices authServices, IBookingService bookingService, ILocalizationResourceManager localizationResourceManager, IDesktopCourtSportsService desktopCourtSportsService)
        {
            _authServices = authServices;
            _bookingService = bookingService;
            _desktopCourtSportsService = desktopCourtSportsService;
            _localizationResourceManager = localizationResourceManager;
            CalendarItems = new ObservableCollection<CalendarItem>();
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
                    await GetAllCourts();
                    await GenerateTimeSlots(CourtLists.FirstOrDefault().SportName);
                    Utils.IsHomeUpdated = true;
                }
            }
            catch { }
        }
        private async Task GetAvailableSlots(string SelectedField)
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

        private async Task GetAllCourts()
        {
            try
            {
                var response = await _desktopCourtSportsService.CourtSports();
                if (response != null && response.code != null && response.code.Equals("0000"))
                {
                    var types = JsonSerializer.Deserialize<List<BookingTypes>>(response.data.ToString());
                    if (types != null && types.Count > 0)
                    {
                        BookingTypesList = types;
                        CourtLists = new ObservableCollection<Courts>(types.GroupBy(s => s.sportsName).Select(g => new Courts
                        {
                            SportName = g.Key,
                            Fields = g.Select(s => s.fieldName).ToList()
                        }).ToList());
                        SelectedCourt = CourtLists.FirstOrDefault();
                        CourtLists.FirstOrDefault().IsSelected = true;
                    }
                }
                else if (response != null && response.code != null)
                    await Shell.Current.DisplayAlert(AppResources.Error, response.message, AppResources.OK);
                else
                    await Shell.Current.DisplayAlert(AppResources.Error, AppResources.SomethingWrong, AppResources.OK);
            }
            catch { }
        }

        public async Task GenerateTimeSlots(string SelectedField)
        {
            IsBusy = true;
            var tempSlots = new TimeSlot();
            await GetAvailableSlots(SelectedField);
            TimeSlots = new ObservableCollection<TimeSlot>();

            foreach (var item in CourtLists.FirstOrDefault(e => e.SportName.Equals(SelectedField)).Fields)
            {
                var CurrentCourts = BookingTypesList.FirstOrDefault(e => e.fieldName.Equals(item));
                string timePart = CurrentCourts.openingHours.Split(' ')[1];
                string[] times = timePart.Split('-');

                DateTime startTime = SelectedDate.Date.AddHours(int.Parse(times[0].Split(':')[0]));
                DateTime endTime = SelectedDate.Date.AddHours(int.Parse(times[1].Split(':')[0]));

                var parts = CurrentCourts.openingHours.Split(' ')[0];
                string today = SelectedDate.ToString("dddd", CultureInfo.InvariantCulture);
                bool isOpenToday = parts.Split('-').Contains(today, StringComparer.OrdinalIgnoreCase);
                if (!isOpenToday)
                    continue;

                tempSlots = new TimeSlot
                {
                    FieldName = item,
                    TimeSlots = new ObservableCollection<TimeSlotDetails>()
                };
                while (startTime <= endTime)
                {
                    tempSlots.TimeSlots.Add(new TimeSlotDetails
                    {
                        FieldName = CurrentCourts.fieldType,
                        Time = startTime,
                        Status = ""
                    });
                    startTime = startTime.AddMinutes(30);
                }

                if (CurrentTimeList == null || !CurrentTimeList.Any())
                {
                    TimeSlots.Add(tempSlots);
                    continue;
                }

                var selectedDay = CurrentTimeList.FirstOrDefault(d => d.fieldName.Equals(item));
                if (selectedDay != null)
                {
                    var bookedSlots = selectedDay.slots
                        .Select(s => new
                        {
                            Start = DateTime.Parse(s.startTime),
                            End = DateTime.Parse(s.endTime)
                        })
                        .ToList();

                    var slotsToRemove = new List<TimeSlotDetails>();
                    foreach (var booking in bookedSlots)
                    {
                        foreach (var slot in tempSlots.TimeSlots)
                        {
                            if (slot.Time >= booking.Start && slot.Time < booking.End)
                            {
                                slot.Status = "Prenotato";
                                if (booking.End - booking.Start > TimeSpan.FromMinutes(30) && slot.Time > booking.Start)
                                    slotsToRemove.Add(slot);
                            }
                        }
                    }

                    foreach (var slot in slotsToRemove)
                    {
                        tempSlots.TimeSlots.Remove(slot);
                    }
                }

                TimeSlots.Add(tempSlots);
            }

            IsBusy = false;
        }

        public async void GenerateCalendarForYears()
        {
            try
            {
                IsBusy = true;
                CalendarItems.Clear();
                var startDate = DateTime.Today;
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
                SelectedCalender = CalendarItems.FirstOrDefault(c => c.Date == startDate) ?? CalendarItems.FirstOrDefault();

                await GetAllCourts();
                await GenerateTimeSlots(CourtLists.FirstOrDefault().SportName);
            }
            catch
            {
                IsBusy = false;
            }
        }

        #endregion
    }
}
