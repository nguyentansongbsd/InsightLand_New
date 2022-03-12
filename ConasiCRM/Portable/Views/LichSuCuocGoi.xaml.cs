using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.ViewModels;
using Xamarin.Forms;
using ConasiCRM.Portable.Controls;
using System.Collections.ObjectModel;
using ConasiCRM.Portable.Models;
using Xamarin.Essentials;

namespace ConasiCRM.Portable.Views
{
    public partial class LichSuCuocGoi : ContentPage
    {
        CallLogViewModel viewModel;

        public LichSuCuocGoi()
        {
            InitializeComponent();

            this.BindingContext = viewModel = new CallLogViewModel();

            LoadCallLog().GetAwaiter();
        }

        public async Task LoadCallLog()
        {
            if (await Permissions.CheckStatusAsync<Permissions.Phone>() != PermissionStatus.Granted
                || await Permissions.CheckStatusAsync<Permissions.ContactsRead>() != PermissionStatus.Granted 
                || await Permissions.CheckStatusAsync<Permissions.ContactsWrite>() != PermissionStatus.Granted)
            {
                await Navigation.PopAsync();
                return;
            }         

            viewModel.IsBusy = true;

            viewModel.CallLogs = DependencyService.Get<IAccessService>().GetCallLogs();
            //System.Diagnostics.Debugger.Break();
            viewModel.IsBusy = false;
        }
    }
}
