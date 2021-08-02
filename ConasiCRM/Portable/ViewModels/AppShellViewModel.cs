using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace ConasiCRM.Portable.ViewModels
{
    class AppShellViewModel
    {
        public ICommand LogoutCommand { get; }
        public ICommand Develop { get; }

        public AppShellViewModel()
        {
            LogoutCommand = new Command(Logout);
            Develop = new Command(Developing);
        }

        private async void Logout()
        {
            await Shell.Current.Navigation.PushModalAsync(new Login(),false);          
        }

        private async void Developing()
        {
            await Shell.Current.DisplayAlert("Thông báo", "Chức năng đang được phát triển", "Đóng");
        }
    }
}
