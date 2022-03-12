using System;
using Android.App;
using Android.Content;
using Android.OS;
using ConasiCRM.Portable;

namespace ConasiCRM.Android
{
    [Activity(Label = "Conasi CRM", Icon = "@drawable/cns_logo_icon_app", Theme = "@style/Theme.Splash", MainLauncher = true, NoHistory = true)] // ConasiCRM ------CNS_Logo_1024
    public class SplashScreen : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            var intent = new Intent(this, typeof(MainActivity));
            //intent.AddFlags(ActivityFlags.ClearTop);
            //intent.AddFlags(ActivityFlags.SingleTop);
            StartActivity(typeof(MainActivity));
           // Finish();

            App.ScreenHeight = (int)(Resources.DisplayMetrics.HeightPixels / Resources.DisplayMetrics.Density);
            App.ScreenWidth = (int)(Resources.DisplayMetrics.WidthPixels / Resources.DisplayMetrics.Density);
        }
    }
}