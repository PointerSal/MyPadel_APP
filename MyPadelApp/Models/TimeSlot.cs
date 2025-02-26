using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPadelApp.Models
{
    public class TimeSlot
    {
        public string? FieldName { get; set; }
        public ObservableCollection<TimeSlotDetails> TimeSlots { get; set; } = new ObservableCollection<TimeSlotDetails>();
    }
    public class TimeSlotDetails
    {
        public string? FieldName { get; set; }
        public DateTime Time { get; set; }
        public string Status { get; set; }
    }
}
