using MyPadelApp.Models.Responses;

namespace MyPadelApp.Services.PriceServices
{
    public interface IPriceService
    {
        Task<GeneralResponse> BookingPrices();
        Task<GeneralResponse> FITMemberShipPrices();
    }
}