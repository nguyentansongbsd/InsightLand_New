using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Helpers;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.Settings;
using ConasiCRM.Portable.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConasiCRM.Portable.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ContactDetailPage : ContentPage
    {
        public Action<bool> OnCompleted;
        public static bool? NeedToRefresh = null;
        private ContactDetailPageViewModel viewModel;
        private Guid Id;
        public ContactDetailPage(Guid contactId)
        {
            InitializeComponent();
            this.BindingContext = viewModel = new ContactDetailPageViewModel();
            LoadingHelper.Show();
            NeedToRefresh = false;
            Tab_Tapped(1);
            Id = contactId;
            Init();
        }
        public async void Init()
        {
            await LoadDataThongTin(Id.ToString());

            if (viewModel.singleContact.employee_id != UserLogged.Id)
            {
                frameEdit.IsVisible = false;
            }

            if (viewModel.singleContact.contactid != Guid.Empty)
                OnCompleted(true);
            else
                OnCompleted(false);
            LoadingHelper.Hide();
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (NeedToRefresh == true)
            {
                LoadingHelper.Show();
                viewModel.singleContact = new ContactFormModel();
                await LoadDataThongTin(this.Id.ToString());
                viewModel.PhongThuy = null;
                LoadDataPhongThuy();
                NeedToRefresh = false;
                LoadingHelper.Hide();
            }
        }

        // tab thong tin
        private async Task LoadDataThongTin(string Id)
        {
            if (Id != null && viewModel.singleContact.contactid == Guid.Empty)
            {
                LoadingHelper.Show();
                await viewModel.loadOneContact(Id);
                if (viewModel.singleContact.gendercode != null)
                { 
                   viewModel.singleGender = ContactGender.GetGenderById(viewModel.singleContact.gendercode); 
                }
                if (viewModel.singleContact.bsd_localization != null)
                {
                    viewModel.SingleLocalization = AccountLocalization.GetLocalizationById(viewModel.singleContact.bsd_localization);
                }
                else
                {
                    viewModel.SingleLocalization = null;
                }
                LoadingHelper.Hide();
            }
        }

        #region Tab giao dich
        private async Task LoadDataGiaoDich(string Id)
        {
            if (viewModel.list_danhsachdatcho == null || viewModel.list_danhsachdatcoc == null || viewModel.list_danhsachhopdong == null || viewModel.list_chamsockhachhang == null)
            {
                LoadingHelper.Show();
                viewModel.PageDanhSachDatCho = 1;
                viewModel.PageDanhSachDatCoc = 1;
                viewModel.PageDanhSachHopDong = 1;
                viewModel.PageChamSocKhachHang = 1;

                viewModel.list_danhsachdatcho = new ObservableCollection<QueueFormModel>();
                viewModel.list_danhsachdatcoc = new ObservableCollection<QuotationReseravtion>();
                viewModel.list_danhsachhopdong = new ObservableCollection<OptionEntry>();
                viewModel.list_chamsockhachhang = new ObservableCollection<Case>();

                await Task.WhenAll(
                   viewModel.LoadQueuesForContactForm(Id),
                   viewModel.LoadReservationForContactForm(Id),
                   viewModel.LoadOptoinEntryForContactForm(Id),
                   viewModel.LoadCaseForContactForm(Id)
               );
                LoadingHelper.Hide();
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
               viewModel.PhongThuy = new PhongThuyModel();
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

        private void Close_LookUpImagePhongThuy_Tapped(object sender, EventArgs e)
        {
            LookUpImagePhongThuy.IsVisible = false;
        }

        #endregion

        private async void NhanTin_Tapped(object sender, EventArgs e)
        {           
            string phone = viewModel.singleContact.mobilephone;
            if (phone != string.Empty)
            {
                LoadingHelper.Show();
                var checkVadate = PhoneNumberFormatVNHelper.CheckValidate(phone);
                if (checkVadate == true)
                {
                    SmsMessage sms = new SmsMessage(null, phone);
                    await Sms.ComposeAsync(sms);
                    LoadingHelper.Hide();
                }
                else
                {
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage("Số điện thoại sai định dạng. Vui lòng kiểm tra lại");
                }
            }
            else
            {
                LoadingHelper.Hide();
                ToastMessageHelper.ShortMessage("Khách hàng không có số điện thoại. Vui lòng kiểm tra lại");
            }
        }

        private async void GoiDien_Tapped(object sender, EventArgs e)
        {          
            string phone = viewModel.singleContact.mobilephone;
            if (phone != string.Empty)
            {
                LoadingHelper.Show();
                var checkVadate = PhoneNumberFormatVNHelper.CheckValidate(phone);
                if (checkVadate == true)
                {
                   await Launcher.OpenAsync($"tel:{phone}");
                    LoadingHelper.Hide();
                }
                else
                {
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage("Số điện thoại sai định dạng. Vui lòng kiểm tra lại");
                }
            }
            else
            {
                LoadingHelper.Hide();
                ToastMessageHelper.ShortMessage("Khách hàng không có số điện thoại. Vui lòng kiểm tra lại");
            }
        }

        private async void ThongTin_Tapped(object sender, EventArgs e)
        {
            Tab_Tapped(1);
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
            if (!string.IsNullOrEmpty(viewModel.singleContact._parentcustomerid_value))
            {
                LoadingHelper.Show();
                AccountDetailPage newPage = new AccountDetailPage(Guid.Parse(viewModel.singleContact._parentcustomerid_value));
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
                        ToastMessageHelper.ShortMessage("Không tìm thấy thông tin công ty");
                    }
                };
            }
        }

        private void EditContact_Clicked(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            ContactForm newPage = new ContactForm(Id);
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
                    ToastMessageHelper.ShortMessage("Không tìm thấy thông tin khách hàng");
                }
            };
        }

        private void GiuChoItem_Tapped(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            var itemId = (Guid)((sender as StackLayout).GestureRecognizers[0] as TapGestureRecognizer).CommandParameter;
            QueuesDetialPage queuesDetialPage = new QueuesDetialPage(itemId);
            queuesDetialPage.OnCompleted = async (IsSuccess) => {
                if (IsSuccess)
                {
                    await Navigation.PushAsync(queuesDetialPage);
                    LoadingHelper.Hide();
                }
                else
                {
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage("Không tìm thấy thông tin");
                }
            };
        }
    }
}