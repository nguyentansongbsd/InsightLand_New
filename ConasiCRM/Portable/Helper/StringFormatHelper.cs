using ConasiCRM.Portable.Settings;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace ConasiCRM.Portable.Helper
{
    public class StringFormatHelper
    {
        public static string FormatCurrency(decimal? input)
        {
            if (input.HasValue)
            {
                if (input.Value == 0)
                    return null;
                else if (UserLogged.Language == "en") 
                    return string.Format("{0:#,##0.##}", input.Value); // luôn có 2 số thập phân 0.00 thay ## nếu k cần
                else
                    return String.Format(new CultureInfo("vi-VN"), "{0:#,##0.##}", input.Value);
            }
            return null;
        }
    }
}
