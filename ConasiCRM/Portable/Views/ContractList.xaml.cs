using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConasiCRM.Portable.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ContractList : ContentPage
    {
        public ActivityListViewModel viewModel;
        public ContractList()
        {
            InitializeComponent(); 
            BindingContext = viewModel = new ActivityListViewModel();
            LoadingHelper.Show();
            Init();
        }
        public async void Init()
        {
            await viewModel.LoadData();
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
    }
}