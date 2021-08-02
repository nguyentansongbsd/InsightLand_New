using System;
using System.Collections.Generic;
using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.ViewModels;
using Xamarin.Forms;

namespace ConasiCRM.Portable.Views
{
    public partial class LichLamViecTheoTuan : ContentPage
    {
        LichLamViecViewModel viewModel;
        public Action<bool> OnComplete;
        public LichLamViecTheoTuan()
        {
            InitializeComponent();

            this.BindingContext = viewModel = new LichLamViecViewModel();
            loadData();
        }

        public async void loadData()
        {
            //viewModel.selectedDate = DateTime.Now.Date;
            //viewModel.UpdateSelectedEventsForWeekView(viewModel.selectedDate.Value);

            //listView.ItemsSource = viewModel.selectedDateEventsGrouped;
            await viewModel.loadAllActivities();
            Handle_DateSelected(null, new Xamarin.Forms.DateChangedEventArgs(DateTime.Now, DateTime.Now));         
            if (viewModel.lstEvents != null && viewModel.lstEvents.Count > 0)
                OnComplete?.Invoke(true);
            else
                OnComplete?.Invoke(false);
        }

        void Handle_DateSelected(object sender, Xamarin.Forms.DateChangedEventArgs e)
        {
            viewModel.selectedDate = e.NewDate;
            viewModel.UpdateSelectedEventsForWeekView(viewModel.selectedDate.Value);

            listView.ItemsSource = viewModel.selectedDateEventsGrouped;
        }

        async void AddButton_Clicked(object sender, System.EventArgs e)
        {
            var choice = await DisplayActionSheet("Chọn Activity để thêm", "Huỷ", null, new String[] { "Cuộc gọi", "Công việc", "Cuộc họp" });
            LoadingHelper.Show();
            if (choice == "Công việc")
            {
                await Navigation.PushAsync(new TaskForm(viewModel.selectedDate.Value));
                LoadingHelper.Hide();
            }
            else if (choice == "Cuộc gọi")
            {
                await Navigation.PushAsync(new PhoneCallForm(viewModel.selectedDate.Value));
                LoadingHelper.Hide();
            }
            else if (choice == "Cuộc họp")
            {
                await Navigation.PushAsync(new MeetingForm(viewModel.selectedDate.Value));
                LoadingHelper.Hide();
            }
            LoadingHelper.Hide();
        }

        async void Event_Tapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            var val = e.Item as CalendarEvent;          
            if (val.Title != null)
            {
                LoadingHelper.Show();
                if (val.Activity.activitytypecode == "task")
                {
                    TaskForm newPage = new TaskForm(val.Activity.activityid);
                    newPage.CheckTaskForm = async (CheckEventData) =>
                    {
                        if (CheckEventData == true)
                        {
                            await Navigation.PushAsync(newPage);
                            LoadingHelper.Hide();
                        }
                        else
                        {
                            LoadingHelper.Hide();
                            await DisplayAlert("Thông Báo", "Không tìm thấy lịch làm việc", "Đóng");
                        }
                    };
                }
                else if (val.Activity.activitytypecode == "phonecall")
                {
                    PhoneCallForm newPage = new PhoneCallForm(val.Activity.activityid);
                    newPage.CheckPhoneCell = async (CheckEventData) =>
                    {
                        if (CheckEventData == true)
                        {
                            await Navigation.PushAsync(newPage);
                            LoadingHelper.Hide();
                        }
                        else
                        {
                            LoadingHelper.Hide();
                            await DisplayAlert("Thông Báo", "Không tìm thấy lịch làm việc", "Đóng");
                        }
                    };
                }
                else if (val.Activity.activitytypecode == "appointment")
                {
                    MeetingForm newPage = new MeetingForm(val.Activity.activityid);
                    newPage.CheckMeeting = async (CheckEventData) =>
                    {
                        if (CheckEventData == true)
                        {
                            await Navigation.PushAsync(newPage);
                            LoadingHelper.Hide();
                        }
                        else
                        {
                            LoadingHelper.Hide();
                            await DisplayAlert("Thông Báo", "Không tìm thấy lịch làm việc", "Đóng");
                        }
                    };
                }
            }
            else
            {
                LoadingHelper.Hide();
                await DisplayAlert("Thông Báo", "Không tìm thấy lịch làm việc", "Đóng");
            }
        }
    }
}
