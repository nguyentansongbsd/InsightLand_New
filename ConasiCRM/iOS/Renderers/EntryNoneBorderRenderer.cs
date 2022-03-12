﻿using System;
using ConasiCRM.iOS.Renderers;
using ConasiCRM.Portable.Controls;
using CoreGraphics;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(EntryNoneBorderControl),typeof(EntryNoneBorderRenderer))]
namespace ConasiCRM.iOS.Renderers
{
    public class EntryNoneBorderRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.Layer.BorderWidth = 0;
                Control.BackgroundColor = UIColor.White;
                Control.BorderStyle = UITextBorderStyle.None;
            }
        }
    }
}
