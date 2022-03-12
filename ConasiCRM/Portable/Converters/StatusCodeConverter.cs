using ConasiCRM.Portable.Resources;
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
            if ((string)value == "1") //lead_new_sts
            {
                return Language.lead_new_sts; // New
            }
            else if ((string)value == "2") //lead_contacted_sts
            {
                return Language.lead_contacted_sts; // Contacted
            }
            else if ((string)value == "3") //lead_qualified_sts
            {
                return Language.lead_qualified_sts; //Qualified
            }
            else if ((string)value == "4") //lead_lost_sts
            {
                return Language.lead_lost_sts; // Lost
            }
            else if ((string)value == "5") //lead_cannot_contact_sts
            {
                return Language.lead_cannot_contact_sts; // Cannot Contact
            }
            else if ((string)value == "6") //lead_no_longer_interested_sts
            {
                return Language.lead_no_longer_interested_sts;  //No Longer Interested
            }
            else if ((string)value == "7") //lead_canceled_sts
            {
                return Language.lead_canceled_sts; // Canceled
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
