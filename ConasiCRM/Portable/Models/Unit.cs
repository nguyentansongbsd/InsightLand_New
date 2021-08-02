using ConasiCRM.Portable.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConasiCRM.Portable.Models
{
    public class Unit
    {
        public List<Unit> Items { get; set; }

        public Guid productid { get; set; }
        public string name { get; set; }
        public decimal bsd_totalprice { get; set; }
        public decimal price { get; set; }
        public int statuscode { get; set; }
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
        public string item_background
        {
            get
            {
                switch (statuscode)
                {
                    case 1:
                        return "#F1C40F";
                    case 100000000:
                        return "#2ECC71";
                    case 100000001:
                        return "#7F8C8D";  // 1st installmetn
                    case 100000004:
                        return "#03A9F4";
                    case 100000006:
                        return "#16A085";
                    case 100000005:
                        return "#8E44AD";
                    case 100000003:
                        return "#E67E22";
                    case 100000009:
                        return "#ce8686"; // thoar thuan dat coc
                    case 100000002:
                        return "#C0392B";
                    default:
                        return "Red";
                };
            }
        }

        public string statusCodeFormat
        {
            get
            {
                switch (statuscode)
                {
                    case 1:
                        return "Preparing";
                    case 100000000:
                        return "Available";
                    case 100000007:
                        return "Booking";
                    case 100000004:
                        return "Queuing";
                    case 100000006:
                        return "Reserve";
                    case 100000005:
                        return "Collected";
                    case 100000003:
                        return "Deposited";
                    case 100000001:
                        return "1st Installment";
                    case 100000009:
                        return "Singed D.A";
                    case 100000008:
                        return "Qualified";
                    case 100000002:
                        return "Sold";
                    default:
                        return null;
                }
            }
        }

        public DateTime createdon { get; set; }
    }
}
