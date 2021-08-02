
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ConasiCRM.Portable.Models
{
    public class MandatorySecondaryModel : ContentView
    {
        public string bsd_mandatorysecondaryid { get; set; }
        public string bsd_name { get; set; }
        private DateTime? _createdon;
        public DateTime? createdon
        {
            get => _createdon;
            set
            {
                if (value.HasValue)
                { _createdon = value.Value.ToLocalTime();
                    OnPropertyChanged(nameof(createdon));
                }
            }
        }
        public string statuscode { get; set; }
        public string ownerid { get; set; }
        public string bsd_jobtitlevn { get; set; }
        public string bsd_jobtitleen { get; set; }

        private DateTime? _bsd_effectivedateto;
        public DateTime? bsd_effectivedateto { get { return _bsd_effectivedateto; }
            set { if (value.HasValue) { _bsd_effectivedateto = value.Value.ToLocalTime(); OnPropertyChanged(nameof(bsd_effectivedateto)); } } }
        public string bsd_effectivedatefrom { get; set; }
        public string _bsd_developeraccount_value { get; set; }
        public string bsd_developeraccount { get; set; }
        public string bsd_contact_name { get; set; }
        public string _bsd_contact_value { get; set; }

    }
}
