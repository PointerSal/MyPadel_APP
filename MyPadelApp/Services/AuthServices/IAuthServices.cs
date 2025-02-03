using MyPadelApp.Models;
using MyPadelApp.Models.Responses;

namespace MyPadelApp.Services.AuthServices
{
    public interface IAuthServices
    {
        Task<GeneralResponse> Login(User user);
        Task<GeneralResponse> RegisterUser(User user);
        Task<GeneralResponse> ResetPassword(User user);
        Task<GeneralResponse> VerifyEmail(User user);
    }
}