using System;
using Xamarin.Forms;

namespace ConasiCRM.Portable.Controls
{
    public class LeadStateCodeToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var a = value == null || ((string)value) == "0";
            return a;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
