using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.Settings;
using Xamarin.Forms;

namespace ConasiCRM.Portable.ViewModels
{
    class AppShellViewModel :BaseViewModel
    {
        public ICommand LogoutCommand { get; }
        private string _avartar;
        public string Avartar { get => _avartar; set { _avartar = value;OnPropertyChanged(nameof(Avartar)); } }

        private string _userName;
        public string UserName { get => _userName; set { _userName = value;OnPropertyChanged(nameof(UserName)); } }

        private string _contactName;
        public string ContactName { get => _contactName; set { _contactName = value; OnPropertyChanged(nameof(ContactName)); } }

        private string _verApp;
        public string VerApp { get => _verApp; set { _verApp = value; OnPropertyChanged(nameof(VerApp)); } }

        public AppShellViewModel()
        {
            LogoutCommand = new Command(Logout);
            UserName = UserLogged.User;
            ContactName = string.IsNullOrWhiteSpace(UserLogged.ContactName) ? UserLogged.User : UserLogged.ContactName;
            Avartar = UserLogged.Avartar;
            VerApp = Config.OrgConfig.VerApp;
        }

        private async void Logout()
        {
            await Shell.Current.Navigation.PushModalAsync(new Login(),false);          
        }
    }
}
