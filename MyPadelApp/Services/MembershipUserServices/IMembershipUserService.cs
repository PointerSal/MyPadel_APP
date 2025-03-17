using MyPadelApp.Models;
using MyPadelApp.Models.Responses;

namespace MyPadelApp.Services.MembershipUserServices
{
    public interface IMembershipUserService
    {
        Task<GeneralResponse> CardDetails(string email);
        Task<GeneralResponse> ExpiryDate(string email);
        Task<GeneralResponse> RegisterFitMemberShipUser(MembershipRequestModel membershipRequestModel, string MedicalCertificate);
        Task<GeneralResponse> RegisterMemberShipUser(MembershipRequestModel membershipRequestModel, string MedicalCertificate);
        Task<GeneralResponse> UpdateFitMemberShipUser(string MedicalCertificate);
    }
}