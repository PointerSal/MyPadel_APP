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
using System.Threading.Tasks;

namespace MyPadelApp.ViewModels
{
    public partial class PaymentBookingSummaryViewModel : BaseViewModel, IQueryAttributable
    {
        #region Services

        private readonly IBookingService _bookingService;

        #endregion

        #region Properties

        [ObservableProperty]
        public Booking _booking;

        [ObservableProperty]
        private string _courtName;

        #endregion

        #region Commands

        [RelayCommand]
        public async Task CancelBooking()
        {
            try
            {
                IsBusy = true;
                if (Booking.endTime.HasValue && (Booking.endTime.Value - DateTime.UtcNow).TotalHours < 24)
                {
                    bool result = await Shell.Current.DisplayAlert(
                                 AppResources.Notice,
                                 AppResources.CancelledPolicy,
                                 AppResources.Yes,
                                 AppResources.No
                             );
                    if (!result)
                    {
                        IsBusy = false;
                        return;
                    }
                }
                var response = await _bookingService.CancelBooking(new { bookingId = Booking.id, email = Utils.GetUser.email });
                if (response != null && response.code != null && response.code.Equals("0000"))
                {
                    await Shell.Current.DisplayAlert(AppResources.Success, AppResources.BookingCanceled, AppResources.OK);
                    await Shell.Current.Navigation.PopToRootAsync(true);
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
        public PaymentBookingSummaryViewModel(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        #endregion

        #region InterfaceImplementation
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            try
            {
                if (query != null)
                {
                    Booking = (Booking)query["BookingData"];
                    CourtName = (string)query["CourtName"];
                }
            }
            catch { }
        }

        #endregion
    }
}