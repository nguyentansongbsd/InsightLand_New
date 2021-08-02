using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ConasiCRM.Portable.Config;
using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.ViewModels;
using FormsVideoLibrary;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace ConasiCRM.Portable.Views
{
    public partial class UnitVideoGallery : ContentPage
    {
        public Action<bool> OnCompleted;
        public UnitVideoGalleryViewModel viewModel;
        public string Category { get; set; }
        public string Folder { get; set; }

        public UnitVideoGallery(string category, string unitId, string unitName, string title)
        {
            InitializeComponent();
            this.BindingContext = viewModel = new UnitVideoGalleryViewModel();
            this.LoadFromServer();

            this.Category = category;
            this.Folder = unitName.Replace('.', '-') + "_" + unitId.Replace("-", string.Empty).ToUpper();

            this.Title = title;
        }

        public async Task LoadFromServer()
        {
            GetTokenResponse getTokenResponse = await CrmHelper.getSharePointToken();
            var client = BsdHttpClient.Instance();
            string fileListUrl = $"{OrgConfig.SharePointResource}/sites/" + OrgConfig.SharePointSiteName + "/_api/web/Lists/GetByTitle('" + Category + "')/RootFolder/Folders('" + Folder + "')/Files";
            var request = new HttpRequestMessage(HttpMethod.Get, fileListUrl);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", getTokenResponse.access_token);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                SharePointFieldResult sharePointFieldResult = JsonConvert.DeserializeObject<SharePointFieldResult>(body);
                var list = sharePointFieldResult.value;
                foreach (var item in list)
                {
                    if (item.Name.Split('.')[1] == "flv" || item.Name.Split('.')[1] == "mp4" || item.Name.Split('.')[1] == "m3u8" || item.Name.Split('.')[1] == "3gp" || item.Name.Split('.')[1] == "mov" || item.Name.Split('.')[1] == "avi" || item.Name.Split('.')[1] == "wmv")
                    {
                        //var detail = new UnitVideoGalleryDetail(item.Name, "E,30-1022_1222EBDBD03BE911A99C000D3AA21FA9");
                        this.viewModel.VideoList.Add(item);
                    }
                }
                OnCompleted?.Invoke(true);
            }
            else
            {
                OnCompleted?.Invoke(false);
            }
            if (viewModel.VideoList.Count == 0) { EmptyText.IsVisible = true; listview.IsVisible = false; }
            viewModel.IsBusy = false;
        }

        //public async Task LoadFromServer()
        //{
        //    string token = (await getSharePointToken()).access_token;

        //    VideoPlayer.Source = VideoSource.FromUri("https://conasivn.sharepoint.com/sites/Conasi/_layouts/15/download.aspx?SourceUrl=/sites/Conasi/product/E,30-1022_1222EBDBD03BE911A99C000D3AA21FA9/Peru%208K%20HDR%2060FPS%20%28FUHD%29%2Emp4&access_token=" + token);
        //}

        //public async Task<GetTokenResponse> getSharePointToken()
        //{
        //    var client = BsdHttpClient.Instance();
        //    var request = new HttpRequestMessage(HttpMethod.Post, "https://login.microsoftonline.com/common/oauth2/token");
        //    var formContent = new FormUrlEncodedContent(new[]
        //        {
        //                new KeyValuePair<string, string>("client_id", "2ad88395-b77d-4561-9441-d0e40824f9bc"),
        //                new KeyValuePair<string, string>("username", OrgConfig.Username),
        //                new KeyValuePair<string, string>("password", OrgConfig.Password),
        //                new KeyValuePair<string, string>("grant_type", "password"),
        //                new KeyValuePair<string, string>("resource", OrgConfig.SharePointResource)
        //            });
        //    request.Content = formContent;
        //    var response = await client.SendAsync(request);
        //    var body = await response.Content.ReadAsStringAsync();
        //    GetTokenResponse tokenData = JsonConvert.DeserializeObject<GetTokenResponse>(body);
        //    return tokenData;
        //}

        private async void LstVideo_ItemTapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            var item = (SharePointFile)e.Item;
            var detailPage = new UnitVideoGalleryDetail(item.Name, Folder, Category);
            this.listview.SelectedItem = null;
            await Navigation.PushAsync(detailPage);
        }
    }
}
