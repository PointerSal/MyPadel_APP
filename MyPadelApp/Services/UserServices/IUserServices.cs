using MyPadelApp.Models.Responses;

namespace MyPadelApp.Services.UserServices
{
    public interface IUserServices
    {
        Task<GeneralResponse> GetProfile(string email);
        Task<GeneralResponse> UpdateProfile(object UpdatedProfile);
    }
}