using Android.Graphics;
using Android.Media;
using Android.OS;
using ConasiCRM.Droid.Services;
using ConasiCRM.Portable.IServices;
using Java.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(ThumbnailService))]
namespace ConasiCRM.Droid.Services
{
    public class ThumbnailService : IThumbnailService
    {
        public async Task<ImageSource> GetImageSourceAsync(string url)
        {
            MediaMetadataRetriever retriever = new MediaMetadataRetriever();
            if ((int)Build.VERSION.SdkInt >=16)
            {
                retriever.SetDataSource(url, new Dictionary<string, string>());
            }
            else
            {
                retriever.SetDataSource(url);
            }
            
            Bitmap bitmap = retriever.GetFrameAtTime(5000, Option.Closest);
            if (bitmap != null)
            {
                MemoryStream stream = new MemoryStream();
                bitmap.Compress(Bitmap.CompressFormat.Png, 0, stream);
                byte[] bitmapData = stream.ToArray();
                ImageSource imageSource = ImageSource.FromStream(() => new MemoryStream(bitmapData));
                return imageSource;
            }
            return null;
        }
    }
}
