using System;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Threading.Tasks;
using Android.Content;
using System.IO;
using ConasiCRM.Droid;
using Xamarin.Forms;
using ConasiCRM.Portable.IServices;
using MediaManager;
using FFImageLoading;
using Android.Content.Res;
using ConasiCRM.Portable;

namespace ConasiCRM.Android
{
    [Activity(Label = "ConasiCRM", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = ConasiCRM.Droid.Resource.Layout.Tabbar;
            ToolbarResource = ConasiCRM.Droid.Resource.Layout.Toolbar;          

            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            Stormlion.PhotoBrowser.Droid.Platform.Init(this);
            //CrossMediaManager.Current.Init(this);
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init(true);

            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            LoadApplication(new Portable.App());
            DependencyService.Get<ILoadingService>().Initilize();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, global::Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public static MainActivity Current { private set; get; }

        public static readonly int PickImageId = 1000;

        public TaskCompletionSource<string> PickImageTaskCompletionSource { set; get; }

        public TaskCompletionSource<Stream> PickImageCameraTaskCompletionSource { set; get; }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (requestCode == PickImageId)
            {
                if ((resultCode == Result.Ok) && (data != null))
                {
                    if (PickImageTaskCompletionSource != null)
                    {
                        // Set the filename as the completion of the Task
                        PickImageTaskCompletionSource.SetResult(data.DataString);
                    } else if(PickImageCameraTaskCompletionSource != null)
                    {
                        global::Android.Net.Uri uri = data.Data;
                        Stream stream = ContentResolver.OpenInputStream(uri);

                        PickImageCameraTaskCompletionSource.SetResult(stream);
                    }
                }
                else
                {
                    if(PickImageTaskCompletionSource != null)
                    {
                        PickImageTaskCompletionSource.SetResult(null);
                    }
                    else if(PickImageCameraTaskCompletionSource != null)
                    {
                        PickImageCameraTaskCompletionSource.SetResult(null);
                    }
                }
            }
        }
    }
}