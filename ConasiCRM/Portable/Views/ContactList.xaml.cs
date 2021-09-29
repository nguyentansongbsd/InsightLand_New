using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConasiCRM.Portable.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ContactList : ContentPage
    {
        private readonly ContactListViewModel viewModel;
        public ContactList()
        {
            InitializeComponent();
            BindingContext = viewModel = new ContactListViewModel();
            LoadingHelper.Show();
            Init();
        }
        public async void Init()
        {
            await viewModel.LoadData();
            LoadingHelper.Hide();
        }

        protected override bool OnBackButtonPressed()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                var result = await this.DisplayAlert("Thông báo!", "Bạn có thực sự muốn thoát?", "Đồng ý", "Huỹ");

                if (result)
                {
                    System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow(); 
                }
            });
            return true;
        }

        private async void NewMenu_Clicked(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            await Navigation.PushAsync(new ContactForm());
            LoadingHelper.Hide();
        }
        protected override void OnAppearing()
        {
            if (App.Current.Properties.ContainsKey("update") && App.Current.Properties["update"] as string == "1")
            {
                App.Current.Properties["update"] = "0";
                viewModel.RefreshCommand.Execute(null);
            }
        }

        private void listView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            LoadingHelper.Show();
            var item = e.Item as ContactListModel;
            ContactForm newPage = new ContactForm(item.contactid);
            newPage.OnCompleted = async (CheckSingleContact) =>
            {
                if (CheckSingleContact == true)
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