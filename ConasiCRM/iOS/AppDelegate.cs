using System;
using System.Collections.Generic;
using System.Linq;
using ConasiCRM.Portable.IServices;
using FFImageLoading;
using FFImageLoading.Config;
using FFImageLoading.Forms.Platform;
using Foundation;
using MediaManager;
using UIKit;
using Xamarin.Forms;

namespace ConasiCRM.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            //Rg.Plugins.Popup.Popup.Init();

            global::Xamarin.Forms.Forms.Init();
            Stormlion.PhotoBrowser.iOS.Platform.Init();
            CrossMediaManager.Current.Init();
            CachedImageRenderer.Init();
            LoadApplication(new Portable.App());
            DependencyService.Get<ILoadingService>().Initilize();

            return base.FinishedLaunching(app, options);
        }
    }
}
