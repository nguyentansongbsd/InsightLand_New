using System;
using Foundation;
using UIKit;
using ConasiCRM.iOS.Services;
using ConasiCRM.Portable.IServices;

[assembly: Xamarin.Forms.Dependency(typeof(OpenAppSettings))]
namespace ConasiCRM.Portable.IServices
{
    public class OpenAppSettings : IOpenAppSettings
    {
        public void Open()
        {
            UIApplication.SharedApplication.OpenUrl(new NSUrl("app-settings:"));
        }
    }
}
