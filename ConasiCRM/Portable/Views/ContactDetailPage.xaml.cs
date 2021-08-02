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
    public partial class ContactDetailPage : ContentPage
    {
        public Action<bool> OnCompleted;
        private ContactDetailPageViewModel viewModel;
        private Guid Id;
        public ContactDetailPage(Guid contactId)
        {
            InitializeComponent();
            this.BindingContext = viewModel = new ContactDetailPageViewModel();
            LoadingHelper.Show();
            Tab_Tapped(1);
            Id = contactId;
            Init();
        }
        public async void Init()
        {
            await LoadDataThongTin(Id.ToString());
            if (viewModel.singleContact != null)
                OnCompleted(true);
            else
                OnCompleted(false);
            LoadingHelper.Hide();
        }

        // tab thong tin
        private async Task LoadDataThongTin(string Id)
        {
            if (Id != null && viewModel.singleContact == null)
            {
                await viewModel.loadOneContact(Id);
                if (viewModel.singleContact.gendercode != null)
                { 
                   viewModel.LoadOneGender(viewModel.singleContact.gendercode); 
                }
                if (viewModel.singleContact.bsd_customergroup != null)
                {
                    viewModel.SingleContactgroup = ContactGroup.GetContactGroupById(viewModel.singleContact.bsd_customergroup);
                }
                if(viewModel.singleContact.bsd_type !=null)
                {
                    viewModel.SingleType = ContactType.GetTypeById(viewModel.singleContact.bsd_type);
                }
            }
        }

        #region Tab giao dich
        private async Task LoadDataGiaoDich(string Id)
        {
            if (viewModel.list_danhsachdatcho.Count == 0)
            {
                viewModel.PageDanhSachDatCho = 1;
                await viewModel.LoadQueuesForContactForm(Id);
            }
            if (viewModel.list_danhsachdatcoc.Count == 0)
            {
                viewModel.PageDanhSachDatCoc = 1;
                await viewModel.LoadReservationForContactForm(Id);
            }
            if (viewModel.list_danhsachhopdong.Count == 0)
            {
                viewModel.PageDanhSachHopDong = 1;
                await viewModel.LoadOptoinEntryForContactForm(Id);
            }
            if (viewModel.list_chamsockhachhang.Count == 0)
            {
                viewModel.PageChamSocKhachHang = 1;
                await viewModel.LoadCaseForContactForm(Id);
            }            
        }
        // danh sach dat cho
        private async void ShowMoreDanhSachDatCho_Clicked(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            viewModel.PageDanhSachDatCho++;
            await viewModel.LoadQueuesForContactForm(viewModel.singleContact.contactid.ToString());
            LoadingHelper.Hide();
        }

        // danh sach dat coc
        private async void ShowMoreDanhSachDatCoc_Clicked(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            viewModel.PageDanhSachDatCoc++;
            await viewModel.LoadReservationForContactForm(viewModel.singleContact.contactid.ToString());
            LoadingHelper.Hide();
        }

        // danh sach hop dong
        private async void ShowMoreDanhSachHopDong_Clicked(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            viewModel.PageDanhSachDatCoc++;
            await viewModel.LoadOptoinEntryForContactForm(viewModel.singleContact.contactid.ToString());
            LoadingHelper.Hide();
        }

        //Cham soc khach hang
        private async void ShowMoreChamSocKhachHang_Clicked(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            viewModel.PageChamSocKhachHang++;
            await viewModel.LoadCaseForContactForm(viewModel.singleContact.contactid.ToString());
            LoadingHelper.Hide();
        }

        #endregion

        #region TabPhongThuy
        private void LoadDataPhongThuy()
        {
            if(viewModel.PhongThuy == null)
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

        private async void NhanTin_Tapped(object sender, EventArgs e)
        {           
            string phone = viewModel.singleContact.mobilephone;
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
            string phone = viewModel.singleContact.mobilephone;
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

        private async void ThongTin_Tapped(object sender, EventArgs e)
        {
            Tab_Tapped(1);
            await LoadDataThongTin(Id.ToString());
        }

        private async void GiaoDich_Tapped(object sender, EventArgs e)
        {
            Tab_Tapped(2);
            await LoadDataGiaoDich(Id.ToString());
        }

        private void PhongThuy_Tapped(object sender, EventArgs e)
        {
            Tab_Tapped(3);
            LoadDataPhongThuy();
        }

        private void Tab_Tapped(int tab)
        {
            if (tab == 1)
            {
                VisualStateManager.GoToState(radBorderThongTin, "Selected");
                VisualStateManager.GoToState(lbThongTin, "Selected");
                TabThongTin.IsVisible = true;
            }
            else
            {
                VisualStateManager.GoToState(radBorderThongTin, "Normal");
                VisualStateManager.GoToState(lbThongTin, "Normal");
                TabThongTin.IsVisible = false;
            }
            if (tab == 2)
            {
                VisualStateManager.GoToState(radBorderGiaoDich, "Selected");
                VisualStateManager.GoToState(lbGiaoDich, "Selected");
                TabGiaoDich.IsVisible = true;
            }
            else
            {
                VisualStateManager.GoToState(radBorderGiaoDich, "Normal");
                VisualStateManager.GoToState(lbGiaoDich, "Normal");
                TabGiaoDich.IsVisible = false;
            }
            if (tab == 3)
            {
                VisualStateManager.GoToState(radBorderPhongThuy, "Selected");
                VisualStateManager.GoToState(lbPhongThuy, "Selected");
                TabPhongThuy.IsVisible = true;
            }
            else
            {
                VisualStateManager.GoToState(radBorderPhongThuy, "Normal");
                VisualStateManager.GoToState(lbPhongThuy, "Normal");
                TabPhongThuy.IsVisible = false;
            }
        }

        private void ThongTinCongTy_Tapped(object sender, EventArgs e)
        {            
            if (viewModel.singleContact._parentcustomerid_value != string.Empty)
            {
                LoadingHelper.Show();
                AccountForm newPage = new AccountForm(Guid.Parse(viewModel.singleContact._parentcustomerid_value));
                newPage.CheckSingleAccount = async (CheckSingleAccount) =>
                {
                    if (CheckSingleAccount == true)
                    {
                        await Navigation.PushAsync(newPage);
                        LoadingHelper.Hide();
                    }
                    else
                    {
                        LoadingHelper.Hide();
                        await DisplayAlert("Thông Báo", "Không tìm thấy thông tin công ty", "Đóng");
                    }
                };
            }
        }
        
    }
}