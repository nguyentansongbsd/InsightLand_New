using ConasiCRM.Portable.Helper;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConasiCRM.Portable.Models
{
    public class PhiMoGioiGiaoDichListModel
    {
        public Guid bsd_brokeragetransactionid { get; set; }
        public string project_bsd_name { get; set; } // dự án
        public string product_name { get; set; } // bất động sản
        public string contact_bsd_fullname { get; set; }
        public string account_bsd_name { get; set; }
        public string customer_name
        {
            get
            {
                if (this.contact_bsd_fullname != null)
                {
                    return this.contact_bsd_fullname;
                }
                else if (this.account_bsd_name != null)
                {
                    return this.account_bsd_name;
                }
                else return "";
            }
        }
        public string brokeragefees_name { get; set; } // phi mo gioi
        public string sales_name { get; set; }
        public string bsd_name { get; set; }
        public decimal bsd_percent { get; set; }
        public string bsd_percent_format
        {
            get => StringHelper.DecimalToPercentFormat(this.bsd_percent);
        }
        public decimal bsd_feeamount { get; set; }
        public string bsd_feeamount_format
        {
            get => StringHelper.DecimalToCurrencyText(bsd_feeamount);
        }
        public decimal bsd_amount { get; set; }
        public string bsd_amount_format
        {
            get => StringHelper.DecimalToCurrencyText(this.bsd_amount);
        }
        public string quote_name { get; set; } // phiếu đặt cọc
        public int bsd_quantity { get; set; }
        public decimal bsd_totalamount { get; set; }
        public string bsd_totalamount_format
        {
            get => StringHelper.DecimalToCurrencyText(this.bsd_totalamount);
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
                        return "";
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
