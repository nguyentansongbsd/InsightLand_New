using System;
namespace ConasiCRM.Portable.Models
{
    public class LeadsRating
    {
        public string bsd_leadsratingid { get; set; }
        public decimal bsd_point { get; set; }
        public string bsd_name { get; set; }
        public string statecode { get; set; }
        public string statuscode { get; set; }
        public DateTime? bsd_startdate { get; set; }
        public DateTime? bsd_enddate { get; set; }
    }
}
