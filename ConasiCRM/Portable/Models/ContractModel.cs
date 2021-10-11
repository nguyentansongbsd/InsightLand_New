using System;
using System.Collections.Generic;
using System.Text;

namespace ConasiCRM.Portable.Models
{
    public class ContractModel
    {
        public Guid salesorderid { get; set; } // id hợp đồng
        public string ordernumber { get; set; } // số hợp đồng
        public Guid project_id { get; set; }
        public string project_name { get; set; }
        public Guid unit_id { get; set; }
        public string unit_name { get; set; }
        public decimal totalamount { get; set; } // tổng tiền
        public int statuscode { get; set; }
        public string statuscode_format { get => ContractStatusCodeData.GetContractStatusCodeById(statuscode.ToString()).Name; }
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
    }
}
