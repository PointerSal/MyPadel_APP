using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyPadelApp.Helpers;
using MyPadelApp.Models;
using MyPadelApp.Resources.Languages;
using MyPadelApp.Services.BookingServices;
using MyPadelApp.Services.DesktopCourtSportsServices;
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
        private readonly IDesktopCourtSportsService _desktopCourtSportsService;

        #endregion

        #region Properties

        [ObservableProperty]
        private MembershipRequestModel _cardModel = null;

        [ObservableProperty]
        private string _paymentUrl = null;

        [ObservableProperty]
        public Booking _booking = null;

        private bool IsRenewFIT { get; set; } = false;

        private string CourtName = "";

        #endregion

        #region Commands

        [RelayCommand]
        public async Task AddCard()
        {
            try
            {
                IsBusy = true;
                if (!IsRenewFIT)
                {
                    if (Booking == null)
                    {
                        var response = await _membershipUserService.RegisterMemberShipUser(CardModel, ImageToBase64.BytesToBase64(CardModel.MedicalCertificate));
                        if (response != null && response.code != null && response.code.Equals("0000"))
                        {
                            Utils.GetUser.isFitVerified = true;
                            Preferences.Default.Set("username", Utils.GetUser.email);
                            Preferences.Default.Set("Password", Utils.GetUser.password);
                            await Shell.Current.GoToAsync("../../../BookedFieldPage");
                        }
                        else if (response != null && response.code != null)
                        {
                            await Shell.Current.DisplayAlert(AppResources.Error, response.message, AppResources.OK);
                            await Shell.Current.GoToAsync("..");
                        }
                        else
                        {
                            await Shell.Current.DisplayAlert(AppResources.Error, AppResources.SomethingWrong, AppResources.OK);
                            await Shell.Current.GoToAsync("..");
                        }
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
                            { "BookingData",Booking },
                            { "CourtName",CourtName }
                        });
                        }
                        else if (response != null && response.code != null)
                        {
                            await Shell.Current.DisplayAlert(AppResources.Error, response.message, AppResources.OK);
                            await Shell.Current.GoToAsync("..");
                        }
                        else
                        {
                            await Shell.Current.DisplayAlert(AppResources.Error, AppResources.SomethingWrong, AppResources.OK);
                            await Shell.Current.GoToAsync("..");
                        }
                    }
                }
                else
                {
                    var response = await _desktopCourtSportsService.RenewMembership(Utils.GetUser.email);
                    if (response != null && response.code != null && response.code.Equals("0000"))
                    {
                        Utils.GetUser.isFitVerified = false;
                        await Shell.Current.DisplayAlert(AppResources.Success, AppResources.CardRenew, AppResources.OK);
                        await Shell.Current.GoToAsync("..");
                    }
                    else if (response != null && response.code != null)
                    {
                        await Shell.Current.DisplayAlert(AppResources.Error, response.message, AppResources.OK);
                        await Shell.Current.GoToAsync("..");
                    }
                    else
                    {
                        await Shell.Current.DisplayAlert(AppResources.Error, AppResources.SomethingWrong, AppResources.OK);
                        await Shell.Current.GoToAsync("..");
                    }
                }
            }
            catch (Exception ex) { }
            IsBusy = false;
        }
  
        #endregion

        #region Constructor
        public FITPaymentViewModel(IMembershipUserService membershipUserService, IBookingService bookingService, IDesktopCourtSportsService desktopCourtSportsService)
        {
            _bookingService = bookingService;
            _membershipUserService = membershipUserService;
            _desktopCourtSportsService = desktopCourtSportsService;
        }

        #endregion

        #region InterfaceImplementation
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            try
            {
                if (query != null)
                {
                    if (query.ContainsKey("CurrentBooking"))
                    {
                        Booking = (Booking)query["CurrentBooking"];
                        CourtName = (string)query["CourtName"];
                    }
                    else if (query.ContainsKey("CurrentFIT"))
                        CardModel = (MembershipRequestModel)query["CurrentFIT"];
                    else if (query.ContainsKey("Renew"))
                        IsRenewFIT = true;

                    PaymentUrl = (string)query["PaymentURL"];
                    query.Clear();
                }
            }
            catch { }
        }

        #endregion
    }
}
