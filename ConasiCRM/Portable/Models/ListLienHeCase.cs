using ConasiCRM.Portable.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

using Xamarin.Forms;

namespace ConasiCRM.Portable.Models
{
    public class ListLienHeCase : BaseViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Nameen { get; set; }

        private string _bsd_fullname;
        public string bsd_fullname { get { return _bsd_fullname; } set { _bsd_fullname = value; OnPropertyChanged(nameof(bsd_fullname)); } }

        private string _contactid;
        public string contactid { get { return _contactid; } set { _contactid = value; OnPropertyChanged(nameof(contactid)); } }
    }
}


