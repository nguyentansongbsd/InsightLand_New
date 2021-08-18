using System;
namespace ConasiCRM.Portable.Models
{
    public class QueuesDetailModel
    {
        public string opportunityid { get; set; }
        public string name { get; set; }
        public string description { get; set; }

        public Guid _bsd_units_value { get; set; }
        public string unit_name { get; set; }

        public Guid _bsd_project_value { get; set; }
        public string project_name { get; set; }

        public string bsd_queuenumber { get; set; }

        public string contact_name { get; set; }
        public string PhoneContact { get; set; }

        public Guid accountid { get; set; }
        public string account_name { get; set; }
        public string PhoneAccount { get; set; }

        public Guid _bsd_salesagentcompany_value { get; set; }
        public string salesagentcompany_name { get; set; }

        public double? bsd_queuingfee { get; set; }
        public double? budgetamount { get; set; }

        public Guid _bsd_phaselaunch_value { get; set; }
        public string phaselaunch_name { get; set; }

        public string bsd_nameofstaffagent { get; set; }

        public int statuscode { get; set; }
        public DateTime bsd_bookingtime { get; set; }
        public DateTime createdon { get; set; }
        public DateTime bsd_queuingexpired { get; set; }
    }
}
