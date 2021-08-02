using System;
using System.Collections.Generic;
using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.ViewModels;
using Xamarin.Forms;

namespace ConasiCRM.Portable.Views
{
    public partial class FollowUpListPage : ContentPage
    {
        public FollowUpListPageViewModel viewModel;
        public FollowUpListPage()
        {
            InitializeComponent();
            this.BindingContext = viewModel = new FollowUpListPageViewModel();
            LoadingHelper.Show();
            Init();
        }

        public async void Init()
        {
            await viewModel.LoadData();
            LoadingHelper.Hide();
        }

        private void listView_ItemTapped(System.Object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            LoadingHelper.Show();
            var item = e.Item as FollowUpListPageModel;
            FollowDetailPage followDetailPage = new FollowDetailPage(item.bsd_followuplistid);
            followDetailPage.OnLoaded = async(isSuccess) =>
            {
                if (isSuccess)
                {
                    await Navigation.PushAsync(followDetailPage);
                    LoadingHelper.Hide();
                }
                else
                {
                    await DisplayAlert("", "Không tìm thấy dữ liệu", "Đóng");
                    LoadingHelper.Hide();
                }
            };
            
        }

        private async void Search_Pressed(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            await viewModel.LoadOnRefreshCommandAsync();
            LoadingHelper.Hide();
        }

        private void Search_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(viewModel.Keyword))
            {
                Search_Pressed(null, EventArgs.Empty);
            }
        }
    }
}
