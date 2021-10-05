using System;
using System.Collections.Generic;
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
            await viewModel.LoadTasks();

            BindableLayout.SetItemsSource(stTaskList, viewModel.Tasks);

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