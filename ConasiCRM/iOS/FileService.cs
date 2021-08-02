using System;
using System.IO;
using ConasiCRM.iOS;
using ConasiCRM.Portable.Controls;
using Foundation;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(FileService))]
namespace ConasiCRM.iOS
{
    public class FileService : IFileService
    {
        public void SaveFile(string name, byte[] data, string location = "Download/Conasi")
        {
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            documentsPath = Path.Combine(documentsPath, location);
            Directory.CreateDirectory(documentsPath);

            string filePath = Path.Combine(documentsPath, name);

            //Java.IO.File sdCard = Environment.ExternalStorageDirectory;
            //Java.IO.File dir = new Java.IO.File(sdCard.AbsolutePath + "/" + location);
            //dir.Mkdirs();

            //var filePath = Path.Combine(dir.Path, name);

            using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate))
            {
                int length = data.Length;
                fs.Write(data, 0, length);
            }
        }

        public void OpenFile(string fileName, string location = "Download/Conasi")
        {
            //var fileExtension = System.IO.Path.GetExtension(fileName);

            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            documentsPath = Path.Combine(documentsPath, location);
            string filePath = Path.Combine(documentsPath, fileName);

            var PreviewController = UIDocumentInteractionController.FromUrl(NSUrl.FromFilename(filePath));
            PreviewController.Delegate = new UIDocumentInteractionControllerDelegateClass(UIApplication.SharedApplication.KeyWindow.RootViewController);
            Device.BeginInvokeOnMainThread(() =>
            {
                PreviewController.PresentPreview(true);
            });
        }
    }

    public class UIDocumentInteractionControllerDelegateClass : UIDocumentInteractionControllerDelegate
    {
        UIViewController ownerVC;

        public UIDocumentInteractionControllerDelegateClass(UIViewController vc)
        {
            ownerVC = vc;
        }

        public override UIViewController ViewControllerForPreview(UIDocumentInteractionController controller)
        {
            return ownerVC;
        }

        public override UIView ViewForPreview(UIDocumentInteractionController controller)
        {
            return ownerVC.View;
        }
    }
}
