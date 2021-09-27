using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace ConasiCRM.Portable.Converters
{
    public class InstallmentsStatusCodeConverterColor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                switch ((int)value)
                {
                    case 1:
                        return "#06CF79";
                    case 100000000:
                        return "#03ACF5";
                    case 100000001:
                        return "#FDC206";
                    case 2:
                        return "#FA7901";
                    default:
                        return "#f1f1f1";
                }
            }
            return "#f1f1f1";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
