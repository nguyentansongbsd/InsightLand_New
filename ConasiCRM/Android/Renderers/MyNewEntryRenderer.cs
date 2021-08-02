using System;
using Android.Content;
using Android.Runtime;
using Android.Widget;
using ConasiCRM.Portable.Controls;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Content.Res;
using Android.Graphics;
using Android.Text;

[assembly: ExportRenderer(typeof(EntryUnfocused),
                          typeof(ConasiCRM.Android.Renders.MyNewEntryRenderer))]

namespace ConasiCRM.Android.Renders
{
    public class MyNewEntryRenderer : EntryRenderer
    {
        public MyNewEntryRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if(e.NewElement != null && Control != null)
            {
                Control.SetCursorVisible(false);
                //Control.SetHighlightColor(Xamarin.Forms.Color.White.ToAndroid());
                Control.BackgroundTintList = ColorStateList.ValueOf(Xamarin.Forms.Color.FromHex("#7b8189").ToAndroid());
            }
        }
    }
}
