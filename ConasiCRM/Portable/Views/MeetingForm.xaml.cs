using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Helpers;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.Resources;
using ConasiCRM.Portable.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConasiCRM.Portable.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MeetingForm : ContentPage
    {
        public Action<bool> OnCompleted;
        public MeetingViewModel viewModel;
        private Guid MeetId;
        private bool IsInit;
        public MeetingForm()
        {
            InitializeComponent();
            Init();
            Create();
        }

        public MeetingForm(Guid id)
        {
            InitializeComponent();
            Init();
            MeetId = id;
            Update();
        }

        public MeetingForm(Guid idCustomer, string nameCustomer, string codeCustomer)
        {
            InitializeComponent();
            Init();
            Create();
            viewModel.CustomerMapping = new OptionSet { Val = idCustomer.ToString(), Label = nameCustomer, Title = codeCustomer };
            Lookup_Required.IsVisible = false;
            CustomerMapping.IsVisible = true;
        }

        private void Init()
        {
            LoadingHelper.Show();
            BindingContext = viewModel = new MeetingViewModel();
            DatePickerStart.DefaultDisplay = DateTime.Now;
            DatePickerEnd.DefaultDisplay = DateTime.Now;
        }

        private void Create()
        {
            this.Title = Language.tao_moi_cuoc_hop_title;
            BtnSave.Text = Language.tao_cuoc_hop;
            IsInit = true;
            BtnSave.Clicked += Create_Clicked;
        }

        private void Create_Clicked(object sender, EventArgs e)
        {
            SaveData(Guid.Empty);
        }

        private async void Update()
        {
            this.Title = Language.cap_nhat_cuoc_hop_title;
            BtnSave.Text = Language.cap_nhat_cuoc_hop;
            BtnSave.Clicked += Update_Clicked;
            await viewModel.loadDataMeet(this.MeetId);
            await viewModel.LoadAllLookUp();
            var _data = await viewModel.loadDataParty(this.MeetId);
            if (_data.Any())
            {
                List<OptionSetFilter> requiredIds = new List<OptionSetFilter>();
                List<OptionSetFilter> optionalIds = new List<OptionSetFilter>();
                foreach (var item in _data)
                {
                    if (item.typemask == 5)
                    {
                        requiredIds.Add(new OptionSetFilter { Val = item.partyID.ToString(), Label = item.cutomer_name, Title = item.title_code, Selected = true });
                    }
                    else if (item.typemask == 6)
                    {
                        optionalIds.Add(new OptionSetFilter { Val = item.partyID.ToString(), Label = item.cutomer_name, Title = item.title_code, Selected = true });
                    }
                }
                viewModel.Required = requiredIds;
                viewModel.Optional = optionalIds;
            }

            if (viewModel.MeetingModel.activityid != Guid.Empty)
            {
                OnCompleted?.Invoke(true);
                IsInit = true;
            }
            else
                OnCompleted?.Invoke(false);
        }

        private void Update_Clicked(object sender, EventArgs e)
        {
            SaveData(this.MeetId);
        }
        private async void SaveData(Guid id)
        {
            if (string.IsNullOrWhiteSpace(viewModel.MeetingModel.subject))
            {
                ToastMessageHelper.ShortMessage(Language.vui_long_nhap_chu_de);
                return;
            }
            if (viewModel.CustomerMapping == null)
            {
                if (viewModel.Required == null || viewModel.Required.Count <= 0)
                {
                    ToastMessageHelper.ShortMessage(Language.vui_long_chon_nguoi_tham_du_bat_buoc);
                    return;
                }
            }
            if (viewModel.MeetingModel.scheduledstart == null)
            {
                ToastMessageHelper.ShortMessage(Language.vui_long_chon_thoi_gian_bat_dau);
                return;
            }
            if (viewModel.MeetingModel.scheduledend == null)
            {
                ToastMessageHelper.ShortMessage(Language.vui_long_chon_thoi_gian_ket_thuc);
                return;
            }
            if (viewModel.MeetingModel.scheduledstart != null && viewModel.MeetingModel.scheduledend != null)
            {
                if (this.compareDateTime(viewModel.MeetingModel.scheduledstart, viewModel.MeetingModel.scheduledend) != -1)
                {
                    ToastMessageHelper.ShortMessage(Language.thoi_gian_ket_thuc_phai_lon_hon_thoi_gian_bat_dau);
                    return;
                }
            }

            LoadingHelper.Show();

            if (id == Guid.Empty)
            {
                if (await viewModel.createMeeting())
                {
                    if (Dashboard.NeedToRefreshActivity.HasValue) Dashboard.NeedToRefreshActivity = true;
                    if (ActivityList.NeedToRefreshMeet.HasValue) ActivityList.NeedToRefreshMeet = true;
                    if (LichLamViecTheoThang.NeedToRefresh.HasValue) LichLamViecTheoThang.NeedToRefresh = true;
                    if (LichLamViecTheoTuan.NeedToRefresh.HasValue) LichLamViecTheoTuan.NeedToRefresh = true;
                    if (LichLamViecTheoNgay.NeedToRefresh.HasValue) LichLamViecTheoNgay.NeedToRefresh = true;
                    if (ContactDetailPage.NeedToRefreshActivity.HasValue) ContactDetailPage.NeedToRefreshActivity = true;
                    if (AccountDetailPage.NeedToRefreshActivity.HasValue) AccountDetailPage.NeedToRefreshActivity = true;
                    if (LeadDetailPage.NeedToRefreshActivity.HasValue) LeadDetailPage.NeedToRefreshActivity = true;
                    ToastMessageHelper.ShortMessage(Language.tao_cuoc_hop_thanh_cong);
                    await Navigation.PopAsync();
                    LoadingHelper.Hide();
                }
                else
                {
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage(Language.tao_cuoc_hop_that_bai);
                }
            }
            else
            {
                if (await viewModel.UpdateMeeting(id))
                {
                    if (Dashboard.NeedToRefreshActivity.HasValue) Dashboard.NeedToRefreshActivity = true;
                    if (ActivityList.NeedToRefreshMeet.HasValue) ActivityList.NeedToRefreshMeet = true;
                    if (LichLamViecTheoThang.NeedToRefresh.HasValue) LichLamViecTheoThang.NeedToRefresh = true;
                    if (LichLamViecTheoTuan.NeedToRefresh.HasValue) LichLamViecTheoTuan.NeedToRefresh = true;
                    if (LichLamViecTheoNgay.NeedToRefresh.HasValue) LichLamViecTheoNgay.NeedToRefresh = true;
                    if (ContactDetailPage.NeedToRefreshActivity.HasValue) ContactDetailPage.NeedToRefreshActivity = true;
                    if (AccountDetailPage.NeedToRefreshActivity.HasValue) AccountDetailPage.NeedToRefreshActivity = true;
                    if (LeadDetailPage.NeedToRefreshActivity.HasValue) LeadDetailPage.NeedToRefreshActivity = true;
                    ToastMessageHelper.ShortMessage(Language.cap_nhat_cuoc_hop_thanh_cong);
                    await Navigation.PopAsync();
                    LoadingHelper.Hide();
                }
                else
                {
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage(Language.cap_nhat_cuoc_hop_that_bai);
                }
            }
        }

        private int compareDateTime(DateTime? date, DateTime? date1)
        {
            if (date != null && date1 != null)
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

        private void DatePickerStart_DateSelected(object sender, EventArgs e)
        {
            if (IsInit)
            {
                if (viewModel.MeetingModel.scheduledend != null)
                {
                    if (this.compareDateTime(viewModel.MeetingModel.scheduledstart, viewModel.MeetingModel.scheduledend) != -1)
                    {
                        ToastMessageHelper.ShortMessage(Language.thoi_gian_ket_thuc_phai_lon_hon_thoi_gian_bat_dau);
                        viewModel.MeetingModel.scheduledstart = viewModel.MeetingModel.scheduledend;
                    }
                }
            }
        }

        private void DatePickerEnd_DateSelected(object sender, EventArgs e)
        {
            if (IsInit)
            {
                if (viewModel.MeetingModel.scheduledstart != null)
                {
                    if (this.compareDateTime(viewModel.MeetingModel.scheduledstart, viewModel.MeetingModel.scheduledend) != -1)
                    {
                        ToastMessageHelper.ShortMessage(Language.thoi_gian_ket_thuc_phai_lon_hon_thoi_gian_bat_dau);
                        viewModel.MeetingModel.scheduledend = viewModel.MeetingModel.scheduledstart;
                    }
                }
                else
                {
                    ToastMessageHelper.ShortMessage(Language.vui_long_chon_thoi_gian_bat_dau);
                }
            }
        }

        private void AllDayEvent_changeChecked(object sender, EventArgs e)
        {
            if (viewModel.MeetingModel.scheduledstart != null)
            {
                if (viewModel.MeetingModel.isalldayevent)
                {
                    var timeStart = viewModel.MeetingModel.scheduledstart.Value;
                    viewModel.MeetingModel.timeStart = new TimeSpan(timeStart.Hour, timeStart.Minute, timeStart.Second);
                    if (viewModel.MeetingModel.scheduledend != null)
                    {
                        var actualdurationminutes = Math.Round((viewModel.MeetingModel.scheduledend.Value - viewModel.MeetingModel.scheduledstart.Value).TotalMinutes);
                        viewModel.MeetingModel.scheduleddurationminutes = int.Parse(actualdurationminutes.ToString());
                    }
                    else
                    {
                        viewModel.MeetingModel.scheduleddurationminutes = 0;
                    }    

                    viewModel.MeetingModel.scheduledstart = new DateTime(timeStart.Year, timeStart.Month, timeStart.Day, 0, 0, 0);
                    viewModel.MeetingModel.scheduledend = viewModel.MeetingModel.scheduledstart.Value.AddDays(1);
                }
                else
                {
                    var dateStart = viewModel.MeetingModel.scheduledstart.Value;
                    TimeSpan timeStart = viewModel.MeetingModel.timeStart;

                    viewModel.MeetingModel.scheduledstart = new DateTime(dateStart.Year, dateStart.Month, dateStart.Day, timeStart.Hours, timeStart.Minutes, timeStart.Seconds);
                    viewModel.MeetingModel.scheduledend = viewModel.MeetingModel.scheduledstart.Value.AddMinutes(viewModel.MeetingModel.scheduleddurationminutes);
                }
            }
            else
            {
                viewModel.MeetingModel.isalldayevent = false;
                ToastMessageHelper.ShortMessage(Language.vui_long_chon_thoi_gian_bat_dau);
            }    
        }
    }
}