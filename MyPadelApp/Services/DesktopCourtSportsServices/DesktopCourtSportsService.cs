using MyPadelApp.Models.Responses;
using MyPadelApp.Services.HttpClientServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPadelApp.Services.DesktopCourtSportsServices
{
    public class DesktopCourtSportsService(IHttpClientService httpClientService) : IDesktopCourtSportsService
    {
        public async Task<GeneralResponse> CourtSports()
        {
            return await httpClientService.GetAsync("courtsports/all", true);
        }
    }
}
