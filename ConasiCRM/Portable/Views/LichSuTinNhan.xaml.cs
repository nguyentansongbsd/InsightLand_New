﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.ViewModels;
using Xamarin.Forms;
using ConasiCRM.Portable.Controls;
using Xamarin.Essentials;

namespace ConasiCRM.Portable.Views
{
    public partial class LichSuTinNhan : ContentPage
    {
        SMSViewModel viewModel;
        public LichSuTinNhan()
        {
            InitializeComponent();

            this.BindingContext = viewModel = new SMSViewModel();

            this.LoadSMS().GetAwaiter();
        }

        public async Task LoadSMS()
        {
            if (await Permissions.CheckStatusAsync<Permissions.Sms>() != PermissionStatus.Granted
                 || await Permissions.CheckStatusAsync<Permissions.ContactsRead>() != PermissionStatus.Granted
                 || await Permissions.CheckStatusAsync<Permissions.ContactsWrite>() != PermissionStatus.Granted)
            {
                await Navigation.PopAsync();
                return;
            }

            viewModel.IsBusy = true;

            viewModel.lstAllSMS = DependencyService.Get<IAccessService>().GetSMS();
            await viewModel.GroupSMS();
            //System.Diagnostics.Debugger.Break();
            viewModel.IsBusy = false;
        }

        void SMS_Tapped(object sender, System.EventArgs e)
        {
            var stacklayout = sender as StackLayout;
            var tapGes = (TapGestureRecognizer)stacklayout.GestureRecognizers[0];
            var item = (Models.SMSGroupedModel)tapGes.CommandParameter;

            if(item != null)
            {
                Navigation.PushAsync(new LichSuTinNhanDetail(item));
            }
        }
    }
}
