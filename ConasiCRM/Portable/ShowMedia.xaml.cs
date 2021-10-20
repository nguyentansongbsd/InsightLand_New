using ConasiCRM.Portable.Helper;
using MediaManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConasiCRM.Portable
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShowMedia : ContentPage
    {
        public ShowMedia(string MediaSource)
        {
            InitializeComponent();
            if(videoView != null)
            {
                LoadingHelper.Show();
                videoView.Source = MediaSource;
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