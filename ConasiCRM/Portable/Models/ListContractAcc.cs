﻿using System;
using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Resources;
using Xamarin.Forms;

namespace ConasiCRM.Portable.Models
{
    public class ListContractAcc : ContentView
    {
        public string customerid { get; set; }
        public string bsd_optionno { get; set; }
        //public decimal? bsd_optionno { get; set; }
        //public string bsd_optionnovalueformat => bsd_optionno.HasValue ? string.Format("{0:#,0.#}", bsd_optionno.Value) + " đ" : null;

        public decimal? totalamount { get; set; }
        public string totalamountformat => totalamount.HasValue ? string.Format("{0:#,0.#}", totalamount.Value) + " đ" : null;
        public int statuscode { get; set; }
        public string statuscodevalue
        {
            get
            {
                switch (statuscode)
                {
                    case 1:
                        return Language.contract_open_sts;
                    case 2:
                        return Language.contract_pending_sts;
                    case 100000000:
                        return Language.contract_option_sts;
                    case 100000001:
                        return Language.contract_1st_installment_sts;
                    case 100000002:
                        return Language.contract_signed_contract_sts;
                    case 100000003:
                        return Language.contract_being_payment_sts;
                    case 100000004:
                        return Language.contract_complete_payment_sts;
                    case 100000005:
                        return Language.contract_handover_sts;
                    case 100000006:
                        return Language.contract_terminated_sts;
                    default:
                        return "";
                }
            }
        }
        public DateTime bsd_signingexpired { get; set; }
        public string bsd_signingexpiredformat
        {
            get => StringHelper.DateFormat(bsd_signingexpired);
        }

        public DateTime createdon { get; set; }
        public string createdonformat
        {
            get => StringHelper.DateFormat(createdon);
        }
        public string contract_nameproject { get; set; }
        public string contract_nameaccount { get; set; }
        public string contract_namecontact { get; set; }
        public string contract_nameproduct { get; set; }
    }
}

