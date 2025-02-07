using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPadelApp.Models.Responses
{
    public class GeneralResponse
    {
        public string? code { get; set; }
        public string? message { get; set; } = "";
        public object? data { get; set; }
    }
}
