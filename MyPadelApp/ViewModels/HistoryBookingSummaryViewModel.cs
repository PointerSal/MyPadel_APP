using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyPadelApp.Models;
using MyPadelApp.Resources.Languages;
using MyPadelApp.ViewModels.ViewBaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MyPadelApp.ViewModels
{
    public partial class HistoryBookingSummaryViewModel : BaseViewModel, IQueryAttributable
    {
        #region Properties

        [ObservableProperty]
        public Booking _booking;
        
        [ObservableProperty]
        public bool _canCancelBooking;

        #endregion

        #region Commands

        [RelayCommand]
        public async Task CancelBooking()
        {
            try
            {
                await Shell.Current.GoToAsync("CancelBookingPage", true, new Dictionary<string, object>
                {
                    { "BookingData",Booking }
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
                    Booking = (Booking)query["BookingData"];
                    CanCancelBooking = (bool)query["CanCancelled"];
                }
            }
            catch { }
        }

        #endregion
    }
}