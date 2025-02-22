using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPadelApp.Models
{
    public class BookingPrices
    {
        public int id { get; set; } = 0;
        public int duration { get; set; } = 0;
        public double price { get; set; } = 0;
        public double fitMembershipFee { get; set; } = 0;
    }
}
