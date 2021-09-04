﻿using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Helpers;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.ViewModels;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConasiCRM.Portable.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LeadDetailPage : ContentPage
    {
        public static bool? NeedToRefreshLeadDetail = null;
        public Action<bool> OnCompleted;
        private LeadDetailPageViewModel viewModel;
        private Guid Id;
        public LeadDetailPage(Guid id)
        {
            InitializeComponent();
            this.Title = "THÔNG TIN KHÁCH HÀNG";
            this.Id = id;
            this.BindingContext = viewModel = new LeadDetailPageViewModel();
            LoadingHelper.Show();
            NeedToRefreshLeadDetail = false;
            Init();
        }

        public async void Init()
        {
            await LoadDataThongTin(Id.ToString());

            if (viewModel.singleLead.statecode == "1") // qualified
            {
                floatingButtonGroup.IsVisible = false;
            }
            else
            {
                viewModel.ButtonCommandList.Add(new FloatButtonItem("Lead Qualify", "FontAwesomeSolid", "\uf12e", null, LeadQualify));
                viewModel.ButtonCommandList.Add(new FloatButtonItem("Chỉnh sửa", "FontAwesomeRegular", "\uf044", null, Update));
            }

            if (viewModel.singleLead.leadid != Guid.Empty)
                OnCompleted?.Invoke(true);
            else
                OnCompleted?.Invoke(false);
            LoadingHelper.Hide();
        }

        protected async override void OnAppearing()
        {
            if (NeedToRefreshLeadDetail==true)
            {
                await viewModel.LoadOneLead(Id.ToString()) ;
                if (viewModel.singleLead.new_gender != null) { await viewModel.loadOneGender(viewModel.singleLead.new_gender); }
                if (viewModel.singleLead.industrycode != null) { await viewModel.loadOneIndustrycode(viewModel.singleLead.industrycode); }
                NeedToRefreshLeadDetail = false;
            }
            base.OnAppearing();
        }

        private async void Update(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            LeadForm leadForm = new LeadForm(viewModel.singleLead.leadid);
            leadForm.CheckSingleLead = async (IsSuccess) =>
            {
                if (IsSuccess)
                {
                    await Navigation.PushAsync(leadForm);
                    LoadingHelper.Hide();
                }
                else
                {
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage("Đã xảy ra lỗi. Vui lòng thử lại.");
                }
            };
            
        }

        private async void LeadQualify(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            bool IsSuccessQualify = await viewModel.Qualify(viewModel.singleLead.leadid);
            if (IsSuccessQualify == true)
            {
                await viewModel.CreateContact();
                if(!string.IsNullOrWhiteSpace(viewModel.singleLead.companyname))
                {
                    if (viewModel.IsSuccessContact == true && viewModel.IsSuccessAccount == true)
                    {
                        await viewModel.LoadOneLead(Id.ToString());
                        LoadingHelper.Hide();
                        floatingButtonGroup.IsVisible = false;
                        ToastMessageHelper.ShortMessage("Thành công");
                    }
                    else
                    {
                        if (viewModel.IsSuccessContact != true && viewModel.IsSuccessAccount != true)
                        {
                            LoadingHelper.Hide();
                            ToastMessageHelper.ShortMessage("Đã xảy ra lỗi. Vui lòng thử lại.");
                        }
                        else if (viewModel.IsSuccessContact == true)
                        {
                            await viewModel.LoadOneLead(Id.ToString());
                            LoadingHelper.Hide();
                            ToastMessageHelper.ShortMessage("Qualify Contact thành công, Qualify Account thất bại");
                        }
                        else
                        {
                            await viewModel.LoadOneLead(Id.ToString());
                            LoadingHelper.Hide();
                            ToastMessageHelper.ShortMessage("Qualify Account thành công, Qualify Contact thất bại");
                        }
                    }
                }
                else
                {
                    if (viewModel.IsSuccessContact == true)
                    {
                        await viewModel.LoadOneLead(Id.ToString());
                        LoadingHelper.Hide();
                        floatingButtonGroup.IsVisible = false;
                        ToastMessageHelper.ShortMessage("Thành công");
                    }
                    else
                    {
                        LoadingHelper.Hide();
                        ToastMessageHelper.ShortMessage("Đã xảy ra lỗi. Vui lòng thử lại.");
                    }
                }
            }
            else
            {
                LoadingHelper.Hide();
                ToastMessageHelper.ShortMessage("Không thể qualify");
            }

        }
        private async void NhanTin_Tapped(object sender, EventArgs e)
        {
            string phone = viewModel.singleLead.mobilephone; // thêm sdt ở đây
            if (phone != string.Empty)
            {              
                var checkVadate = PhoneNumberFormatVNHelper.CheckValidate(phone);
                if (checkVadate == true)
                {
                    SmsMessage sms = new SmsMessage(null, phone);
                    await Sms.ComposeAsync(sms);                   
                }
                else
                {
                    ToastMessageHelper.ShortMessage("Số điện thoại sai định dạng. Vui lòng kiểm tra lại");
                }
            }
            else
            {
                ToastMessageHelper.ShortMessage("Khách hàng không có số điện thoại. Vui lòng kiểm tra lại");
            }
        }

        private async void GoiDien_Tapped(object sender, EventArgs e)
        {
            string phone = viewModel.singleLead.mobilephone; // thêm sdt ở đây
            if (phone != string.Empty)
            {              
                var checkVadate = PhoneNumberFormatVNHelper.CheckValidate(phone);
                if (checkVadate == true)
                {
                    await Launcher.OpenAsync($"tel:{phone}");                   
                }
                else
                {
                    ToastMessageHelper.ShortMessage("Số điện thoại sai định dạng. Vui lòng kiểm tra lại");
                }
            }
            else
            {
                ToastMessageHelper.ShortMessage("Khách hàng không có số điện thoại. Vui lòng kiểm tra lại");
            }
        }
        // Tab Thong tin
        private async Task LoadDataThongTin(string leadid)
        {
            if (leadid != null && viewModel.singleLead == null)
            {
                await viewModel.LoadOneLead(leadid);
                if (viewModel.singleLead.new_gender != null) { await viewModel.loadOneGender(viewModel.singleLead.new_gender); }
                if (viewModel.singleLead.industrycode != null) { await viewModel.loadOneIndustrycode(viewModel.singleLead.industrycode); }                
            }
        }

        #region TabPhongThuy
        private void LoadDataPhongThuy()
        {
            if (viewModel.PhongThuy == null)
            {
                viewModel.LoadPhongThuy();
            }
        }
        private void ShowImage_Tapped(object sender, EventArgs e)
        {
            LookUpImagePhongThuy.IsVisible = true;
        }

        protected override bool OnBackButtonPressed()
        {
            if (LookUpImagePhongThuy.IsVisible)
            {
                LookUpImagePhongThuy.IsVisible = false;
                return true;
            }
            return base.OnBackButtonPressed();
        }

        private void Close_LookUpImagePhongThuy_Clicked(object sender, EventArgs e)
        {
            LookUpImagePhongThuy.IsVisible = false;
        }

        #endregion
    }
}