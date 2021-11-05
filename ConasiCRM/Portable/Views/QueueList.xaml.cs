using ConasiCRM.Portable.Config;
using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Helpers;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.Services;
using ConasiCRM.Portable.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telerik.XamarinForms.Common;
using Telerik.XamarinForms.DataGrid;
using Telerik.XamarinForms.DataGrid.Commands;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConasiCRM.Portable.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QueueList : ContentPage
    {
        private readonly QueuListViewModel viewModel;
        public ICRMService<QueueListModel> service;
        public QueueList()
        {
            InitializeComponent();
            LoadingHelper.Show();
            BindingContext = viewModel = new QueuListViewModel();
            Init();
        }
        public async void Init()
        {
            await Task.WhenAll(
                  viewModel.LoadData(),
                  viewModel.LoadProject()
                  );
            viewModel.LoadStatus();
            LoadingHelper.Hide();
        }

        private async void NewMenu_Clicked(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            await Navigation.PushAsync(new AccountForm());
            LoadingHelper.Hide();
        }

        private async void listView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            QueuesModel val = e.Item as QueuesModel;
            LoadingHelper.Show();
            //QueueForm newPage = new QueueForm(val.opportunityid);
            //newPage.CheckQueueInfo = async (CheckQueueInfo) =>
            //{
            //    if (CheckQueueInfo == true)
            //    {
            //        await Navigation.PushAsync(newPage);                 
            //    }
            //    LoadingHelper.Hide();
            //};
            await Navigation.PushAsync(new QueuesDetialPage(val.opportunityid));
            LoadingHelper.Hide();
        }

        private async void SearchBar_SearchButtonPressed(System.Object sender, System.EventArgs e)
        {
            LoadingHelper.Show();
            await viewModel.LoadOnRefreshCommandAsync();
            LoadingHelper.Hide();
        }

        private void SearchBar_TextChanged(System.Object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(viewModel.Keyword))
            {
                SearchBar_SearchButtonPressed(null, EventArgs.Empty);
            }
        }        

        private async void FiltersProject_SelectedItemChange(object sender, LookUpChangeEvent e)
        {
            LoadingHelper.Show();
            await viewModel.LoadOnRefreshCommandAsync();
            viewModel.FiltersUnit.Clear();
            await viewModel.LoadUnit();
            LoadingHelper.Hide();
        }

        private async void FiltersStatus_SelectedItemChanged(object sender, LookUpChangeEvent e)
        {
            LoadingHelper.Show();
            await viewModel.LoadOnRefreshCommandAsync();
            LoadingHelper.Hide();
        }

        private async void FiltersUnit_SelectedItemChanged(object sender, LookUpChangeEvent e)
        {
            LoadingHelper.Show();
            await viewModel.LoadOnRefreshCommandAsync();
            LoadingHelper.Hide();
        }
    }
}