using System;
using System.Collections.ObjectModel;
using ConasiCRM.Portable.Models;
using FormsVideoLibrary;
using Xamarin.Essentials;

namespace ConasiCRM.Portable.ViewModels
{
    public class UnitVideoGalleryViewModel : BaseViewModel
    {
        public ObservableCollection<SharePointFile> VideoList { get; set; }


        public UnitVideoGalleryViewModel()
        {
            IsBusy = true;
            VideoList = new ObservableCollection<SharePointFile>();
        }
    }
}
