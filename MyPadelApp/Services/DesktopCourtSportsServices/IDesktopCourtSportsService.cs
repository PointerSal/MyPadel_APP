using MyPadelApp.Models.Responses;

namespace MyPadelApp.Services.DesktopCourtSportsServices
{
    public interface IDesktopCourtSportsService
    {
        Task<GeneralResponse> CourtSports();
        Task<GeneralResponse> RenewMembership(string email);
    }
}