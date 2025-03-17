using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPadelApp.Models
{
    public partial class CourtsDuration : ObservableObject
    {
        [ObservableProperty]
        public bool? _isSelected = false;
        public int? Duration { get; set; }
    }
}
