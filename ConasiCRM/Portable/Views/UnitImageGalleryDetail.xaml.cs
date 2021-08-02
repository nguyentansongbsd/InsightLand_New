using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConasiCRM.Portable.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UnitImageGalleryDetail : ContentPage
    {
        public UnitImageGalleryDetail(string url)
        {
            InitializeComponent();
            image.Source = url;
        }
    }
}