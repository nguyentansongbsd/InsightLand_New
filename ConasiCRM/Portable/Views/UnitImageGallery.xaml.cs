using ConasiCRM.Portable.Config;
using ConasiCRM.Portable.Controls;
using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.ViewModels;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConasiCRM.Portable.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UnitImageGallery : ContentPage
    {
        public Action<bool> OnCompleted;
        public UnitImageGalleryViewModel viewModel;
        public string Folder { get; set; }
        public string Category { get; set; }


        public UnitImageGallery(string category, string unitId, string unitName, string title)
        {
            InitializeComponent();
            this.BindingContext = viewModel = new UnitImageGalleryViewModel();
            LoadFromServer();
            this.Title = title;

            Category = category;
            Folder = unitName.Replace('.','-') + "_" + unitId.Replace("-", string.Empty).ToUpper();          
          //  this.listView.LayoutDefinition.BindingContext = viewModel;

            //SlideView.SelectedIndex = -1;
        }

        public async Task LoadFromServer()
        {
            GetTokenResponse getTokenResponse = await CrmHelper.getSharePointToken();
            var client = BsdHttpClient.Instance();
            string fileListUrl = $"{OrgConfig.SharePointResource}/sites/{OrgConfig.SharePointSiteName}/_api/web/Lists/GetByTitle('" + Category + "')/RootFolder/Folders('" + Folder + "')/Files";
            var request = new HttpRequestMessage(HttpMethod.Get, fileListUrl);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", getTokenResponse.access_token);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                SharePointFieldResult sharePointFieldResult = JsonConvert.DeserializeObject<SharePointFieldResult>(body);
                if (sharePointFieldResult != null)
                { 
                    var list = sharePointFieldResult.value;
                    foreach (var item in list)
                    {
                        if (item.Name.ToLower().Split('.')[1] == "jpg" || item.Name.ToLower().Split('.')[1] == "jpeg" || item.Name.ToLower().Split('.')[1] == "png")
                        {
                            try
                            {
                                string category_value = "";
                                switch (Category)
                                {
                                    case "Units": category_value = "product";
                                        break;
                                    case "Project": category_value = "bsd_project";
                                        break;

                                }

                                var fileRequest = new HttpRequestMessage(HttpMethod.Get, OrgConfig.SharePointResource
                                + "/sites/" + OrgConfig.SharePointSiteName + "/_api/web/GetFileByServerRelativeUrl('/sites/" + OrgConfig.SharePointSiteName + "/" + category_value + "/" + Folder + "/"
                                    + HttpUtility.UrlEncode(item.Name) + "')/$value");
                                var fileResponse = await client.SendAsync(fileRequest);

                                var str = await fileResponse.Content.ReadAsByteArrayAsync();
                                item.ImageSource = ImageSource.FromStream(() => new MemoryStream(str));
                                viewModel.ImageList.Add(item);                                
                            }
                            catch (Exception ex)
                            {
                                string mess = ex.Message;
                            }
                        }
                    }
                    OnCompleted?.Invoke(true);
                }
            }
            else
            {
                OnCompleted?.Invoke(false);
            }
            if(viewModel.ImageList.Count == 0) { EmptyText.IsVisible = true; }
            viewModel.IsBusy = false;
        }      
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

        //private async void LstImage_ItemTapped(object sender, Telerik.XamarinForms.DataControls.ListView.ItemTapEventArgs e)
        //{
        //    //this.addZoomViewToDetailImage();
        //    var item = (SharePointFile)e.Item;
        //    var newIndex = viewModel.ImageList.IndexOf(item);

        //    this.SlideView.SelectedIndex = newIndex;

        //    //this.SlideView.SlidedToIndex += (sender2, e2) => {
        //    //    if (e2.Index == newIndex)
        //    //    {
        //    //        this.modalImage.IsVisible = true;
        //    //    }
        //    //};
        //    await Task.Delay(300);
        //    NavigationPage.SetHasNavigationBar(this, false);
        //    this.modalImage.IsVisible = true;

        //}

        async void LstImage_ItemTapped(object sender, System.EventArgs e)
        {
            var grid = sender as Grid;
            var tapGes = (TapGestureRecognizer)grid.GestureRecognizers[0];
            var item = (SharePointFile)tapGes.CommandParameter;

            var newIndex = viewModel.ImageList.IndexOf(item);

            this.SlideView.SelectedIndex = newIndex;

            //this.SlideView.SlidedToIndex += (sender2, e2) => {
            //    if (e2.Index == newIndex)
            //    {
            //        this.modalImage.IsVisible = true;
            //    }
            //};         
            NavigationPage.SetHasNavigationBar(this, false);
            this.modalImage.IsVisible = true;
        }

        private void BtnCloseModalImage_Clicked(object sender, EventArgs e)
        {
            NavigationPage.SetHasNavigationBar(this, true);
            this.modalImage.IsVisible = false;
            //SlideView.SelectedIndex = -1;
            //SlideView.ItemsSource = viewModel.ImageList;

            //DetailZoom.resetZoom();
        }

        private void rightToLeft(object sender, SwipedEventArgs e)
        {
            var index = viewModel.ImageList.IndexOf(viewModel.CurrentImage);
            try
            {
                viewModel.CurrentImage = viewModel.ImageList[index + 1];
            }
            catch
            {

            }
        }

        private void leftToRight(object sender, SwipedEventArgs e)
        {
            var index = viewModel.ImageList.IndexOf(viewModel.CurrentImage);
            try
            {
                viewModel.CurrentImage = viewModel.ImageList[index - 1];
            }
            catch
            {

            }
        }

        //protected override void OnSizeAllocated(double width, double height)
        //{
        //    var mainDisplayInfo = DeviceDisplay.MainDisplayInfo;
        //    var a = (mainDisplayInfo.Width / mainDisplayInfo.Density) / 3;
        //}



        void Handle_LayoutChanged(object sender, System.EventArgs e)
        {
            viewModel.displayInfo = DeviceDisplay.MainDisplayInfo;
        }

        protected override bool OnBackButtonPressed()
        {
            if (this.modalImage.IsVisible)
            {
                this.BtnCloseModalImage_Clicked(null, null);
                return true;
            }
            return base.OnBackButtonPressed();
        }

        void Handle_OnZoomUpdated(object sender, System.EventArgs e)
        {
            if((sender as PinchToZoomContainer).isZooming)
            {
                SlideView.IsSwipingEnabled = false;
                SlideView.ShowButtons = false;
            }
            else
            {
                SlideView.IsSwipingEnabled = true;
                SlideView.ShowButtons = true;
            }
        }

        //private void addZoomViewToDetailImage()
        //{
        //    SlideView.ItemTemplate = null;

        //    SlideView.ItemTemplate = new DataTemplate(() => {
        //        var image = new Image();
        //        image.SetBinding(Image.SourceProperty, "ImageSource");

        //        PinchToZoomContainer a = new PinchToZoomContainer();
        //        a.Content = image;

        //        return new Grid() { Children = { a,} };
        //    });

        //}
    }


}