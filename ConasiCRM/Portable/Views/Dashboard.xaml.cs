using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConasiCRM.Portable.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Dashboard : ContentPage
    {
        public DashboardViewModel viewModel;
        public Dashboard()
        {
            InitializeComponent();
            LoadingHelper.Show();
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
                 viewModel.LoadUnitFourMonths()
                ) ;


            BindableLayout.SetItemsSource(stTaskList, viewModel.Activities.Take(5));

            LoadingHelper.Hide();
        }

        private async void ShowMore_Tapped(object sender,EventArgs e)
        {
            LoadingHelper.Show();
            await Shell.Current.GoToAsync("//HoatDong");
            LoadingHelper.Hide();
        }
    }
}