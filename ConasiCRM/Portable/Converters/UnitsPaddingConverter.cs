using System;
using System.Globalization;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ConasiCRM.Portable.Converters
{
    public class UnitsPaddingConverter :IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var width = DeviceDisplay.MainDisplayInfo.Width;
            if (width <= 720)
            {
                return 8;
            }
            else if (width > 720 && width <= 750)
            {
                return 10;
            }
            else
            {
                return 14;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
