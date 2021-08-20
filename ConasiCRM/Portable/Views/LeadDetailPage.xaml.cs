using ConasiCRM.Portable.Controls;
using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.XamarinForms.Primitives;
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

            if (viewModel.singleLead.statuscode == "3") // qualified
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
                    await Shell.Current.DisplayAlert("", "Đã xảy ra lỗi. Vui lòng thử lại.", "OK");
                }
            };
            
        }

        private async void LeadQualify(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            bool IsSuccess = await viewModel.Qualify(viewModel.singleLead.leadid);
            if (IsSuccess)
            {
                await viewModel.LoadOneLead(Id.ToString());
                LoadingHelper.Hide();
                await Shell.Current.DisplayAlert("", "Thành công", "OK");
            }
            else
            {
                LoadingHelper.Hide();
                await Shell.Current.DisplayAlert("", "Đã xảy ra lỗi. Vui lòng thử lại.", "OK");
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
                    await Application.Current.MainPage.DisplayAlert("Thông Báo", "Số điện thoại sai định dạng. Vui lòng kiểm tra lại", "OK");
                }
            }
            else
            {                
                await Application.Current.MainPage.DisplayAlert("Thông Báo", "Khách hàng không có số điện thoại. Vui lòng kiểm tra lại", "OK");
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
                    await Application.Current.MainPage.DisplayAlert("Thông Báo", "Số điện thoại sai định dạng. Vui lòng kiểm tra lại", "OK");
                }
            }
            else
            {               
                await Application.Current.MainPage.DisplayAlert("Thông Báo", "Khách hàng không có số điện thoại. Vui lòng kiểm tra lại", "OK");
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