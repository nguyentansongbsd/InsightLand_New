using ConasiCRM.Portable.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConasiCRM.Portable.Models
{
    public class Contact : BaseViewModel
    {
        public Guid contactid { get; set; }
        public string _bsd_fullname;
        public string bsd_fullname
        {
            get
            {
                return _bsd_fullname;
            }
            set
            {
                if (_bsd_fullname != value)
                {
                    this._bsd_fullname = value;
                    OnPropertyChanged(nameof(bsd_fullname));
                }
            }
        }

        public string _bsd_type;
        public string bsd_type
        {
            get
            {
                return _bsd_type;
            }
            set
            {
                if (_bsd_type != value)
                {
                    this._bsd_type = value;
                    OnPropertyChanged(nameof(bsd_type));
                }
            }
        }

        public string _mobilephone;
        public string mobilephone
        {
            get
            {
                return _mobilephone;
            }
            set
            {
                if (_mobilephone != value)
                {
                    this._mobilephone = value;
                    OnPropertyChanged(nameof(mobilephone));
                }
            }
        }
    }
}
