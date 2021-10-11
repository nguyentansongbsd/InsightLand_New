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
        public static bool? NeedToRefreshPhoneCall = null;
        public static bool? NeedToRefreshMeet = null;
        public static bool? NeedToRefreshTask = null;
        public Action<bool> OnCompleted;
        public ActivityListViewModel viewModel;

        public ActivityList()
        {
            InitializeComponent();
            BindingContext = viewModel = new ActivityListViewModel();
            NeedToRefreshPhoneCall = false;
            NeedToRefreshMeet = false;
            NeedToRefreshTask = false;
            Init();
        }

        public async void Init()
        {
            viewModel.EntityName = "tasks";
            viewModel.entity = "task";
            await viewModel.LoadData();
            if (viewModel.Data.Count > 0)
            {
                VisualStateManager.GoToState(radBorderTask, "Active");
                VisualStateManager.GoToState(radBorderMeeting, "InActive");
                VisualStateManager.GoToState(radBorderPhoneCall, "InActive");
                VisualStateManager.GoToState(lblTask, "Active");
                VisualStateManager.GoToState(lblMeeting, "InActive");
                VisualStateManager.GoToState(lblPhoneCall, "InActive");
                OnCompleted?.Invoke(true);
            }
            else
            {
                OnCompleted?.Invoke(false);
            }
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            if (viewModel.Data != null && NeedToRefreshPhoneCall == true)
            {
                LoadingHelper.Show();
                viewModel.EntityName = "phonecalls";
                viewModel.entity = "phonecall";
                await viewModel.LoadOnRefreshCommandAsync();
                if (viewModel.PhoneCall.activityid != Guid.Empty)
                {
                    await viewModel.loadPhoneCall(viewModel.PhoneCall.activityid);
                    await viewModel.loadFromTo(viewModel.PhoneCall.activityid);
                }
                NeedToRefreshPhoneCall = false;
                LoadingHelper.Hide();
            }
            if (viewModel.Data != null && NeedToRefreshMeet == true)
            {
                LoadingHelper.Show();
                viewModel.EntityName = "appointments";
                viewModel.entity = "appointment";
                await viewModel.LoadOnRefreshCommandAsync();
                if (viewModel.Meet.activityid != Guid.Empty)
                {
                    await viewModel.loadMeet(viewModel.Meet.activityid);
                    await viewModel.loadFromToMeet(viewModel.Meet.activityid);
                }
                NeedToRefreshMeet = false;
                LoadingHelper.Hide();
            }

            if (viewModel.Data != null && NeedToRefreshTask == true)
            {
                LoadingHelper.Show();
                viewModel.EntityName = "tasks";
                viewModel.entity = "task";
                await viewModel.LoadOnRefreshCommandAsync();
                if (ContentTask.IsVisible == true)
                {
                    await viewModel.loadTask(viewModel.Task.activityid);
                }
                
                NeedToRefreshTask = false;
                LoadingHelper.Hide();
            }
        }

        private async void listView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item != null)
            {
                var item = e.Item as HoatDongListModel;
                if (item.activityid != Guid.Empty)
                {
                    LoadingHelper.Show();
                    if (item.activitytypecode == "phonecall")
                    {
                        await viewModel.loadPhoneCall(item.activityid);
                        await viewModel.loadFromTo(item.activityid);
                        viewModel.ActivityStatusCode = StatusCodeActivity.GetStatusCodeById(viewModel.PhoneCall.statecode.ToString());
                        viewModel.ActivityType = "Phone Call";
                        if (viewModel.PhoneCall.activityid != Guid.Empty)
                        {
                            ContentActivity.IsVisible = true;
                            ContentPhoneCall.IsVisible = true;
                            ContentTask.IsVisible = false;
                            ContentMeet.IsVisible = false;

                            viewModel.Task.activityid = Guid.Empty;
                            viewModel.Meet.activityid = Guid.Empty;
                            LoadingHelper.Hide();
                        }
                        else
                        {
                            LoadingHelper.Hide();
                            ToastMessageHelper.ShortMessage("Không tìm thấy thông tin. Vui lòng thử lại");
                        }
                    }
                    else if (item.activitytypecode == "task")
                    {
                        await viewModel.loadTask(item.activityid);
                        viewModel.ActivityStatusCode = StatusCodeActivity.GetStatusCodeById(viewModel.Task.statecode.ToString());
                        viewModel.ActivityType = "Task";
                        if (viewModel.Task.activityid != Guid.Empty)
                        {
                            ContentActivity.IsVisible = true;
                            ContentPhoneCall.IsVisible = false;
                            ContentTask.IsVisible = true;
                            ContentMeet.IsVisible = false;

                            viewModel.PhoneCall.activityid = Guid.Empty;
                            viewModel.Meet.activityid = Guid.Empty;
                            LoadingHelper.Hide();
                        }
                        else
                        {
                            LoadingHelper.Hide();
                            ToastMessageHelper.ShortMessage("Không tìm thấy thông tin. Vui lòng thử lại");
                        }
                    }
                    else if (item.activitytypecode == "appointment")
                    {
                        await viewModel.loadMeet(item.activityid);
                        await viewModel.loadFromToMeet(item.activityid);
                        viewModel.ActivityStatusCode = StatusCodeActivity.GetStatusCodeById(viewModel.Meet.statecode.ToString());
                        viewModel.ActivityType = "Collection Meeting";
                        if (viewModel.Meet.activityid != Guid.Empty)
                        {
                            ContentActivity.IsVisible = true;
                            ContentPhoneCall.IsVisible = false;
                            ContentTask.IsVisible = false;
                            ContentMeet.IsVisible = true;

                            viewModel.Task.activityid = Guid.Empty;
                            viewModel.PhoneCall.activityid = Guid.Empty;
                            LoadingHelper.Hide();
                        }
                        else
                        {
                            LoadingHelper.Hide();
                            ToastMessageHelper.ShortMessage("Không tìm thấy thông tin. Vui lòng thử lại");
                        }
                    }
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
                await Navigation.PushAsync(new MeetingForm());
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

        private void CloseContentActivity_Tapped(object sender, EventArgs e)
        {
            ContentActivity.IsVisible = false;
        }

        private async void Update_Clicked(object sender, EventArgs e)
        {
            if (viewModel.PhoneCall.activityid != Guid.Empty)
            {
                LoadingHelper.Show();
                PhoneCallForm newPage = new PhoneCallForm(viewModel.PhoneCall.activityid);
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
            else if (viewModel.Task.activityid != Guid.Empty)
            {
                LoadingHelper.Show();
                TaskForm newPage = new TaskForm(viewModel.Task.activityid);
                newPage.CheckTaskForm = async (OnCompleted) =>
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
            else if (viewModel.Meet.activityid != Guid.Empty)
            {
                LoadingHelper.Show();
                MeetingForm newPage = new MeetingForm(viewModel.Meet.activityid);
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
        }

        private async void Completed_Clicked(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            string[] options = new string[] { "Hoàn Thành", "Hủy" };
            string asw = await DisplayActionSheet("Tuỳ chọn", "Hủy", null, options);
            if (asw == "Hoàn Thành")
            {
                if (viewModel.PhoneCall.activityid != Guid.Empty)
                {
                    LoadingHelper.Show();
                    if (await viewModel.UpdateStatusPhoneCall(viewModel.CodeCompleted))
                    {
                        viewModel.ActivityStatusCode = StatusCodeActivity.GetStatusCodeById(viewModel.PhoneCall.statecode.ToString());
                        LoadingHelper.Hide();
                        ToastMessageHelper.ShortMessage("Cuộc gọi đã hoàn thành");
                    }
                    else
                    {
                        LoadingHelper.Hide();
                        ToastMessageHelper.ShortMessage("Lỗi khi hoàn thành cuộc gọi. Vui lòng thử lại");
                    }
                }
                else if (viewModel.Task.activityid != Guid.Empty)
                {
                    LoadingHelper.Show();
                    if (await viewModel.UpdateStatusTask(viewModel.CodeCompleted))
                    {
                        viewModel.ActivityStatusCode = StatusCodeActivity.GetStatusCodeById(viewModel.Task.statecode.ToString());
                        LoadingHelper.Hide();
                        ToastMessageHelper.ShortMessage("Công việc đã hoàn thành");
                    }
                    else
                    {
                        LoadingHelper.Hide();
                        ToastMessageHelper.ShortMessage("Lỗi khi hoàn thành công việc. Vui lòng thử lại");
                    }
                }
                else if (viewModel.Meet.activityid != Guid.Empty)
                {
                    LoadingHelper.Show();
                    if (await viewModel.UpdateStatusMeet(viewModel.CodeCompleted))
                    {
                        viewModel.ActivityStatusCode = StatusCodeActivity.GetStatusCodeById(viewModel.Meet.statecode.ToString());
                        LoadingHelper.Hide();
                        ToastMessageHelper.ShortMessage("Cuộc họp đã hoàn thành");
                    }
                    else
                    {
                        LoadingHelper.Hide();
                        ToastMessageHelper.ShortMessage("Lỗi khi hoàn thành cuộc họp. Vui lòng thử lại");
                    }
                }
            }
            else if (asw == "Hủy")
            {
                if (viewModel.PhoneCall.activityid != Guid.Empty)
                {
                    LoadingHelper.Show();
                    if (await viewModel.UpdateStatusPhoneCall(viewModel.CodeCancel))
                    {
                        viewModel.ActivityStatusCode = StatusCodeActivity.GetStatusCodeById(viewModel.PhoneCall.statecode.ToString());
                        LoadingHelper.Hide();
                        ToastMessageHelper.ShortMessage("Cuộc gọi đã được hủy");
                    }
                    else
                    {
                        LoadingHelper.Hide();
                        ToastMessageHelper.ShortMessage("Lỗi khi hủy cuộc gọi. Vui lòng thử lại");
                    }
                }
                else if (viewModel.Task.activityid != Guid.Empty)
                {
                    LoadingHelper.Show();
                    if (await viewModel.UpdateStatusTask(viewModel.CodeCancel))
                    {
                        viewModel.ActivityStatusCode = StatusCodeActivity.GetStatusCodeById(viewModel.Task.statecode.ToString());
                        LoadingHelper.Hide();
                        ToastMessageHelper.ShortMessage("Công việc đã được hủy");
                    }
                    else
                    {
                        LoadingHelper.Hide();
                        ToastMessageHelper.ShortMessage("Lỗi khi hủy công việc. Vui lòng thử lại");
                    }
                }
                else if (viewModel.Meet.activityid != Guid.Empty)
                {
                    LoadingHelper.Show();
                    if (await viewModel.UpdateStatusMeet(viewModel.CodeCancel))
                    {
                        viewModel.ActivityStatusCode = StatusCodeActivity.GetStatusCodeById(viewModel.Meet.statecode.ToString());
                        LoadingHelper.Hide();
                        ToastMessageHelper.ShortMessage("Cuộc họp đã được hủy");
                    }
                    else
                    {
                        LoadingHelper.Hide();
                        ToastMessageHelper.ShortMessage("Lỗi khi hủy cuộc họp. Vui lòng thử lại");
                    }
                }
            }
            LoadingHelper.Hide();
        }

        private async void Task_Tapped(object sender,EventArgs e)
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
                await viewModel.LoadOnRefreshCommandAsync();
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
                await viewModel.LoadOnRefreshCommandAsync();
            }
            
            LoadingHelper.Hide();
        }

        private async void PhoneCall_Tapped(object sender,EventArgs e)
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
                await viewModel.LoadOnRefreshCommandAsync();
            }

            LoadingHelper.Hide();
        }
    }
}