using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyPadelApp.Helpers;
using MyPadelApp.Models;
using MyPadelApp.Resources.Languages;
using MyPadelApp.Services.MembershipUserServices;
using MyPadelApp.Services.PriceServices;
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
    public partial class FITCardViewModel : BaseViewModel
    {
        #region Services

        private readonly IMembershipUserService _membershipUserService;
        private readonly IStripeService _stripeService;
        private readonly IPriceService _priceService;

        #endregion

        #region Properties

        [ObservableProperty]
        public Card _card;

        [ObservableProperty]
        public bool _isCardRenew = false;

        [ObservableProperty]
        public string _expiryMessage = "";

        #endregion

        #region Commands

        public async void OnPageAppearing()
        {
            try
            {
                IsBusy = true;
                var response = await _membershipUserService.CardDetails(Preferences.Default.Get("username",string.Empty));
                if (response != null && response.code != null && response.code.Equals("0000"))
                {
                    Card = JsonSerializer.Deserialize<Card>(response.data.ToString());
                    DateTime today = DateTime.Today;

                    if(Card == null || Card.expiryDate == null)
                        IsCardRenew = false;
                    if (Card.expiryDate >= today && Card.expiryDate < today.AddDays(30))
                    {
                        IsCardRenew = true;
                        ExpiryMessage = AppResources.ThirtyDaysExpiredError;
                    }
                    else if (Card.expiryDate <= today)
                    {
                        IsCardRenew = true;
                        ExpiryMessage = AppResources.CardExpiredError;
                    }
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
        public async Task Renew()
        {
            try
            {
                var Prices = await GetPrices();
                if (!Prices.Item1)
                    return;

                var Data = new StripePayment { email = Utils.GetUser.email, amount = Prices.Item2 };
                var response = await _stripeService.CheckoutSession(Data);
                if (response != null && response.code != null && response.code.Equals("0000"))
                {
                    var result = JsonSerializer.Deserialize<PaymentResponse>(response.data.ToString());
                    await Shell.Current.GoToAsync("FITPaymentPage", true, new Dictionary<string, object>
                    {
                        { "PaymentURL",result.sessionUrl },
                        { "Renew",true },
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
        public FITCardViewModel(IMembershipUserService membershipUserService, IStripeService stripeService, IPriceService priceService)
        {
            _membershipUserService = membershipUserService;
            _stripeService = stripeService;
            _priceService = priceService;
        }

        #endregion

        #region Methods
        public async Task<(bool,double)> GetPrices()
        {
            try
            {
                var response = await _priceService.FITMemberShipPrices();
                if (response != null && response.code != null && response.code.Equals("0000"))
                    return (true, JsonSerializer.Deserialize<BookingPrices>(response.data.ToString()).fitMembershipFee);
                else if (response != null && response.code != null)
                    await Shell.Current.DisplayAlert(AppResources.Error, response.message, AppResources.OK);
                else
                    await Shell.Current.DisplayAlert(AppResources.Error, AppResources.SomethingWrong, AppResources.OK);
            }
            catch (Exception ex) { }
            return (false,0);
        }

        #endregion
    }
}