using ConasiCRM.Portable.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

using Xamarin.Forms;

namespace ConasiCRM.Portable.Models
{
    public class ListCountry : BaseViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Nameen { get; set; }

        private string _bsd_name;
        public string bsd_name { get { return _bsd_name; } set { _bsd_name = value; OnPropertyChanged(nameof(bsd_nameen)); } }

        private string _bsd_countryname;
        public string bsd_countryname { get { return _bsd_countryname; } set { _bsd_countryname = value; OnPropertyChanged(nameof(bsd_countryname)); } }

        private string _bsd_nameen;
        public string bsd_nameen { get { return _bsd_nameen; } set { _bsd_nameen = value; OnPropertyChanged(nameof(bsd_nameen)); } }

        private string _bsd_countryid;
        public string bsd_countryid { get { return _bsd_countryid; } set { _bsd_countryid = value; OnPropertyChanged(nameof(bsd_countryid)); } }

    }
}