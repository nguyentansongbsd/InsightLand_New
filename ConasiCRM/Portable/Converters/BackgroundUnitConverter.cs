using System;
using System.Globalization;
using ConasiCRM.Portable.Models;
using Xamarin.Forms;

namespace ConasiCRM.Portable.Converters
{
    public class BackgroundUnitConverter :IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return StatusCodeUnit.GetStatusCodeById(value.ToString()).Background;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
