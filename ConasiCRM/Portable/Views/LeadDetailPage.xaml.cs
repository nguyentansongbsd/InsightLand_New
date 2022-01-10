using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Helpers;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.Resources;
using ConasiCRM.Portable.ViewModels;
using System;
using System.Collections.Generic;
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
            this.Title = Language.thong_tin_khach_hang_title;
            this.Id = id;
            this.BindingContext = viewModel = new LeadDetailPageViewModel();
            LoadingHelper.Show();
            NeedToRefreshLeadDetail = false;
            Init();
        }

        public async void Init()
        {
            await LoadDataThongTin(Id.ToString());

            SetButtonFloatingButton();

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

        private void SetButtonFloatingButton()
        {
            if (viewModel.singleLead.statuscode == "3") // qualified
            {
                floatingButtonGroup.IsVisible = false;
            }
            else if (viewModel.singleLead.statuscode == "4" || viewModel.singleLead.statuscode == "5" || viewModel.singleLead.statuscode == "6"|| viewModel.singleLead.statuscode == "7")
            {
                viewModel.ButtonCommandList.Add(new FloatButtonItem(Language.kich_hoat_lai_kh, "FontAwesomeSolid", "\uf1b8", null, ReactivateLead));
            }
            else
            {
                viewModel.ButtonCommandList.Add(new FloatButtonItem(Language.chuyen_doi_khach_hang, "FontAwesomeSolid", "\uf542", null, LeadQualify));
                viewModel.ButtonCommandList.Add(new FloatButtonItem(Language.khong_chuyen_doi, "FontAwesomeSolid", "\uf05e", null, LeadDisQualify));
                viewModel.ButtonCommandList.Add(new FloatButtonItem(Language.chinh_sua, "FontAwesomeRegular", "\uf044", null, Update));
            }
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
                    ToastMessageHelper.ShortMessage(Language.da_xay_ra_loi_vui_long_thu_lai);
                }
            };
            
        }

        private async void LeadQualify(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            bool IsSuccessQualify = await viewModel.Qualify(viewModel.singleLead.leadid);
            if (IsSuccessQualify == true)
            {
                if (Dashboard.NeedToRefreshLeads.HasValue) Dashboard.NeedToRefreshLeads = true;
                if (CustomerPage.NeedToRefreshAccount.HasValue) CustomerPage.NeedToRefreshAccount = true;
                if (CustomerPage.NeedToRefreshContact.HasValue) CustomerPage.NeedToRefreshContact = true;
                if (CustomerPage.NeedToRefreshLead.HasValue) CustomerPage.NeedToRefreshLead = true;
                await viewModel.CreateContact();
                if(!string.IsNullOrWhiteSpace(viewModel.singleLead.companyname))
                {
                    if (viewModel.IsSuccessContact == true && viewModel.IsSuccessAccount == true)
                    {
                        await viewModel.LoadOneLead(Id.ToString());
                        LoadingHelper.Hide();
                        floatingButtonGroup.IsVisible = false;
                        ToastMessageHelper.ShortMessage(Language.thanh_cong);
                    }
                    else
                    {
                        if (viewModel.IsSuccessContact != true && viewModel.IsSuccessAccount != true)
                        {
                            LoadingHelper.Hide();
                            ToastMessageHelper.ShortMessage(Language.da_xay_ra_loi_vui_long_thu_lai);
                        }
                        else if (viewModel.IsSuccessContact == true)
                        {
                            await viewModel.LoadOneLead(Id.ToString());
                            LoadingHelper.Hide();
                            ToastMessageHelper.ShortMessage(Language.qualify_contact_thanh_cong_qualify_account_that_bai);
                        }
                        else
                        {
                            await viewModel.LoadOneLead(Id.ToString());
                            LoadingHelper.Hide();
                            ToastMessageHelper.ShortMessage(Language.qualify_account_thanh_cong_qualify_contact_that_bai);
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
                        ToastMessageHelper.ShortMessage(Language.thanh_cong);
                    }
                    else
                    {
                        LoadingHelper.Hide();
                        ToastMessageHelper.ShortMessage(Language.da_xay_ra_loi_vui_long_thu_lai);
                    }
                }
            }
            else
            {
                LoadingHelper.Hide();
                ToastMessageHelper.ShortMessage(Language.khong_the_qualify);
            }

        }

        private async void LeadDisQualify(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            string[] options = new string[] { Language.mat_khach_hang, Language.khong_lien_he_duoc, Language.khong_quan_tam, Language.da_huy };
            
            string aws = await DisplayActionSheet(Language.tuy_chon, Language.huy, null, options);

            if (aws == Language.mat_khach_hang)
            {
                viewModel.LeadStatusCode = 4;
            }
            else if (aws == Language.khong_lien_he_duoc)
            {
                viewModel.LeadStatusCode = 5;
            }
            else if (aws == Language.khong_quan_tam)
            {
                viewModel.LeadStatusCode = 6;
            }
            else if (aws == Language.da_huy)
            {
                viewModel.LeadStatusCode = 7;
            }

            if (viewModel.LeadStatusCode != 0)
            {
                viewModel.LeadStateCode = 2;
                bool isSuccess = await viewModel.UpdateStatusCodeLead();
                if (isSuccess)
                {
                    if (Dashboard.NeedToRefreshLeads.HasValue) Dashboard.NeedToRefreshLeads = true;
                    await viewModel.LoadOneLead(Id.ToString());
                    viewModel.ButtonCommandList.Clear();
                    SetButtonFloatingButton();
                    ToastMessageHelper.ShortMessage(Language.thanh_cong);
                }
                else
                {
                    ToastMessageHelper.ShortMessage(Language.that_bai);
                }
            }
            
            LoadingHelper.Hide();
        }

        private async void ReactivateLead(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            viewModel.LeadStateCode = 0;
            viewModel.LeadStatusCode = 1;
            bool isSuccess = await viewModel.UpdateStatusCodeLead();
            if (isSuccess)
            {
                if (Dashboard.NeedToRefreshLeads.HasValue) Dashboard.NeedToRefreshLeads = true;
                await viewModel.LoadOneLead(Id.ToString());
                viewModel.ButtonCommandList.Clear();
                SetButtonFloatingButton();
                ToastMessageHelper.ShortMessage(Language.thanh_cong);
            }
            else
            {
                ToastMessageHelper.ShortMessage(Language.that_bai);
            }
            LoadingHelper.Hide();
        }

        private async void NhanTin_Tapped(object sender, EventArgs e)
        {
            string phone = viewModel.singleLead.mobilephone.Replace(" ", ""); // thêm sdt ở đây
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
                    ToastMessageHelper.ShortMessage(Language.sdt_sai_dinh_dang_vui_long_kiem_tra_lai);
                }
            }
            else
            {
                ToastMessageHelper.ShortMessage(Language.khach_hang_khong_co_sdt_vui_long_kiem_tra_lai);
            }
        }

        private async void GoiDien_Tapped(object sender, EventArgs e)
        {
            string phone = viewModel.singleLead.mobilephone.Replace(" ",""); // thêm sdt ở đây
            if (phone != string.Empty)
            {              
                var checkVadate = PhoneNumberFormatVNHelper.CheckValidate(phone);
                if (checkVadate == true)
                {
                    await Launcher.OpenAsync($"tel:{phone}");                   
                }
                else
                {
                    ToastMessageHelper.ShortMessage(Language.sdt_sai_dinh_dang_vui_long_kiem_tra_lai);
                }
            }
            else
            {
                ToastMessageHelper.ShortMessage(Language.khach_hang_khong_co_sdt_vui_long_kiem_tra_lai);
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