using MyPadelApp.Models.Responses;
using MyPadelApp.Services.HttpClientServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPadelApp.Services.PriceServices
{
    public class PriceService(IHttpClientService httpClientService) : IPriceService
    {
        public async Task<GeneralResponse> BookingPrices()
        {
            return await httpClientService.GetAsync("price", true);
        }
        public async Task<GeneralResponse> FITMemberShipPrices()
        {
            return await httpClientService.GetAsync("price/fitmembership", true);
        }
    }
}
