using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPadelApp.Models
{
    public class Certificate
    {
        public string cardNumber { get; set; } = "";
        public DateTime? expiryDate { get; set; }
        public DateTime? medicalCertificateDate { get; set; }
        public string medicalCertificate { get; set; } = "";
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string gender { get; set; }
        public string birthDate { get; set; }
        public string address { get; set; }
        public string postalCode { get; set; }
        public string municipality { get; set; }
        public string paymentMethod { get; set; }
        public string phoneNumber { get; set; }
        public string provinceOfBirth { get; set; }
        public string municipalityOfBirth { get; set; }
        public string citizenship { get; set; }
        public string taxCode { get; set; }
        public string provinceOfResidence { get; set; }
        public string municipalityOfResidence { get; set; }
        public string residentialAddress { get; set; }
        public string email { get; set; }
    }
}
