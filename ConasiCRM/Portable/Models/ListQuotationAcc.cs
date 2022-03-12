using System;
using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Resources;
using Xamarin.Forms;

namespace ConasiCRM.Portable.Models
{
    public class ListQuotationAcc : ContentView
    {
        public string customerid { get; set; }
        public int statuscode { get; set; }
        public decimal? totalamount { get; set; }
        public string totalamountformat => totalamount.HasValue ? string.Format("{0:#,0.#}", totalamount.Value) + " đ" : null;
        public string bsd_quotationnumber { get; set; }
        public string statuscodevalue
        {
            get
            {
                switch (statuscode)
                {
                    case 1:
                        return Language.quote_in_progress_draf_sts;
                    case 100000007:
                        return Language.quote_quotation_sts;
                    case 100000000:
                        return Language.quote_reservation_sts;
                    case 100000006:
                        return Language.quote_collected_sts;
                    case 2:
                        return Language.quote_in_progress_active_sts;
                    case 3:
                        return Language.quote_deposited_sts;
                    case 100000002:
                        return Language.quote_pending_cancel_deposit_sts;
                    case 100000004:
                        return Language.quote_signed_RF_sts;
                    case 100000009:
                        return Language.quote_expired_sts;
                    case 4:
                        return Language.quote_won_sts;
                    case 100000001:
                        return Language.quote_terminated_sts;
                    case 100000003:
                        return Language.quote_reject_sts;
                    case 5:
                        return Language.quote_lost_sts;
                    case 6:
                        return Language.quote_canceled_sts;
                    case 7:
                        return Language.quote_revised_sts;
                    case 100000005:
                        return Language.quote_expired_of_signing_RF_sts;
                    case 100000008:
                        return Language.quote_expired_quotation_sts;
                    default:
                        return "";
                }
            }
        }
        public DateTime createdon { get; set; }
        public string createdonformat
        {
            get => StringHelper.DateFormat(createdon);
        }
        public string quo_nameproject { get; set; }
        public string quo_nameaccount { get; set; }
        public string quo_namecontact { get; set; }
        public string quo_nameproduct { get; set; }
    }
}

