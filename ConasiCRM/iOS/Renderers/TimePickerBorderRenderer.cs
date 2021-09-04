using ConasiCRM.iOS.Renderers;
using ConasiCRM.Portable.Controls;
using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(TimePickerBorder), typeof(TimePickerBorderRenderer))]
namespace ConasiCRM.iOS.Renderers
{
    public class TimePickerBorderRenderer : TimePickerRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<TimePicker> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null)
            {
                Control.BorderStyle = UIKit.UITextBorderStyle.RoundedRect;
                Control.Layer.BorderWidth = 1f;
                Control.Layer.CornerRadius = 6;
                Control.Layer.BorderColor = Color.FromHex("#c9c9c9").ToCGColor();
                Control.AdjustsFontSizeToFitWidth = true;
                Element.HeightRequest = 40f;
            }
        }
    }
}