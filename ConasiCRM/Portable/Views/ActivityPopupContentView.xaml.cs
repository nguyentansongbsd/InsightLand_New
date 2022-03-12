﻿using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Helpers;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.Resources;
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
    public partial class ActivityPopupContentView : ContentView
    {
        private ActivityPopupContentViewViewModel viewModel;
        public static bool? NeedToRefreshPhoneCall = null;
        public static bool? NeedToRefreshMeet = null;
        public static bool? NeedToRefreshTask = null;
        private List<Label> meetRequired = new List<Label>();
        private Guid ActivityId;
        private string activitytype;
        public event EventHandler HidePopupActivity;

        public ActivityPopupContentView()
        {
            InitializeComponent();
            BindingContext = viewModel = new ActivityPopupContentViewViewModel();
            this.IsVisible = false;
        }

        private void CloseContentActivity_Tapped(object sender, EventArgs e)
        {
            this.IsVisible = false;
            HidePopupActivity?.Invoke((object)this, EventArgs.Empty);
        }

        public async void ShowActivityPopup(Guid activityid,string activitytypecode)
        {
            if (activityid != Guid.Empty)
            {
                ActivityId = activityid;
                activitytype = activitytypecode;
                LoadingHelper.Show();
                if (activitytypecode == "phonecall")
                {
                    await viewModel.loadPhoneCall(activityid);
                    await viewModel.loadFromTo(activityid);
                    viewModel.ActivityStatusCode = StatusCodeActivity.GetStatusCodeById(viewModel.PhoneCall.statecode.ToString());
                    viewModel.ActivityType = Language.cuoc_goi_title;
                    if (viewModel.PhoneCall != null && viewModel.PhoneCall.activityid != Guid.Empty)
                    {
                        this.IsVisible = true;
                        ContentPhoneCall.IsVisible = true;
                        ContentTask.IsVisible = false;
                        ContentMeet.IsVisible = false;

                        if (viewModel.Task != null)
                            viewModel.Task.activityid = Guid.Empty;
                        if (viewModel.Meet != null)
                            viewModel.Meet.activityid = Guid.Empty;
                        LoadingHelper.Hide();
                    }
                    else
                    {
                        LoadingHelper.Hide();
                        ToastMessageHelper.ShortMessage(Language.khong_tim_thay_thong_tin_vui_long_thu_lai);
                    }
                }
                else if (activitytypecode == "task")
                {
                    await viewModel.loadTask(activityid);
                    viewModel.ActivityStatusCode = StatusCodeActivity.GetStatusCodeById(viewModel.Task.statecode.ToString());
                    viewModel.ActivityType = Language.cong_viec_title;
                    if (viewModel.Task != null && viewModel.Task.activityid != Guid.Empty)
                    {
                        this.IsVisible = true;
                        ContentPhoneCall.IsVisible = false;
                        ContentTask.IsVisible = true;
                        ContentMeet.IsVisible = false;

                        if (viewModel.PhoneCall != null)
                            viewModel.PhoneCall.activityid = Guid.Empty;
                        if (viewModel.Meet != null)
                            viewModel.Meet.activityid = Guid.Empty;
                        LoadingHelper.Hide();
                    }
                    else
                    {
                        LoadingHelper.Hide();
                        ToastMessageHelper.ShortMessage(Language.khong_tim_thay_thong_tin_vui_long_thu_lai);
                    }
                }
                else if (activitytypecode == "appointment")
                {
                    await viewModel.loadMeet(activityid);
                    await viewModel.loadFromToMeet(activityid);
                    viewModel.ActivityStatusCode = StatusCodeActivity.GetStatusCodeById(viewModel.Meet.statecode.ToString());
                    viewModel.ActivityType = Language.cuoc_hop_title;
                    if (viewModel.Meet != null && viewModel.Meet.activityid != Guid.Empty)
                    {
                        this.IsVisible = true;
                        ContentPhoneCall.IsVisible = false;
                        ContentTask.IsVisible = false;
                        ContentMeet.IsVisible = true;
                        SetItem();

                        if (viewModel.Task != null)
                            viewModel.Task.activityid = Guid.Empty;
                        if (viewModel.PhoneCall != null)
                            viewModel.PhoneCall.activityid = Guid.Empty;
                        LoadingHelper.Hide();
                    }
                    else
                    {
                        LoadingHelper.Hide();
                        ToastMessageHelper.ShortMessage(Language.khong_tim_thay_thong_tin_vui_long_thu_lai);
                    }
                }
            }
        }

        private async void Update_Clicked(object sender, EventArgs e)
        {
            if (viewModel.PhoneCall != null && viewModel.PhoneCall.activityid != Guid.Empty)
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
                        ToastMessageHelper.ShortMessage(Language.khong_tim_thay_thong_tin_vui_long_thu_lai);
                    }
                };
            }
            else if (viewModel.Task != null && viewModel.Task.activityid != Guid.Empty)
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
                        ToastMessageHelper.ShortMessage(Language.khong_tim_thay_thong_tin_vui_long_thu_lai);
                    }
                };
            }
            else if (viewModel.Meet != null && viewModel.Meet.activityid != Guid.Empty)
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
                        ToastMessageHelper.ShortMessage(Language.khong_tim_thay_thong_tin_vui_long_thu_lai);
                    }
                };
            }
        }

        private async void Completed_Clicked(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            string[] options = new string[] { Language.hoan_thanh, Language.huy };
            string asw = await App.Current.MainPage.DisplayActionSheet(Language.tuy_chon, Language.dong, null, options);
            if (asw == Language.hoan_thanh)
            {
                if (viewModel.PhoneCall != null && viewModel.PhoneCall.activityid != Guid.Empty)
                {
                    LoadingHelper.Show();
                    if (await viewModel.UpdateStatusPhoneCall(viewModel.CodeCompleted))
                    {
                        viewModel.ActivityStatusCode = StatusCodeActivity.GetStatusCodeById(viewModel.PhoneCall.statecode.ToString());
                        NeedRefresh();
                        LoadingHelper.Hide();
                        ToastMessageHelper.ShortMessage(Language.cuoc_goi_da_hoan_thanh);
                    }
                    else
                    {
                        LoadingHelper.Hide();
                        ToastMessageHelper.ShortMessage(Language.loi_khi_hoan_thanh_cuoc_goi_vui_long_thu_lai);
                    }
                }
                else if (viewModel.Task != null && viewModel.Task.activityid != Guid.Empty)
                {
                    LoadingHelper.Show();
                    if (await viewModel.UpdateStatusTask(viewModel.CodeCompleted))
                    {
                        viewModel.ActivityStatusCode = StatusCodeActivity.GetStatusCodeById(viewModel.Task.statecode.ToString());
                        NeedRefresh();
                        LoadingHelper.Hide();
                        ToastMessageHelper.ShortMessage(Language.cong_viec_da_hoan_thanh);
                    }
                    else
                    {
                        LoadingHelper.Hide();
                        ToastMessageHelper.ShortMessage(Language.loi_khi_hoan_thanh_cong_viec_vui_long_thu_lai);
                    }
                }
                else if (viewModel.Meet != null && viewModel.Meet.activityid != Guid.Empty)
                {
                    LoadingHelper.Show();
                    if (await viewModel.UpdateStatusMeet(viewModel.CodeCompleted))
                    {
                        viewModel.ActivityStatusCode = StatusCodeActivity.GetStatusCodeById(viewModel.Meet.statecode.ToString());
                        NeedRefresh();
                        LoadingHelper.Hide();
                        ToastMessageHelper.ShortMessage(Language.cuoc_hop_da_hoan_thanh);
                    }
                    else
                    {
                        LoadingHelper.Hide();
                        ToastMessageHelper.ShortMessage(Language.loi_khi_hoan_thanh_cuoc_hop_vui_long_thu_lai);
                    }
                }
            }
            else if (asw == Language.huy)
            {
                if (viewModel.PhoneCall != null && viewModel.PhoneCall.activityid != Guid.Empty)
                {
                    LoadingHelper.Show();
                    if (await viewModel.UpdateStatusPhoneCall(viewModel.CodeCancel))
                    {
                        viewModel.ActivityStatusCode = StatusCodeActivity.GetStatusCodeById(viewModel.PhoneCall.statecode.ToString());
                        NeedRefresh();
                        LoadingHelper.Hide();
                        ToastMessageHelper.ShortMessage(Language.cuoc_goi_da_duoc_huy);
                    }
                    else
                    {
                        LoadingHelper.Hide();
                        ToastMessageHelper.ShortMessage(Language.loi_khi_huy_cuoc_goi_vui_long_thu_lai);
                    }
                }
                else if (viewModel.Task != null && viewModel.Task.activityid != Guid.Empty)
                {
                    LoadingHelper.Show();
                    if (await viewModel.UpdateStatusTask(viewModel.CodeCancel))
                    {
                        viewModel.ActivityStatusCode = StatusCodeActivity.GetStatusCodeById(viewModel.Task.statecode.ToString());
                        NeedRefresh();
                        LoadingHelper.Hide();
                        ToastMessageHelper.ShortMessage(Language.cong_viec_da_duoc_huy);
                    }
                    else
                    {
                        LoadingHelper.Hide();
                        ToastMessageHelper.ShortMessage(Language.loi_khi_huy_cong_viec_vui_long_thu_lai);
                    }
                }
                else if (viewModel.Meet != null && viewModel.Meet.activityid != Guid.Empty)
                {
                    LoadingHelper.Show();
                    if (await viewModel.UpdateStatusMeet(viewModel.CodeCancel))
                    {
                        viewModel.ActivityStatusCode = StatusCodeActivity.GetStatusCodeById(viewModel.Meet.statecode.ToString());
                        NeedRefresh();
                        LoadingHelper.Hide();
                        ToastMessageHelper.ShortMessage(Language.cuoc_hop_da_duoc_huy);
                    }
                    else
                    {
                        LoadingHelper.Hide();
                        ToastMessageHelper.ShortMessage(Language.loi_khi_huy_cuoc_hop_vui_long_thu_lai);
                    }
                }
            }
            LoadingHelper.Hide();
        }

        private void PhoneCallTo_Tapped(object sender, EventArgs e)
        {
            if (viewModel.PhoneCall != null)
            {
                if (viewModel.PhoneCall.callto_lead_id != Guid.Empty)
                {
                    LeadDetailPage newPage = new LeadDetailPage(viewModel.PhoneCall.callto_lead_id);
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
                            ToastMessageHelper.ShortMessage(Language.khong_tim_thay_thong_tin_vui_long_thu_lai);
                        }
                    };

                }
                else if (viewModel.PhoneCall.callto_contact_id != Guid.Empty)
                {
                    ContactDetailPage newPage = new ContactDetailPage(viewModel.PhoneCall.callto_contact_id);
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
                            ToastMessageHelper.ShortMessage(Language.khong_tim_thay_thong_tin_vui_long_thu_lai);
                        }
                    };

                }
                else if (viewModel.PhoneCall.callto_account_id != Guid.Empty)
                {
                    AccountDetailPage newPage = new AccountDetailPage(viewModel.PhoneCall.callto_account_id);
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
                            ToastMessageHelper.ShortMessage(Language.khong_tim_thay_thong_tin_vui_long_thu_lai);
                        }
                    };
                }
            }
        }

        private void TaskCustomer_Tapped(object sender, EventArgs e)
        {
            if (viewModel.Task != null)
            {
                if (viewModel.Task.lead_id != Guid.Empty)
                {
                    LeadDetailPage newPage = new LeadDetailPage(viewModel.Task.lead_id);
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
                            ToastMessageHelper.ShortMessage(Language.khong_tim_thay_thong_tin_vui_long_thu_lai);
                        }
                    };

                }
                else if (viewModel.Task.contact_id != Guid.Empty)
                {
                    ContactDetailPage newPage = new ContactDetailPage(viewModel.Task.contact_id);
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
                            ToastMessageHelper.ShortMessage(Language.khong_tim_thay_thong_tin_vui_long_thu_lai);
                        }
                    };

                }
                else if (viewModel.Task.account_id != Guid.Empty)
                {
                    AccountDetailPage newPage = new AccountDetailPage(viewModel.Task.account_id);
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
                            ToastMessageHelper.ShortMessage(Language.khong_tim_thay_thong_tin_vui_long_thu_lai);
                        }
                    };
                }
            }
        }

        private void SetItem()
        {
            if (viewModel.MeetRequired != null && viewModel.MeetRequired.Count > 0)
            {
                flexRequired.Children.Clear();
                BindableLayout.SetItemsSource(flexRequired, viewModel.MeetRequired);
            }
        }
        public void Refresh()
        {
            if (this.IsVisible && ActivityId != Guid.Empty && !string.IsNullOrWhiteSpace(activitytype))
            {
                ShowActivityPopup(ActivityId, activitytype);
            }
        }
        private void Required_Tapped(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            var item = (OptionSet)((sender as Label).GestureRecognizers[0] as TapGestureRecognizer).CommandParameter;
            if (item != null && !string.IsNullOrWhiteSpace(item.Val))
            {
                if (item.Title == viewModel.CodeAccount)
                {
                    AccountDetailPage newPage = new AccountDetailPage(Guid.Parse(item.Val));
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
                            ToastMessageHelper.ShortMessage(Language.khong_tim_thay_thong_tin_vui_long_thu_lai);
                        }
                    };
                }
                else if (item.Title == viewModel.CodeContac)
                {
                    ContactDetailPage newPage = new ContactDetailPage(Guid.Parse(item.Val));
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
                            ToastMessageHelper.ShortMessage(Language.khong_tim_thay_thong_tin_vui_long_thu_lai);
                        }
                    };
                }
                else if (item.Title == viewModel.CodeLead)
                {
                    LeadDetailPage newPage = new LeadDetailPage(Guid.Parse(item.Val));
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
                            ToastMessageHelper.ShortMessage(Language.khong_tim_thay_thong_tin_vui_long_thu_lai);
                        }
                    };
                }
            }
        }
        private void NeedRefresh()
        {
            if (Dashboard.NeedToRefreshActivity.HasValue) Dashboard.NeedToRefreshActivity = true;
            if (ActivityList.NeedToRefreshMeet.HasValue) ActivityList.NeedToRefreshMeet = true;
            if (LichLamViecTheoThang.NeedToRefresh.HasValue) LichLamViecTheoThang.NeedToRefresh = true;
            if (LichLamViecTheoTuan.NeedToRefresh.HasValue) LichLamViecTheoTuan.NeedToRefresh = true;
            if (LichLamViecTheoNgay.NeedToRefresh.HasValue) LichLamViecTheoNgay.NeedToRefresh = true;
            if (ContactDetailPage.NeedToRefreshActivity.HasValue) ContactDetailPage.NeedToRefreshActivity = true;
            if (AccountDetailPage.NeedToRefreshActivity.HasValue) AccountDetailPage.NeedToRefreshActivity = true;
            if (LeadDetailPage.NeedToRefreshActivity.HasValue) LeadDetailPage.NeedToRefreshActivity = true;
        }
    }
}