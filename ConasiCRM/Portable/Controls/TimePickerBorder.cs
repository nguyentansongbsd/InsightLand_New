using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ConasiCRM.Portable.Controls
{
    public class TimePickerBorder : TimePicker
    {
        public TimePickerBorder()
        {
            this.FontSize = 15;
            this.TextColor = Color.FromHex("#333333");
        }
    }
}
