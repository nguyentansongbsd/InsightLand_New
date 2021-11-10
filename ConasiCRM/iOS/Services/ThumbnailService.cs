using System;
using System.IO;
using System.Threading.Tasks;
using AVFoundation;
using ConasiCRM.iOS.Services;
using ConasiCRM.Portable.IServices;
using CoreGraphics;
using CoreMedia;

using Foundation;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(ThumbnailService))]
namespace ConasiCRM.iOS.Services
{
    public class ThumbnailService : IThumbnailService
    {
        TaskCompletionSource<ImageSource> taskCompletionSource;
        public Task<ImageSource> GetImageSourceAsync(string url)
        {
            taskCompletionSource = new TaskCompletionSource<ImageSource>();
            CoreMedia.CMTime actualTime;
            NSError outError;
            using (var asset = AVAsset.FromUrl(NSUrl.FromString(url)))
            using (var imageGen = new AVAssetImageGenerator(asset))
            using (var imageRef = imageGen.CopyCGImageAtTime(new CoreMedia.CMTime(5000, 1), out actualTime, out outError))
            {
                if (imageRef == null)
                    return null;
                var image = UIImage.FromImage(imageRef);

                //Stream imagestream = image.AsJPEG(1).AsStream();
                ImageSource imageSource = ImageSource.FromStream(() => image.AsPNG().AsStream());
                taskCompletionSource.SetResult(imageSource);
            }

            return taskCompletionSource.Task;
        }
    }
}
