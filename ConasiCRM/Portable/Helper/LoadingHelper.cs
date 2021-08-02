using System;
using System.Threading.Tasks;
using ConasiCRM.Portable.IServices;
using Xamarin.Forms;

namespace ConasiCRM.Portable.Helper
{
    public class LoadingHelper
    {
        public static void Show()
        {
            try
            {
                DependencyService.Get<IServices.ILoadingService>().Show();
            }
            catch (Exception ex)
            {

            }
        }

        public static void Hide()
        {
            DependencyService.Get<ILoadingService>().Hide();
        }
    }
}
