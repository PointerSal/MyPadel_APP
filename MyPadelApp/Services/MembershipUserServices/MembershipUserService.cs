using MyPadelApp.Helpers;
using MyPadelApp.Models;
using MyPadelApp.Models.Responses;
using MyPadelApp.Services.HttpClientServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MyPadelApp.Services.MembershipUserServices
{
    public class MembershipUserService(IHttpClientService httpClientService) : IMembershipUserService
    {
        public async Task<GeneralResponse> RegisterMemberShipUser(MembershipRequestModel membershipRequestModel, string filePath)
        {
            Dictionary<string, string> formData = new Dictionary<string, string>
            {
                { "CardNumber", membershipRequestModel.CardNumber },
                { "ExpiryDate", membershipRequestModel.ExpiryDate.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.fffZ") },
                { "MedicalCertificateDate", membershipRequestModel.MedicalCertificateDate.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.fffZ") },
                { "FirstName", membershipRequestModel.FirstName },
                { "LastName", membershipRequestModel.LastName },
                { "Gender", membershipRequestModel.Gender },
                { "BirthDate", membershipRequestModel.BirthDate.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.fffZ") },
                { "Address", membershipRequestModel.Address },
                { "PostalCode", membershipRequestModel.PostalCode },
                { "Municipality", membershipRequestModel.Municipality },
                { "PaymentMethod", membershipRequestModel.PaymentMethod },
                { "Email",Utils.GetUser.email }
            };

            string fileFieldName = "MedicalCertificate";
            return await httpClientService.PostMultipartAsync("membershipuser/register", formData, filePath, fileFieldName);
        }

        public async Task<GeneralResponse> RegisterFitMemberShipUser(MembershipRequestModel membershipRequestModel, string filePath)
        {
            Dictionary<string, string> formData = new Dictionary<string, string>
            {
                { "MembershipNumber", membershipRequestModel.CardNumber },
                { "ExpiryDate", membershipRequestModel.ExpiryDate.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.fffZ") },
                { "MedicalCertificateDate", membershipRequestModel.MedicalCertificateDate.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.fffZ") },
                { "Email",Utils.GetUser.email }
            };

            string fileFieldName = "MedicalCertificate";
            return await httpClientService.PostMultipartAsync("membershipuser/register/fit", formData, filePath, fileFieldName, false);
        }
        public async Task<GeneralResponse> CardDetails(string email)
        {
            return await httpClientService.GetAsync("membershipuser/carddetails/" + email + "", true);
        }
        public async Task<GeneralResponse> ExpiryDate(string email)
        {
            return await httpClientService.GetAsync("membershipuser/expirydate/" + email + "", true);
        }
    }
}
