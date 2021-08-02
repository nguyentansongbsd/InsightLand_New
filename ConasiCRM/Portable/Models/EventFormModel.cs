using System;
using System.Collections.Generic;
using System.Text;

namespace ConasiCRM.Portable.Models
{
    public class EventFormModel
    {
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
        public string bsd_project_name { get; set; }
        public string bsd_description { get; set; }
        public int statuscode { get; set; }
        public string statuscode_format
        {
            get
            {
                switch (statuscode)
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

        //Thông tin đợt mở bán
        public string phaselaunch_bsd_name { get; set; }
        public string pricelevel_name { get; set; } // bảng giá
        public DateTime phaselaunch_startdate { get; set; }
        public string phaselaunch_startdate_format
        {
            get
            {
                return Helper.StringHelper.DateFormat(phaselaunch_startdate);
            }
        }
        public DateTime phaselaunch_enddate { get; set; }
        public string phaselaunch_enddate_format
        {
            get
            {
                return Helper.StringHelper.DateFormat(phaselaunch_enddate);
            }
        }
        public int phaselaunch_statuscode { get; set; }
        public string phaselaunch_statuscode_format {
            get
            {
                switch (phaselaunch_statuscode)
                {
                    case 1:
                        return "Not Launch";
                    case 100000000:
                        return "Launched";
                    case 2:
                        return "Inactive";
                    case 100000001:
                        return "Recovery";
                    case 100000002:
                        return "Unit Recovery";
                    default:
                        return "";
                }
            }
        }
    }
}
