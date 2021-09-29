using ConasiCRM.Portable.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ConasiCRM.Portable.Helper
{
    public class CollectionDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate Image { get; set; }
        public DataTemplate Media { get; set; }
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            return ((CollectionData)item).MediaSource != null ? Media : Image;
        }
    }
}
