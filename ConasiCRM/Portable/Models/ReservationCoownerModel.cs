using System;
using System.Collections.Generic;
using System.Text;

namespace ConasiCRM.Portable.Models
{
    public class ReservationCoownerModel
    {
        public Guid bsd_coownerid { get; set; }
        public string bsd_name { get; set; }
        public string account_name { get; set; }
        public string contact_name { get; set; }
        public string customer { get; set; }
    }
}
