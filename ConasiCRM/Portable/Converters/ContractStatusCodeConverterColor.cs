using ConasiCRM.Portable.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace ConasiCRM.Portable.Converters
{
    public class ContractStatusCodeConverterColor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                return ContractStatusCodeData.GetContractStatusCodeById(value.ToString()).Background;
            }
            else
            {
                return "#bfbfbf";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
