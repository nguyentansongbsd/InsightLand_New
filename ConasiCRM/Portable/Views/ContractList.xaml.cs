using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Helpers;
using ConasiCRM.Portable.Models;
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
        public ContractListViewModel viewModel;
        public ContractList()
        {
            InitializeComponent(); 
            BindingContext = viewModel = new ContractListViewModel();
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

        private async void listView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            LoadingHelper.Show();
            ContractModel item = e.Item as ContractModel;
            ContractDetailPage contractDetailPage = new ContractDetailPage(item.salesorderid);
            await Navigation.PushAsync(contractDetailPage);
            //contractDetailPage.OnCompleted = async (OnCompleted) =>
            //{
            //    if (OnCompleted == true)
            //    {
            //        await Navigation.PushAsync(contractDetailPage);
            //        LoadingHelper.Hide();
            //    }
            //    else
            //    {
            //        LoadingHelper.Hide();
            //        ToastMessageHelper.ShortMessage("Không tìm thấy thông tin hợp đồng");
            //    }

            //};
        }
    }
}