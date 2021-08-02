using System;
using Android.Content;
using ConasiCRM.Portable.Controls;
using ConasiCRM.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(MainEntry), typeof(MainEntryRenderer))]
namespace ConasiCRM.Droid.Renderers
{
    public class MainEntryRenderer : EntryRenderer
    {
        public MainEntryRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Entry> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement == null && Control != null)
            {
                FormsEditText editText = Control;
                editText.SetBackgroundResource(Resource.Drawable.bg_main_entry);
                editText.SetPadding(20, 0, 15, 0);
            }
        }
    }
}
