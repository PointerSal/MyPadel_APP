using Android.Accounts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyPadelApp.Models;
using MyPadelApp.Resources.Languages;
using MyPadelApp.ViewModels.ViewBaseModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPadelApp.ViewModels
{
    public partial class BookingSummaryViewModel : BaseViewModel, IQueryAttributable
    {
        #region Properties

        [ObservableProperty]
        public Booking _booking;

        [ObservableProperty]
        private int _amount = 44;

        [ObservableProperty]
        private int _duration = 30;

        [ObservableProperty]
        private int _selectedTime = 1;

        [ObservableProperty]
        private string _selectedPaymentMethod;
        public ObservableCollection<string> PaymentMethods { get; } = new()
        {
            "PayPal",
            "CreditCard",
            "Satispay"
        };

        [ObservableProperty]
        private string _paymentmethodError;

        [ObservableProperty]
        private bool _hasPaymentmethodError;

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
                await Shell.Current.GoToAsync("PaymentScreenPage", true, new Dictionary<string, object>
                {
                    { "CurrentBooking",Booking }
                });
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

        #region InterfaceImplementation
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            try
            {
                if (query != null)
                {
                    Booking = (Booking)query["CurrentBooking"];
                    Amount = 44;
                    Duration = 30;
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