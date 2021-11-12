using Android.Graphics;
using Android.Media;
using ConasiCRM.Droid.Services;
using ConasiCRM.Portable.IServices;
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
            try
            {
                MediaMetadataRetriever retriever = new MediaMetadataRetriever();
                retriever.SetDataSource(url, new Dictionary<string, string>());
                //await retriever.SetDataSourceAsync(url,new Dictionary<string,string>());

                Bitmap bitmap = retriever.GetFrameAtTime(5000);
                if (bitmap != null)
                {
                    MemoryStream stream = new MemoryStream();
                    bitmap.Compress(Bitmap.CompressFormat.Png, 0, stream);
                    byte[] bitmapData = stream.ToArray();
                    ImageSource imageSource = ImageSource.FromStream(() => new MemoryStream(bitmapData));
                    return imageSource;
                }
            }
            catch (Exception ex)
            {
                var a = ex.Message;
            }
            
            return null;
        }

        public ImageSource GenerateThumbnailImageSource(string url, long usecond)
        {
            // Extract thumbnail from video into a bitmap          
            MediaMetadataRetriever retriever = new MediaMetadataRetriever();
            retriever.SetDataSource(url,new Dictionary<string,string>());
            Bitmap bitmap = retriever.GetFrameAtTime(usecond);

            //Convert bitmap to a 'Stream' and then to an 'ImageSource' 
            if (bitmap != null)
            {
                MemoryStream stream = new MemoryStream();
                bitmap.Compress(Bitmap.CompressFormat.Png, 0, stream);
                byte[] bitmapData = stream.ToArray();
                return ImageSource.FromStream(() => new MemoryStream(bitmapData));
            }
            return null;
        }
    }
}
