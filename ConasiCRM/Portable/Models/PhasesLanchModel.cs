﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ConasiCRM.Portable.Models
{
    public class PhasesLanchModel
    {
        public string bsd_name { get; set; }
        public DateTime createdon { get; set; }
        public Guid bsd_phaseslaunchid { get; set; }
        public DateTime startdate_event { get; set; }
        public DateTime enddate_event { get; set; }
        public string statuscode_event { get; set; }
    }
}
