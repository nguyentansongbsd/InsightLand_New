using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ConasiCRM.Portable.Models
{
    public class CollectionData
    {
        public string MediaSource { get; set; }
        public ImageSource PosterMediaSource { get; set; }
        public string ImageSource { get; set; }
        public int Index { get; set; }
        public CollectionData()
        { }
    }
}
