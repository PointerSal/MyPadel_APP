using MyPadelApp.Models.Responses;

namespace MyPadelApp.Services.BookingServices
{
    public interface IBookingService
    {
        Task<GeneralResponse> AvailableSlots(DateTime dateTime, string SportName);
        Task<GeneralResponse> BookingHistory(string email);
        Task<GeneralResponse> CancelBooking(object booking);
        Task<GeneralResponse> ReserveBooking(object Booking);
    }
}