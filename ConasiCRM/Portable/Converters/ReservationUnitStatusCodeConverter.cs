using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace ConasiCRM.Portable.Converters
{
    public class ReservationUnitStatusCodeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                switch ((int)value)
                {
                    case 0:
                        return "Nháp";
                    case 1:
                        return "Chuẩn bị";
                    case 100000000:
                        return "Sẵn sàng";
                    case 100000004:
                        return "Giữ chỗ";
                    case 100000006:
                        return "Đặt cọc";
                    case 100000005:
                        return "Đồng ý chuyển cọc";
                    case 100000003:
                        return "Đã đủ tiền cọc";
                    case 100000001:
                        return "Thanh toán đợt 1";
                    case 100000002:
                        return "Đã bán";
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
