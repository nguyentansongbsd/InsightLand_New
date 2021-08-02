using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.Views;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Telerik.XamarinForms.DataGrid;
using Telerik.XamarinForms.DataGrid.Commands;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ConasiCRM.Portable.ViewModels
{
    public class GridCellQueueListTapCommand : DataGridCommand      
    {        
        public GridCellQueueListTapCommand()
        {
            this.Id = DataGridCommandId.CellTap;           
        }
        public override bool CanExecute(object parameter)
        {
            return true;
        }
        public override async void Execute(object parameter)
        {
            var context = parameter as DataGridCellInfo;
            QueueListModel val = (QueueListModel)context.Item;        

            string selected = await Application.Current.MainPage.DisplayActionSheet(null, null, null, "Liên hệ", "Chi tiết");
            if(selected == "Liên hệ")
            {
                var checkVadate = PhoneNumberFormatVNHelper.CheckValidate(val.telephone);
                if(checkVadate==true)
                    await Launcher.OpenAsync($"tel:{val.telephone}");
            }
            if (selected == "Chi tiết")
            {
                await Application.Current.MainPage.Navigation.PushAsync(new QueueForm(val.opportunityid));
                this.Owner.CommandService.ExecuteDefaultCommand(DataGridCommandId.CellTap, parameter);
            }                     
        }
    }
}
