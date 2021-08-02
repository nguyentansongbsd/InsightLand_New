using ConasiCRM.Portable.Helper;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConasiCRM.Portable.Models
{
    public class ReservationPromotionModel
    {
        public Guid bsd_promotionid { get; set; }
        public string project_name { get; set; }
        public string bsd_name { get; set; }
        public string phaseslaunch_name { get; set; }
        public decimal? bsd_values { get; set; }
        public string bsd_values_format { get => StringHelper.DecimalToCurrencyText(bsd_values); }

        public DateTime? bsd_startdate { get; set; }
        public string bsd_startdate_format { get => StringHelper.DateFormat(bsd_startdate); }

        public DateTime? bsd_enddate { get; set; }
        public string bsd_enddate_format { get => StringHelper.DateFormat(bsd_enddate); }

        public string bsd_description { get; set; }
        public int statuscode { get; set; }
        public DateTime createdon { get; set; }
        public string createdon_format { get => StringHelper.DateFormat(createdon); }
    }
}
