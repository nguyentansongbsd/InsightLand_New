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
        public static bool? NeedToRefreshActivity = null;
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
            NeedToRefreshActivity = false;
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
            base.OnAppearing();
            if (NeedToRefreshLeadDetail == true)
            {
                await viewModel.LoadOneLead(Id.ToString());
                if (viewModel.singleLead.new_gender != null) { await viewModel.loadOneGender(viewModel.singleLead.new_gender); }
                if (viewModel.singleLead.industrycode != null) { await viewModel.loadOneIndustrycode(viewModel.singleLead.industrycode); }
                NeedToRefreshLeadDetail = false;
            }
            if (NeedToRefreshActivity == true)
            {
                LoadingHelper.Show();
                viewModel.PageCase = 1;
                viewModel.list_case.Clear();
                await viewModel.LoadCase(Id.ToString());
                ActivityPopup.Refresh();
                NeedToRefreshActivity = false;
                LoadingHelper.Hide();
            }
        }
        private void SetButtonFloatingButton()
        {
            if (viewModel.singleLead.statuscode == "3") // qualified
            {
                floatingButtonGroup.IsVisible = false;
                if (viewModel.singleLead.account_id != Guid.Empty)
                {
                    viewModel.ButtonCommandList.Add(new FloatButtonItem(Language.di_den_kh_doanh_nghiep, "FontAwesomeRegular", "\uf1ad", null, GoToAccount));
                    floatingButtonGroup.IsVisible = true;
                }
                if (viewModel.singleLead.contact_id != Guid.Empty)
                {
                    viewModel.ButtonCommandList.Add(new FloatButtonItem(Language.di_den_kh_ca_nhan, "FontAwesomeRegular", "\uf2c1", null, GoToContact));
                    floatingButtonGroup.IsVisible = true;
                }
            }
            else if (viewModel.singleLead.statuscode == "4" || viewModel.singleLead.statuscode == "5" || viewModel.singleLead.statuscode == "6" || viewModel.singleLead.statuscode == "7")
            {
                viewModel.ButtonCommandList.Add(new FloatButtonItem(Language.kich_hoat_lai_kh, "FontAwesomeSolid", "\uf1b8", null, ReactivateLead));
            }
            else
            {
                RadExpanderCase.IsVisible = true;
                viewModel.ButtonCommandList.Add(new FloatButtonItem(Language.them_cuoc_hop, "FontAwesomeRegular", "\uf274", null, NewMeet));
                viewModel.ButtonCommandList.Add(new FloatButtonItem(Language.them_cuoc_goi, "FontAwesomeSolid", "\uf095", null, NewPhoneCall));
                viewModel.ButtonCommandList.Add(new FloatButtonItem(Language.them_cong_viec, "FontAwesomeSolid", "\uf073", null, NewTask));
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
                if (!string.IsNullOrWhiteSpace(viewModel.singleLead.companyname))
                {
                    if (viewModel.IsSuccessContact == true && viewModel.IsSuccessAccount == true)
                    {
                        await viewModel.LoadOneLead(Id.ToString());
                        LoadingHelper.Hide();
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
                        ToastMessageHelper.ShortMessage(Language.thanh_cong);
                    }
                    else
                    {
                        LoadingHelper.Hide();
                        ToastMessageHelper.ShortMessage(Language.da_xay_ra_loi_vui_long_thu_lai);
                    }
                }
                viewModel.ButtonCommandList.Clear();
                SetButtonFloatingButton();
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
            // string phone = viewModel.singleLead.mobilephone.Replace(" ", ""); // thêm sdt ở đây
            if (viewModel.singleLead != null && !string.IsNullOrWhiteSpace(viewModel.singleLead.mobilephone))
            {
                string phone = viewModel.singleLead.mobilephone.Replace(" ", "");
                var checkVadate = PhoneNumberFormatVNHelper.CheckValidate(phone);
                if (checkVadate == true)
                {
                    try
                    {
                        var message = new SmsMessage(null, new[] { phone });
                        await Sms.ComposeAsync(message);
                    }
                    catch (FeatureNotSupportedException ex)
                    {
                        ToastMessageHelper.ShortMessage(Language.sms_khong_duoc_ho_tro_tren_thiet_bi);
                    }
                    catch (Exception ex)
                    {
                        ToastMessageHelper.ShortMessage(Language.da_xay_ra_loi_vui_long_thu_lai);
                    }
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
            if (viewModel.singleLead != null && !string.IsNullOrWhiteSpace(viewModel.singleLead.mobilephone))
            {
                string phone = viewModel.singleLead.mobilephone.Replace(" ", ""); // thêm sdt ở đây
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
                await viewModel.LoadCase(leadid);
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

        private void GoToContact(object sender, EventArgs e)
        {
            if(viewModel.singleLead != null && viewModel.singleLead.contact_id != Guid.Empty)
            {
                LoadingHelper.Show();
                ContactDetailPage newPage = new ContactDetailPage(viewModel.singleLead.contact_id);
                newPage.OnCompleted = async (IsSuccess) =>
                {
                    if (IsSuccess)
                    {
                        await Navigation.PushAsync(newPage);
                        LoadingHelper.Hide();
                    }
                    else
                    {
                        LoadingHelper.Hide();
                        ToastMessageHelper.ShortMessage(Language.da_xay_ra_loi_vui_long_thu_lai);
                    }
                };
            }   
            else
            {
                ToastMessageHelper.ShortMessage(Language.da_xay_ra_loi_vui_long_thu_lai);
            }    
        }
        private void GoToAccount(object sender, EventArgs e)
        {
            if (viewModel.singleLead != null && viewModel.singleLead.account_id != Guid.Empty)
            {
                LoadingHelper.Show();
                AccountDetailPage newPage = new AccountDetailPage(viewModel.singleLead.account_id);
                newPage.OnCompleted = async (IsSuccess) =>
                {
                    if (IsSuccess)
                    {
                        await Navigation.PushAsync(newPage);
                        LoadingHelper.Hide();
                    }
                    else
                    {
                        LoadingHelper.Hide();
                        ToastMessageHelper.ShortMessage(Language.da_xay_ra_loi_vui_long_thu_lai);
                    }
                };
            }
            else
            {
                ToastMessageHelper.ShortMessage(Language.da_xay_ra_loi_vui_long_thu_lai);
            }
        }

        #region Chăm sóc khách hàng
        private async void ShowMoreCase_Clicked(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            viewModel.PageCase++;
            await viewModel.LoadCase(Id.ToString());
            LoadingHelper.Hide();
        }
        private void CaseItem_Tapped(object sender, EventArgs e)
        {
            var item = (HoatDongListModel)((sender as StackLayout).GestureRecognizers[0] as TapGestureRecognizer).CommandParameter;
            if (item != null && item.activityid != Guid.Empty)
            {
                ActivityPopup.ShowActivityPopup(item.activityid, item.activitytypecode);
            }
        }
        private async void NewMeet(object sender, EventArgs e)
        {
            if (viewModel.singleLead != null && viewModel.singleLead.leadid != Guid.Empty)
            {
                LoadingHelper.Show();
                await Navigation.PushAsync(new MeetingForm(viewModel.singleLead.leadid, viewModel.singleLead.lastname, viewModel.CodeLead));
                LoadingHelper.Hide();
            }
        }
        private async void NewPhoneCall(object sender, EventArgs e)
        {
            if (viewModel.singleLead != null && viewModel.singleLead.leadid != Guid.Empty)
            {
                LoadingHelper.Show();
                await Navigation.PushAsync(new PhoneCallForm(viewModel.singleLead.leadid, viewModel.singleLead.lastname, viewModel.CodeLead));
                LoadingHelper.Hide();
            }
        }
        private async void NewTask(object sender, EventArgs e)
        {
            if (viewModel.singleLead != null && viewModel.singleLead.leadid != Guid.Empty)
            {
                LoadingHelper.Show();
                await Navigation.PushAsync(new TaskForm(viewModel.singleLead.leadid, viewModel.singleLead.lastname, viewModel.CodeLead));
                LoadingHelper.Hide();
            }
        }
        #endregion
    }
}