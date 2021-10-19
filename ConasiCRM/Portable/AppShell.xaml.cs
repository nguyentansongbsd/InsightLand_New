using System;
using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Helpers;
using ConasiCRM.Portable.Settings;
using ConasiCRM.Portable.ViewModels;
using ConasiCRM.Portable.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConasiCRM.Portable
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AppShell : Shell
    {     
        private AppShellViewModel viewModel;
        public AppShell()
        {
            InitializeComponent();
            this.BindingContext = viewModel = new AppShellViewModel();
        }

        private async void UserInfor_Tapped(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            if (UserLogged.ContactId == Guid.Empty)
            {
                ToastMessageHelper.ShortMessage("Chưa có contact, không thể chỉnh sửa thông tin");
                LoadingHelper.Hide();
                return;
            }
            
            await Shell.Current.Navigation.PushAsync(new UserInfoPage());
            this.FlyoutIsPresented = false;
            LoadingHelper.Hide();
            //userInfo.IsCompleted = async (isSuccess) => {
            //    if (isSuccess)
            //    {
            //        await Shell.Current.Navigation.PushAsync(userInfo);
            //        LoadingHelper.Hide();
            //    }
            //    else
            //    {
            //        LoadingHelper.Hide();
            //        ToastMessageHelper.ShortMessage("Không tìm thấy thông tin user");
            //    }
            //};

        }
    }       
}