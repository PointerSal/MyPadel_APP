using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPadelApp.Converters
{
    public class DatePickerPlaceholderConverter : IValueConverter
    {
        public bool IsInvertedBool { get; set; } = false;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                var MyValue = value == null || (DateTime)value == default ? true : false;
                return IsInvertedBool ? !MyValue : MyValue;
            }
            catch { }
            return IsInvertedBool ? true : false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return "";
        }
    }
}
