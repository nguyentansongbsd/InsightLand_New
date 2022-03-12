using System;
using ConasiCRM.iOS.Renderers;
using ConasiCRM.Portable;
using ConasiCRM.Portable.Controls;
using FFImageLoading;
using FFImageLoading.Config;
using FFImageLoading.Forms;
using Ricardo.SDWebImage.iOS;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ImageControl),typeof(ImageCustomRenderer))]
namespace ConasiCRM.iOS.Renderers
{
    public class ImageCustomRenderer : ImageRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Image> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null)
            {
                var image =  (ImageControl)e.NewElement;
                //ImageService.Instance.Initialize(new Configuration
                //{
                //    HttpClient = new HttpClient(new AuthenticatedHttpImageClientHandler(image.Token)
                //});
                ImageService.Instance.LoadUrl(image.Url).Into(Control);
            }
        }
    }
}
