using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConasiCRM.Portable.Controls;
using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Helpers;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConasiCRM.Portable.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Dashboard : ContentPage
    {
        public static bool? NeedToRefreshQueue = null;
        public static bool? NeedToRefreshLeads = null;
        public static bool? NeedToRefreshActivity = null;
        public DashboardViewModel viewModel;

        public Dashboard()
        {
            InitializeComponent();
            LoadingHelper.Show();
            NeedToRefreshQueue = false;
            NeedToRefreshLeads = false;
            Init();
        }

        public async void Init()
        {
            this.BindingContext = viewModel = new DashboardViewModel();
           
            await Task.WhenAll(
                 viewModel.LoadTasks(),
                 viewModel.LoadMettings(),
                 viewModel.LoadPhoneCalls(),
                 viewModel.LoadQueueFourMonths(),
                 viewModel.LoadQuoteFourMonths(),
                 viewModel.LoadOptionEntryFourMonths(),
                 viewModel.LoadUnitFourMonths(),
                 viewModel.LoadLeads(),
                 viewModel.LoadCommissionTransactions()
                ) ;
            LoadingHelper.Hide();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            if (NeedToRefreshQueue == true)
            {
                LoadingHelper.Show();
                viewModel.DataMonthQueue.Clear();
                await viewModel.LoadQueueFourMonths();
                NeedToRefreshQueue = false;
                LoadingHelper.Hide();
            }

            if (NeedToRefreshLeads == true)
            {
                LoadingHelper.Show();
                viewModel.LeadsChart.Clear();
                await viewModel.LoadLeads();
                NeedToRefreshLeads = false;
                LoadingHelper.Hide();
            }
            if (NeedToRefreshActivity == true && viewModel.Activities != null)
            {
                LoadingHelper.Show();
                viewModel.Activities.Clear();
                await Task.WhenAll(
                     viewModel.LoadMettings(),
                     viewModel.LoadTasks(),
                     viewModel.LoadPhoneCalls()
                     );
                ActivityPopup.Refresh();
                NeedToRefreshActivity = false;
                LoadingHelper.Hide();
            }
        }

        private async void ShowMore_Tapped(object sender,EventArgs e)
        {
            LoadingHelper.Show();
            await Shell.Current.GoToAsync("//HoatDong");
            LoadingHelper.Hide();
        }

        private async void ActivitiItem_Tapped(object sender,EventArgs e)
        {
            LoadingHelper.Show();
            var item = (ActivitiModel)((sender as ExtendedFrame).GestureRecognizers[0] as TapGestureRecognizer).CommandParameter;
            if (item != null && item.activityid != Guid.Empty)
            {
                ActivityPopup.ShowActivityPopup(item.activityid, item.activitytypecode);
            }
            LoadingHelper.Hide();
        }
    }
}