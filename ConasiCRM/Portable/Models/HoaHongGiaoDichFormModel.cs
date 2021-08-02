using System;
using System.Collections.Generic;
using System.Text;

namespace ConasiCRM.Portable.Models
{
    public class HoaHongGiaoDichFormModel
    {
        public string bsd_name { get; set; }
        public string salesorder_name { get; set; } // hợp đồng
        public string accounts_bsd_name { get; set; } // đại lý

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
        public DateTime bsd_enddate { get; set; }
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

        public string sale_name { get; set; } // nhan vien kinh doanh
        public int bsd_type { get; set; } // loại
        public string bsd_type_format
        {
            get
            {
                switch (this.bsd_type)
                {
                    case 100000000:
                        return "Hoa hồng cho nhân viên";
                    case 100000001:
                        return "Hoa hồng cho đại lý";
                    default:
                        return " ";
                }
            }
        }
        // thông tin duyệt
        public DateTime bsd_approvaldate { get; set; } // ngày duyệt
        public string systemusers_name { get; set; }

        // thông tin bất động sản
        public string project_bsd_name { get; set; } // dự án
        public string products_name { get; set; } // bất động sản
        public string quotes_name { get; set; } // phiếu đặt cọc

        // thong tin thanh toán
        public decimal bsd_cmscollecteddepositedpercent { get; set; } // thanh toán cọc (%)
        public decimal bsd_cmscollecteddepositedamount { get; set; } // thanh toán cọc tiền
        public decimal bsd_cmsqualifiedpercent { get; set; } // điều kiện(%)
        public decimal bsd_cmsqualifiedamount { get; set; } // diều kiện số tiền
        public decimal bsd_cmssigncontractpercent { get; set; } // ky hop dong
        public decimal bsd_cmssigncontractamount { get; set; } // ký hợp đồng số tiền
        public bool bsd_cmsinstallments { get; set; } // từng đợt thanh toán
        public string bsd_cmsinstallments_format
        {
            get
            {
                switch (this.bsd_cmsinstallments)
                {
                    case true:
                        return "Có";
                    case false:
                        return "Không";
                    default:
                        return " ";
                }
            }
        }
        public decimal bsd_cmsiinstallmentercent { get; set; } // đợt thanh toán (%)
        public decimal bsd_cmsinstallmentamount { get; set; } // đợt thanh toán tiền
        public int bsd_typeofcommissiontime { get; set; } // thời gian tính hoa hồng
        public string bsd_typeofcommissiontime_format
        {
            get
            {
                switch (this.bsd_typeofcommissiontime)
                {
                    case 100000000:
                        return "Sau ngày cố định";
                    case 100000001:
                        return "Sau ngày ký hợp đồng";
                    case 100000002:
                        return "Sau đợt thanh toán";
                    default:
                        return " ";
                }
            }
        }
        public DateTime bsd_fixdate { get; set; } // ngày cố định
        public int bsd_installmentno { get; set; } // số thứ tự đợt thanh toán
        public string paymentschemedetail_name { get; set; } // đợt thanh toán
        public decimal bsd_commissiontransactionpercent { get; set; } // phần trăm được hưởng hoa hồng
        public decimal bsd_commissiontransactionamount { get; set; } // số tiền hưởng hoa hồng
        public decimal bsd_totalcommission { get; set; }
    }
}
