using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConasiCRM.Portable.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PhotoShow : ContentPage
    {
        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource), typeof(ObservableCollection<PhotoCMND>), typeof(LookUp), null, BindingMode.TwoWay, null);
        public ObservableCollection<PhotoCMND> ItemsSource { get => (ObservableCollection<PhotoCMND>)GetValue(ItemsSourceProperty); set { SetValue(ItemsSourceProperty, value); } }

        public PhotoShow(ObservableCollection<PhotoCMND> list_image, int i)
        {
            InitializeComponent();
            this.BindingContext = this;
            if(list_image != null && list_image.Count>0)
            {
                ItemsSource = list_image;
                if (i >= 0 && i < ItemsSource.Count)
                    carousel.ScrollTo(i);
            }
        }    

        public async void Show(Page view)
        {
            if (ItemsSource != null)
            {
                await view.Navigation.PushModalAsync(this);
            }
        }

        private void PinchZoomImage_OnZoom(object sender, EventArgs e)
        {
            carousel.IsSwipeEnabled = false;
        }

        private void PinchZoomImage_OutZoom(object sender, EventArgs e)
        {
            carousel.IsSwipeEnabled = true;
        }

        // chưa sử dụng được
        //private async void SwipeGestureRecognizer_Swiped(object sender, SwipedEventArgs e)
        //{
        //    FFImageLoading.Forms.CachedImage cachedImage = sender as FFImageLoading.Forms.CachedImage;
        //    if (cachedImage.Scale == 1)
        //    {
        //        await carousel.TranslateTo(0, 100, 10);
        //        await Navigation.PopModalAsync();
        //    }
        //}
    } 
}