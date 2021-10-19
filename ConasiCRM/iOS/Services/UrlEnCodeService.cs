using System;
using System.Threading.Tasks;
using ConasiCRM.iOS.Services;
using ConasiCRM.Portable.IServices;
using Xamarin.Forms;

[assembly: Dependency(typeof(UrlEnCodeService))]
namespace ConasiCRM.iOS.Services
{
    public class UrlEnCodeService : IUrlEnCodeSevice
    {
        public async Task<string> GetUrlEnCode(string url)
        {
            var jrURL = new Foundation.NSUrl(new System.Uri(url).AbsoluteUri);
            return jrURL.AbsoluteString;
        }
    }
}
