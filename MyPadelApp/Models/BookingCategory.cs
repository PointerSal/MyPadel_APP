using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPadelApp.Models
{
    public class BookingCategory
    {
        public List<Booking>? active { get; set; }
        public List<Booking>? archived { get; set; }
        public List<Booking>? canceled { get; set; }
    }
}
