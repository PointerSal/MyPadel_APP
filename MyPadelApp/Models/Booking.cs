using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPadelApp.Models
{
    public class Booking
    {
        public int? id { get; set; }
        public string? sportType { get; set; }
        public DateTime? date { get; set; }
        public DateTime? endTime { get; set; }
        public string? timeSlot { get; set; }
        public string? duration { get; set; }
        public int? fieldId { get; set; }
        public string? paymentMethod { get; set; }
        public double? amount { get; set; }
        public bool? flagBooked { get; set; }
        public bool? flagCanceled { get; set; }
        public string? email { get; set; }
        public int? DurationInMinutes
        {
            get
            {
                try
                {
                    if (date.HasValue && endTime.HasValue)
                    {
                        return (int)(endTime.Value - date.Value).TotalMinutes;
                    }
                }
                catch { }   
                return null;
            }
        }
        public string? BookingStatus
        {
            get
            {
                try
                {
                    if (flagBooked == true && flagCanceled == true)
                        return "Cancelled";
                    else if (flagCanceled == false)
                        return "Booked";
                }
                catch { }
                return "";
            }
        }
    }
}
