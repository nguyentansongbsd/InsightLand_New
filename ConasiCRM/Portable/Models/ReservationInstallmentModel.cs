using ConasiCRM.Portable.Helper;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConasiCRM.Portable.Models
{
    public class ReservationInstallmentModel
    {
        public int? bsd_ordernumber { get; set; }
        public string bsd_name { get; set; }

        public DateTime? bsd_duedate { get; set; } // ngày đến hạn
        public string bsd_duedate_format { get => StringHelper.DateFormat(bsd_duedate); }

        public int statuscode { get; set; } // tình trạng.
        public string statuscode_format
        {
            get
            {
                switch (statuscode)
                {
                    case 1:
                        return "Active";
                    case 100000000:
                        return "Not Paid";
                    case 100000001:
                        return "Paid";
                    case 2:
                        return "Inactive";
                    default:
                        return "";
                }
            }
        }

        public decimal? bsd_amountofthisphase { get; set; } // số tiền đợi thnah toán.
        public string bsd_amountofthisphase_format { get => StringHelper.DecimalToCurrencyText(bsd_amountofthisphase); }

        public decimal? bsd_amountwaspaid { get; set; } // số tiền đã thanh toán
        public string bsd_amountwaspaid_format { get => StringHelper.DecimalToCurrencyText(bsd_amountwaspaid); }

        public decimal? bsd_depositamount { get; set; } // số tiền đặt cọc
        public string bsd_depositamount_format { get => StringHelper.DecimalToCurrencyText(bsd_depositamount); }

        public decimal? bsd_waiveramount { get; set; } // số tiền còn lại
        public string bsd_waiveramount_format { get => StringHelper.DecimalToCurrencyText(bsd_waiveramount); }

    }
}
