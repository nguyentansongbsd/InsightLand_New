using ConasiCRM.Portable.Controls;
using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Helpers;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Telerik.XamarinForms.Primitives;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConasiCRM.Portable.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PhoneCallForm : ContentPage
	{
        public Action<bool> OnCompleted;
        public PhoneCallViewModel viewModel;

        public PhoneCallForm()
        {
            InitializeComponent();            
            Init();
            Create();
        }

        private void Init()
        {
            LoadingHelper.Show();
            BindingContext = viewModel = new PhoneCallViewModel();         
            SetPreOpen();
        }

        private void Create()
        {
            viewModel.Title = "Tạo mới cuộc gọi";
        }

        public void SetPreOpen()
        {
            Lookup_CallFrom.PreOpenAsync = async () =>
            {
                LoadingHelper.Show();
                await viewModel.LoadAllLookUp();
                LoadingHelper.Hide();
            };

            Lookup_CallTo.PreShow = async () =>
            {
                LoadingHelper.Show();
                await viewModel.LoadAllLookUp();
                LoadingHelper.Hide();
            };

            Lookup_Customer.PreOpenAsync = async () =>
            {
                LoadingHelper.Show();
                await viewModel.LoadAllLookUp();
                LoadingHelper.Hide();
            };
        }

        private void CreatePhoneCall_Clicked(object sender, EventArgs e)
        {
            SaveData();
        }

        private async void SaveData()
        {
            if (string.IsNullOrWhiteSpace(viewModel.PhoneCellModel.subject))
            {
                ToastMessageHelper.ShortMessage("Vui lòng nhập chủ đề cuộc gọi");
                return;
            }
            if (viewModel.CallFrom == null)
            {
                ToastMessageHelper.ShortMessage("Vui lòng chọn người thực hiện cuộc gọi");
                return;
            }
            if (viewModel.CallTo == null)
            {
                ToastMessageHelper.ShortMessage("Vui lòng chọn người nhận cuộc gọi");
                return;
            }
            if (string.IsNullOrWhiteSpace(viewModel.PhoneCellModel.phonenumber))
            {
                ToastMessageHelper.ShortMessage("Vui lòng nhập số điện thoại");
                return;
            }
            if (viewModel.PhoneCellModel.scheduledstart == null || viewModel.PhoneCellModel.scheduledend == null)
            {
                ToastMessageHelper.ShortMessage("Vui lòng chọn thời gian kết thúc và thời gian bắt đầu");
                    return;
            }
            if (viewModel.PhoneCellModel.scheduledstart != null && viewModel.PhoneCellModel.scheduledend != null)
            {
                if (this.compareDateTime(viewModel.PhoneCellModel.scheduledstart, viewModel.PhoneCellModel.scheduledend) != -1)
                {
                    ToastMessageHelper.ShortMessage("Vui lòng chọn thời gian kết thúc lớn hơn thời gian bắt đầu");
                    return;
                }
            }
            viewModel.ConvertStringToOptionSet();
            if (await viewModel.createPhoneCall())
            {
                ToastMessageHelper.ShortMessage("Đã thêm cuộc gọi");
            }
            else
            {
                ToastMessageHelper.ShortMessage("Thêm cuộc gọi thất bại");
            }
        }

        private void DateEnd_DateSelected(object sender, DateChangedEventArgs e)
        {
            DateTime timeNew = e.NewDate;
            TimeSpan time = viewModel.PhoneCellModel.timeEnd;
            DateTime _scheduledend = new DateTime(timeNew.Year, timeNew.Month, timeNew.Day, time.Hours, time.Minutes, time.Seconds);

            if (viewModel.PhoneCellModel.scheduledstart != null )
            {
                if (this.compareDateTime(viewModel.PhoneCellModel.scheduledstart, _scheduledend) == -1)
                {
                    viewModel.PhoneCellModel.scheduledend = _scheduledend;
                }
                else
                {
                    ToastMessageHelper.ShortMessage("Vui lòng chọn thời gian kết thúc lớn hơn thời gian bắt đầu");
                    viewModel.PhoneCellModel.scheduledend = viewModel.PhoneCellModel.scheduledstart;               
                }
            }
            else
            {
                ToastMessageHelper.ShortMessage("Vui lòng chọn thời gian bắt đầu");
            }
        }

        private void DateStart_DateSelected(object sender, DateChangedEventArgs e)
        {
            DateTime timeNew = e.NewDate;
            TimeSpan time = viewModel.PhoneCellModel.timeStart;
            var scheduledstart = new DateTime(timeNew.Year, timeNew.Month, timeNew.Day, time.Hours, time.Minutes, time.Seconds);
            if (viewModel.PhoneCellModel.scheduledend != null)
            {
                if (this.compareDateTime(scheduledstart, viewModel.PhoneCellModel.scheduledend) == -1)
                {
                    viewModel.PhoneCellModel.scheduledstart = scheduledstart;
                }
                else
                {
                    ToastMessageHelper.ShortMessage("Vui lòng chọn thời gian kết thúc lớn hơn thời gian bắt đầu");
                    viewModel.PhoneCellModel.scheduledstart = viewModel.PhoneCellModel.scheduledend;
                }
            }
            else
            {
                viewModel.PhoneCellModel.scheduledstart = scheduledstart;
            }
        }

        private int compareDateTime(DateTime? date, DateTime? date1)
        {
            if (date != null && date1 != null )
            {
                int result = DateTime.Compare(date.Value, date1.Value);
                if (result < 0)
                    return -1;
                else if (result == 0)
                    return 0;
                else
                    return 1;
            }
            if (date == null && date1 != null)
                return -1;
            if (date1 == null && date != null)
                return 1;
            return 0;
        }     

        private void TimeEnd_Unfocused(object sender, FocusEventArgs e)
        {
            if (viewModel.PhoneCellModel.scheduledend != null && viewModel.PhoneCellModel.scheduledstart != null)
            {
                var timeNew = timePickerEnd.Time;
                DateTime time = viewModel.PhoneCellModel.scheduledend.Value;
                DateTime _scheduledend = new DateTime(time.Year, time.Month, time.Day, timeNew.Hours, timeNew.Minutes, timeNew.Seconds);
                if (this.compareDateTime(viewModel.PhoneCellModel.scheduledstart.Value, _scheduledend) == -1)
                {
                    viewModel.PhoneCellModel.timeEnd = timeNew;
                    viewModel.PhoneCellModel.scheduledend = _scheduledend;
                }
                else
                {
                    ToastMessageHelper.ShortMessage("Vui lòng chọn thời gian kết thúc lớn hơn thời gian bắt đầu");
                    viewModel.PhoneCellModel.timeEnd = viewModel.PhoneCellModel.timeStart;
                }
            }
            else
            {
                ToastMessageHelper.ShortMessage("Vui lòng chọn ngày bắt đầu và ngày kết thúc");
            }
        }

        private void TimeStart_Unfocused(object sender, FocusEventArgs e)
        {
            if (viewModel.PhoneCellModel.scheduledend != null && viewModel.PhoneCellModel.scheduledstart != null)
            {
                var timeNew = timePickerStart.Time;
                DateTime time = viewModel.PhoneCellModel.scheduledstart.Value;
                DateTime scheduledstart = new DateTime(time.Year, time.Month, time.Day, timeNew.Hours, timeNew.Minutes, timeNew.Seconds);

                if (this.compareDateTime(scheduledstart, viewModel.PhoneCellModel.scheduledend.Value) == -1)
                {
                    viewModel.PhoneCellModel.timeStart = timeNew;
                    viewModel.PhoneCellModel.scheduledstart = scheduledstart;
                }
                else
                {
                    ToastMessageHelper.ShortMessage("Vui lòng chọn thời gian kết thúc lớn hơn thời gian bắt đầu");
                    viewModel.PhoneCellModel.timeStart = viewModel.PhoneCellModel.timeEnd;
                }
            }
            else
            {
                ToastMessageHelper.ShortMessage("Vui lòng chọn thời gian bắt đầu và thời gian kết thúc");
            }
        }
    }
}