using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Helpers;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.Resources;
using ConasiCRM.Portable.ViewModels;
using Telerik.XamarinForms.Input;
using Xamarin.Forms;

namespace ConasiCRM.Portable.Views
{
    public partial class LichLamViecTheoThang : ContentPage
    {
        LichLamViecViewModel viewModel;
        public Action<bool> OnComplete;
        public static bool? NeedToRefresh = null;
        public LichLamViecTheoThang()
        {
            InitializeComponent();
            this.BindingContext = viewModel = new LichLamViecViewModel();
            NeedToRefresh = false;
            reset();
        }

        public void reset()
        {
            this.viewModel.reset();
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

            await viewModel.loadAllActivities();
            viewModel.selectedDate = DateTime.Today;
            this.seletedDay(viewModel.selectedDate.Value);
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
                viewModel.selectedDateEvents.Clear();
                await viewModel.loadAllActivities();
                this.seletedDay(viewModel.selectedDate.Value);
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
                viewModel.selectedDateEvents.Clear();
                await viewModel.loadAllActivities();
                this.seletedDay(viewModel.selectedDate.Value);
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
                viewModel.selectedDateEvents.Clear();
                await viewModel.loadAllActivities();
                this.seletedDay(viewModel.selectedDate.Value);
            }

            LoadingHelper.Hide();
        }

        private void seletedDay(DateTime d)
        {
            viewModel.selectedDate = d;
            viewModel.UpdateSelectedEvents(d);
        }

        private void Handle_SelectionChanged(object sender, Telerik.XamarinForms.Common.ValueChangedEventArgs<object> e)
        {
            this.seletedDay((DateTime)e.NewValue);
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

        private void Event_Tapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            LoadingHelper.Show();
            var val = e.Item as CalendarEvent;
            if (val != null && val.Activity.activityid != Guid.Empty)
            {
                ActivityPopup.ShowActivityPopup(val.Activity.activityid, val.Activity.activitytypecode);
            }
            LoadingHelper.Hide();
        }
    }
}
