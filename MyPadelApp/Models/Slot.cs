using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPadelApp.Models
{
    public class Slot
    {
        public string? startTime { get; set; }
        public string? endTime { get; set; }
    }
    public class Datum
    {
        public string? fieldName { get; set; }
        public List<Slot>? slots { get; set; }
    }
}
