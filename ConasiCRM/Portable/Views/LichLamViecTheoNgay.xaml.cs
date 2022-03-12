using System;
using System.Collections.Generic;
using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Helpers;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.Resources;
using ConasiCRM.Portable.ViewModels;
using Xamarin.Forms;

namespace ConasiCRM.Portable.Views
{
    public partial class LichLamViecTheoNgay : ContentPage
    {
        LichLamViecViewModel viewModel;
        public Action<bool> OnComplete;
        public static bool? NeedToRefresh = null;
        public LichLamViecTheoNgay()
        {
            InitializeComponent();
            this.BindingContext = viewModel = new LichLamViecViewModel();
            NeedToRefresh = false;
            this.loadData();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            if (NeedToRefresh == true)
            {
                LoadingHelper.Show();
                await viewModel.loadAllActivities();
                NeedToRefresh = false;
                LoadingHelper.Hide();
            }
        }

        public async void loadData()
        {
            viewModel.EntityName = "tasks";
            viewModel.entity = "task";
            VisualStateManager.GoToState(radBorderTask, "Active");
            VisualStateManager.GoToState(radBorderMeeting, "InActive");
            VisualStateManager.GoToState(radBorderPhoneCall, "InActive");
            VisualStateManager.GoToState(lblTask, "Active");
            VisualStateManager.GoToState(lblMeeting, "InActive");
            VisualStateManager.GoToState(lblPhoneCall, "InActive");

            viewModel.selectedDate = DateTime.Today;
            await viewModel.loadAllActivities();
            if (viewModel.lstEvents != null && viewModel.lstEvents.Count > 0)
                OnComplete?.Invoke(true);
            else
                OnComplete?.Invoke(false);
        }

        private async void Task_Tapped(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            VisualStateManager.GoToState(radBorderTask, "Active");
            VisualStateManager.GoToState(radBorderMeeting, "InActive");
            VisualStateManager.GoToState(radBorderPhoneCall, "InActive");
            VisualStateManager.GoToState(lblTask, "Active");
            VisualStateManager.GoToState(lblMeeting, "InActive");
            VisualStateManager.GoToState(lblPhoneCall, "InActive");

            if (viewModel.entity != "task")
            {
                viewModel.EntityName = "tasks";
                viewModel.entity = "task";
                viewModel.lstEvents.Clear();
                await viewModel.loadAllActivities();
            }

            LoadingHelper.Hide();
        }

        private async void Meeting_Tapped(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            VisualStateManager.GoToState(radBorderTask, "InActive");
            VisualStateManager.GoToState(radBorderMeeting, "Active");
            VisualStateManager.GoToState(radBorderPhoneCall, "InActive");
            VisualStateManager.GoToState(lblTask, "InActive");
            VisualStateManager.GoToState(lblMeeting, "Active");
            VisualStateManager.GoToState(lblPhoneCall, "InActive");

            if (viewModel.entity != "appointment")
            {
                viewModel.EntityName = "appointments";
                viewModel.entity = "appointment";
                viewModel.lstEvents.Clear();
                await viewModel.loadAllActivities();
            }

            LoadingHelper.Hide();
        }

        private async void PhoneCall_Tapped(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            VisualStateManager.GoToState(radBorderTask, "InActive");
            VisualStateManager.GoToState(radBorderMeeting, "InActive");
            VisualStateManager.GoToState(radBorderPhoneCall, "Active");
            VisualStateManager.GoToState(lblTask, "InActive");
            VisualStateManager.GoToState(lblMeeting, "InActive");
            VisualStateManager.GoToState(lblPhoneCall, "Active");

            if (viewModel.entity != "phonecall")
            {
                viewModel.EntityName = "phonecalls";
                viewModel.entity = "phonecall";
                viewModel.lstEvents.Clear();
                await viewModel.loadAllActivities();
            }

            LoadingHelper.Hide();
        }

        private void Handle_AppointmentTapped(object sender, Telerik.XamarinForms.Input.AppointmentTappedEventArgs e)
        {
            LoadingHelper.Show();
            var val = e.Appointment as CalendarEvent;
            if (val != null && val.Activity.activityid != Guid.Empty)
            {
                ActivityPopup.ShowActivityPopup(val.Activity.activityid, val.Activity.activitytypecode);
            }
            LoadingHelper.Hide();
        }

        private async void AddButton_Clicked(object sender, System.EventArgs e)
        {
            LoadingHelper.Show();
            string[] options = new string[] { Language.them_cong_viec, Language.them_cuoc_hop, Language.them_cuoc_goi };
            string asw = await DisplayActionSheet(Language.tuy_chon, Language.huy, null, options);
            if (asw == Language.them_cong_viec)
            {
                await Navigation.PushAsync(new TaskForm());
            }
            else if (asw == Language.them_cuoc_goi)
            {
                await Navigation.PushAsync(new PhoneCallForm());
            }
            else if (asw == Language.them_cuoc_hop)
            {
                await Navigation.PushAsync(new MeetingForm());
            }
            LoadingHelper.Hide();
        }

        void Handle_TimeSlotTapped(object sender, Telerik.XamarinForms.Input.TimeSlotTapEventArgs e)
        {
            viewModel.selectedDate = e.StartTime;
        }

        void Handle_DisplayDateChanged(object sender, Telerik.XamarinForms.Common.ValueChangedEventArgs<object> e)
        {
            viewModel.selectedDate = (DateTime)e.NewValue;
        }
    }
}
