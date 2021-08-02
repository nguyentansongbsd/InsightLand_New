using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Telerik.XamarinForms.DataGrid;
using Telerik.XamarinForms.DataGrid.Commands;
using Xamarin.Forms;

namespace ConasiCRM.Portable.ViewModels
{
    public class GridCellTapCommand<T, TView> : DataGridCommand
        where T : class
        where TView : ContentPage
    {
        private string _idAttrName;
        public GridCellTapCommand(string idAttrName)
        {
            Id = DataGridCommandId.CellTap;
            _idAttrName = idAttrName;
        }
        public override bool CanExecute(object parameter)
        {
            return true;
        }
        public override void Execute(object parameter)
        {
            var context = parameter as DataGridCellInfo;
            T model = (T)context.Item;
            //string idAttrName = contact.GetType().Name.ToLower() + "id";
            PropertyInfo myPropInfo = typeof(T).GetProperty(_idAttrName);
            object val = myPropInfo.GetValue(model, null);
            TView view = (TView)Activator.CreateInstance(typeof(TView), val);
            if (App.Current.MainPage.Navigation.NavigationStack[App.Current.MainPage.Navigation.NavigationStack.Count - 1].GetType() != view.GetType())
            {
                Application.Current.MainPage.Navigation.PushAsync(view);
                this.Owner.CommandService.ExecuteDefaultCommand(DataGridCommandId.CellTap, parameter);
            }
        }
    }
}
