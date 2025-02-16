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
    public partial class PaymentScreenViewModel : BaseViewModel, IQueryAttributable
    {
        #region Services

        private readonly IBookingService _bookingService;

        #endregion

        #region Properties

        [ObservableProperty]
        public Booking _booking;

        #endregion

        #region Commands

        [RelayCommand]
        public async Task Pay()
        {
            try
            {
                IsBusy = true;
                var Data = new { Booking.sportType, Booking.date, Booking.duration, Booking.timeSlot, Booking.fieldId, Booking.paymentMethod, Booking.amount, Booking.email };
                var response = await _bookingService.ReserveBooking(Data);
                if (response != null && response.code != null && response.code.Equals("0000"))
                {
                    Booking = JsonSerializer.Deserialize<Booking>(response.data.ToString());
                    Utils.IsHomeUpdated = false;
                    Utils.IsBookingPageUpdated = false;
                    await Shell.Current.GoToAsync("../../PaymentBookingSummary", true, new Dictionary<string, object>
                    {
                        { "BookingData",Booking }
                    });
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
        public PaymentScreenViewModel(IBookingService bookingService)
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
                    Booking = (Booking)query["CurrentBooking"];
                }
            }
            catch { }
        }

        #endregion
    }
}