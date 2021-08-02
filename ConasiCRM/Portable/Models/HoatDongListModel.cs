using System;
using System.Collections.Generic;
using System.Text;
using ConasiCRM.Portable.Helper;
namespace ConasiCRM.Portable.Models
{
    public class HoatDongListModel
    {
        public Guid activityid { get; set; }
        public string subject { get; set; }
        public string accounts_bsd_name { get; set; }
        public string contact_bsd_fullname { get; set; }
        public string lead_fullname { get; set; }
        public string systemsetup_bsd_name { get; set; }
        public string regarding_name
        {
            get
            {
                if(this.accounts_bsd_name != null)
                {
                    return this.accounts_bsd_name;
                }
                else if(this.contact_bsd_fullname != null)
                {
                    return this.contact_bsd_fullname;
                }else if(this.lead_fullname != null)
                {
                    return this.lead_fullname;
                }
                else if(this.systemsetup_bsd_name != null)
                {
                    return this.systemsetup_bsd_name;
                }
                else
                {
                    return " ";
                }
            }
        }
        public string activitytypecode { get; set; }
        public string activitytypecode_format
        {
            get
            {
                switch (activitytypecode)
                {
                    case "task":
                        return "Task";
                    case "phonecall":
                        return "Phone Call";
                    case "appointment":
                        return "Collection Meeting";
                    default:
                        return " ";
                }
            }
        }
        public int statecode { get; set; }
        public string statecode_format
        {
            get
            {
                switch (this.statecode)
                {
                    case 0:
                        return "Open";
                    case 1:
                        return "Completed";
                    case 2:
                        return "Canceled";
                    case 3:
                        return "Scheduled";
                    default:
                        return " ";
                }
            }
        }
        public string owners_fullname { get; set; }
        public int prioritycode { get; set; }
        public string prioritycode_format
        {
            get
            {
                switch (prioritycode)
                {
                    case 0:
                        return "Low";
                    case 1:
                        return "Normal";
                    case 2:
                        return "High";
                    default:
                        return " ";
                }
            }
        }
        public DateTime scheduledstart { get; set; }
        public string scheduledstart_format
        {
            get => StringHelper.DateFormat(this.scheduledstart);
        }
        public DateTime scheduledend { get; set; }
        public string scheduledend_format
        {
            get => StringHelper.DateFormat(this.scheduledend);
        }
        public DateTime createdon { get; set; }
        public string createdon_format
        {
            get => StringHelper.DateFormat(this.createdon);
        }

    }
}
