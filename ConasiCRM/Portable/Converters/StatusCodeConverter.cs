using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace ConasiCRM.Portable.Converters
{
    public class StatusCodeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((string)value == "1")
            {
                return "New";
            }
            else if ((string)value == "2")
            {
                return "Contacted";
            }
            else if ((string)value == "3")
            {
                return "Qualified";
            }
            else if ((string)value == "4")
            {
                return "Lost";
            }
            else if ((string)value == "5")
            {
                return "Cannot Contact";
            }
            else if ((string)value == "6")
            {
                return "No Longer Interested";
            }
            else if ((string)value == "7")
            {
                return "Canceled";
            }
            else
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
