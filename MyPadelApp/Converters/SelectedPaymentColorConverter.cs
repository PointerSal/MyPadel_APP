using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPadelApp.Converters
{
    public class SelectedPaymentColorConverter : IValueConverter
    {
        public ObservableCollection<string> PaymentMethods = new ObservableCollection<string> 
        {
            "PayPal",
            "CreditCard",
            "Satispay"
        };
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null && PaymentMethods.Any(e=>e.Equals(value.ToString())) ? Colors.Blue : Colors.White;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
