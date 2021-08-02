using ConasiCRM.Portable.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

using Xamarin.Forms;

namespace ConasiCRM.Portable.Models
{
    public class ListSubjectCase : BaseViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Nameen { get; set; }

        private string _title;
        public string title { get { return _title; } set { _title = value; OnPropertyChanged(nameof(title)); } }

        private string _subjectid;
        public string subjectid { get { return _subjectid; } set { _subjectid = value; OnPropertyChanged(nameof(subjectid)); } }
    }
}

