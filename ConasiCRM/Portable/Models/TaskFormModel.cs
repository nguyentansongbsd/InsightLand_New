using ConasiCRM.Portable.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConasiCRM.Portable.Models
{
    public class TaskFormModel : BaseViewModel
    {
        public Guid activityid { get; set; }
        public string subject { get; set; }
        public string description { get; set; }

        public DateTime? scheduledstart { get; set; }    
        public DateTime? scheduledend { get; set; }
        
        public Guid contact_id { get; set; }
        public string contact_name { get; set; }

        public Guid account_id { get; set; }
        public string account_name { get; set; }

        public Guid lead_id { get; set; }
        public string lead_name { get; set; }

        private CustomerLookUp _customer;
        public CustomerLookUp Customer
        {
            get => _customer;
            set
            {
                if (_customer != value)
                {
                    _customer = value;
                    OnPropertyChanged(nameof(Customer));
                }
            }
        }
        public int statecode { get; set; } 
        public int statuscode { get; set; }
        public DateTime createdon { get; set; }

    }
}
