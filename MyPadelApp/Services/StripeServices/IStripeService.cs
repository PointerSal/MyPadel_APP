
using MyPadelApp.Models.Responses;

namespace MyPadelApp.Services.StripeServices
{
    public interface IStripeService
    {
        Task<GeneralResponse> CheckoutSession(object data);
    }
}