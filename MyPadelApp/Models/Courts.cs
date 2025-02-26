using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPadelApp.Models
{
    public partial class Courts : ObservableObject
    {
        [ObservableProperty]
        public bool? _isSelected = false;
        public string? SportName {  get; set; }  
        public List<string>? Fields {  get; set; }  
    }
}
