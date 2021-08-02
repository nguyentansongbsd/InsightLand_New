using ConasiCRM.Portable.Helper;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConasiCRM.Portable.Models
{
    public class ReservationSpecialDiscountListModel
    {
        public Guid bsd_discountspecialid { get; set; }
        public string bsd_name { get; set; }

        public decimal? bsd_percentdiscount { get; set; } // phan tram giam gia.
        public string bsd_percentdiscount_format { get => StringHelper.DecimalToPercentFormat(bsd_percentdiscount); }

        public decimal? bsd_amountdiscount;
        public string bsd_amountdiscount_format
        {
            get => StringHelper.DecimalToCurrencyText(bsd_amountdiscount);
        }

        public string reservation_name { get; set; }
        public string bsd_reasons { get; set; }

        public int statuscode { get; set; }
        public string statuscode_format
        {
            get
            {
                switch (statuscode)
                {
                    case 1:
                        return "Active";
                    case 2:
                        return "Inactive";
                    case 100000000:
                        return "Approved";
                    case 100000001:
                        return "Reject";
                    default:
                        return "";
                }
            }
        }

        public string createdby_name { get; set; }

        public DateTime createdon { get; set; }
        public string createdon_format { get => StringHelper.DateFormat(createdon); }
    }
}
