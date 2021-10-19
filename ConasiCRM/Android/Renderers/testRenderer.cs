using System;
using Android.Content;
using ConasiCRM.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Shell), typeof(testRenderer))]
namespace ConasiCRM.Droid.Renderers
{
    public class testRenderer : ShellRenderer
    {
        public testRenderer(Context context) : base(context)
        {
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
