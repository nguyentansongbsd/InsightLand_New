using ConasiCRM.Portable.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

using Xamarin.Forms;

namespace ConasiCRM.Portable.Models
{
    public class ContactMandatoryPrimary : BaseViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Nameen { get; set; }

        private string _fullname;
        public string fullname { get { return _fullname; } set { _fullname = value; OnPropertyChanged(nameof(fullname)); } }

        private string _contactid;
        public string contactid { get { return _contactid; } set { _contactid = value; OnPropertyChanged(nameof(contactid)); } }
    }
}

