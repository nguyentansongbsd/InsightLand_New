using ConasiCRM.Portable.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

using Xamarin.Forms;

namespace ConasiCRM.Portable.Models
{
    public class ListUnitCase : BaseViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Nameen { get; set; }

        private string _name;
        public string name { get { return _name; } set { _name = value; OnPropertyChanged(nameof(name)); } }

        private string _productid;
        public string productid { get { return _productid; } set { _productid = value; OnPropertyChanged(nameof(productid)); } }
    }
}

