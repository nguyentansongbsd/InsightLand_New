using ConasiCRM.Portable.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

using Xamarin.Forms;

namespace ConasiCRM.Portable.Models
{
    public class ListDistrict : BaseViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Nameen { get; set; }

        private string _new_name;
        public string new_name { get { return _new_name; } set { _new_name = value; OnPropertyChanged(nameof(new_name)); } }

        private string _bsd_nameen;
        public string bsd_nameen { get { return _bsd_nameen; } set { _bsd_nameen = value; OnPropertyChanged(nameof(bsd_nameen)); } }

        private string _new_districtid;
        public string new_districtid { get { return _new_districtid; } set { _new_districtid = value; OnPropertyChanged(nameof(new_districtid)); } }


    }
}