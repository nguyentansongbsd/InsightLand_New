﻿using System;
namespace ConasiCRM.Portable.Models
{
    public class ActivitiModel
    {
        public Guid activityid { get; set; }
        public string subject { get; set; }
        public string activitytypecode { get; set; }

        public string customer { get; set; }
        public Guid contact_id { get; set; }
        public string contact_name { get; set; }

        public Guid account_id { get; set; }
        public string account_name { get; set; }

        public Guid lead_id { get; set; }
        public string lead_name { get; set; }

        public DateTime scheduledstart { get; set; }
        public DateTime scheduledend { get; set; }
        public DateTime createdon { get; set; }
    }
}
