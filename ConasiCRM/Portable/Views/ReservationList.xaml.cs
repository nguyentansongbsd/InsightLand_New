using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConasiCRM.Portable.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReservationList : ContentPage
    {
        private readonly ReservationListViewModel viewModel;
        public ReservationList()
        {
            InitializeComponent();
            BindingContext = viewModel = new ReservationListViewModel();
            LoadingHelper.Show();
            Init();
        }
        public async void Init()
        {
            await viewModel.LoadData();
            LoadingHelper.Hide();
        }    

        private void listView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ReservationListModel val = e.Item as ReservationListModel;
            LoadingHelper.Show();
            ReservationForm newPage = new ReservationForm(val.quoteid);
            newPage.CheckReservation = async (CheckReservation) =>
            {
                if (CheckReservation == true)
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