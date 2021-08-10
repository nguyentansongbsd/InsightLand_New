using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        private AccountDetailPageViewModel viewModel;
        
        public AccountDetailPage(Guid accountId)
        {
            InitializeComponent();
            AccountId = accountId;
            this.BindingContext = viewModel = new AccountDetailPageViewModel();
            LoadingHelper.Show();
            Tab_Tapped(1);
            Init();
        }

        public async void Init()
        {
            await LoadDataThongTin(AccountId.ToString());
            viewModel.ButtonCommandList.Add(new FloatButtonItem("Thêm Người ủy quyền", "FontAwesomeSolid", "\uf2b5", null, AddMandatorySecondary));
            viewModel.ButtonCommandList.Add(new FloatButtonItem("Chỉnh sửa", "FontAwesomeRegular", "\uf044", null, Update));
            if (viewModel.singleAccount.accountid != null)
                OnCompleted?.Invoke(true);
            else
                OnCompleted?.Invoke(false);
            LoadingHelper.Hide();
        }

        // tab thong tin
        private async Task LoadDataThongTin(string Id)
        {
            if (Id != null && viewModel.singleAccount == null)
            {
                await viewModel.LoadOneAccount(Id);
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

        #region tab giao dich
        // tab giao dich
        private async Task LoadDataGiaoDich(string Id)
        {
            if(Id!= null)
            {
                if (viewModel.list_thongtinqueing.Count <= 0) { await viewModel.LoadDSQueueingAccount(AccountId); }

                if (viewModel.list_thongtinquotation.Count <= 0) { await viewModel.LoadDSQuotationAccount(AccountId); }

                if (viewModel.list_thongtincontract.Count <= 0) { await viewModel.LoadDSContractAccount(AccountId); }

                if (viewModel.list_thongtincase.Count <= 0) { await viewModel.LoadDSCaseAccount(AccountId); }
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
            await viewModel.LoadDSCaseAccount(AccountId);
            LoadingHelper.Hide();
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

        private async void DeleteMandatory_Clicked(object sender, EventArgs e)
        {
            Label lblClicked = (Label)sender;
            var a = (TapGestureRecognizer)lblClicked.GestureRecognizers[0];
            MandatorySecondaryModel item = a.CommandParameter as MandatorySecondaryModel;
            var conform = await DisplayAlert("Xác nhận", "Bạn có muốn xóa người ủy quyền không ?", "Đồng ý", "Hủy");
            if (conform == false) return;            
            LoadingHelper.Show();
            var IsSuccess = await viewModel.DeleteMandatory_Secondary(item.bsd_mandatorysecondaryid.ToString());
            if(IsSuccess)
            {
                viewModel.list_MandatorySecondary.Remove(item);
                LoadingHelper.Hide();
                await DisplayAlert("Thông Báo", "Đã xóa người ủy quyền được chọn!", "Đóng");
            }   
            else
            {
                LoadingHelper.Hide();
                await DisplayAlert("Thông Báo", "Xóa người ủy quyền thất bại!", "Đóng");
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
            string phone = viewModel.singleAccount.telephone1;
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
            string phone = viewModel.singleAccount.telephone1;
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
            await LoadDataThongTin(AccountId.ToString());
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
            if(viewModel.PrimaryContact.contactid != null)
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
                        await Shell.Current.DisplayAlert("Thông báo", "Không tìm thấy thông tin. Vui lòng thử lại.", "Đóng");
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
                    await DisplayAlert("Thông báo", "Không tìm thấy thông tin. Vui lòng thử lại.", "Đóng");
                }
            };    
        }

        private async void AddMandatorySecondary(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            MandatorySecondaryForm newPage = new MandatorySecondaryForm(viewModel.singleAccount.accountid);
            await Navigation.PushAsync(newPage);
            LoadingHelper.Hide();
        }
    }
}