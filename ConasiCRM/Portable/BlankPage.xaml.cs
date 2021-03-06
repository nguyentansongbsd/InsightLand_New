using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using ConasiCRM.Portable.Config;
using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.IServices;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.Resources;
using ConasiCRM.Portable.Settings;
using ConasiCRM.Portable.ViewModels;
using FFImageLoading;
using FFImageLoading.Config;
using FFImageLoading.Forms;
using FormsVideoLibrary;
using Newtonsoft.Json;
using Stormlion.PhotoBrowser;
using Xamarin.CommunityToolkit.Core;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;

namespace ConasiCRM.Portable
{
    public class AuthenticatedHttpImageClientHandler : HttpClientHandler
    {
        private readonly string _getToken;

        public AuthenticatedHttpImageClientHandler(string getToken)
        {
            if (getToken == null) throw new ArgumentNullException("getToken");
            _getToken = getToken;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Add("Authorization", "Bearer " + _getToken);
            return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }

        
    }

    public partial class BlankPage : ContentPage
    {
        public DateTime? date { get; set; }
        public ViewModel viewModel;

        public BlankPage()
        {

            InitializeComponent();
            //this.BindingContext = this;
            this.BindingContext = viewModel = new ViewModel();
            //media1.Source = MediaSource.FromUri("https://firebasestorage.googleapis.com/v0/b/gglogin-c3e8a.appspot.com/o/Screen%20-%20CNS%20-%20Figma%202021-07-20%2016-28-24.mp4?alt=media&token=4a31d437-ffe2-4a98-8ac3-e39a6ce57fd3");
            //  media1.Source = MediaSource.FromUri("https://www.deviantart.com/sakimichan/art/Ahri-D-vafied-nsfw-optional-681732764");
            //image.Source = "https://raw.githubusercontent.com/stfalcon-studio/FrescoImageViewer/v.0.5.0/images/posters/Vincent.jpg";
            //  var a = media1.CurrentState;
            Init();

        }

        public async void Init()
        {

            this.BindingContext = this;
            

            //string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
            //                      <entity name='sharepointdocument'>
            //                        <attribute name='documentid' />
            //                        <attribute name='sharepointdocumentid' />
            //                        <attribute name='absoluteurl' />
            //                        <attribute name='fullname' />
            //                        <attribute name='filetype' />
            //                        <attribute name='relativelocation' />
            //                        <attribute name='author' />
            //                        <order attribute='relativelocation' descending='false' />
            //                        <link-entity name='bsd_project' from='bsd_projectid' to='regardingobjectid' link-type='inner' alias='ad'>
            //                          <filter type='and'>
            //                            <condition attribute='bsd_projectid' operator='eq' value='A7ABBBAF-0A2C-EC11-B6E6-000D3A80FA69' />
            //                          </filter>
            //                        </link-entity>
            //                      </entity>
            //                    </fetch>";
            //var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<SharePonitModel>>("sharepointdocuments", fetchXml);

            //var Category = "Project";
            //var category_value = "bsd_project";
            //List<SharePonitModel> list = result.value;

            //var videos = list.Where(x => x.filetype == "mp4" || x.filetype == "flv" || x.filetype == "m3u8" || x.filetype == "3gp" || x.filetype == "mov" || x.filetype == "avi" || x.filetype == "wmv").ToList();

            //byte[] resultImg;
            //for (int i = 0; i < videos.Count; i++)
            //{
            //    resultImg = await CrmHelper.RetrieveImagesSharePoint<byte[]>($"{OrgConfig.SharePointSiteName}/{category_value}/{videos[i].relativelocation}");
            //}


            //string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
            //                      <entity name='sharepointdocument'>
            //                        <attribute name='documentid' />
            //                        <attribute name='sharepointdocumentid' />
            //                        <attribute name='absoluteurl' />
            //                        <attribute name='fullname' />
            //                        <attribute name='filetype' />
            //                        <attribute name='relativelocation' />
            //                        <attribute name='author' />
            //                        <order attribute='relativelocation' descending='false' />
            //                        <link-entity name='bsd_project' from='bsd_projectid' to='regardingobjectid' link-type='inner' alias='ad'>
            //                          <filter type='and'>
            //                            <condition attribute='bsd_projectid' operator='eq' value='A7ABBBAF-0A2C-EC11-B6E6-000D3A80FA69' />
            //                          </filter>
            //                        </link-entity>
            //                      </entity>
            //                    </fetch>";
            //var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<SharePonitModel>>("sharepointdocuments", fetchXml);

            //List<SharePonitModel> data = result.value;

            //HttpClient httpClient = new HttpClient();
            //ImageService.Instance.Initialize(new Configuration
            //{
            //    HttpClient = new HttpClient(new AuthenticatedHttpImageClientHandler(UserLogged.AccessTokenSharePoint))
            //});


            //test.Url = "https://images.unsplash.com/photo-1453728013993-6d66e9c9123a?ixid=MnwxMjA3fDB8MHxzZWFyY2h8Mnx8dmlld3xlbnwwfHwwfHw%3D&ixlib=rb-1.2.1&w=1000&q=80"; //data[0].absoluteurl;
            //test.Token = UserLogged.AccessTokenSharePoint;

            //test.Source = ImageService.Instance.LoadUrl(data[0].absoluteurl).Path;

            //ImageService.Instance.Initialize(new Configuration
            //{
            //    HttpClient = NetworkHelper.GetAuthenticatedHttpClient(Constants.__IOS__)
            //});
            //ImageService.Instance.LoadUrl(url).Into(imageView);


            //string url = "https://conasivn.sharepoint.com/sites/Conasi/_layouts/15/download.aspx?SourceUrl=/sites/Conasi/bsd_project/THẢO ĐIỀN GREEN_1F0E1C763DE5EB11BACB00224816626E/Condotel Ariyana Da Nang.mp4&access_token=eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsIng1dCI6Imwzc1EtNTBjQ0g0eEJWWkxIVEd3blNSNzY4MCIsImtpZCI6Imwzc1EtNTBjQ0g0eEJWWkxIVEd3blNSNzY4MCJ9.eyJhdWQiOiJodHRwczovL2NvbmFzaXZuLnNoYXJlcG9pbnQuY29tIiwiaXNzIjoiaHR0cHM6Ly9zdHMud2luZG93cy5uZXQvYjhmZjFkMmUtMjhiYS00NGU2LWJmNWItYzk2MTg4MTk2NzExLyIsImlhdCI6MTYzNjQyNDcwMiwibmJmIjoxNjM2NDI0NzAyLCJleHAiOjE2MzY0Mjg2MDIsImFjciI6IjEiLCJhaW8iOiJFMlpnWUpBcjhEUjdxU21lcmZ0dmhwM3lMaVhQYXcvak9MY0gzOUQ4L2VtcFFyckk1YThBIiwiYW1yIjpbInB3ZCJdLCJhcHBfZGlzcGxheW5hbWUiOiJEeW5hbWljcyAzNjUgRGV2ZWxvcG1lbnQgVG9vbHMiLCJhcHBpZCI6IjJhZDg4Mzk1LWI3N2QtNDU2MS05NDQxLWQwZTQwODI0ZjliYyIsImFwcGlkYWNyIjoiMCIsImdpdmVuX25hbWUiOiJic2QiLCJpZHR5cCI6InVzZXIiLCJpcGFkZHIiOiIxMTYuOTkuMTQwLjk3IiwibmFtZSI6IkNvbmcgdHkgQlNEIiwib2lkIjoiNTkwOWUzZGItZjhhMy00NTA2LWI1OGYtZGE0ODdmZjAxZDBhIiwicHVpZCI6IjEwMDMyMDAwMzcwMDcxMDUiLCJyaCI6IjAuQVQ0QUxoM191TG9vNWtTX1c4bGhpQmxuRVpXRDJDcDl0MkZGbEVIUTVBZ2stYnctQUlRLiIsInNjcCI6InVzZXJfaW1wZXJzb25hdGlvbiIsInNpZCI6IjExMTNkYmM3LTllYzAtNDUxNC05OWZhLWE4YmY3ZjczNTRhZSIsInN1YiI6IkZzaUJTZTFLelhUNmpNTkhScHQ0aGNFczhkZjExNE95MjdzZTdYTDM1SUEiLCJ0aWQiOiJiOGZmMWQyZS0yOGJhLTQ0ZTYtYmY1Yi1jOTYxODgxOTY3MTEiLCJ1bmlxdWVfbmFtZSI6ImJzZGRldkBjb25hc2kudm4iLCJ1cG4iOiJic2RkZXZAY29uYXNpLnZuIiwidXRpIjoiWWJ0VzVPbi1FazJvQm5uZDFVTUVBQSIsInZlciI6IjEuMCIsIndpZHMiOlsiYjc5ZmJmNGQtM2VmOS00Njg5LTgxNDMtNzZiMTk0ZTg1NTA5Il19.pYOUAXeWsVBOsmxQgiam2R3P_0N5L6hrmZOC1lXNm9seToWSQVtKZC4wx2HEqD5ocDGLf3mUp73CZgFCT7XFV4XEhDIeu8Cuh941BspT8iaJqAxIIQV9vsWaTsJVrd5jR9v2I09TmHQr5AQTCeio_86njG6jjmjU7qGLBdo-FTdhoeAiZ8ei4kM5JcXvjAD3h1f4olfdhXkMfdMyS-fqIT62-O15r0bJiPrehs_LYehdggTdJSA13wqY-Q3xCETTz9r7JHNCiK0gh_L54rTX6eQe6FmgmC7zY_kxSLccK4pqdJN8HNTxhgzIbksakkn_b1yZbAOh092Fs3dH6G9JQA";
            //ImageSource imageSource = await DependencyService.Get<IThumbnailService>().GetImageSourceAsync(url);

        }

        async void Button_Clicked_2(System.Object sender, System.EventArgs e)
        {
            //await DisplayAlert("", date.ToString(), "ok");
            
        }

        private async void Meida_Tapped(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            Grid mediaElement = (Grid)sender;
            var a = (TapGestureRecognizer)mediaElement.GestureRecognizers[0];
            CollectionData item = a.CommandParameter as CollectionData;
            if (item != null)
            {
                //LoadingHelper.Show();
                //await Navigation.PushAsync(new ShowMedia(item.MediaSource));
                //LoadingHelper.Hide();              
            }
        }

        private void Image_Tapped(object sender, EventArgs e)
        {
            CachedImage image = (CachedImage)sender;
            var a = (TapGestureRecognizer)image.GestureRecognizers[0];
            CollectionData item = a.CommandParameter as CollectionData;
            if (item != null)
            {
                //viewModel.photoBrowser.StartIndex = item.Index;
                //viewModel.photoBrowser.Show();
            }
        }

        private void MediaElement_MediaOpened(object sender, EventArgs e)
        {
            //viewModel.OnComplate = false;
        }

        private void MediaElement_MediaFailed(object sender, EventArgs e)
        {
            Grid mediaElement = (Grid)sender;
            var a = (TapGestureRecognizer)mediaElement.GestureRecognizers[0];
            CollectionData item = a.CommandParameter as CollectionData;
            if (item != null)
            {
                //viewModel.Data.Remove(item);
            }
        }

        private async void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            var response = await LoginHelper.Login();
            if (response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                GetTokenResponse tokenData = JsonConvert.DeserializeObject<GetTokenResponse>(body);
            }
        }

        void Button_Clicked_1(System.Object sender, System.EventArgs e)
        {
            //var a = mydate;
        }

        private void Image_Pinching(object sender, MR.Gestures.PinchEventArgs e)
        {
        }

        private void Image_Swiped(object sender, MR.Gestures.SwipeEventArgs e)
        {

        }

        private void Image_Up(object sender, MR.Gestures.DownUpEventArgs e)
        {

        }

        private void Image_Down(object sender, MR.Gestures.DownUpEventArgs e)
        {

        }

        private void Image_Pinched(object sender, MR.Gestures.PinchEventArgs e)
        {
           var a = e.DistanceX;
            var b = e.DistanceY;
            var c = e.DeltaScaleX;
            var d = e.DeltaScaleX;
            var f = e.DeltaScale;
            var g = e.Distance;
            var l = e.TotalScale;
            var p = e.TotalScaleX;
            var o = e.TotalScaleY;
            var t = e.Center;
        }

        private void ContentView_Swiped(object sender, MR.Gestures.SwipeEventArgs e)
        {
            //if (img.Scale == 1)
            //{

            //}
        }
    }
    public class CategoricalData
    {
        public object Category { get; set; }

        public double Value { get; set; }
    }
    public class ViewModel : BaseViewModel
    {
      //  public ObservableCollection<ChartModel> CommissionTransactionChart { get; set; } = new ObservableCollection<ChartModel>();
        public ObservableCollection<CollectionData> Data { get; set; }
        public List<Photo> Photos;
        public List<Photo> Media;
        public PhotoBrowser photoBrowser;

        private ObservableCollection<ChartModel> _commissionTransactionChart;
        public ObservableCollection<ChartModel> CommissionTransactionChart { get => _commissionTransactionChart; set { _commissionTransactionChart = value; OnPropertyChanged(nameof(CommissionTransactionChart)); } }
        private DateTime _dateBefor;
        public DateTime dateBefor { get => _dateBefor; set { _dateBefor = value; OnPropertyChanged(nameof(dateBefor)); } }
        public DateTime dateAfter { get; set; }
        public DateTime firstMonth { get; set; }
        public DateTime secondMonth { get; set; }
        public DateTime thirdMonth { get; set; }
        public DateTime fourthMonth { get; set; }

        public ViewModel()
        {
            CommissionTransactionChart = new ObservableCollection<ChartModel>();
            dateBefor = DateTime.Now;
            DateTime threeMonthsAgo = DateTime.Now.AddMonths(-3);
            dateAfter = new DateTime(threeMonthsAgo.Year, threeMonthsAgo.Month, 1);
            firstMonth = dateAfter;
            secondMonth = dateAfter.AddMonths(1);
            thirdMonth = secondMonth.AddMonths(1);
            fourthMonth = dateBefor;

            ChartModel chartFirstMonth = new ChartModel() { Category = firstMonth.ToString("MM/yyyy"), Value = 100000 };
            ChartModel chartSecondMonth = new ChartModel() { Category = secondMonth.ToString("MM/yyyy"), Value = 300000 };
            ChartModel chartThirdMonth = new ChartModel() { Category = thirdMonth.ToString("MM/yyyy"), Value = 200000 };
            ChartModel chartFourthMonth = new ChartModel() { Category = fourthMonth.ToString("MM/yyyy"), Value = 150000 };

            ObservableCollection<ChartModel> list = new ObservableCollection<ChartModel>();
            list.Add(chartFirstMonth);
            list.Add(chartSecondMonth);
            list.Add(chartThirdMonth);
            list.Add(chartFourthMonth);

            this.CommissionTransactionChart = list;
            //this.Data = new ObservableCollection<CollectionData>();
            //this.Data = GetCollectionData();
            //OnComplate = true;
            //photoBrowser = new PhotoBrowser
            //{
            //    Photos = Photos,
            //};
        }
    }
}
