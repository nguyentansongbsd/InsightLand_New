using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ConasiCRM.Portable.Models
{
    public class CollectionData
    {
        public string MediaSource { get; set; }
        public ImageSource ImageSource { get; set; }
        public SharePointType SharePointType { get; set; }
        public int Index { get; set; }
        public CollectionData()
        { }
    }
    public enum SharePointType
    {
        Video,
        Image
    }
}
