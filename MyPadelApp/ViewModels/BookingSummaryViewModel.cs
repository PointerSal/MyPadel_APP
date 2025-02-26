using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyPadelApp.Helpers;
using MyPadelApp.Models;
using MyPadelApp.Resources.Languages;
using MyPadelApp.Services.BookingServices;
using MyPadelApp.Services.PriceServices;
using MyPadelApp.Services.StripeServices;
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
    public partial class BookingSummaryViewModel : BaseViewModel, IQueryAttributable
    {
        #region Services

        private readonly IStripeService _stripeService;
        private readonly IPriceService _priceService;

        #endregion

        #region Properties

        [ObservableProperty]
        public Booking _booking;

        [ObservableProperty]
        private int _amount = 0;

        [ObservableProperty]
        private int _duration = 60;

        [ObservableProperty]
        private int _selectedTime = 1;

        [ObservableProperty]
        private string _selectedPaymentMethod;

        private BookingTypes MySelectedBooking = new BookingTypes();
        public ObservableCollection<string> PaymentMethods { get; } = new()
        {
            //"PayPal",
            "CreditCard"
            //"Satispay"
        };

        [ObservableProperty]
        private string _paymentmethodError;

        [ObservableProperty]
        private string _courtName;

        [ObservableProperty]
        private bool _hasPaymentmethodError;
        public List<BookingPrices> bookingPricesList = null;

        #endregion

        #region Commands

        [RelayCommand]
        public async Task Pay()
        {
            try
            {
                if (!ValidateFields())
                    return;

                Booking.amount = Amount;
                Booking.duration = Duration.ToString();
                Booking.paymentMethod = SelectedPaymentMethod;

                var Data = new StripePayment { email = Utils.GetUser.email, amount = Amount };
                var response = await _stripeService.CheckoutSession(Data);
                if (response != null && response.code != null && response.code.Equals("0000"))
                {
                    var result = JsonSerializer.Deserialize<PaymentResponse>(response.data.ToString());
                    await Shell.Current.GoToAsync("FITPaymentPage", true, new Dictionary<string, object>
                    {
                        { "PaymentURL",result.sessionUrl },
                        { "CurrentBooking",Booking },
                        { "CourtName",CourtName }
                    });
                }
                else if (response != null && response.code != null)
                    await Shell.Current.DisplayAlert(AppResources.Error, response.message, AppResources.OK);
                else
                    await Shell.Current.DisplayAlert(AppResources.Error, AppResources.SomethingWrong, AppResources.OK);
            }
            catch { }
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
        public BookingSummaryViewModel(IStripeService stripeService, IPriceService priceService)
        {
            _stripeService = stripeService;
            _priceService = priceService;
            SelectedPaymentMethod = "CreditCard";
        }

        #endregion

        #region Methods
        public void GetPrices()
        {
            try
            {
                bookingPricesList = new List<BookingPrices>
                {
                    new BookingPrices { duration = MySelectedBooking.slot1Duration??0, price = MySelectedBooking.slot1Price??0},
                    new BookingPrices { duration = MySelectedBooking.slot2Duration??0, price = MySelectedBooking.slot2Price??0},
                    new BookingPrices { duration = MySelectedBooking.slot3Duration??0, price = MySelectedBooking.slot3Price??0}
                };
                CourtName = MySelectedBooking.fieldName;
                Duration = bookingPricesList.FirstOrDefault().duration;
                Amount = (int)(bookingPricesList.FirstOrDefault(e => e.duration == Duration).price);
            }
            catch (Exception ex) { }
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
                    MySelectedBooking = (BookingTypes)query["BookingType"];
                    GetPrices();
                    query.Clear();
                }
            }
            catch { }
        }

        #endregion

        #region Validation
        public bool ValidateFields()
        {
            bool isValid = true;

            if (string.IsNullOrWhiteSpace(SelectedPaymentMethod))
            {
                PaymentmethodError = AppResources.PaymentMethodSelected;
                HasPaymentmethodError = true;
                isValid = false;
            }
            else
            {
                PaymentmethodError = string.Empty;
                HasPaymentmethodError = false;
            }

            return isValid;
        }

        #endregion
    }
}