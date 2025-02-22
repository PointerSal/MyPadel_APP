using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPadelApp.Models
{
    public class StripePayment
    {
        public string email { get; set; } = string.Empty;
        public double amount { get; set; } = 0;
        public string paymentMethod { get; set; } = "card";
        public string currency { get; set; } = "EUR";
    }

    public class PaymentResponse
    {
        public string? sessionUrl { get; set; }
    }
}
