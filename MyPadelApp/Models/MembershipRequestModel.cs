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
        public DateTime ExpiryDate { get; set; } = DateTime.Now;
        public DateTime MedicalCertificateDate { get; set; } = DateTime.Now;
        public byte[]? MedicalCertificate { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Gender { get; set; }
        public DateTime BirthDate { get; set; } = DateTime.Now;
        public string? Address { get; set; }
        public string? PostalCode { get; set; }
        public string? Municipality { get; set; }
        public string? PaymentMethod { get; set; }
    }
}
