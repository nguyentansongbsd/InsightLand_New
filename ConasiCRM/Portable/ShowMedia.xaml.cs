using ConasiCRM.Portable.Helper;
using MediaManager;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Core;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConasiCRM.Portable
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShowMedia : ContentPage
    {
        public ShowMedia(string MediaSources)
        {
            InitializeComponent();
            if(videoView != null)
            {
                LoadingHelper.Show();
                MediaManager.Forms.VideoView video = new MediaManager.Forms.VideoView();
                MediaSource media ;
                StreamMediaSource stream;
                
                videoView.Source = MediaSources;
                LoadingHelper.Hide();
                
            }  
            else
            {
                 Navigation.PopAsync();
            }
            LoadingHelper.Hide();
        }      
    }
}