using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace ConasiCRM.Portable.Converters
{
    public class ReservationStatusCodeConverterColor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                switch ((int)value)
                {
                    case 6:
                        return "#FFCC33";
                    case 3:
                        return "#00FFFF";
                    case 100000006:
                        return "#FF3333";
                    case 100000009:
                        return "#3399FF";
                    case 100000005:
                        return "#FF00FF";
                    case 100000008:
                        return "#FF33FF";
                    case 2:
                        return "#FFFF99";
                    case 1:
                        return "#33ff66";
                    case 5:
                        return "#FF6633";
                    case 100000002:
                        return "#00CCFF";
                    case 100000007:
                        return "#FFFF00";
                    case 100000003:
                        return "#FF3399";
                    case 100000000:
                        return "#FFCC66";
                    case 7:
                        return "#990033";
                    case 100000004:
                        return "#33CCFF";
                    case 100000001:
                        return "#FF9933";
                    case 4:
                        return "#60111111";
                    default:
                        return "#60111111";
                }
            }
            return "#60111111";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
