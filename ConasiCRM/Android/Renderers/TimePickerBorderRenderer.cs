using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ConasiCRM.Droid.Renderers;
using ConasiCRM.Portable.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(TimePickerBorder), typeof(TimePickerBorderRenderer))]
namespace ConasiCRM.Droid.Renderers
{
    public class TimePickerBorderRenderer : TimePickerRenderer
    {
        public TimePickerBorderRenderer(Context context) : base(context)
        {
        }
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.TimePicker> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null)
            {
                Control.SetBackgroundResource(Resource.Drawable.bg_main_entry);
                Control.SetPadding(15, 18, 15, 18);
            }
        }
    }
}