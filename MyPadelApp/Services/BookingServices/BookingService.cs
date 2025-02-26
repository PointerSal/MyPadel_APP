using MyPadelApp.Models.Responses;
using MyPadelApp.Services.HttpClientServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPadelApp.Services.BookingServices
{
    public class BookingService(IHttpClientService httpClientService) : IBookingService
    {
        public async Task<GeneralResponse> AvailableSlots(DateTime dateTime,string SportName)
        {
            return await httpClientService.GetAsync("booking/available-slots?Date=" + dateTime.ToString("yyyy-MM-dd") + "&SportsName=" + SportName + "", true);
        }
        public async Task<GeneralResponse> BookingHistory(string email)
        {
            return await httpClientService.GetAsync("booking/history?Email=" + email + "", true);
        }
        public async Task<GeneralResponse> ReserveBooking(object Booking)
        {
            return await httpClientService.PostAsync("booking/reserve", Booking, true);
        }
        public async Task<GeneralResponse> CancelBooking(object booking)
        {
            return await httpClientService.DeleteAsync("booking/cancel", booking, true);
        }
    }
}
