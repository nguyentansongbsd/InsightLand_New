using System;
using System.Collections.Generic;
using System.Text;

namespace ConasiCRM.Portable.Models
{
    public class ProjectInfoModel
    {
        public string bsd_projectcode { get; set; }
        public string bsd_name { get; set; } // tên dự án

        public string bsd_investor_name { get; set; } // chủ dự án

        public int bsd_loaiduan { get; set; }
        public string bsd_loaiduan_format
        {
            get
            {
                switch (bsd_loaiduan)
                {
                    case 100000000:
                        return "Dự án đơn";
                    case 100000001:
                        return "Dự án phức tạp";
                    case 100000002:
                        return "Dự án nghiên cứu";
                    default:
                        return "";
                }
            }
        }

        public string bsd_address { get; set; } // địa chỉ vn
        public string bsd_addressen { get; set; } // dia chỉ en

        public decimal bsd_depositpercentda { get; set; } // phần trăm cọc

        public DateTime? bsd_esttopdate { get; set; } // ngày dự kiến có top
        public DateTime? bsd_estimatehandoverdate { get; set; } // ngày dự kiến bàn giao

        public decimal? bsd_landvalueofproject { get; set; } // giá đất dự án
        public decimal? bsd_maintenancefeespercent { get; set; }// phí bảo trì

        public int? bsd_numberofmonthspaidmf { get; set; } // số tháng tính phí quản lý
        public decimal? bsd_managementamount { get; set; } // đơn giá phí quản lý

        public decimal? bsd_bookingfee { get; set; } // phí đặt chỗ
        public decimal? bsd_depositamount { get; set; } // tiền đặt cọc

        public string bsd_description { get; set; } // mô tả dự án nghiên cứu r&d
    }
}
