using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPadelApp.Models
{
    public class User
    {
        public int? id { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; } 
        public string? Cells { get; set; }
        public string? OTP { get; set; }
        public bool FlagMailVerified { get; set; } = false;
        public bool FlagPhoneVerified { get; set; } = false;
        public bool FlagActive { get; set; } = false;
    }
}
