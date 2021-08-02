using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConasiCRM.Portable.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LeadsContentView : ContentView
    {
        private readonly LeadsContentViewViewModel viewModel;
        public Action<bool> OnCompleted;
        public LeadsContentView()
        {
            InitializeComponent();
            this.BindingContext = viewModel = new LeadsContentViewViewModel();
            LoadingHelper.Show();
            Init();
        }
        public async void Init()
        {
            await viewModel.LoadData();
            if (viewModel.Data.Count > 0)
            {
                OnCompleted?.Invoke(true);
            }
            else
            {
                OnCompleted?.Invoke(false);
            }
        }

        private void listView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            LoadingHelper.Show();
            var item = e.Item as LeadListModel;
            LeadDetailPage newPage = new LeadDetailPage(item.leadid);
            newPage.OnCompleted = async (OnCompleted) =>
            {
                if (OnCompleted == true)
                {
                    await Navigation.PushAsync(newPage);
                    LoadingHelper.Hide();
                }
                else
                {
                    LoadingHelper.Hide();
                    await Shell.Current.DisplayAlert("Thông báo", "Không tìm thấy thông tin", "Đóng");
                }
            };
        }

        private async void Search_Pressed(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            await viewModel.LoadOnRefreshCommandAsync();
            LoadingHelper.Hide();
        }

        private async void Search_TextChanged(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            if (string.IsNullOrEmpty(viewModel.Keyword))
            {
                await viewModel.LoadOnRefreshCommandAsync();
            }
            LoadingHelper.Hide();
        }
    }
}