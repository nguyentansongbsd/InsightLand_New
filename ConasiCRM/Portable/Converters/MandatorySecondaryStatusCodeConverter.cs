using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace ConasiCRM.Portable.Converters
{
    public class MandatorySecondaryStatusCodeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                switch ((int)value)
                {
                    case 1: 
                        return "Active";
                    case 100000000: 
                        return "Applying";
                    case 100000001: 
                        return "Cancel";
                    case 2: 
                        return "Inactive";
                    default:
                        return "";
                }
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
