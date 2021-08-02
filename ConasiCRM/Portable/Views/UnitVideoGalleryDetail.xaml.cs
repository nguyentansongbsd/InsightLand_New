using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using ConasiCRM.Portable.Config;
using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.ViewModels;
using FormsVideoLibrary;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace ConasiCRM.Portable.Views
{
    public partial class UnitVideoGalleryDetail : ContentPage
    {
        private string videoName;
        private string folderName;
        private string category;
        UnitVideoGalleryViewModel vm;

        public UnitVideoGalleryDetail(string videoName, string folderName, string category)
        {
            InitializeComponent();

            this.videoName = HttpUtility.UrlEncode(videoName);
            this.folderName = folderName;
            this.category = category;

            if (Device.RuntimePlatform == Device.Android)
            {
                NavigationPage.SetHasNavigationBar(this, false);
            }

            this.BindingContext = vm = new UnitVideoGalleryViewModel();

            this.LoadFromServer();
        }

        void Handle_UpdateStatus(object sender, System.EventArgs e)
        {
           if((sender as VideoPlayer).Status == VideoStatus.NotReady)
            {
                this.vm.IsBusy = true;
                return;
            }
            this.vm.IsBusy = false;
        }

        public async Task LoadFromServer()
        {
            vm.IsBusy = true;

            string category_value = "";
            switch (category)
            {
                case "Units":
                    category_value = "product";
                    break;
                case "Project":
                    category_value = "bsd_project";
                    break;

            }

            string token = (await CrmHelper.getSharePointToken()).access_token;

            VideoPlayer.Source = VideoSource.FromUri(OrgConfig.SharePointResource + "/sites/" + OrgConfig.SharePointSiteName + "/_layouts/15/download.aspx?SourceUrl=/sites/" + OrgConfig.SharePointSiteName + "/" + category_value + "/" + folderName + "/" + videoName + "&access_token=" + token);
            vm.IsBusy = false;
        }
    }
}
