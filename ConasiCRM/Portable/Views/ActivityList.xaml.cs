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

        private void listView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            LoadingHelper.Show();
            //HoatDongListModel val = e.Item as HoatDongListModel;
            //if (val.activitytypecode == "task")
            //{
            //    TaskForm newPage = new TaskForm(val.activityid);
            //    newPage.CheckTaskForm = async (CheckEventData) =>
            //    {
            //        if (CheckEventData == true)
            //        {
            //            await Navigation.PushAsync(newPage);
            //        }
            //        LoadingHelper.Hide();
            //    };
            //}
            //else if (val.activitytypecode == "phonecall")
            //{
            //    PhoneCallForm newPage = new PhoneCallForm(val.activityid);
            //    newPage.CheckPhoneCell = async (CheckEventData) =>
            //    {
            //        if (CheckEventData == true)
            //        {
            //            await Navigation.PushAsync(newPage);
            //        }
            //        LoadingHelper.Hide();
            //    };
            //}
            //else if (val.activitytypecode == "appointment")
            //{
            //    MeetingForm newPage = new MeetingForm(val.activityid);
            //    newPage.CheckMeeting = async (CheckEventData) =>
            //    {
            //        if (CheckEventData == true)
            //        {
            //            await Navigation.PushAsync(newPage);
            //        }
            //        LoadingHelper.Hide();
            //    };
            //}
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

        private async Task NewActivity_ClickedAsync(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            string[] options = new string[] { "Tạo Cuộc Họp", "Tạo Cuộc Gọi", "Tạo Công Việc" };
            string asw = await DisplayActionSheet("Tuỳ chọn", "Hủy", null, options);
            if (asw == "Tạo Cuộc Họp")
            {
                await Navigation.PushAsync(new LeadForm());
            }
            else if (asw == "Tạo Cuộc Gọi")
            {
                await Navigation.PushAsync(new ContactForm());
            }
            else if (asw == "Tạo Công Việc")
            {
                await Navigation.PushAsync(new AccountForm());
            }
            LoadingHelper.Hide();
        }
    }
}