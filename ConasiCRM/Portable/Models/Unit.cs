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

        public string statusCodeFormat
        {
            get
            {
                switch (statuscode)
                {
                    case 1:
                        return "Chuẩn bị";
                    case 100000000:
                        return "Sẵn sàng";
                    case 100000004:
                        return "Giữ chỗ";
                    case 100000006:
                        return "Đặt cọc";
                    case 100000005:
                        return "Đồng ý chuyển cọc";
                    case 100000003:
                        return "Đã đủ tiền cọc";
                    case 100000001:
                        return "Thanh toán đợt 1";
                    case 100000002:
                        return "Đã bán";
                    default:
                        return null;
                }
            }
        }

        public string queseid { get; set; }
        public string queses_statuscode { get; set; }
        public int NumQueses { get; set; }

        public decimal bsd_constructionarea { get; set; }
        public string bsd_unittype_name { get; set; }
        public string bsd_direction { get; set; }
        public string bsd_view { get; set; }
        public bool bsd_vippriority { get; set; }

        public DateTime createdon { get; set; }
    }
}
