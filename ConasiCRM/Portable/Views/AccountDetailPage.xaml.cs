using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Helpers;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.Resources;
using ConasiCRM.Portable.Settings;
using ConasiCRM.Portable.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConasiCRM.Portable.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AccountDetailPage : ContentPage
    {
        public Action<bool> OnCompleted;
        private Guid AccountId;
        public static bool? NeedToRefreshAccount = null;
        public static bool? NeedToRefreshMandatory = null;
        public static bool? NeedToRefreshQueues = null;
        public static bool? NeedToRefreshActivity = null;
        public static OptionSet FromCustomer = null;
        private AccountDetailPageViewModel viewModel;

        public AccountDetailPage(Guid accountId)
        {
            InitializeComponent();
            AccountId = accountId;
            this.BindingContext = viewModel = new AccountDetailPageViewModel();
            NeedToRefreshAccount = false;
            NeedToRefreshMandatory = false;
            NeedToRefreshActivity = false;
            LoadingHelper.Show();
            Tab_Tapped(1);
            Init();
        }

        public async void Init()
        {
            await LoadDataThongTin(AccountId.ToString());
            if ((viewModel.singleAccount.employee_id != Guid.Empty && !string.IsNullOrWhiteSpace(viewModel.singleAccount.employee_name)) && (viewModel.singleAccount.employee_id == UserLogged.Id && viewModel.singleAccount.employee_name == UserLogged.User))
            {
                viewModel.ButtonCommandList.Add(new FloatButtonItem(Language.them_cuoc_hop, "FontAwesomeRegular", "\uf274", null, NewMeet));
                viewModel.ButtonCommandList.Add(new FloatButtonItem(Language.them_cuoc_goi, "FontAwesomeSolid", "\uf095", null, NewPhoneCall));
                viewModel.ButtonCommandList.Add(new FloatButtonItem(Language.them_cong_viec, "FontAwesomeSolid", "\uf073", null, NewTask));
                viewModel.ButtonCommandList.Add(new FloatButtonItem(Language.chinh_sua, "FontAwesomeRegular", "\uf044", null, Update));
            }
            else
            {
                floatingButtonGroup.IsVisible = false;
            }

            if (viewModel.singleAccount.accountid != Guid.Empty)
            {
                FromCustomer = new OptionSet { Val = viewModel.singleAccount.accountid.ToString(), Label = viewModel.singleAccount.bsd_name, Title = viewModel.CodeAccount };
                OnCompleted?.Invoke(true);
            }
            else
                OnCompleted?.Invoke(false);
            LoadingHelper.Hide();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (NeedToRefreshAccount == true)
            {
                LoadingHelper.Show();
                await viewModel.LoadOneAccount(AccountId.ToString());
                viewModel.singleAccount.bsd_address = await SetAddress();
                if (viewModel.singleAccount.bsd_businesstypesys != null)
                {
                    viewModel.GetTypeById(viewModel.singleAccount.bsd_businesstypesys);
                }
                if (viewModel.singleAccount.bsd_localization != null)
                {
                    viewModel.Localization = AccountLocalization.GetLocalizationById(viewModel.singleAccount.bsd_localization);
                }
                NeedToRefreshAccount = false;
                LoadingHelper.Hide();
            }
            if (NeedToRefreshMandatory == true)
            {
                LoadingHelper.Show();
                viewModel.PageMandatory = 1;
                viewModel.list_MandatorySecondary.Clear();
                await LoadDataNguoiUyQuyen(AccountId.ToString());
                NeedToRefreshMandatory = false;
                LoadingHelper.Hide();
            }

            if (NeedToRefreshQueues == true)
            {
                LoadingHelper.Show();
                viewModel.PageQueueing = 1;
                viewModel.list_thongtinqueing.Clear();
                await viewModel.LoadDSQueueingAccount(AccountId);
                NeedToRefreshQueues = false;
                LoadingHelper.Hide();
            }
            if (NeedToRefreshActivity == true)
            {
                LoadingHelper.Show();
                viewModel.PageCase = 1;
                viewModel.list_thongtincase.Clear();
                await viewModel.LoadCaseForAccountForm();
                ActivityPopup.Refresh();
                NeedToRefreshActivity = false;
                LoadingHelper.Hide();
            }
        }

        // tab thong tin
        private async Task LoadDataThongTin(string Id)
        {
            if (Id != null && viewModel.singleAccount == null)
            {
                await viewModel.LoadOneAccount(Id);

                viewModel.singleAccount.bsd_address = await SetAddress();

                if (viewModel.singleAccount.bsd_businesstypesys != null)
                {
                    viewModel.GetTypeById(viewModel.singleAccount.bsd_businesstypesys);
                }
                if (viewModel.singleAccount.bsd_localization != null)
                {
                    viewModel.Localization = AccountLocalization.GetLocalizationById(viewModel.singleAccount.bsd_localization);
                }
            }
        }
        private async Task<string> SetAddress()
        {
            List<string> listaddress = new List<string>();
            if (!string.IsNullOrWhiteSpace(viewModel.singleAccount.bsd_housenumberstreet))
            {
                listaddress.Add(viewModel.singleAccount.bsd_housenumberstreet);
            }
            if (!string.IsNullOrWhiteSpace(viewModel.singleAccount.district_name))
            {
                listaddress.Add(viewModel.singleAccount.district_name);
            }
            if (!string.IsNullOrWhiteSpace(viewModel.singleAccount.province_name))
            {
                listaddress.Add(viewModel.singleAccount.province_name);
            }
            if (!string.IsNullOrWhiteSpace(viewModel.singleAccount.bsd_postalcode))
            {
                listaddress.Add(viewModel.singleAccount.bsd_postalcode);
            }
            if (!string.IsNullOrWhiteSpace(viewModel.singleAccount.country_name))
            {
                listaddress.Add(viewModel.singleAccount.country_name);
            }

            string address = string.Join(", ", listaddress);

            return address;
        }
        private async void Website_Tapped(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            await Xamarin.Essentials.Browser.OpenAsync(viewModel.singleAccount.websiteurl);
            LoadingHelper.Hide();
        }

        #region tab giao dich
        // tab giao dich
        private async Task LoadDataGiaoDich(string Id)
        {
            if (Id != null)
            {
                LoadingHelper.Show();
                if (viewModel.list_thongtinqueing.Count == 0 && viewModel.list_thongtinquotation.Count == 0 && viewModel.list_thongtincontract.Count == 0 && viewModel.list_thongtincase.Count == 0)
                {
                    await Task.WhenAll(
                        viewModel.LoadDSQueueingAccount(AccountId),
                        viewModel.LoadDSQuotationAccount(AccountId),
                        viewModel.LoadDSContractAccount(AccountId),
                        viewModel.LoadCaseForAccountForm()
                        ); 
                }
                LoadingHelper.Hide();
            }
        }

        private async void ShowMoreQueueing_Clicked(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            viewModel.PageQueueing++;
            await viewModel.LoadDSQueueingAccount(AccountId);
            LoadingHelper.Hide();
        }

        private async void ShowMoreQuotation_Clicked(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            viewModel.PageQuotation++;
            await viewModel.LoadDSQuotationAccount(AccountId);
            LoadingHelper.Hide();
        }

        private async void ShowMoreContract_Clicked(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            viewModel.PageContract++;
            await viewModel.LoadDSContractAccount(AccountId);
            LoadingHelper.Hide();
        }

        private async void ShowMoreCase_Clicked(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            viewModel.PageCase++;
            await viewModel.LoadCaseForAccountForm();
            LoadingHelper.Hide();
        }

        private void ChiTietDatCoc_Tapped(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            var itemId = (Guid)((sender as StackLayout).GestureRecognizers[0] as TapGestureRecognizer).CommandParameter;
            BangTinhGiaDetailPage bangTinhGiaDetail = new BangTinhGiaDetailPage(itemId) { Title=Language.dat_coc_title};
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

        private void CaseItem_Tapped(object sender, EventArgs e)
        {
            var item = (HoatDongListModel)((sender as StackLayout).GestureRecognizers[0] as TapGestureRecognizer).CommandParameter;
            if (item != null && item.activityid != Guid.Empty)
            {
                ActivityPopup.ShowActivityPopup(item.activityid, item.activitytypecode);
            }
        }

        #endregion

        #region tab nguoi uy quyyen 

        private async Task LoadDataNguoiUyQuyen(string Id)
        {
            if (Id != null && viewModel.list_MandatorySecondary.Count <= 0)
            {
                await viewModel.Load_List_Mandatory_Secondary(Id);
            }
        }        

        private async void ShowMoreMandatory_Clicked(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            viewModel.PageMandatory++;
            await viewModel.Load_List_Mandatory_Secondary(AccountId.ToString());
            LoadingHelper.Hide();
        }

        #endregion

        private async void NhanTin_Tapped(object sender, EventArgs e)
        {
            if(viewModel.singleAccount != null && !string.IsNullOrWhiteSpace(viewModel.singleAccount.telephone1))
            {
                string phone = viewModel.singleAccount.telephone1.Replace(" ", "");
                var checkVadate = PhoneNumberFormatVNHelper.CheckValidate(phone);
                if (checkVadate == true)
                {
                    try
                    {
                        var message = new SmsMessage(null, phone);
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
            if (viewModel.singleAccount != null && !string.IsNullOrWhiteSpace(viewModel.singleAccount.telephone1))
            {
                string phone = viewModel.singleAccount.telephone1.Replace(" ", "");
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

        private void ThongTin_Tapped(object sender, EventArgs e)
        {
            Tab_Tapped(1);
        }

        private async void GiaoDich_Tapped(object sender, EventArgs e)
        {
            Tab_Tapped(2);
            await LoadDataGiaoDich(AccountId.ToString());
        }

        private async void NguoiUyQuyen_Tapped(object sender, EventArgs e)
        {
            Tab_Tapped(3);
            await LoadDataNguoiUyQuyen(AccountId.ToString());
            SetHeightListView();
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
                VisualStateManager.GoToState(radBorderNguoiUyQuyen, "Selected");
                VisualStateManager.GoToState(lbNguoiUyQuyen, "Selected");
                TabNguoiUyQuyen.IsVisible = true;
            }
            else
            {
                VisualStateManager.GoToState(radBorderNguoiUyQuyen, "Normal");
                VisualStateManager.GoToState(lbNguoiUyQuyen, "Normal");
                TabNguoiUyQuyen.IsVisible = false;
            }
        }

        private void NguoiDaiDien_Tapped(object sender, EventArgs e)
        {
            if (viewModel.PrimaryContact.contactid != null)
            {
                LoadingHelper.Show();
                ContactDetailPage newPage = new ContactDetailPage(viewModel.PrimaryContact.contactid);
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
                        ToastMessageHelper.ShortMessage(Language.khong_tim_thay_thong_tin);
                    }
                };
            }
        }

        private async void Update(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            AccountForm newPage = new AccountForm(viewModel.singleAccount.accountid);
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
                    ToastMessageHelper.ShortMessage(Language.khong_tim_thay_thong_tin);
                }
            };
        }

        private void GiuChoItem_Tapped(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            var itemId = (Guid)((sender as StackLayout).GestureRecognizers[0] as TapGestureRecognizer).CommandParameter;
            QueuesDetialPage queuesDetialPage = new QueuesDetialPage(itemId);
            queuesDetialPage.OnCompleted = async (IsSuccess) =>
            {
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

        private void CloseContentMandatorySecondary_Tapped(object sender, EventArgs e)
        {
            ContentMandatorySecondary.IsVisible = false;
        }

        private void ListMandatorySecondary_Tapped(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            Grid grid = (Grid)sender;
            var item = (TapGestureRecognizer)grid.GestureRecognizers[0];
            viewModel.MandatorySecondary = item.CommandParameter as MandatorySecondaryModel;
            ContentMandatorySecondary.IsVisible = true;
            LoadingHelper.Hide();
        }
        // loadmore
        private async void ListMoreMandatory_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            if (e.Item != null)
            {
                var item = e.Item as MandatorySecondaryModel;
                if (viewModel.list_MandatorySecondary.IndexOf(item) == viewModel.list_MandatorySecondary.Count() - 1)
                {
                    viewModel.isLoadMore = true;
                    viewModel.PageMandatory++;
                    await viewModel.Load_List_Mandatory_Secondary(this.AccountId.ToString());
                    viewModel.isLoadMore = false;
                    SetHeightListView();
                }
            }
        }
        private void SetHeightListView()
        {
            double height_item = (viewModel.list_MandatorySecondary.Count() * 110) + 50;
            double height_mb = ((DeviceDisplay.MainDisplayInfo.Height / DeviceDisplay.MainDisplayInfo.Density) * 2 / 3) + 50;
            if (height_item > height_mb)
            {
                ListMandatory.HeightRequest = height_mb;
            }
            else
            {
                ListMandatory.HeightRequest = height_item;
            }
            if (viewModel.list_MandatorySecondary.Count() == 0)
            {
                lb_ListMandatory.IsVisible = true;
                ListMandatory.IsVisible = false;
            }
            else
            {
                lb_ListMandatory.IsVisible = false;
                ListMandatory.IsVisible = true;
            }
        }
        private async void NewMeet(object sender, EventArgs e)
        {
            if (viewModel.singleAccount != null)
            {
                LoadingHelper.Show();
                await Navigation.PushAsync(new MeetingForm());
                LoadingHelper.Hide();
            }
        }
        private async void NewPhoneCall(object sender, EventArgs e)
        {
            if (viewModel.singleAccount != null)
            {
                LoadingHelper.Show();
                await Navigation.PushAsync(new PhoneCallForm());
                LoadingHelper.Hide();
            }
        }
        private async void NewTask(object sender, EventArgs e)
        {
            if (viewModel.singleAccount != null)
            {
                LoadingHelper.Show();
                await Navigation.PushAsync(new TaskForm());
                LoadingHelper.Hide();
            }
        }
        private void ActivityPopup_HidePopupActivity(object sender, EventArgs e)
        {
            OnAppearing();
        }
    }
}