using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ConasiCRM.Portable.Controls
{
    public class ModalContentView : ContentView
    {
        public ModalContentView()
        {
            AbsoluteLayout.SetLayoutBounds(this, new Rectangle(0, 0, 1, 1));
            AbsoluteLayout.SetLayoutFlags(this, AbsoluteLayoutFlags.All);
            this.BackgroundColor = Color.FromRgba(0, 0, 0, 0.5);
            this.Padding = new Thickness(20);
        }
    }
}
