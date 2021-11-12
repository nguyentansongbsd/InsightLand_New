using System;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ConasiCRM.Portable.IServices
{
    public interface IThumbnailService
    {
        Task<ImageSource> GetImageSourceAsync(string url);
        ImageSource GenerateThumbnailImageSource(string url, long usecond);
    }
}
