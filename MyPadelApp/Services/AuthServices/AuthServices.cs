using MyPadelApp.Models;
using MyPadelApp.Models.Responses;
using MyPadelApp.Services.HttpClientServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPadelApp.Services.AuthServices
{
    public class AuthServices(IHttpClientService httpClientService) : IAuthServices
    {
        public async Task<GeneralResponse> RegisterUser(User user)
        {
            return await httpClientService.PostAsync("auth/register", user, false);
        }
        public async Task<GeneralResponse> Login(User user)
        {
            return await httpClientService.PostAsync("auth/login", user, false);
        }
        public async Task<GeneralResponse> VerifyEmail(User user)
        {
            return await httpClientService.PostAsync("auth/verify-email", user, false);
        }
        public async Task<GeneralResponse> ResetPassword(User user)
        {
            return await httpClientService.PostAsync("auth/reset-password", user, false);
        }
    }
}
