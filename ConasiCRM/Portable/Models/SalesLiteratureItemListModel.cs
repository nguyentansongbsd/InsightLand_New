﻿using System;
using ConasiCRM.Portable.Resources;
using ConasiCRM.Portable.ViewModels;

namespace ConasiCRM.Portable.Models
{
    public class SalesLiteratureItemListModel : BaseViewModel
    {
        public Guid salesliteratureitemid { get; set; }
        public string title_label { get; set; }
        public DateTime? modifiedon { get; set; }
        public string filename { get; set; }
        public string documentbody { get; set; }

        private string _statusImage;
        public string statusImage { get => _statusImage == null ? "dowloanding_icon.png" : _statusImage; set { _statusImage = value; OnPropertyChanged(nameof(statusImage)); } }

        private int _status;
        public int status { get => _status; set
            {
                switch (value)
                {
                    case 0: statusLabel = Language.dang_tai; //downloading
                        statusColor = Xamarin.Forms.Color.Black;
                        break;
                    case 1: statusLabel = Language.thanh_cong; //successed
                        statusColor = Xamarin.Forms.Color.Green;
                        break;
                    case 2: statusLabel = Language.that_bai; //Failed
                        statusColor = Xamarin.Forms.Color.Red;
                        break;
                }

                _status = value;
                OnPropertyChanged(nameof(status));
            } }
        
        private string _statusLabel;
        public string statusLabel { get => _statusLabel == null ? Language.dang_tai : _statusLabel; set { _statusLabel = value; OnPropertyChanged(nameof(statusLabel)); } }

        private Xamarin.Forms.Color _statusColor;
        public Xamarin.Forms.Color statusColor { get => _statusColor; set { _statusColor = value;  OnPropertyChanged(nameof(statusColor)); } }
    }
}
