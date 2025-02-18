using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyPadelApp.Helpers;
using MyPadelApp.Models;
using MyPadelApp.Resources.Languages;
using MyPadelApp.Services.BookingServices;
using MyPadelApp.ViewModels.ViewBaseModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MyPadelApp.ViewModels
{
    public partial class MatchHistoryViewModel : BaseViewModel
    {
        #region Services

        private readonly IBookingService _bookingService;

        #endregion

        #region Properties

        [ObservableProperty]
        public ObservableCollection<Booking> _bookingList;

        [ObservableProperty]
        public bool _isEmpty;

        public BookingCategory AllBookings = new BookingCategory();

        private int ClickedTab = 1;

        #endregion

        #region Commands

        public async void OnPageAppearing()
        {
            try
            {
                if (!Utils.IsBookingPageUpdated)
                {
                    IsBusy = true;
                    var response = await _bookingService.BookingHistory(Utils.GetUser.email);
                    if (response != null && response.code != null && response.code.Equals("0000"))
                    {
                        AllBookings = JsonSerializer.Deserialize<BookingCategory>(response.data.ToString());
                        if(ClickedTab == 1)
                            BookingList = new ObservableCollection<Booking>(AllBookings.active);
                        else if (ClickedTab == 2)
                            BookingList = new ObservableCollection<Booking>(AllBookings.archived);
                        else if (ClickedTab == 3)
                            BookingList = new ObservableCollection<Booking>(AllBookings.canceled);

                        Utils.IsBookingPageUpdated = true;
                    }
                    else if (response != null && response.code != null)
                        await Shell.Current.DisplayAlert(AppResources.Error, response.message, AppResources.OK);
                    else
                        await Shell.Current.DisplayAlert(AppResources.Error, AppResources.SomethingWrong, AppResources.OK);
                }
            }
            catch(Exception ex) { }
            IsEmpty = BookingList == null || BookingList.Count == 0;
            IsBusy = false;
        }
        public void TabChanged(int TabNumber)
        {
            try
            {
                ClickedTab = TabNumber;
                if (ClickedTab == 1)
                    BookingList = new ObservableCollection<Booking>(AllBookings.active);
                else if (ClickedTab == 2)
                    BookingList = new ObservableCollection<Booking>(AllBookings.archived);
                else if (ClickedTab == 3)
                    BookingList = new ObservableCollection<Booking>(AllBookings.canceled);
            }
            catch { }
            IsEmpty = BookingList == null || BookingList.Count == 0;
            IsBusy = false;
        }

        [RelayCommand]
        public async Task HistoryBookingDetail(Booking booking)
        {
            try
            {
                await Shell.Current.GoToAsync("HistoryBookingSummaryPage", true, new Dictionary<string, object>
                {
                    { "BookingData",booking }
                });
            }
            catch(Exception ex) { }
        }

        #endregion

        #region Constructor
        public MatchHistoryViewModel(IBookingService bookingService)
        {
            _bookingService = bookingService ;
            OnPageAppearing();
        }

        #endregion
    }
}