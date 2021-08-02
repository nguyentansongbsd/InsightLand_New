using ConasiCRM.Portable.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConasiCRM.Portable.Models
{
    public class AccountListModel
    {
        public Guid accountid { get; set; }

        public string bsd_name { get; set; }
        public string bsd_registrationcode { get; set; }
        public string bsd_vatregistrationnumber { get; set; }
        public string bsd_companycode { get; set; }
        public string primarycontact_name { get; set; }
        public string bsd_address { get; set; }
        public string telephone1 { get; set; }
    }
}
