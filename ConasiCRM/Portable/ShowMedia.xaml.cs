using ConasiCRM.Portable.Helper;
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
        public ShowMedia( string MediaSource)
        {
            InitializeComponent();
            if(mediaShow!=null)
            {
                LoadingHelper.Show();
                mediaShow.Source = MediaSource;
            }    
        }

        private void mediaShow_MediaOpened(object sender, EventArgs e)
        {
            LoadingHelper.Hide();
        }
    }
}