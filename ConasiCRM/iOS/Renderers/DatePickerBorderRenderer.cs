﻿using System;
using ConasiCRM.iOS.Renderers;
using ConasiCRM.Portable.Controls;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(DatePickerBorder),typeof(DatePickerBorderRenderer))]
namespace ConasiCRM.iOS.Renderers
{
    public class DatePickerBorderRenderer : DatePickerRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<DatePicker> e)
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
