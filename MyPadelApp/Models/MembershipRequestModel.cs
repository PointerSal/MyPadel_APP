using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MyPadelApp.Models
{
    public class MembershipRequestModel
    {
        public string? CardNumber { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public DateTime? MedicalCertificateDate { get; set; }
        public byte[]? MedicalCertificate { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Gender { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? Address { get; set; }
        public string? TaxCode { get; set; }
        public string? PostalCode { get; set; }
        public string? Municipality { get; set; }
        public string? PaymentMethod { get; set; }
        public string? BrithMunicipality { get; set; }
        public string? CitizenShips { get; set; }
        public string? ResidenceMunicipality { get; set; }
        public string? Cell { get; set; }
        public string provinceOfBirth { get; set; }
        public string municipalityOfBirth { get; set; }
        public string provinceOfResidence { get; set; }
        public string municipalityOfResidence { get; set; }
        public string residentialAddress { get; set; }
    }
}
