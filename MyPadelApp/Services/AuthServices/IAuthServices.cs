using MyPadelApp.Models;
using MyPadelApp.Models.Responses;

namespace MyPadelApp.Services.AuthServices
{
    public interface IAuthServices
    {
        Task<GeneralResponse> AddPhoneNumber(User user);
        Task<GeneralResponse> Login(User user);
        Task<GeneralResponse> RegisterUser(User user);
        Task<GeneralResponse> ResendOTP(User user);
        Task<GeneralResponse> ResendPhoneOTP(User user);
        Task<GeneralResponse> ResetPassword(User user);
        Task<GeneralResponse> VerifyEmail(User user);
        Task<GeneralResponse> VerifyPhone(User user);
    }
}