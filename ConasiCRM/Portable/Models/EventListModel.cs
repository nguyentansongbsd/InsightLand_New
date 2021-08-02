using System;
using System.Collections.Generic;
using System.Text;

namespace ConasiCRM.Portable.Models
{
    public class EventListModel
    {
        public Guid bsd_eventid { get; set; }
        public string bsd_eventcode { get; set; }
        public string bsd_name { get; set; }
        public DateTime bsd_startdate { get; set; }
        public string bsd_startdate_format
        {
            get
            {
                return Helper.StringHelper.DateFormat(bsd_startdate);
            }
        }
        public DateTime bsd_enddate { get; set; }
        public string bsd_enddate_format
        {
            get
            {
                return Helper.StringHelper.DateFormat(bsd_enddate);
            }
        }
        public string bsd_phaseslaunch_name { get; set; }
        public string bsd_project_name { get; set; }
        public string bsd_description { get; set; }
        public int statuscode { get; set; }
        public string statuscode_format
        {
            get
            {
                switch (this.statuscode)
                {
                    case 1:
                        return "Active";
                    case 100000006:
                        return "Submited";
                    case 100000000:
                        return "Approved";
                    case 100000005:
                        return "Reject";
                    case 2:
                        return "Inactive";
                    case 100000003:
                        return "Expired";
                    case 100000004:
                        return "Cancel";
                    default:
                        return "";
                }
            }
        }

        public DateTime createdon { get; set; }
        public string createdon_format
        {
            get
            {
                return this.createdon.ToString("dd/MM/yyyy");
            }
        }
    }
}
