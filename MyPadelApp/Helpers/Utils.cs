using MyPadelApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPadelApp.Helpers
{
    public class Utils
    {
        public static User GetUser { get; set; } = null;
        public static bool IsUserLogin { get; set; } = false;
        public static bool IsHomeUpdated { get; set; } = true;
        public static bool IsBookingPageUpdated { get; set; } = false;
    }
}
