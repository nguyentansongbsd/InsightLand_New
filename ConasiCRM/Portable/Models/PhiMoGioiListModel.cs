using ConasiCRM.Portable.Converters.Unit;
using ConasiCRM.Portable.Helper;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConasiCRM.Portable.Models
{
    public class PhiMoGioiListModel
    {
        public Guid bsd_brokeragefeesid { get; set; }
        public string bsd_name { get; set; }
        public string project_bsd_name { get; set; }
        public int bsd_loaimoigioi { get; set; }
        public string bsd_loaimoigioi_format
        {
            get
            {
                switch (this.bsd_loaimoigioi)
                {
                    case 100000000:
                        return "Cộng tác viên";
                    case 100000001:
                        return "Khách hàng giới thiệu";
                    case 100000002:
                        return "Đơn vị liên kết";
                    default:
                        return " ";
                }
            }
        }
        public int bsd_calculation { get; set; } // cách tính
        public string bsd_calculation_format
        {
            get
            {
                switch (this.bsd_calculation)
                {
                    case 100000000:
                        return "Số lượng";
                    case 100000001:
                        return "Số tiền";
                    default:
                        return " ";
                }
            }
        }
        public int bsd_method { get; set; } // phương thức
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
        public bool bsd_progressive { get; set; } //lũy tiến
        public int bsd_quantityfrom { get; set; } // số lượng từ
        public int bsd_quantityto { get; set; } // số lượng đến
        public decimal bsd_amountfrom { get; set; } // số tiền từ
        public string bsd_amountfrom_format
        {
            get => StringHelper.DecimalToCurrencyText(bsd_amountfrom);
        }
        public decimal bsd_amountto { get; set; } // số tiền đến
        public string bsd_amountto_format
        {
            get => StringHelper.DecimalToCurrencyText(bsd_amountto);
        }
        public decimal bsd_feepercent { get; set; } // phi %
        public string bsd_feepercent_format
        {
            get => StringHelper.DecimalToPercentFormat(bsd_feepercent);
        }
        public decimal bsd_feeamount { get; set; } // phi số tiền
        public string bsd_feeamount_format
        {
            get => StringHelper.DecimalToCurrencyText(bsd_feeamount);
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
                        return "Từ chối";
                    case 100000002:
                        return "Hết hạn";
                    default:
                        return " ";
                }
            }
        }
        public DateTime createdon { get; set; }
        public string createdon_format
        {
            get
            {
                return this.createdon.ToString("dd/MM/yyyy");
            }
        }
    }
}
