using System;
using System.Collections.Generic;
using System.Text;

namespace ConasiCRM.Portable.Models
{
    public class PhiMoGioiGiaoDichFormModel
    {
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
        public string sales_name { get; set; } // nhân viên kinh doanh
        public string bsd_name { get; set; }
        public decimal bsd_percent { get; set; }
        public decimal bsd_feeamount { get; set; }
        public decimal bsd_amount { get; set; }
        public string quote_name { get; set; } // phiếu đặt cọc
        public int bsd_quantity { get; set; }
        public string bsd_quantity_format
        {
            get => this.bsd_quantity.ToString();
        }
        public decimal bsd_totalamount { get; set; }
        public bool bsd_progressive { get; set; } // lũy tiến
    }
}
