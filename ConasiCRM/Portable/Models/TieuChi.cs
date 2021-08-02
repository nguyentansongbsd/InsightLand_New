using ConasiCRM.Portable.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConasiCRM.Portable.Models
{
    public class TieuChi : BaseViewModel
    {
        private int _id;
        public int Id { get { return _id; } set { _id = value; OnPropertyChanged(nameof(Id)); } }

        private string _name;
        public string Name { get { return _name; } set { _name = value; OnPropertyChanged(nameof(Name)); } }

        public TieuChi()
        {           
        }
    }
}
