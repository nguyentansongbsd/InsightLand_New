using ConasiCRM.Portable.Config;
using ConasiCRM.Portable.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ConasiCRM.Portable.Models
{
    public class SharePointFile : BaseViewModel
    {
        private ImageSource imageSource;
        public ImageSource ImageSource
        {
            get => imageSource;
            set
            {
                imageSource = value;
                OnPropertyChanged(nameof(ImageSource));
            }
        }
        //public string CheckInComment { get; set; }
        //public int CheckOutType { get; set; }
        //public string ContentTag { get; set; }
        //public int CustomizedPageStatus { get; set; }
        //public string ETag { get; set; }
        //public bool Exists { get; set; }
        //public bool IrmEnabled { get; set; }
        //public string Length { get; set; }
        //public int Level { get; set; }
        //public object LinkingUri { get; set; }
        //public string LinkingUrl { get; set; }
        //public int MajorVersion { get; set; }
        //public int MinorVersion { get; set; }
        public string Name { get; set; }
        public string ServerRelativeUrl { get; set; }

        //public DateTime TimeCreated { get; set; }
        //public DateTime TimeLastModified { get; set; }
        //public object Title { get; set; }
        //public int UIVersion { get; set; }
        //public string UIVersionLabel { get; set; }
        //public string UniqueId { get; set; }
    }

    public class SharePointFieldResult
    {
        public List<SharePointFile> value { get; set; }
    }
}
