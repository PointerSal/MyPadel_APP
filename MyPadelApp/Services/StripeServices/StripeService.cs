using MyPadelApp.Models.Responses;
using MyPadelApp.Services.HttpClientServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPadelApp.Services.StripeServices
{
    public class StripeService(IHttpClientService httpClientService) : IStripeService
    {
        public async Task<GeneralResponse> CheckoutSession(object data)
        {
            return await httpClientService.PostAsync("stripe/checkout-session", data, true);
        }
    }
}
