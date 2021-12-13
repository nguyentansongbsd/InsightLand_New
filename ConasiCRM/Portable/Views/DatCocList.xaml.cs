using ConasiCRM.Portable.Helper;
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
    public partial class DatCocList : ContentPage
    {
        private readonly DatCocListViewModel viewModel;
        public static bool? NeedToRefresh;

        public DatCocList()
        {
            InitializeComponent();
            BindingContext = viewModel = new DatCocListViewModel();
            NeedToRefresh = false;
            LoadingHelper.Show();
            Init();
        }
        public async void Init()
        {
            await viewModel.LoadData();
            LoadingHelper.Hide();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            if(NeedToRefresh == true)
            {
                LoadingHelper.Show();
                await viewModel.LoadOnRefreshCommandAsync();
                LoadingHelper.Hide();
                NeedToRefresh = false;
            }    
        }

        private void listView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ReservationListModel val = e.Item as ReservationListModel;
            LoadingHelper.Show();
            BangTinhGiaDetailPage newPage = new BangTinhGiaDetailPage(val.quoteid) { Title = "Đặt cọc" };
            newPage.OnCompleted = async (OnCompleted) =>
            {
                if (OnCompleted == true)
                {
                    await Navigation.PushAsync(newPage);
                }
                LoadingHelper.Hide();
            };
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