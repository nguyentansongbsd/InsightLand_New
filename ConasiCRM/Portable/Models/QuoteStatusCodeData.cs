using ConasiCRM.Portable.Resources;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConasiCRM.Portable.Models
{
    public class QuoteStatusCodeData
    {
        public static List<StatusCodeModel> QuoteStatusData()
        {
            return new List<StatusCodeModel>()
            {
                new StatusCodeModel("0","","#FFFFFF"),
                new StatusCodeModel("1",Language.quote_in_progress_draf_sts,"#808080"),//In Progress  quote_in_progress_draf_sts
                new StatusCodeModel("2",Language.quote_in_progress_active_sts,"#808080"), //In Progress quote_in_progress_active_sts
                new StatusCodeModel("3",Language.quote_deposited_sts,"#808080"), //Deposited  quote_deposited_sts
                new StatusCodeModel("4",Language.quote_won_sts,"#8bce3d"), //Won  quote_won_sts
                new StatusCodeModel("5",Language.quote_lost_sts,"#808080"),//Lost  quote_lost_sts
                new StatusCodeModel("6",Language.quote_canceled_sts,"#808080"), // Canceled  quote_canceled_sts
                new StatusCodeModel("7",Language.quote_revised_sts,"#808080"),//Revised  quote_revised_sts

                new StatusCodeModel("100000000",Language.quote_reservation_sts,"#ffc43d"), // Reservation  quote_reservation_sts
                new StatusCodeModel("100000001",Language.quote_terminated_sts,"#F43927"), // Terminated  quote_terminated_sts
                new StatusCodeModel("100000002",Language.quote_pending_cancel_deposit_sts,"#808080"), //Pending Cancel Deposit  quote_pending_cancel_deposit_sts
                new StatusCodeModel("100000003",Language.quote_reject_sts,"#808080"),//Reject  quote_reject_sts
                new StatusCodeModel("100000004",Language.quote_signed_RF_sts,"#808080"), //Signed RF  quote_signed_RF_sts
                new StatusCodeModel("100000005",Language.quote_expired_of_signing_RF_sts,"#808080"),//Expired of signing RF  quote_expired_of_signing_RF_sts
                new StatusCodeModel("100000006",Language.quote_collected_sts,"#808080"), //Collected  quote_collected_sts
                new StatusCodeModel("100000007",Language.quote_quotation_sts,"#FF8F4F"), //Quotation  quote_quotation_sts
                new StatusCodeModel("100000008",Language.quote_expired_quotation_sts,"#808080"),//Expired Quotation  quote_expired_quotation_sts
                new StatusCodeModel("100000009",Language.quote_expired_sts,"#B3B3B3"), // Expired   quote_expired_sts
            };
        }

        public static StatusCodeModel GetQuoteStatusCodeById(string id)
        {
            return QuoteStatusData().SingleOrDefault(x => x.Id == id);
        }
    }
}
