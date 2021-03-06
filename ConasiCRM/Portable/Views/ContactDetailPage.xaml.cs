using Conasi.Portable.Views;
using ConasiCRM.Portable.Controls;
using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Helpers;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.Resources;
using ConasiCRM.Portable.Settings;
using ConasiCRM.Portable.ViewModels;
using Stormlion.PhotoBrowser;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
        public static bool? NeedToRefreshQueues = null;
        public static bool? NeedToRefreshActivity = null;
        private ContactDetailPageViewModel viewModel;
        private Guid Id;
        private PhotoBrowser photoBrowser;
        PhotoShow photoShow;
        public static OptionSet FromCustomer = null;
        public ContactDetailPage(Guid contactId)
        {
            InitializeComponent();
            this.BindingContext = viewModel = new ContactDetailPageViewModel();
            LoadingHelper.Show();
            NeedToRefresh = false;
            NeedToRefreshActivity = false;
            Tab_Tapped(1);
            Id = contactId;
            Init();
        }
        public async void Init()
        {
            await LoadDataThongTin(Id.ToString());

            viewModel.ButtonCommandList.Add(new FloatButtonItem(Language.them_cuoc_hop, "FontAwesomeRegular", "\uf274", null, NewMeet));
            viewModel.ButtonCommandList.Add(new FloatButtonItem(Language.them_cuoc_goi, "FontAwesomeSolid", "\uf095", null, NewPhoneCall));
            viewModel.ButtonCommandList.Add(new FloatButtonItem(Language.them_cong_viec, "FontAwesomeSolid", "\uf073", null, NewTask));
            viewModel.ButtonCommandList.Add(new FloatButtonItem(Language.chinh_sua, "FontAwesomeRegular", "\uf044", null, EditContact));
            if (viewModel.singleContact.employee_id != UserLogged.Id)
            {

                floatingButtonGroup.IsVisible = false;
            }

            if (viewModel.singleContact.contactid != Guid.Empty)
            {
                FromCustomer = new OptionSet { Val = viewModel.singleContact.contactid.ToString(), Label = viewModel.singleContact.bsd_fullname, Title = viewModel.CodeContac };
                OnCompleted(true);
            }
            else
                OnCompleted(false);
            LoadingHelper.Hide();
        }

        private async void NewMeet(object sender, EventArgs e)
        {
            if (viewModel.singleContact != null)
            {
                LoadingHelper.Show();
                await Navigation.PushAsync(new MeetingForm());
                LoadingHelper.Hide();
            }
        }

        private async void NewPhoneCall(object sender, EventArgs e)
        {
            if (viewModel.singleContact != null)
            {
                LoadingHelper.Show();
                await Navigation.PushAsync(new PhoneCallForm());
                LoadingHelper.Hide();
            }
        }

        private async void NewTask(object sender, EventArgs e)
        {
            if (viewModel.singleContact != null)
            {
                LoadingHelper.Show();
                await Navigation.PushAsync(new TaskForm());
                LoadingHelper.Hide();
            }
        }

        private void EditContact(object sender, EventArgs e)
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
                    ToastMessageHelper.ShortMessage(Language.khong_tim_thay_thong_tin_khach_hang);
                }
            };
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
            if (NeedToRefreshQueues == true)
            {
                LoadingHelper.Show();
                viewModel.PageDanhSachDatCho = 1;
                viewModel.list_danhsachdatcho.Clear();
                await viewModel.LoadQueuesForContactForm(viewModel.singleContact.contactid.ToString());
                NeedToRefreshQueues = false;
                LoadingHelper.Hide();
            }
            if(NeedToRefreshActivity == true && viewModel.list_chamsockhachhang != null)
            {
                LoadingHelper.Show();
                viewModel.PageChamSocKhachHang = 1;
                viewModel.list_chamsockhachhang.Clear();
                await viewModel.LoadCaseForContactForm();
                ActivityPopup.Refresh();
                NeedToRefreshActivity = false;
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
                await viewModel.GetImageCMND();
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
                photoShow = new PhotoShow(viewModel.CollectionCMNDs);
                LoadingHelper.Hide();
            }
        }

        private void CMNDFront_Tapped(object sender, EventArgs e)
        {
            if (viewModel.singleContact != null && !string.IsNullOrWhiteSpace(viewModel.singleContact.bsd_etag_front_url))
            {
                photoBrowser = new PhotoBrowser
                {
                    Photos = new List<Photo>
                {
                    new Photo{
                        URL = viewModel.singleContact.bsd_etag_front_url
                    }
                }
                };
                photoBrowser.Show();
            }
        }

        private void CMNDBehind_Tapped(object sender, EventArgs e)
        {
            if (viewModel.singleContact != null && !string.IsNullOrWhiteSpace(viewModel.singleContact.bsd_etag_behind_url))
            {
                photoBrowser = new PhotoBrowser
                {
                    Photos = new List<Photo>
                {
                    new Photo{
                        URL = viewModel.singleContact.bsd_etag_behind_url
                    }
                }
                };
                photoBrowser.Show();
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
                viewModel.list_danhsachdatcoc = new ObservableCollection<ReservationListModel>();
                viewModel.list_danhsachhopdong = new ObservableCollection<ContractModel>();
                viewModel.list_chamsockhachhang = new ObservableCollection<HoatDongListModel>();

                await Task.WhenAll(
                   viewModel.LoadQueuesForContactForm(Id),
                   viewModel.LoadReservationForContactForm(Id),
                   viewModel.LoadOptoinEntryForContactForm(Id),
                   viewModel.LoadCaseForContactForm()
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
            viewModel.PageDanhSachHopDong++;
            await viewModel.LoadOptoinEntryForContactForm(viewModel.singleContact.contactid.ToString());
            LoadingHelper.Hide();
        }

        //Cham soc khach hang
        private async void ShowMoreChamSocKhachHang_Clicked(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            viewModel.PageChamSocKhachHang++;
            await viewModel.LoadCaseForContactForm();
            LoadingHelper.Hide();
        }

        private void ChiTietDatCoc_Tapped(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            var itemId = (Guid)((sender as StackLayout).GestureRecognizers[0] as TapGestureRecognizer).CommandParameter;
            BangTinhGiaDetailPage bangTinhGiaDetail = new BangTinhGiaDetailPage(itemId) { Title =Language.dat_coc_title};
            bangTinhGiaDetail.OnCompleted = async (isSuccess) =>
            {
                if (isSuccess)
                {
                    await Navigation.PushAsync(bangTinhGiaDetail);
                    LoadingHelper.Hide();
                }
                else
                {
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage(Language.khong_tim_thay_thong_tin);
                }
            };
        }

        private void ItemHopDong_Tapped(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            var itemId = (Guid)((sender as Grid).GestureRecognizers[0] as TapGestureRecognizer).CommandParameter;
            ContractDetailPage contractDetail = new ContractDetailPage(itemId);
            contractDetail.OnCompleted = async (isSuccess) =>
            {
                if (isSuccess)
                {
                    await Navigation.PushAsync(contractDetail);
                    LoadingHelper.Hide();
                }
                else
                {
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage(Language.khong_tim_thay_thong_tin);
                }
            };
        }

        private async void CaseItem_Tapped(object sender, EventArgs e)
        {
            var item = (HoatDongListModel)((sender as StackLayout).GestureRecognizers[0] as TapGestureRecognizer).CommandParameter;
            if (item != null && item.activityid != Guid.Empty)
            {
                ActivityPopup.ShowActivityPopup(item.activityid, item.activitytypecode);
            }
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
            LookUpImage.IsVisible = true;
            ImageDetail.Source = viewModel.PhongThuy.image;
        }

        protected override bool OnBackButtonPressed()
        {
            if (LookUpImage.IsVisible)
            {
                LookUpImage.IsVisible = false;
                return true;
            }
            return base.OnBackButtonPressed();
        }

        private void Close_LookUpImage_Tapped(object sender, EventArgs e)
        {
            LookUpImage.IsVisible = false;
        }

        #endregion

        private async void NhanTin_Tapped(object sender, EventArgs e)
        {
            //string phone = viewModel.singleContact.mobilephone.Replace(" ", "");
            if (viewModel.singleContact != null && !string.IsNullOrWhiteSpace(viewModel.singleContact.mobilephone))
            {
                string phone = viewModel.singleContact.mobilephone.Replace(" ", "");
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
            if (viewModel.singleContact != null && !string.IsNullOrWhiteSpace(viewModel.singleContact.mobilephone))
            {
                string phone = viewModel.singleContact.mobilephone.Replace(" ", "");
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
                        ToastMessageHelper.ShortMessage(Language.sdt_sai_dinh_dang_vui_long_kiem_tra_lai);
                    }
                }
                else
                {
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage(Language.khach_hang_khong_co_sdt_vui_long_kiem_tra_lai);
                }
            }
            else
            {
                LoadingHelper.Hide();
                ToastMessageHelper.ShortMessage(Language.khach_hang_khong_co_sdt_vui_long_kiem_tra_lai);
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
                        ToastMessageHelper.ShortMessage(Language.khong_tim_thay_thong_tin_cong_ty);
                    }
                };
            }
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
                    ToastMessageHelper.ShortMessage(Language.khong_tim_thay_thong_tin);
                }
            };
        }

        private void ActivityPopup_HidePopupActivity(object sender, EventArgs e)
        {
            OnAppearing();
        }
    }
}