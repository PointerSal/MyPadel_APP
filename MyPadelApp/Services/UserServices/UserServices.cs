using MyPadelApp.Models.Responses;
using MyPadelApp.Services.HttpClientServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPadelApp.Services.UserServices
{
    public class UserServices(IHttpClientService httpClientService) : IUserServices
    {
        public async Task<GeneralResponse> GetProfile(string email)
        {
            return await httpClientService.GetAsync("user/profile?email=" + email + "", true);
        }
        public async Task<GeneralResponse> UpdateProfile(object UpdatedProfile)
        {
            return await httpClientService.PostAsync("user/update-profile", UpdatedProfile, true, false);
        }
    }
}
