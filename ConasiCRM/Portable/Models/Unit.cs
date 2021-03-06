using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConasiCRM.Portable.Models
{
    public class Unit : BaseViewModel
    {
        public Guid productid { get; set; }
        public string name { get; set; }
        public decimal price { get; set; }
        public string price_format { get => StringFormatHelper.FormatCurrency(price); }
        public int? statuscode { get; set; }

        public Guid floorid { get; set; }
        public Guid blockid { get; set; }
        public Guid event_id { get; set; } // join voi phaseslauch va event de lay ra vent id de biet duoc co phai nam trong event ko.
        public bool has_event { get { return (event_id != Guid.Empty && statuscode == 100000000 || event_id != Guid.Empty && statuscode == 100000004) ? true : false; } }
        public string queseid { get; set; }
        public string queses_statuscode { get; set; }
        public int NumQueses { get; set; }

        public decimal bsd_constructionarea { get; set; }
        public string bsd_unittype_name { get; set; }
        public string bsd_direction { get; set; }
        public string bsd_view { get; set; }

        public Guid _bsd_employee_value { get; set; }
        public Guid _bsd_projectcode_value { get; set; }
        public Guid _bsd_phaseslaunchid_value { get; set; }
        public bool bsd_vippriority { get; set; }
        public DateTime createdon { get; set; }
        public bool is_event { get { if (event_id != Guid.Empty) return true; else return false; } }
    }
}
