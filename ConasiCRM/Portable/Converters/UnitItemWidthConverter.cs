using System;
using System.Globalization;
using SkiaSharp;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ConasiCRM.Portable.Converters
{
    public class UnitItemWidthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Thickness thickness = (Thickness)value;
            var padding = thickness.Left;
            var deviceWidth = DeviceDisplay.MainDisplayInfo.Width;
            var px = deviceWidth / (int)DeviceDisplay.MainDisplayInfo.Density; // convert sang pixel
            //{Binding ., Converter={StaticResource UnitsPaddingConverter}}
            //(padding * 2 + 8 + 8)
            var width = (px - (10 * 2 + 8 + 8)) / 2; // padding * 2 la tong trai va phai, 8 la tong marrign trai phai cua 1 item unit
            return width;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
