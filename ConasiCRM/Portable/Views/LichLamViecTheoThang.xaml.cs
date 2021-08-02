using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.ViewModels;
using Telerik.XamarinForms.Input;
using Xamarin.Forms;

namespace ConasiCRM.Portable.Views
{
    public partial class LichLamViecTheoThang : ContentPage
    {
        LichLamViecViewModel viewModel;
        public Action<bool> OnComplete;
        public LichLamViecTheoThang()
        {
            InitializeComponent();
            this.BindingContext = viewModel = new LichLamViecViewModel();
            reset();
        }

        public void reset()
        {
            this.viewModel.reset();
            this.loadData();
        }

        public async void loadData()
        {            
            viewModel.selectedDate = DateTime.Today;
            this.seletedDay(viewModel.selectedDate.Value);
            await viewModel.loadAllActivities();
            if (viewModel.lstEvents != null && viewModel.lstEvents.Count > 0)
                OnComplete?.Invoke(true);
            else
                OnComplete?.Invoke(false);
        }

        void seletedDay(DateTime d)
        {
            viewModel.selectedDate = d;
            viewModel.UpdateSelectedEvents(d);
        }

        void Handle_SelectionChanged(object sender, Telerik.XamarinForms.Common.ValueChangedEventArgs<object> e)
        {
            this.seletedDay((DateTime)e.NewValue);
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
            LoadingHelper.Show();
            var val = e.Item as CalendarEvent;
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
    }
}
