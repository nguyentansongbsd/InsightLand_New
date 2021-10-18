using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace ConasiCRM.Portable.Views
{
    public partial class UserInfoPage : ContentPage
    {
        public Action OnCompleted;
        public UserInfoPage()
        {
            InitializeComponent();

            OnCompleted?.Invoke();
        }
    }
}
