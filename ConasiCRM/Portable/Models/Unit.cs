using ConasiCRM.Portable.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConasiCRM.Portable.Models
{
    public class Unit :BaseViewModel
    {
        public List<Unit> Items { get; set; }

        public Guid productid { get; set; }
        public string name { get; set; }
        public decimal bsd_totalprice { get; set; }
        public decimal price { get; set; }
        public int? statuscode { get; set; }
        public Guid _bsd_floor_value { get; set; }
        public string floor_name { get; set; }
        public string block_name { get; set; }
        public Guid floorid { get; set; }
        public Guid blockid { get; set; }
        public Guid event_id { get; set; } // join voi phaseslauch va event de lay ra vent id de biet duoc co phai nam trong event ko.
        public string bsd_totalprice_format
        {
            get
            {
                return String.Format("{0:0,0 đ}", bsd_totalprice);
            }
        }
        public string price_format
        {
            get
            {
                return String.Format("{0:0,0 đ}", price);
            }
        }
        public decimal bsd_netsaleablearea { get; set; }
        public string bsd_netsaleablearea_format
        {
            get
            {
                return Math.Round(bsd_netsaleablearea, 1, MidpointRounding.AwayFromZero).ToString() + " m2";
            }
        }
        public string item_background { get; set; }

        public string queseid { get; set; }
        public string queses_statuscode { get; set; }
        public int NumQueses { get; set; }

        public decimal bsd_constructionarea { get; set; }
        private string _bsd_unittype_name;
        public string bsd_unittype_name { get=>_bsd_unittype_name; set { _bsd_unittype_name = value;OnPropertyChanged(nameof(bsd_unittype_name)); } }
        public string bsd_direction { get; set; }
        public string bsd_view { get; set; }
        public bool bsd_vippriority { get; set; }

        public Guid _bsd_employee_value { get; set; }
        public Guid _bsd_projectcode_value { get; set; }
        public Guid _bsd_phaseslaunchid_value { get; set; }

        public DateTime createdon { get; set; }
    }
}
