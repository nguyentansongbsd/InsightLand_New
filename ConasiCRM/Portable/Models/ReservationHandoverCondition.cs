using ConasiCRM.Portable.Helper;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConasiCRM.Portable.Models
{
    public class ReservationHandoverCondition
    {
        public Guid bsd_packagesellingid { get; set; }
        public string bsd_name { get; set; }

        public int bsd_method { get; set; }
        public string bsd_method_format
        {
            get
            {
                switch (bsd_method)
                {
                    case 100000000:
                        return "Price per sqm";
                    case 100000001:
                        return "Amount";
                    case 100000002:
                        return "Percent(%)";
                    default:
                        return "";
                }
            }
        }

        public decimal? bsd_percent { get; set; }
        public string bsd_percent_format { get => StringHelper.DecimalToPercentFormat(bsd_percent); }

        public decimal? bsd_amount { get; set; }
        public string bsd_amount_format { get => StringHelper.DecimalToCurrencyText(bsd_amount); }

        public decimal? bsd_priceperm2 { get; set; }
        public string bsd_priceperm2_format { get => StringHelper.DecimalToCurrencyText(bsd_priceperm2); }

        public DateTime createdon { get; set; }
        public string createdon_format { get => StringHelper.DateFormat(createdon); }
    }

}
