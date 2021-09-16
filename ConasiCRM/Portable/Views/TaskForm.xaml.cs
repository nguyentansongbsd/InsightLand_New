using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Helpers;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConasiCRM.Portable.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TaskForm : ContentPage
    {
        public Action<bool> CheckTaskForm;
        public TaskFormViewModel viewModel;

        public TaskForm()
        {
            InitializeComponent();
            Init();
            InitAdd();
        }

        public TaskForm(Guid taskId)
        {
            InitializeComponent();
            Init();
            viewModel.TaskId = taskId;
            InitUpdate();
        }

        public TaskForm(DateTime dateTimeNew)
        {
            InitializeComponent();

        }

        public async void Init()
        {
            this.BindingContext = viewModel = new TaskFormViewModel();
        }

        public async void InitAdd()
        {
            viewModel.Title = "Tạo Công Việc";
            viewModel.TaskFormModel = new TaskFormModel();
            dateTimeTGBatDau.DefaultDisplay = DateTime.Now;
            dateTimeTGKetThuc.DefaultDisplay = DateTime.Now;

            SetPreOpen();
        }

        public async void InitUpdate()
        {
            await viewModel.LoadTask();
            if (viewModel.TaskFormModel != null)
            {
                viewModel.Title = "Cập Nhật Công Việc";
                btnSave.Text = "Cập nhật công việc";

                SetPreOpen();

                CheckTaskForm?.Invoke(true);
            }
            else
            {
                CheckTaskForm?.Invoke(false);
            }
            
        }

        private void SetPreOpen()
        {
            Lookup_NguoiLienQuan.PreOpenAsync = async () =>
            {
                LoadingHelper.Show();
                viewModel.SetUpTabs();
                viewModel.LoadAllLookUp();
                LoadingHelper.Hide();
            };
        }

        private void DateStart_Selected(object sender, EventArgs e)
        {
            if (viewModel.ScheduledStart.HasValue && viewModel.ScheduledEnd.HasValue)
            {
                if (viewModel.ScheduledStart > viewModel.ScheduledEnd || viewModel.ScheduledStart == viewModel.ScheduledEnd)
                {
                    ToastMessageHelper.ShortMessage("Thời gian bắt đầu phải nhỏ hơn thời gian kết thúc");
                }
            }
        }

        private void DateEnd_Selected(object sender, EventArgs e)
        {
            if (viewModel.ScheduledStart.HasValue && viewModel.ScheduledEnd.HasValue)
            {
                if (viewModel.ScheduledStart > viewModel.ScheduledEnd || viewModel.ScheduledStart == viewModel.ScheduledEnd)
                {
                    ToastMessageHelper.ShortMessage("Thời gian kết thúc phải lớn hơn thời gian bắt đầu");
                }
            }
        }

        private void EventAllDay_Tapped(object sender, EventArgs e)
        {
            viewModel.IsEventAllDay = !viewModel.IsEventAllDay;
        }

        private void CheckedBoxEventAllDay_Change(object sender, EventArgs e)
        {
            if (!viewModel.ScheduledStart.HasValue)
            {
                ToastMessageHelper.ShortMessage("Vui lòng chọn thời gian bắt đầu");
                viewModel.IsEventAllDay = false;
                return;
            }
            if (viewModel.IsEventAllDay == true)
            {
                viewModel.ScheduledStart = new DateTime(viewModel.ScheduledStart.Value.Year, viewModel.ScheduledStart.Value.Month, viewModel.ScheduledStart.Value.Day, 8, 0, 0); ;
                viewModel.ScheduledEnd = viewModel.ScheduledStart.Value.AddDays(1);
            }
        }

        private async void SaveTask_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(viewModel.TaskFormModel.subject))
            {
                ToastMessageHelper.ShortMessage("Vui lòng nhập chủ đề");
                return;
            }

            if (!viewModel.ScheduledStart.HasValue)
            {
                ToastMessageHelper.ShortMessage("Vui lòng chọn thời gian bắt đầu");
                return;
            }

            if (!viewModel.ScheduledEnd.HasValue)
            {
                ToastMessageHelper.ShortMessage("Vui lòng chọn thời gian kết thúc");
                return;
            }

            if ((viewModel.ScheduledStart.HasValue && viewModel.ScheduledEnd.HasValue) && (viewModel.ScheduledStart > viewModel.ScheduledEnd))
            {
                ToastMessageHelper.ShortMessage("Thời gian kết thúc phải lớn hơn thời gian bắt đầu");
                return;
            }

            LoadingHelper.Show();
            if (viewModel.TaskFormModel.activityid == Guid.Empty)
            {
                bool isSuccess = await viewModel.CreateTask();
                if (isSuccess)
                {
                    if (ActivityList.NeedToRefreshTask.HasValue) ActivityList.NeedToRefreshTask = true;
                    ToastMessageHelper.ShortMessage("Tạo công việc thành công");
                    await Navigation.PopAsync();
                    LoadingHelper.Hide();
                }
                else
                {
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage("Tạo công việc thất bại");
                }
            }
            else
            {
                bool isSuccess = await viewModel.UpdateTask();
                if (isSuccess)
                {
                    if (ActivityList.NeedToRefreshTask.HasValue) ActivityList.NeedToRefreshTask = true;
                    ToastMessageHelper.ShortMessage("Cập nhật công việc thành công");
                    await Navigation.PopAsync();
                    LoadingHelper.Hide();
                }
                else
                {
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage("Cập nhật công việc thất bại");
                }
            }
        }
    }
}