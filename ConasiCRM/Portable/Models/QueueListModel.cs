using ConasiCRM.Portable.Helper;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConasiCRM.Portable.Models
{
    public class QueueListModel
    {
        public Guid opportunityid { get; set; }
        public Guid customer_id { get; set; }
        public Guid project_id { get; set; }
        public string project_name { get; set; }
        public string contact_name { get; set; }
        public string account_name { get; set; }

        public decimal bsd_queuingfee { get; set; }
        public string bsd_queuingfee_format
        {
            get => StringHelper.DecimalToCurrencyText(bsd_queuingfee);
        }

        public string unit_name { get; set; }
        public DateTime? createdon { get; set; }

        public DateTime? bsd_queuingexpired { get; set; }
        public string bsd_queuingexpired_format
        {
            get => StringHelper.DateFormat(bsd_queuingexpired);
        }

        public DateTime? actualclosedate { get; set; }
        public string actualclosedate_format { get { return actualclosedate.HasValue ? actualclosedate.Value.ToString("dd/MM/yyyy") : null; } }
        public int statuscode { get; set; }
        public string statuscode_label
        {
            get
            {
                switch (statuscode)
                {
                    case 1: return "Draf";
                    case 2: return "On Hold";
                    case 3: return "Won";
                    case 4: return "Canceled";
                    case 5: return "Out-Sold";
                    case 100000000: return "Queuing";
                    case 100000002: return "Waiting";
                    case 100000003: return "Expired";
                    case 100000004: return "Completed";
                    case 100000008: return "Đề  nghị huỷ";
                    case 100000009: return "Huỷ giữ chỗ nhưng chưa hoàn tiền";
                    case 100000010: return "Huỷ giũ chỗ đã hoàn tiền";
                    default: return null;
                }

            }
        }
        public string bsd_queuenumber { get; set; }
        public decimal? estimatedvalue { get; set; }
        public string estimatedvalue_format => estimatedvalue.HasValue ? string.Format("{0:#,0.#}", estimatedvalue.Value) + " đ" : null;

        public string customername
        {
            get { return contact_name ?? account_name ?? ""; }
        }

        public string createdon_format
        {
            get { return StringHelper.DateFormat(createdon); }
        }

        public string telephone { get; set; }
        public QueueListModel()
        {
            this.unit_name = " ";
        }
    }
}
