using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ConasiCRM.Portable;

namespace ConasiCRM.Android
{
    [Activity(Label = "Insight Land", Icon = "@drawable/conasi_logo", Theme = "@style/Theme.Splash", MainLauncher = true, NoHistory = true)]
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