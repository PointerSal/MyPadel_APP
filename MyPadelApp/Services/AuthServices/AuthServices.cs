﻿using MyPadelApp.Models;
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
            return await httpClientService.PostAsync("auth/verify-email", user, true);
        }
        public async Task<GeneralResponse> VerifyPhone(User user)
        {
            return await httpClientService.PostAsync("auth/verify-phone", user, true);
        }
        public async Task<GeneralResponse> ResetPassword(object user)
        {
            return await httpClientService.PostAsync("auth/reset-password", user, false);
        }
        public async Task<GeneralResponse> UpdatePassword(object user)
        {
            return await httpClientService.PutAsync("auth/update-password", user, true);
        }
        public async Task<GeneralResponse> ResendOTP(User user)
        {
            return await httpClientService.PostAsync("auth/resendEmail-otp", user, true);
        }
        public async Task<GeneralResponse> AddPhoneNumber(User user)
        {
            return await httpClientService.PostAsync("auth/add-phone-number", user, true);
        }
        public async Task<GeneralResponse> DeleteAccount(User user)
        {
            return await httpClientService.DeleteAsync("auth/delete-account", user, true);
        }
    }
}
