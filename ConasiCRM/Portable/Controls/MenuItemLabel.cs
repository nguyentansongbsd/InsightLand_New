using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ConasiCRM.Portable.Controls
{
    public class MenuItemLabel : Label
    {
        public event EventHandler Clicked;

        public MenuItemLabel()
        {
            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += (s, e) =>
            {
                try
                {
                    Clicked.Invoke(s, e);
                }
                catch { }
            };
            this.GestureRecognizers.Add(tapGestureRecognizer);
        }
    }
}
