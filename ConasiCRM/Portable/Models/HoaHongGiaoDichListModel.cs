using System;
using System.Collections.Generic;
using System.Text;
using ConasiCRM.Portable.Helper;
namespace ConasiCRM.Portable.Models
{
    public class HoaHongGiaoDichListModel
    {
        public Guid bsd_commissiontransactionid { get; set; }
        public string project_bsd_name { get; set; } // dự án
        public string products_name { get; set; } // bất động sản
        public string bsd_name { get; set; }
        public string accounts_bsd_name { get; set; } // đại lý
        public string quotes_name { get; set; } // phiếu đặt cọc
        public int bsd_method { get; set; }
        public string bsd_method_format
        {
            get
            {
                switch (this.bsd_method)
                {
                    case 100000000:
                        return "Phần trăm";
                    case 100000001:
                        return "Số tiền";
                    default:
                        return " ";
                }
            }
        }
        public DateTime bsd_startdate { get; set; }
        public string bsd_startdate_format
        {
            get => StringHelper.DateFormat(this.bsd_startdate);
        }
        public DateTime bsd_enddate { get; set; }
        public string bsd_enddate_format
        {
            get => StringHelper.DateFormat(this.bsd_enddate);
        }
        public decimal bsd_totalcommission { get; set; }
        public string bsd_totalcommission_format
        {
            get => StringHelper.DecimalToCurrencyText(this.bsd_totalcommission);
        }
        public int statuscode { get; set; }
        public string statuscode_format
        {
            get
            {
                switch (this.statuscode)
                {
                    case 1:
                        return "Nháp";
                    case 100000000:
                        return "Áp dụng";
                    case 100000001:
                        return "Chốt";
                    case 100000002:
                        return "Huỷ";
                    default:
                        return " ";
                }
            }
        }
        public DateTime createdon { get; set; }

        public string createdon_format
        {
            get => StringHelper.DateFormat(this.createdon);
        }
    }
}
