using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace ConasiCRM.Portable.Converters.Unit
{
    public class StatusLabelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return "";
            var result = string.Empty;
            switch ((int)value)
            {
                case 1:
                    result = "Preparing";
                    break;
                case 100000000:
                    result = "Available";
                    break;
                case 100000004:
                    result = "Queuing";
                    break;
                case 100000007:
                    result = "Giữ chỗ";
                    break;
                case 100000006:
                    result = "Reserve";
                    break;
                case 100000005:
                    result = "Collected";
                    break;
                case 100000003:
                    result = "Deposited";
                    break;
                case 100000001:
                    result = "1st Installment";
                    break;
                case 100000008:
                    result = "Đủ điều kiện";
                    break;
                case 100000009:
                    result = "Thỏa thuận đặt cọc";
                    break;
                case 100000002:
                    result = "Sold";
                    break;
                default:
                    break;
            }
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
