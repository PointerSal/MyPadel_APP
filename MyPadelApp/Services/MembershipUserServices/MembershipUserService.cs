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
        public async Task<GeneralResponse> RegisterMemberShipUser(MembershipRequestModel membershipRequestModel, string MedicalCertificate)
        {
            var data = new Certificate
            {
                cardNumber = "",
                expiryDate = null,
                medicalCertificateDate = null,
                medicalCertificate = "",
                firstName = membershipRequestModel.FirstName,
                lastName = membershipRequestModel.LastName,
                gender = membershipRequestModel.Gender,
                birthDate = membershipRequestModel.BirthDate.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                address = membershipRequestModel.Address,
                postalCode = membershipRequestModel.PostalCode,
                municipality = membershipRequestModel.Municipality,
                paymentMethod = membershipRequestModel.PaymentMethod,
                phoneNumber = membershipRequestModel.Cell,
                provinceOfBirth = membershipRequestModel.provinceOfBirth,
                municipalityOfBirth = membershipRequestModel.municipalityOfBirth,
                citizenship = membershipRequestModel.CitizenShips,
                taxCode = membershipRequestModel.TaxCode,
                provinceOfResidence = membershipRequestModel.provinceOfResidence,
                municipalityOfResidence = membershipRequestModel.municipalityOfResidence,
                residentialAddress = membershipRequestModel.Cell,
                email = Utils.GetUser.email
            };

            return await httpClientService.PostAsync("membershipuser/register", data, true);
        }

        public async Task<GeneralResponse> RegisterFitMemberShipUser(MembershipRequestModel membershipRequestModel, string MedicalCertificate)
        {
            var data = new
            {
                membershipNumber = membershipRequestModel.CardNumber,
                expiryDate = membershipRequestModel.ExpiryDate.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                medicalCertificateDate = membershipRequestModel.MedicalCertificateDate.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                medicalCertificate = MedicalCertificate,
                email = Utils.GetUser.email
            };

            return await httpClientService.PostAsync("membershipuser/fit", data, true, false);
        }
        public async Task<GeneralResponse> UpdateFitMemberShipUser(string MedicalCertificate)
        {
            var data = new
            {
                medicalCertificate = MedicalCertificate,
                email = Utils.GetUser.email
            };

            return await httpClientService.PostAsync("membershipuser/fit", data, true, false);
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
