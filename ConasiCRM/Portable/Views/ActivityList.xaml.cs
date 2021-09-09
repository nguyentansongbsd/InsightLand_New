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
    public partial class ActivityList : ContentPage
    {
        public Action<bool> OnCompleted;
        public ActivityListViewModel viewModel;
        public ActivityList()
        {
            InitializeComponent();
            BindingContext = viewModel = new ActivityListViewModel();
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

        private async void listView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item != null)
            {               
                var item = e.Item as HoatDongListModel;
                if (item.activityid != Guid.Empty)
                {
                    if (item.activitytypecode == "phonecall")
                    {
                        LoadingHelper.Show();
                        PhoneCallForm newPage = new PhoneCallForm(item.activityid);
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
                                ToastMessageHelper.ShortMessage("Không tìm thấy thông tin. Vui lòng thử lại");
                            }
                        };
                    }
                    else if (item.activitytypecode == "task")
                    {
                        
                    }
                    else
                    { }
                }
            }
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

        private async void NewActivity_Clicked(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            string[] options = new string[] { "Tạo Cuộc Họp", "Tạo Cuộc Gọi", "Tạo Công Việc" };
            string asw = await DisplayActionSheet("Tuỳ chọn", "Hủy", null, options);
            if (asw == "Tạo Cuộc Họp")
            {             
            }
            else if (asw == "Tạo Cuộc Gọi")
            {
                await Navigation.PushAsync(new PhoneCallForm());
            }
            else if (asw == "Tạo Công Việc")
            {
                await Navigation.PushAsync(new TaskForm());
            }
            LoadingHelper.Hide();
        }
    }
}