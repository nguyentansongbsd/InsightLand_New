using System;
using System.Collections.Generic;
using System.Text;

namespace ConasiCRM.Portable.Models
{
    public class UnitInfoModel
    {
        public string bsd_units { get; set; } // Units Number
        public string name { get; set; } // Units Code
        public string productnumber { get; set; } // Unit Description - mã can hộ dự án
        public string productid { get; set; }

        public int statuscode { get; set; }
        public string statuscode_format
        {
            get
            {
                var result = string.Empty;
                switch (statuscode)
                {
                    case 1:
                        result = "Preparing";
                        break;
                    case 100000000:
                        result = "Available";
                        break;
                    case 100000004:
                        result = "Queuing";
                        break;
                    case 100000007:
                        result = "Giữ chỗ";
                        break;
                    case 100000006:
                        result = "Reserve";
                        break;
                    case 100000005:
                        result = "Collected";
                        break;
                    case 100000003:
                        result = "Deposited";
                        break;
                    case 100000001:
                        result = "1st Installment";
                        break;
                    case 100000008:
                        result = "Đủ điều kiện";
                        break;
                    case 100000009:
                        result = "Thỏa thuận đặt cọc";
                        break;
                    case 100000002:
                        result = "Sold";
                        break;
                    default:
                        break;
                }
                return result;
            }
        }

        public Guid bsd_project_id { get; set; } // khong su dung trong form unit , su dung trong form queue
        public string bsd_project_name { get; set; }

        public Guid bsd_block_id { get; set; } // khong su dung trong form unit , su dung trong form queue
        public string bsd_block_name { get; set; }

        public Guid bsd_floor_id { get; set; } // khong su dung trong form unit , su dung trong form queue
        public string bsd_floor_name { get; set; }

        public Guid bsd_phaseslaunch_id { get; set; }// khong su dung trong form unit , su dung trong form queue
        public string bsd_phaseslaunch_name { get; set; } // khong su dung trong form unit , su dung trong form queue

        public Guid pricelist_id { get; set; }// khong su dung trong form unit , su dung trong form queue
        public string pricelist_name { get; set; } // khong su dung trong form unit , su dung trong form queue

        public decimal bsd_depositamount { get; set; } // Deposit Amount

        public decimal bsd_queuingfee { get; set; } // Queuing Amount

        public string bsd_unittype_name { get; set; }

        public bool bsd_vippriority { get; set; }
        public string bsd_vippriority_format
        {
            get => bsd_vippriority == true ? "Yes" : "No";
        }

        // thong tin dien tich
        public decimal bsd_areavariance { get; set; } // Biên độ diện tích cho phép

        public decimal bsd_constructionarea { get; set; } // diện tích xây dựng

        public decimal bsd_netsaleablearea { get; set; } // diện tích sử dụng 

        // thong itn gia
        public decimal price { get; set; } // Giá bán
        public string price_format
        {
            get => String.Format("{0:0,0.00 đ}", price);
        }

        public decimal bsd_landvalueofunit { get; set; } // đơn giá giá trị đất

        public decimal bsd_landvalue { get; set; } // giá trị đất

        public decimal bsd_maintenancefeespercent { get; set; } // phần trăm phí bảo trì

        public decimal bsd_maintenancefees { get; set; } // tiền phí bảo trị

        public decimal bsd_taxpercent { get; set; } // phằn trăm thuế

        public decimal bsd_vat { get; set; } // tiền tuế

        public decimal bsd_totalprice { get; set; } // tiền tuế


        // bàn giao

        public DateTime bsd_estimatehandoverdate { get; set; } // ngày dự kiến bàn giao.

        public int bsd_numberofmonthspaidmf { get; set; } // số tháng tính phí quản lý.

        public decimal bsd_managementamountmonth { get; set; }// đơn giá tính phí quản lý (tháng/m2)

        public decimal bsd_handovercondition { get; set; } // Điều kiện bàn giao %
    }
}
