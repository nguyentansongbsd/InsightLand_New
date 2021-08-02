using System;
using Android.Content;
using ConasiCRM.Droid.Renderers;
using ConasiCRM.Portable.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(MainEditor), typeof(MainEditorRenderer))]
namespace ConasiCRM.Droid.Renderers
{
    public class MainEditorRenderer : EditorRenderer
    {
        public MainEditorRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null)
            {
                if (Control != null)
                {
                    Control.SetBackgroundResource(Resource.Drawable.bg_main_entry);
                    Control.SetPadding(15, 25, 15, 25);
                }
            }
        }
    }
}
