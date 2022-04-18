﻿using ConasiCRM.Portable.Helper;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConasiCRM.Portable.Models
{
    public class ProjectInfoModel
    {
        public string bsd_projectcode { get; set; }
        public string bsd_name { get; set; } // tên dự án

        public Guid bsd_investor_id { get; set; }
        public string bsd_investor_name { get; set; } // chủ dự án

        public string bsd_address { get; set; } // địa chỉ vn

        public decimal bsd_depositpercentda { get; set; } // phần trăm cọc
        public string bsd_depositpercentda_format { get => StringFormatHelper.FormatPercent(bsd_depositpercentda); }

        public DateTime? bsd_esttopdate { get; set; } // ngày dự kiến có top
        public DateTime? bsd_estimatehandoverdate { get; set; } // ngày dự kiến bàn giao

        public decimal? bsd_landvalueofproject { get; set; } // giá đất dự án
        public string bsd_landvalueofproject_format { get => StringFormatHelper.FormatCurrency(bsd_landvalueofproject); }
        public decimal? bsd_maintenancefeespercent { get; set; }// phí bảo trì
        public string bsd_maintenancefeespercent_format { get => StringFormatHelper.FormatPercent(bsd_maintenancefeespercent); }

        public int? bsd_numberofmonthspaidmf { get; set; } // số tháng tính phí quản lý
        public decimal? bsd_managementamount { get; set; } // đơn giá phí quản lý
        public string bsd_managementamount_format { get => StringFormatHelper.FormatCurrency(bsd_managementamount); }
        public decimal? bsd_bookingfee { get; set; } // phí đặt chỗ
        public string bsd_bookingfee_format { get => StringFormatHelper.FormatCurrency(bsd_bookingfee); }
        public decimal? bsd_depositamount { get; set; } // tiền đặt cọc
        public string bsd_depositamount_format { get => StringFormatHelper.FormatCurrency(bsd_depositamount); }

        public string bsd_description { get; set; } // mô tả dự án nghiên cứu r&d

        public string bsd_projecttype { get; set; }
        public int bsd_propertyusagetype { get; set; }
        public int? bsd_handoverconditionminimum { get; set; }
        public decimal bsd_unitspersalesman { get; set; } // giữ chỗ trên người
        public decimal bsd_queuesperunit { get; set; } // giữ chỗ trên dự án
        public decimal bsd_longqueuingtime { get; set; } // thời gian giữ chỗ dài hạn
        public decimal bsd_shortqueingtime { get; set; } // thời gian giữ chỗ ngắn hạn
        public string bsd_logo { get; set; }
    }
}
