
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

        private string _bsd_developeraccount;
        public string bsd_developeraccount { get { return _bsd_developeraccount; } set { _bsd_developeraccount = value; OnPropertyChanged(nameof(bsd_developeraccount)); } }
        public string bsd_contact_name { get; set; }
        public string bsd_contacmobilephone { get; set; }
        public string bsd_contactaddress { get; set; }        
        public string _bsd_contact_value { get; set; }
        public string statuscode_title { get; set; }
        public Guid bsd_contactid { get; set; }
        public string bsd_descriptionsvn { get; set; }
        public string bsd_descriptionsen { get; set; }

    }
}
