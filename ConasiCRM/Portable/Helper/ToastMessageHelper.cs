using System;
using ConasiCRM.Portable.IServices;
using Xamarin.Forms;

namespace ConasiCRM.Portable.Helpers
{
    public class ToastMessageHelper
    {
        public static void ShortMessage(string message)
        {
            DependencyService.Get<IToastMessage>().ShortAlert(message);
        }

        public static void LongMessage(string message)
        {
            DependencyService.Get<IToastMessage>().LongAlert(message);
        }
    }
}
