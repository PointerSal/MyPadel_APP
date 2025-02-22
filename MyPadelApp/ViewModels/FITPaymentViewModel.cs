using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyPadelApp.Helpers;
using MyPadelApp.Models;
using MyPadelApp.Resources.Languages;
using MyPadelApp.Services.BookingServices;
using MyPadelApp.Services.MembershipUserServices;
using MyPadelApp.Services.StripeServices;
using MyPadelApp.ViewModels.ViewBaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MyPadelApp.ViewModels
{
    public partial class FITPaymentViewModel : BaseViewModel, IQueryAttributable
    {
        #region Services

        private readonly IMembershipUserService _membershipUserService;
        private readonly IBookingService _bookingService;

        #endregion

        #region Properties

        [ObservableProperty]
        private MembershipRequestModel _cardModel = null;

        [ObservableProperty]
        private string _paymentUrl = null;

        [ObservableProperty]
        public Booking _booking = null;

        #endregion

        #region Commands

        [RelayCommand]
        public async Task AddCard()
        {
            try
            {
                IsBusy = true;
                if (Booking == null)
                {
                    var response = await _membershipUserService.RegisterMemberShipUser(CardModel, ImageToBase64.BytesToBase64(CardModel.MedicalCertificate));
                    if (response != null && response.code != null && response.code.Equals("0000"))
                    {
                        Preferences.Default.Set("username", Utils.GetUser.email);
                        Preferences.Default.Set("Password", Utils.GetUser.password);
                        await Shell.Current.GoToAsync("../../../BookedFieldPage");
                    }
                    else if (response != null && response.code != null)
                        await Shell.Current.DisplayAlert(AppResources.Error, response.message, AppResources.OK);
                    else
                        await Shell.Current.DisplayAlert(AppResources.Error, AppResources.SomethingWrong, AppResources.OK);
                }
                else
                {
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
            }
            catch (Exception ex) { }
            IsBusy = false;
        }
  
        #endregion

        #region Constructor
        public FITPaymentViewModel(IMembershipUserService membershipUserService, IBookingService bookingService)
        {
            _bookingService = bookingService;
            _membershipUserService = membershipUserService;
        }

        #endregion

        #region InterfaceImplementation
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            try
            {
                if (query != null)
                {
                    if(query.ContainsKey("CurrentBooking"))
                        Booking = (Booking)query["CurrentBooking"];
                    else if(query.ContainsKey("CurrentFIT"))
                        CardModel = (MembershipRequestModel)query["CurrentFIT"];

                    PaymentUrl = (string)query["PaymentURL"];
                    query.Clear();
                }
            }
            catch { }
        }

        #endregion
    }
}
