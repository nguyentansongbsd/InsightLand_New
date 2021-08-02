using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.ViewModels;
using Xamarin.Forms;
using permissionType = Plugin.Permissions.Abstractions.Permission;
using permissionStatus = Plugin.Permissions.Abstractions.PermissionStatus;
using ConasiCRM.Portable.Controls;
using System.Collections.ObjectModel;
using ConasiCRM.Portable.Models;

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
            if (await PermissionHelper.CheckPermissions(permissionType.Contacts) != permissionStatus.Granted
                || await PermissionHelper.CheckPermissions(permissionType.Phone) != permissionStatus.Granted)
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
