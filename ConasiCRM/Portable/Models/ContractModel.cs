using System;
using System.Collections.Generic;
using System.Text;

namespace ConasiCRM.Portable.Models
{
    public class ContractModel
    {
        // list
        public Guid salesorderid { get; set; } // id hợp đồng
        public string salesorder_name { get; set; } // tên hợp đồng
        public string bsd_optionno { get; set; } // no hợp đồng
        public string ordernumber { get; set; } // số hợp đồng
        public Guid project_id { get; set; }
        public string project_name { get; set; }
        public Guid unit_id { get; set; }
        public string unit_name { get; set; }
        public decimal totalamount { get; set; } // tổng tiền
        public int statuscode { get; set; }
        public string statuscode_format
        {
            get
            {
                var status = ContractStatusCodeData.GetContractStatusCodeById(statuscode.ToString());
                if (status != null)
                    return status.Name;
                else
                    return "";
            }
        }
        public string statuscode_color
        {
            get
            {
                var status = ContractStatusCodeData.GetContractStatusCodeById(statuscode.ToString());
                if (status != null)
                    return status.Background;
                else
                    return "#bfbfbf";
            }
        }
        public Guid customerid { get; set; } // id khách hàng
        public string customer_name // tên khách hàng 
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(account_name))
                {
                    return account_name;
                }
                else
                {
                    return contact_name ?? "";
                }
            }
        }
        public string contact_name { get; set; } // tên khách hàng cá nhân
        public string account_name { get; set; } // tên khách hàng doanh nghiệp

        // detail
        public Guid queue_id { get; set; } // id giữ chô
        public string queue_name { get; set; } // title giữ chỗ
        public Guid reservation_id { get; set; } // id đặt cọc
        public string reservation_name { get; set; } // title đặt cọc
        public Guid salesagentcompany_id { get; set; } //id đại lý/ sàn giao dịch
        public string salesagentcompany_name { get; set; } // tên đại lý/ sàn giao dịch
        public string nameofstaffagent { get; set; } // tên nhân viên
        public string bsd_referral { get; set; } // giới thiệu
        public int bsd_unitstatus { get; set; } // trạng thái unit
        public string bsd_unitstatus_format { get => StatusCodeUnit.GetStatusCodeById(bsd_unitstatus.ToString()).Name; }
        public decimal bsd_constructionarea { get; set; } // diện tích xây dựng
        public decimal bsd_netusablearea { get; set; } // diện tích sử dụng
        public decimal unit_actualarea { get; set; } // diện tích thực
        public Guid phaseslaunch_id { get; set; } //id đợt mở bán
        public string phaseslaunch_name { get; set; } // tên đợt mở bán
        public Guid pricelevelid { get; set; } // bảng giá gốc
        public string pricelevel_name { get; set; } // tên bảng giá gốc
        public Guid taxcode_id { get; set; } // id mã số thuế
        public string taxcode_name { get; set; } // mã số thuể
        public decimal bsd_queuingfee { get; set; } // phí giũ chỗ
        public decimal bsd_depositamount { get; set; } // phí đặt cọc
        public bool bsd_allowchangeunitsspec { get; set; } // thay đổi đđktsp
        public Guid bsd_unitsspecification_id { get; set; } // id đđktsp
        public string bsd_unitsspecification_name { get; set; } // tên đđktsp
        public Guid bsd_exchangeratedetailid { get; set; } // id tỷ giá
        public string bsd_exchangeratedetail_name { get; set; } // tên tỷ giá
    }
}
