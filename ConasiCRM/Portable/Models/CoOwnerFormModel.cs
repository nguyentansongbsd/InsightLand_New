using ConasiCRM.Portable.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConasiCRM.Portable.Models
{
    public class CoOwnerFormModel : BaseViewModel
    {
        private string _bsd_name;
        public string bsd_name
        {
            get => _bsd_name;
            set
            {
                _bsd_name = value;
                OnPropertyChanged(nameof(bsd_name));
            }
        }

        // khai bao de hung trong form type 2
        private string _bsd_relationship;
        public string bsd_relationship
        {
            get => _bsd_relationship;
            set
            {
                if (_bsd_relationship != value)
                {
                    _bsd_relationship = value;
                    OnPropertyChanged(nameof(bsd_relationship));
                }
            }
        }

        // khai bao de hung trong form type 2
        public Guid contact_id { get; set; }
        public string contact_name { get; set; }

        // khai bao de hung tron gform type 2
        public Guid account_id { get; set; }
        public string account_name { get; set; }

        public Guid reservation_id { get; set; }
        public string reservation_name { get; set; }
    }

}
