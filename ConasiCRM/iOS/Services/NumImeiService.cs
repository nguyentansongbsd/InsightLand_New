using System;
using System.Threading.Tasks;
using ConasiCRM.iOS.Services;
using ConasiCRM.Portable.IServices;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(NumImeiService))]
namespace ConasiCRM.iOS.Services
{
    public class NumImeiService :INumImeiService
    {
        public async Task<string> GetImei()
        {
            return UIDevice.CurrentDevice.IdentifierForVendor.AsString();
        }
    }
}
