using System;
using System.Collections.Generic;
using ConasiCRM.Portable.Models;
using Xamarin.Forms;

namespace ConasiCRM.Portable.Views
{
    public partial class LichSuTinNhanDetail : ContentPage
    {
        SMSGroupedModel SMSGroupedModel;
        public LichSuTinNhanDetail(SMSGroupedModel groupSMS)
        {
            InitializeComponent();

            this.SMSGroupedModel = groupSMS;

            this.listview.ItemsSource = SMSGroupedModel.lstSMS;

            this.Title = SMSGroupedModel.name_display;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var target = SMSGroupedModel.lstSMS[SMSGroupedModel.lstSMS.Count - 1];
            listview.ScrollTo(target, ScrollToPosition.Start, false);
        }
    }
}
