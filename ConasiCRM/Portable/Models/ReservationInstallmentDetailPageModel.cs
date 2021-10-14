using System;
using System.Collections.Generic;
using System.Text;

namespace ConasiCRM.Portable.Models
{
    public class ReservationInstallmentDetailPageModel
    {
        public Guid bsd_paymentschemedetailid { get; set; }
        public string bsd_name { get; set; }
        public DateTime bsd_duedate { get; set; } // ngày đến hạn
        public int statuscode { get; set; } // tình trạng.
        public string statuscode_format { get => InstallmentsStatusCodeData.GetInstallmentsStatusCodeById(statuscode.ToString()).Name; }
        public string statuscode_color { get => InstallmentsStatusCodeData.GetInstallmentsStatusCodeById(statuscode.ToString()).Background; }
        public decimal bsd_amountofthisphase { get; set; } // số tiền đợi thnah toán.
        public decimal bsd_amountwaspaid { get; set; } // số tiền đã thanh toán
        public decimal bsd_depositamount { get; set; } // số tiền đặt cọc
        public string bsd_depositamount_format
        {
            get
            {
                if (bsd_depositamount == 0)
                    return null;
                else
                    return bsd_depositamount.ToString();
            }
        }
    }
}
