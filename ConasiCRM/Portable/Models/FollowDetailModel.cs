using ConasiCRM.Portable.Resources;
using ConasiCRM.Portable.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConasiCRM.Portable.Models
{
    public class FollowDetailModel
    {
        public string bsd_followuplistcode { get; set; }
        public string bsd_followuplistid { get; set; }
        public int statuscode { get; set; }
        public DateTime bsd_date { get; set; }
        public double bsd_sellingprice_base { get; set; }
        public double bsd_totalamount_base { get; set; }
        public double bsd_totalamountpaid_base { get; set; }
        public double bsd_forfeitureamount_base { get; set; }
        public double bsd_totalforfeitureamount_base { get; set; }
        public int bsd_takeoutmoney { get; set; }
        public bool? bsd_terminateletter { get; set; }
        public bool? bsd_termination { get; set; }
        public bool? bsd_resell { get; set; }
        public string bsd_name_dotmoban { get; set; }
        public string resell
        {
            get
            {
                if (bsd_resell == true)
                {
                    return Language.string_co_sts;
                }else if (bsd_resell == false)
                {
                    return Language.string_khong_sts;
                }
                else
                {
                    return null;
                }
            }
        }
        public string takeoutmoney
        {
            get
            {
                if (bsd_takeoutmoney == 100000000)
                {
                    return Language.ful_refund_takeoutmoney; //Refund // ful_refund_takeoutmoney
                }
                else if (bsd_takeoutmoney == 100000001)
                {
                    return Language.ful_forfeiture_takeoutmoney; //Forfeiture //ful_forfeiture_takeoutmoney
                }
                else
                {
                    return null;
                }
            }
        }


        public int bsd_type { get; set; }
        public string type
        {
            get
            {
                if (bsd_type == 100000007)
                {
                    return Language.ful_units_type; //Units ful_units_type
                }
                else if (bsd_type == 100000000)
                {
                    return Language.ful_reservation_sign_off_RF_type; //Reservation - Sign off RF ful_reservation_sign_off_RF_type
                }
                else if (bsd_type == 100000001)
                {
                    return Language.ful_reservation_deposited_type; //Reservation - Deposited  ful_reservation_deposited_type
                }
                else if (bsd_type == 100000005)
                {
                    return Language.ful_reservation_terminate_type; //Reservation - Terminate  ful_reservation_terminate_type
                }
                else if (bsd_type == 100000002)
                {
                    return Language.ful_option_entry_1st_installment_type; //Option Entry - 1st installment  ful_option_entry_1st_installment_type
                }
                else if (bsd_type == 100000003)
                {
                    return Language.ful_option_entry_contract_type; //Option Entry - Contract  ful_option_entry_contract_type
                }
                else if (bsd_type == 100000004)
                {
                    return Language.ful_option_entry_installments_type; //Option Entry - Installments  ful_option_entry_installments_type
                }
                else if (bsd_type == 100000006)
                {
                    return Language.ful_option_entry_terminate_type; //Option Entry - Terminate  ful_option_entry_terminate_type
                }
                else { return null; }
            }
        }

        public DateTime bsd_expiredate { get; set; }
        public string bsd_name { get; set; }
        public string _bsd_project_value { get; set; }
        public int bsd_group { get; set; }
        public string _bsd_reservation_value { get; set; }
        public DateTime createdon { get; set; }
        public string _bsd_units_value { get; set; }

        public string bsd_name_project { get; set; }
        public double bsd_bookingfee_project { get; set; }
        public string bsd_projectcode_project { get; set; }

        public double totalamount_quote { get; set; }
        public double totallineitemamount_base_quote { get; set; }
        public string name_quote { get; set; }
        public string customer_name_contact { get; set; }
        public string customer_name_account_quote { get; set; }

        public string name_salesorder { get; set; }
        public double bsd_totalpaidincludecoa_salesorder { get; set; }
        public double bsd_totalamountlessfreight_salesorder { get; set; }
        public string customer_name_account { get; set; }

        public string name_work
        {
            get
            {
                if (this.name_quote != null)
                {
                    return this.name_quote;
                }
                else if (this.name_salesorder != null)
                {
                    return this.name_salesorder;
                }
                else
                {
                    return null;
                }
            }
        }

        public string customer
        {
            get
            {
                if (this.customer_name_account != null)
                {
                    return this.customer_name_account;
                }
                else if (this.customer_name_contact != null)
                {
                    return this.customer_name_contact;
                }
                else if (this.customer_name_account_quote != null)
                {
                    return this.customer_name_account_quote;
                }
                else
                {
                    return null;
                }
            }
        }
        public string name_unit { get; set; }
        public string productnumber_unit { get; set; }
        public string bsd_areavariance_unit { get; set; }
        public double price_unit { get; set; }
        public string block { get; set; }
        public string floor { get; set; }

    }
}
