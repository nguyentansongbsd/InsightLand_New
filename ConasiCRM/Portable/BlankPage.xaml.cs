using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.ViewModels;
using FFImageLoading.Forms;
using FormsVideoLibrary;
using Newtonsoft.Json;
using Stormlion.PhotoBrowser;
using Xamarin.CommunityToolkit.Core;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;

namespace ConasiCRM.Portable
{
    public partial class BlankPage : ContentPage
    {
        public ViewModel viewModel;
        public DateTime? mydate { get; set; }
        public BlankPage()
        {
            InitializeComponent();
            //this.BindingContext = this;
            this.BindingContext = viewModel = new ViewModel();
            //media1.Source = MediaSource.FromUri("https://firebasestorage.googleapis.com/v0/b/gglogin-c3e8a.appspot.com/o/Screen%20-%20CNS%20-%20Figma%202021-07-20%2016-28-24.mp4?alt=media&token=4a31d437-ffe2-4a98-8ac3-e39a6ce57fd3");
            //  media1.Source = MediaSource.FromUri("https://www.deviantart.com/sakimichan/art/Ahri-D-vafied-nsfw-optional-681732764");
            //image.Source = "https://raw.githubusercontent.com/stfalcon-studio/FrescoImageViewer/v.0.5.0/images/posters/Vincent.jpg";
            //  var a = media1.CurrentState;

        }

        private async void Meida_Tapped(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            Grid mediaElement = (Grid)sender;
            var a = (TapGestureRecognizer)mediaElement.GestureRecognizers[0];
            CollectionData item = a.CommandParameter as CollectionData;
            if (item != null)
            {
                LoadingHelper.Show();
                await Navigation.PushAsync(new ShowMedia(item.MediaSource));
                LoadingHelper.Hide();              
            }
        }

        private void Image_Tapped(object sender, EventArgs e)
        {
            CachedImage image = (CachedImage)sender;
            var a = (TapGestureRecognizer)image.GestureRecognizers[0];
            CollectionData item = a.CommandParameter as CollectionData;
            if (item != null)
            {
               viewModel.photoBrowser.StartIndex = item.Index;
               viewModel.photoBrowser.Show();
            }
        }

        private void MediaElement_MediaOpened(object sender, EventArgs e)
        {
            viewModel.OnComplate = false;
        }

        private void MediaElement_MediaFailed(object sender, EventArgs e)
        {
            Grid mediaElement = (Grid)sender;
            var a = (TapGestureRecognizer)mediaElement.GestureRecognizers[0];
            CollectionData item = a.CommandParameter as CollectionData;
            if (item != null)
            {
                viewModel.Data.Remove(item);               
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
            var a = mydate;
        }
    }
    public class CategoricalData
    {
        public object Category { get; set; }

        public double Value { get; set; }
    }
    public class ViewModel : BaseViewModel
    {
        public ObservableCollection<CollectionData> Data { get; set; }
        public List<Photo> Photos;
        public List<Photo> Media;
        public PhotoBrowser photoBrowser;

        private bool _onComplate;
        public bool OnComplate { get => _onComplate; set { _onComplate = value; OnPropertyChanged(nameof(OnComplate)); } }

        public ViewModel()
        {
            this.Data = new ObservableCollection<CollectionData>();         
            this.Data = GetCollectionData();
            OnComplate = true;
            photoBrowser = new PhotoBrowser
            {
                Photos = Photos,
            };
        }
        private ObservableCollection<CollectionData> GetCollectionData()
        {
            var list = new List<CollectionData>
            {
                new CollectionData { MediaSource = "https://sec.ch9.ms/ch9/5d93/a1eab4bf-3288-4faf-81c4-294402a85d93/XamarinShow_mid.mp4",ImageSource= null,Index = 1},
                new CollectionData { MediaSource = "https://sec.ch9.ms/ch9/5d93/a1eab4bf-3288-4faf-81c4-294402a85d93/XamarinShow_mid.mp4",ImageSource= null,Index = 2},
                new CollectionData { MediaSource = null,ImageSource="https://raw.githubusercontent.com/stfalcon-studio/FrescoImageViewer/v.0.5.0/images/posters/Vincent.jpg",Index = 1},
                new CollectionData { MediaSource = null,ImageSource="https://raw.githubusercontent.com/stfalcon-studio/FrescoImageViewer/v.0.5.0/images/posters/Vincent.jpg",Index = 2},
            };
            var data = new ObservableCollection<CollectionData>();
            Photos = new List<Photo>();
            Media = new List<Photo>();

            foreach(var item in list)
            {
                if (item.ImageSource != null)
                {
                    Photos.Add(new Photo { URL = item.ImageSource });
                    data.Add(item);
                }
                else
                {
                    Media.Add(new Photo { URL = item.ImageSource });
                    data.Add(item);
                }
            }                                 
            return data;
        }
    }
}
