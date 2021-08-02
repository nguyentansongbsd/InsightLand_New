using System;
using Xamarin.Forms;

namespace ConasiCRM.Portable.Controls
{
    public class LookUpEntry : Entry
    {
        public LookUpEntry()
        {
            TextColor = Color.Black;
            this.FontSize = 16;
            this.PlaceholderColor = Color.Gray;
            this.HeightRequest = 40;
            this.FontFamily = "Segoe";
        }
    }
}
