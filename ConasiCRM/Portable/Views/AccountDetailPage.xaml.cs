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
            Init();
        }

        public async void Init()
        {
            await LoadDataThongTin(AccountId.ToString());
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
                if (viewModel.singleAccount.bsd_businesstype != null)
                {
                    viewModel.GetTypeById(viewModel.singleAccount.bsd_businesstype);
                }
                if (viewModel.singleAccount.bsd_localization != null)
                {
                    viewModel.Localization = AccountLocalization.GetLocalizationById(viewModel.singleAccount.bsd_localization);
                }
            }
        }

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

    }
}